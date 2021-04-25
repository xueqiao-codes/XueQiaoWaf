﻿using business_foundation_lib.xq_thriftlib_config;
using IDLAutoGenerated.Util;
using lib.xqclient_base.thriftapi_mediation;
using lib.xqclient_base.thriftapi_mediation.Interface;
using Manage.Applications.DataModels;
using Manage.Applications.ServiceControllers;
using Manage.Applications.ServiceControllers.Events;
using Manage.Applications.ViewModels;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Controls;
using Thrift.Collections;
using xueqiao.trade.hosting;
using xueqiao.trade.hosting.asset.thriftapi;
using xueqiao.trade.hosting.terminal.ao;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoFoundation.UI.Components.Navigation;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;

namespace Manage.Applications.Controllers
{
    /// <summary>
    /// 交易管理-资金-按操作账户查看 controller
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class FundManageBySubAccountController : IController
    {
        private readonly SimpleNavigationContainerView navigationContainerView;
        private readonly FundManageBySubAccountViewModel contentViewModel;
        private readonly ExportFactory<SubAccountEquityDailyHistoryController> equityDailyHistoryCtrlFactory;
        private readonly IManageSubAccountItemsController manageSubAccountItemsCtrl;
        private readonly ILoginDataService loginDataService;
        private readonly IMessageWindowService messageWindowService;
        private readonly IEventAggregator eventAggregator;

        private readonly DelegateCommand toShowDailyHistoryCmd;
        private readonly DelegateCommand refreshPageDataCmd;

        private SubAccountEquityDailyHistoryController equityDailyHistoryCtrl;

        private bool isRefreshingPageData;

        [ImportingConstructor]
        public FundManageBySubAccountController(SimpleNavigationContainerView navigationContainerView,
            FundManageBySubAccountViewModel contentViewModel,
            ExportFactory<SubAccountEquityDailyHistoryController> equityDailyHistoryCtrlFactory,
            IManageSubAccountItemsController manageSubAccountItemsCtrl,
            ILoginDataService loginDataService,
            IMessageWindowService messageWindowService,
            IEventAggregator eventAggregator)
        {
            this.navigationContainerView = navigationContainerView;
            this.contentViewModel = contentViewModel;
            this.equityDailyHistoryCtrlFactory = equityDailyHistoryCtrlFactory;
            this.manageSubAccountItemsCtrl = manageSubAccountItemsCtrl;
            this.loginDataService = loginDataService;
            this.messageWindowService = messageWindowService;
            this.eventAggregator = eventAggregator;

            toShowDailyHistoryCmd = new DelegateCommand(ToShowDailyHistory);
            refreshPageDataCmd = new DelegateCommand(RefreshPageData, CanRefreshPageData);
        }

        public object ContentView => navigationContainerView;

        public void Initialize()
        {
            contentViewModel.ToShowDailyHistoryCmd = toShowDailyHistoryCmd;
            contentViewModel.DataRefreshCmd = refreshPageDataCmd;

            eventAggregator.GetEvent<ManageSubAccountItemsRefreshEvent>().Subscribe(RecvManageSubAccountItemsRefreshEvent, ThreadOption.UIThread);

            var allSubAccounts = manageSubAccountItemsCtrl.AllSubAccountItems?.ToArray();
            contentViewModel.SubAccountItems.Clear();
            contentViewModel.SubAccountItems.AddRange(allSubAccounts);
            contentViewModel.SelectedSubAccountItem = allSubAccounts?.FirstOrDefault();

            if (allSubAccounts?.Any() != true)
            {
                manageSubAccountItemsCtrl.RefreshSubAccountItemsIfNeed();
            }

            // set contentViewModel.View as the first page of navigation
            navigationContainerView.Navigate(contentViewModel.View as Page);
        }

        public void Run()
        {
            PropertyChangedEventManager.AddHandler(contentViewModel, ContentViewModelPropChanged, "");
            RefreshCurrentSubAccountEquityData();
        }

        public void Shutdown()
        {
            equityDailyHistoryCtrl?.Shutdown();
            eventAggregator.GetEvent<ManageSubAccountItemsRefreshEvent>().Unsubscribe(RecvManageSubAccountItemsRefreshEvent);
            PropertyChangedEventManager.RemoveHandler(contentViewModel, ContentViewModelPropChanged, "");
            
        }

        private void ContentViewModelPropChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(FundManageBySubAccountViewModel.SelectedSubAccountItem))
            {
                RefreshCurrentSubAccountEquityData();
            }
        }

        private void RecvManageSubAccountItemsRefreshEvent(ManageSubAccountItemsRefreshEventArgs args)
        {
            var loginToken = loginDataService.ProxyLoginResp?.HostingSession?.Token;
            if (loginToken != args.LoginUserToken) return;

            var originSubAccountId = this.contentViewModel.SelectedSubAccountItem?.SubAccountId;

            PropertyChangedEventManager.RemoveHandler(contentViewModel, ContentViewModelPropChanged, "");

            // 刷新资金账户列表
            contentViewModel.SubAccountItems.Clear();
            contentViewModel.SubAccountItems.AddRange(args.SubAccountItems);
            HostingSubAccount newSelectedSubAccountItem = null;
            if (originSubAccountId != null)
                newSelectedSubAccountItem = args.SubAccountItems?.FirstOrDefault(i => i.SubAccountId == originSubAccountId);
            if (newSelectedSubAccountItem == null)
                newSelectedSubAccountItem = args.SubAccountItems?.FirstOrDefault();
            contentViewModel.SelectedSubAccountItem = newSelectedSubAccountItem;

            PropertyChangedEventManager.AddHandler(contentViewModel, ContentViewModelPropChanged, "");

            // 如果选中的子账号发生变化，则刷新权益数据
            if (newSelectedSubAccountItem?.SubAccountId != originSubAccountId)
            {
                RefreshCurrentSubAccountEquityData();
            }
        }

        private void UpdatePageDataIsRefreshing(bool isRefreshing)
        {
            isRefreshingPageData = isRefreshing;
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                refreshPageDataCmd.RaiseCanExecuteChanged();
            });
        }

        private void ToShowDailyHistory()
        {
            var currentSubAccount = contentViewModel.SelectedSubAccountItem;
            if (currentSubAccount == null) return;

            equityDailyHistoryCtrl?.Shutdown();
            equityDailyHistoryCtrl = null;

            var newHistoryCtrl = equityDailyHistoryCtrlFactory.CreateExport().Value;
            this.equityDailyHistoryCtrl = newHistoryCtrl;

            newHistoryCtrl.SubAccount = currentSubAccount;
            newHistoryCtrl.PageGoBackHandler = _ctrl =>
            {
                navigationContainerView.GoBackIfPossible();
                equityDailyHistoryCtrl?.Shutdown();
                equityDailyHistoryCtrl = null;
            };
            newHistoryCtrl.Initialize();
            newHistoryCtrl.Run();

            navigationContainerView.Navigate(newHistoryCtrl.PageView as Page);
        }

        private bool CanRefreshPageData()
        {
            return !isRefreshingPageData;
        }

        private void RefreshPageData()
        {
            manageSubAccountItemsCtrl.RefreshSubAccountItemsIfNeed();
            RefreshCurrentSubAccountEquityData();
        }

        private void RefreshCurrentSubAccountEquityData()
        {
            if (isRefreshingPageData) return;

            var currentSubAccountId = contentViewModel.SelectedSubAccountItem?.SubAccountId;
            if (currentSubAccountId == null) return;

            var landingInfo = loginDataService.ProxyLoginResp?.HostingSession?.HostingSession2LandingInfo();
            if (landingInfo == null) return;

            UpdatePageDataIsRefreshing(true);

            Task.Run(() =>
            {
                var taskFactory = new TaskFactory();
                var tasks = new List<Task>();

                // query base currency equity
                IInterfaceInteractResponse<IEnumerable<HostingFund>> baseCurrencyEquityResp = null;
                var task1 = taskFactory.StartNew(() =>
                {
                    baseCurrencyEquityResp = QuerySubAccountEquities(landingInfo, currentSubAccountId.Value, true);
                });
                tasks.Add(task1);

                // query children currency equity
                IInterfaceInteractResponse<IEnumerable<HostingFund>> childrenCurrencyEquityResp = null;
                var task2 = taskFactory.StartNew(() =>
                {
                    childrenCurrencyEquityResp = QuerySubAccountEquities(landingInfo, currentSubAccountId.Value, false);
                });
                tasks.Add(task2);

                Task.WaitAll(tasks.ToArray());

                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    UpdatePageDataIsRefreshing(false);

                    if (baseCurrencyEquityResp == null || baseCurrencyEquityResp?.SourceException != null)
                    {
                        // error handle
                    }
                    else if (childrenCurrencyEquityResp == null || childrenCurrencyEquityResp?.SourceException != null)
                    {
                        // error handle
                    }

                    contentViewModel.CurrentEquityDataUpdateTimestampMs = (long)DateHelper.NowUnixTimeSpan().TotalMilliseconds;
                    contentViewModel.TotalEquityItems.Clear();
                    contentViewModel.TotalEquityItems.AddRange(baseCurrencyEquityResp?.CorrectResult?.Select(i => new SubAccountEquityModel(i)).ToArray());
                    contentViewModel.CurrencyGroupedEquityItems.Clear();
                    contentViewModel.CurrencyGroupedEquityItems.AddRange(childrenCurrencyEquityResp?.CorrectResult?.Select(i => new SubAccountEquityModel(i)).ToArray());
                });
            });
        }

        private IInterfaceInteractResponse<IEnumerable<HostingFund>> QuerySubAccountEquities(LandingInfo landingInfo,
            long subAccountId, bool queryBaseCurrency)
        {
            if (landingInfo == null) return null;
            
            var option = new ReqHostingFundOption
            {
                SubAccountIds = new THashSet<long> { subAccountId },
                BaseCurrency = queryBaseCurrency
            };

            var resp = XqThriftLibConfigurationManager.SharedInstance.TradeHostingTerminalAoHttpStub.getHostingSubAccountFund(landingInfo, option);
            if (resp == null) return null;

            var queryItems = resp.CorrectResult?.Page?.Where(i => i.SubAccountId == subAccountId).ToArray();
            var tarResp = new InterfaceInteractResponse<IEnumerable<HostingFund>>(resp.Servant,
                resp.InterfaceName,
                resp.SourceException,
                resp.HasTransportException,
                resp.HttpResponseStatusCode,
                queryItems)
            {
                CustomParsedExceptionResult = resp.CustomParsedExceptionResult,
                InteractInformation = resp.InteractInformation
            };
            return tarResp;
        }
    }
}