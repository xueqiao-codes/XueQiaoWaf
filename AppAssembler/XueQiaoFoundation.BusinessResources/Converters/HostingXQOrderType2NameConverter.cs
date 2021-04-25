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
    public class HostingXQOrderType2NameConverter : IValueConverter
    {
        /// <summary>
        /// 是否包含标的类型名称，否则只显示订单类型名称
        /// </summary>
        public bool IncludeTargetTypeName { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is HostingXQOrderType type)
            {
                string key = null;
                if (IncludeTargetTypeName)
                    key = $"HostingXQOrderType_IncludeTargetTypeName_{type.ToString()}";
                else
                    key = $"HostingXQOrderType_{type.ToString()}";
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
