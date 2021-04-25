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
    public class HostingXQCondition2DisplayTextConverter : IValueConverter
    {
        private readonly HostingXQConditionTrigger2DisplayTextConverter conditionTrigger2TextConverter = new HostingXQConditionTrigger2DisplayTextConverter();
        private readonly HostingXQConditionAction2DisplayTextConverter conditionAction2TextConverter = new HostingXQConditionAction2DisplayTextConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is HostingXQCondition condition)
            {
                var triggerText = conditionTrigger2TextConverter.Convert(condition.ConditionTrigger, typeof(string), null, culture) as string;
                var actionText = conditionAction2TextConverter.Convert(condition.ConditionAction, typeof(string), null, culture) as string;

                if (string.IsNullOrEmpty(triggerText) || string.IsNullOrEmpty(actionText))
                {
                    return null;
                }

                return $"当{triggerText}时，{actionText}";
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
