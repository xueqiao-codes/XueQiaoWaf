using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.mailbox.user.message.thriftapi;

namespace ContainerShell.Interfaces.Services
{
    /// <summary>
    /// `用户消息数据服务` 协议
    /// </summary>
    public interface IUserMessageService : INotifyPropertyChanged
    {
        /// <summary>
        /// 用户消息列表
        /// </summary>
        ObservableCollection<UserMessage> MessageItems { get; }
        
        /// <summary>
        /// 未读消息数量
        /// </summary>
        int UnreadMessageItemCount { get; }
    }
}
