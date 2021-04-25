using Manage.Applications.DataModels;
using Manage.Applications.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Manage.Presentations.Views
{
    /// <summary>
    /// SubAccountAddWizardStep1Page.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(ISubAccountAddWizardStepPage_SetAccountName)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class SubAccountAddWizardStepPage_SetAccountName : ISubAccountAddWizardStepPage_SetAccountName
    {
        public SubAccountAddWizardStepPage_SetAccountName()
        {
            InitializeComponent();
        }

        public void ForwardToNextStep(PageFunction<int> nextStepPage)
        {
            if (nextStepPage != null)
            {
                nextStepPage.Return -= NextStepPage_Return;
                nextStepPage.Return += NextStepPage_Return;
                NavigationService?.Navigate(nextStepPage);
            }
        }

        private void NextStepPage_Return(object sender, ReturnEventArgs<int> e)
        {
            // If returning, wizard was completed (finished or canceled),
            // so continue returning to calling page
            OnReturn(e);
        }
    }
}
