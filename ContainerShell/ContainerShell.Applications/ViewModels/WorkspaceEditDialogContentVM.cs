using ContainerShell.Applications.Domain;
using ContainerShell.Applications.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;

namespace ContainerShell.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class WorkspaceEditDialogContentVM : ViewModel<IWorkspaceEditDialogContentView>
    {
        [ImportingConstructor]
        protected WorkspaceEditDialogContentVM(IWorkspaceEditDialogContentView view) : base(view)
        {
        }

        public object DisplayInWindow => ViewCore.DisplayInWindow;

        public void CloseDisplayInWindow()
        {
            ViewCore.CloseDisplayInWindow();
        }

        private EditWorkspace editWorkspace;
        public EditWorkspace EditWorkspace
        {
            get { return editWorkspace; }
            set { SetProperty(ref editWorkspace, value); }
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
