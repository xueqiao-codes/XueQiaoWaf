using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class PositionDiscreteTradeDetailContentVM : ViewModel<IPositionDiscreteTradeDetailContentView>
    {
        [ImportingConstructor]
        protected PositionDiscreteTradeDetailContentVM(IPositionDiscreteTradeDetailContentView view) : base(view)
        {
            PositionTradeDetails = new ObservableCollection<AssetTradeDetailDM>();
        }
        
        private TargetContract_TargetContractDetail contractDetailContainer;
        public TargetContract_TargetContractDetail ContractDetailContainer
        {
            get { return contractDetailContainer; }
            set { SetProperty(ref contractDetailContainer, value); }
        }

        // 持仓成交明细
        public ObservableCollection<AssetTradeDetailDM> PositionTradeDetails { get; private set; }
        
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
