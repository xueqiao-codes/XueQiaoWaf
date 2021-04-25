using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using XueQiaoFoundation.BusinessResources.DataModels;

namespace XueQiaoFoundation.BusinessResources.Converters
{
    public class TradeDetailSource2NameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string srcStr = null;
            if (value is TradeDetailSource source)
            {
                srcStr = source.ToString();
            }
            else if (value is string sourceStr)
            {
                srcStr = sourceStr;
            }
            if (srcStr != null)
            {
                return Properties.Resources.ResourceManager
                    .GetString($"{typeof(TradeDetailSource).Name}_{srcStr}", culture);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
