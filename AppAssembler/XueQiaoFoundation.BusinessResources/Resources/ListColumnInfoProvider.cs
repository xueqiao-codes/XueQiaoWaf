using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using XueQiaoFoundation.BusinessResources.Models;

namespace XueQiaoFoundation.BusinessResources.Resources
{
    public class ListColumnInfoProvider
    {
        public static ListColumnInfo GetColumnInfo(DependencyObject obj)
        {
            return (ListColumnInfo)obj.GetValue(ColumnInfoProperty);
        }

        public static void SetColumnInfo(DependencyObject obj, ListColumnInfo value)
        {
            obj.SetValue(ColumnInfoProperty, value);
        }
        
        public static readonly DependencyProperty ColumnInfoProperty =
            DependencyProperty.RegisterAttached("ColumnInfo", typeof(ListColumnInfo), typeof(ListColumnInfoProvider), new PropertyMetadata(null));
        
    }
}
