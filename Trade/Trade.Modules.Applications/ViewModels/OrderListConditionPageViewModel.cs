using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Data;
using System.Windows.Input;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.BusinessResources.Models;
using XueQiaoWaf.Trade.Modules.Applications.Services;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class OrderListConditionPageViewModel : ViewModel<IOrderListConditionPageView>
    {
        private readonly OrderItemsService orderItemsService;

        [ImportingConstructor]
        public OrderListConditionPageViewModel(IOrderListConditionPageView view,
            OrderItemsService orderItemsService) : base(view)
        {
            this.orderItemsService = orderItemsService;

            this.OrderListDataContext = new ConditionOrderRealtimeListDataContext();
            OrderListDataContext.OrderListFilterTypeChanged = (dc, filterType) => 
            {
                ResetOrderListFilter();
            };
            OrderListDataContext.SelectAllOrderItemsHandler = dc => ViewCore.SelectAllOrderItems();

            var synchronizingOrders = new SynchronizingCollection<OrderItemDataModel, OrderItemDataModel>(orderItemsService.OrderItems, i => i);
            OrderListCollectionView = CollectionViewSource.GetDefaultView(synchronizingOrders) as ListCollectionView;
            ConditionOrderRealtimeListDataContext.CommonConfigConditionOrderListCollectionView(OrderListCollectionView,
                new SortDescription(nameof(OrderItemDataModel_Condition.OrderTimestampMs), ListSortDirection.Descending));
            ResetOrderListFilter();
        }

        /// <summary>
        /// 订单列表 data context
        /// </summary>
        public ConditionOrderRealtimeListDataContext OrderListDataContext { get; private set; }
        
        public IEnumerable<ListColumnInfo> ListDisplayColumnInfos => ViewCore.ListDisplayColumnInfos;

        public void ResetListDisplayColumns(IEnumerable<ListColumnInfo> listColumnInfos)
        {
            ViewCore.ResetListDisplayColumns(listColumnInfos);
        }

        public void UpdatePresentSubAccountId(long subAccountId)
        {
            if (this.PresentSubAccountId == subAccountId) return;

            this.PresentSubAccountId = subAccountId;
            ResetOrderListFilter();
        }

        public long PresentSubAccountId { get; private set; }

        public ListCollectionView OrderListCollectionView { get; private set; }


        private ICommand subscribeTargetQuotationCmd;
        public ICommand SubscribeTargetQuotationCmd
        {
            get { return subscribeTargetQuotationCmd; }
            set { SetProperty(ref subscribeTargetQuotationCmd, value); }
        }

        private ICommand toConfigOrderListColumnsCmd;
        public ICommand ToConfigOrderListColumnsCmd
        {
            get { return toConfigOrderListColumnsCmd; }
            set { SetProperty(ref toConfigOrderListColumnsCmd, value); }
        }

        private ICommand toApplyDefaultOrderListColumnsCmd;
        public ICommand ToApplyDefaultOrderListColumnsCmd
        {
            get { return toApplyDefaultOrderListColumnsCmd; }
            set { SetProperty(ref toApplyDefaultOrderListColumnsCmd, value); }
        }

        /// <summary>
        /// 点击项目的唯一标识相关列 command
        /// cmd param: <see cref="OrderItemDataModel"/>类型
        /// </summary>
        private ICommand clickItemTargetKeyRelatedColumnCmd;
        public ICommand ClickItemTargetKeyRelatedColumnCmd
        {
            get { return clickItemTargetKeyRelatedColumnCmd; }
            set { SetProperty(ref clickItemTargetKeyRelatedColumnCmd, value); }
        }
        
        private ICommand toShowChildOrderCmd;
        public ICommand ToShowChildOrderCmd
        {
            get { return toShowChildOrderCmd; }
            set { SetProperty(ref toShowChildOrderCmd, value); }
        }
        
        private void ResetOrderListFilter()
        {
            var collectionView = this.OrderListCollectionView;
            if (collectionView == null) return;
            
            var compositFilter = new Predicate<object>(i =>
            {
                var orderModel = i as OrderItemDataModel_Condition;
                if (orderModel == null) return false;
                if (orderModel.SubAccountFields.SubAccountId != PresentSubAccountId) return false;
                if (orderModel.BelongListFilterTypes?.Contains(this.OrderListDataContext.ListFilterType) != true) return false;
                return true;
            });
            collectionView.Filter = compositFilter;
            OrderListCollectionView.Refresh();
        }
    }
}
