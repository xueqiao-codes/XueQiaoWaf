using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;

namespace XueQiaoWaf.Trade.Modules.Applications.ServiceControllers.Events
{
    /// <summary>
    /// 订阅组合已添加的事件
    /// </summary>
    internal class SubscribeComposeAddedEvent : PubSubEvent<SubscribeComposeDataModel>
    {
    }
}
