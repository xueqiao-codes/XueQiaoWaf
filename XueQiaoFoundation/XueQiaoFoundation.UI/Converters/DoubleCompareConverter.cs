using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using XueQiaoFoundation.Shared.Model;

namespace XueQiaoFoundation.UI.Converters
{
    /// <summary>
    /// 比较两个值。返回 <see cref="XueQiaoFoundation.Shared.Model.XQCompareResult"/> 类型结果。如果两个值不能转换成 double，则返回 null
    /// </summary>
    public class DoubleCompareConverter : IValueConverter, IMultiValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var d1 = System.Convert.ToDouble(value);
                var d2 = System.Convert.ToDouble(parameter);
                
                if (d1 < d2) return XQCompareResult.Smaller;
                if (d1 == d2) return XQCompareResult.Same;
                return XQCompareResult.Larger;
            }
            catch (Exception) { }
            return null;
        }
        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length != 2) return null;
            try
            {
                var d1 = System.Convert.ToDouble(values[0]);
                var d2 = System.Convert.ToDouble(values[1]);

                if (d1 < d2) return XQCompareResult.Smaller;
                if (d1 == d2) return XQCompareResult.Same;
                return XQCompareResult.Larger;
            }
            catch (Exception) { }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
