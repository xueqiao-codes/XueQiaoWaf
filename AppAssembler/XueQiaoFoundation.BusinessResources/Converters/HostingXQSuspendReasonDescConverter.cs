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
    /// <summary>
    /// 订单暂停原因描述的转换器。需提供value0:reason, value1:suspendStateCode 两个参数
    /// </summary>
    public class HostingXQSuspendReasonDescConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2) return null;
            try
            {
                var suspendReasonType = values[0] as HostingXQSuspendReason?;
                var suspendStateCode = System.Convert.ToInt32(values[1]);
                if (suspendReasonType == null) return null;

                var resMan = Properties.Resources.ResourceManager;
                string desc = null;
                if (suspendReasonType == HostingXQSuspendReason.SUSPENDED_FUNCTIONAL
                    || suspendReasonType == HostingXQSuspendReason.SUSPENDED_ERROR_OCCURS)
                {
                    desc = resMan.GetString($"HostingXQSuspendReason_StateCode_{suspendStateCode}", culture);
                }
                if (desc == null)
                {
                    desc = resMan.GetString($"HostingXQSuspendReason_{suspendReasonType.ToString()}", culture);
                }
                return desc;
            }
            catch
            {
                return null;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
