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
    /// SubAccountAddWizardStepPage_AuthToSubUsers.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(ISubAccountAddWizardStepPage_AuthToSubUsers)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class SubAccountAddWizardStepPage_AuthToSubUsers : ISubAccountAddWizardStepPage_AuthToSubUsers
    {
        public SubAccountAddWizardStepPage_AuthToSubUsers()
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
