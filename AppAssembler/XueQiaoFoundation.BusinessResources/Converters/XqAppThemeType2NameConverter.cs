using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using XueQiaoFoundation.BusinessResources.Models;

namespace XueQiaoFoundation.BusinessResources.Converters
{
    public class XqAppThemeType2NameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is XqAppThemeType tarType)
            {
                return Properties.Resources.ResourceManager.GetString($"{tarType.GetType().Name}_{tarType.ToString()}", culture);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
