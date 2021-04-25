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
    public class SubAccountAddWizardStepPageModel_Finished : ViewModel<ISubAccountAddWizardStepPage_Finished>
    {
        [ImportingConstructor]
        protected SubAccountAddWizardStepPageModel_Finished(ISubAccountAddWizardStepPage_Finished view) : base(view)
        {
        }

        private ICommand doneCmd;
        public ICommand DoneCmd
        {
            get { return  doneCmd; }
            set { SetProperty(ref  doneCmd, value); }
        }

        public void FinishedWizard()
        {
            ViewCore.FinishedWizard();
        }
    }
}
