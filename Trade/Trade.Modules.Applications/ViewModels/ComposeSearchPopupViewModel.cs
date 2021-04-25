using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class ComposeSearchPopupViewModel : ViewModel<IComposeSearchPopupView>
    {
        [ImportingConstructor]
        protected ComposeSearchPopupViewModel(IComposeSearchPopupView view) : base(view)
        {
            ComposeItems = new ObservableCollection<SubscribeComposeDataModel>();
        }

        public ObservableCollection<SubscribeComposeDataModel> ComposeItems { get; private set; }

        private SubscribeComposeDataModel selectedComposeItem;
        public SubscribeComposeDataModel SelectedComposeItem
        {
            get { return selectedComposeItem; }
            set { SetProperty(ref selectedComposeItem, value); }
        }
        
        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set { SetProperty(ref searchText, value); }
        }

        private ICommand confirmSelComposeCmd;
        public ICommand ConfirmSelComposeCmd
        {
            get { return confirmSelComposeCmd; }
            set { SetProperty(ref confirmSelComposeCmd, value); }
        }

        private ICommand selectPrevComposeCmd;
        public ICommand SelectPrevComposeCmd
        {
            get { return selectPrevComposeCmd; }
            set { SetProperty(ref selectPrevComposeCmd, value); }
        }

        private ICommand selectNextComposeCmd;
        public ICommand SelectNextComposeCmd
        {
            get { return selectNextComposeCmd; }
            set { SetProperty(ref selectNextComposeCmd, value); }
        }


        public void ScrollToComposeItemWithData(SubscribeComposeDataModel composeItem)
        {
            ViewCore.ScrollToComposeItemWithData(composeItem);
        }

        public void FocusSearchTextBox()
        {
            ViewCore.FocusSearchTextBox();
        }

        public event EventHandler Closed
        {
            add { ViewCore.Closed += value; }
            remove { ViewCore.Closed -= value; }
        }

        public void ShowPopup(object targetElement)
        {
            ViewCore.ShowPopup(targetElement);
        }

        public void Close()
        {
            ViewCore.Close();
        }
    }
}
