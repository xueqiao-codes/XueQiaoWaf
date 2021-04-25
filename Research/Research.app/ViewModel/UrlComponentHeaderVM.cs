using Research.app.View;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;

namespace Research.app.ViewModel
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class UrlComponentHeaderVM : ViewModel<UrlComponentHeaderView>
    {
        [ImportingConstructor]
        protected UrlComponentHeaderVM(UrlComponentHeaderView view) : base(view)
        {
        }

        private string urlText;
        public string UrlText
        {
            get { return urlText; }
            set { SetProperty(ref urlText, value); }
        }

        private ICommand navigateToUrlCmd;
        public ICommand NavigateToUrlCmd
        {
            get { return navigateToUrlCmd; }
            set { SetProperty(ref navigateToUrlCmd, value); }
        }

        private ICommand refreshUrlCmd;
        public ICommand RefreshUrlCmd
        {
            get { return refreshUrlCmd; }
            set { SetProperty(ref refreshUrlCmd, value); }
        }

        private ICommand closeComponentCommand;
        public ICommand CloseComponentCommand
        {
            get { return closeComponentCommand; }
            set { SetProperty(ref closeComponentCommand, value); }
        }

        /// <summary>
        /// 是否显示 url 信息
        /// </summary>
        private bool isShowUrlInfo = true;
        public bool IsShowUrlInfo
        {
            get { return isShowUrlInfo; }
            set { SetProperty(ref isShowUrlInfo, value); }
        }
    }
}
