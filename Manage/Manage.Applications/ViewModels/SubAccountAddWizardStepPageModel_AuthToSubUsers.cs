using Manage.Applications.DataModels;
using Manage.Applications.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Manage.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class SubAccountAddWizardStepPageModel_AuthToSubUsers : ViewModel<ISubAccountAddWizardStepPage_AuthToSubUsers>
    {
        [ImportingConstructor]
        protected SubAccountAddWizardStepPageModel_AuthToSubUsers(ISubAccountAddWizardStepPage_AuthToSubUsers view) : base(view)
        {
        }

        private SubAccountAuthToSubUsersCheckViewData subUsersCheckViewData;
        public SubAccountAuthToSubUsersCheckViewData SubUsersCheckViewData
        {
            get { return subUsersCheckViewData; }
            set { SetProperty(ref subUsersCheckViewData, value); }
        }
        
        private WizardStepItem wizardStepItem;
        public WizardStepItem WizardStepItem
        {
            get { return wizardStepItem; }
            set { SetProperty(ref wizardStepItem, value); }
        }

        private ICommand saveCmd;
        public ICommand SaveCmd
        {
            get { return saveCmd; }
            set { SetProperty(ref saveCmd, value); }
        }

        private ICommand skipStepCmd;
        public ICommand SkipStepCmd
        {
            get { return skipStepCmd; }
            set { SetProperty(ref skipStepCmd, value); }
        }

        public void ForwardToNextStep(PageFunction<int> nextStepPage)
        {
            ViewCore.ForwardToNextStep(nextStepPage);
        }
    }
}
