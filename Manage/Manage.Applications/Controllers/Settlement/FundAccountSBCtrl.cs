using business_foundation_lib.xq_thriftlib_config;
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
using System.Threading.Tasks;
using System.Waf.Applications;
using xueqiao.trade.hosting;
using xueqiao.trade.hosting.tradeaccount.data;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;

namespace Manage.Applications.Controllers
{
    /// <summary>
    /// 资金账号结算单页面 controller
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class FundAccountSBCtrl : IController
    {
        private readonly FundAccountSettlementContainerVM contentVM;
        private readonly IManageFundAccountItemsController manageFundAccountItemsCtrl;
        private readonly ILoginDataService loginDataService;
        private readonly IMessageWindowService messageWindowService;
        private readonly IEventAggregator eventAggregator;

        private readonly DelegateCommand refreshSettlementCmd;

        private bool isRefreshingPageData;

        [ImportingConstructor]
        public FundAccountSBCtrl(
            FundAccountSettlementContainerVM contentVM,
            IManageFundAccountItemsController manageFundAccountItemsCtrl,
            ILoginDataService loginDataService,
            IMessageWindowService messageWindowService,
            IEventAggregator eventAggregator)
        {
            this.contentVM = contentVM;
            this.manageFundAccountItemsCtrl = manageFundAccountItemsCtrl;
            this.loginDataService = loginDataService;
            this.messageWindowService = messageWindowService;
            this.eventAggregator = eventAggregator;

            refreshSettlementCmd = new DelegateCommand(RefreshSettlement, CanRefreshSettlement);
        }

        public object ContentView => contentVM.View;

        public void Initialize()
        {
            contentVM.RefreshSettlementCmd = refreshSettlementCmd;

            eventAggregator.GetEvent<ManageFundAccountItemsRefreshEvent>().Subscribe(RecvManageFundAccountItemsRefreshEvent, ThreadOption.UIThread);

            var allFundAccounts = manageFundAccountItemsCtrl.AllFundAccountItems?.ToArray();
            contentVM.FundAccountItems.Clear();
            contentVM.FundAccountItems.AddRange(allFundAccounts);
            contentVM.SelectedFundAccountItem = allFundAccounts?.FirstOrDefault();

            if (allFundAccounts?.Any() != true)
            {
                manageFundAccountItemsCtrl.RefreshFundAccountItemsIfNeed();
            }
        }

        public void Run()
        {
            PropertyChangedEventManager.AddHandler(contentVM, ContentViewModelPropChanged, "");
            RefreshCurrentFundAccountSettlementInfo();
        }

        public void Shutdown()
        {
            eventAggregator.GetEvent<ManageFundAccountItemsRefreshEvent>().Unsubscribe(RecvManageFundAccountItemsRefreshEvent);
            PropertyChangedEventManager.RemoveHandler(contentVM, ContentViewModelPropChanged, "");
        }
        
        private void ContentViewModelPropChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(FundAccountSettlementContainerVM.SelectedFundAccountItem)
                || e.PropertyName == nameof(FundAccountSettlementContainerVM.SelectedDate))
            {
                RefreshCurrentFundAccountSettlementInfo();
            }
        }

        private void RecvManageFundAccountItemsRefreshEvent(ManageFundAccountItemsRefreshEventArgs args)
        {
            var loginToken = loginDataService.ProxyLoginResp?.HostingSession?.Token;
            if (loginToken != args.LoginUserToken) return;

            var originFundAccountId = this.contentVM.SelectedFundAccountItem?.TradeAccountId;

            PropertyChangedEventManager.RemoveHandler(contentVM, ContentViewModelPropChanged, "");

            // 刷新资金账户列表
            contentVM.FundAccountItems.Clear();
            contentVM.FundAccountItems.AddRange(args.FundAccountItems);
            HostingTradeAccount newSelectedFundAccountItem = null;
            if (originFundAccountId != null)
                newSelectedFundAccountItem = args.FundAccountItems?.FirstOrDefault(i => i.TradeAccountId == originFundAccountId);
            if (newSelectedFundAccountItem == null)
                newSelectedFundAccountItem = args.FundAccountItems?.FirstOrDefault();
            contentVM.SelectedFundAccountItem = newSelectedFundAccountItem;

            PropertyChangedEventManager.AddHandler(contentVM, ContentViewModelPropChanged, "");

            // 如果选中的资金账号发生变化，则刷新权益数据
            if (newSelectedFundAccountItem?.TradeAccountId != originFundAccountId)
            {
                RefreshCurrentFundAccountSettlementInfo();
            }
        }

        private void UpdatePageDataIsRefreshing(bool isRefreshing)
        {
            isRefreshingPageData = isRefreshing;
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                refreshSettlementCmd?.RaiseCanExecuteChanged();
            });
        }

        private bool CanRefreshSettlement()
        {
            return !isRefreshingPageData;
        }

        private void RefreshSettlement()
        {
            RefreshCurrentFundAccountSettlementInfo();
        }

        private async void RefreshCurrentFundAccountSettlementInfo()
        {
            if (isRefreshingPageData) return;

            var currentFundAccountId = contentVM.SelectedFundAccountItem?.TradeAccountId;
            if (currentFundAccountId == null) return;
            
            var selectedDate = this.contentVM.SelectedDate;
            if (selectedDate == null) return;
            var dateFormated = selectedDate.Value.Date.ToString("yyyy-MM-dd");

            var landingInfo = loginDataService.LandingInfo;
            if (landingInfo == null) return;
            
            UpdatePageDataIsRefreshing(true);
            // query equity data 
            var dataResp = await XqThriftLibConfigurationManager.SharedInstance.TradeHostingTerminalAoHttpStub
                    .getTradeAccountSettlementInfosAsync(landingInfo, currentFundAccountId.Value, dateFormated, dateFormated, CancellationToken.None);

            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                TradeAccountSettlementInfo settlementInfo = dataResp?.CorrectResult?
                    .FirstOrDefault(i => i.TradeAccountId == currentFundAccountId.Value && i.SettlementDate == dateFormated);

                UpdatePageDataIsRefreshing(false);
                if (dataResp == null || dataResp?.SourceException != null)
                {
                    // error handle
                }

                contentVM.SettlementRefreshTimestampMs = (long)DateHelper.NowUnixTimeSpan().TotalMilliseconds;
                contentVM.SettlementText = settlementInfo?.SettlementContent;
            });
        }
    }
}
