using Manage.Applications.DataModels;
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

namespace Manage.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class PersonalUserTradeAccountManageVM : ViewModel<IPersonalUserTradeAccountManageView>
    {
        [ImportingConstructor]
        public PersonalUserTradeAccountManageVM(IPersonalUserTradeAccountManageView view) : base(view)
        {
            InnerTradeAccountItems = new ObservableCollection<FundAccountModel>();
            OutterTradeAccountItems = new ObservableCollection<FundAccountModel>();
        }

        private ICommand addInnerTradeAccountCmd;
        public ICommand AddInnerTradeAccountCmd
        {
            get { return addInnerTradeAccountCmd; }
            set { SetProperty(ref addInnerTradeAccountCmd, value); }
        }

        private ICommand addOutterTradeAccountCmd;
        public ICommand AddOutterTradeAccountCmd
        {
            get { return addOutterTradeAccountCmd; }
            set { SetProperty(ref addOutterTradeAccountCmd, value); }
        }

        private ICommand dataRefreshCmd;
        public ICommand DataRefreshCmd
        {
            get { return dataRefreshCmd; }
            set { SetProperty(ref dataRefreshCmd, value); }
        }

        private ICommand editAccountCmd;    
        public ICommand EditAccountCmd
        {
            get { return editAccountCmd; }
            set { SetProperty(ref editAccountCmd, value); }
        }

        private ICommand showAccountExtraInfoCmd;
        public ICommand ShowAccountExtraInfoCmd
        {
            get { return showAccountExtraInfoCmd; }
            set { SetProperty(ref showAccountExtraInfoCmd, value); }
        }
        
        public ObservableCollection<FundAccountModel> InnerTradeAccountItems { get; private set; }

        public ObservableCollection<FundAccountModel> OutterTradeAccountItems { get; private set; }
    }
}
