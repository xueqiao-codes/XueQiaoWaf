using Research.app.View;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;
using XueQiaoFoundation.Shared.Model;

namespace Research.app.ViewModel
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class ResearchWorkspaceVM : ViewModel<ResearchWorkspaceView>
    {
        [ImportingConstructor]
        protected ResearchWorkspaceVM(ResearchWorkspaceView view) : base(view)
        {
            this.DraggableComponentPanelContext = new DraggableComponentPanelContext(view);
        }

        /// <summary>
        /// 组件面板上下文
        /// </summary>
        public DraggableComponentPanelContext DraggableComponentPanelContext { get; private set; }

        private ICommand addUrlComponentCmd;
        public ICommand AddUrlComponentCmd
        {
            get { return addUrlComponentCmd; }
            set { SetProperty(ref addUrlComponentCmd, value); }
        }

        /// <summary>
        /// 加载所有 URL command
        /// </summary>
        private ICommand loadAllUrlsCmd;
        public ICommand LoadAllUrlsCmd
        {
            get { return loadAllUrlsCmd; }
            set { SetProperty(ref loadAllUrlsCmd, value); }
        }

        /// <summary>
        /// 隐藏所有 URL command
        /// </summary>
        private ICommand hideAllUrlsCmd;
        public ICommand HideAllUrlsCmd
        {
            get { return hideAllUrlsCmd; }
            set { SetProperty(ref hideAllUrlsCmd, value); }
        }

        /// <summary>
        /// 显示所有 URL command
        /// </summary>
        private ICommand showAllUrlsCmd;
        public ICommand ShowAllUrlsCmd
        {
            get { return showAllUrlsCmd; }
            set { SetProperty(ref showAllUrlsCmd, value); }
        }
    }
}
