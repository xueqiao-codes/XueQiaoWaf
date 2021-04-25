using lib.xqclient_base.thriftapi_mediation.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using xueqiao.trade.hosting;
using XueQiaoFoundation.Shared.Helper.WeakDelegate;

namespace Manage.Applications.ServiceControllers
{
    /// <summary>
    /// 操作账户查询 controller
    /// </summary>
    internal interface IManageSubAccountQueryCtrl
    {
        /// <summary>
        /// 查询所有操作账户
        /// </summary>
        /// <param name="handler">结果处理</param>
        /// <returns>请求 id</returns>
        long QueryAllSubAccounts(ActionDelegateReference<IInterfaceInteractResponse<IEnumerable<HostingSubAccount>>> handler);

        /// <summary>
        /// 删除查询所有操作账户的某个请求
        /// </summary>
        /// <param name="reqId">请求 id</param>
        void RemoveQueryAllSubAccountsHandler(long reqId);

        /// <summary>
        /// 同步查询所有操作账户
        /// </summary>
        /// <param name="ct">取消 token</param>
        /// <returns></returns>
        IInterfaceInteractResponse<IEnumerable<HostingSubAccount>> QueryAllSubAccounts(CancellationToken ct);

        /// <summary>
        /// 同步查询某个操作账户
        /// </summary>
        /// <param name="subAccountId">操作账户 id</param>
        /// <returns></returns>
        IInterfaceInteractResponse<HostingSubAccount> QuerySubAccount(long subAccountId);
    }
}
