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
    public class PositionVerifyManageVM : ViewModel<IPositionVerifyManageView>
    {
        [ImportingConstructor]
        protected PositionVerifyManageVM(IPositionVerifyManageView view) : base(view)
        {
            FundAccountItems = new ObservableCollection<HostingTradeAccount>();
        }

        private ModuleLockStatusDM moduleLockStatus;
        public ModuleLockStatusDM ModuleLockStatus
        {
            get { return moduleLockStatus; }
            set { SetProperty(ref moduleLockStatus, value); }
        }
        
        public ObservableCollection<HostingTradeAccount> FundAccountItems { get; private set; }

        private HostingTradeAccount selectedFundAccountItem;
        public HostingTradeAccount SelectedFundAccountItem
        {
            get { return selectedFundAccountItem; }
            set { SetProperty(ref selectedFundAccountItem, value); }
        }

        private ICommand refreshCurrentAccountDataCmd;
        public ICommand RefreshCurrentAccountDataCmd
        {
            get { return refreshCurrentAccountDataCmd; }
            set { SetProperty(ref refreshCurrentAccountDataCmd, value); }
        }

        private object settlementContentView;
        /// <summary>
        /// 结算内容视图
        /// </summary>
        public object SettlementContentView
        {
            get { return settlementContentView; }
            set { SetProperty(ref settlementContentView, value); }
        }
    }
}
