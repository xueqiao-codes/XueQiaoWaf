using business_foundation_lib.helper;
using business_foundation_lib.xq_thriftlib_config;
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
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Controls;
using xueqiao.trade.hosting;
using xueqiao.trade.hosting.position.adjust.thriftapi;
using XueQiaoFoundation.BusinessResources.Helpers;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoFoundation.UI.Components.Navigation;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;

namespace Manage.Applications.Controllers
{
    /// <summary>
    /// 持仓核对管理页面 controller
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class PositionVerifyManageCtrl : IController
    {
        private readonly SimpleNavigationContainerView navigationContainerView;
        private readonly PositionVerifyManageVM contentVM;
        private readonly ExportFactory<PositionDiffOverviewCtrl> positionDiffOverviewCtrlFactory;
        private readonly IManageFundAccountItemsController manageFundAccountItemsCtrl;
        private readonly ILoginDataService loginDataService;
        private readonly IMessageWindowService messageWindowService;
        private readonly IEventAggregator eventAggregator;

        private readonly DelegateCommand requestModuleLockCmd;
        private readonly DelegateCommand toExitModuleLockCmd;

        private readonly Dictionary<long, PositionDiffOverviewCtrl> positionDiffOverviewCtrls
            = new Dictionary<long, PositionDiffOverviewCtrl>();

        private readonly ModuleLockStatusDM moduleLockStatus = new ModuleLockStatusDM();

        [ImportingConstructor]
        public PositionVerifyManageCtrl(
            SimpleNavigationContainerView navigationContainerView,
            PositionVerifyManageVM contentVM,
            ExportFactory<PositionDiffOverviewCtrl> positionDiffOverviewCtrlFactory,
            IManageFundAccountItemsController manageFundAccountItemsCtrl,
            ILoginDataService loginDataService,
            IMessageWindowService messageWindowService,
            IEventAggregator eventAggregator)
        {
            this.navigationContainerView = navigationContainerView;
            this.contentVM = contentVM;
            this.positionDiffOverviewCtrlFactory = positionDiffOverviewCtrlFactory;
            this.manageFundAccountItemsCtrl = manageFundAccountItemsCtrl;
            this.loginDataService = loginDataService;
            this.messageWindowService = messageWindowService;
            this.eventAggregator = eventAggregator;

            requestModuleLockCmd = new DelegateCommand(RequestModuleLock);
            toExitModuleLockCmd = new DelegateCommand(ToExitModuleLock);
        }

        public object ContentView => this.navigationContainerView;

        public void Initialize()
        {
            eventAggregator.GetEvent<ManageFundAccountItemsRefreshEvent>().Subscribe(RecvManageFundAccountItemsRefreshEvent, ThreadOption.UIThread);

            moduleLockStatus.RequestModuleLockCmd = requestModuleLockCmd;
            moduleLockStatus.ToExitModuleLockCmd = toExitModuleLockCmd;
            contentVM.ModuleLockStatus = moduleLockStatus;

            var allFundAccounts = manageFundAccountItemsCtrl.AllFundAccountItems?.ToArray();
            contentVM.FundAccountItems.Clear();
            contentVM.FundAccountItems.AddRange(allFundAccounts);
            contentVM.SelectedFundAccountItem = allFundAccounts?.FirstOrDefault();

            if (allFundAccounts?.Any() != true)
            {
                manageFundAccountItemsCtrl.RefreshFundAccountItemsIfNeed();
            }

            // set content view as navigation container's first page
            navigationContainerView.Navigate(contentVM.View as Page);
        }

        public void Run()
        {
            RefreshModuleLockStatus();
        }

        public void Shutdown()
        {
            foreach (var _ctrl in positionDiffOverviewCtrls.Values)
            {
                _ctrl.Shutdown();
            }
            positionDiffOverviewCtrls.Clear();
        }

        public void RefreshPageDataIfNeed()
        {
            RefreshModuleLockStatus();
        }

        private void ContentViewModelPropChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(FundManageByFundAccountViewModel.SelectedFundAccountItem))
            {
                InvalidateSettlementContemtView();
            }
        }

        private void RequestModuleLock()
        {
            var landingInfo = loginDataService.ProxyLoginResp?.HostingSession?.HostingSession2LandingInfo();
            if (landingInfo == null) return;

            var editLock = new PositionEditLock { LockArea = trade_hosting_position_adjustConstants.POSITION_EDIT_AREA_MANUAL_INPUT };
            XqThriftLibConfigurationManager.SharedInstance.TradeHostingTerminalAoHttpStub.addPositionEditLockAsync(landingInfo, editLock, CancellationToken.None)
                .ContinueWith(t =>
                {
                    var resp = t.Result;
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        var _win = UIHelper.GetWindowOfUIElement(contentVM.View);
                        if (resp == null || resp.SourceException != null)
                        {
                            var errorMsg = FoundationHelper.FormatResponseDisplayErrorMsg(resp, "请求锁定状态出错！\n");
                            if (_win != null)
                            {
                                messageWindowService.ShowMessageDialog(_win, null, null, null, errorMsg);
                            }
                        }
                        RefreshModuleLockStatus();
                    });
                });
        }

        private void ToExitModuleLock()
        {
            var containerWin = UIHelper.GetWindowOfUIElement(contentVM.View);
            if (true != messageWindowService.ShowQuestionDialog(containerWin, null, null, null, "要退出锁定吗？", false, "退出", "取消"))
                return;

            var landingInfo = loginDataService.ProxyLoginResp?.HostingSession?.HostingSession2LandingInfo();
            if (landingInfo == null) return;

            var editLock = new PositionEditLock { LockArea = trade_hosting_position_adjustConstants.POSITION_EDIT_AREA_MANUAL_INPUT };
            XqThriftLibConfigurationManager.SharedInstance.TradeHostingTerminalAoHttpStub
                .removePositionEditLockAsync(landingInfo, editLock, CancellationToken.None)
                .ContinueWith(t =>
                {
                    var resp = t.Result;
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        var _win = UIHelper.GetWindowOfUIElement(contentVM.View);
                        if (resp == null || resp.SourceException != null)
                        {
                            var errorMsg = FoundationHelper.FormatResponseDisplayErrorMsg(resp, "退出锁定状态出错！\n");
                            if (_win != null)
                            {
                                messageWindowService.ShowMessageDialog(_win, null, null, null, errorMsg);
                            }
                        }
                        RefreshModuleLockStatus();
                    });
                });
        }

        private void RefreshModuleLockStatus()
        {
            var landingInfo = loginDataService.ProxyLoginResp?.HostingSession?.HostingSession2LandingInfo();
            if (landingInfo == null) return;

            XqThriftLibConfigurationManager.SharedInstance.TradeHostingTerminalAoHttpStub.reqPositionEditLockAsync(landingInfo, trade_hosting_position_adjustConstants.POSITION_EDIT_AREA_MANUAL_INPUT, CancellationToken.None)
                .ContinueWith(t =>
                {
                    var resp = t.Result;
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        InvalidateModuleLockStatus(resp?.CorrectResult);
                    });
                });
        }

        private void InvalidateModuleLockStatus(PositionEditLock lockData)
        {
            if (lockData == null || !lockData.__isset.subUserId || lockData.SubUserId == 0)
            {
                this.moduleLockStatus.LockState = ModuleLockState.UnLocked;
                this.moduleLockStatus.ModuleLockedUser = null;
            }
            else
            {
                ModuleLockState lockState = ModuleLockState.UnLocked;
                // 设置 lockState
                if (lockData.SubUserId == loginDataService.ProxyLoginResp?.HostingSession?.SubUserId)
                    lockState = ModuleLockState.LockedBySelf;
                else
                    lockState = ModuleLockState.LockedByOtherUser;

                this.moduleLockStatus.LockState = lockState;

                // 设置 ModuleLockedUser
                var landingInfo = loginDataService.ProxyLoginResp?.HostingSession?.HostingSession2LandingInfo();
                if (landingInfo != null)
                {
                    XqThriftLibConfigurationManager.SharedInstance.TradeHostingTerminalAoHttpStub
                    .getHostingUserPageAsync(landingInfo,
                        new QueryHostingUserOption { SubUserId = (int)lockData.SubUserId },
                        new IndexedPageOption { PageIndex = 0, PageSize = 1 },
                        CancellationToken.None)
                    .ContinueWith(t =>
                    {
                        this.moduleLockStatus.ModuleLockedUser = t.Result?.CorrectResult?.ResultList?.FirstOrDefault(i => i.SubUserId == lockData.SubUserId);
                    });
                }
            }

            if (this.moduleLockStatus.LockState == ModuleLockState.LockedBySelf)
            {
                PropertyChangedEventManager.RemoveHandler(contentVM, ContentViewModelPropChanged, "");
                InvalidateSettlementContemtView();
                PropertyChangedEventManager.AddHandler(contentVM, ContentViewModelPropChanged, "");
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
                InvalidateSettlementContemtView();
            }
        }

        private void InvalidateSettlementContemtView()
        {
            var selectedFundAccountId = contentVM.SelectedFundAccountItem?.TradeAccountId;
            if (selectedFundAccountId == null) return;

            PositionDiffOverviewCtrl overviewCtrl = null;
            if (!positionDiffOverviewCtrls.TryGetValue(selectedFundAccountId.Value, out overviewCtrl))
            {
                overviewCtrl = positionDiffOverviewCtrlFactory.CreateExport().Value;
                overviewCtrl.TradeAccountId = selectedFundAccountId.Value;
                overviewCtrl.NavigationContainerGetter = _ctrl => this.navigationContainerView;
                overviewCtrl.Initialize();
                overviewCtrl.Run();

                positionDiffOverviewCtrls[selectedFundAccountId.Value] = overviewCtrl;
            }

            contentVM.SettlementContentView = overviewCtrl.ContentView;
        }
    }
}
