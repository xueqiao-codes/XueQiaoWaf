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
    public class XqAppLanguages2NameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is XqAppLanguages tarLang)
            {
                return Properties.Resources.ResourceManager
                    .GetString($"{tarLang.GetType().Name}_{tarLang.ToString()}", culture);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
