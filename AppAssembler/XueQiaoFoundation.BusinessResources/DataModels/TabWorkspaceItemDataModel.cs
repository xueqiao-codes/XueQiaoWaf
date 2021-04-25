using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;

namespace XueQiaoFoundation.BusinessResources.DataModels
{
    public partial class TabWorkspaceItemDataModel : Model
    {
        public TabWorkspaceItemDataModel(TabWorkspace tabWorkspace)
        {
            this.TabWorkspace = tabWorkspace;
        }

        public TabWorkspace TabWorkspace { get; private set; }
        
        private object displayView;
        public object DisplayView
        {
            get { return displayView; }
            set { SetProperty(ref displayView, value); }
        }

        /// <summary>
        /// 缓存工作空间控制器对象，在处理拖拽时可用于复用
        /// </summary>
        public object CachedWorkspaceController { get; set; }
    }
}
