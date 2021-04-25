using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.Shared.Helper
{
    public static class DateHelper
    {
        /// <summary>
        /// 让未定义类型的 dataTime 类型化
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="specifiedKind"></param>
        /// <returns></returns>
        public static DateTime SpecifiedWithKindIfNeed(DateTime dateTime, DateTimeKind specifiedKind)
        {
            if (dateTime.Kind == DateTimeKind.Unspecified)
            {
                dateTime = DateTime.SpecifyKind(dateTime, specifiedKind);
            }
            return dateTime;
        }

        /// <summary>
        /// 现在距离 1970/1/1的时间间隔
        /// </summary>
        /// <returns></returns>
        public static TimeSpan NowUnixTimeSpan()
        {
            return UnixTimspan(DateTime.UtcNow, DateTimeKind.Utc);
        }

        /// <summary>
        /// 源时间距离 1970/1/1的时间间隔
        /// </summary>
        /// <param name="srcDT">源时间</param>
        /// <param name="srcDTSpecifiedKindIfNeed">源日期的Kind为<see cref="DateTimeKind.Unspecified"/>时，需类型化的类型</param>
        /// <returns></returns>
        public static TimeSpan UnixTimspan(DateTime srcDT, DateTimeKind srcDTSpecifiedKindIfNeed)
        {
            srcDT = SpecifiedWithKindIfNeed(srcDT, srcDTSpecifiedKindIfNeed);
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var timespan = srcDT.ToUniversalTime() - epoch;
            return timespan;
        }

        /// <summary>
        /// 将 Unix 毫秒单位时间戳转换为 DateTime 对象
        /// </summary>
        /// <param name="unixTimeStampMs"></param>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static DateTime UnixTimeStampMsToDateTime(double unixTimeStampMs)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddMilliseconds(unixTimeStampMs);
        }

        /// <summary>
        /// 将 Unix 秒单位时间戳转换为 DateTime 对象
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTimeStamp);
        }

        /// <summary>
        /// 获取某个日期的开始时间戳和结束时间戳
        /// </summary>
        /// <param name="srcDateTime">源日期</param>
        /// <param name="srcDTSpecifiedKindIfNeed">源日期的Kind为<see cref="DateTimeKind.Unspecified"/>时，需类型化的类型</param>
        /// <param name="startTimestampMs">计算的开始时间戳</param>
        /// <param name="endTimestampMs">计算的结束时间戳</param>
        public static void GetDateStartAndEndTimestampMs(DateTime srcDateTime, DateTimeKind srcDTSpecifiedKindIfNeed,
            out long startTimestampMs, out long endTimestampMs)
        {
            var date = srcDateTime.Date;
            startTimestampMs = (long)DateHelper.UnixTimspan(date, srcDTSpecifiedKindIfNeed).TotalMilliseconds;
            endTimestampMs = (long)DateHelper.UnixTimspan(date.AddDays(1), srcDTSpecifiedKindIfNeed).TotalMilliseconds - 1;
        }

        /// <summary>
        /// 日期选择控件的默认 culture
        /// </summary>
        public readonly static CultureInfo DefaultDateTimePickerCulture = new CultureInfo("zh-CN")
        {
            DateTimeFormat = new DateTimeFormatInfo { LongDatePattern = "yyyy/MM/dd", ShortDatePattern = "MM/dd", LongTimePattern = "HH:mm:ss", ShortTimePattern = "mm:ss" }
        };
    }
}
