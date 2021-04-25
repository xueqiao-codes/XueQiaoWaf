using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using XueQiaoFoundation.Shared.Helper;

namespace XueQiaoFoundation.UI.Extra.converter
{
    public class Timestamp2DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                long timestamp = System.Convert.ToInt64(value);
                if (timestamp <= 0)
                    return null;

                var dateTime = DateHelper.UnixTimeStampToDateTime(timestamp).ToLocalTime();
                return dateTime;
            }
            catch
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                return (long)DateHelper.UnixTimspan(dateTime, DateTimeKind.Local).TotalSeconds;
            }
            return 0;
        }
    }
}
