using CefSharp;
using CefSharp.Wpf;
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

namespace Touyan.app.view
{
    /// <summary>
    /// ChartDetailContainerView.xaml 的交互逻辑
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class ChartDetailContainerView : IView
    {
        private const string ChartHtmlContentCefAddress = "http://www.xueqiao.cn/xqtouyan/chart-html-content-cef-address.html";

        public ChartDetailContainerView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 使用 url 方式加载图表内容
        /// </summary>
        /// <param name="url"></param>
        public void LoadChartWebContentWithUrl(string url)
        {
            var chartWebBrowser = this.ChartWebBrowser;
            if (CheckAvailable(chartWebBrowser))
            {
                chartWebBrowser.Load(url);
            }
        }
        
        /// <summary>
        /// 使用 html 内容方式加载图表内容
        /// </summary>
        /// <param name="htmlContent"></param>
        public void LoadChartWebContentWithHtmlContent(string htmlContent)
        {
            var chartWebBrowser = this.ChartWebBrowser;
            if (CheckAvailable(chartWebBrowser))
            {
                chartWebBrowser.LoadHtml(htmlContent, ChartHtmlContentCefAddress);
            }
        }

        private bool CheckAvailable(ChromiumWebBrowser webBrowser)
        {
            return webBrowser?.IsBrowserInitialized == true;
        }
    }
}
