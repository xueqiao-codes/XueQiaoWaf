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
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.BusinessResources.Models;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoWaf.Trade.Modules.Applications.Helper;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class OrderHistoryEntrustedListVM : ViewModel<IOrderHistoryEntrustedListView>, IShutdownObject
    {
        [ImportingConstructor]
        public OrderHistoryEntrustedListVM(IOrderHistoryEntrustedListView view) : base(view)
        {
            OrderList = new ObservableCollection<OrderItemDataModel_Entrusted>();
            OrderListCollectionView = CollectionViewSource.GetDefaultView(OrderList) as ListCollectionView;
            ConfigOrderListCollectionView();
        }
        
        public IEnumerable<ListColumnInfo> ListDisplayColumnInfos => ViewCore.ListDisplayColumnInfos;

        public void ResetListDisplayColumns(IEnumerable<ListColumnInfo> listColumnInfos)
        {
            ViewCore.ResetListDisplayColumns(listColumnInfos);
        }

        public ObservableCollection<OrderItemDataModel_Entrusted> OrderList { get; private set; }
        public ListCollectionView OrderListCollectionView { get; private set; }
        
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


        private void ConfigOrderListCollectionView()
        {
            var listCollectionView = OrderListCollectionView;
            if (listCollectionView == null) return;

            // 设置默认排序
            var orderTimePropName = nameof(OrderItemDataModel.OrderTimestampMs);
            listCollectionView.SortDescriptions.Add(new SortDescription(orderTimePropName, ListSortDirection.Descending));
            listCollectionView.LiveSortingProperties.Add(orderTimePropName);
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

            // update PageAvailableOrdersFilter of view
            ViewCore.PageAvailableOrdersFilter = obj => true;
        }

        #region IShutdownObject 

        private bool isDisposed = false;

        public void Shutdown()
        {
            if (isDisposed) return;

            isDisposed = true;
        }

        #endregion
    }
}
