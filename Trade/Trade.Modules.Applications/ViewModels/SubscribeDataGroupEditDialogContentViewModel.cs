using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;
using XueQiaoWaf.Trade.Modules.Applications.Views;
using XueQiaoWaf.Trade.Modules.Domain.Trades;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class SubscribeDataGroupEditDialogContentViewModel : ViewModel<ISubscribeDataGroupEditDialogContentView>
    {
        [ImportingConstructor]
        protected SubscribeDataGroupEditDialogContentViewModel(ISubscribeDataGroupEditDialogContentView view) : base(view)
        {
        }

        public object DisplayInWindow => ViewCore.DisplayInWindow;

        public void CloseDisplayInWindow()
        {
            ViewCore.CloseDisplayInWindow();
        }

        private EditSubscribeDataGroup editGroup;
        public EditSubscribeDataGroup EditGroup
        {
            get { return editGroup; }
            set { SetProperty(ref editGroup, value); }
        }

        private ICommand okCmd;
        public ICommand OkCmd
        {
            get { return okCmd; }
            set { SetProperty(ref okCmd, value); }
        }

        private ICommand cancelCmd;
        public ICommand CancelCmd
        {
            get { return cancelCmd; }
            set { SetProperty(ref cancelCmd, value); }
        }
    }
}
