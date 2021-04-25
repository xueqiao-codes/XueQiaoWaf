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
using xueqiao.trade.hosting.asset.thriftapi;
using XueQiaoFoundation.BusinessResources.DataModels;

namespace Manage.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class SubAccountPositionDetailVM : ViewModel<ISubAccountPositionDetailView>
    {
        [ImportingConstructor]
        protected SubAccountPositionDetailVM(ISubAccountPositionDetailView view) : base(view)
        {
            PositionTradeDetails = new ObservableCollection<AssetTradeDetailDM>();
        }

        // 持仓成交明细
        public ObservableCollection<AssetTradeDetailDM> PositionTradeDetails { get; private set; }

        private HostingSubAccount subAccount;
        public HostingSubAccount SubAccount
        {
            get { return subAccount; }
            set { SetProperty(ref subAccount, value); }
        }
        
        private TargetContract_TargetContractDetail contractDetailContainer;
        public TargetContract_TargetContractDetail ContractDetailContainer
        {
            get { return contractDetailContainer; }
            set { SetProperty(ref contractDetailContainer, value); }
        }

        private ICommand toRefreshListCmd;
        public ICommand ToRefreshListCmd
        {
            get { return toRefreshListCmd; }
            set { SetProperty(ref toRefreshListCmd, value); }
        }

        private object pagerContentView;
        public object PagerContentView
        {
            get { return pagerContentView; }
            set { SetProperty(ref pagerContentView, value); }
        }
    }
}
