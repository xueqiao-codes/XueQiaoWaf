using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Data;
using XueQiaoFoundation.BusinessResources.Constants;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Services;
using XueQiaoWaf.Trade.Modules.Applications.Views;
using NativeModel.Trade;
using System.Collections.Specialized;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.ServiceControllers;
using System.Collections.ObjectModel;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class PlaceOrderHangingOrdersAreaVM : ViewModel<IPlaceOrderHangingOrdersAreaView>
    {
        private readonly XqTargetPresentOrderListCtrl xqTargetPresentOrderListCtrl;
        private readonly SynchronizingCollection<TradeItemDataModel, TradeItemDataModel> synchonizingTradeItems;
        private PlaceOrderComponentPresentKey currentPresentKey;

        [ImportingConstructor]
        protected PlaceOrderHangingOrdersAreaVM(IPlaceOrderHangingOrdersAreaView view,
            OrderItemsService orderItemsService,
            TradeItemsService tradeItemsService,
            XqTargetPresentOrderListCtrl xqTargetPresentOrderListCtrl) : base(view)
        {
            this.xqTargetPresentOrderListCtrl = xqTargetPresentOrderListCtrl;

            CollectionChangedEventManager.AddHandler(orderItemsService.OrderItems, OrderItemServiceOrderCollectionChanged);

            synchonizingTradeItems = new SynchronizingCollection<TradeItemDataModel, TradeItemDataModel>(tradeItemsService.TradeItems, i => i);
            CurrentItemTradeListView = CollectionViewSource.GetDefaultView(synchonizingTradeItems) as ListCollectionView;

            ConfigCurrentItemTradeListView();
            RefreshCurrentItemTradeListView();
        }

        private bool CanPassHangingOrderFilter(OrderItemDataModel order)
        {
            if (order == null) return false;

            var subAccountId = this.currentPresentKey?.SubAccountId;
            var targetType = this.currentPresentKey?.TargetType;
            var targetKey = this.currentPresentKey?.TargetKey;

            if (order.SubAccountFields.SubAccountId != subAccountId) return false;
            if (order.TargetType != targetType) return false;
            if (order.TargetKey != targetKey) return false;

            // 只显示委托单
            if (!OrderItemDataModel_Entrusted.IsOrder_Entrusted(order.OrderType)) return false;

            // 只显示挂单状态订单
            if (!XueQiaoConstants.UnfinishedOrderStates.Contains(order.OrderState)) return false;

            return true;
        }

        private bool CanPassSellHangingOrderFilter(OrderItemDataModel_Entrusted order)
        {
            if (order == null) return false;
            return order.Direction == ClientTradeDirection.SELL && CanPassHangingOrderFilter(order);
        }

        private void OrderItemServiceOrderCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var newItems = e.NewItems?.Cast<OrderItemDataModel>()?.ToArray();
            var oldItems = e.OldItems?.Cast<OrderItemDataModel>()?.ToArray();

            if (newItems?.Any() == true)
            {
                var sellOrders = newItems
                    .OfType<OrderItemDataModel_Entrusted>()
                    .Where(i => CanPassSellHangingOrderFilter(i)).ToArray();
                if (sellOrders?.Any() == true)
                {
                    foreach (var item in sellOrders)
                    {
                        PropertyChangedEventManager.RemoveHandler(item, SellOrderItemPropChanged, "");
                        PropertyChangedEventManager.AddHandler(item, SellOrderItemPropChanged, "");
                    }
                    DispatcherHelper.CheckBeginInvokeOnUI(() => 
                    {
                        InvalidateCurrentLowestPriceSellOrder();
                    });
                }
            }

            if (oldItems?.Any() == true)
            {
                var sellOrders = oldItems
                    .OfType<OrderItemDataModel_Entrusted>()
                    .Where(i => CanPassSellHangingOrderFilter(i)).ToArray();
                if (sellOrders?.Any() == true)
                {
                    foreach (var item in sellOrders)
                    {
                        PropertyChangedEventManager.RemoveHandler(item, SellOrderItemPropChanged, "");
                    }
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        InvalidateCurrentLowestPriceSellOrder();
                    });
                }
            }
        }

        private void SellOrderItemPropChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(OrderItemDataModel_Entrusted.OrderState))
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    var order = sender as OrderItemDataModel_Entrusted;
                    if (order != null)
                    {
                        InvalidateCurrentLowestPriceSellOrder();
                    }
                });
            }
        }
        
        // 该组合或合约的成交列表
        public ListCollectionView CurrentItemTradeListView { get; private set; }
        
        private ListCollectionView _currentItemOrderListView;
        // 该组合或合约的订单列表
        public ListCollectionView CurrentItemOrderListView
        {
            get { return _currentItemOrderListView; }
            private set { SetProperty(ref _currentItemOrderListView, value); }
        }

        private OrderItemDataModel_Entrusted currentLowestPriceSellOrder;
        // 当前最低价卖方向的订单
        public OrderItemDataModel_Entrusted CurrentLowestPriceSellOrder
        {
            get { return currentLowestPriceSellOrder; }
            set { SetProperty(ref currentLowestPriceSellOrder, value); }
        }

        public void UpdateViewPresentKey(PlaceOrderComponentPresentKey presentKey)
        {
            if (this.currentPresentKey == presentKey) return;
            this.currentPresentKey = presentKey;
            this.TargetType = presentKey?.TargetType;

            if (targetType != null)
            {
                ViewCore.ResetOrderListColumnsByPresentTarget(targetType.Value);
            }

            InvalidateXqTargetPresentOrders();
            RefreshCurrentItemTradeListView();
            InvalidateCurrentLowestPriceSellOrder();
        }

        private ClientXQOrderTargetType? targetType;
        public ClientXQOrderTargetType? TargetType
        {
            get { return targetType; }
            private set { SetProperty(ref targetType, value); }
        }
        
        private SelectedOrdersOperateCommands selectedOrdersOptCommands;
        public SelectedOrdersOperateCommands SelectedOrdersOptCommands
        {
            get { return selectedOrdersOptCommands; }
            set { SetProperty(ref selectedOrdersOptCommands, value); }
        }

        private void InvalidateXqTargetPresentOrders()
        {
            ObservableCollection<OrderItemDataModel> orderList = null;
            var subAccountId = this.currentPresentKey?.SubAccountId;
            var targetType = this.currentPresentKey?.TargetType;
            var targetKey = this.currentPresentKey?.TargetKey;
            if (subAccountId != null && targetType != null && !string.IsNullOrEmpty(targetKey))
            {
                var listKey = new XqTargetPresentOrderListKey(subAccountId.Value, targetType.Value, targetKey, XQClientOrderType.Entrusted);
                orderList = xqTargetPresentOrderListCtrl.TryGetXqTargetPresentOrderList(listKey);
            }

            if (orderList == null)
            {
                this.CurrentItemOrderListView = null;
            }
            else
            {
                var xqTargetPresentOrders = new SynchronizingCollection<OrderItemDataModel, OrderItemDataModel>(orderList, i => i);
                this.CurrentItemOrderListView = CollectionViewSource.GetDefaultView(xqTargetPresentOrders) as ListCollectionView;
                ConfigCurrentItemOrderListView();
                this.CurrentItemOrderListView?.Refresh();
            }
        }
        
        private void ConfigCurrentItemOrderListView()
        {
            var listView = this.CurrentItemOrderListView;
            if (listView == null) return;

            // 设置排序。
            // 排序优先规则：Direction > Price > OrderId。这样能确保列表排序的稳定。
            // 如果去除 OrderId 排序，那么在 Price 相同时，可能某次 item1 排在前，某次可能是 Item2 排在前，这样便会引起排序的不稳定
            listView.SortDescriptions.Add(new SortDescription(nameof(OrderItemDataModel_Entrusted.Direction), ListSortDirection.Descending));
            listView.SortDescriptions.Add(new SortDescription(nameof(OrderItemDataModel_Entrusted.Price), ListSortDirection.Descending));
            listView.SortDescriptions.Add(new SortDescription(nameof(OrderItemDataModel_Entrusted.OrderId), ListSortDirection.Descending));
            listView.LiveSortingProperties.Add(nameof(OrderItemDataModel_Entrusted.Direction));
            listView.LiveSortingProperties.Add(nameof(OrderItemDataModel_Entrusted.Price));
            listView.LiveSortingProperties.Add(nameof(OrderItemDataModel_Entrusted.OrderId));


            // 设置过滤器
            listView.Filter = i =>
            {
                var order = i as OrderItemDataModel;
                if (order == null) return false;
                return CanPassHangingOrderFilter(order);
            };
            listView.LiveFilteringProperties.Add(nameof(OrderItemDataModel.OrderState));

            listView.IsLiveSorting = true;
            listView.IsLiveFiltering = true;
        }

        private void ConfigCurrentItemTradeListView()
        {
            var listView = CurrentItemTradeListView;
            if (listView == null) return;

            listView.SortDescriptions.Add(new SortDescription(nameof(TradeItemDataModel.CreateTimestampMs),
                ListSortDirection.Descending));

            listView.Filter = i =>
            {
                var trade = i as TradeItemDataModel;
                if (trade == null) return false;

                var subAccountId = this.currentPresentKey?.SubAccountId;
                var targetType = this.currentPresentKey?.TargetType;
                var targetKey = this.currentPresentKey?.TargetKey;

                if (trade.SubAccountFields.SubAccountId != subAccountId) return false;
                if (trade.TargetType != targetType) return false;
                if (trade.TargetKey != targetKey) return false;

                return true;
            };

            listView.IsLiveSorting = true;
            listView.IsLiveFiltering = true;
        }
        
        private void RefreshCurrentItemTradeListView()
        {
            CurrentItemTradeListView?.Refresh();
        }

        private void InvalidateCurrentLowestPriceSellOrder()
        {
            var collection = CurrentItemOrderListView?
                    .OfType<OrderItemDataModel_Entrusted>()
                    .Where(i => CanPassSellHangingOrderFilter(i)).ToArray();
            CurrentLowestPriceSellOrder = collection?
                .OrderBy(i => new OrderPriceCompareItem(i.Price, i.OrderId), new OrderPriceComparer())
                .FirstOrDefault();
            
        }

        /// <summary>
        /// 订单价格比较器。
        /// 比较优先级：Price > OrderId。在 Price 相同时再根据 OrderId 比较
        /// </summary>
        class OrderPriceComparer : IComparer<OrderPriceCompareItem>
        {
            public int Compare(OrderPriceCompareItem x, OrderPriceCompareItem y)
            {
                if (x.Price < y.Price)
                {
                    return -1;
                }
                else if (x.Price > y.Price)
                {
                    return 1;
                }

                return (x.OrderId ?? "a").CompareTo(y.OrderId ?? "b");
            }
        }

        class OrderPriceCompareItem
        {
            public OrderPriceCompareItem(double price, string orderId)
            {
                this.Price = price;
                this.OrderId = orderId;
            }

            public readonly double Price;
            public readonly string OrderId;
        }
    }
}
