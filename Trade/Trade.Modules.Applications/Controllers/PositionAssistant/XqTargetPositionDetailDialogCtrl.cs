using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using xueqiao.trade.hosting.proxy;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.ViewModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers
{
    /// <summary>
    /// 雪橇标的持仓明细弹窗 controller
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class XqTargetPositionDetailDialogCtrl : IController
    {
        private readonly XqTargetPositionDetailVM contentViewModel;
        private readonly XqTargetPositionItemDialogTitleVM dialogTitleVM;
        private readonly ExportFactory<XqTargetPositionDetailContentViewCtrl> positionDetailContentViewCtrlFactory;
        private readonly ExportFactory<XqTargetClosePositionHistoryViewCtrl> closePositionHistoryViewCtrlFactory;
        private readonly IMessageWindowService messageWindowService;
        private readonly Lazy<ILoginUserManageService> loginUserManageService;
        private readonly IEventAggregator eventAggregator;

        private XqTargetPositionDetailContentViewCtrl positionDetailContentViewCtrl;
        private XqTargetClosePositionHistoryViewCtrl closePositionHistoryViewCtrl;

        private IMessageWindow dialog;

        [ImportingConstructor]
        public XqTargetPositionDetailDialogCtrl(
            XqTargetPositionDetailVM contentViewModel,
            XqTargetPositionItemDialogTitleVM dialogTitleVM,
            ExportFactory<XqTargetPositionDetailContentViewCtrl> positionDetailContentViewCtrlFactory,
            ExportFactory<XqTargetClosePositionHistoryViewCtrl> closePositionHistoryViewCtrlFactory,
            IMessageWindowService messageWindowService,
            Lazy<ILoginUserManageService> loginUserManageService,
            IEventAggregator eventAggregator)
        {
            this.contentViewModel = contentViewModel;
            this.dialogTitleVM = dialogTitleVM;
            this.positionDetailContentViewCtrlFactory = positionDetailContentViewCtrlFactory;
            this.closePositionHistoryViewCtrlFactory = closePositionHistoryViewCtrlFactory;
            this.messageWindowService = messageWindowService;
            this.loginUserManageService = loginUserManageService;
            this.eventAggregator = eventAggregator;

            loginUserManageService.Value.HasLogouted += RecvUserHasLogouted;
        }

        public object DialogOwner { get; set; }

        public TargetPositionDataModel XqTargetPositionItem { get; set; }

        public void Initialize()
        {
            if (XqTargetPositionItem == null) throw new ArgumentNullException("XqTargetPositionItem");

            dialogTitleVM.XqTargetPositionItem = XqTargetPositionItem;
            dialogTitleVM.TitlePrefix = "持仓明细：";
            contentViewModel.XqTargetPositionItem = XqTargetPositionItem;
        }

        public void Run()
        {
            contentViewModel.SelectedContentTabType = XqTargetPositionContentTabType.PositionDetailTab;
            PropertyChangedEventManager.AddHandler(contentViewModel, ContentViewModelPropChanged, "");
            InvalidateTabContentView();

            dialog = messageWindowService.CreateContentCustomWindow(DialogOwner, null, new Size(1000, 800), true, true,
                true, dialogTitleVM.View, contentViewModel.View);
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
            PropertyChangedEventManager.RemoveHandler(contentViewModel, ContentViewModelPropChanged, "");
            positionDetailContentViewCtrl?.Shutdown();
            positionDetailContentViewCtrl = null;
            closePositionHistoryViewCtrl?.Shutdown();
            closePositionHistoryViewCtrl = null;
        }

        private void RecvUserHasLogouted(ProxyLoginResp lastLoginResp)
        {
            Shutdown();
        }

        private void ContentViewModelPropChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(XqTargetPositionDetailVM.SelectedContentTabType))
            {
                InvalidateTabContentView();
            }
        }

        private void InvalidateTabContentView()
        {
            var selTabType = contentViewModel.SelectedContentTabType;
            if (selTabType == XqTargetPositionContentTabType.PositionDetailTab)
            {
                ShowPositionDetailTabContentView();
            }
            else if (selTabType == XqTargetPositionContentTabType.HistoryClosePositionTab)
            {
                ShowHistoryClosePositionTabContentView();
            }
        }

        private void ShowPositionDetailTabContentView()
        {
            if (positionDetailContentViewCtrl == null)
            {
                var ctrl = positionDetailContentViewCtrlFactory.CreateExport().Value;
                ctrl.XqTargetPositionItem = this.XqTargetPositionItem;
                ctrl.Initialize();
                ctrl.Run();

                positionDetailContentViewCtrl = ctrl;
            }
            else
            {
                positionDetailContentViewCtrl.RefreshViewDataIfNeed();
            }
            contentViewModel.ContentTabContentView = positionDetailContentViewCtrl.ContentView;
        }

        private void ShowHistoryClosePositionTabContentView()
        {
            if (closePositionHistoryViewCtrl == null)
            {
                var ctrl = closePositionHistoryViewCtrlFactory.CreateExport().Value;
                ctrl.SubAccountId = this.XqTargetPositionItem.SubAccountFields.SubAccountId;
                ctrl.XqTargetKey = this.XqTargetPositionItem.TargetKey;
                ctrl.XqTargetType = this.XqTargetPositionItem.TargetType;
                ctrl.Initialize();
                ctrl.Run();

                closePositionHistoryViewCtrl = ctrl;
            }
            contentViewModel.ContentTabContentView = closePositionHistoryViewCtrl.ContentView;
        }
    }
}
