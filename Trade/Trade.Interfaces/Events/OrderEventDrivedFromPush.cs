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
    /// 订单事件（该事件只由订单的 push 消息驱动而来）
    /// </summary>
    public class OrderEventDrivedFromPush : PubSubEvent<OrderEventPayload>
    {
    }

    public enum OrderEventType
    {
        Create = 1,     // 创建新订单
        Update =2,      // 更新订单
        Remove = 3      // 移除订单
    }

    /// <summary>
    /// 订单事件消息体
    /// </summary>
    public class OrderEventPayload
    {
        public OrderEventPayload(OrderEventType orderEventType, OrderItemDataModel order, string removedOrderId = null)
        {
            this.OrderEventType = orderEventType;
            this.Order = order;
            this.RemovedOrderId = removedOrderId;
        }

        public readonly OrderEventType OrderEventType;

        /// <summary>
        /// 订单信息。在 OrderEventType 为 <see cref="OrderEventType.Create"/> 或 <see cref="OrderEventType.Update"/> 时设置
        /// </summary>
        public readonly OrderItemDataModel Order;

        /// <summary>
        /// 删除的订单 id。在 OrderEventType 为 <see cref="OrderEventType.Remove"/> 时设置
        /// </summary>
        public readonly string RemovedOrderId;
    }
}
