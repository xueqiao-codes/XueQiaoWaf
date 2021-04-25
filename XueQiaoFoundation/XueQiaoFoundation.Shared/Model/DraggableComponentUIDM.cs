using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;

namespace XueQiaoFoundation.Shared.Model
{
    /// <summary>
    /// 可拖动的组件 ui data model
    /// </summary>
    public class DraggableComponentUIDM : System.Waf.Foundation.Model
    {
        public DraggableComponentUIDM(XQComponentBase component,
            object componentHeaderView,
            object componentContentView)
        {
            this.Component = component;
            this.ComponentHeaderView = componentHeaderView;
            this.ComponentContentView = componentContentView;
        }
        
        /// <summary>
        /// 组件信息
        /// </summary>
        public XQComponentBase Component { get; private set; }
        
        /// <summary>
        /// 组件的头部视图
        /// </summary>
        public object ComponentHeaderView { get; private set; }
        
        /// <summary>
        /// 组件的内容视图
        /// </summary>
        public object ComponentContentView { get; private set; }

        /// <summary>
        /// 组件最小宽度
        /// </summary>
        private double componentMinWidth;
        public double ComponentMinWidth
        {
            get { return componentMinWidth; }
            set { SetProperty(ref componentMinWidth, value); }
        }

        /// <summary>
        /// 组件最小高度
        /// </summary>
        private double componentMinHeight;
        public double ComponentMinHeight
        {
            get { return componentMinHeight; }
            set { SetProperty(ref componentMinHeight, value); }
        }

        /// <summary>
        /// 是否选中
        /// </summary>
        private bool isPicked;
        public bool IsPicked
        {
            get { return isPicked; }
            set { SetProperty(ref isPicked, value); }
        }
    }
}
