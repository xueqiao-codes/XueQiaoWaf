using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace XueQiaoFoundation.Shared.Helper
{
    public static class UIHelper
    {
        /// <summary>
        /// 获取UI元素的所在窗体
        /// </summary>
        /// <param name="uielement"></param>
        /// <param name="returnNullWhenWindowNotLoad">如果窗体 IsLoaded == false, 则返回null</param>
        /// <returns></returns>
        public static object GetWindowOfUIElement(object uielement, bool returnNullWhenWindowNotLoad = true)
        {
            if (uielement is DependencyObject dpo)
            {
                var win = Window.GetWindow(dpo);
                if (returnNullWhenWindowNotLoad && win != null && !win.IsLoaded)
                {
                    return null;
                }
                return win;
            }
            return null;
        }

        /// <summary>
        /// 转换为 WPF 的点
        /// </summary>
        /// <param name="pixelPoint">像素点</param>
        /// <param name="visualObj">wpf visual 对象</param>
        /// <returns></returns>
        public static Point TransformToWpfPoint(Point pixelPoint, Visual visualObj)
        {
            Matrix matrix;
            var source = PresentationSource.FromVisual(visualObj);
            if (source != null)
            {
                matrix = source.CompositionTarget.TransformToDevice;
            }
            else
            {
                using (var src = new HwndSource(new HwndSourceParameters()))
                {
                    matrix = src.CompositionTarget.TransformToDevice;
                }
            }

            var unitX = pixelPoint.X / matrix.M11;
            var unitY = pixelPoint.Y / matrix.M22;
            return new Point(unitX, unitY);
        }
    }
}
