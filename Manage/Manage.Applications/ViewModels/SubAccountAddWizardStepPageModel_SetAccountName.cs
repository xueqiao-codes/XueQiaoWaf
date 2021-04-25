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
using System.Windows.Navigation;

namespace Manage.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class SubAccountAddWizardStepPageModel_SetAccountName : ViewModel<ISubAccountAddWizardStepPage_SetAccountName>
    {
        [ImportingConstructor]
        protected SubAccountAddWizardStepPageModel_SetAccountName(ISubAccountAddWizardStepPage_SetAccountName view) : base(view)
        {
        }

        private WizardStepItem wizardStepItem;
        public WizardStepItem WizardStepItem
        {
            get { return wizardStepItem; }
            set { SetProperty(ref wizardStepItem, value); }
        }
        
        private SubAccountAddWizardStepData_SetAccountName stepData;
        public SubAccountAddWizardStepData_SetAccountName StepData
        {
            get { return stepData; }
            set { SetProperty(ref stepData, value); }
        }

        private ICommand saveCmd;
        public ICommand SaveCmd
        {
            get { return saveCmd; }
            set { SetProperty(ref saveCmd, value); }
        }

        public void ForwardToNextStep(PageFunction<int> nextStepPage)
        {
            ViewCore.ForwardToNextStep(nextStepPage);
        }
    }
}
