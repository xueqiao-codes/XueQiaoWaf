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
    public class HostingXQComposeLimitOrderFirstLegChooseMode2NameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is HostingXQComposeLimitOrderFirstLegChooseMode _type)
            {
                var key = $"HostingXQComposeLimitOrderFirstLegChooseMode_{_type.ToString()}";
                return Properties.Resources.ResourceManager.GetString(key, culture);
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
