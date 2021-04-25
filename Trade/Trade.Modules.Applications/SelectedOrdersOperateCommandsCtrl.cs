using AppAssembler.Interfaces.Applications;
using lib.xqclient_base.logger;
using NativeModel.Trade;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using xueqiao.trade.hosting.arbitrage.thriftapi;
using XueQiaoFoundation.BusinessResources.Constants;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.Interfaces.Applications;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoWaf.Trade.Interfaces.Applications;
using XueQiaoWaf.Trade.Modules.Applications.ViewModels;

namespace XueQiaoWaf.Trade.Modules.Applications
{
    /// <summary>
    /// 选中的订单项操作 command 控制。在引用方的生命周期结束前需要保持该实例，才能有效控制选中订单的操作 command
    /// </summary>
    [Export, Export(typeof(ISelectedOrdersOperateCommandsCtrl)), PartCreationPolicy(CreationPolicy.NonShared)]
    internal class SelectedOrdersOperateCommandsCtrl : ISelectedOrdersOperateCommandsCtrl
    {
        // 可暂停的订单标的类型
        private readonly static ClientXQOrderTargetType[] CanSuspendOrderTargetTypes 
            = new ClientXQOrderTargetType[] { ClientXQOrderTargetType.COMPOSE_TARGET };

        private readonly IOrderItemsController orderItemsController;
        private readonly IAppAssemblerService appAssemblerService;
        private readonly IMessageWindowService messageWindowService;
        private readonly ExportFactory<OrderOperateConfirmVM> orderOperateConfirmVMFactory;

        private readonly DelegateCommand orderItemsSelectionChangedCmd;
        private readonly DelegateCommand revokeSelectedOrdersCmd;
        private readonly DelegateCommand suspendSelectedOrdersCmd;
        private readonly DelegateCommand resumeSelectedOrdersCmd;
        private readonly DelegateCommand strongChaseSelectedOrdersCmd;

        private IEnumerable<OrderItemDataModel> selectedOrderItems;

        [ImportingConstructor]
        public SelectedOrdersOperateCommandsCtrl(
            IOrderItemsController orderItemsController,
            IAppAssemblerService appAssemblerService,
            IMessageWindowService messageWindowService,
            ExportFactory<OrderOperateConfirmVM> orderOperateConfirmVMFactory)
        {
            this.orderItemsController = orderItemsController;
            this.appAssemblerService = appAssemblerService;
            this.messageWindowService = messageWindowService;
            this.orderOperateConfirmVMFactory = orderOperateConfirmVMFactory;

            orderItemsSelectionChangedCmd = new DelegateCommand(OrderItemsSelectionChanged);
            revokeSelectedOrdersCmd = new DelegateCommand(RevokeSelectedOrders, CanRevokeSelectedOrders);
            suspendSelectedOrdersCmd = new DelegateCommand(SuspendSelectedOrders, CanSuspendSelectedOrders);
            resumeSelectedOrdersCmd = new DelegateCommand(ResumeSelectedOrders, CanResumeSelectedOrders);
            strongChaseSelectedOrdersCmd = new DelegateCommand(StrongChaseSelectedOrders, CanStrongChaseSelectedOrders);

            this.SelectedOrdersOptCommands = new SelectedOrdersOperateCommands
            {
                OrderItemsSelectionChangedCmd = orderItemsSelectionChangedCmd,
                RevokeSelectedOrdersCmd = revokeSelectedOrdersCmd,
                SuspendSelectedOrdersCmd = suspendSelectedOrdersCmd,
                ResumeSelectedOrdersCmd = resumeSelectedOrdersCmd,
                StrongChaseSelectedOrdersCmd = strongChaseSelectedOrdersCmd,
            };
        }

        public Func<object> WindowOwnerGetter { get; set; }
        
        public SelectedOrdersOperateCommands SelectedOrdersOptCommands { get; private set; }

        public void Initialize()
        {

        }

        public void Shutdown()
        {

        }

        private void OrderItemsSelectionChanged(object obj)
        {
            var oldSelOrderItems = this.selectedOrderItems;
            RemovePropertyChangedHandlerForOrderItems(oldSelOrderItems);

            var newSelOrders = (obj as IList)?.Cast<OrderItemDataModel>().ToArray();
            if (newSelOrders == null) return;
            
            this.selectedOrderItems = newSelOrders;
            AddPropertyChangedHandlerForOrderItems(newSelOrders);

            RaiseCanExecuteChangedOfOrderOptCmds();
        }

