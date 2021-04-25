using ContainerShell.Applications.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.UI.Components.ToastNotification;

namespace ContainerShell.Applications.ViewModels
{
    /// <summary>
    /// 订单状态不明确 Toast 消息 NotificationDisplayPart ViewModel
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class OrderAmbiguousNDPVM : XqToastNotificationNDPVMBase<IOrderAmbiguousNDP>
    {
        [ImportingConstructor]
        protected OrderAmbiguousNDPVM(IOrderAmbiguousNDP view) : base(view)
        {
        }

        private ICommand closeCmd;
        public ICommand CloseCmd
        {
            get { return closeCmd; }
            set { SetProperty(ref closeCmd, value); }
        }

        private ICommand showDetailCmd;
        public ICommand ShowDetailCmd
        {
            get { return showDetailCmd; }
            set { SetProperty(ref showDetailCmd, value); }
        }

        /// <summary>
        /// 订单信息
        /// </summary>
        private OrderItemDataModel order;
        public OrderItemDataModel Order
        {
            get { return order; }
            set { SetProperty(ref order, value); }
        }
    }
}
