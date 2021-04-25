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

namespace Research.app.View
{
    /// <summary>
    /// UrlComponentContentView.xaml 的交互逻辑
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class UrlComponentContentView : IView
    {
        public UrlComponentContentView()
        {
            InitializeComponent();
            this.WebBrowser.IsBrowserInitializedChanged += WebBrowser_IsBrowserInitializedChanged;
        }

        private void WebBrowser_IsBrowserInitializedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // TODO: 在拖拽工作空间时，会引发 IsBrowserInitializedChanged
            Console.WriteLine($"WebBrowser_IsBrowserInitializedChanged. IsBrowserInitialized:{e.NewValue}");
        }

        /// <summary>
        /// 加载到某个 web 页面
        /// </summary>
        /// <param name="uri"></param>
        public void WebBrowserNavigate(string address)
        {
            this.WebBrowser.Address = address;
        }

        /// <summary>
        /// 刷新当前 web 页面
        /// </summary>
        public void RefreshWebBrowser()
        {
            if (this.WebBrowser?.IsInitialized == true && this.WebBrowser?.IsDisposed == false)
            {
                this.WebBrowser?.GetBrowser()?.Reload();
            }
        }
    }
}
