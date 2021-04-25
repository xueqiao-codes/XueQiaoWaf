using ContainerShell.Applications.ViewModels;
using ContainerShell.Applications.Views;
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

namespace ContainerShell.Presentations.Views
{
    /// <summary>
    /// ExceptionOrderPanelView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IExceptionOrderPanelView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class ExceptionOrderPanelView : IExceptionOrderPanelView
    {
        private readonly Lazy<ExceptionOrderPanelVM> vm;

        public ExceptionOrderPanelView()
        {
            InitializeComponent();
            this.vm = new Lazy<ExceptionOrderPanelVM>(() => ViewHelper.GetViewModel<ExceptionOrderPanelVM>(this));
        }

        public void SelectExceptionOrderTab()
        {
            this.ExceptionOrderTabItem.IsSelected = true;
        }

        public void SelectAmbiguousOrderTab()
        {
            this.AmbiguousOrderTabItem.IsSelected = true;
        }

        public void SelectTradeLameTaskNoteTab()
        {
            this.TradeLameTaskNoteTabItem.IsSelected = true;
        }

        public void SelectExceptionOrderItemAndBringIntoView(object orderItem)
        {
            if (orderItem == null) return;
            var dataGrid = this.ExceptionOrdersOptView?.OrdersDataGrid;
            if (dataGrid != null)
            {
                var originIsSynchronizedWithCurrentItem = dataGrid.IsSynchronizedWithCurrentItem;
                
                // 设置 IsSynchronizedWithCurrentItem 为 true。才能使用 ItemContainerGenerator，才能使用 CollectionView 的选中方法
                dataGrid.IsSynchronizedWithCurrentItem = true;

                // 选中目标项
                var collectionView = vm?.Value.ExceptionOrderCollectionView;
                if (collectionView != null)
                {
                    collectionView.MoveCurrentTo(orderItem);
                }

                // 移动到选中项
                var row = dataGrid.ItemContainerGenerator.ContainerFromItem(orderItem) as DataGridRow;
                row?.BringIntoView();

                // 恢复为原来的 IsSynchronizedWithCurrentItem
                dataGrid.IsSynchronizedWithCurrentItem = originIsSynchronizedWithCurrentItem;
            }
        }

        public void SelectAmbiguousOrderItemAndBringIntoView(object orderItem)
        {
            if (orderItem == null) return;
            var dataGrid = this.AmbiguousStateOrdersOptView?.OrdersDataGrid;
            if (dataGrid != null)
            {
                var originIsSynchronizedWithCurrentItem = dataGrid.IsSynchronizedWithCurrentItem;

                // 设置 IsSynchronizedWithCurrentItem 为 true。才能使用 ItemContainerGenerator，才能使用 CollectionView 的选中方法
                dataGrid.IsSynchronizedWithCurrentItem = true;

                // 选中目标项
                var collectionView = vm?.Value.AmbiguousOrderCollectionView;
                if (collectionView != null)
                {
                    collectionView.MoveCurrentTo(orderItem);
                }

                // 移动到选中项
                var row = dataGrid.ItemContainerGenerator.ContainerFromItem(orderItem) as DataGridRow;
                row?.BringIntoView();

                // 恢复为原来的 IsSynchronizedWithCurrentItem
                dataGrid.IsSynchronizedWithCurrentItem = originIsSynchronizedWithCurrentItem;
            }
        }

        public void SelectTradeLameTaskNoteAndBringIntoView(object lameTaskNote)
        {
            if (lameTaskNote == null) return;
            var dataGrid = this.TradeLameTaskNoteOptView?.ItemsDataGrid;
            if (dataGrid != null)
            {
                var originIsSynchronizedWithCurrentItem = dataGrid.IsSynchronizedWithCurrentItem;

                // 设置 IsSynchronizedWithCurrentItem 为 true。才能使用 ItemContainerGenerator，才能使用 CollectionView 的选中方法
                dataGrid.IsSynchronizedWithCurrentItem = true;

                // 选中目标项
                var collectionView = vm?.Value.TradeLameTaskNoteCollectionView;
                if (collectionView != null)
                {
                    collectionView.MoveCurrentTo(lameTaskNote);
                }

                // 移动到选中项
                var row = dataGrid.ItemContainerGenerator.ContainerFromItem(lameTaskNote) as DataGridRow;
                row?.BringIntoView();

                // 恢复为原来的 IsSynchronizedWithCurrentItem
                dataGrid.IsSynchronizedWithCurrentItem = originIsSynchronizedWithCurrentItem;
            }
        }
    }
}
