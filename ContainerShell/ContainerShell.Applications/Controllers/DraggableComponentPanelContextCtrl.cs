using ContainerShell.Interfaces.Applications;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using XueQiaoFoundation.Shared.Model;

namespace ContainerShell.Applications.Controllers
{
    [Export, Export(typeof(IDraggableComponentPanelContextCtrl)), PartCreationPolicy(CreationPolicy.NonShared)]
    internal class DraggableComponentPanelContextCtrl : IDraggableComponentPanelContextCtrl
    {
        [ImportingConstructor]
        public DraggableComponentPanelContextCtrl()
        {
        }

        public DraggableComponentPanelContext ComponentPanelContext { get; set; }

        public void Initialize()
        {
            if (ComponentPanelContext == null) throw new ArgumentNullException("ComponentPanelContext");
            ComponentPanelContext.MouseDownWithinComponent += ComponentPanelContext_MouseDownWithinComponent;
        }
        
        public void Shutdown()
        {
            ComponentPanelContext.MouseDownWithinComponent -= ComponentPanelContext_MouseDownWithinComponent;
        }

        /// <summary>
        /// 添加一个组件
        /// </summary>
        /// <param name="componentUIItem"></param>
        public void AddComponent(DraggableComponentUIDM componentUIItem)
        {
            ComponentPanelContext.ComponentItems.Add(componentUIItem);
        }

        /// <summary>
        /// 删除一个组件
        /// </summary>
        /// <param name="componentUIItem"></param>
        public void RemoveComponent(DraggableComponentUIDM componentUIItem)
        {
            ComponentPanelContext.ComponentItems.Remove(componentUIItem);
        }

        /// <summary>
        /// 聚焦到某个组件
        /// </summary>
        public void FocusOnComponent(DraggableComponentUIDM componentUIItem, bool deselectOtherComponents = true)
        {
            if (!ComponentPanelContext.ComponentItems.Contains(componentUIItem))
                return;
            componentUIItem.Component.ZIndex = ShuffleComponentsZIndex();
            componentUIItem.IsPicked = true;
            ComponentPanelContext.ComponentItemBringIntoView(componentUIItem);
            if (deselectOtherComponents)
            {
                foreach (var item in ComponentPanelContext.ComponentItems)
                {
                    if (item != componentUIItem)
                    {
                        item.IsPicked = false;
                    }
                }
            }
        }

        /// <summary>
        /// 设置组件的位置。location 为 null 时，居中放置
        /// </summary>
        /// <param name="location"></param>
        public void PositionComponent(DraggableComponentUIDM componentUIItem, Point? location)
        {
            var presentAreaRect = ComponentPanelContext.DraggableComponentPanelPresentAreaRect;
            if (location == null)
            {
                componentUIItem.Component.Left = presentAreaRect.X + (presentAreaRect.Width - componentUIItem.Component.Width) / 2;
                componentUIItem.Component.Top = presentAreaRect.Y + (presentAreaRect.Height - componentUIItem.Component.Height) / 2;
            }
            else
            {
                componentUIItem.Component.Left = presentAreaRect.X + location.Value.X;
                componentUIItem.Component.Top = presentAreaRect.Y + location.Value.Y;
            }
        }

        /// <summary>
        /// 重置给定组件列表的 Zindex 值，并返回一个大于它们的值
        /// </summary>
        /// <param name="components">给定的组件列表</param>
        /// <returns></returns>
        private int ShuffleComponentsZIndex()
        {
            var components = ComponentPanelContext.ComponentItems.ToArray();
            if (components == null) return 0;
            // 让新加的组件 ZIndex 最大值
            var originComponentsCount = components.Count();
            var zIndex = originComponentsCount;
            foreach (var source in components.OrderByDescending(i => i.Component.ZIndex))
            {
                source.Component.ZIndex = --zIndex;
            }
            return originComponentsCount;
        }
        
        private void ComponentPanelContext_MouseDownWithinComponent(DraggableComponentUIDM obj)
        {
            obj.IsPicked = true;

            // deselect others
            foreach (var item in ComponentPanelContext.ComponentItems)
            {
                if (item != obj)
                {
                    item.IsPicked = false;
                }
            }
        }
    }
}
