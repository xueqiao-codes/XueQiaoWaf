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
    public class HostingExecOrderStateValue2NameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is HostingExecOrderStateValue _state)
            {
                return Properties.Resources.ResourceManager
                    .GetString($"{typeof(HostingExecOrderStateValue).Name}_{_state.ToString()}", culture);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
