using ContainerShell.Applications.ViewModels;
using ContainerShell.Interfaces.Applications;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.Shared.Model;

namespace ContainerShell.Applications.Controllers
{
    /// <summary>
    /// 工作空间拆分窗口 controller
    /// </summary>
    [Export, Export(typeof(IWorkspaceInterTabWindowCtrl)), PartCreationPolicy(CreationPolicy.NonShared)]
    internal class WorkspaceInterTabWindowCtrl : IWorkspaceInterTabWindowCtrl
    {
        private readonly IWorkspaceTabControlViewCtrl workspaceTabControlViewCtrl;
        private readonly WorkspaceInterTabWindowVM windowViewModel;
        private readonly IEventAggregator eventAggregator;

        private readonly ChromeWindowCaptionDataHolder windowCaptionDataHolder = new ChromeWindowCaptionDataHolder();

        [ImportingConstructor]
        public WorkspaceInterTabWindowCtrl(
            IWorkspaceTabControlViewCtrl workspaceTabControlViewCtrl,
            WorkspaceInterTabWindowVM windowViewModel,
            IEventAggregator eventAggregator)
        {
            this.workspaceTabControlViewCtrl = workspaceTabControlViewCtrl;
            this.windowViewModel = windowViewModel;
            this.eventAggregator = eventAggregator;

            this.windowViewModel.Loaded += WorkspaceInterTabWindowViewModel_Loaded;
            this.windowViewModel.Closed += WorkspaceInterTabWindowViewModel_Closed;
        }

        /// <summary>
        /// 当前分拆窗口容器所在的分拆窗口列表容器
        /// </summary>
        public InterTabWorkspaceWindowListContainer InterTabWindowListContainer { get; set; }

        /// <summary>
        /// 当前分拆窗口容器
        /// </summary>
        public InterTabWorkspaceWindowContainer InterTabWindowContainer { get; set; }

        /// <summary>
        /// 新建某个工作空间视图控制器的方法
        /// </summary>
        public WorkspaceItemViewCtrlFactory NewWorkspaceItemViewCtrlFactory { get; set; }

        /// <summary>
        /// 获取新的工作空间方法
        /// </summary>
        public NewWorkspaceFactory NewWorkspaceFactory { get; set; }

        /// <summary>
        /// tab control 的 tab 拆分控制 key。从相同 key 的tab control 拆分出去的 tab 可以合并回至该 tab control
        /// </summary>
        public string InterTabPartitionKey { get; set; }

        /// <summary>
        /// 是否在 run 时显示窗口，否则不显示
        /// </summary>
        public bool ShowWindowWhenRun { get; set; } = true;

        /// <summary>
        /// 窗口关闭回调
        /// </summary>
        public Action<IWorkspaceInterTabWindowCtrl> WindowCloseAction { get; set; }

        /// <summary>
        /// 窗口UI
        /// </summary>
        public object WindowElement => windowViewModel.ConainerWindow;

        /// <summary>
        /// 显示工作空间的 tab control UI元素
        /// </summary>
        public object WorkspaceTabControl => workspaceTabControlViewCtrl.ItemsTabControlView;
        
        public void Initialize()
        {
            if (InterTabWindowListContainer == null) throw new ArgumentNullException("`InterTabWindowListContainer` must be init before call `Initialize` function.");
            if (InterTabWindowContainer == null) throw new ArgumentNullException("`InterTabWindowContainer` must be init before call `Initialize` function.");
            
            this.windowCaptionDataHolder.CloseWindowMenuButtonClickHandler = this.WindowMenuCloseButtonManualClicked;

            workspaceTabControlViewCtrl.InterTabWindowListContainer = this.InterTabWindowListContainer;
            workspaceTabControlViewCtrl.WorkspaceListContainer = this.InterTabWindowContainer.WorkspaceListContainer;
            workspaceTabControlViewCtrl.FixedItemsCount = 0;
            workspaceTabControlViewCtrl.CloseWindowWhenItemsEmptied = true;
            workspaceTabControlViewCtrl.NewItemViewCtrlFactory = this.NewWorkspaceItemViewCtrlFactory;
            workspaceTabControlViewCtrl.NewWorkspaceFactory = this.NewWorkspaceFactory;
            workspaceTabControlViewCtrl.InterTabPartitionKey = this.InterTabPartitionKey;
            workspaceTabControlViewCtrl.IsEmbedInWindowCaption = true;
            workspaceTabControlViewCtrl.EmbedInWindowCaptionDataHolder = this.windowCaptionDataHolder;

            workspaceTabControlViewCtrl.Initialize();
            workspaceTabControlViewCtrl.Run();

            windowViewModel.WorkspaceWindow = InterTabWindowContainer.WindowInfo;
            windowViewModel.WorkspaceTabControlView = workspaceTabControlViewCtrl.ContentView;
            windowViewModel.WindowCaptionDataHolder = this.windowCaptionDataHolder;
        }

        public void Run()
        {
            if (ShowWindowWhenRun)
            {
                windowViewModel.ShowWindow();
            }
        }

        public void Shutdown()
        {
            this.windowCaptionDataHolder.CloseWindowMenuButtonClickHandler = null;
            WindowCloseAction = null;
            windowViewModel.Loaded -= WorkspaceInterTabWindowViewModel_Loaded;
            windowViewModel.Closed -= WorkspaceInterTabWindowViewModel_Closed;
            windowViewModel.CloseWindow(true);
            workspaceTabControlViewCtrl.Shutdown();
        }

        private void WorkspaceInterTabWindowViewModel_Loaded(object sender, RoutedEventArgs e)
        {
            var intertabWindows = this.InterTabWindowListContainer.Windows;
            if (!intertabWindows.Contains(InterTabWindowContainer))
            {
                intertabWindows.Add(InterTabWindowContainer);
            }
        }

        private void WorkspaceInterTabWindowViewModel_Closed(object sender, EventArgs e)
        {
            var interTabWindowWorkspaces = this.InterTabWindowContainer.WorkspaceListContainer?.Workspaces;
            if (interTabWindowWorkspaces != null && interTabWindowWorkspaces.Count == 0)
            {
                this.InterTabWindowListContainer.Windows.Remove(this.InterTabWindowContainer);
            }
            WindowCloseAction?.Invoke(this);
        }

        private void WindowMenuCloseButtonManualClicked(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            this.InterTabWindowListContainer.Windows.Remove(this.InterTabWindowContainer);
            WindowCloseAction?.Invoke(this);
        }
    }
}
