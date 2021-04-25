using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace Manage.Presentations.Views
{
    public class SubAccountAddWizardLauncher : PageFunction<int>
    {
        private readonly PageFunction<int> firstPage;
        private readonly double? initialWidth;
        private readonly double? initialHeight;

        public SubAccountAddWizardLauncher(PageFunction<int> firstPage, double? initialWidth, double? initialHeight)
        {
            this.firstPage = firstPage;
            this.initialWidth = initialWidth;
            this.initialHeight = initialHeight;
        }

        public Action<int> WizardReturnHandler;

        protected override void Start()
        {
            base.Start();

            // So we remember the WizardCompleted event registration
            KeepAlive = true;
            if (initialWidth != null) Width = initialWidth.Value;
            if (initialHeight != null) Height = initialHeight.Value;

            // Launch the wizard
            if (firstPage != null)
            {
                firstPage.Return -= firstPage_Return;
                firstPage.Return += firstPage_Return;
                NavigationService?.Navigate(firstPage);
            }
        }

        public void firstPage_Return(object sender, ReturnEventArgs<int> e)
        {
            // Notify client that wizard has completed
            // NOTE: We need this custom event because the Return event cannot be
            // registered by window code - if WizardDialogBox registers an event handler with
            // the WizardLauncher's Return event, the event is not raised.
            WizardReturnHandler?.Invoke(e.Result);
            OnReturn(null);
        }
    }
}
