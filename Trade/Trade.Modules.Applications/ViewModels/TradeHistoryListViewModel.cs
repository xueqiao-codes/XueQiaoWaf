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
using XueQiaoWaf.Trade.Modules.Applications.Helper;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class TradeHistoryListViewModel : ViewModel<ITradeHistoryListView>
    {
        [ImportingConstructor]
        protected TradeHistoryListViewModel(ITradeHistoryListView view) : base(view)
        {
            TradeList = new ObservableCollection<TradeItemDataModel>();
            TradeListCollectionView = CollectionViewSource.GetDefaultView(TradeList) as ListCollectionView;
            InitialConfigTradeListCollectionView();
        }
        
        public ObservableCollection<TradeItemDataModel> TradeList { get; private set; }
        public ListCollectionView TradeListCollectionView { get; private set; }

        public object DisplayInWindow => ViewCore.DisplayInWindow;

        public IEnumerable<ListColumnInfo> ListDisplayColumnInfos => ViewCore.ListDisplayColumnInfos;

        public void ResetListDisplayColumns(IEnumerable<ListColumnInfo> listColumnInfos)
        {
            ViewCore.ResetListDisplayColumns(listColumnInfos);
        }

        private DateTime? selectedDate;
        public DateTime? SelectedDate
        {
            get { return selectedDate; }
            set { SetProperty(ref selectedDate, value); }
        }

        private long? refreshListTimestamp;
        public long? RefreshListTimestamp
        {
            get { return refreshListTimestamp; }
            set { SetProperty(ref refreshListTimestamp, value); }
        }

        private ICommand toShowOrderExecuteDetailCmd;
        public ICommand ToShowOrderExecuteDetailCmd
        {
            get { return toShowOrderExecuteDetailCmd; }
            set { SetProperty(ref toShowOrderExecuteDetailCmd, value); }
        }

        private ICommand refreshListCmd;
        public ICommand RefreshListCmd
        {
            get { return refreshListCmd; }
            set { SetProperty(ref refreshListCmd, value); }
        }
        
        private ICommand clickItemTargetKeyRelatedColumnCmd;
        public ICommand ClickItemTargetKeyRelatedColumnCmd
        {
            get { return clickItemTargetKeyRelatedColumnCmd; }
            set { SetProperty(ref clickItemTargetKeyRelatedColumnCmd, value); }
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
