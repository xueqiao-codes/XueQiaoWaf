using Manage.Applications.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;

namespace Manage.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class TradeManageTabContentViewModel : ViewModel<ITradeManageTabContentView>
    {
        [ImportingConstructor]
        protected TradeManageTabContentViewModel(ITradeManageTabContentView view) : base(view)
        {
        }

        private ICommand fundManageByFundAccountEntryCmd;
        public ICommand FundManageByFundAccountEntryCmd
        {
            get { return fundManageByFundAccountEntryCmd; }
            set { SetProperty(ref fundManageByFundAccountEntryCmd, value); }
        }

        private ICommand fundManageBySubAccountEntryCmd;
        public ICommand FundManageBySubAccountEntryCmd
        {
            get { return fundManageBySubAccountEntryCmd; }
            set { SetProperty(ref fundManageBySubAccountEntryCmd, value); }
        }

        private ICommand positionManageByFundAccountEntryCmd;
        public ICommand PositionManageByFundAccountEntryCmd
        {
            get { return positionManageByFundAccountEntryCmd; }
            set { SetProperty(ref positionManageByFundAccountEntryCmd, value); }
        }

        private ICommand positionManageBySubAccountEntryCmd;
        public ICommand PositionManageBySubAccountEntryCmd
        {
            get { return positionManageBySubAccountEntryCmd; }
            set { SetProperty(ref positionManageBySubAccountEntryCmd, value); }
        }

        private ICommand positionVerifyManageEntryCmd;
        public ICommand PositionVerifyManageEntryCmd
        {
            get { return positionVerifyManageEntryCmd; }
            set { SetProperty(ref positionVerifyManageEntryCmd, value); }
        }

        private ICommand notAssignTradeManageEntryCmd;
        public ICommand NotAssignTradeManageEntryCmd
        {
            get { return notAssignTradeManageEntryCmd; }
            set { SetProperty(ref notAssignTradeManageEntryCmd, value); }
        }
        

        private ICommand settlementFundAccountEntryCmd;
        public ICommand SettlementFundAccountEntryCmd
        {
            get { return settlementFundAccountEntryCmd; }
            set { SetProperty(ref settlementFundAccountEntryCmd, value); }
        }

        private ICommand settlementSubAccountEntryCmd;
        public ICommand SettlementSubAccountEntryCmd
        {
            get { return settlementSubAccountEntryCmd; }
            set { SetProperty(ref settlementSubAccountEntryCmd, value); }
        }

        private object entryContentView;
        public object EntryContentView
        {
            get { return entryContentView; }
            set { SetProperty(ref entryContentView, value); }
        }
    }
}
