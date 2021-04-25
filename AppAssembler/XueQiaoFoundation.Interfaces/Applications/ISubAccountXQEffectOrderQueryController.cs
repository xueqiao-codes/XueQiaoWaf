using lib.xqclient_base.thriftapi_mediation.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using xueqiao.trade.hosting.arbitrage.thriftapi;
using XueQiaoFoundation.Shared.Helper.WeakDelegate;

namespace XueQiaoFoundation.Interfaces.Applications
{
    /// <summary>
    /// 子账户雪橇未清理订单的查询控制器
    /// </summary>
    public interface ISubAccountXQEffectOrderQueryController
    {
        /// <summary>
        /// 异步查询子账户的订单
        /// </summary>
        /// <param name="subAccountId">子账户 id</param>
        /// <param name="handler">结果处理</param>
        /// <returns>请求 id</returns>
        long SubAccountQueryXQOrders(long subAccountId,
            ActionDelegateReference<IInterfaceInteractResponse<IEnumerable<HostingXQOrderWithTradeList>>> handler);

        /// <summary>
        /// 移除查询子账户的订单的处理
        /// </summary>
        /// <param name="subAccountId">子账户 id</param>
        /// <param name="reqId">请求 id</param>
        void RemoveSubAccountQueryXQOrdersHandler(long subAccountId, long reqId);

        /// <summary>
        /// 同步查询子账户的订单
        /// </summary>
        /// <param name="subAccountId">子账户 id</param>
        /// <returns></returns>
        IInterfaceInteractResponse<IEnumerable<HostingXQOrderWithTradeList>> SubAccountQueryXQOrdersSync(long subAccountId, CancellationToken cancelToken);
    }
}
