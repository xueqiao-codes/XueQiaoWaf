using IDLAutoGenerated.Util;
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
using xueqiao.trade.hosting;
using XueQiaoFoundation.Interfaces.Applications;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.UI.Components.Navigation;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;

namespace Manage.Applications.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class PositionShowByFundAccountCtrl : IController
    {
        private readonly SimpleNavigationContainerView navigationContainerView;
        private readonly PositionShowByFundAccountVM contentViewModel;
        private readonly IManageFundAccountItemsController manageFundAccountItemsCtrl;
        private readonly ILoginDataService loginDataService;
        private readonly IContractItemTreeQueryController contractItemTreeQueryCtrl;
        private readonly IEventAggregator eventAggregator;

        private readonly DelegateCommand toShowPositionDetailCmd;
        private readonly DelegateCommand refreshDataCmd;
        private readonly DelegateCommand toShowHistoryCmd;

        private bool isRefreshingData;

        [ImportingConstructor]
        public PositionShowByFundAccountCtrl(
            SimpleNavigationContainerView navigationContainerView,
            PositionShowByFundAccountVM contentViewModel,
            IManageFundAccountItemsController manageFundAccountItemsCtrl,
            ILoginDataService loginDataService,
            IContractItemTreeQueryController contractItemTreeQueryCtrl,
            IEventAggregator eventAggregator)
        {
            this.navigationContainerView = navigationContainerView;
            this.contentViewModel = contentViewModel;
            this.manageFundAccountItemsCtrl = manageFundAccountItemsCtrl;
            this.loginDataService = loginDataService;
            this.contractItemTreeQueryCtrl = contractItemTreeQueryCtrl;
            this.eventAggregator = eventAggregator;

            toShowPositionDetailCmd = new DelegateCommand(ToShowPositionDetail);
            refreshDataCmd = new DelegateCommand(RefreshPageData, CanRefreshPageData);
            toShowHistoryCmd = new DelegateCommand(ToShowHistory);
        }

        public object ContentView => navigationContainerView;

        public void Initialize()
        {
            contentViewModel.RefreshDataCmd = refreshDataCmd;
            contentViewModel.ToShowHistoryCmd = toShowHistoryCmd;

            eventAggregator.GetEvent<ManageFundAccountItemsRefreshEvent>().Subscribe(RecvManageFundAccountItemsRefreshEvent, ThreadOption.UIThread);

            var allSubAccounts = manageFundAccountItemsCtrl.AllFundAccountItems?.ToArray();
            contentViewModel.FundAccountItems.Clear();
            contentViewModel.FundAccountItems.AddRange(allSubAccounts);
            contentViewModel.SelectedFundAccountItem = allSubAccounts?.FirstOrDefault();

            if (allSubAccounts?.Any() != true)
            {
                manageFundAccountItemsCtrl.RefreshFundAccountItemsIfNeed();
            }

            // set contentViewModel.View as the first page of navigation
            navigationContainerView.Navigate(contentViewModel.View as Page);
        }

        public void Run()
        {
            PropertyChangedEventManager.AddHandler(contentViewModel, ContentViewModelPropChanged, "");
            RefreshCurrentFundAccountPositionData();
        }

        public void Shutdown()
        {
            //equityDailyHistoryCtrl?.Shutdown();
            eventAggregator.GetEvent<ManageFundAccountItemsRefreshEvent>().Unsubscribe(RecvManageFundAccountItemsRefreshEvent);
            PropertyChangedEventManager.RemoveHandler(contentViewModel, ContentViewModelPropChanged, "");
        }

        private void ContentViewModelPropChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PositionShowByFundAccountVM.SelectedFundAccountItem))
            {
                RefreshCurrentFundAccountPositionData();
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
                RefreshCurrentFundAccountPositionData();
            }
        }

        private void ToShowPositionDetail(object obj)
        {
            // do something
        }

        private void ToShowHistory()
        {
            // do something
        }

        private bool CanRefreshPageData()
        {
            return !isRefreshingData;
        }

        private void RefreshPageData()
        {
            manageFundAccountItemsCtrl.RefreshFundAccountItemsIfNeed();
            RefreshCurrentFundAccountPositionData();
        }

        private void UpdateIsRefreshingData(bool isRefreshing)
        {
            this.isRefreshingData = isRefreshing;
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                refreshDataCmd?.RaiseCanExecuteChanged();
            });
        }

        private void RefreshCurrentFundAccountPositionData()
        {
            var fundAccountId = contentViewModel.SelectedFundAccountItem?.TradeAccountId;
            if (fundAccountId == null) return;

            var landingInfo = loginDataService.ProxyLoginResp?.HostingSession?.HostingSession2LandingInfo();
            if (landingInfo == null) return;

            // do something
        }
    }
}
