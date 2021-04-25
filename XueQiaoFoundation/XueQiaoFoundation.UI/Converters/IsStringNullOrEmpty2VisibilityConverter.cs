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
    public class IsStringNullOrEmpty2VisibilityConverter : IValueConverter
    {
        /// <summary>
        /// 当 IsStringNullOrEmpty == true 时，返回的 Visibility 值
        /// </summary>
        public Visibility ReturnVisibilityWhenResultTrue { get; set; } = Visibility.Visible;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string _str)
            {
                if (string.IsNullOrEmpty(_str))
                {
                    return this.ReturnVisibilityWhenResultTrue;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
