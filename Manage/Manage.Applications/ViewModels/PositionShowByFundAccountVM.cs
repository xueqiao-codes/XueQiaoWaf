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
    public class PositionShowByFundAccountVM : ViewModel<IPositionShowByFundAccountView>
    {
        [ImportingConstructor]
        protected PositionShowByFundAccountVM(IPositionShowByFundAccountView view) : base(view)
        {
            FundAccountItems = new ObservableCollection<HostingTradeAccount>();
            PositionItems = new ObservableCollection<PositionManageDM>();
        }

        public ObservableCollection<HostingTradeAccount> FundAccountItems { get; private set; }

        private HostingTradeAccount selectedFundAccountItem;
        public HostingTradeAccount SelectedFundAccountItem
        {
            get { return selectedFundAccountItem; }
            set { SetProperty(ref selectedFundAccountItem, value); }
        }

        private ICommand refreshDataCmd;
        public ICommand RefreshDataCmd
        {
            get { return refreshDataCmd; }
            set { SetProperty(ref refreshDataCmd, value); }
        }

        private ICommand toShowHistoryCmd;
        public ICommand ToShowHistoryCmd
        {
            get { return toShowHistoryCmd; }
            set { SetProperty(ref toShowHistoryCmd, value); }
        }

        public ObservableCollection<PositionManageDM> PositionItems { get; private set; }
    }
}
