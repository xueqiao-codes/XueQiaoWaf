using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows;

namespace ContainerShell.Applications.Views
{
    public interface IWorkspaceTabablzControl : IView
    {
        object WorkspaceTabControl { get; }

        object ContainerWindow { get; }

        /// <summary>
        /// 某个工作空间项的显示元素
        /// </summary>
        /// <param name="workspaceItemData"></param>
        /// <returns></returns>
        UIElement WorkspaceItemElement(object workspaceItemData);
    }
}
