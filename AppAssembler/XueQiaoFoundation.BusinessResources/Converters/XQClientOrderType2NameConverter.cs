using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using XueQiaoFoundation.BusinessResources.DataModels;

namespace XueQiaoFoundation.BusinessResources.Converters
{
    /// <summary>
    /// 将<see cref="XQClientOrderType"/> 转换为名称的转换器
    /// </summary>
    public class XQClientOrderType2NameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is XQClientOrderType orderType)
            {
                return Properties.Resources.ResourceManager
                    .GetString($"{orderType.GetType().Name}_{orderType.ToString()}", culture);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
