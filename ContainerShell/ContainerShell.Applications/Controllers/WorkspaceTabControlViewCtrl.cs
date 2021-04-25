using ContainerShell.Applications.ViewModels;
using ContainerShell.Interfaces.Applications;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Model;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;

namespace ContainerShell.Applications.Controllers
{
    /// <summary>
    /// 工作空间 tab control 页面控制器
    /// </summary>
    [Export, Export(typeof(IWorkspaceTabControlViewCtrl)), PartCreationPolicy(CreationPolicy.NonShared)]
    internal class WorkspaceTabControlViewCtrl : IWorkspaceTabControlViewCtrl
    {
        private readonly WorkspaceTabablzControlVM contentVM;
        private readonly WorkspaceInterTabClient workspaceInterTabClient;
        private readonly IMessageWindowService messageWindowService;
        private readonly ExportFactory<IWorkspaceInterTabWindowCtrl> workspaceInterTabWindowCtrlFactory;
        private readonly ExportFactory<WorkspaceEditDialogController> workspaceEditDialogCtrlFactory;

        private readonly DelegateCommand newCommand;
        private readonly DelegateCommand splitWorkspaceAsWindowCmd;
        private readonly DelegateCommand renameWorkspaceCmd;
        private readonly Dictionary<TabWorkspaceItemDataModel, IWorkspaceItemViewCtrl> workspaceItemCtrls 
            = new Dictionary<TabWorkspaceItemDataModel, IWorkspaceItemViewCtrl>();
        private readonly List<IWorkspaceInterTabWindowCtrl> workspaceInterTabWindowCtrls 
            = new List<IWorkspaceInterTabWindowCtrl>();

        [ImportingConstructor]
        public WorkspaceTabControlViewCtrl(
            WorkspaceTabablzControlVM contentVM,
            WorkspaceInterTabClient workspaceInterTabClient,
            IMessageWindowService messageWindowService,
            ExportFactory<IWorkspaceInterTabWindowCtrl> workspaceInterTabWindowCtrlFactory,
            ExportFactory<WorkspaceEditDialogController> workspaceEditDialogCtrlFactory)
        {
            this.contentVM = contentVM;
            this.workspaceInterTabClient = workspaceInterTabClient;
            this.messageWindowService = messageWindowService;
            this.workspaceInterTabWindowCtrlFactory = workspaceInterTabWindowCtrlFactory;
            this.workspaceEditDialogCtrlFactory = workspaceEditDialogCtrlFactory;

            newCommand = new DelegateCommand(NewWorkspace);
            splitWorkspaceAsWindowCmd = new DelegateCommand(SplitWorkspaceAsWindow, CanSplitWorkspaceAsWindow);
            renameWorkspaceCmd = new DelegateCommand(RenameWorkspace, CanRenameWorkspace);
        }

        /// <summary>
        /// 分拆窗口列表容器，记录分拆工作空间窗口的增删变化
        /// </summary>
        public InterTabWorkspaceWindowListContainer InterTabWindowListContainer { get; set; }

        /// <summary>
        /// 当前工作空间列表容器，能记录该 tabControl 下的工作空间的增删改变化
        /// </summary>
        public TabWorkspaceListContainer WorkspaceListContainer { get; set; }
        
        /// <summary>
        /// 固定工作空间的数目
        /// </summary>
        public int FixedItemsCount { get; set; }

        /// <summary>
        /// 当工作空间数目为空时,是否关闭所在窗口
        /// </summary>
        public bool CloseWindowWhenItemsEmptied { get; set; }

        /// <summary>
        /// 新建某个工作空间视图控制器的方法
        /// </summary>
        public WorkspaceItemViewCtrlFactory NewItemViewCtrlFactory { get; set; }

        /// <summary>
        /// 获取新的工作空间方法
        /// </summary>
        public NewWorkspaceFactory NewWorkspaceFactory { get; set; }

        /// <summary>
        /// tab control 的 tab 拆分控制 key。从相同 key 的tab control 拆分出去的 tab 可以合并回至该 tab control
        /// </summary>
        public string InterTabPartitionKey { get; set; }
        
        /// <summary>
        /// 是否嵌入到窗体头部
        /// </summary>
        public bool IsEmbedInWindowCaption { get; set; }

