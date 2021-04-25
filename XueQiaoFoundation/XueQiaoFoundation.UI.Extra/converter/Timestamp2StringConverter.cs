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
    public enum TimestampValueType
    {
        Millisecond = 1, // Millisecond 类型的时间戳，请提供 long 类型的数据
        Second = 2       // Second 类型的时间戳，请提供 int 类型的数据
    }

    public enum Timestamp2StringFormatType
    {
        DateTime = 1,
        Date = 2,
        Time = 3,
        DateTimeIgnoreYear = 4,
        DateTimeIgnoreYearWithMs = 5,     // 显示到毫秒的时间格式
    }

    public class Timestamp2StringConverter : IValueConverter
    {
        public TimestampValueType ValueType { get; set; } = TimestampValueType.Millisecond;

        public Timestamp2StringFormatType Convert2StringFormatType { get; set; } = Timestamp2StringFormatType.DateTime;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            long tarValue = 0;
            try
            {
                tarValue = System.Convert.ToInt64(value);
            }
            catch (Exception)
            {
                return null;
            }
            var formatType = this.Convert2StringFormatType;

            DateTime? dateTime = null;
            if (ValueType == TimestampValueType.Millisecond)
            {
                dateTime = DateHelper.UnixTimeStampMsToDateTime(tarValue);
            } else if (ValueType == TimestampValueType.Second)
            {
                dateTime = DateHelper.UnixTimeStampToDateTime(tarValue);
            }
            if (dateTime == null)
            {
                return null;
            }

            dateTime = dateTime.Value.ToLocalTime();
            string result = null;
            switch (formatType)
            {
                case Timestamp2StringFormatType.DateTime:
                    result = dateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    break;
                case Timestamp2StringFormatType.Date:
                    result = dateTime.Value.ToString("yyyy-MM-dd");
                    break;
                case Timestamp2StringFormatType.Time:
                    result = dateTime.Value.ToString("HH:mm:ss");
                    break;
                case Timestamp2StringFormatType.DateTimeIgnoreYear:
                    result = dateTime.Value.ToString("MM-dd HH:mm:ss");
                    break;
                case Timestamp2StringFormatType.DateTimeIgnoreYearWithMs:
                    result = dateTime.Value.ToString("MM-dd HH:mm:ss.fff");
                    break;
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
