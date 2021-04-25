using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace XueQiaoFoundation.UI.Extra.helper
{
    public class TabablzControlHelper
    {
        public static double GetHeaderContainerHeight(DependencyObject obj)
        {
            return (double)obj.GetValue(HeaderContainerHeightProperty);
        }

        public static void SetHeaderContainerHeight(DependencyObject obj, double value)
        {
            obj.SetValue(HeaderContainerHeightProperty, value);
        }

        // Using a DependencyProperty as the backing store for HeaderContainerHeight.  This enables animation, styling, binding, etc...
        // 存储头部容器的高度（水平方向），或宽度（竖直方向）
        public static readonly DependencyProperty HeaderContainerHeightProperty =
            DependencyProperty.RegisterAttached("HeaderContainerHeight", typeof(double), typeof(TabablzControlHelper), new PropertyMetadata(0d));



        public static Thickness GetHeaderContainerMargin(DependencyObject obj)
        {
            return (Thickness)obj.GetValue(HeaderContainerMarginProperty);
        }

        public static void SetHeaderContainerMargin(DependencyObject obj, Thickness value)
        {
            obj.SetValue(HeaderContainerMarginProperty, value);
        }

        // Using a DependencyProperty as the backing store for HeaderContainerMargin.  This enables animation, styling, binding, etc...
        // 存储头部容器的 margin
        public static readonly DependencyProperty HeaderContainerMarginProperty =
            DependencyProperty.RegisterAttached("HeaderContainerMargin", typeof(Thickness), typeof(TabablzControlHelper), new PropertyMetadata(new Thickness(0d)));


    }
}
