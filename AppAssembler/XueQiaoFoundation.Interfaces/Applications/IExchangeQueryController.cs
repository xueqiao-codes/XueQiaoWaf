using lib.xqclient_base.thriftapi_mediation.Interface;
using NativeModel.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XueQiaoFoundation.Shared.Helper.WeakDelegate;

namespace XueQiaoFoundation.Interfaces.Applications
{
    /// <summary>
    /// 交易所查询管理
    /// </summary>
    public interface IExchangeQueryController
    {
        /// <summary>
        /// 查询所有交易所
        /// </summary>
        /// <param name="handler">结果处理</param>
        /// <returns>请求 id</returns>
        long QueryAllExchanges(ActionDelegateReference<IInterfaceInteractResponse<IEnumerable<NativeExchange>>> handler);

        /// <summary>
        /// 删除查询所有交易所的某个请求
        /// </summary>
        /// <param name="reqId">请求 id</param>
        void RemoveQueryAllExchangesHandler(long reqId);

        /// <summary>
        /// 同步查询所有交易所
        /// </summary>
        /// <param name="ct">取消 token</param>
        /// <returns></returns>
        IInterfaceInteractResponse<IEnumerable<NativeExchange>> QueryAllExchanges(CancellationToken ct);

        /// <summary>
        /// 同步查询某个交易所
        /// </summary>
        /// <param name="exchangeMic">交易所代码</param>
        /// <returns></returns>
        IInterfaceInteractResponse<NativeExchange> QueryExchange(string exchangeMic);

    }
}
