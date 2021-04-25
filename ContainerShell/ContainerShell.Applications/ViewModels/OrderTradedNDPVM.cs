using ContainerShell.Applications.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.UI.Components.ToastNotification;

namespace ContainerShell.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class OrderTradedNDPVM : XqToastNotificationNDPVMBase<IOrderTradedNDP>
    {
        [ImportingConstructor]
        protected OrderTradedNDPVM(IOrderTradedNDP view) : base(view)
        {
        }

        private ICommand closeCmd;
        public ICommand CloseCmd
        {
            get { return closeCmd; }
            set { SetProperty(ref closeCmd, value); }
        }

        private TradeItemDataModel trade;
        public TradeItemDataModel Trade
        {
            get { return trade; }
            set { SetProperty(ref trade, value); }
        }
    }
}
