using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;

namespace XueQiaoFoundation.UI.Components.ToastNotification
{
    public interface IXqToastNotificationNDP : IView
    {
        event MouseEventHandler MouseEnter;

        event MouseEventHandler MouseLeave;
    }
}
