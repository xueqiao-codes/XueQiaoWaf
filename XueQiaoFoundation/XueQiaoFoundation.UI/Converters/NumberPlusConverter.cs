using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace XueQiaoFoundation.UI.Converters
{
    /// <summary>
    /// 数值相加转换器
    /// </summary>
    public class NumberPlusConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null) return null;

            var nums = new List<double>();
            foreach (var item in values)
            {
                try
                {
                    var num = System.Convert.ToDouble(item);
                    nums.Add(num);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Fail to convert to double. e:{e.StackTrace}");
                }
            }
            return nums.Sum();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
