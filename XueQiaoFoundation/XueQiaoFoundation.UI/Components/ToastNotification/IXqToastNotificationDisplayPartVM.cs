using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToastNotifications.Core;

namespace XueQiaoFoundation.UI.Components.ToastNotification
{
    /// <summary>
    /// 雪橇 Toast 消息显示视图 view model 协议
    /// </summary>
    public interface IXqToastNotificationDisplayPartVM
    {
        /// <summary>
        /// 显示视图
        /// </summary>
        NotificationDisplayPart DisplayPart { get; }

        /// <summary>
        /// Toast 数据
        /// </summary>
        XqToastNotification ToastNotification { get; set; }

        /// <summary>
        /// 在鼠标进入时是否不关闭 Toast
        /// </summary>
        bool FreezeToastOnMouseEnter { get; set; }
    }
}
