using Dragablz;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows;
using System.Windows.Controls;
using XueQiaoWaf.Trade.Modules.Applications.ViewModels;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Presentation.Views
{
    /// <summary>
    /// TradeWorkspaceView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(ITradeWorkspaceView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class TradeWorkspaceView : ITradeWorkspaceView
    {
        private const string ComponentItemsContainerScrollViewerName = "ComponentItemsContainerScrollViewer";

        private readonly Lazy<TradeWorkspaceViewModel> vm;

        [ImportingConstructor]
        public TradeWorkspaceView()
        {
            InitializeComponent();
            this.vm = new Lazy<TradeWorkspaceViewModel>(() => ViewHelper.GetViewModel<TradeWorkspaceViewModel>(this));
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
