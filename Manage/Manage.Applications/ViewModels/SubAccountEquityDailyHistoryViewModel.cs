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
    public class SubAccountEquityDailyHistoryViewModel : ViewModel<ISubAccountEquityDailyHistoryView>
    {
        [ImportingConstructor]
        protected SubAccountEquityDailyHistoryViewModel(ISubAccountEquityDailyHistoryView view) : base(view)
        {
            TotalEquityItems = new ObservableCollection<SubAccountEquityModel>();
            CurrencyGroupedEquityItems = new ObservableCollection<SubAccountEquityModel>();
        }

        private HostingSubAccount subAccount;
        public HostingSubAccount SubAccount
        {
            get { return subAccount; }
            set { SetProperty(ref subAccount, value); }
        }

        public ObservableCollection<SubAccountEquityModel> TotalEquityItems { get; private set; }

        public ObservableCollection<SubAccountEquityModel> CurrencyGroupedEquityItems { get; private set; }

        private long? equityUpdateTimestampMs;
        public long? EquityUpdateTimestampMs
        {
            get { return equityUpdateTimestampMs; }
            set { SetProperty(ref equityUpdateTimestampMs, value); }
        }

        private DateTime? selectedDate;
        public DateTime? SelectedDate
        {
            get { return selectedDate; }
            set { SetProperty(ref selectedDate, value); }
        }

        private ICommand pageGoBackCmd;
        public ICommand PageGoBackCmd
        {
            get { return pageGoBackCmd; }
            set { SetProperty(ref pageGoBackCmd, value); }
        }

        private ICommand dataRefreshCmd;
        public ICommand DataRefreshCmd
        {
            get { return dataRefreshCmd; }
            set { SetProperty(ref dataRefreshCmd, value); }
        }
    }
}
