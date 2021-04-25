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
    /// <summary>
    /// 将 <see cref="HostingXQComposeLimitOrderExecParams"/> 转换为执行发单形式名称
    /// </summary>
    public class OrderExecParams2SendTypeNameConverter : IValueConverter
    {
        private readonly XQComposeOrderExecParamsSendType2NameConverter composeOrderExecParamsSendType2NameConverter = new XQComposeOrderExecParamsSendType2NameConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is HostingXQComposeLimitOrderExecParams _execParams)
            {
                return GetComposeOrderSendTypeName(_execParams, culture);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private string GetComposeOrderSendTypeName(HostingXQComposeLimitOrderExecParams execParams, CultureInfo culture)
        {
            var orderSendType = execParams?.ParseComposeOrderExecParamsSendType();
            if (orderSendType != null)
            {
                return composeOrderExecParamsSendType2NameConverter.Convert(orderSendType, typeof(string), null, culture) as string;
            }
            return null;
        }
    }
}
