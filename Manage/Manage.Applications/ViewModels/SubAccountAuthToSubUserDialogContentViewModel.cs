using Manage.Applications.DataModels;
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
    public class SubAccountAuthToSubUserDialogContentViewModel : ViewModel<ISubAccountAuthToSubUserDialogContentView>
    {
        [ImportingConstructor]
        protected SubAccountAuthToSubUserDialogContentViewModel(ISubAccountAuthToSubUserDialogContentView view) : base(view)
        {
        }

        public object DisplayInWindow => ViewCore.DisplayInWindow;

        public void CloseDisplayInWindow()
        {
            ViewCore.CloseDisplayInWindow();
        }

        private SubAccountAuthToSubUsersCheckViewData subUsersCheckViewData;
        public SubAccountAuthToSubUsersCheckViewData SubUsersCheckViewData
        {
            get { return subUsersCheckViewData; }
            set { SetProperty(ref subUsersCheckViewData, value); }
        }

        private ICommand saveCmd;
        public ICommand SaveCmd
        {
            get { return saveCmd; }
            set { SetProperty(ref saveCmd, value); }
        }

        private ICommand cancelCmd;
        public ICommand CancelCmd
        {
            get { return cancelCmd; }
            set { SetProperty(ref cancelCmd, value); }
        }
        
    }
}
