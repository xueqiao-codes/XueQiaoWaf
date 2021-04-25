using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using xueqiao.graph.xiaoha.chart.thriftapi;

namespace Touyan.app.converter
{
    /// <summary>
    /// 将 <see cref="ChartType"/> 转换成名称的 Converter
    /// </summary>
    public class ChartType2NameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ChartType _type)
            {
                return Properties.Resources.ResourceManager
                    .GetString($"{_type.GetType().Name}_{_type.ToString()}", culture);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
