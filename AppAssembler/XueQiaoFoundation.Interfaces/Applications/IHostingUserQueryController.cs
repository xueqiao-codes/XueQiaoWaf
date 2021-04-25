using lib.xqclient_base.thriftapi_mediation.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using xueqiao.trade.hosting;
using XueQiaoFoundation.Shared.Helper.WeakDelegate;

namespace XueQiaoFoundation.Interfaces.Applications
{
    public interface IHostingUserQueryController
    {
        /// <summary>
        /// 异步查询所有用户
        /// </summary>
        /// <param name="handler">结果处理</param>
        /// <returns>请求 id</returns>
        long QueryAllUsers(ActionDelegateReference<IInterfaceInteractResponse<IEnumerable<HostingUser>>> handler);

        /// <summary>
        /// 删除查询所有用户的某个请求
        /// </summary>
        /// <param name="reqId">请求 id</param>
        void RemoveQueryAllUsersHandler(long reqId);

        /// <summary>
        /// 同步查询所有用户
        /// </summary>
        /// <param name="cancelToken">取消 token</param>
        /// <returns></returns>
        IInterfaceInteractResponse<IEnumerable<HostingUser>> QueryAllUsers(CancellationToken cancelToken);

        /// <summary>
        /// 异步查询某个用户
        /// </summary>
        /// <param name="userId">某个用户id</param>
        /// <param name="handler">结果处理</param>
        /// <returns>请求 id</returns>
        long QueryUser(int userId, ActionDelegateReference<IInterfaceInteractResponse<HostingUser>> handler);

        /// <summary>
        /// 删除查询某个用户的某个请求
        /// </summary>
        /// <param name="reqId">请求 id</param>
        void RemoveQueryUserHandler(int userId, long reqId);

        /// <summary>
        /// 同步查询某个用户
        /// </summary>
        /// <param name="userId">某个用户id</param>
        /// <param name="cancelToken">取消token</param>
        /// <returns></returns>
        IInterfaceInteractResponse<HostingUser> QueryUser(int userId, CancellationToken cancelToken);
    }
}
