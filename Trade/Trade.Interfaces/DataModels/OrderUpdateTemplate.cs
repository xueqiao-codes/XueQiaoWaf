using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.trade.hosting.arbitrage.thriftapi;

namespace XueQiaoWaf.Trade.Interfaces.DataModels
{
    /// <summary>
    /// 订单信息修改模板
    /// </summary>
    public class OrderUpdateTemplateBase
    {
        /// <summary>
        /// 用于更新的`订单状态`
        /// </summary>
        public Tuple<ClientXQOrderState> OrderState { get; set; }

        /// <summary>
        /// 用于更新的`订单状态详情`
        /// </summary>
        public Tuple<HostingXQOrderState> OrderStateDetail { get; set; }

        /// <summary>
        /// 用于更新的`版本号`
        /// </summary>
        public Tuple<int> Version { get; set; }

        /// <summary>
        /// 用于更新的`订单创建时间`
        /// </summary>
        public Tuple<long> CreateTimestampMs { get; set; }

        /// <summary>
        /// 用于更新的`订单更新时间`
        /// </summary>
        public Tuple<long> UpdateTimestamMs { get; set; }

        /// <summary>
        /// 用于更新的`引向订单 id`
        /// </summary>
        public Tuple<string> SourceOrderId { get; set; }

        /// <summary>
        /// 用于更新的`触发后产生的订单 id`
        /// </summary>
        public Tuple<string> ActionOrderId { get; set; }

        /// <summary>
        /// 有效时间过期时间
        /// </summary>
        public Tuple<long?> OrderEffectEndTimestampMs { get; set; }
    }

    /// <summary>
    /// 委托单修改模板
    /// </summary>
    public class OrderUpdateTemplate_Entrusted : OrderUpdateTemplateBase
    {
        /// <summary>
        /// 用于更新的`订单成交信息`
        /// </summary>
        public Tuple<HostingXQTradeSummary> TradeSummary { get; set; }

        /// <summary>
        /// 用于更新的`有效期`
        /// </summary>
        public Tuple<HostingXQEffectDate> EffectDate { get; set; }

        /// <summary>
        /// 用于更新的`组合标的订单执行参数`
        /// </summary>
        public Tuple<HostingXQComposeLimitOrderExecParams> ComposeOrderExecParams { get; set; }

    }

    /// <summary>
    /// 预埋单修改模板
    /// </summary>
    public class OrderUpdateTemplate_Parked : OrderUpdateTemplateBase
    {

    }

    /// <summary>
    /// 条件单修改模板
    /// </summary>
    public class OrderUpdateTemplate_Condition : OrderUpdateTemplateBase
    {
        /// <summary>
        /// 用于更新的`条件`
        /// </summary>
        public Tuple<IEnumerable<HostingXQCondition>> Conditions { get; set; }

        /// <summary>
        /// 用于更新的`标签`
        /// </summary>
        public Tuple<HostingXQConditionOrderLabel?> OrderLabel { get; set; }

        /// <summary>
        /// 用于更新的`有效期`
        /// </summary>
        public Tuple<HostingXQEffectDate> EffectDate { get; set; }
    }
}
