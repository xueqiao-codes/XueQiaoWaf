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
    public class HostingXQConditionTrigger2DisplayTextConverter : IValueConverter
    {
        private readonly HostingXQConditionTriggerPriceType2NameConverter triggerPriceType2NameConverter = new HostingXQConditionTriggerPriceType2NameConverter();
        private readonly HostingXQConditionTriggerOperator2SymbolConverter triggerOperator2SymbolConverter = new HostingXQConditionTriggerOperator2SymbolConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is HostingXQConditionTrigger trigger)
            {
                var triggerPriceTypeText = triggerPriceType2NameConverter.Convert(trigger.TriggerPriceType, typeof(string), null, culture);
                var triggerOperatorSymbolText = triggerOperator2SymbolConverter.Convert(trigger.TriggerOperator, typeof(string), null, culture);

                return $"{triggerPriceTypeText}{triggerOperatorSymbolText}{trigger.ConditionPrice}";
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
