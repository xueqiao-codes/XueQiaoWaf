using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.Shared.Model;

namespace ContainerShell.Interfaces.Applications
{
    public delegate IWorkspaceItemViewCtrl WorkspaceItemViewCtrlFactory(IWorkspaceTabControlViewCtrl viewCtrl, TabWorkspace workspace);

    public delegate TabWorkspace NewWorkspaceFactory(IWorkspaceTabControlViewCtrl viewCtrl);

    /// <summary>
    /// 工作空间 TabControl 视图控制器协议
    /// </summary>
    public interface IWorkspaceTabControlViewCtrl 
    {
        /// <summary>
        /// 分拆窗口列表容器，记录分拆工作空间窗口的增删变化
        /// </summary>
        InterTabWorkspaceWindowListContainer InterTabWindowListContainer { get; set; }

        /// <summary>
        /// 当前工作空间列表容器，能记录该 tabControl 下的工作空间的增删改变化
        /// </summary>
        TabWorkspaceListContainer WorkspaceListContainer { get; set; }

        /// <summary>
        /// 固定工作空间的数目
        /// </summary>
        int FixedItemsCount { get; set; }

        /// <summary>
        /// 当工作空间数目为空时,是否关闭所在窗口
        /// </summary>
        bool CloseWindowWhenItemsEmptied { get; set; }

        /// <summary>
        /// 新建某个工作空间视图控制器的方法
        /// </summary>
        WorkspaceItemViewCtrlFactory NewItemViewCtrlFactory { get; set; }

        /// <summary>
        /// 获取新的工作空间方法
        /// </summary>
        NewWorkspaceFactory NewWorkspaceFactory { get; set; }

        /// <summary>
        /// tab control 的 tab 拆分控制 key。从相同 key 的tab control 拆分出去的 tab 可以合并回至该 tab control
        /// </summary>
        string InterTabPartitionKey { get; set; }

        /// <summary>
        /// 是否嵌入到窗体头部
        /// </summary>
        bool IsEmbedInWindowCaption { get; set; }

        /// <summary>
        /// 当嵌入到窗体头部时，窗体头部数据的 holder
        /// </summary>
        ChromeWindowCaptionDataHolder EmbedInWindowCaptionDataHolder { get; set; }
        
        /// <summary>
        /// 内容视图
        /// </summary>
        object ContentView { get; }

        /// <summary>
        /// 工作空间的 TabControl 视图
        /// </summary>
        object ItemsTabControlView { get; }

        /// <summary>
        /// 当前工作空间数量
        /// </summary>
        int ItemCount { get; }

        void Initialize();

        void Run();

        void Shutdown();
    }
}
