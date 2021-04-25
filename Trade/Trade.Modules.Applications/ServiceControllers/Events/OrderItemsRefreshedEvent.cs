using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.trade.hosting.arbitrage.thriftapi;

namespace XueQiaoWaf.Trade.Modules.Applications.ServiceControllers.Events
{
    /// <summary>
    /// 订单列表已刷新 event
    /// </summary>
    public class OrderItemsRefreshedEvent : PubSubEvent<IEnumerable<HostingXQOrder>>
    {
    }
}
