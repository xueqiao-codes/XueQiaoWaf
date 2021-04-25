using BolapanControl.ItemsFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace XueQiaoFoundation.UI.Styles
{
    public class DataGridColumnHeaderHelper
    {
        #region ShowColumnFilter

        /// <summary>
        /// Get the value to define whether to show column filter or not 
        /// </summary>
        [AttachedPropertyBrowsableForType(typeof(DataGridColumnHeader))]
        public static bool GetShowColumnFilter(DependencyObject obj)
        {
            return (bool)obj.GetValue(ShowColumnFilterProperty);
        }

        /// <summary>
        /// Set the value to define whether to show column filter or not 
        /// </summary>
        public static void SetShowColumnFilter(DependencyObject obj, bool value)
        {
            obj.SetValue(ShowColumnFilterProperty, value);
        }

        public static readonly DependencyProperty ShowColumnFilterProperty =
            DependencyProperty.RegisterAttached("ShowColumnFilter", typeof(bool), typeof(DataGridColumnHeaderHelper), new PropertyMetadata(false));

        #endregion


        #region ColumnFilterItemTemplate

        /// <summary>
        /// Get the value to define the filter item data template
        /// </summary>
        [AttachedPropertyBrowsableForType(typeof(DataGridColumnHeader))]
        public static DataTemplate GetColumnFilterItemTemplate(DependencyObject obj)
        {
            return (DataTemplate)obj.GetValue(ColumnFilterItemTemplateProperty);
        }

        /// <summary>
        /// Set the value to define the filter item data template
        /// </summary>
        public static void SetColumnFilterItemTemplate(DependencyObject obj, DataTemplate value)
        {
            obj.SetValue(ColumnFilterItemTemplateProperty, value);
        }

        public static readonly DependencyProperty ColumnFilterItemTemplateProperty =
            DependencyProperty.RegisterAttached("ColumnFilterItemTemplate", typeof(DataTemplate), typeof(DataGridColumnHeaderHelper), new PropertyMetadata(null));

        #endregion

        #region ColumnFilterItemTemplateSelector

        /// <summary>
        /// Get the value to define the selector of filter item data template
        /// </summary>
        [AttachedPropertyBrowsableForType(typeof(DataGridColumnHeader))]
        public static DataTemplateSelector GetColumnFilterItemTemplateSelector(DependencyObject obj)
        {
            return (DataTemplateSelector)obj.GetValue(ColumnFilterItemTemplateSelectorProperty);
        }

        /// <summary>
        /// Set the value to define the selector of filter item data template
        /// </summary>
        public static void SetColumnFilterItemTemplateSelector(DependencyObject obj, DataTemplateSelector value)
        {
            obj.SetValue(ColumnFilterItemTemplateSelectorProperty, value);
        }

        public static readonly DependencyProperty ColumnFilterItemTemplateSelectorProperty =
            DependencyProperty.RegisterAttached("ColumnFilterItemTemplateSelector", typeof(DataTemplateSelector), typeof(DataGridColumnHeaderHelper), new PropertyMetadata(null));

        #endregion

        #region FilterControlStateChangedCallbak

        [AttachedPropertyBrowsableForType(typeof(DataGridColumnHeader))]
        public static FilterControlStateChanged GetFilterControlStateChangedCallbak(DependencyObject obj)
        {
            return (FilterControlStateChanged)obj.GetValue(FilterControlStateChangedCallbakProperty);
        }

        public static void SetFilterControlStateChangedCallbak(DependencyObject obj, FilterControlStateChanged value)
        {
            obj.SetValue(FilterControlStateChangedCallbakProperty, value);
        }

        // Using a DependencyProperty as the backing store for FilterControlStateChangedCallbak.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilterControlStateChangedCallbakProperty =
            DependencyProperty.RegisterAttached("FilterControlStateChangedCallbak", typeof(FilterControlStateChanged), typeof(DataGridColumnHeaderHelper), new PropertyMetadata(null));

        #endregion

    }
}
