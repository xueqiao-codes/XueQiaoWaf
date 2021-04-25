using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Data;
using System.Windows.Input;
using XueQiaoFoundation.BusinessResources.Constants;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.BusinessResources.Models;
using XueQiaoWaf.Trade.Modules.Applications.Helper;
using XueQiaoWaf.Trade.Modules.Applications.ServiceControllers;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class XqTargetParkedOrdersVM : ViewModel<IXqTargetParkedOrdersView>
    {
        private readonly XqTargetPresentOrderListCtrl xqTargetPresentOrderListCtrl;

        [ImportingConstructor]
        protected XqTargetParkedOrdersVM(IXqTargetParkedOrdersView view,
            XqTargetPresentOrderListCtrl xqTargetPresentOrderListCtrl) : base(view)
        {
            this.xqTargetPresentOrderListCtrl = xqTargetPresentOrderListCtrl;

            this.OrderListDataContext = new ParkedOrderRealtimeListDataContext();
            OrderListDataContext.OrderListFilterTypeChanged = (dc, filterType) =>
            {
                ResetOrderListFilter();
            };
            OrderListDataContext.SelectAllOrderItemsHandler = dc => ViewCore.SelectAllOrderItems();

            // set show columns 
            var initialColumnInfos = TradeWorkspaceDataDisplayHelper.DefaultOrderListParkedDisplayColumns
                .Select(i => new ListColumnInfo
                {
                    ColumnCode = i.GetHashCode(),
                    ContentAlignment = XueQiaoConstants.ListColumnContentAlignment_Left
                }).ToArray();
            ViewCore.ResetListDisplayColumns(initialColumnInfos);
        }

        /// <summary>
        /// 订单列表 data context
        /// </summary>
        public ParkedOrderRealtimeListDataContext OrderListDataContext { get; private set; }

        private ICommand toShowChildOrderCmd;
        public ICommand ToShowChildOrderCmd
        {
            get { return toShowChildOrderCmd; }
            set { SetProperty(ref toShowChildOrderCmd, value); }
        }
        
        public void UpdateXqTargetPresentOrderListKey(long subAccountId, ClientXQOrderTargetType targetType, string targetKey)
        {
            this.ListKey = new XqTargetPresentOrderListKey(subAccountId, targetType, targetKey, XQClientOrderType.Parked);
        }

        private ListCollectionView _orderListCollectionView;
        public ListCollectionView OrderListCollectionView
        {
            get { return _orderListCollectionView; }
            private set { SetProperty(ref _orderListCollectionView, value); }
        }

        private XqTargetPresentOrderListKey listKey;
        public XqTargetPresentOrderListKey ListKey
        {
            get { return listKey; }
            private set
            {
                if (SetProperty(ref listKey, value))
                {
                    InvalidateXqTargetPresentOrders();
                }
            }
        }

        private void InvalidateXqTargetPresentOrders()
        {
            var listKey = this.ListKey;
            ObservableCollection<OrderItemDataModel> orderList = null;
            if (listKey != null)
                orderList = xqTargetPresentOrderListCtrl.TryGetXqTargetPresentOrderList(listKey);

            if (orderList == null)
            {
                this.OrderListCollectionView = null;
            }
            else
            {
                var xqTargetPresentOrders = new SynchronizingCollection<OrderItemDataModel, OrderItemDataModel>(orderList, i => i);
                this.OrderListCollectionView = CollectionViewSource.GetDefaultView(xqTargetPresentOrders) as ListCollectionView;
                ParkedOrderRealtimeListDataContext.CommonConfigParkedOrderListCollectionView(this.OrderListCollectionView,
                    new SortDescription(nameof(OrderItemDataModel_Parked.OrderTimestampMs), ListSortDirection.Descending));
                ResetOrderListFilter();
            }
        }

        private void ResetOrderListFilter()
        {
            var collectionView = this.OrderListCollectionView;
            if (collectionView == null) return;

            var compositFilter = new Predicate<object>(i =>
            {
                var orderModel = i as OrderItemDataModel_Parked;
                if (orderModel == null) return false;
                if (orderModel.BelongListFilterTypes?.Contains(this.OrderListDataContext.ListFilterType) != true) return false;
                return true;
            });

            collectionView.Filter = compositFilter;
            collectionView.Refresh();
        }
    }
}
