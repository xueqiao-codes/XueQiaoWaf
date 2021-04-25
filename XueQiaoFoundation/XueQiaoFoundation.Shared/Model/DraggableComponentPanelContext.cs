using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using System.Windows;

namespace XueQiaoFoundation.Shared.Model
{
    /// <summary>
    /// 可拉动组件面板视图的 data context 类
    /// </summary>
    public class DraggableComponentPanelContext : System.Waf.Foundation.Model
    {
        private readonly IDraggableComponentPanelViewBase view;

        public DraggableComponentPanelContext(IDraggableComponentPanelViewBase view)
        {
            this.view = view;
            ComponentItems = new ObservableCollection<DraggableComponentUIDM>();
        }

        /// <summary>
        /// 组件接收到 Mouse down 事件
        /// </summary>
        public event Action<DraggableComponentUIDM> MouseDownWithinComponent;

        /// <summary>
        /// 组件列表
        /// </summary>
        public ObservableCollection<DraggableComponentUIDM> ComponentItems { get; }

        /// <summary>
        /// 发布组件鼠标按下事件
        /// </summary>
        /// <param name="optItem">操作的项</param>
        public void PublishMouseDownEventWithinComponent(object optItem)
        {
            var currentItem = optItem as DraggableComponentUIDM;
            if (currentItem != null)
            {
                MouseDownWithinComponent?.Invoke(currentItem);
            }
        }

        /// <summary>
        /// 当前显示的区域
        /// </summary>
        public Rect DraggableComponentPanelPresentAreaRect => view.DraggableComponentPanelPresentAreaRect;

        /// <summary>
        /// 显示出某个组件。如果有滚动视图，则会滚动到该交易组件
        /// </summary>
        /// <param name="componentItem">目标交易组件</param>
        public void ComponentItemBringIntoView(DraggableComponentUIDM componentItem)
        {
            view.ComponentItemBringIntoView(componentItem);
        }
    }
}
