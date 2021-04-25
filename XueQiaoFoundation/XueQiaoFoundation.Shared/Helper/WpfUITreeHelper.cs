using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace XueQiaoFoundation.Shared.Helper
{
    public class WpfUITreeHelper
    {
        public static ChildType FindVisualChild<ChildType>(DependencyObject obj)
            where ChildType : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is ChildType)
                    return (ChildType)child;
                else
                {
                    ChildType childOfChild = FindVisualChild<ChildType>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        public static ParentType FindVisualParent<ParentType>(DependencyObject initial)
            where ParentType : DependencyObject
        {
            DependencyObject current = initial;
            ParentType result = null;
            while (true)
            {
                if (current == null || (current is ParentType))
                {
                    result = current as ParentType;
                    break;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            return result;
        }
    }
}
