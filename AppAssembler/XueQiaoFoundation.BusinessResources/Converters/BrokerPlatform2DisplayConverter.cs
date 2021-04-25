using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using xueqiao.broker;

namespace XueQiaoFoundation.BusinessResources.Converters
{
    /// <summary>
    /// 将<see cref="BrokerPlatform"/> 类型数据转换为显示文字
    /// </summary>
    public class BrokerPlatform2DisplayConverter : IValueConverter
    {
        public object Convert(object srcValue, Type targetType, object parameter, CultureInfo culture)
        {
            int? tarItem = null;
            if (srcValue is BrokerPlatform tmp1)
            {
                tarItem = tmp1.GetHashCode();
            }
            else if (srcValue is int tmp2)
            {
                tarItem = tmp2;
            }
            else if (srcValue is string tmp3
                && Enum.TryParse(tmp3, out BrokerPlatform tmp4))
            {
                tarItem = tmp4.GetHashCode();
            }
            if (tarItem != null)
            {
                var key = $"xueqiao_broker_BrokerPlatform_{(int)tarItem}";
                return Properties.Resources.ResourceManager.GetString(key, culture);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
