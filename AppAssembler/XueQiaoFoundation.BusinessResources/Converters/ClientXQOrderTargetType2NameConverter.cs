using NativeModel.Trade;
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
    public class ClientXQOrderTargetType2NameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ClientXQOrderTargetType? orderTargetType = null;
            if (value is HostingXQTargetType _hostingXQTargetType)
            {
                orderTargetType = _hostingXQTargetType.ToClientXQOrderTargetType();
            } else if (value is ClientXQOrderTargetType _clientOrderTargetType)
            {
                orderTargetType = _clientOrderTargetType;
            }

            if (orderTargetType != null)
            {
                return Properties.Resources.ResourceManager
                    .GetString($"{orderTargetType.GetType().Name}_{orderTargetType.ToString()}", culture);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
