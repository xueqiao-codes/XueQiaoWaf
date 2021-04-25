using lib.xqclient_base.logger;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using TimeZoneConverter;
using xueqiao.contract;
using XueQiaoFoundation.Shared.Helper;

namespace XueQiaoWaf.Trade.Modules.Applications.DataModels
{
    public class ContractTradeTimeSpanDailyDetail : Model
    {
        private TimeZoneInfo sourceDataTimeZone;
        public TimeZoneInfo SourceDataTimeZone
        {
            get { return sourceDataTimeZone; }
            set { SetProperty(ref sourceDataTimeZone, value); }
        }

        private TimeZoneInfo destDataTimeZone;
        public TimeZoneInfo DestDataTimeZone
        {
            get { return destDataTimeZone; }
            set { SetProperty(ref destDataTimeZone, value); }
        }

        private ContractTradeDailyTimeSpans sourceZoneDailyDataItem;
        // 合约的源时区的交易时间段
        public ContractTradeDailyTimeSpans SourceZoneDailyDataItem
        {
            get { return sourceZoneDailyDataItem; }
            set { SetProperty(ref sourceZoneDailyDataItem, value); }
        }

        private ContractTradeDailyTimeSpans destZoneDailyDataItem;
        // 合约目的时区的交易时间段
        public ContractTradeDailyTimeSpans DestZoneDailyDataItem
        {
            get { return destZoneDailyDataItem; }
            set { SetProperty(ref destZoneDailyDataItem, value); }
        }
        

        public static ContractTradeTimeSpan GenerateTradeTimeSpan(DateTimeOffset date, TTimeSpan timeSpan, TimeZoneInfo dataTimeZone)
        {
            var timeSpanBeginEndParts = timeSpan.Timespan.Split('-').Select(i => i.Trim()).ToArray();
            if (timeSpanBeginEndParts.Length != 2) return null;

            var beginTimeStr = timeSpanBeginEndParts.First();
            var endTimeStr = timeSpanBeginEndParts.Last();
            var tradeIimeSpan = new ContractTradeTimeSpan();
            tradeIimeSpan.Date = date;
            tradeIimeSpan.State = timeSpan.TimeSpanState;

            var dateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            var formatedBegineTime = $"{date.ToString("yyyy-MM-dd")} {beginTimeStr}";
            var formatedEndTime = $"{date.ToString("yyyy-MM-dd")} {endTimeStr}";
            
            var beginDateTimeOffset = ContractTradeTimeSpan.ParseDateTimeWithOffset(formatedBegineTime, dateTimeFormat, dataTimeZone);
            var endDateTimeOffset = ContractTradeTimeSpan.ParseDateTimeWithOffset(formatedEndTime, dateTimeFormat, dataTimeZone);

            if (beginDateTimeOffset != null)
                tradeIimeSpan.BeginDateTime = beginDateTimeOffset.Value;

            if (endDateTimeOffset != null)
                tradeIimeSpan.EndDateTime = endDateTimeOffset.Value;

            return tradeIimeSpan;
        }

