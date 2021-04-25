using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.BusinessResources.DataModels
{
    /// <summary>
    /// 工作空间拆分窗口容器
    /// </summary>
    public class InterTabWorkspaceWindowContainer
    {
        public InterTabWorkspaceWindowContainer(TabWorkspaceWindow windowInfo)
        {
            if (windowInfo == null) throw new ArgumentNullException("windowInfo");
            this.WindowInfo = windowInfo;
            WorkspaceListContainer = new TabWorkspaceListContainer();
        }

        public TabWorkspaceWindow WindowInfo { get; private set; }

        public TabWorkspaceListContainer WorkspaceListContainer { get; private set; }
    }
}
