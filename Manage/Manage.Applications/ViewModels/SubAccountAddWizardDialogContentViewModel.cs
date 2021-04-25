using Manage.Applications.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Navigation;

namespace Manage.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class SubAccountAddWizardDialogContentViewModel : ViewModel<ISubAccountAddWizardDialogContentView>
    {
        [ImportingConstructor]
        protected SubAccountAddWizardDialogContentViewModel(ISubAccountAddWizardDialogContentView view) : base(view)
        {
        }

        public object DisplayInWindow => ViewCore.DisplayInWindow;

        public void CloseDisplayInWindow()
        {
            ViewCore.CloseDisplayInWindow();
        }

        public void Navigate(PageFunction<int> page, double? initialWidth, double? initialHeight, Action<int> wizardReturnHandler)
        {
            ViewCore.Navigate(page, initialWidth, initialHeight, wizardReturnHandler);
        }
    }
}