        /// <summary>
        /// 当嵌入到窗体头部时，窗体头部数据的 holder
        /// </summary>
        public ChromeWindowCaptionDataHolder EmbedInWindowCaptionDataHolder { get; set; }

        /// <summary>
        /// 内容视图
        /// </summary>
        public object ContentView => contentVM.View;

        /// <summary>
        /// 工作空间的 TabControl 视图
        /// </summary>
        public object ItemsTabControlView => contentVM.WorkspaceTabControl;

        /// <summary>
        /// 工作空间数量
        /// </summary>
        public int ItemCount => contentVM.WorkspaceItems.Count;
        
        public void Initialize()
        {
            if (InterTabWindowListContainer == null) throw new ArgumentNullException("`InterTabWindowListContainer` must be set before call `Initialize` function.");
            if (WorkspaceListContainer == null) throw new ArgumentNullException("`WorkspaceListContainer` must be set before call `Initialize` function.");
            
            this.workspaceInterTabClient.InterTabWindowListContainer = this.InterTabWindowListContainer;
            this.workspaceInterTabClient.WorkspaceListContainer = this.WorkspaceListContainer;
            this.workspaceInterTabClient.CloseWindowWhenTabEmptied = this.CloseWindowWhenItemsEmptied;
            this.workspaceInterTabClient.NewWorkspaceViewCtrlFactory = this.NewItemViewCtrlFactory;
            this.workspaceInterTabClient.NewWorkspaceFactory = this.NewWorkspaceFactory;
            this.workspaceInterTabClient.InterTabPartitionKey = this.InterTabPartitionKey;

            contentVM.IsEmbedInWindowCaption = this.IsEmbedInWindowCaption;
            contentVM.EmbedInWindowCaptionDataHolder = this.EmbedInWindowCaptionDataHolder;
            contentVM.InterTabClient = this.workspaceInterTabClient;
            contentVM.InterTabPartitionKey = this.InterTabPartitionKey;
            contentVM.NewCommand = newCommand;
            contentVM.SplitWorkspaceAsWindowCmd = splitWorkspaceAsWindowCmd;
            contentVM.RenameWorkspaceCmd = renameWorkspaceCmd;
            contentVM.FixedItemCount = this.FixedItemsCount;
            
            // 根据 order 先排序
            // 初始化在列表中的工作空间
            var waitingAddWorkspaceDMs = WorkspaceListContainer.Workspaces
                    .OrderBy(i=>i.Order)
                    .Select(_w => new TabWorkspaceItemDataModel(_w))
                    .ToArray();
            if (waitingAddWorkspaceDMs?.Any() == true)
            {
                AddAndInitializeWorkspaceItemControllersIfNeed(waitingAddWorkspaceDMs);
                var selectedWorkspaceItem = waitingAddWorkspaceDMs.FirstOrDefault(i => i.TabWorkspace.IsSelected);
                if (selectedWorkspaceItem == null)
                {
                    selectedWorkspaceItem = waitingAddWorkspaceDMs.FirstOrDefault();
                    selectedWorkspaceItem.TabWorkspace.IsSelected = true;
                }
                // 选中项目
                contentVM.ActiveWorkspaceItem = selectedWorkspaceItem;
                RunWorkspaceItemControllerWhenActiveItem(selectedWorkspaceItem);
            }

            contentVM.WorkspaceItemClosing += WorkspaceTabablzControlViewModel_WorkspaceItemClosing;

            // DragTriggerWorkspaceCollection_Changed 只处理被动（如拖拽引发的）变化，主动（如主动点击添加、关闭按钮引起的）变化都要求主动处理
            CollectionChangedEventManager.AddHandler(contentVM.WorkspaceItems, DragTriggerWorkspaceCollection_Changed);
            PropertyChangedEventManager.AddHandler(contentVM, ContentVM_PropertyChanged, "");
        }
        
        public void Run()
        {
            
        }
        
