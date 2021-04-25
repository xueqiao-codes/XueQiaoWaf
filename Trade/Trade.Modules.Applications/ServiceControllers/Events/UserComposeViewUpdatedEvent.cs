using NativeModel.Trade;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoWaf.Trade.Modules.Applications.ServiceControllers.Events
{
    /// <summary>
    /// 用户组合视图更新事件
    /// </summary>
    public class UserComposeViewUpdatedEvent : PubSubEvent<NativeComposeView>
    {
    }
}
