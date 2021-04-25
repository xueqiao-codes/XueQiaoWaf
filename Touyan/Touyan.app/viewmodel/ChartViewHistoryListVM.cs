using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Data;
using Touyan.app.view;
using Touyan.Interface.datamodel;
using Touyan.Interface.service;

namespace Touyan.app.viewmodel
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class ChartViewHistoryListVM : ViewModel<ChartViewHistoryListView>
    {
        private readonly ITouyanChartViewHistoryService chartViewHistoryService;
        private readonly ListCollectionView historyCollectionView;

        [ImportingConstructor]
        public ChartViewHistoryListVM(ChartViewHistoryListView view,
            ITouyanChartViewHistoryService chartViewHistoryService) : base(view)
        {
            this.chartViewHistoryService = chartViewHistoryService;

            var syncHistorys = new SynchronizingCollection<ChartViewHistoryDM, ChartViewHistoryDM>(chartViewHistoryService.HistoryItems, i => i);
            this.historyCollectionView = CollectionViewSource.GetDefaultView(syncHistorys) as ListCollectionView;
            InitialConfigHistoryCollectionView();

        }
        
        public ICollectionView HistoryCollectionView
        {
            get { return historyCollectionView; }
        }
        
        private ChartViewHistoryDM selectedHistoryItem;
        public ChartViewHistoryDM SelectedHistoryItem
        {
            get { return selectedHistoryItem; }
            set { SetProperty(ref selectedHistoryItem, value); }
        }

        private void InitialConfigHistoryCollectionView()
        {
            var collectionView = this.historyCollectionView;
            if (collectionView == null) return;

            var viewTimestampProp = nameof(ChartViewHistoryDM.ViewTimestamp);
            collectionView.SortDescriptions.Add(new SortDescription(viewTimestampProp, ListSortDirection.Descending));
        }
    }
}
