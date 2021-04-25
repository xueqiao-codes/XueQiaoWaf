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
    /// 雪橇标的市值（货值）格式化转化器
    /// </summary>
    public class XqTargetMarketValueFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double? marketValue = null;
            try
            {
                marketValue = System.Convert.ToDouble(value);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Fail to convert to double. e:{e.StackTrace}");
            }
            if (marketValue != null)
            {
                double tmpMarket = marketValue.Value;
                var absMarket = Math.Abs(tmpMarket);

                double resultValue = tmpMarket;
                string unitStr = null;
                const long c_yi = 100000000;

                if (absMarket >= 10000 && absMarket < c_yi)
                {
                    resultValue = tmpMarket / 10000;
                    unitStr = "万";
                }
                else if (absMarket >= c_yi)
                {
                    resultValue = tmpMarket / c_yi;
                    unitStr = "亿";
                }

                // 格式化成 0.00 形式，为了防止 ToString 的 ceiling 操作，先使用以下方法转换一下
                resultValue = Math.Floor(resultValue * 100) / 100;
                return $"{resultValue.ToString("0.00")}{unitStr}";
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
