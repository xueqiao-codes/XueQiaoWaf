using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace XueQiaoFoundation.Shared.Model
{
    /// <summary>
    /// app 的模块 tab。比如（交易、管理、投研）
    /// </summary>
    public class AppModuleTabNode : System.Waf.Foundation.Model
    {
        public Action<AppModuleTabNode> ShowAction { get; set; }

        public Action<AppModuleTabNode> CloseAction { get; set; }

        private string nodeTitle;
        public string NodeTitle
        {
            get { return nodeTitle; }
            set { SetProperty(ref nodeTitle, value); }
        }

        private Geometry nodeIconGeometry;
        public Geometry NodeIconGeometry
        {
            get { return nodeIconGeometry; }
            set { SetProperty(ref nodeIconGeometry, value); }
        }
    }
}
