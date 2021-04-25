using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.BusinessResources.DataModels;

namespace ContainerShell.Interfaces.Applications
{
    /// <summary>
    /// 工作空间 TabControl 视图控制器协议
    /// </summary>
    public interface IWorkspaceInterTabWindowCtrl
    {
        /// <summary>
        /// 当前分拆窗口容器所在的分拆窗口列表容器
        /// </summary>
        InterTabWorkspaceWindowListContainer InterTabWindowListContainer { get; set; }

        /// <summary>
        /// 当前分拆窗口容器
        /// </summary>
        InterTabWorkspaceWindowContainer InterTabWindowContainer { get; set; }

        /// <summary>
        /// 新建某个工作空间视图控制器的方法
        /// </summary>
        WorkspaceItemViewCtrlFactory NewWorkspaceItemViewCtrlFactory { get; set; }

        /// <summary>
        /// 获取新的工作空间方法
        /// </summary>
        NewWorkspaceFactory NewWorkspaceFactory { get; set; }

        /// <summary>
        /// tab control 的 tab 拆分控制 key。从相同 key 的tab control 拆分出去的 tab 可以合并回至该 tab control
        /// </summary>
        string InterTabPartitionKey { get; set; }

        /// <summary>
        /// 是否在 run 时显示窗口，否则不显示
        /// </summary>
        bool ShowWindowWhenRun { get; set; }

        /// <summary>
        /// 窗口关闭回调
        /// </summary>
        Action<IWorkspaceInterTabWindowCtrl> WindowCloseAction { get; set; }

        /// <summary>
        /// 窗口UI
        /// </summary>
        object WindowElement { get; }

        /// <summary>
        /// 显示工作空间的 tab control UI元素
        /// </summary>
        object WorkspaceTabControl { get; }

        void Initialize();

        void Run();

        void Shutdown();
    }
}
