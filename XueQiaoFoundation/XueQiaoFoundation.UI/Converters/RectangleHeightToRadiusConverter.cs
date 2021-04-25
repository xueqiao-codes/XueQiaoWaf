using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace XueQiaoFoundation.UI.Converters
{
    public class RectangleHeightToRadiusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var height = value as double?;
            return height.GetValueOrDefault(0) / 2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var radius = value as double?;
            return radius.GetValueOrDefault(0) * 2;
        }
    }
}
