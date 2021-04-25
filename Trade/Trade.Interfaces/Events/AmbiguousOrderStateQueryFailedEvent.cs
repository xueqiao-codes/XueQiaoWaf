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
    /// 不明确状态订单的状态获取失败通知
    /// </summary>
    public class AmbiguousOrderStateQueryFailedEvent : PubSubEvent<OrderItemDataModel>
    {
    }
}
