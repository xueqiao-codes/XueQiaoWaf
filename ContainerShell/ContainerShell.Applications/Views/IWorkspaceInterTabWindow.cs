using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows;

namespace ContainerShell.Applications.Views
{
    public interface IWorkspaceInterTabWindow : IView
    {
        object ConainerWindow { get; }

        void Show();

        void Close();

        event RoutedEventHandler Loaded;

        event CancelEventHandler Closing;

        event EventHandler Closed;
        
    }
}
