using Manage.Applications.DataModels;
using Manage.Applications.Domain;
using Manage.Applications.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;
using xueqiao.broker;

namespace Manage.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class FundAccounEditDialogContentViewModel : ViewModel<IFundAccountEditDialogContentView>
    {
        [ImportingConstructor]
        protected FundAccounEditDialogContentViewModel(IFundAccountEditDialogContentView view) : base(view)
        {
            AvailableAccessEntries = new ObservableCollection<BrokerAccessEntry>();
        }

        private ICommand okCmd;
        public ICommand OkCmd
        {
            get { return okCmd; }
            set { SetProperty(ref okCmd, value); }
        }

        private ICommand cancelCmd;
        public ICommand CancelCmd
        {
            get { return cancelCmd; }
            set { SetProperty(ref cancelCmd, value); }
        }

        public ObservableCollection<BrokerAccessEntry> AvailableAccessEntries { get; private set; }

        private BrokerAccessEntry selectedAccessEntry;
        public BrokerAccessEntry SelectedAccessEntry
        {
            get { return selectedAccessEntry; }
            set { SetProperty(ref selectedAccessEntry, value); }
        }
        
        private FundAccountModel accountInfo;
        public FundAccountModel AccountInfo
        {
            get { return accountInfo; }
            set { SetProperty(ref accountInfo, value); }
        }

        private EditFundAccount editAccount;
        public EditFundAccount EditAccount
        {
            get { return editAccount; }
            set { SetProperty(ref editAccount, value); }
        }

        private string editPassword;
        public string EditPassword
        {
            get { return editPassword; }
            set { SetProperty(ref editPassword, value); }
        }

    }
}
