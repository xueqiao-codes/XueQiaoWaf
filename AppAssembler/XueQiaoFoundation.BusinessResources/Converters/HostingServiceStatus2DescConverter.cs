using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using xueqiao.trade.hosting.proxy;

namespace XueQiaoFoundation.BusinessResources.Converters
{
    public class HostingServiceStatus2DescConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is HostingServiceStatus sta)
            {
                return Properties.Resources.ResourceManager
                    .GetString($"{typeof(HostingServiceStatus).Name}_Desc_{sta.ToString()}", culture);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
