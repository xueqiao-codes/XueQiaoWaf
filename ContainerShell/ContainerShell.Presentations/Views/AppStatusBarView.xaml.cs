using ContainerShell.Applications.Views;
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

namespace ContainerShell.Presentations.Views
{
    /// <summary>
    /// AppStatusBarView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IAppStatusBarView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class AppStatusBarView : IAppStatusBarView
    {
        public AppStatusBarView()
        {
            InitializeComponent();
        }

        private void AppResourceShowButn_Click(object sender, RoutedEventArgs e)
        {
            AppResourcePopup.IsOpen = !AppResourcePopup.IsOpen;
        }

        private void LoginUserNameButton_Click(object sender, RoutedEventArgs e)
        {
            LoginUserExtraInfoPopup.IsOpen = !LoginUserExtraInfoPopup.IsOpen;
        }
    }
}
