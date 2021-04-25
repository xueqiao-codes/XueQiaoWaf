using System;
using System.Globalization;
using System.Windows.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Presentations.Converters
{
    /// <summary>
    /// 单位是分的金额转换器
    /// </summary>
    public class CentOfMoney2FormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            long? moneyOfCent = null;
            try
            {
                moneyOfCent = System.Convert.ToInt64(value);
            }
            catch (Exception)
            {
                return Binding.DoNothing;
            }

            if (moneyOfCent != null)
            {
                float money = (float)(moneyOfCent / (float)100.0);
                return money.ToString("0,0.00", CultureInfo.InvariantCulture);
            }

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
