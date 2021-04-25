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
    /// 订单触发 Toast 消息 NotificationDisplayPart ViewModel
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class OrderTriggeredNDPVM : XqToastNotificationNDPVMBase<IOrderTriggeredNDP>
    {
        [ImportingConstructor]
        protected OrderTriggeredNDPVM(IOrderTriggeredNDP view) : base(view)
        {
        }
        
        private ICommand closeCmd;
        public ICommand CloseCmd
        {
            get { return closeCmd; }
            set { SetProperty(ref closeCmd, value); }
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
