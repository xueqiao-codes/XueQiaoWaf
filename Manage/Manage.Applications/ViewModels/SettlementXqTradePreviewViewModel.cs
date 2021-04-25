using Manage.Applications.DataModels;
using Manage.Applications.Services;
using Manage.Applications.Views;
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

namespace Manage.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class SettlementXqTradePreviewViewModel : ViewModel<ISettlementXqTradePreviewView>
    {
        [ImportingConstructor]
        protected SettlementXqTradePreviewViewModel(ISettlementXqTradePreviewView view) : base(view)
        {
            PreviewTradeItems = new ObservableCollection<PositionVerifyTradeInputDM>();

            PreviewTradeItemsCollectionView = CollectionViewSource.GetDefaultView(PreviewTradeItems) as ListCollectionView;
            PreviewTradeItemsCollectionView.SortDescriptions.Add(new SortDescription { PropertyName = nameof(PositionVerifyTradeInputDM.TradeTimestamp), Direction = ListSortDirection.Ascending });
            PreviewTradeItemsCollectionView.SortDescriptions.Add(new SortDescription { PropertyName = nameof(PositionVerifyTradeInputDM.ContractId), Direction = ListSortDirection.Ascending });
        }
        
        public ObservableCollection<PositionVerifyTradeInputDM> PreviewTradeItems { get; private set; }
        public ListCollectionView PreviewTradeItemsCollectionView { get; private set; }

        private ICommand editXqTradeItemCmd;
        public ICommand EditXqTradeItemCmd
        {
            get { return editXqTradeItemCmd; }
            set { SetProperty(ref editXqTradeItemCmd, value); }
        }

        private ICommand rmXqTradeItemCmd;
        public ICommand RmXqTradeItemCmd
        {
            get { return rmXqTradeItemCmd; }
            set { SetProperty(ref rmXqTradeItemCmd, value); }
        }

        private ICommand toUploadAllPreviewItemsCmd;
        public ICommand ToUploadAllPreviewItemsCmd
        {
            get { return toUploadAllPreviewItemsCmd; }
            set { SetProperty(ref toUploadAllPreviewItemsCmd, value); }
        }

        private bool isUploading;
        public bool IsUploading
        {
            get { return isUploading; }
            set { SetProperty(ref isUploading, value); }
        }

    }
}
