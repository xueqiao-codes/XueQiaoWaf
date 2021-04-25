using Research.app.View;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;

namespace Research.app.ViewModel
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class UrlComponentContentVM : ViewModel<UrlComponentContentView>
    {
        [ImportingConstructor]
        protected UrlComponentContentVM(UrlComponentContentView view) : base(view)
        {
        }

        /// <summary>
        /// 加载到某个 web 页面
        /// </summary>
        /// <param name="uri"></param>
        public void WebBrowserNavigate(string address)
        {
            this.ViewCore.WebBrowserNavigate(address);
        }

        /// <summary>
        /// 刷新当前 web 页面
        /// </summary>
        public void RefreshWebBrowser()
        {
            this.ViewCore.RefreshWebBrowser();
        }
    }
}
