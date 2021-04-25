using ContainerShell.Applications.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.UI.Components.ToastNotification;

namespace ContainerShell.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class TradeLameNDPVM : XqToastNotificationNDPVMBase<ITradeLameNDP>
    {
        [ImportingConstructor]
        protected TradeLameNDPVM(ITradeLameNDP view) : base(view)
        {
        }

        private ICommand closeCmd;
        public ICommand CloseCmd
        {
            get { return closeCmd; }
            set { SetProperty(ref closeCmd, value); }
        }

        private ICommand showDetailCmd;
        public ICommand ShowDetailCmd
        {
            get { return showDetailCmd; }
            set { SetProperty(ref showDetailCmd, value); }
        }
        
        private XQTradeLameTaskNote lameTaskNote;
        public XQTradeLameTaskNote LameTaskNote
        {
            get { return lameTaskNote; }
            set { SetProperty(ref lameTaskNote, value); }
        }
    }
}