        public void Shutdown()
        {
            contentVM.WorkspaceItemClosing -= WorkspaceTabablzControlViewModel_WorkspaceItemClosing;
            CollectionChangedEventManager.RemoveHandler(contentVM.WorkspaceItems, DragTriggerWorkspaceCollection_Changed);
            PropertyChangedEventManager.RemoveHandler(contentVM, ContentVM_PropertyChanged, "");

            workspaceInterTabClient.Shutdown();

            foreach (var i in workspaceItemCtrls.ToArray())
            {
                i.Value.Shutdown();
            }
            workspaceItemCtrls.Clear();

            foreach (var i in workspaceInterTabWindowCtrls.ToArray())
            {
                i.Shutdown();
            }
            workspaceInterTabWindowCtrls.Clear();
        }

        /// <summary>
        /// 这里只处理被动（如拖拽引发的）变化，主动（如主动点击添加、关闭按钮引起的）变化不在此处理
        /// </summary>
        private void DragTriggerWorkspaceCollection_Changed(object sender, NotifyCollectionChangedEventArgs e)
        {
            IEnumerable<TabWorkspaceItemDataModel> newItems = null;
            IEnumerable<TabWorkspaceItemDataModel> oldItems = null;

            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                oldItems = workspaceItemCtrls.Keys.ToArray();
            }
            else
            {
                newItems = e.NewItems?.Cast<TabWorkspaceItemDataModel>();
                oldItems = e.OldItems?.Cast<TabWorkspaceItemDataModel>();
            }

            if (newItems?.Any() == true)
            {
                foreach (var waitingAddItem in newItems)
                {
                    var reusedController = waitingAddItem.CachedWorkspaceController;
                    if (reusedController == null) continue;

                    var workspace = waitingAddItem.TabWorkspace;
                    var workspaceType = waitingAddItem.TabWorkspace.WorkspaceType;
                    
                    var itemController = reusedController as IWorkspaceItemViewCtrl;
                    if (itemController == null) continue;

                    workspaceItemCtrls.Remove(waitingAddItem);
                    workspaceItemCtrls[waitingAddItem] = itemController;

                    if (waitingAddItem.DisplayView != itemController.ContentView)
                    {
                        waitingAddItem.DisplayView = itemController.ContentView;
                    }

                    // 如果不存在于 WorkspaceListContainer 的工作空间列表中，则添加
                    if (!WorkspaceListContainer.Workspaces.Contains(workspace))
                    {
                        WorkspaceListContainer.Workspaces.Add(workspace);
                    }
                }
            }

            if (oldItems?.Any() == true)
            {
                // 拖拉时不 shutdown controller，让 controller 复用
                foreach (var waitingRemoveItem in oldItems)
                {
                    var workspace = waitingRemoveItem.TabWorkspace;

                    workspaceItemCtrls.Remove(waitingRemoveItem);
                    WorkspaceListContainer.Workspaces.Remove(workspace);
                }
            }
        }

