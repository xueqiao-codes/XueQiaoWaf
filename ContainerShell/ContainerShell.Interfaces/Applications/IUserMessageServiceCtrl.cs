using lib.xqclient_base.thriftapi_mediation.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.mailbox.user.message.thriftapi;
using XueQiaoFoundation.Shared.Helper;

namespace ContainerShell.Interfaces.Applications
{
    /// <summary>
    /// `用户消息服务管理` 协议
    /// </summary>
    public interface IUserMessageServiceCtrl
    {
        /// <summary>
        /// 刷新消息列表
        /// </summary>
        /// <param name="forceRefresh">是否强制刷新</param>
        /// <returns></returns>
        Task<IInterfaceInteractResponse<QueryItemsByPageResult<UserMessage>>> RefreshMessageList(bool forceRefresh);

        /// <summary>
        /// 加载更旧的消息
        /// </summary>
        /// <param name="referenceTimestamp">参考时间。如果不提供，则参考当前最旧的一条消息创建时间；如果不存在参考消息，则参考当前时间</param>
        /// <returns></returns>
        Task<IInterfaceInteractResponse<QueryItemsByPageResult<UserMessage>>> 
            LoadOldMessages(long? referenceTimestamp = null);

        /// <summary>
        /// 加载更新的消息
        /// </summary>
        /// <param name="referenceTimestamp">参考时间。如果不提供，则参考当前最新的一条消息创建时间；如果不存在参考消息，则参考当前时间</param>
        /// <returns></returns>
        Task<IInterfaceInteractResponse<QueryItemsByPageResult<UserMessage>>>
            LoadNewMessages(long? referenceTimestamp = null);

        /// <summary>
        /// 设置消息为已读
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        Task<IInterfaceInteractResponse> MarkMessageAsRead(long messageId);

        /// <summary>
        /// 获取当前列表最新的消息
        /// </summary>
        /// <returns></returns>
        UserMessage GetCurrentNewestMessage();

        /// <summary>
        /// 获取当前列表最旧的消息
        /// </summary>
        /// <returns></returns>
        UserMessage GetCurrentOldestMessage();
    }
}
