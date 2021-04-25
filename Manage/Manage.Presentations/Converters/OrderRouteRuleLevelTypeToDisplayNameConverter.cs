using Manage.Applications.DataModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Manage.Presentations.Converters
{
    /// <summary>
    /// 将<see cref="OrderRouteRuleLevelType"/>转换为字面显示
    /// </summary>
    public class OrderRouteRuleLevelTypeToDisplayNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int? typeCode = null;
            if (value is OrderRouteRuleLevelType typeTar)
            {
                typeCode = typeTar.GetHashCode();
            }
            else if (value is int intTar)
            {
                typeCode = intTar;
            }
            if (typeCode == null) return value;

            var key = $"OrderRouteRuleLevelType_{typeCode}";
            return Properties.Resources.ResourceManager.GetString(key, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
