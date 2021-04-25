using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace XueQiaoFoundation.Shared.Model
{
    /// <summary>
    /// 可拖动组件面板视图基协议
    /// </summary>
    public interface IDraggableComponentPanelViewBase
    {
        /// <summary>
        /// 当前显示的区域
        /// </summary>
        Rect DraggableComponentPanelPresentAreaRect { get; }

        /// <summary>
        /// 显示出某个组件。如果有滚动视图，则会滚动到该交易组件
        /// </summary>
        /// <param name="componentItem">交易组件</param>
        void ComponentItemBringIntoView(object componentItem);
    }
}
