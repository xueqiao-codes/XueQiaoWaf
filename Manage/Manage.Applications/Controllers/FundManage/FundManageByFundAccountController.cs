﻿using business_foundation_lib.xq_thriftlib_config;
using IDLAutoGenerated.Util;
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
using System.Threading;
using System.Waf.Applications;
using System.Windows.Controls;
using xueqiao.trade.hosting;
using xueqiao.trade.hosting.terminal.ao;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoFoundation.UI.Components.Navigation;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;

namespace Manage.Applications.Controllers
{
    /// <summary>
    /// 交易管理-资金-按资金账户查看 controller
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class FundManageByFundAccountController : IController
    {
        private readonly SimpleNavigationContainerView navigationContainerView;
        private readonly FundManageByFundAccountViewModel contentViewModel;
        private readonly ExportFactory<FundAccountEquityDetailDialogController> equityDetailDialogCtrlFactory;
        private readonly ExportFactory<FundAccountEquityDailyHistoryController> equityDailyHistoryCtrlFactory;
        private readonly IManageFundAccountItemsController manageFundAccountItemsCtrl;
        private readonly ILoginDataService loginDataService;
        private readonly IMessageWindowService messageWindowService;
        private readonly IEventAggregator eventAggregator;

        private readonly DelegateCommand toShowCurrencyGroupedEquityDetailCmd;
        private readonly DelegateCommand toShowDailyHistoryCmd;
        private readonly DelegateCommand refreshPageDataCmd;

        private FundAccountEquityDailyHistoryController equityDailyHistoryCtrl;

        private bool isRefreshingPageData;
        private HostingTAFundItem currentFundAccountEquityData;

        [ImportingConstructor]
        public FundManageByFundAccountController(
            SimpleNavigationContainerView navigationContainerView,
            FundManageByFundAccountViewModel contentViewModel,
            ExportFactory<FundAccountEquityDetailDialogController> equityDetailDialogCtrlFactory,
            ExportFactory<FundAccountEquityDailyHistoryController> equityDailyHistoryCtrlFactory,
            IManageFundAccountItemsController manageFundAccountItemsCtrl,
            ILoginDataService loginDataService,
            IMessageWindowService messageWindowService,
            IEventAggregator eventAggregator)
        {
            this.navigationContainerView = navigationContainerView;
            this.contentViewModel = contentViewModel;
            this.equityDetailDialogCtrlFactory = equityDetailDialogCtrlFactory;
            this.equityDailyHistoryCtrlFactory = equityDailyHistoryCtrlFactory;
            this.manageFundAccountItemsCtrl = manageFundAccountItemsCtrl;
            this.loginDataService = loginDataService;
            this.messageWindowService = messageWindowService;
            this.eventAggregator = eventAggregator;

            toShowCurrencyGroupedEquityDetailCmd = new DelegateCommand(ToShowCurrencyGroupedEquityDetail);
            toShowDailyHistoryCmd = new DelegateCommand(ToShowDailyHistory);
            refreshPageDataCmd = new DelegateCommand(RefreshPageData, CanRefreshPageData);
        }

        public object ContentView => navigationContainerView;
        
        public void Initialize()
        {
            contentViewModel.ToShowDailyHistoryCmd = toShowDailyHistoryCmd;
            contentViewModel.DataRefreshCmd = refreshPageDataCmd;

            eventAggregator.GetEvent<ManageFundAccountItemsRefreshEvent>().Subscribe(RecvManageFundAccountItemsRefreshEvent, ThreadOption.UIThread);

            var allFundAccounts = manageFundAccountItemsCtrl.AllFundAccountItems?.ToArray();
            contentViewModel.FundAccountItems.Clear();
            contentViewModel.FundAccountItems.AddRange(allFundAccounts);
            contentViewModel.SelectedFundAccountItem = allFundAccounts?.FirstOrDefault();

            if (allFundAccounts?.Any() != true)
            {
                manageFundAccountItemsCtrl.RefreshFundAccountItemsIfNeed();
            }
            
            // set contentViewModel.View as the first page of navigation
            navigationContainerView.Navigate(contentViewModel.View as Page);
        }

        public void Run()
        {
            PropertyChangedEventManager.AddHandler(contentViewModel, ContentViewModelPropChanged, "");
            RefreshCurrentFundAccountEquityData();
        }

        public void Shutdown()
        {
            equityDailyHistoryCtrl?.Shutdown();
            eventAggregator.GetEvent<ManageFundAccountItemsRefreshEvent>().Unsubscribe(RecvManageFundAccountItemsRefreshEvent);
            PropertyChangedEventManager.RemoveHandler(contentViewModel, ContentViewModelPropChanged, "");
        }

        private void ContentViewModelPropChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(FundManageByFundAccountViewModel.SelectedFundAccountItem))
            {
                RefreshCurrentFundAccountEquityData();
            }
        }

        private void RecvManageFundAccountItemsRefreshEvent(ManageFundAccountItemsRefreshEventArgs args)
        {
            var loginToken = loginDataService.ProxyLoginResp?.HostingSession?.Token;
            if (loginToken != args.LoginUserToken) return;

            var originFundAccountId = this.contentViewModel.SelectedFundAccountItem?.TradeAccountId;

            PropertyChangedEventManager.RemoveHandler(contentViewModel, ContentViewModelPropChanged, "");
            
            // 刷新资金账户列表
            contentViewModel.FundAccountItems.Clear();
            contentViewModel.FundAccountItems.AddRange(args.FundAccountItems);
            HostingTradeAccount newSelectedFundAccountItem = null;
            if (originFundAccountId != null)
                newSelectedFundAccountItem = args.FundAccountItems?.FirstOrDefault(i => i.TradeAccountId == originFundAccountId);
            if (newSelectedFundAccountItem == null)
                newSelectedFundAccountItem = args.FundAccountItems?.FirstOrDefault();
            contentViewModel.SelectedFundAccountItem = newSelectedFundAccountItem;
            
            PropertyChangedEventManager.AddHandler(contentViewModel, ContentViewModelPropChanged, "");

            // 如果选中的资金账号发生变化，则刷新权益数据
            if (newSelectedFundAccountItem?.TradeAccountId != originFundAccountId)
            {
                RefreshCurrentFundAccountEquityData();
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

        private void ToShowCurrencyGroupedEquityDetail(object obj)
        {
            var model = obj as FundAccountEquityModel;
            if (model == null) return;

            var tarCurrencyNo = model.EquityData.CurrencyNo;
            var tarCurrencyEquityItem = currentFundAccountEquityData?.GroupFunds[tarCurrencyNo];
            if (tarCurrencyEquityItem == null) return;

            var dialogCtrl = equityDetailDialogCtrlFactory.CreateExport().Value;
            dialogCtrl.DialogOwner = contentViewModel.DisplayInWindow;
            dialogCtrl.CurrencyEquityItem = tarCurrencyEquityItem;
            dialogCtrl.Initialize();
            dialogCtrl.Run();
            dialogCtrl.Shutdown();
        }

        private void ToShowDailyHistory()
        {
            var currentFundAccount = contentViewModel.SelectedFundAccountItem;
            if (currentFundAccount == null) return;

            equityDailyHistoryCtrl?.Shutdown();
            equityDailyHistoryCtrl = null;
            
            var newHistoryCtrl = equityDailyHistoryCtrlFactory.CreateExport().Value;
            this.equityDailyHistoryCtrl = newHistoryCtrl;

            newHistoryCtrl.FundAccount = currentFundAccount;
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
            manageFundAccountItemsCtrl.RefreshFundAccountItemsIfNeed();
            RefreshCurrentFundAccountEquityData();
        }
        
        private async void RefreshCurrentFundAccountEquityData()
        {
            if (isRefreshingPageData) return;

            var currentFundAccountId = contentViewModel.SelectedFundAccountItem?.TradeAccountId;
            if (currentFundAccountId == null) return;

            var landingInfo = loginDataService.ProxyLoginResp?.HostingSession?.HostingSession2LandingInfo();
            if (landingInfo == null) return;

            UpdatePageDataIsRefreshing(true);
            // query equity data 
            var equityDataResp = await XqThriftLibConfigurationManager.SharedInstance.TradeHostingTerminalAoHttpStub
                    .getTradeAccountFundNowAsync(landingInfo, currentFundAccountId.Value, CancellationToken.None);

            DispatcherHelper.CheckBeginInvokeOnUI(() => 
            {
                var equityData = equityDataResp?.CorrectResult?.FirstOrDefault(i => i.TradeAccountId == currentFundAccountId.Value);

                UpdatePageDataIsRefreshing(false);
                this.currentFundAccountEquityData = equityData;
                
                if (equityDataResp == null || equityDataResp?.SourceException != null || equityData == null)
                {
                    // error handle
                }

                contentViewModel.CurrentEquityDataUpdateTimestampMs = equityData?.UpdateTimestampMs;

                FundAccountEquityModel.GenerateEquityModels(equityData, 
                        out IEnumerable<FundAccountEquityModel> totalEquityItems,
                        out IEnumerable<FundAccountEquityModel> currencyGroupedEquityItems,
                        _model => { _model.ShowDetailCmd = this.toShowCurrencyGroupedEquityDetailCmd; });

                contentViewModel.TotalEquityItems.Clear();
                contentViewModel.TotalEquityItems.AddRange(totalEquityItems);
                contentViewModel.CurrencyGroupedEquityItems.Clear();
                contentViewModel.CurrencyGroupedEquityItems.AddRange(currencyGroupedEquityItems);
            });
        }
    }
}
