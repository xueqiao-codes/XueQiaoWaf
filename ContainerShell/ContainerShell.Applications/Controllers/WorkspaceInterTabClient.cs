using ContainerShell.Interfaces.Applications;
using Dragablz;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using XueQiaoFoundation.BusinessResources.DataModels;

namespace ContainerShell.Applications.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class WorkspaceInterTabClient : IInterTabClient
    {
        private readonly ExportFactory<IWorkspaceInterTabWindowCtrl> interTabWindowCtrlFactory;
        private readonly List<IWorkspaceInterTabWindowCtrl> interTabWindowCtrls 
            = new List<IWorkspaceInterTabWindowCtrl>();

        [ImportingConstructor]
        public WorkspaceInterTabClient(ExportFactory<IWorkspaceInterTabWindowCtrl> interTabWindowCtrlFactory)
        {
            this.interTabWindowCtrlFactory = interTabWindowCtrlFactory;
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
        /// 在tab数量为空时，是否关闭
        /// </summary>
        public bool CloseWindowWhenTabEmptied { get; set; }

        /// <summary>
        /// 新建某个工作空间视图控制器的方法
        /// </summary>
        public WorkspaceItemViewCtrlFactory NewWorkspaceViewCtrlFactory { get; set; }

        /// <summary>
        /// 获取新的工作空间方法
        /// </summary>
        public NewWorkspaceFactory NewWorkspaceFactory { get; set; }

        /// <summary>
        /// tab control 的 tab 拆分控制 key。从相同 key 的tab control 拆分出去的 tab 可以合并回至该 tab control
        /// </summary>
        public string InterTabPartitionKey { get; set; }

        public void Shutdown()
        {
            foreach (var i in interTabWindowCtrls.ToArray())
            {
                i.Shutdown();
            }
            interTabWindowCtrls.Clear();
        }

        public INewTabHost<Window> GetNewHost(IInterTabClient interTabClient, object partition, TabablzControl source)
        {
            var sourceWindow = Window.GetWindow(source);
            var interTabWindowCtrl = interTabWindowCtrlFactory.CreateExport().Value;
            interTabWindowCtrl.InterTabWindowListContainer = this.InterTabWindowListContainer;
            
            interTabWindowCtrl.InterTabWindowContainer = new InterTabWorkspaceWindowContainer(new TabWorkspaceWindow
            {
                Left = sourceWindow.Left,
                Top = sourceWindow.Top,
                Width = sourceWindow.Width,
                Height = sourceWindow.Height,
                IsMaximized = false
            });

            interTabWindowCtrl.NewWorkspaceItemViewCtrlFactory = this.NewWorkspaceViewCtrlFactory;
            interTabWindowCtrl.NewWorkspaceFactory = this.NewWorkspaceFactory;
            interTabWindowCtrl.InterTabPartitionKey = this.InterTabPartitionKey;
            interTabWindowCtrl.ShowWindowWhenRun = false;
            interTabWindowCtrl.WindowCloseAction = ctrl => 
            {
                var closeCtrlItem = interTabWindowCtrls.FirstOrDefault(i => i == ctrl);
                if (closeCtrlItem != null)
                {
                    closeCtrlItem.Shutdown();
                    interTabWindowCtrls.Remove(closeCtrlItem);
                }
            };

            interTabWindowCtrls.Add(interTabWindowCtrl);

            interTabWindowCtrl.Initialize();
            interTabWindowCtrl.Run();
            
            return new NewTabHost<Window>(interTabWindowCtrl.WindowElement as Window,
                interTabWindowCtrl.WorkspaceTabControl as TabablzControl);
        }

        public TabEmptiedResponse TabEmptiedHandler(TabablzControl tabControl, Window window)
        {
            if (CloseWindowWhenTabEmptied)
            {
                return TabEmptiedResponse.CloseWindowOrLayoutBranch;
            }
            return TabEmptiedResponse.DoNothing;
        }
    }
}
