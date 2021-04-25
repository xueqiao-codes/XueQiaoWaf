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
    public class HostingXQConditionAction2DisplayTextConverter : IValueConverter
    {
        private readonly OrderExecParams2SendTypeNameConverter orderExecParams2SendTypeNameConverter = new OrderExecParams2SendTypeNameConverter();
        private readonly HostingXQOrderPrice2DisplayTextConverter orderPrice2DisplayTextConverter = new HostingXQOrderPrice2DisplayTextConverter();
        private readonly TradeDirection2NameConverter tradeDirection2DescNameConverter = new TradeDirection2NameConverter { IsConvert2DescriptionName = true };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is HostingXQConditionAction action)
            {
                string composeOrderSendTypeDesc= orderExecParams2SendTypeNameConverter.Convert(action.Extra, typeof(string), null, culture) as string;
                if (!string.IsNullOrEmpty(composeOrderSendTypeDesc))
                    composeOrderSendTypeDesc = $"以{composeOrderSendTypeDesc}方式发单";

                var priceText = orderPrice2DisplayTextConverter.Convert(action.Price, typeof(string), null, culture);
                if (priceText == null) return null;
                var tradeDirText = tradeDirection2DescNameConverter.Convert(action.TradeDirection, typeof(string), null, culture);
                var actionDesc = $"以{priceText}{tradeDirText}{action.Quantity}个该标的";

                return string.Join("，", new string[] { composeOrderSendTypeDesc, actionDesc }.Where(i => !string.IsNullOrEmpty(i)));
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