        public static ContractTradeDailyTimeSpans[] GenerateTradeDailyTimeSpansItems(IEnumerable<DateTimeSpan> sourceDateTimeSpans, 
            TimeZoneInfo srcDateTimeZone,
            Func<TTimeSpan, bool> timeSpanFilter)
        {
            if (sourceDateTimeSpans == null) return null;

            var discreteTimeSpans = new List<ContractTradeTimeSpan>();
            foreach (var dateSpanObj in sourceDateTimeSpans)
            {
                var timeSpans = dateSpanObj.TTimeSpan?.ToArray() ?? new TTimeSpan[] { };
                timeSpans = timeSpans.OrderBy(i => i.Timespan).ToArray();
                var dateTimeOffset = ContractTradeTimeSpan.ParseDateTimeWithOffset(dateSpanObj.Date, "yyyy-MM-dd", srcDateTimeZone);
                if (dateTimeOffset == null) continue;
                foreach (var timeSpan in timeSpans)
                {
                    var passFilter = timeSpanFilter?.Invoke(timeSpan) ?? true;
                    if (passFilter)
                    {
                        discreteTimeSpans.Add(GenerateTradeTimeSpan(dateTimeOffset.Value, timeSpan, srcDateTimeZone));
                    }
                }
            }

            var dateKeyedTimespans = discreteTimeSpans.GroupBy(i => i.Date).Select(i => new Tuple<DateTimeOffset, List<ContractTradeTimeSpan>>(i.Key, i.ToList())).ToArray();
            for (var idx = dateKeyedTimespans.Length - 1; idx >= 0; idx--)
            {
                var dateKeyedItem = dateKeyedTimespans[idx];
                var firstTimespan = dateKeyedItem.Item2.FirstOrDefault();
                if (firstTimespan != null)
                {
                    if (idx > 0)
                    {
                        var prevDateKeyedItem = dateKeyedTimespans[idx - 1];
                        var prevDateLastTimeSpan = prevDateKeyedItem.Item2.LastOrDefault();
                        if (prevDateLastTimeSpan != null)
                        {
                            if (firstTimespan.State == prevDateLastTimeSpan.State
                                && (firstTimespan.BeginDateTime - prevDateLastTimeSpan.EndDateTime) <= new TimeSpan(0, 0, 1))
                            {
                                dateKeyedItem.Item2.Remove(firstTimespan);
                                prevDateKeyedItem.Item2.Remove(prevDateLastTimeSpan);

                                var newTimespan = new ContractTradeTimeSpan
                                {
                                    Date = prevDateLastTimeSpan.Date,
                                    BeginDateTime = prevDateLastTimeSpan.BeginDateTime,
                                    EndDateTime = firstTimespan.EndDateTime,
                                    State = prevDateLastTimeSpan.State
                                };
                                prevDateKeyedItem.Item2.Add(newTimespan);
                            }
                        }
                    }
                }
            }

            return dateKeyedTimespans.Select(i =>
                {
                    var timeSpansObj = new ContractTradeDailyTimeSpans { Date = i.Item1 };
                    timeSpansObj.TimeSpans.AddRange(i.Item2.ToArray());
                    return timeSpansObj;
                }).ToArray();
        }

        public static DateTime? ConvertDateTime2DestTimeZone(DateTime srcDateTime, string srcTimeZoneId, string destTimeZoneId)
        {
            TimeZoneInfo srcTimeZone = null, destTimeZone = null;
            Exception getTimeZoneExp = null;
            try
            {
                srcTimeZone = TZConvert.GetTimeZoneInfo(srcTimeZoneId);
                destTimeZone = TZConvert.GetTimeZoneInfo(destTimeZoneId);
            }
            catch (Exception e)
            {
                getTimeZoneExp = e;
            }

            if (srcTimeZone == null || destTimeZone == null || getTimeZoneExp != null)
            {
                AppLog.Error($"Failed GetTimeZoneInfo with id `{srcTimeZoneId}` or `{destTimeZoneId}`. e:{getTimeZoneExp}");
                return null;
            }

            return ConvertDateTime2DestTimeZone(srcDateTime, srcTimeZone, destTimeZone);
        }

        public static DateTime ConvertDateTime2DestTimeZone(DateTime srcDateTime, TimeZoneInfo srcTimeZone, TimeZoneInfo destTimeZone)
        {
            return TimeZoneInfo.ConvertTime(srcDateTime, srcTimeZone, destTimeZone);
        }

        public static DateTimeOffset ConvertDateTime2DestTimeZone(DateTimeOffset srcDateTime, TimeZoneInfo destTimeZone)
        {
            return TimeZoneInfo.ConvertTime(srcDateTime, destTimeZone);
        }

