using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers.Events
{
    /// <summary>
    /// 组合列表自定义分组列表变化事件
    /// </summary>
    internal class ComposeListCustomGroupsChangedEvent : PubSubEvent<SubscribeDataGroupsChangedEventArgs>
    {
    }
}
