using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Data;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Services;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class FundPageViewModel : ViewModel<IFundPageView>
    {
        private readonly FundItemsService fundItemsService;

        [ImportingConstructor]
        protected FundPageViewModel(IFundPageView view,
            FundItemsService fundItemsService) : base(view)
        {
            this.fundItemsService = fundItemsService;

            var synchronizingFundItems = new SynchronizingCollection<FundItemDataModel, FundItemDataModel>(fundItemsService.FundItems, i => i);
            FundCollectionView = CollectionViewSource.GetDefaultView(synchronizingFundItems) as ListCollectionView;
        }

        public void UpdatePresentSubAccountId(long subAccountId)
        {
            if (this.PresentSubAccountId == subAccountId) return;
            this.PresentSubAccountId = subAccountId;
            InvalidateListGlobalFilter();
        }

        public long PresentSubAccountId { get; private set; }

        public ListCollectionView FundCollectionView { get; private set; }
        
        private void InvalidateListGlobalFilter()
        {
            var existFundLiveFilter = new Predicate<FundItemDataModel>(fund =>
            {
                return fund.IsExistFund;
            });
            FundCollectionView.LiveFilteringProperties.Add(nameof(FundItemDataModel.IsExistFund));

            var subAccountFilter = new Predicate<FundItemDataModel>(fund =>
            {
                return fund.SubAccountFields.SubAccountId == this.PresentSubAccountId;
            });

            FundCollectionView.Filter = new Predicate<object>(obj =>
            {
                var fund = obj as FundItemDataModel;
                if (fund == null) return false;
                if (!existFundLiveFilter.Invoke(fund)) return false;
                if (!subAccountFilter.Invoke(fund)) return false;
                return true;
            });

            FundCollectionView.IsLiveFiltering = true;
            FundCollectionView.Refresh();
        }
    }
}
