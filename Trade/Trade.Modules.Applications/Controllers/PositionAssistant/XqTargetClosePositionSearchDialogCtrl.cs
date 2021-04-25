using ContainerShell.Interfaces.Applications;
using NativeModel.Trade;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows;
using xueqiao.trade.hosting.proxy;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.BusinessResources.Models;
using XueQiaoFoundation.Interfaces.Applications;
using XueQiaoFoundation.Interfaces.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;
using XueQiaoWaf.Trade.Modules.Applications.Helper;
using XueQiaoWaf.Trade.Modules.Applications.ViewModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers
{
    /// <summary>
    /// 配对历史查询 dialog controller
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class XqTargetClosePositionSearchDialogCtrl : IController
    {
        private readonly XqTargetClosePositionSearchVM contentViewModel;
        private readonly ExportFactory<XqTargetClosePositionHistoryViewCtrl> historyViewCtrlFactory;
        private readonly ExportFactory<ComposeSearchPopupController> composeSearchPopupCtrlFactory;
        private readonly IContainerShellService containerShellService;
        private readonly ILoginDataService loginDataService;
        private readonly Lazy<ILoginUserManageService> loginUserManageService;
        private readonly IMessageWindowService messageWindowService;
        private readonly IEventAggregator eventAggregator;
        private readonly IComposeGraphCacheController composeGraphCacheController;
        private readonly IComposeGraphQueryController composeGraphQueryController;
        private readonly IUserComposeViewCacheController userComposeViewCacheController;
        private readonly IUserComposeViewQueryController userComposeViewQueryController;
        private readonly IContractItemTreeQueryController contractItemTreeQueryController;


        private readonly DelegateCommand triggerSelectComposeCmd;
        private readonly DelegateCommand triggerSelectContractCmd;

        private XqTargetClosePositionHistoryViewCtrl historyViewCtrl;
        private IMessageWindow dialog;

        private string selectedTargetKey;

        [ImportingConstructor]
        public XqTargetClosePositionSearchDialogCtrl(
            XqTargetClosePositionSearchVM contentViewModel,
            ExportFactory<XqTargetClosePositionHistoryViewCtrl> historyViewCtrlFactory,
            ExportFactory<ComposeSearchPopupController> composeSearchPopupCtrlFactory,
            IContainerShellService containerShellService,
            ILoginDataService loginDataService,
            Lazy<ILoginUserManageService> loginUserManageService,
            IMessageWindowService messageWindowService,
            IEventAggregator eventAggregator,
            IComposeGraphCacheController composeGraphCacheController,
            IComposeGraphQueryController composeGraphQueryController,
            IUserComposeViewCacheController userComposeViewCacheController,
            IUserComposeViewQueryController userComposeViewQueryController,
            IContractItemTreeQueryController contractItemTreeQueryController)
        {
            this.contentViewModel = contentViewModel;
            this.historyViewCtrlFactory = historyViewCtrlFactory;
            this.composeSearchPopupCtrlFactory = composeSearchPopupCtrlFactory;
            this.containerShellService = containerShellService;
            this.loginDataService = loginDataService;
            this.loginUserManageService = loginUserManageService;
            this.messageWindowService = messageWindowService;
            this.eventAggregator = eventAggregator;
            this.composeGraphCacheController = composeGraphCacheController;
            this.composeGraphQueryController = composeGraphQueryController;
            this.userComposeViewCacheController = userComposeViewCacheController;
            this.userComposeViewQueryController = userComposeViewQueryController;
            this.contractItemTreeQueryController = contractItemTreeQueryController;

            triggerSelectComposeCmd = new DelegateCommand(TriggerSelectCompose);
            triggerSelectContractCmd = new DelegateCommand(TriggerSelectContract);

            loginUserManageService.Value.HasLogouted += RecvUserHasLogouted;
        }

        public object DialogOwner { get; set; }

        /// <summary>
        /// 操作账户 id
        /// </summary>
        public long SubAccountId { get; set; }

        /// <summary>
        /// 标的类型
        /// </summary>
        public ClientXQOrderTargetType InitialSelectedXqTargetType { get; set; }

        public void Initialize()
        {
            contentViewModel.TriggerSelectComposeCmd = triggerSelectComposeCmd;
            contentViewModel.TriggerSelectContractCmd = triggerSelectContractCmd;
            contentViewModel.HasSelectedXqTarget = false;

            PropertyChangedEventManager.AddHandler(contentViewModel, ContentVMPropChanged, "");
            contentViewModel.SelectedXqTargetType = this.InitialSelectedXqTargetType;
        }

        public void Run()
        {
            dialog = messageWindowService.CreateContentCustomWindow(DialogOwner, null, new Size(1000, 800), true, true,
                true, "配对记录", contentViewModel.View);
            dialog.ShowDialog();
        }

        public void Shutdown()
        {
            if (dialog != null)
            {
                dialog.Close();
                dialog = null;
            }
            loginUserManageService.Value.HasLogouted -= RecvUserHasLogouted;
            PropertyChangedEventManager.RemoveHandler(contentViewModel, ContentVMPropChanged, "");

            historyViewCtrl?.Shutdown();
            historyViewCtrl = null;
        }

        private void RecvUserHasLogouted(ProxyLoginResp lastLoginResp)
        {
            Shutdown();
        }

        private void ContentVMPropChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(XqTargetClosePositionSearchVM.SelectedXqTargetType))
            {
                this.selectedTargetKey = null;
                contentViewModel.HasSelectedXqTarget = false;
                contentViewModel.TargetComposeUserComposeViewContainer = null;
                contentViewModel.TargetContractDetailContainer = null;
                contentViewModel.XqTargetClosePositionHistoryContentView = null;
            }
        }

        private void TriggerSelectCompose(object triggerElement)
        {
            var popCtrl = composeSearchPopupCtrlFactory.CreateExport().Value;
            popCtrl.PopupPalcementTarget = triggerElement;
            popCtrl.PopupCloseHandler = (_ctrl, _selectComposeId) =>
            {
                _ctrl.Shutdown();
                if (_selectComposeId == null) return;

                if (contentViewModel.SelectedXqTargetType == ClientXQOrderTargetType.COMPOSE_TARGET
                    && this.selectedTargetKey != $"{_selectComposeId}")
                {
                    this.selectedTargetKey = $"{_selectComposeId}";
                    contentViewModel.HasSelectedXqTarget = true;

                    contentViewModel.TargetComposeUserComposeViewContainer = new UserComposeViewContainer(_selectComposeId.Value);
                    XueQiaoFoundationHelper.SetupUserComposeView(contentViewModel.TargetComposeUserComposeViewContainer,
                        userComposeViewCacheController, userComposeViewQueryController, false, true);

                    InvalidateClosePositionHistoryView();
                }
            };
            popCtrl.Initialize();
            popCtrl.Run();
        }

        private void TriggerSelectContract(object triggerElement)
        {
            containerShellService.ShowContractQuickSearchPopup(triggerElement, null,
                _selContractId =>
                {
                    if (_selContractId == null) return;

                    if (contentViewModel.SelectedXqTargetType == ClientXQOrderTargetType.CONTRACT_TARGET
                        && this.selectedTargetKey != $"{_selContractId}")
                    {
                        this.selectedTargetKey = $"{_selContractId}";
                        contentViewModel.HasSelectedXqTarget = true;

                        var loginUserId = loginDataService.ProxyLoginResp?.HostingSession?.SubUserId ?? 0;
                        contentViewModel.TargetContractDetailContainer = new TargetContract_TargetContractDetail(_selContractId.Value);
                        XueQiaoFoundationHelper.SetupTargetContract_ContractDetail(contentViewModel.TargetContractDetailContainer, 
                            contractItemTreeQueryController, XqContractNameFormatType.CommodityAcronym_Code_ContractCode);

                        InvalidateClosePositionHistoryView();
                    }
                });
        }

        private void InvalidateClosePositionHistoryView()
        {
            if (historyViewCtrl != null)
            {
                historyViewCtrl.Shutdown();
                historyViewCtrl = null;
            }

            historyViewCtrl = historyViewCtrlFactory.CreateExport().Value;
            historyViewCtrl.SubAccountId = this.SubAccountId;
            historyViewCtrl.XqTargetKey = this.selectedTargetKey;
            historyViewCtrl.XqTargetType = this.contentViewModel.SelectedXqTargetType;
            historyViewCtrl.Initialize();
            historyViewCtrl.Run();

            contentViewModel.XqTargetClosePositionHistoryContentView = historyViewCtrl.ContentView;
        }
    }
}
