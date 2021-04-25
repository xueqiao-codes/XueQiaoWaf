using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeModel.Trade
{
    /**
     * 该文件定义了该命名空间的所有参考项
     **/
     
     
    /// <summary>
    /// 组合视图行情订阅状态
    /// </summary>
    public enum ClientComposeViewSubscribeStatus
    {
        UNSUBSCRIBED = 0,
        SUBSCRIBED = 1
    }

    /// <summary>
    /// 客户端的交易方向类型
    /// </summary>
    public enum ClientTradeDirection
    {
        BUY = 0,   
        SELL = 1,
    }

    /// <summary>
    /// 客户端雪橇订单状态。综合了服务端和客户端的一些临时状态
    /// </summary>
    public enum ClientXQOrderState
    {
        XQ_ORDER_CREATED = 1,
        XQ_ORDER_CANCELLED = 2,
        XQ_ORDER_CANCELLING = 3,
        XQ_ORDER_SUSPENDED = 4,
        XQ_ORDER_SUSPENDING = 5,
        XQ_ORDER_RUNNING = 6,
        XQ_ORDER_STARTING = 7,
        XQ_ORDER_FINISHED = 8,
        XQ_ORDER_FINISHING = 9,

        // 本地不确切状态：未知
        ClientInaccurate_Unkown = 1000000,

        // 本地状态：请求创建中
        Client_RequestCreating = 999999,

        // 本地状态：请求撤销中
        Client_RequestRevoking = 999998,

        // 本地状态：请求暂停中
        Client_RequestSuspending = 999997,

        // 本地状态：请求启动中
        Client_RequestResuming = 999996,

        // 本地状态：请求强追中
        Client_RequestStrongChasing = 999995,
    }

    /// <summary>
    /// 客户端订单标的类型
    /// </summary>
    public enum ClientXQOrderTargetType
    {
        CONTRACT_TARGET = 1,     // 合约标的
        COMPOSE_TARGET = 2,      // 组合标的        
    }

    /// <summary>
    /// 客户端订单下单类型
    /// </summary>
    public enum ClientPlaceOrderType
    {
        PRICE_LIMIT = 1,    // 限价单
        PARKED = 2,         // 预埋单
        BUY_STOP_LOST = 3,      // 买入止损单
        SELL_STOP_LOST = 4,     // 卖出止损单
        BUY_STOP_PROFIT = 5,    // 买入止盈单
        SELL_STOP_PROFIT = 6,   // 卖出止盈单
    }

    /// <summary>
    /// 成交来源类型
    /// </summary>
    public enum ClientTradeItemSourceType
    {
        Unkown              = 0,    // 未知
        ContractTarget      = 1,    // 合约
        ComposeTarget       = 2,    // 组合
        ComposeTargetLame   = 3,    // 组合(瘸腿)
    }

    /// <summary>
    /// 资金货币类型参考
    /// </summary>
    public struct ClientFundCurrencyReference
    {
        public const string AUD = "AUD";    // 澳元
        public const string CAD = "CAD";    // 加元
        public const string CHF = "CHF";    // 瑞士法郎
        public const string CNY = "CNY";    // 人民币
        public const string CNH = "CNH";    // 离岸人民币
        public const string EUR = "EUR";    // 欧元
        public const string GBP = "GBP";    // 英镑
        public const string HKD = "HKD";    // 港币
        public const string JPY = "JPY";    // 日元
        public const string KRW = "KRW";    // 韩元
        public const string MYR = "MYR";    // 马来西亚林吉特
        public const string SGD = "SGD";    // 新加坡元
        public const string THB = "THB";    // 泰铢
        public const string USD = "USD";    // 美元

        public readonly static string[] AllClientFundCurrencyList = new string[] 
        {
             CNY,USD,CNH,HKD,EUR,GBP,JPY,AUD,CAD,CHF,KRW,MYR,SGD,THB,
        };
    }


    /// <summary>
    /// 雪橇标的持仓来源渠道类型
    /// </summary>
    public enum XqTargetPositionDataSourceChannel
    {
        FROM_XQ_CONTRACT_TRADE = 1,                   // 合约标的成交
        FROM_XQ_COMPOSE_TRADE = 2,                    // 组合标的成交
        FROM_XQ_PARTIAL_COMPOSE_TRADE = 3,            // 组合标的部分成交（瘸腿）

        FROM_CONTRACT_ASSIGNATION = 10,               // 合约分配

        FROM_COMPOSE_CONSTRUCTION = 20,               // 组合构造
        FROM_COMPOSE_REVERSE_CONSTRUCTION = 21,       // 组合反向构造
        FROM_CONTRACT_MERGE = 22,                     // 合并合约

        FROM_COMPOSE_TRADE_DISASSEMBLY = 30,          // 成交的组合标的拆解
        FROM_COMPOSE_CONSTRUCTION_DISASSEMBLY = 31,   // 构造的组合拆解
        FROM_COMPOSE_DISASSEMBLY = 32,                // 组合的拆解
    }

    /// <summary>
    /// 雪橇组合标的订单执行参数发单方式
    /// </summary>
    public enum XQComposeOrderExecParamsSendType
    {
        PARALLEL_LEG = 1,               // 市价并发
        SERIAL_LEG_PRICE_BEST = 2,      // 市价逐腿
        SERIAL_LEG_PRICE_TRYING = 3,    // 限价逐腿
    }
}
