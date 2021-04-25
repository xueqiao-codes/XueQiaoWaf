using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business_foundation_lib.jsbridge
{
    public class BridgeQuotation
    {
        // 更新时间
        public long UpdateTimestampMs { get; set; }

        // 最新价
        public double? LastPrice { get; set; }

        // 最新量
        public long? LastQty { get; set; }

        // 买量
        public long? BidQty1 { get; set; }

        // 买价
        public double? BidPrice { get; set; }

        // 卖价
        public double? AskPrice1 { get; set; }

        // 卖量
        public long? AskQty { get; set; }
        
        // 涨跌
        public double? IncreasePrice { get; set; }

        // 涨幅（（最新价-昨结算价）/昨结算价）
        public double? IncreasePriceRate { get; set; }

        // 成交量
        public long? Volume { get; set; }

        // 持仓量
        public long? OpenInterest { get; set; }

        // 日增量（持仓量-昨持仓量）
        public long? DailyIncrementOpenInterest { get; set; }

        // 开盘价
        public double? OpenPrice { get; set; }


        // 最高价
        public double? HighPrice { get; set; }


        // 最低价
        public double? LowPrice { get; set; }


        // 收盘价
        public double? ClosePrice { get; set; }


        // 昨结算价
        public double? PreSettlementPrice { get; set; }


        // 昨持仓量
        public long? PreOpenInterest { get; set; }


        // 昨收盘价
        public double? PreClosePrice { get; set; }


        // 成交额
        public double? Turnover { get; set; }

        // 涨停价
        public double? UpperLimitPrice { get; set; }


        // 跌停价
        public double? LowerLimitPrice { get; set; }


        // (成交)均价
        public double? AveragePrice { get; set; }
    }
}
