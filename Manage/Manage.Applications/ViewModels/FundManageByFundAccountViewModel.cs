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
    public class FundManageByFundAccountViewModel : ViewModel<IFundManageByFundAccountView>
    {
        [ImportingConstructor]
        protected FundManageByFundAccountViewModel(IFundManageByFundAccountView view) : base(view)
        {
            FundAccountItems = new ObservableCollection<HostingTradeAccount>();
            TotalEquityItems = new ObservableCollection<FundAccountEquityModel>();
            CurrencyGroupedEquityItems = new ObservableCollection<FundAccountEquityModel>();
        }

        public object DisplayInWindow => ViewCore.DisplayInWindow;

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
        
        public ObservableCollection<HostingTradeAccount> FundAccountItems { get; private set; }

        private HostingTradeAccount selectedFundAccountItem;
        public HostingTradeAccount SelectedFundAccountItem
        {
            get { return selectedFundAccountItem; }
            set { SetProperty(ref selectedFundAccountItem, value); }
        }

        public ObservableCollection<FundAccountEquityModel> TotalEquityItems { get; private set; }

        public ObservableCollection<FundAccountEquityModel> CurrencyGroupedEquityItems { get; private set; }
    }
}
