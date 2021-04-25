using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Waf.Foundation;
using System.Windows.Data;
using System.Windows.Input;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Helper;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    public class ParkedOrderRealtimeListDataContext : Model
    {
        private readonly DelegateCommand _orderListFilterTypeSelectCommand;
        private readonly DelegateCommand _selectAllOrderItemsCmd;

        public ParkedOrderRealtimeListDataContext()
        {
            _orderListFilterTypeSelectCommand = new DelegateCommand(obj =>
            {
                if (obj is ParkedOrderListFilterType selectedFilterType)
                {
                    this.ListFilterType = selectedFilterType;
                }
            });

            _selectAllOrderItemsCmd = new DelegateCommand(() => SelectAllOrderItemsHandler?.Invoke(this));

            this.ListFilterType = ParkedOrderListFilterType.Hanging;
        }

        public Action<ParkedOrderRealtimeListDataContext, ParkedOrderListFilterType> OrderListFilterTypeChanged;

        public Action<ParkedOrderRealtimeListDataContext> SelectAllOrderItemsHandler;

        private SelectedOrdersOperateCommands selectedOrdersOptCommands;
        public SelectedOrdersOperateCommands SelectedOrdersOptCommands
        {
            get { return selectedOrdersOptCommands; }
            set { SetProperty(ref selectedOrdersOptCommands, value); }
        }

        public ICommand OrderListFilterTypeSelectCommand => _orderListFilterTypeSelectCommand;

        public ICommand SelectAllOrderItemsCmd => _selectAllOrderItemsCmd;

        private ParkedOrderListFilterType listFilterType;
        public ParkedOrderListFilterType ListFilterType
        {
            get { return listFilterType; }
            private set
            {
                if (SetProperty(ref listFilterType, value))
                {
                    OrderListFilterTypeChanged?.Invoke(this, value);
                }
            }
        }

        /// <summary>
        /// 常规配置预埋订单列表 collection view
        /// </summary>
        /// <param name="orderListCollectionView"></param>
        public static void CommonConfigParkedOrderListCollectionView(ListCollectionView orderListCollectionView,
                    SortDescription initialSortDescription)
        {
            var collectionView = orderListCollectionView;
            if (collectionView == null) return;

            collectionView.IsLiveSorting = true;
            collectionView.IsLiveFiltering = true;

            if (initialSortDescription != null)
            {
                collectionView.SortDescriptions.Add(initialSortDescription);
                collectionView.LiveSortingProperties.Add(initialSortDescription.PropertyName);
            }

            var columnKeyedLiveProps = ListColumnLivePropertiesHelper.ParkedOrderColumnKeyedOrderLiveProperties.ToArray();

            // 设置 live sorting properties
            foreach (var sortableColumn in ListColumnLivePropertiesHelper.ParkedOrderListSortableColumns)
            {
                var existLiveProps = columnKeyedLiveProps.FirstOrDefault(i => i.Key == sortableColumn).Value;
                if (existLiveProps?.Any() == true)
                {
                    foreach (var liveProp in existLiveProps)
                    {
                        if (!collectionView.LiveSortingProperties.Contains(liveProp))
                        {
                            collectionView.LiveSortingProperties.Add(liveProp);
                        }
                    }
                }
            }

            // 设置 live filter properties
            foreach (var filterableColumn in ListColumnLivePropertiesHelper.ParkedOrderListFilterableColumns)
            {
                var existLiveProps = columnKeyedLiveProps.FirstOrDefault(i => i.Key == filterableColumn).Value;
                if (existLiveProps?.Any() == true)
                {
                    foreach (var liveProp in existLiveProps)
                    {
                        if (!collectionView.LiveFilteringProperties.Contains(liveProp))
                        {
                            collectionView.LiveFilteringProperties.Add(liveProp);
                        }
                    }
                }
            }
        }
    }
}
