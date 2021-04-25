using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.BusinessResources.Models
{
    /// <summary>
    /// 工作空间窗口树
    /// </summary>
    public class WorkspaceWindowTree
    {   
        /// <summary>
        /// 工作空间的拆分窗口列表
        /// </summary>
        public TabWorkspaceWindow[] WorkspaceInterTabWindowList { get; set; }

        /// <summary>
        /// 主窗口包含的工作空间 id 列表
        /// </summary>
        public string[] MainWindowWorkspaceKeyList { get; set; }
        
    }

    /// <summary>
    /// 工作区窗口
    /// </summary>
    public class TabWorkspaceWindow
    {
        /// <summary>
        /// 所在坐标 left
        /// </summary>
        public double Left { get; set; }

        /// <summary>
        /// 所在坐标 top
        /// </summary>
        public double Top { get; set; }

        /// <summary>
        /// 宽度
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// 高度
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// 是否最大化
        /// </summary>
        public bool IsMaximized { get; set; }

        /// <summary>
        /// 包含的工作空间 key 列表
        /// </summary>
        public string[] ChildWorkspaceKeys { get; set; }
    }
}
