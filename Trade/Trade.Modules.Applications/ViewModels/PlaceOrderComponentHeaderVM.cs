using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class PlaceOrderComponentHeaderVM : ViewModel<IPlaceOrderComponentHeaderView>
    {
        [ImportingConstructor]
        public PlaceOrderComponentHeaderVM(
            IPlaceOrderComponentHeaderView view) : base(view)
        {
        }
        
        private ICommand showOrHideChartPartCommand;
        public ICommand ShowOrHideChartPartCommand
        {
            get { return showOrHideChartPartCommand; }
            set { SetProperty(ref showOrHideChartPartCommand, value); }
        }

        private ICommand showOrHidePlaceOrderPartCommand;
        public ICommand ShowOrHidePlaceOrderPartCommand
        {
            get { return showOrHidePlaceOrderPartCommand; }
            set { SetProperty(ref showOrHidePlaceOrderPartCommand, value); }
        }
        
        private XueQiaoFoundation.BusinessResources.DataModels.TradeComponent componentInfo;
        public XueQiaoFoundation.BusinessResources.DataModels.TradeComponent ComponentInfo
        {
            get { return componentInfo; }
            set { SetProperty(ref componentInfo, value); }
        }

        private bool existAttachTarget;
        // 是否存在附着标的
        public bool ExistAttachTarget
        {
            get { return existAttachTarget; }
            set { SetProperty(ref existAttachTarget, value); }
        }

        private ICommand triggerSelectContractCmd;
        public ICommand TriggerSelectContractCmd
        {
            get { return triggerSelectContractCmd; }
            set { SetProperty(ref triggerSelectContractCmd, value); }
        }

        private ICommand triggerSelectComposeCmd;
        public ICommand TriggerSelectComposeCmd
        {
            get { return triggerSelectComposeCmd; }
            set { SetProperty(ref triggerSelectComposeCmd, value); }
        }
    }
}
