using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoWaf.Trade.Modules.Applications.DataModels
{
    /// <summary>
    /// 订阅合约列表显示的字段类型
    /// </summary>
    public enum SubscribeContractListDisplayColumn
    {
        Name = 1,        // 名称
        LastPrice   = 2, // 最新价
        LastQty     = 3, // 最新量
        BidQty      = 4, // 买量
        BidPrice    = 5, // 买价
        AskPrice    = 6, // 卖价
        AskQty      = 7, // 卖量
        IncreasePrice     = 8,  // 涨跌
        IncreasePriceRate = 9,  // 涨幅
        Volume = 10,            // 成交量
        OpenInterest = 11,      // 持仓量
        DailyIncrementOpenInterest = 12, // 日增仓
        UpdateTime = 13,        // 更新时间
        SubscribeState = 14,    // 订阅状态

        SubscribeStateMsg = 30, //订阅状态信息
        OpenPrice   = 31, // 开盘价
        HighPrice   = 32, // 最高价
        LowPrice    = 33, // 最低价
        ClosePrice  = 34, // 收盘价
        PreSettlementPrice  = 35,   // 昨结算价
        PreOpenInterest     = 36,   // 昨持仓量
        PreClosePrice       = 37,   // 昨收盘价
        Turnover            = 38,   // 成交额
        UpperLimitPrice     = 39,   // 涨停价
        LowerLimitPrice     = 40,   // 跌停价
        AveragePrice        = 41,   // 成交均价
        Exchange            = 43,   // 交易所
    }
}
