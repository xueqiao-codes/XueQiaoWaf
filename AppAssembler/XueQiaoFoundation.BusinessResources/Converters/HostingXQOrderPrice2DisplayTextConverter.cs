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
    public class HostingXQOrderPrice2DisplayTextConverter : IValueConverter
    {
        private readonly HostingXQOrderPriceType2NameConverter orderPriceType2NameConverter = new HostingXQOrderPriceType2NameConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is HostingXQOrderPrice orderPrice)
            {
                var priceTypeText = orderPriceType2NameConverter.Convert(orderPrice.PriceType, typeof(string), null, culture) as string;
                if (orderPrice.PriceType == HostingXQOrderPriceType.ACTION_PRICE_LIMIT)
                {
                    return $"{priceTypeText}={orderPrice.LimitPrice}";
                }
                else
                {
                    return $"{priceTypeText}追加{orderPrice.ChasePriceTicks}";
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