        public static IEnumerable<ContractTradeTimeSpanDailyDetail> GenerateDailyDetails(IEnumerable<DateTimeSpan> sourceDateTimeSpans,
            string sourceTimeZoneId, string destTimeZoneId)
        {
            if (sourceDateTimeSpans == null) return null;

            TimeZoneInfo srcDateTimeZone = null, destDateTimeZone = null;
            Exception getTimeZoneExp = null;
            try
            {
                srcDateTimeZone = TZConvert.GetTimeZoneInfo(sourceTimeZoneId);
                destDateTimeZone = TZConvert.GetTimeZoneInfo(destTimeZoneId);
            }
            catch (Exception e)
            {
                getTimeZoneExp = e;
            }

            if (srcDateTimeZone == null || destDateTimeZone == null || getTimeZoneExp != null)
            {
                AppLog.Error($"Failed GetTimeZoneInfo with id `{sourceTimeZoneId}` or `{destTimeZoneId}`. e:{getTimeZoneExp}");
                return null;
            }

            // 筛除 Closed, Rest 状态的项目
            var srcDailyTimeSpansItems = GenerateTradeDailyTimeSpansItems(sourceDateTimeSpans, srcDateTimeZone,
                timeSpan => timeSpan.TimeSpanState == TimeSpanState.T_OPEN || timeSpan.TimeSpanState == TimeSpanState.T_PLUS_ONE_OPEN);
            if (srcDailyTimeSpansItems == null) srcDailyTimeSpansItems = new ContractTradeDailyTimeSpans[] { };

            var dailyDetails = new List<ContractTradeTimeSpanDailyDetail>();
            foreach (var srcDailyTimeSpansItem in srcDailyTimeSpansItems)
            {
                var destZoneTimeSpanItems = new List<ContractTradeTimeSpan>();
                foreach (var srcTimeSpan in srcDailyTimeSpansItem.TimeSpans)
                {
                    var destBeginDateTime = ConvertDateTime2DestTimeZone(srcTimeSpan.BeginDateTime, destDateTimeZone);
                    var destEndDateTime = ConvertDateTime2DestTimeZone(srcTimeSpan.EndDateTime, destDateTimeZone);
                    if (destBeginDateTime == null || destEndDateTime == null) continue;
                    var destZoneTimeSpan = new ContractTradeTimeSpan { Date = destBeginDateTime, BeginDateTime = destBeginDateTime, EndDateTime = destEndDateTime, State = srcTimeSpan.State };
                    destZoneTimeSpanItems.Add(destZoneTimeSpan);
                }

                var destZoneDate = destZoneTimeSpanItems.FirstOrDefault()?.BeginDateTime;
                if (destZoneDate != null)
                {
                    var destDailyTimeSpansItem = new ContractTradeDailyTimeSpans { Date = destZoneDate.Value };
                    destDailyTimeSpansItem.TimeSpans.AddRange(destZoneTimeSpanItems.ToArray());

                    dailyDetails.Add(new ContractTradeTimeSpanDailyDetail
                    {
                        SourceDataTimeZone = srcDateTimeZone,
                        DestDataTimeZone = destDateTimeZone,
                        SourceZoneDailyDataItem = srcDailyTimeSpansItem,
                        DestZoneDailyDataItem = destDailyTimeSpansItem,
                    });
                }
            }

            return dailyDetails;
        }
    }

    public class ContractTradeTimeSpan
    {
        public DateTimeOffset Date {  get; set; }

        public DateTimeOffset BeginDateTime { get; set; }

        public DateTimeOffset EndDateTime { get; set; }
        
        public TimeSpanState State { get; set; }

        public static DateTimeOffset? ParseDateTimeWithOffset(string formatedSrcDateTime, string srcDateTimeFormat, TimeZoneInfo srcDateTimezone)
        {
            // formatedSrcDateTime 是合约的当地时间
            DateTime? srcDateTime = null;
            try
            {
                srcDateTime = DateTime.ParseExact(formatedSrcDateTime, srcDateTimeFormat, CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                AppLog.Error($"Failed to parse `{formatedSrcDateTime}` using format `{srcDateTimeFormat}`. e:{e}");
            }

            if (srcDateTime == null) return null;

            int year = srcDateTime.Value.Year,
                month = srcDateTime.Value.Month,
                day = srcDateTime.Value.Day,
                hour = srcDateTime.Value.Hour,
                minute = srcDateTime.Value.Minute,
                second = srcDateTime.Value.Second;

            var utcDate = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc);
            var offset = srcDateTimezone.GetUtcOffset(utcDate);
            return new DateTimeOffset(year, month, day, hour, minute, second, offset);
        }
    }

    public class ContractTradeDailyTimeSpans
    {
        public ContractTradeDailyTimeSpans()
        {
            TimeSpans = new ObservableCollection<ContractTradeTimeSpan>();
        }

        public DateTimeOffset Date { get; set; }

        public ObservableCollection<ContractTradeTimeSpan> TimeSpans { get; private set; }
    }
}
