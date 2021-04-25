using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.UI.Components.MessageWindow.Services
{
    public interface IMessageWindow
    {
        event CancelEventHandler Closing;

        event EventHandler Closed;

        event EventHandler ContentRendered;

        void ShowDialog(bool topmost = false);

        void Close();

        void Show(bool topmost = false);

        void Activate();

        void Hide();

        bool Topmost { get; set; }
    }
}
