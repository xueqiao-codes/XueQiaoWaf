using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using ToastNotifications.Core;

namespace XueQiaoFoundation.UI.Components.ToastNotification
{
    public class XqToastNotificationNDPVMBase<TView> : ViewModel<TView>, IXqToastNotificationDisplayPartVM where TView : IXqToastNotificationNDP
    {
        protected XqToastNotificationNDPVMBase(TView view) : base(view)
        {
            view.MouseEnter += View_MouseEnter;
            view.MouseLeave += View_MouseLeave;
        }
        
        public NotificationDisplayPart DisplayPart => (this.ViewCore as NotificationDisplayPart);
        
        public XqToastNotification ToastNotification { get; set; }

        public bool FreezeToastOnMouseEnter { get; set; }

        private void View_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var toast = this.ToastNotification;
            if (toast != null && this.FreezeToastOnMouseEnter)
                toast.CanClose = false;
        }

        private void View_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var toast = this.ToastNotification;
            if (toast != null)
                toast.CanClose = true;
        }
    }
}
