using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using XueQiaoFoundation.Shared.Model;

namespace ContainerShell.Interfaces.Applications
{
    /// <summary>
    /// 可拖动组件面板视图上下文控制器协议
    /// </summary>
    public interface IDraggableComponentPanelContextCtrl
    {
        DraggableComponentPanelContext ComponentPanelContext { get; set; }

        void Initialize();

        void Shutdown();

        /// <summary>
        /// 添加一个组件
        /// </summary>
        /// <param name="componentUIItem"></param>
        void AddComponent(DraggableComponentUIDM componentUIItem);

        /// <summary>
        /// 删除一个组件
        /// </summary>
        /// <param name="componentUIItem"></param>
        void RemoveComponent(DraggableComponentUIDM componentUIItem);

        /// <summary>
        /// 聚焦到某个组件
        /// </summary>
        /// <param name="componentUIItem">要聚焦的组件</param>
        /// <param name="deselectOtherComponents">是否同时不选中其他组件</param>
        void FocusOnComponent(DraggableComponentUIDM componentUIItem, bool deselectOtherComponents = true);

        /// <summary>
        /// 设置组件的位置。location 为 null 时，居中放置
        /// </summary>
        /// <param name="location"></param>
        void PositionComponent(DraggableComponentUIDM componentUIItem, Point? location);
    }
}
