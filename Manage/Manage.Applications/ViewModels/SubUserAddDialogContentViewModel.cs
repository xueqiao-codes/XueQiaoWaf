using Manage.Applications.Domain;
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
    public class SubUserAddDialogContentViewModel : ViewModel<ISubUserAddDialogContentView>
    {
        [ImportingConstructor]
        public SubUserAddDialogContentViewModel(ISubUserAddDialogContentView view) : base(view)
        {
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

        private AddSubUser addUser;
        public AddSubUser AddSubUser
        {
            get { return addUser; }
            set { SetProperty(ref addUser, value); }
        }

        public object DisplayInWindow => ViewCore.DisplayInWindow;

        public void CloseDisplayInWindow()
        {
            ViewCore.CloseDisplayInWindow();
        }

    }
}
