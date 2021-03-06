using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Security;
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
    /// CompanyUserLoginOperateView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(ICompanyUserLoginOperateView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class CompanyUserLoginOperateView : ICompanyUserLoginOperateView
    {
        public CompanyUserLoginOperateView()
        {
            InitializeComponent();
        }
        
        public SecureString SecurePassword
        {
            get
            {
                return this.PasswordBox?.SecurePassword;
            }
        }
    }
}
