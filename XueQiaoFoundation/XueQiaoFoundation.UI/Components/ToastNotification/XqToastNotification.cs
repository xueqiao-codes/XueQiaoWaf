using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToastNotifications.Core;

namespace XueQiaoFoundation.UI.Components.ToastNotification
{
    /// <summary>
    /// 雪橇 Toast 数据类
    /// </summary>
    public class XqToastNotification : NotificationBase
    {
        public XqToastNotification(IXqToastNotificationDisplayPartVM notificationDisplayPartVM, bool freezeToastOnMouseEnter)
        {
            this.NotificationDisplayPartVM = notificationDisplayPartVM;
            if (notificationDisplayPartVM != null)
            {
                notificationDisplayPartVM.ToastNotification = this;
                notificationDisplayPartVM.FreezeToastOnMouseEnter = freezeToastOnMouseEnter;
            }
        }

        public readonly IXqToastNotificationDisplayPartVM NotificationDisplayPartVM;

        public override NotificationDisplayPart DisplayPart => NotificationDisplayPartVM.DisplayPart;
    }
}
