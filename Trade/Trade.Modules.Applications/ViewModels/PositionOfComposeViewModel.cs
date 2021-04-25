using NativeModel.Trade;
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
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Services;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class PositionOfComposeViewModel : ViewModel<IPositionOfComposeView>
    {
        private readonly TargetPositionService targetPositionService;
        private readonly SynchronizingCollection<TargetPositionDataModel, TargetPositionDataModel> synchingPositionItems;

        [ImportingConstructor]
        protected PositionOfComposeViewModel(IPositionOfComposeView view, 
            TargetPositionService targetPositionService) : base(view)
        {
            this.targetPositionService = targetPositionService;
            this.synchingPositionItems = new SynchronizingCollection<TargetPositionDataModel, TargetPositionDataModel>(targetPositionService.PositionItems, i => i);
            this.PositionCollectionView = CollectionViewSource.GetDefaultView(synchingPositionItems) as ListCollectionView;
            ConfigPositionCollectionView();
            InvalidateListGlobalFilter();
        }
        
        public void UpdatePresentSubAccountId(long subAccountId)
        {
            if (this.PresentSubAccountId == subAccountId) return;
            this.PresentSubAccountId = subAccountId;
            InvalidateListGlobalFilter();
        }

        public long PresentSubAccountId { get; private set; }

        public ListCollectionView PositionCollectionView { get; private set; }
        
        private ICommand listItemsSelectionChangedCmd;
        public ICommand ListItemsSelectionChangedCmd
        {
            get { return listItemsSelectionChangedCmd; }
            set { SetProperty(ref listItemsSelectionChangedCmd, value); }
        }
        
        private ICommand selectedItemsSubscribeQuotationCmd;
        public ICommand SelectedItemsSubscribeQuotationCmd
        {
            get { return selectedItemsSubscribeQuotationCmd; }
            set { SetProperty(ref selectedItemsSubscribeQuotationCmd, value); }
        }
        
        private ICommand clickItemTargetKeyRelatedColumnCmd;
        public ICommand ClickItemTargetKeyRelatedColumnCmd
        {
            get { return clickItemTargetKeyRelatedColumnCmd; }
            set { SetProperty(ref clickItemTargetKeyRelatedColumnCmd, value); }
        }
        
        private ICommand toStopLostOrProfitCmd;
        public ICommand ToStopLostOrProfitCmd
        {
            get { return toStopLostOrProfitCmd; }
            set { SetProperty(ref toStopLostOrProfitCmd, value); }
        }

        private ICommand toShowPositionDetailCmd;
        public ICommand ToShowPositionDetailCmd
        {
            get { return toShowPositionDetailCmd; }
            set { SetProperty(ref toShowPositionDetailCmd, value); }
        }

        private ICommand toSplitComposePositionCmd;
        public ICommand ToSplitComposePositionCmd
        {
            get { return toSplitComposePositionCmd; }
            set { SetProperty(ref toSplitComposePositionCmd, value); }
        }


        private void ConfigPositionCollectionView()
        {
            var listView = this.PositionCollectionView;
            if (listView == null) return;

            var sortPropNames = new string[] { nameof(TargetPositionDataModel.TargetKey) };
            foreach (var sortProp in sortPropNames)
            {
                listView.SortDescriptions.Add(new SortDescription(sortProp, ListSortDirection.Ascending));
                PositionCollectionView.LiveSortingProperties.Add(sortProp);
            }
            PositionCollectionView.IsLiveSorting = true;
        }

        private void InvalidateListGlobalFilter()
        {
            var isComposeTypeFilter = new Predicate<TargetPositionDataModel>(position => position.TargetType == ClientXQOrderTargetType.COMPOSE_TARGET);
            var subAccountFilter = new Predicate<TargetPositionDataModel>(position =>
            {
                return position.SubAccountFields.SubAccountId == this.PresentSubAccountId;
            });
            
            PositionCollectionView.Filter = new Predicate<object>(obj =>
            {
                var position = obj as TargetPositionDataModel;
                if (position == null) return false;
                if (!isComposeTypeFilter.Invoke(position)) return false;
                if (!subAccountFilter.Invoke(position)) return false;
                return true;
            });

            PositionCollectionView.IsLiveFiltering = true;
            PositionCollectionView.Refresh();
        }
    }
}
