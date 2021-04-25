using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.mailbox.user.message.thriftapi;

namespace ContainerShell.Interfaces.Events
{
    /// <summary>
    /// 发现新的用户消息事件
    /// </summary>
    public class FindNewUserMessagesEvent : PubSubEvent<FindNewUserMessagesEventPayload>
    {
    }

    /// <summary>
    /// 事件消息体
    /// </summary>
    public class FindNewUserMessagesEventPayload
    {
        public IEnumerable<UserMessage> NewMessages;
    }
}
