using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.BusinessResources.DataModels;

namespace XueQiaoWaf.Trade.Interfaces.Events
{
    /// <summary>
    /// 订单成交事件（该事件只由订单成交的 push 消息驱动而来）
    /// </summary>
    public class OrderTradedEventDrivedFromPush : PubSubEvent<OrderTradedEventPayload>
    {
    }

    /// <summary>
    /// 订单成交事件消息体
    /// </summary>
    public class OrderTradedEventPayload
    {
        public OrderTradedEventPayload(TradeItemDataModel trade)
        {
            this.Trade = trade;
        }

        public readonly TradeItemDataModel Trade;
    }
}
