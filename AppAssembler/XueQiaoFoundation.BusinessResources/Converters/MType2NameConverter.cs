using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using xueqiao.mailbox.user.message.thriftapi;

namespace XueQiaoFoundation.BusinessResources.Converters
{
    /// <summary>
    /// 将 <see cref="MType"/> 转换为名称显示的转换器
    /// </summary>
    public class MType2NameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MType type)
            {
                return Properties.Resources.ResourceManager.GetString($"{type.GetType().Name}_{type.ToString()}", culture);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
