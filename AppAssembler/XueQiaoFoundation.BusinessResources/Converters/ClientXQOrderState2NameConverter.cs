using NativeModel.Trade;
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
    /// <summary>
    /// 订单状态转换成名称转换器
    /// </summary>
    public class ClientXQOrderState2NameConverter : IValueConverter, IMultiValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ClientXQOrderState orderState)
            {
                return Properties.Resources.ResourceManager
                    .GetString($"ClientXQOrderState_{orderState.ToString()}", culture);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var orderState = values[0] as ClientXQOrderState?;
            var clientOrderType = values[1] as XQClientOrderType?;
            if (orderState != null)
            {
                if (clientOrderType != null)
                {
                    if (clientOrderType == XQClientOrderType.Condition
                        || clientOrderType == XQClientOrderType.Parked)
                    {
                        if (orderState == ClientXQOrderState.XQ_ORDER_RUNNING)
                        {
                            return Properties.Resources.ResourceManager
                                .GetString($"ClientXQOrderState_TriggerWaiting", culture);
                        }
                        else if (orderState == ClientXQOrderState.XQ_ORDER_FINISHED)
                        {
                            return Properties.Resources.ResourceManager
                                .GetString($"ClientXQOrderState_Triggered", culture);
                        }
                    }
                }
                return Properties.Resources.ResourceManager
                    .GetString($"ClientXQOrderState_{orderState.ToString()}", culture);
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
