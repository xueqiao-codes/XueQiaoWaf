using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace XueQiaoFoundation.BusinessResources.Converters
{
    /// <summary>
    /// 乘法数值转换器
    /// </summary>
    public class MultiplicationValueConverter : IValueConverter
    {
        /// <summary>
        /// 乘数
        /// </summary>
        public double Multiplier { get; set; } = 1;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var tarVal = System.Convert.ToDouble(value);
                return tarVal * (1.0d/this.Multiplier);
            }
            catch (Exception) { }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var tarVal = System.Convert.ToDouble(value);
                return tarVal / (1.0d / this.Multiplier);
            }
            catch (Exception) { }
            return value;
        }
    }
}
