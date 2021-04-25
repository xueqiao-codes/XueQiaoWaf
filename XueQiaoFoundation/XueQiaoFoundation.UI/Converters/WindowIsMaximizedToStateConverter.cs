using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace XueQiaoFoundation.UI.Converters
{
    [ValueConversion(typeof(bool), typeof(WindowState))]
    public class WindowIsMaximizedToStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isMaximized)
            {
                return isMaximized ? WindowState.Maximized : WindowState.Normal;
            }
            else if (value is WindowState state)
            {
                return state == WindowState.Maximized;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isMaximized)
            {
                return isMaximized ? WindowState.Maximized : WindowState.Normal;
            }
            else if (value is WindowState state)
            {
                return state == WindowState.Maximized;
            }
            return value;
        }
    }
}
