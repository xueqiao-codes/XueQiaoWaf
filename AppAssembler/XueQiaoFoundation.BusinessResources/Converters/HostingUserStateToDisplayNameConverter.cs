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
    /// 将<see cref="HostingUserState"/>转换为名称的转换器
    /// </summary>
    public class HostingUserStateToDisplayNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int? stateCode = null;
            if (value is HostingUserState stateTar)
            {
                stateCode = stateTar.GetHashCode();
            }
            else if (value is int intTar)
            {
                stateCode = intTar;
            }
            if (stateCode == null) return value;

            var key = $"HostingUserState_{stateCode}";
            return Properties.Resources.ResourceManager.GetString(key, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
