using BolapanControl.ItemsFilter;
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
using XueQiaoFoundation.BusinessResources.ItemsFilters;
using XueQiaoFoundation.BusinessResources.Models;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Helper;
using XueQiaoWaf.Trade.Modules.Applications.Services;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class OrderListEntrustedPageViewModel : ViewModel<IOrderListEntrustedPageView>, IShutdownObject
    {
        private readonly OrderItemsService orderItemsService;

        [ImportingConstructor]
        public OrderListEntrustedPageViewModel(IOrderListEntrustedPageView view,
            OrderItemsService orderItemsService) : base(view)
        {
            this.orderItemsService = orderItemsService;
            var synchronizingOrders = new SynchronizingCollection<OrderItemDataModel, OrderItemDataModel>(orderItemsService.OrderItems, i => i);

            OrderListCollectionView = CollectionViewSource.GetDefaultView(synchronizingOrders) as ListCollectionView;
            ConfigOrderListCollectionView();

            OrderListFilterTypeSelectCommand = new DelegateCommand(SelectOrderListFilterType);
            // 默认显示挂单订单 
            ListFilterType = EntrustedOrderListFilterType.Hanging;
        }
        
        public IEnumerable<ListColumnInfo> ListDisplayColumnInfos => ViewCore.ListDisplayColumnInfos;

        public void ResetListDisplayColumns(IEnumerable<ListColumnInfo> listColumnInfos)
        {
            ViewCore.ResetListDisplayColumns(listColumnInfos);
        }

        public void UpdatePresentSubAccountId(long subAccountId)
        {
            if (this.PresentSubAccountId == subAccountId) return;

            this.PresentSubAccountId = subAccountId;
            InvalidateOrderCollectionViewGlobalFilter();
        }

        public long PresentSubAccountId { get; private set; }

        public ListCollectionView OrderListCollectionView { get; private set; }

        public DelegateCommand OrderListFilterTypeSelectCommand { get; private set; }
        
        private EntrustedOrderListFilterType listFilterType;
        public EntrustedOrderListFilterType ListFilterType
        {
            get { return listFilterType; }
            set
            {
                if (SetProperty(ref listFilterType, value))
                {
                    InvalidateOrderCollectionViewGlobalFilter();
                }
            }
        }

        private ICommand toShowOrderExecuteDetailCmd;
        public ICommand ToShowOrderExecuteDetailCmd
        {
            get { return toShowOrderExecuteDetailCmd; }
            set { SetProperty(ref toShowOrderExecuteDetailCmd, value); }
        }

        private ICommand toShowParentOrderCmd;
        public ICommand ToShowParentOrderCmd
        {
            get { return toShowParentOrderCmd; }
            set { SetProperty(ref toShowParentOrderCmd, value); }
        }

        private SelectedOrdersOperateCommands selectedOrdersOptCommands;
        public SelectedOrdersOperateCommands SelectedOrdersOptCommands
        {
            get { return selectedOrdersOptCommands; }
            set { SetProperty(ref selectedOrdersOptCommands, value); }
        }

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

        private void SelectOrderListFilterType(object param)
        {
            if (param is EntrustedOrderListFilterType selectedFilterType)
            {
                this.ListFilterType = selectedFilterType;
            }
        }


        private void ConfigOrderListCollectionView()
        {
            var listCollectionView = OrderListCollectionView;
            if (listCollectionView == null) return;

            // 设置默认排序
            var orderTimePropName = nameof(OrderItemDataModel.OrderTimestampMs);
            listCollectionView.SortDescriptions.Add(new SortDescription(orderTimePropName, ListSortDirection.Descending));
            listCollectionView.IsLiveSorting = true;

            var columnKeyedLiveProps = ListColumnLivePropertiesHelper.EntrustedOrderColumnKeyedOrderLiveProperties.ToArray();

            // 设置 live sorting properties
            foreach (var sortableColumn in ListColumnLivePropertiesHelper.EntrustedOrderListSortableColumns)
            {
                var existLiveProps = columnKeyedLiveProps.FirstOrDefault(i => i.Key == sortableColumn).Value;
                if (existLiveProps?.Any() == true)
                {
                    foreach (var liveProp in existLiveProps)
                    {
                        if (!listCollectionView.LiveSortingProperties.Contains(liveProp))
                        {
                            listCollectionView.LiveSortingProperties.Add(liveProp);
                        }
                    }
                }
            }
            listCollectionView.IsLiveSorting = true;

            // 设置 live filter properties
            foreach (var filterableColumn in ListColumnLivePropertiesHelper.EntrustedOrderListFilterableColumns)
            {
                var existLiveProps = columnKeyedLiveProps.FirstOrDefault(i => i.Key == filterableColumn).Value;
                if (existLiveProps?.Any() == true)
                {
                    foreach (var liveProp in existLiveProps)
                    {
                        if (!listCollectionView.LiveFilteringProperties.Contains(liveProp))
                        {
                            listCollectionView.LiveFilteringProperties.Add(liveProp);
                        }
                    }
                }
            }
            listCollectionView.IsLiveFiltering = true;
        }

        private const string FilterKey_OrderCollectionViewGlobalFilter = "OrderCollectionViewGlobalFilter";
        private void InvalidateOrderCollectionViewGlobalFilter()
        {
            if (OrderListCollectionView == null) return;
            var filterPresenter = FilterPresenter.TryGet(OrderListCollectionView.SourceCollection);
            if (filterPresenter == null) return;

            this.orderCollectionViewFilterPresenter = filterPresenter;

            var globalFilter = filterPresenter.TryGetFilter(FilterKey_OrderCollectionViewGlobalFilter, new DataItemsCustomFilterInitializer())
                    as DataItemsCustomFilter;
            if (globalFilter != null)
            {
                globalFilter.CustomFilter = item =>
                {
                    var entrustedOrder = item as OrderItemDataModel_Entrusted;
                    if (entrustedOrder == null) return false;
                    if (entrustedOrder.SubAccountFields.SubAccountId != this.PresentSubAccountId) return false;
                    if (entrustedOrder.BelongListFilterTypes?.Contains(this.ListFilterType) != true) return false;
                    return true;
                };
                // active filter
                globalFilter.IsActive = true;
            }

            // update PageAvailableOrdersFilter of view
            ViewCore.PageAvailableOrdersFilter = globalFilter?.CustomFilter;
            

            var liveFilteringProps = new string[] { nameof(OrderItemDataModel_Entrusted.BelongListFilterTypes) };
            var collectionViewLiveFilteringProperties = OrderListCollectionView.LiveFilteringProperties;
            foreach (var prop in liveFilteringProps)
            {
                if (!collectionViewLiveFilteringProperties.Contains(prop))
                {
                    collectionViewLiveFilteringProperties.Add(prop);
                }
            }
            OrderListCollectionView.IsLiveFiltering = true;

            // Refresh after config filter 
            filterPresenter.IsFilterActive = true;
            filterPresenter.DeferRefresh().Dispose();
        }

        private FilterPresenter orderCollectionViewFilterPresenter;

        private void DisposeOrderCollectionViewFilterPresenter()
        {
            var fp = this.orderCollectionViewFilterPresenter;
            if (fp != null)
            {
                FilterPresenter.TryRemoveFilterPresenter(fp.CollectionView);
            }
        }

        #region IShutdownObject 

        private bool isDisposed = false;

        public void Shutdown()
        {
            if (isDisposed) return;
            DisposeOrderCollectionViewFilterPresenter();
            isDisposed = true;
        }
        
        #endregion
    }
}
