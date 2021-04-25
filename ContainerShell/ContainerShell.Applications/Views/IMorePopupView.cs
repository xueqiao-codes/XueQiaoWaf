using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;

namespace ContainerShell.Applications.Views
{
    public interface IMorePopupView : IView
    {
        event EventHandler Closed;

        void Close();

        void ShowPopup(object targetElement);
    }
}
