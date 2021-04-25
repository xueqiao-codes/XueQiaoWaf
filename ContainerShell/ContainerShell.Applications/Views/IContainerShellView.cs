using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;

namespace ContainerShell.Applications.Views
{
    public interface IContainerShellView : IView
    {
        /// <summary>
        /// Show shell
        /// </summary>
        void Show();

        /// <summary>
        /// Close shell
        /// </summary>
        void Close();

        /// <summary>
        /// Closing event 
        /// </summary>
        event CancelEventHandler Closing;

        /// <summary>
        /// Closed event
        /// </summary>
        event EventHandler Closed;
        
        /// <summary>
        /// 显示更多功能触发元素
        /// </summary>
        object ShowMoreFunctionTriggerElement { get; }
    }
}
