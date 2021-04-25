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
    public class HostingUserRole2DisplayNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int? role = null;
            if (value is EHostingUserRole _role1)
            {
                role = _role1.GetHashCode();
            }
            try
            {
                role = System.Convert.ToInt32(value);
            }
            catch (Exception)
            {
                return null;
            }

            if (role.HasValue)
            {
                return Properties.Resources.ResourceManager.GetString($"HostingUserRole_{role.Value}", culture);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
