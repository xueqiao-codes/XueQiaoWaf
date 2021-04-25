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
using XueQiaoWaf.LoginUserManage.Modules.Applications.Views;

namespace XueQiaoWaf.LoginUserManage.Modules.Presentation.Views
{
    /// <summary>
    /// Link2TouyanUserView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(ILink2TouyanUserView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class Link2TouyanUserView : ILink2TouyanUserView
    {
        public Link2TouyanUserView()
        {
            InitializeComponent();
        }
    }
}
