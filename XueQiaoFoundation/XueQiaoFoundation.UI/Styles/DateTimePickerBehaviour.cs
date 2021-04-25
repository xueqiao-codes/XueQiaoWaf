using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace XueQiaoFoundation.UI.Styles
{
    public static class DateTimePickerBehaviour
    {
        public static bool GetCloseDropDownWhenSelectedDate(DependencyObject obj)
        {
            return (bool)obj.GetValue(CloseDropDownWhenSelectedDateProperty);
        }

        public static void SetCloseDropDownWhenSelectedDate(DependencyObject obj, bool value)
        {
            obj.SetValue(CloseDropDownWhenSelectedDateProperty, value);
        }

        // Using a DependencyProperty as the backing store for CloseDropDownWhenSelectedDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CloseDropDownWhenSelectedDateProperty =
            DependencyProperty.RegisterAttached("CloseDropDownWhenSelectedDate",
                typeof(bool), typeof(DateTimePickerBehaviour),
                new UIPropertyMetadata(false, OnCloseDropDownWhenSelectedDateChanged));

        private static void OnCloseDropDownWhenSelectedDateChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var picker = sender as DateTimePicker;
            if (picker == null) return;
            var closeDropDownWhenSelectedDate = (bool)e.NewValue;

            if (closeDropDownWhenSelectedDate)
            {
                picker.SelectedDateChanged -= CloseDatePickerWhenSelectedDateChanged;
                picker.SelectedDateChanged += CloseDatePickerWhenSelectedDateChanged;
            }
            else
            {
                picker.SelectedDateChanged -= CloseDatePickerWhenSelectedDateChanged;
            }
        }

        private static void CloseDatePickerWhenSelectedDateChanged(object sender, TimePickerBaseSelectionChangedEventArgs<DateTime?> e)
        {
            var picker = sender as DateTimePicker;
            if (picker == null) return;
            picker.IsDropDownOpen = false;
        }
    }
}
