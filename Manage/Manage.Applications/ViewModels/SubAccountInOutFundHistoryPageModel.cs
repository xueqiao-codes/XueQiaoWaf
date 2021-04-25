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
using xueqiao.trade.hosting.asset.thriftapi;

namespace Manage.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class SubAccountInOutFundHistoryPageModel : ViewModel<ISubAccountInOutFundHistoryPage>
    {
        [ImportingConstructor]
        protected SubAccountInOutFundHistoryPageModel(ISubAccountInOutFundHistoryPage view) : base(view)
        {
            InOutFundHistoryItems = new ObservableCollection<HostingSubAccountMoneyRecord>();
        }

        public ObservableCollection<HostingSubAccountMoneyRecord> InOutFundHistoryItems { get; private set; }

        private xueqiao.trade.hosting.HostingSubAccount subAccount;
        public xueqiao.trade.hosting.HostingSubAccount SubAccount
        {
            get { return subAccount; }
            set { SetProperty(ref subAccount, value); }
        }

        private ICommand goBackCmd;
        public ICommand GoBackCmd
        {
            get { return goBackCmd; }
            set { SetProperty(ref goBackCmd, value); }
        }
        
        private object pagerContentView;
        public object PagerContentView
        {
            get { return pagerContentView; }
            set { SetProperty(ref pagerContentView, value); }
        }
    }
}
