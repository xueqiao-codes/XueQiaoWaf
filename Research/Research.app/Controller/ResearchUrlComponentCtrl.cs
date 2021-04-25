using Research.app.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Model;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;

namespace Research.app.Controller
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class ResearchUrlComponentCtrl : IResearchComponentCtrl
    {
        private readonly UrlComponentHeaderVM compHeaderVM;
        private readonly UrlComponentContentVM compContentVM;
        private readonly IMessageWindowService messageWindowService;

        private readonly DelegateCommand navigateToBoxUrlCmd;
        private readonly DelegateCommand refreshUrlCmd;
        private readonly DelegateCommand closeCompCmd;

        [ImportingConstructor]
        public ResearchUrlComponentCtrl(
            UrlComponentHeaderVM compHeaderVM,
            UrlComponentContentVM compContentVM,
            IMessageWindowService messageWindowService)
        {
            this.compHeaderVM = compHeaderVM;
            this.compContentVM = compContentVM;
            this.messageWindowService = messageWindowService;

            navigateToBoxUrlCmd = new DelegateCommand(NavigateToBoxUrl);
            refreshUrlCmd = new DelegateCommand(RefreshUrl);
            closeCompCmd = new DelegateCommand(()=> CloseComponentHandler?.Invoke(this));
        }

        #region ITradeComponentController

        public ResearchComponent Component { get; set; }

        public TabWorkspace ParentWorkspace { get; set; }

        public Action<IResearchComponentCtrl> CloseComponentHandler { get; set; }

        /// <summary>
        /// 组件 data model，在 Initialize 后可获得
        /// </summary>
        public DraggableComponentUIDM ComponentItemDataModel { get; private set; }

        #endregion

        public void Initialize()
        {
            if (Component == null) throw new ArgumentNullException("`Component` can't be null before initialize.");
            if (ParentWorkspace == null) throw new ArgumentNullException("`ParentWorkspace` must be setted before initialize.");

            var urlCompDetail = Component.UrlCompDetail;
            if (urlCompDetail == null) throw new ArgumentNullException("Component.UrlCompDetail");
            
            this.ComponentItemDataModel = new DraggableComponentUIDM(Component, compHeaderVM.View, compContentVM.View)
            {
                ComponentMinWidth = 600,
                ComponentMinHeight = 600
            };

            compHeaderVM.UrlText = Component.UrlCompDetail.Url;
            compHeaderVM.NavigateToUrlCmd = navigateToBoxUrlCmd;
            compHeaderVM.RefreshUrlCmd = refreshUrlCmd;
            compHeaderVM.CloseComponentCommand = closeCompCmd;
        }

        public void Run()
        {

        }

        public void Shutdown()
        {
        }

        public void ShowUrlInfo()
        {
            compHeaderVM.IsShowUrlInfo = true;
        }

        public void HideUrlInfo()
        {
            compHeaderVM.IsShowUrlInfo = false;
        }

        public void LoadUrl(string url)
        {
            NavigateToUrl(url, false);
        }

        private void NavigateToBoxUrl()
        {
            var urlText = compHeaderVM.UrlText?.Trim() ?? "";
            NavigateToUrl(urlText, true);
        }

        private void RefreshUrl()
        {
            this.compContentVM.RefreshWebBrowser();
        }

        private void NavigateToUrl(string url, bool showUrlInvalidTip)
        {
            if (url == null) return;
            Uri uri = null;
            try
            {
                uri = new Uri(url);
            }
            catch (Exception)
            {
            }
            if (uri == null && showUrlInvalidTip)
            {
                messageWindowService.ShowMessageDialog(UIHelper.GetWindowOfUIElement(compContentVM.View), null, null, null, "请输入正确格式的 url");
                return;
            }

            this.compContentVM.WebBrowserNavigate(uri.AbsoluteUri);
            this.Component.UrlCompDetail.Url = uri.AbsoluteUri;
        }
    }
}
