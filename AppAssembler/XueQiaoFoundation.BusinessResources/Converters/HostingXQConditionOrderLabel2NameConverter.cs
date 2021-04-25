using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using xueqiao.trade.hosting.arbitrage.thriftapi;

namespace XueQiaoFoundation.BusinessResources.Converters
{
    public class HostingXQConditionOrderLabel2NameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is HostingXQConditionOrderLabel label)
            {
                var resMan = Properties.Resources.ResourceManager;
                return resMan.GetString($"{label.GetType().Name}_{label.ToString()}", culture);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
