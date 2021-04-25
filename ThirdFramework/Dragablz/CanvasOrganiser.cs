using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Dragablz
{
    public class CanvasOrganiser : IItemsOrganiser
    {
        /// <summary>
        /// 是否放在 ScrollViewer 中。在scrollViewer 中的长宽计算不一样
        /// </summary>
        public bool IsHostInScrollViewer { get; set; }

        public virtual void Organise(DragablzItemsControl requestor, Size measureBounds, IEnumerable<DragablzItem> items)
        {
            OrganiseInternal(requestor, measureBounds, items);
        }

        public virtual void Organise(DragablzItemsControl requestor, Size measureBounds, IOrderedEnumerable<DragablzItem> items)
        {
            OrganiseInternal(requestor, measureBounds, items);
        }

        private void OrganiseInternal(DragablzItemsControl requestor, Size measureBounds,
            IEnumerable<DragablzItem> items)
        {
            if (IsHostInScrollViewer && items?.Any() == true)
            {
                
                // 限制在 x >= 0, y >= 0 的范围内 
                foreach (var i in items)
                {
                    if (i.X < 0)
                    {
                        i.X = 0;
                    }
                    if (i.Y < 0)
                    {
                        i.Y = 0;
                    }
                }
            }
        }

        public virtual void OrganiseOnMouseDownWithin(DragablzItemsControl requestor, Size measureBounds, List<DragablzItem> siblingItems, DragablzItem dragablzItem)
        {
            var siblingItemsCount = siblingItems.Count;
            var zIndex = siblingItemsCount;
            foreach (var source in siblingItems.OrderByDescending(Panel.GetZIndex))
            {
                Panel.SetZIndex(source, --zIndex);

            }
            Panel.SetZIndex(dragablzItem, siblingItemsCount);
        }

        public virtual void OrganiseOnDragStarted(DragablzItemsControl requestor, Size measureBounds, IEnumerable<DragablzItem> siblingItems, DragablzItem dragItem)
        {

        }

        public virtual void OrganiseOnDrag(DragablzItemsControl requestor, Size measureBounds, IEnumerable<DragablzItem> siblingItems, DragablzItem dragItem)
        {
            if (IsHostInScrollViewer)
            {
                // 规定不能向上和向左拖拽
                if ((dragItem.Y + dragItem.ActualHeight) > requestor.ItemsPresenterHeight)
                {
                    requestor.ItemsPresenterHeight = dragItem.Y + dragItem.ActualHeight;
                }

                if ((dragItem.X + dragItem.ActualWidth) > requestor.ItemsPresenterWidth)
                {
                    requestor.ItemsPresenterWidth = dragItem.X + dragItem.ActualWidth;
                }
            }
            else
            {
                dragItem.BringIntoView();
            }
        }

        public virtual void OrganiseOnDragCompleted(DragablzItemsControl requestor, Size measureBounds,
            IEnumerable<DragablzItem> siblingItems, DragablzItem dragItem,
            double? dragItemHorizontalChange, double? dragItemVerticalChange)
        {
            if (IsHostInScrollViewer)
            {
                if ((dragItemHorizontalChange != null && dragItemHorizontalChange != 0)
                  || (dragItemVerticalChange != null && dragItemVerticalChange != 0))
                {
                    dragItem.BringIntoView();
                }
            }
        }

        public virtual Point ConstrainLocation(DragablzItemsControl requestor, Size measureBounds, Point itemCurrentLocation, Size itemCurrentSize, Point itemDesiredLocation, Size itemDesiredSize)
        {
            if (!IsHostInScrollViewer)
            {
                // we will stop it pushing beyond the bounds...unless it's already beyond...
                var reduceBoundsWidth = itemCurrentLocation.X + itemCurrentSize.Width > measureBounds.Width
                    ? 0
                    : itemDesiredSize.Width;
                var reduceBoundsHeight = itemCurrentLocation.Y + itemCurrentSize.Height > measureBounds.Height
                    ? 0
                    : itemDesiredSize.Height;

                return new Point(
                    Math.Min(Math.Max(itemDesiredLocation.X, 0), measureBounds.Width - reduceBoundsWidth),
                    Math.Min(Math.Max(itemDesiredLocation.Y, 0), measureBounds.Height - reduceBoundsHeight));
            }
            else
            {
                // 规定不能向上和向左拖拽
                // 向右只能移动到控件宽的2倍和当前显示宽度间的最大值
                // 向下只能移动到控件高的2倍和当前显示高度间的最大值
                Point tarPoint = new Point(itemDesiredLocation.X, itemDesiredLocation.Y);
                var containerControlActualWidth = requestor.ActualWidth;
                var containerControlActualHeight = requestor.ActualHeight;
                
                {
                    double desireX = 0;
                    if (itemDesiredLocation.X <= 0)
                    {
                        desireX = 0;
                    }
                    else
                    {
                        var limitWidth = Math.Max(containerControlActualWidth * 2, requestor.ItemsPresenterWidth);
                        var limitX = limitWidth - itemDesiredSize.Width;
                        if (limitX < 0) limitX = 0;
                        desireX = (itemDesiredLocation.X > limitX) ? limitX : itemDesiredLocation.X;
                    }
                    tarPoint.X = desireX;
                }

                {
                    double desireY = 0;
                    if (itemDesiredLocation.Y <= 0)
                    {
                        desireY = 0;
                    }
                    else
                    {
                        var limitHeight = Math.Max(containerControlActualHeight * 2, requestor.ItemsPresenterHeight);
                        var limitY = limitHeight - itemDesiredSize.Height;
                        if (limitY < 0) limitY = 0;
                        desireY = (itemDesiredLocation.Y > limitY) ? limitY : itemDesiredLocation.Y;
                    }
                    tarPoint.Y = desireY;
                }

                //Console.WriteLine($"containerControlActualWidth:{containerControlActualWidth}, " +
                //    $"containerControlActualHeight:{containerControlActualHeight}," +
                //    $" tarPoint:{tarPoint}");

                return tarPoint;
            }
        }

        public virtual Size Measure(DragablzItemsControl requestor, Size availableSize, IEnumerable<DragablzItem> items)
        {
            if (!IsHostInScrollViewer)
            {
                return availableSize;
            }
            else
            {
                double maxRightPoint = 0;
                double maxBottomPoint = 0;
                if (items.Any())
                {
                    maxRightPoint = items.Max(i => i.X + i.ActualWidth);
                    maxBottomPoint = items.Max(i => i.Y + i.ActualHeight);
                }
                else
                {
                    maxRightPoint = availableSize.Width;
                    maxBottomPoint = availableSize.Height;
                }
                maxRightPoint = Math.Max(requestor.ItemsPresenterWidth, Math.Max(maxRightPoint, availableSize.Width));
                maxBottomPoint = Math.Max(requestor.ItemsPresenterHeight, Math.Max(maxBottomPoint, availableSize.Height));
                return new Size(maxRightPoint, maxBottomPoint);
            }
        }

        public virtual IEnumerable<DragablzItem> Sort(IEnumerable<DragablzItem> items)
        {
            return items;
        }
    }
}