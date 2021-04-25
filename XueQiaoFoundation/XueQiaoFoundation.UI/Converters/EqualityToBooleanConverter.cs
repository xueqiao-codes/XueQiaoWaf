using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace XueQiaoFoundation.UI.Converters
{
    public class EqualityToBooleanConverter : IValueConverter, IMultiValueConverter
    {
        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Equals(value, parameter);
        }
        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
        #endregion

        #region IMultiValueConverter
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length <= 1) return false;
            if (values.Any(i => i == null)) return false;

            bool result = true;
            object indexObj = values.FirstOrDefault();
            foreach (var item in values.Skip(1))
            {
                if (!indexObj.Equals(item))
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
        #endregion
    }
}
