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
    /// 资金账户查询 controller
    /// </summary>
    internal interface IManageFundAccountQueryCtrl
    {
        /// <summary>
        /// 查询所有资金账户
        /// </summary>
        /// <param name="handler">结果处理</param>
        /// <returns>请求 id</returns>
        long QueryAllFundAccounts(ActionDelegateReference<IInterfaceInteractResponse<IEnumerable<HostingTradeAccount>>> handler);

        /// <summary>
        /// 删除查询所有资金账户的某个请求
        /// </summary>
        /// <param name="reqId">请求 id</param>
        void RemoveQueryAllFundAccountsHandler(long reqId);

        /// <summary>
        /// 同步查询所有资金账户
        /// </summary>
        /// <param name="ct">取消 token</param>
        /// <returns></returns>
        IInterfaceInteractResponse<IEnumerable<HostingTradeAccount>> QueryAllFundAccounts(CancellationToken ct);

        /// <summary>
        /// 同步查询某个资金账户
        /// </summary>
        /// <param name="fundAccountId">资金账户 id</param>
        /// <returns></returns>
        IInterfaceInteractResponse<HostingTradeAccount> QueryFundAccount(long fundAccountId);
    }
}
