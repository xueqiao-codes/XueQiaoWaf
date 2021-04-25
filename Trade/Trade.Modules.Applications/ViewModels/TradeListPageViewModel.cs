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
using XueQiaoWaf.Trade.Modules.Applications.Helper;
using XueQiaoWaf.Trade.Modules.Applications.Services;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class TradeListPageViewModel : ViewModel<ITradeListPageView>
    {
        [ImportingConstructor]
        public TradeListPageViewModel(ITradeListPageView view,
            TradeItemsService tradeDealService) : base(view)
        {
            var syncDeals = new SynchronizingCollection<TradeItemDataModel, TradeItemDataModel>(tradeDealService.TradeItems, i => i);
            TradeListCollectionView = CollectionViewSource.GetDefaultView(syncDeals) as ListCollectionView;
            InitialConfigTradeListCollectionView();
        }
        
        public void UpdatePresentSubAccountId(long subAccountId)
        {
            if (this.PresentSubAccountId == subAccountId) return;

            this.PresentSubAccountId = subAccountId;
            InvalidateListGlobalFilter();
        }

        public long PresentSubAccountId { get; private set; }

        public ListCollectionView TradeListCollectionView { get; private set; }
        
        private ICommand subscribeTargetQuotationCmd;
        public ICommand SubscribeTargetQuotationCmd
        {
            get { return subscribeTargetQuotationCmd; }
            set { SetProperty(ref subscribeTargetQuotationCmd, value); }
        }

        private ICommand toShowOrderExecuteDetailCmd;
        public ICommand ToShowOrderExecuteDetailCmd
        {
            get { return toShowOrderExecuteDetailCmd; }
            set { SetProperty(ref toShowOrderExecuteDetailCmd, value); }
        }

        private ICommand clickItemTargetKeyRelatedColumnCmd;
        public ICommand ClickItemTargetKeyRelatedColumnCmd
        {
            get { return clickItemTargetKeyRelatedColumnCmd; }
            set { SetProperty(ref clickItemTargetKeyRelatedColumnCmd, value); }
        }

        private ICommand toConfigTradeListColumnsCmd;
        public ICommand ToConfigTradeListColumnsCmd
        {
            get { return toConfigTradeListColumnsCmd; }
            set { SetProperty(ref toConfigTradeListColumnsCmd, value); }
        }

        private ICommand toApplyDefaultTradeListColumnsCmd;
        public ICommand ToApplyDefaultTradeListColumnsCmd
        {
            get { return toApplyDefaultTradeListColumnsCmd; }
            set { SetProperty(ref toApplyDefaultTradeListColumnsCmd, value); }
        }

        public object DisplayInWindow => ViewCore.DisplayInWindow;
        
        public IEnumerable<ListColumnInfo> ListDisplayColumnInfos => ViewCore.ListDisplayColumnInfos;

        public void ResetListDisplayColumns(IEnumerable<ListColumnInfo> listColumnInfos)
        {
            ViewCore.ResetListDisplayColumns(listColumnInfos);
        }


        private void InvalidateListGlobalFilter()
        {
            var listCollectionView = TradeListCollectionView;
            if (listCollectionView == null) return;

            listCollectionView.Filter = new Predicate<object>(i =>
            {
                var tradeItem = i as TradeItemDataModel;
                if (tradeItem == null) return false;
                if (tradeItem.SubAccountFields.SubAccountId != this.PresentSubAccountId) return false;
                return true;
            });
            listCollectionView.IsLiveFiltering = true;
            listCollectionView.Refresh();
        }

        private void InitialConfigTradeListCollectionView()
        {
            var listCollectionView = TradeListCollectionView;
            if (listCollectionView == null) return;

            // 设置默认排序
            var createTimePropName = nameof(TradeItemDataModel.CreateTimestampMs);
            listCollectionView.SortDescriptions.Add(new SortDescription(createTimePropName, ListSortDirection.Descending));
            listCollectionView.LiveSortingProperties.Add(createTimePropName);
            listCollectionView.IsLiveSorting = true;

            var columnKeyedLiveProps = ListColumnLivePropertiesHelper.TradeItemColumnKeyedLiveProperties.ToArray();

            // 设置 live sorting properties
            foreach (var sortableColumn in ListColumnLivePropertiesHelper.TradeListSortableColumns)
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
            foreach (var filterableColumn in ListColumnLivePropertiesHelper.TradeListFilterableColumns)
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
    }
}
