using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using xueqiao.trade.hosting;

namespace XueQiaoFoundation.BusinessResources.Converters
{
    /// <summary>
    /// 将<see cref="HostingTradeAccount"/>的某些字段转换成显示内容
    /// parameter根据<see cref="HostingUser2DisplayField"/> 指定 field 类型
    /// value 传入要转换的 field
    /// </summary>
    public class HostingTradeAccountField2DisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is HostingUser2DisplayField fieldType)
            {
                switch (fieldType)
                {
                    case HostingUser2DisplayField.BrokerTechPlatform:
                        return GetBrokerTechPlatformDisplayName(value, culture);
                    case HostingUser2DisplayField.TradeAccountState:
                        return GetTradeAccountStateDisplayName(value, culture);
                    case HostingUser2DisplayField.TradeAccountAccessState:
                        return GetAccountAccessStateDisplayName(value, culture);
                }
                return null;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }

        private string GetBrokerTechPlatformDisplayName(object srcValue, CultureInfo culture)
        {
            int? tarItem = null;
            if (srcValue is BrokerTechPlatform tmp1)
            {
                tarItem = tmp1.GetHashCode();
            }
            else if (srcValue is int tmp2)
            {
                tarItem = tmp2;
            }
            else if (srcValue is string tmp3
                && Enum.TryParse(tmp3, out BrokerTechPlatform tmp4))
            {
                tarItem = tmp4.GetHashCode();
            }
            if (tarItem != null)
            {
                var key = $"BrokerTechPlatform_{(int)tarItem}";
                return Properties.Resources.ResourceManager.GetString(key, culture);
            }
            return null;
        }

        private string GetTradeAccountStateDisplayName(object srcValue, CultureInfo culture)
        {
            int? tarItem = null;
            if (srcValue is TradeAccountState tmp1)
            {
                tarItem = tmp1.GetHashCode();
            }
            else if (srcValue is int tmp2)
            {
                tarItem = tmp2;
            }
            else if (srcValue is string tmp3
                && Enum.TryParse(tmp3, out TradeAccountState tmp4))
            {
                tarItem = tmp4.GetHashCode();
            }
            if (tarItem != null)
            {
                var key = $"TradeAccountState_{(int)tarItem}";
                return Properties.Resources.ResourceManager.GetString(key, culture);
            }
            return null;
        }

        private string GetAccountAccessStateDisplayName(object srcValue, CultureInfo culture)
        {
            int? tarItem = null;
            if (srcValue is TradeAccountAccessState tmp1)
            {
                tarItem = tmp1.GetHashCode();
            }
            else if (srcValue is int tmp2)
            {
                tarItem = tmp2;
            }
            else if (srcValue is string tmp3
                && Enum.TryParse(tmp3, out TradeAccountAccessState tmp4))
            {
                tarItem = tmp4.GetHashCode();
            }
            if (tarItem != null)
            {
                var key = $"TradeAccountAccessState_{(int)tarItem}";
                return Properties.Resources.ResourceManager.GetString(key, culture);
            }
            return null;
        }
    }

    public enum HostingUser2DisplayField
    {
        BrokerTechPlatform = 1,
        TradeAccountState = 2,
        TradeAccountAccessState = 3
    }
}
