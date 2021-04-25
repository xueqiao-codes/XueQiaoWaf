using Manage.Applications.DataModels;
using Manage.Applications.Views;
using NativeModel.Trade;
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
    public class SubAccountAddWizardStepPageModel_SetInitialInFund : ViewModel<ISubAccountAddWizardStepPage_SetInitialInFund>
    {
        [ImportingConstructor]
        protected SubAccountAddWizardStepPageModel_SetInitialInFund(ISubAccountAddWizardStepPage_SetInitialInFund view) : base(view)
        {
            CurrencyItems = ClientFundCurrencyReference.AllClientFundCurrencyList.ToArray();
        }

        public string[] CurrencyItems { get; private set; }

        private WizardStepItem wizardStepItem;
        public WizardStepItem WizardStepItem
        {
            get { return wizardStepItem; }
            set { SetProperty(ref wizardStepItem, value); }
        }

        private SubAccountAddWizardStepData_SetInitialInFund stepData;
        public SubAccountAddWizardStepData_SetInitialInFund StepData
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
