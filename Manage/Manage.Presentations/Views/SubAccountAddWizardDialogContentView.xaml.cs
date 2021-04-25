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
    /// SubAccountAddWizardDialogContentView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(ISubAccountAddWizardDialogContentView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class SubAccountAddWizardDialogContentView : ISubAccountAddWizardDialogContentView
    {
        public SubAccountAddWizardDialogContentView()
        {
            InitializeComponent();
        }

        public object DisplayInWindow => Window.GetWindow(this);

        public void CloseDisplayInWindow()
        {
            if (this.DisplayInWindow is Window _win)
            {
                _win.Close();
            }
        }

        public void Navigate(PageFunction<int> page, double? initialWidth, double? initialHeight, Action<int> wizardReturnHandler)
        {
            if (page != null)
            {
                var launcher = new SubAccountAddWizardLauncher(page, initialWidth, initialHeight);
                launcher.WizardReturnHandler = wizardReturnHandler;
                this.WizardPageFrame.Navigate(launcher);
            }
        }
    }
}
