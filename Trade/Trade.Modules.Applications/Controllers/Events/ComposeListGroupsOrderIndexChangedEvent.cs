using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers.Events
{
    /// <summary>
    /// 合约列表的分组列表顺序改变的事件
    /// </summary>
    internal class ComposeListGroupsOrderIndexChangedEvent : PubSubEvent<SubscribeDataGroupsOrderIndexChangedEventArgs>
    {
    }
}
