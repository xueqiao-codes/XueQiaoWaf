using NativeModel.Trade;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows;
using xueqiao.trade.hosting.proxy;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;

using XueQiaoWaf.Trade.Interfaces.Applications;
using XueQiaoWaf.Trade.Modules.Applications.ViewModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class XqTargetConditionOrdersDialogCtrl : IController
    {
        private readonly XqTargetConditionOrdersVM contentVM;
        private readonly ExportFactory<RelatedOrderDialogCtrl> relatedOrderDialogCtrlFactory;
        private readonly ISelectedOrdersOperateCommandsCtrl selectedOrdersOperateCommandsCtrl;
        private readonly IMessageWindowService messageWindowService;
        private readonly Lazy<ILoginUserManageService> loginUserManageService;
        private readonly IEventAggregator eventAggregator;

        private readonly DelegateCommand toShowChildOrderCmd;

        private IMessageWindow dialog;

        [ImportingConstructor]
        public XqTargetConditionOrdersDialogCtrl(XqTargetConditionOrdersVM contentVM,
            ExportFactory<RelatedOrderDialogCtrl> relatedOrderDialogCtrlFactory,
            ISelectedOrdersOperateCommandsCtrl selectedOrdersOperateCommandsCtrl,
            IMessageWindowService messageWindowService,
            Lazy<ILoginUserManageService> loginUserManageService,
            IEventAggregator eventAggregator)
        {
            this.contentVM = contentVM;
            this.relatedOrderDialogCtrlFactory = relatedOrderDialogCtrlFactory;
            this.selectedOrdersOperateCommandsCtrl = selectedOrdersOperateCommandsCtrl;
            this.messageWindowService = messageWindowService;
            this.loginUserManageService = loginUserManageService;
            this.eventAggregator = eventAggregator;

            toShowChildOrderCmd = new DelegateCommand(ToShowChildOrder);

            loginUserManageService.Value.HasLogouted += RecvUserHasLogouted;
        }

        public object DialogOwner { get; set; }

        public long SubAccountId { get; set; }

        public ClientXQOrderTargetType TargetType { get; set; }

        public string TargetKey { get; set; }

        public void Initialize()
        {
            selectedOrdersOperateCommandsCtrl.WindowOwnerGetter = () => UIHelper.GetWindowOfUIElement(contentVM.View);
            selectedOrdersOperateCommandsCtrl.Initialize();

            contentVM.OrderListDataContext.SelectedOrdersOptCommands = selectedOrdersOperateCommandsCtrl.SelectedOrdersOptCommands;
            contentVM.UpdateXqTargetPresentOrderListKey(this.SubAccountId, this.TargetType, this.TargetKey);
            contentVM.ToShowChildOrderCmd = toShowChildOrderCmd;
        }

        public void Run()
        {
            dialog = messageWindowService.CreateContentCustomWindow(DialogOwner, null, new Size(900, 840), true, true,
                true, "条件单", contentVM.View);
            dialog.ShowDialog();
        }

        public void Shutdown()
        {
            if (dialog != null)
            {
                dialog.Close();
                dialog = null;
            }

            selectedOrdersOperateCommandsCtrl.Shutdown();
            loginUserManageService.Value.HasLogouted -= RecvUserHasLogouted;
        }

        private void RecvUserHasLogouted(ProxyLoginResp lastLoginResp)
        {
            Shutdown();
        }

        private void ToShowChildOrder(object obj)
        {
            var orderItem = obj as OrderItemDataModel_Condition;
            if (orderItem == null) return;

            var dialogCtrl = relatedOrderDialogCtrlFactory.CreateExport().Value;
            dialogCtrl.DialogOwner = UIHelper.GetWindowOfUIElement(contentVM.View);
            dialogCtrl.CurrentOrder = orderItem;

            dialogCtrl.Initialize();
            dialogCtrl.Run();
            dialogCtrl.Shutdown();
        }
    }
}
