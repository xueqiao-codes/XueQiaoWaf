using Manage.Applications.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
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
    /// PersonalUserTradeAccountManageView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IPersonalUserTradeAccountManageView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class PersonalUserTradeAccountManageView : IPersonalUserTradeAccountManageView
    {
        public PersonalUserTradeAccountManageView()
        {
            InitializeComponent();
        }
    }
}
