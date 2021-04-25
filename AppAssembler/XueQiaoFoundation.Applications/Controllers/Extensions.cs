using NativeModel.Contract;
using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.quotation;
using xueqiao.trade.hosting.quot.comb.thriftapi;

namespace XueQiaoFoundation.Applications.Controllers
{
    public static class Extensions
    {
        /// <summary>
        /// 将<see cref="QuotationItem"/>转化为<see cref="NativeQuotationItem"/>
        /// </summary>
        public static NativeQuotationItem ToNativeQuotation(this QuotationItem srcQuot)
        {
            if (srcQuot == null) return null;

            var destQuot = new NativeQuotationItem();
            destQuot.ContractSymbol = new ContractSymbol(srcQuot.SledExchangeCode, srcQuot.SledCommodityType, srcQuot.SledCommodityCode, srcQuot.SledContractCode);

            if (srcQuot.__isset.raceTimestampMs)
                destQuot.UpdateTimestampMs = srcQuot.RaceTimestampMs;

            if (srcQuot.__isset.lastPrice)
                destQuot.LastPrice = RectifyQuotationInvalidProperty(srcQuot.LastPrice);

            if (srcQuot.__isset.lastQty)
                destQuot.LastQty = RectifyQuotationInvalidProperty(srcQuot.LastQty);

            if (srcQuot.__isset.bidQty && srcQuot.BidQty != null)
            {
                var list = new List<long>();
                foreach (var i in srcQuot.BidQty)
                {
                    var t = RectifyQuotationInvalidProperty(i);
                    if (t != null) list.Add(t.Value);
                }
                destQuot.BidQty = new ObservableCollection<long>(list);
            }

            if (srcQuot.__isset.bidPrice && srcQuot.BidPrice != null)
            {
                var list = new List<double>();
                foreach (var i in srcQuot.BidPrice)
                {
                    var t = RectifyQuotationInvalidProperty(i);
                    if (t != null) list.Add(t.Value);
                }
                destQuot.BidPrice = new ObservableCollection<double>(list);
            }

            if (srcQuot.__isset.askPrice && srcQuot.AskPrice != null)
            {
                var list = new List<double>();
                foreach (var i in srcQuot.AskPrice)
                {
                    var t = RectifyQuotationInvalidProperty(i);
                    if (t != null) list.Add(t.Value);
                }
                destQuot.AskPrice = new ObservableCollection<double>(list);
            }

            if (srcQuot.__isset.askQty && srcQuot.AskQty != null)
            {
                var list = new List<long>();
                foreach (var i in srcQuot.AskQty)
                {
                    var t = RectifyQuotationInvalidProperty(i);
                    if (t != null) list.Add(t.Value);
                }
                destQuot.AskQty = new ObservableCollection<long>(list);
            }

            if (srcQuot.__isset.volumn)
                destQuot.Volume = RectifyQuotationInvalidProperty(srcQuot.Volumn);

            if (srcQuot.__isset.openInterest)
                destQuot.OpenInterest = RectifyQuotationInvalidProperty(srcQuot.OpenInterest);

            if (srcQuot.__isset.openPrice)
                destQuot.OpenPrice = RectifyQuotationInvalidProperty(srcQuot.OpenPrice);

            if (srcQuot.__isset.highPrice)
                destQuot.HighPrice = RectifyQuotationInvalidProperty(srcQuot.HighPrice);

            if (srcQuot.__isset.lowPrice)
                destQuot.LowPrice = RectifyQuotationInvalidProperty(srcQuot.LowPrice);

            // FIXME: 暂时用 LastPrice 作为 ClosePrice
            if (srcQuot.__isset.lastPrice)
                destQuot.ClosePrice = RectifyQuotationInvalidProperty(srcQuot.LastPrice);

            if (srcQuot.__isset.preSettlementPrice)
                destQuot.PreSettlementPrice = RectifyQuotationInvalidProperty(srcQuot.PreSettlementPrice);

            if (srcQuot.__isset.preOpenInterest)
                destQuot.PreOpenInterest = RectifyQuotationInvalidProperty(srcQuot.PreOpenInterest);

            if (srcQuot.__isset.preClosePrice)
                destQuot.PreClosePrice = RectifyQuotationInvalidProperty(srcQuot.PreClosePrice);

            if (srcQuot.__isset.turnover)
                destQuot.Turnover = RectifyQuotationInvalidProperty(srcQuot.Turnover);

            if (srcQuot.__isset.upperLimitPrice)
                destQuot.UpperLimitPrice = RectifyQuotationInvalidProperty(srcQuot.UpperLimitPrice);

            if (srcQuot.__isset.lowerLimitPrice)
                destQuot.LowerLimitPrice = RectifyQuotationInvalidProperty(srcQuot.LowerLimitPrice);

            if (srcQuot.__isset.averagePrice)
                destQuot.AveragePrice = RectifyQuotationInvalidProperty(srcQuot.AveragePrice);

            return destQuot;
        }

        /// <summary>
        /// 将<see cref="HostingQuotationComb"/>转化为<see cref="NativeCombQuotationItem"/>
        /// </summary>
        public static NativeCombQuotationItem ToNativeCombQuotation(this HostingQuotationComb srcQuot)
        {
            if (srcQuot == null) return null;

            var destQuot = new NativeCombQuotationItem(srcQuot.ComposeGraphId);
            destQuot.CombQuotation = srcQuot.CombItem?.ToNativeQuotation();
            destQuot.LegQuotations = srcQuot.LegItems?.Select(i => i.ToNativeQuotation()).ToArray();

            return destQuot;
        }

        /// <summary>
        /// 纠正不正确的行情信息
        /// </summary>
        private static int? RectifyQuotationInvalidProperty(int? quotationProp)
        {
            if (quotationProp == null) return null;
            if (quotationProp.Value == int.MaxValue || quotationProp.Value == int.MinValue)
                return null;
            return quotationProp;
        }

        /// <summary>
        /// 纠正不正确的行情信息
        /// </summary>
        private static long? RectifyQuotationInvalidProperty(long? quotationProp)
        {
            if (quotationProp == null) return null;
            if (quotationProp.Value == long.MaxValue || quotationProp.Value == long.MinValue)
                return null;
            return quotationProp;
        }

        /// <summary>
        /// 纠正不正确的行情信息
        /// </summary>
        private static double? RectifyQuotationInvalidProperty(double? quotationProp)
        {
            if (quotationProp == null) return null;
            if (quotationProp.Value == double.MaxValue
                || quotationProp.Value == double.MinValue
                || double.IsInfinity(quotationProp.Value)
                || double.IsNaN(quotationProp.Value))
                return null;
            return quotationProp;
        }
    }
}
