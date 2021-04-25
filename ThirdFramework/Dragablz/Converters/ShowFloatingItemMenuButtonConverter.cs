using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Dragablz.Converters
{
    public class ShowFloatingItemMenuButtonConverter : IMultiValueConverter
    {
        /// <summary>
        /// [0] is owning property (Layout.FloatingItemIsCustomMenuButtons) value.
        /// [1] is owning property (Layout.IsFloatingInLayout) value.
        /// [2] is owning the button's property (IsEnabled) value
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool FloatingItemIsCustomMenuButtons = (values[0] == DependencyProperty.UnsetValue) ? false : (bool)values[0];
            bool IsFloatingInLayout = (values[1] == DependencyProperty.UnsetValue) ? false : (bool)values[1];
            bool IsButtonEnabled = (values[2] == DependencyProperty.UnsetValue) ? false : (bool)values[2];

            return 
                (!FloatingItemIsCustomMenuButtons && IsFloatingInLayout && IsButtonEnabled) 
                ? Visibility.Visible 
                : Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
