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
    public class EqualityToVisibilityConverter : IValueConverter, IMultiValueConverter
    {
        /// <summary>
        /// 相等时是否隐藏，否则在相等时显示
        /// </summary>
        public bool CollapsedWhenEquals { get; set; }

        /// <summary>
        /// 不相等时是否隐藏，否则在不相等时显示
        /// </summary>
        public bool CollapsedWhenNotEquals { get; set; }
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return GetVisibility(Equals(value, parameter));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        private Visibility GetVisibility(bool isEqual)
        {
            if (isEqual)
            {
                return this.CollapsedWhenEquals ? Visibility.Collapsed : Visibility.Visible;
            }
            else
            {
                return this.CollapsedWhenNotEquals ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        #region IMultiValueConverter

        private EqualityToBooleanConverter equalityToBooleanConverter = new EqualityToBooleanConverter();

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var result = equalityToBooleanConverter.Convert(values, targetType, parameter, culture);
            if (result is bool isEqual)
            {
                return GetVisibility(isEqual);
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
        #endregion
    }
}
