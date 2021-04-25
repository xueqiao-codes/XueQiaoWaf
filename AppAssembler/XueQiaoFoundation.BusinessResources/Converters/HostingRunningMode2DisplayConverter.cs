using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using xueqiao.trade.hosting;

namespace XueQiaoFoundation.BusinessResources.Converters
{
    public class HostingRunningMode2DisplayConverter : IValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"><see cref="HostingRunningMode"/></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int? valueCode = null;
            if (value is HostingRunningMode _mode)
            {
                valueCode = _mode.GetHashCode();
            }
            else if (valueCode is int _code)
            {
                valueCode = _code;
            }
            if (valueCode == null) return value;

            var key = $"HostingRunningMode_{valueCode}";
            return Properties.Resources.ResourceManager.GetString(key, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
