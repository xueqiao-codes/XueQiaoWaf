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
    public interface IUserSubAccountRelatedItemQueryController
    {
        /// <summary>
        /// 异步查询登录用户的关联子账户列表
        /// </summary>
        /// <param name="handler">结果处理</param>
        /// <returns>请求 id</returns>
        long QueryUserSubAccountRelatedItems(ActionDelegateReference<IInterfaceInteractResponse<IEnumerable<HostingSubAccountRelatedItem>>> handler);

        /// <summary>
        /// 删除查询登录用户的关联子账户列表的某个请求
        /// </summary>
        /// <param name="reqId">请求 id</param>
        void RemoveQueryUserSubAccountRelatedItemsHandler(long reqId);

        /// <summary>
        /// 同步查询登录用户的关联子账户列表
        /// </summary>
        /// <param name="cancelToken">取消 token</param>
        /// <returns></returns>
        IInterfaceInteractResponse<IEnumerable<HostingSubAccountRelatedItem>> QueryUserSubAccountRelatedItems(CancellationToken cancelToken);
    }
}
