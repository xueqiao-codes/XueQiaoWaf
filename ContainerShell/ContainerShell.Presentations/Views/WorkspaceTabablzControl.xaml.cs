using ContainerShell.Applications.ViewModels;
using ContainerShell.Applications.Views;
using Dragablz;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ContainerShell.Presentations.Views
{
    /// <summary>
    /// WorkspaceTabablzControl.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IWorkspaceTabablzControl)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class WorkspaceTabablzControl : IWorkspaceTabablzControl
    {
        private readonly Lazy<WorkspaceTabablzControlVM> workspaceTabablzControlVM;

        public WorkspaceTabablzControl()
        {
            InitializeComponent();
            workspaceTabablzControlVM = new Lazy<WorkspaceTabablzControlVM>(() => ViewHelper.GetViewModel<WorkspaceTabablzControlVM>(this));
        }

        public object WorkspaceTabControl => this.TabablzControlSelf;

        public object ContainerWindow => Window.GetWindow(this);

        public UIElement WorkspaceItemElement(object workspaceItemData)
        {
            var dragItems = this.TabablzControlSelf?.GetOrderedHeaders();
            var tarDragItem = dragItems?.FirstOrDefault(i=>i.DataContext == workspaceItemData);
            return tarDragItem;
        }

        public ItemActionCallback WorkspaceItemClosing
        {
            get
            {
                return WorkspaceTabablzControl_ItemClosing;
            }
        }

        private void WorkspaceTabablzControl_ItemClosing(ItemActionCallbackArgs<TabablzControl> e)
        {
            var cancelEvtArg = new CancelEventArgs();
            var optItemData = e.DragablzItem.DataContext;
            if (optItemData == null) return;

            workspaceTabablzControlVM.Value.PublishWorkspaceItemClosing(optItemData,
                cancelEvtArg);
            if (cancelEvtArg.Cancel)
            {
                e.Cancel();
            }
        }
        
        private void WorkspaceTabItem_LogicalIndexChanged(object sender, RoutedPropertyChangedEventArgs<int> e)
        {
            DragablzItem optItem = e.Source as DragablzItem;
            if (optItem == null) optItem = e.OriginalSource as DragablzItem;
            if (optItem == null) return;

            var optItemData = optItem.DataContext;
            if (optItemData == null) return;

            workspaceTabablzControlVM.Value.PublishWorkspaceItemLogicalIndexChanged(optItemData,
                e.NewValue, e.OldValue);
        }
    }
}
