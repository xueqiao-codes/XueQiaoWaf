using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace XueQiaoFoundation.UI.Converters
{
    public class NotEqualToBooleanConverter : IValueConverter, IMultiValueConverter
    {
        private readonly EqualityToBooleanConverter equalityToBooleanConverter = new EqualityToBooleanConverter();

        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = equalityToBooleanConverter.Convert(value, targetType, parameter, culture);
            if (result is bool boolResult)
            {
                return !boolResult;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
        #endregion

        #region IMultiValueConverter
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var result = equalityToBooleanConverter.Convert(values, targetType, parameter, culture);
            if (result is bool boolResult)
            {
                return !boolResult;
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
