using Dragablz;
using Research.app.ViewModel;
using System;
using System.Collections.Generic;
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
using XueQiaoFoundation.Shared.Model;

namespace Research.app.View
{
    /// <summary>
    /// ResearchWorkspaceView.xaml 的交互逻辑
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class ResearchWorkspaceView : IDraggableComponentPanelViewBase, IView
    {
        private const string ComponentItemsContainerScrollViewerName = "ComponentItemsContainerScrollViewer";

        private readonly Lazy<ResearchWorkspaceVM> vm;

        [ImportingConstructor]
        public ResearchWorkspaceView()
        {
            InitializeComponent();
            this.vm = new Lazy<ResearchWorkspaceVM>(() => ViewHelper.GetViewModel<ResearchWorkspaceVM>(this));
        }

        public Rect DraggableComponentPanelPresentAreaRect
        {
            get
            {
                var rect = new Rect();
                var scrollViewer = this.ComponentDragablzItemsControl.Template?.FindName(ComponentItemsContainerScrollViewerName, this.ComponentDragablzItemsControl)
                    as ScrollViewer;
                if (scrollViewer != null)
                {
                    rect.Location = new Point(scrollViewer.HorizontalOffset, scrollViewer.VerticalOffset);
                }
                rect.Size = new Size(ComponentDragablzItemsControl.ActualWidth, ComponentDragablzItemsControl.ActualHeight);
                return rect;
            }
        }

        public void ComponentItemBringIntoView(object componentItem)
        {
            var tarDragItem = this.ComponentDragablzItemsControl.ItemContainerGenerator
                .ContainerFromItem(componentItem) as DragablzItem;
            if (tarDragItem != null)
            {
                tarDragItem.BringIntoView();
            }
        }

        private void MouseDownWithinComponent(object sender, DragablzItemEventArgs e)
        {
            DragablzItem componentItem = e.Source as DragablzItem;
            if (componentItem == null) componentItem = e.OriginalSource as DragablzItem;
            if (componentItem == null) componentItem = sender as DragablzItem;

            var optItemData = componentItem.DataContext;
            if (optItemData != null)
            {
                vm.Value?.DraggableComponentPanelContext.PublishMouseDownEventWithinComponent(optItemData);
            }
        }
    }
}