        private void ContentVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender != contentVM) return;
            if (e.PropertyName == nameof(WorkspaceTabablzControlVM.ActiveWorkspaceItem))
            {
                var activeItem = contentVM.ActiveWorkspaceItem;
                if (activeItem != null)
                {
                    foreach (var i in WorkspaceListContainer.Workspaces)
                    {
                        i.IsSelected = (activeItem.TabWorkspace == i);
                    }
                    // 选中工作空间后，再 run
                    RunWorkspaceItemControllerWhenActiveItem(activeItem);
                }
            }
        }

        private void RunWorkspaceItemControllerWhenActiveItem(TabWorkspaceItemDataModel activeItem)
        {
            // 选中工作空间后，再 run
            IWorkspaceItemViewCtrl controller = null;
            if (workspaceItemCtrls.TryGetValue(activeItem, out controller))
            {
                controller.Run();
            }
        }

        private void WorkspaceTabablzControlViewModel_WorkspaceItemClosing(TabWorkspaceItemDataModel closingWorkspaceItem, CancelEventArgs eventArgs)
        {
            if (closingWorkspaceItem == null)
            {
                return;
            }
            var displayInWindow = Window.GetWindow(closingWorkspaceItem.DisplayView as DependencyObject);
            var dialogLocation = WorkspaceItemAppropriateShowDialogLocation(closingWorkspaceItem);
            var result = messageWindowService.ShowQuestionDialog(displayInWindow, dialogLocation, null,"提示", "要关闭该工作空间吗？",false,"关闭","取消");
            if (result != true)
            {
                // 取消操作，不进行任何处理
                eventArgs.Cancel = true;
                return;
            }

            eventArgs.Cancel = false;
            // 关闭页面
            RemoveAndShutdownWorkspaceItemControllersIfNeed(new TabWorkspaceItemDataModel[] { closingWorkspaceItem });
        }
        
        private void NewWorkspace()
        {
            var newWorkspace = NewWorkspaceFactory?.Invoke(this);
            if (newWorkspace == null) return;
            
            newWorkspace.Order = contentVM.WorkspaceItems.Max(i => i.TabWorkspace.Order) + 1;
            newWorkspace.IsSelected = true;

            var newItem = new TabWorkspaceItemDataModel(newWorkspace);
            AddAndInitializeWorkspaceItemControllersIfNeed(new TabWorkspaceItemDataModel[] { newItem });

            // 选中当前新建的项目
            contentVM.ActiveWorkspaceItem = newItem;
            RunWorkspaceItemControllerWhenActiveItem(newItem);
        }

        private void SplitWorkspaceAsWindow(object obj)
        {
            if (CanSplitWorkspaceAsWindow(obj) && (obj is TabWorkspaceItemDataModel wsItem))
            {
                // shut down and remove related workspace controller
                if (workspaceItemCtrls.TryGetValue(wsItem, out IWorkspaceItemViewCtrl cachedCtrl))
                {
                    cachedCtrl.Shutdown();
                    workspaceItemCtrls.Remove(wsItem);
                }
                // 先解除列表变化观察，再删除，防止方法循环调用
                CollectionChangedEventManager.RemoveHandler(contentVM.WorkspaceItems, DragTriggerWorkspaceCollection_Changed);
                contentVM.WorkspaceItems.Remove(wsItem);
                WorkspaceListContainer.Workspaces.Remove(wsItem.TabWorkspace);
                CollectionChangedEventManager.AddHandler(contentVM.WorkspaceItems, DragTriggerWorkspaceCollection_Changed);

                // run new workspace inter tab window
                var interTabWindowCtrl = workspaceInterTabWindowCtrlFactory.CreateExport().Value;
                interTabWindowCtrl.InterTabWindowListContainer = this.InterTabWindowListContainer;
                var sourceWindow = this.contentVM.ContainerWindow as Window;
                interTabWindowCtrl.InterTabWindowContainer = new InterTabWorkspaceWindowContainer(new TabWorkspaceWindow
                {
                    // set (20, 20) offset to source window
                    Left = sourceWindow.Left + 40,
                    Top = sourceWindow.Top + 40,
                    Width = sourceWindow.Width,
                    Height = sourceWindow.Height,
                    IsMaximized = false
                });
                interTabWindowCtrl.InterTabWindowContainer.WorkspaceListContainer.Workspaces.Add(wsItem.TabWorkspace);
                interTabWindowCtrl.NewWorkspaceItemViewCtrlFactory = this.NewItemViewCtrlFactory;
                interTabWindowCtrl.NewWorkspaceFactory = this.NewWorkspaceFactory;
                interTabWindowCtrl.InterTabPartitionKey = this.InterTabPartitionKey;
                interTabWindowCtrl.ShowWindowWhenRun = true;
                interTabWindowCtrl.WindowCloseAction = ctrl =>
                {
                    ctrl.Shutdown();
                    workspaceInterTabWindowCtrls.Remove(ctrl);
                };

                workspaceInterTabWindowCtrls.Add(interTabWindowCtrl);

                interTabWindowCtrl.Initialize();
                interTabWindowCtrl.Run();
            }
        }

        private bool CanSplitWorkspaceAsWindow(object obj)
        {
            if ((obj is TabWorkspaceItemDataModel wsItem))
            {
                var itemsCount = contentVM.WorkspaceItems.Count;
                if (itemsCount <= 1) return false;

                var ascOrderedItems = contentVM.WorkspaceItems.ToArray().OrderBy(i => i.TabWorkspace.Order).ToList();
                var wsOrderIdx = ascOrderedItems.IndexOf(wsItem);
                if (wsOrderIdx >= 0 && wsOrderIdx >= this.FixedItemsCount)
                    return true;
            }
            return false;
        }

        private void RenameWorkspace(object obj)
        {
            if (CanRenameWorkspace(obj) && (obj is TabWorkspaceItemDataModel spaceItem))
            {
                // 弹出重命名窗口
                var dialogCtrl = workspaceEditDialogCtrlFactory.CreateExport().Value;
                dialogCtrl.DialogOwner = this.contentVM.ContainerWindow;
                dialogCtrl.DialogShowLocationRelativeToScreen = WorkspaceItemAppropriateShowDialogLocation(spaceItem);
                dialogCtrl.InitialEditWorkspace = spaceItem.TabWorkspace;

                dialogCtrl.Initialize();
                dialogCtrl.Run();
                dialogCtrl.Shutdown();
            }
        }

        private bool CanRenameWorkspace(object obj)
        {
            return true;
        }

        private void AddAndInitializeWorkspaceItemControllersIfNeed(IEnumerable<TabWorkspaceItemDataModel> waitingAddItems)
        {
            if (waitingAddItems?.Any() != true) return;

            // 先解除列表变化观察，再添加，防止方法循环调用
            CollectionChangedEventManager.RemoveHandler(contentVM.WorkspaceItems, DragTriggerWorkspaceCollection_Changed);
            foreach (var waitingAddItem in waitingAddItems)
            {
                var workspace = waitingAddItem.TabWorkspace;
                IWorkspaceItemViewCtrl ctrl = null;
                if (!workspaceItemCtrls.TryGetValue(waitingAddItem, out ctrl))
                {
                    ctrl = NewItemViewCtrlFactory?.Invoke(this, workspace);
                    if (ctrl != null)
                    {
                        ctrl.Initialize();

                        waitingAddItem.DisplayView = ctrl.ContentView;
                        waitingAddItem.CachedWorkspaceController = ctrl;
                        workspaceItemCtrls[waitingAddItem] = ctrl;
                    }
                }

                // 如果不存在于 WorkspaceItems 中，则添加进去
                if (!contentVM.WorkspaceItems.Contains(waitingAddItem))
                {
                    contentVM.WorkspaceItems.Add(waitingAddItem);
                }

                // 如果不存在于 CurrentWorkspaceListContainer 的工作空间列表中，则添加
                if (!WorkspaceListContainer.Workspaces.Contains(workspace))
                {
                    WorkspaceListContainer.Workspaces.Add(workspace);
                }
            }
            CollectionChangedEventManager.AddHandler(contentVM.WorkspaceItems, DragTriggerWorkspaceCollection_Changed);
        }

        private void RemoveAndShutdownWorkspaceItemControllersIfNeed(IEnumerable<TabWorkspaceItemDataModel> waitingRemoveItems)
        {
            if (waitingRemoveItems?.Any() != true) return;

            // 先解除列表变化观察，再删除，防止方法循环调用
            CollectionChangedEventManager.RemoveHandler(contentVM.WorkspaceItems, DragTriggerWorkspaceCollection_Changed);
            foreach (var waitingRemoveItem in waitingRemoveItems)
            {
                var workspace = waitingRemoveItem.TabWorkspace;

                IWorkspaceItemViewCtrl ctrl = null;
                if (workspaceItemCtrls.TryGetValue(waitingRemoveItem, out ctrl))
                {
                    workspaceItemCtrls.Remove(waitingRemoveItem);
                    ctrl.Shutdown();
                }

                // 如果存在于 contentVM.WorkspaceItems 中，则删除
                if (contentVM.WorkspaceItems.Contains(waitingRemoveItem))
                {
                    contentVM.WorkspaceItems.Remove(waitingRemoveItem);
                }

                WorkspaceListContainer.Workspaces.Remove(workspace);
            }
            CollectionChangedEventManager.AddHandler(contentVM.WorkspaceItems, DragTriggerWorkspaceCollection_Changed);
        }

        private Point? WorkspaceItemAppropriateShowDialogLocation(TabWorkspaceItemDataModel workspaceItemData)
        {
            var itemUIElement = this.contentVM.WorkspaceItemElement(workspaceItemData);
            if (itemUIElement == null) return null;
            var itemUIElementSize = itemUIElement.RenderSize;
            var screenPoint = itemUIElement.PointToScreen(new Point(itemUIElementSize.Width / 2, itemUIElementSize.Height / 2));
            var location = UIHelper.TransformToWpfPoint(screenPoint, itemUIElement);
            return location;
        }
    }
}
