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
using xueqiao.trade.hosting;

namespace Manage.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class FundManageBySubAccountViewModel : ViewModel<IFundManageBySubAccountView>
    {
        [ImportingConstructor]
        protected FundManageBySubAccountViewModel(IFundManageBySubAccountView view) : base(view)
        {
            SubAccountItems = new ObservableCollection<HostingSubAccount>();
            TotalEquityItems = new ObservableCollection<SubAccountEquityModel>();
            CurrencyGroupedEquityItems = new ObservableCollection<SubAccountEquityModel>();
        }

        private ICommand toShowDailyHistoryCmd;
        public ICommand ToShowDailyHistoryCmd
        {
            get { return toShowDailyHistoryCmd; }
            set { SetProperty(ref toShowDailyHistoryCmd, value); }
        }

        private ICommand dataRefreshCmd;
        public ICommand DataRefreshCmd
        {
            get { return dataRefreshCmd; }
            set { SetProperty(ref dataRefreshCmd, value); }
        }

        private long? currentEquityDataUpdateTimestampMs;
        public long? CurrentEquityDataUpdateTimestampMs
        {
            get { return currentEquityDataUpdateTimestampMs; }
            set { SetProperty(ref currentEquityDataUpdateTimestampMs, value); }
        }

        public ObservableCollection<HostingSubAccount> SubAccountItems { get; private set; }

        private HostingSubAccount selectedSubAccountItem;
        public HostingSubAccount SelectedSubAccountItem
        {
            get { return selectedSubAccountItem; }
            set { SetProperty(ref selectedSubAccountItem, value); }
        }

        public ObservableCollection<SubAccountEquityModel> TotalEquityItems { get; private set; }

        public ObservableCollection<SubAccountEquityModel> CurrencyGroupedEquityItems { get; private set; }
    }
}
