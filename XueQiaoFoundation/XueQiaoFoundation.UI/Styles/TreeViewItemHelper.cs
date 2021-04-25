using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace XueQiaoFoundation.UI.Styles
{
    public static class TreeViewItemHelper
    {
        public static bool GetAlwaysDisplayExpandCollapseIcon(DependencyObject obj)
        {
            return (bool)obj.GetValue(AlwaysDisplayExpandCollapseIconProperty);
        }

        public static void SetAlwaysDisplayExpandCollapseIcon(DependencyObject obj, bool value)
        {
            obj.SetValue(AlwaysDisplayExpandCollapseIconProperty, value);
        }


        /// <summary>
        /// 是否总是显示 展开收缩 按钮
        /// </summary>
        public static readonly DependencyProperty AlwaysDisplayExpandCollapseIconProperty =
            DependencyProperty.RegisterAttached("AlwaysDisplayExpandCollapseIcon", typeof(bool), typeof(TreeViewItemHelper), new PropertyMetadata(false));

    }
}
