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
    public class PositionDiscreteListPageViewModel : ViewModel<IPositionDiscreteListPageView>
    {
        private static readonly string[] ListLiveSortingPropertyNames = new string[]
        {
            nameof(PositionDiscreteItemDataModel.ContractId)
        };

        private readonly PositionDiscreteItemsService positionDiscreteItemsService;

        [ImportingConstructor]
        protected PositionDiscreteListPageViewModel(IPositionDiscreteListPageView view,
            PositionDiscreteItemsService positionDiscreteItemsService) : base(view)
        {
            this.positionDiscreteItemsService = positionDiscreteItemsService;

            var synchronizingPositionItems = new SynchronizingCollection<PositionDiscreteItemDataModel, PositionDiscreteItemDataModel>(positionDiscreteItemsService.PositionItems, i => i);
            PositionCollectionView = CollectionViewSource.GetDefaultView(synchronizingPositionItems) as ListCollectionView;
            PositionCollectionView.SortDescriptions.Add(new SortDescription(nameof(PositionDiscreteItemDataModel.ContractId),
                ListSortDirection.Ascending));
            foreach (var liveSortingProp in ListLiveSortingPropertyNames)
            {
                PositionCollectionView.LiveSortingProperties.Add(liveSortingProp);
            }
            PositionCollectionView.IsLiveSorting = true;

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

        private ICommand selectedItemsDeleteExpiredCmd;
        public ICommand SelectedItemsDeleteExpiredCmd
        {
            get { return selectedItemsDeleteExpiredCmd; }
            set { SetProperty(ref selectedItemsDeleteExpiredCmd, value); }
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

        private void InvalidateListGlobalFilter()
        {
            var existPositionLiveFilter = new Predicate<PositionDiscreteItemDataModel>(position => 
            {
                return position.IsExistPosition;
            });
            PositionCollectionView.LiveFilteringProperties.Add(nameof(PositionDiscreteItemDataModel.IsExistPosition));

            var subAccountFilter = new Predicate<PositionDiscreteItemDataModel>(position => 
            {
                return position.SubAccountFields.SubAccountId == this.PresentSubAccountId;
            });

            PositionCollectionView.Filter = new Predicate<object>(obj => 
            {
                var position = obj as PositionDiscreteItemDataModel;
                if (position == null) return false;
                if (!existPositionLiveFilter.Invoke(position)) return false;
                if (!subAccountFilter.Invoke(position)) return false;
                return true;
            });

            PositionCollectionView.IsLiveFiltering = true;
            PositionCollectionView.Refresh();
        }
    }
}
