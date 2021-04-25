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
    public class SubAccountRenameVM : ViewModel<ISubAccountRenameView>
    {
        [ImportingConstructor]
        protected SubAccountRenameVM(ISubAccountRenameView view) : base(view)
        {
        }

        private string editName;
        public string EditName
        {
            get { return editName; }
            set { SetProperty(ref editName, value); }
        }

        private ICommand confirmRenameCmd;
        public ICommand ConfirmRenameCmd
        {
            get { return confirmRenameCmd; }
            set { SetProperty(ref confirmRenameCmd, value); }
        }
    }
}
