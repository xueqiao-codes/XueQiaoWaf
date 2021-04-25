using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace XueQiaoWaf.Trade.Modules.Presentation.Converters
{
    /// <summary>
    /// 订阅列表分组头部尺寸转换器
    /// </summary>
    public class SubscribeDataGroupHeaderSizeConverter : IMultiValueConverter
    {
        /// <summary>
        /// The first value should be the total size available size, typically the parent control size.  
        /// All additional values should be siblings sizes (width or height) which will affect (reduce) the available size.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null) throw new ArgumentNullException("values");

            if (values.Length < 1) return Binding.DoNothing;
            var availableSize = values.Take(1).OfType<double>().First();

            var reduceSize = values.Skip(1)
                .OfType<double>()
                .Where(d => !double.IsInfinity(d) && !double.IsNaN(d))
                .Sum();

            return Math.Max(0, availableSize - reduceSize);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
