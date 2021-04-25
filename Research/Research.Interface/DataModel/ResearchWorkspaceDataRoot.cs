using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.BusinessResources.DataModels;

namespace Research.Interface.DataModel
{
    /// <summary>
    /// 投研模块的工作空间数据根
    /// </summary>
    public class ResearchWorkspaceDataRoot
    {
        public ResearchWorkspaceDataRoot()
        {
            InterTabWorkspaceWindowListContainer = new InterTabWorkspaceWindowListContainer();
            MainWindowWorkspaceListContainer = new TabWorkspaceListContainer();
        }

        /// <summary>
        /// 拆分窗口列表容器
        /// </summary>
        public InterTabWorkspaceWindowListContainer InterTabWorkspaceWindowListContainer { get; private set; }

        /// <summary>
        /// 主窗口的工作空间容器
        /// </summary>
        public TabWorkspaceListContainer MainWindowWorkspaceListContainer { get; private set; }
    }
}
