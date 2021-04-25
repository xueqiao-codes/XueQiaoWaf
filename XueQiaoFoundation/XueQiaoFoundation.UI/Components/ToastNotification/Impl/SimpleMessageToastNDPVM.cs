using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.UI.Components.ToastNotification.Impl
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class SimpleMessageToastNDPVM : XqToastNotificationNDPVMBase<SimpleMessageToastNDP>
    {
        [ImportingConstructor]
        protected SimpleMessageToastNDPVM(SimpleMessageToastNDP view) : base(view)
        {
        }

        private XqToastNotificationType notificationType;
        public XqToastNotificationType NotificationType
        {
            get { return notificationType; }
            set { SetProperty(ref notificationType, value); }
        }

        private object messageContent;
        public object MessageContent
        {
            get { return messageContent; }
            set { SetProperty(ref messageContent, value); }
        }
    }
}
