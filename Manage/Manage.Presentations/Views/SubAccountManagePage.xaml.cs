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
    /// SubAccountManagePage.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(ISubAccountManagePage)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class SubAccountManagePage : ISubAccountManagePage
    {
        public SubAccountManagePage()
        {
            InitializeComponent();
        }

        public object DisplayInWindow => Window.GetWindow(this);
    }
}
