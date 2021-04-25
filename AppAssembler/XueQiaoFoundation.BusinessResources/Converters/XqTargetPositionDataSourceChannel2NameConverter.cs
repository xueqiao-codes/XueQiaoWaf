using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace XueQiaoFoundation.BusinessResources.Converters
{
    public class XqTargetPositionDataSourceChannel2NameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is XqTargetPositionDataSourceChannel channel)
            {
                return Properties.Resources.ResourceManager
                    .GetString($"{typeof(XqTargetPositionDataSourceChannel).Name}_{channel.ToString()}", culture);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