        private void AddPropertyChangedHandlerForOrderItems(IEnumerable<OrderItemDataModel> orderItems)
        {
            if (orderItems == null) return;
            foreach (var o in orderItems)
            {
                PropertyChangedEventManager.RemoveHandler(o, OrderItemPropChanged, "");
                PropertyChangedEventManager.AddHandler(o, OrderItemPropChanged, "");
            }
        }

        private void RemovePropertyChangedHandlerForOrderItems(IEnumerable<OrderItemDataModel> orderItems)
        {
            if (orderItems == null) return;
            foreach (var o in orderItems)
            {
                PropertyChangedEventManager.RemoveHandler(o, OrderItemPropChanged, "");
            }
        }

        private void OrderItemPropChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(OrderItemDataModel.OrderState)
                || e.PropertyName == nameof(OrderItemDataModel_Entrusted.EffectDate))
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() => 
                {
                    RaiseCanExecuteChangedOfOrderOptCmds();
                });
            }
        }

        private void RaiseCanExecuteChangedOfOrderOptCmds()
        {
            revokeSelectedOrdersCmd?.RaiseCanExecuteChanged();
            suspendSelectedOrdersCmd?.RaiseCanExecuteChanged();
            resumeSelectedOrdersCmd?.RaiseCanExecuteChanged();
            strongChaseSelectedOrdersCmd?.RaiseCanExecuteChanged();
        }

        private void RevokeSelectedOrders()
        {
            var toRevokeOrderIds = selectedOrderItems?.Where(i => XueQiaoConstants.RevokeEnabledOrderStates.Contains(i.OrderState))
                .Select(i => i.OrderId).ToArray();
            if (toRevokeOrderIds?.Any() != true) return;

            bool passConfirm = true;
            if (appAssemblerService.PreferenceManager.Config?.RevokeOrderNeedConfirm == true)
            {
                var confirmVM = orderOperateConfirmVMFactory.CreateExport().Value;
                confirmVM.NotConfirmNextTime = false;
                confirmVM.ViewContentMargin = new System.Windows.Thickness(0,0,0,0);
                confirmVM.NeedConfirmMessage = $"确定要撤单选中的订单吗？";
                
                var confirmResult = messageWindowService.ShowQuestionDialog(WindowOwnerGetter?.Invoke(), null, null, "订单撤单确认", confirmVM.View, false, "确认", "取消");
                passConfirm = (confirmResult == true);

                if (confirmResult == true && confirmVM.NotConfirmNextTime)
                {
                    appAssemblerService.PreferenceManager.Config.RevokeOrderNeedConfirm = false;
                    appAssemblerService.PreferenceManager.SaveConfig(out Exception _exception);
                    if (_exception != null)
                    {
                        AppLog.Error("Failed to save config.", _exception);
                    }
                }

                confirmVM = null;
            }

            if (passConfirm) {
                orderItemsController.RequestRevokeOrders(toRevokeOrderIds);
            }
        }

        private bool CanRevokeSelectedOrders()
        {
            return selectedOrderItems?.Any(i => XueQiaoConstants.RevokeEnabledOrderStates.Contains(i.OrderState)) ?? false;
        }

        private void SuspendSelectedOrders()
        {
            var toSuspendOrderIds = selectedOrderItems?
                .Where(i => CanSuspend(i))
                .Select(i => i.OrderId).ToArray();
            if (toSuspendOrderIds?.Any() != true) return;

            bool passConfirm = true;
            if (appAssemblerService.PreferenceManager.Config?.SuspendOrderNeedConfirm == true)
            {
                var confirmVM = orderOperateConfirmVMFactory.CreateExport().Value;
                confirmVM.NotConfirmNextTime = false;
                confirmVM.ViewContentMargin = new System.Windows.Thickness(0, 0, 0, 0);
                confirmVM.NeedConfirmMessage = $"确定要暂停选中的订单吗？";
                
                var confirmResult = messageWindowService.ShowQuestionDialog(WindowOwnerGetter?.Invoke(), null, null, "订单暂停确认", confirmVM.View, false, "确认", "取消");
                passConfirm = (confirmResult == true);

                if (confirmResult == true && confirmVM.NotConfirmNextTime)
                {
                    appAssemblerService.PreferenceManager.Config.SuspendOrderNeedConfirm = false;
                    appAssemblerService.PreferenceManager.SaveConfig(out Exception _exception);
                    if (_exception != null)
                    {
                        AppLog.Error("Failed to save config.", _exception);
                    }
                }

                confirmVM = null;
            }

            if (passConfirm) {
                orderItemsController.RequestSuspendOrders(toSuspendOrderIds);
            }
        }

        private bool CanSuspendSelectedOrders()
        {
            return selectedOrderItems?
                .Any(i => CanSuspend(i)) 
                ?? false;
        }
        
        private void ResumeSelectedOrders()
        {
            var toResumeOrderIds = selectedOrderItems?.Where(i => ClientXQOrderState.XQ_ORDER_SUSPENDED == i.OrderState)
                .Select(i => i.OrderId).ToArray();
            if (toResumeOrderIds?.Any() != true) return;

            bool passConfirm = true;
            if (appAssemblerService.PreferenceManager.Config?.ResumeOrderNeedConfirm == true)
            {
                var confirmVM = orderOperateConfirmVMFactory.CreateExport().Value;
                confirmVM.NotConfirmNextTime = false;
                confirmVM.ViewContentMargin = new System.Windows.Thickness(0, 0, 0, 0);
                confirmVM.NeedConfirmMessage = $"确定要启动选中的订单吗？";

                var confirmResult = messageWindowService.ShowQuestionDialog(WindowOwnerGetter?.Invoke(), null, null, "订单启动确认", confirmVM.View, false, "确认", "取消");
                passConfirm = (confirmResult == true);

                if (confirmResult == true && confirmVM.NotConfirmNextTime)
                {
                    appAssemblerService.PreferenceManager.Config.ResumeOrderNeedConfirm = false;
                    appAssemblerService.PreferenceManager.SaveConfig(out Exception _exception);
                    if (_exception != null)
                    {
                        AppLog.Error("Failed to save config.", _exception);
                    }
                }

                confirmVM = null;
            }

            if (passConfirm)
            {
                orderItemsController.RequestResumeOrders(toResumeOrderIds);
            }
        }

        private bool CanResumeSelectedOrders()
        {
            return selectedOrderItems?.Any(i => ClientXQOrderState.XQ_ORDER_SUSPENDED == i.OrderState) ?? false;
        }

        private void StrongChaseSelectedOrders()
        {
            var toStrongChaseOrderIds = selectedOrderItems?.Where(i => ClientXQOrderState.XQ_ORDER_SUSPENDED == i.OrderState && i.TargetType == ClientXQOrderTargetType.COMPOSE_TARGET)
                .Select(i => i.OrderId).ToArray();
            if (toStrongChaseOrderIds?.Any() != true) return;

            bool passConfirm = true;
            if (appAssemblerService.PreferenceManager.Config?.StrongChaseOrderNeedConfirm == true)
            {
                var confirmVM = orderOperateConfirmVMFactory.CreateExport().Value;
                confirmVM.NotConfirmNextTime = false;
                confirmVM.ViewContentMargin = new System.Windows.Thickness(0, 0, 0, 0);
                confirmVM.NeedConfirmMessage = $"确定要强追选中的订单吗？";

                var confirmResult = messageWindowService.ShowQuestionDialog(WindowOwnerGetter?.Invoke(), null, null, "订单强追确认", confirmVM.View, false, "确认", "取消");
                passConfirm = (confirmResult == true);

                if (confirmResult == true && confirmVM.NotConfirmNextTime)
                {
                    appAssemblerService.PreferenceManager.Config.StrongChaseOrderNeedConfirm = false;
                    appAssemblerService.PreferenceManager.SaveConfig(out Exception _exception);
                    if (_exception != null)
                    {
                        AppLog.Error("Failed to save config.", _exception);
                    }
                }

                confirmVM = null;
            }

            if (passConfirm)
            {
                orderItemsController.RequestStrongChaseOrders(toStrongChaseOrderIds);
            }
        }

        private bool CanStrongChaseSelectedOrders()
        {
            return selectedOrderItems?.Any(i => ClientXQOrderState.XQ_ORDER_SUSPENDED == i.OrderState && i.TargetType == ClientXQOrderTargetType.COMPOSE_TARGET) ?? false;
        }

        /// <summary>
        /// Judge if an order can suspend
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        private static bool CanSuspend(OrderItemDataModel order)
        {
            if (!XueQiaoConstants.SuspendEnabledOrderStates.Contains(order.OrderState))
                return false;

            if (order.TargetType == ClientXQOrderTargetType.CONTRACT_TARGET && order.ClientOrderType == XQClientOrderType.Entrusted)
            {
                var entrustedOrder = order as OrderItemDataModel_Entrusted;
                if (entrustedOrder == null) return false;
                var effecDateType = entrustedOrder.EffectDate?.Type;
                return (effecDateType != null && effecDateType != HostingXQEffectDateType.XQ_EFFECT_TODAY);
            }

            return true;
        }
    }
}
