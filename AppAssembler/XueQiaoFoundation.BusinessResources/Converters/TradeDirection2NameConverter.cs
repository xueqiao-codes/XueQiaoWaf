using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Data;
using xueqiao.trade.hosting;
using xueqiao.trade.hosting.arbitrage.thriftapi;
using NativeModel.Trade;

namespace XueQiaoFoundation.BusinessResources.Converters
{
    /// <summary>
    /// 将交易方向转换为显示名称的转换器
    /// </summary>
    public class TradeDirection2NameConverter : IValueConverter
    {
        /// <summary>
        /// 是否转化成为描述性的名称。buy 为买入，sell 为卖出。否则，buy 为买，sell 为卖
        /// </summary>
        public bool IsConvert2DescriptionName { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int? tarDirCode = null;
            if (value is HostingExecOrderTradeDirection execOrderTradeDirection)
            {
                tarDirCode = execOrderTradeDirection.GetHashCode();
            }
            else if (value is HostingExecTradeDirection execTradeDirection)
            {
                tarDirCode = execTradeDirection.GetHashCode();
            }
            else if (value is HostingXQTradeDirection xqTradeDirection)
            {
                tarDirCode = xqTradeDirection.GetHashCode();
            }
            else if (value is ClientTradeDirection clientTradeDirection)
            {
                tarDirCode = clientTradeDirection.GetHashCode();
            }
            else if (value is int tarDirInt)
            {
                tarDirCode = tarDirInt;
            }
            if (tarDirCode != null)
            {
                if (IsConvert2DescriptionName)
                {
                    return Properties.Resources.ResourceManager.GetString($"TradeDirectionDesc_{tarDirCode}", culture);
                }
                else
                {
                    return Properties.Resources.ResourceManager.GetString($"TradeDirection_{tarDirCode}", culture);
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
