using ContainerShell.Applications.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoWaf.Trade.Interfaces.Services;

namespace ContainerShell.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class ExceptionOrderPanelVM : ViewModel<IExceptionOrderPanelView>
    {
        private readonly IOrderItemsService orderItemsService;
        private readonly IXQTradeLameTaskNoteService tradeLameTaskNoteService;

        [ImportingConstructor]
        protected ExceptionOrderPanelVM(IExceptionOrderPanelView view,
            IOrderItemsService orderItemsService,
            IXQTradeLameTaskNoteService tradeLameTaskNoteService) : base(view)
        {
            this.orderItemsService = orderItemsService;
            this.tradeLameTaskNoteService = tradeLameTaskNoteService;

            WindowCaptionHeightHolder = new MessageWindowCaptionHeightHolder();

            var synchOrders1 = new SynchronizingCollection<OrderItemDataModel, OrderItemDataModel>(orderItemsService.OrderItems, i => i);
            ExceptionOrderCollectionView = CollectionViewSource.GetDefaultView(synchOrders1) as ListCollectionView;
            ConfigExceptionOrderCollectionView();
            
            var synchOrders2 = new SynchronizingCollection<OrderItemDataModel, OrderItemDataModel>(orderItemsService.OrderItems, i => i);
            AmbiguousOrderCollectionView = CollectionViewSource.GetDefaultView(synchOrders2) as ListCollectionView;
            ConfigAmbiguousOrderCollectionView();
            
            var syncTaskNotes1 = new SynchronizingCollection<XQTradeLameTaskNote, XQTradeLameTaskNote>(tradeLameTaskNoteService.TaskNoteItems, i => i);
            TradeLameTaskNoteCollectionView = CollectionViewSource.GetDefaultView(syncTaskNotes1) as ListCollectionView;
            ConfigTradeLameTaskNoteCollectionView();
        }

        public MessageWindowCaptionHeightHolder WindowCaptionHeightHolder { get; private set; }

        /// <summary>
        /// 关闭窗口处理
        /// </summary>
        private RoutedEventHandler closeMenuButtonClickHandler;
        public RoutedEventHandler CloseMenuButtonClickHandler
        {
            get { return closeMenuButtonClickHandler; }
            set { SetProperty(ref closeMenuButtonClickHandler, value); }
        }

        private SelectedOrdersOperateCommands selectedOrdersOptCommands;
        public SelectedOrdersOperateCommands SelectedOrdersOptCommands
        {
            get { return selectedOrdersOptCommands; }
            set { SetProperty(ref selectedOrdersOptCommands, value); }
        }

        private ICommand toShowOrderExecuteDetailCmd;
        public ICommand ToShowOrderExecuteDetailCmd
        {
            get { return toShowOrderExecuteDetailCmd; }
            set { SetProperty(ref toShowOrderExecuteDetailCmd, value); }
        }

        private SelectedTradeLameTNOperateCommands selectedTradeLameTNOptCommands;
        public SelectedTradeLameTNOperateCommands SelectedTradeLameTNOptCommands
        {
            get { return selectedTradeLameTNOptCommands; }
            set { SetProperty(ref selectedTradeLameTNOptCommands, value); }
        }

        /// <summary>
        /// 异常订单列表视图
        /// </summary>
        public ListCollectionView ExceptionOrderCollectionView { get; private set; }

        /// <summary>
        /// 不明确状态订单列表视图
        /// </summary>
        public ListCollectionView AmbiguousOrderCollectionView { get; private set; }

        /// <summary>
        /// 瘸腿成交任务项列表视图
        /// </summary>
        public ListCollectionView TradeLameTaskNoteCollectionView { get; private set; }

        /// <summary>
        /// 选中异常订单 tab
        /// </summary>
        public void SelectExceptionOrderTab()
        {
            ViewCore.SelectExceptionOrderTab();
        }

        /// <summary>
        /// 选中状态不明确订单 tab
        /// </summary>
        public void SelectAmbiguousOrderTab()
        {
            ViewCore.SelectAmbiguousOrderTab();
        }

        /// <summary>
        /// 选中瘸腿成交任务 tab
        /// </summary>
        public void SelectTradeLameTaskNoteTab()
        {
            ViewCore.SelectTradeLameTaskNoteTab();
        }

        /// <summary>
        /// 选中并使某个异常订单在列表上可见
        /// </summary>
        /// <param name="order"></param>
        public void SelectExceptionOrderItemAndBringIntoView(OrderItemDataModel order)
        {
            ViewCore.SelectExceptionOrderItemAndBringIntoView(order);
        }

        /// <summary>
        /// 选中并使某个不明确订单在列表上可见
        /// </summary>
        /// <param name="order"></param>
        public void SelectAmbiguousOrderItemAndBringIntoView(OrderItemDataModel order)
        {
            ViewCore.SelectAmbiguousOrderItemAndBringIntoView(order);
        }

        /// <summary>
        /// 选中并使某个成交瘸腿任务项在列表上可见
        /// </summary>
        /// <param name="lameTaskNote"></param>
        public void SelectTradeLameTaskNoteAndBringIntoView(XQTradeLameTaskNote lameTaskNote)
        {
            ViewCore.SelectTradeLameTaskNoteAndBringIntoView(lameTaskNote);
        }

        private void ConfigExceptionOrderCollectionView()
        {
            var collectionView = ExceptionOrderCollectionView;
            if (collectionView == null) return;

            // 设置默认排序
            var orderTimePropName = nameof(OrderItemDataModel.OrderTimestampMs);
            collectionView.SortDescriptions.Add(new SortDescription(orderTimePropName, ListSortDirection.Descending));
            collectionView.LiveSortingProperties.Add(orderTimePropName);
            collectionView.IsLiveSorting = true;

            collectionView.Filter = obj => 
            {
                var order = obj as OrderItemDataModel;
                if (order == null) return false;
                return order.IsSuspendedWithError;
            };

            collectionView.LiveFilteringProperties.Add(nameof(OrderItemDataModel.IsSuspendedWithError));
            collectionView.IsLiveFiltering = true;
        }

        private void ConfigAmbiguousOrderCollectionView()
        {
            var collectionView = AmbiguousOrderCollectionView;
            if (collectionView == null) return;

            // 设置默认排序
            var orderTimePropName = nameof(OrderItemDataModel.OrderTimestampMs);
            collectionView.SortDescriptions.Add(new SortDescription(orderTimePropName, ListSortDirection.Descending));
            collectionView.LiveSortingProperties.Add(orderTimePropName);
            collectionView.IsLiveSorting = true;

            collectionView.Filter = obj =>
            {
                var order = obj as OrderItemDataModel;
                if (order == null) return false;
                return order.IsStateAmbiguous;
            };

            collectionView.LiveFilteringProperties.Add(nameof(OrderItemDataModel.IsStateAmbiguous));
            collectionView.IsLiveFiltering = true;
        }

        private void ConfigTradeLameTaskNoteCollectionView()
        {
            var collectionView = TradeLameTaskNoteCollectionView;
            if (collectionView == null) return;

            // 设置默认排序
            var createTimePropName = nameof(XQTradeLameTaskNote.CreateTimestampMs);
            collectionView.SortDescriptions.Add(new SortDescription(createTimePropName, ListSortDirection.Descending));
            collectionView.LiveSortingProperties.Add(createTimePropName);
            collectionView.IsLiveSorting = true;
        }
    }
}
