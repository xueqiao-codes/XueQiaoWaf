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
    public class FundAccountSettlementContainerVM : ViewModel<IFundAccountSettlementContainerView>
    {
        [ImportingConstructor]
        public FundAccountSettlementContainerVM(IFundAccountSettlementContainerView view) : base(view)
        {
            FundAccountItems = new ObservableCollection<HostingTradeAccount>();
        }
        
        public ObservableCollection<HostingTradeAccount> FundAccountItems { get; private set; }

        private HostingTradeAccount selectedFundAccountItem;
        public HostingTradeAccount SelectedFundAccountItem
        {
            get { return selectedFundAccountItem; }
            set { SetProperty(ref selectedFundAccountItem, value); }
        }


        private ICommand refreshSettlementCmd;
        public ICommand RefreshSettlementCmd
        {
            get { return refreshSettlementCmd; }
            set { SetProperty(ref refreshSettlementCmd, value); }
        }

        private long? settlementRefreshTimestampMs;
        public long? SettlementRefreshTimestampMs
        {
            get { return settlementRefreshTimestampMs; }
            set { SetProperty(ref settlementRefreshTimestampMs, value); }
        }

        /// <summary>
        /// 所选查询日期
        /// </summary>
        private DateTime? selectedDate;
        public DateTime? SelectedDate
        {
            get { return selectedDate; }
            set { SetProperty(ref selectedDate, value); }
        }

        /// <summary>
        /// 结算单信息
        /// </summary>
        private string settlementText;
        public string SettlementText
        {
            get { return settlementText; }
            set { SetProperty(ref settlementText, value); }
        }
    }
}
