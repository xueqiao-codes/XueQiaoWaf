using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using xueqiao.trade.hosting.position.adjust.thriftapi;

namespace XueQiaoFoundation.BusinessResources.Converters
{
    /// <summary>
    /// 将<see cref="VerifyStatus"/>转换为名称
    /// </summary>
    public class VerifyStatus2NameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is VerifyStatus stat)
            {
                return Properties.Resources.ResourceManager.GetString($"{stat.GetType().Name}_{stat.ToString()}", culture);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
