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
using CefSharp;

namespace ContainerShell.Presentations.Views
{
    /// <summary>
    /// UserMessageContentView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IUserMessageContentView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class UserMessageContentView : IUserMessageContentView
    {
        private const string UserMessageHtmlContentCefAddress = "http://www.xueqiao.cn/trading/usermessage-html-content-cef-address.html";
        
        public UserMessageContentView()
        {
            InitializeComponent();
        }
        
        public void UpdateContentWithHtml(string htmlContent)
        {
            this.WebBrowser?.LoadHtml(htmlContent, UserMessageHtmlContentCefAddress);
        }
    }
}
