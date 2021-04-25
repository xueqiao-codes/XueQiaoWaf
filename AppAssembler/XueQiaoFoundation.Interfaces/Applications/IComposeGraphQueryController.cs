using lib.xqclient_base.thriftapi_mediation.Interface;
using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.Shared.Helper.WeakDelegate;

namespace XueQiaoFoundation.Interfaces.Applications
{
    public interface IComposeGraphQueryController
    {
        /// <summary>
        /// 异步查询某个组合图
        /// </summary>
        /// <param name="composeId">组合id</param>
        /// <param name="handler">结果处理</param>
        /// <returns>请求 id</returns>
        long QueryComposeGraph(long composeId, ActionDelegateReference<IInterfaceInteractResponse<NativeComposeGraph>> handler);

        /// <summary>
        /// 删除查询单个组合图的某个 handler
        /// <param name="composeId">组合id</param>
        /// <param name="reqId">请求id</param>
        /// </summary>
        void RemoveQueryComposeGraphHandler(long composeId, long reqId);

        /// <summary>
        /// 同步查询某个组合图
        /// </summary>
        /// <param name="composeId">组合id</param>
        /// <returns></returns>
        IInterfaceInteractResponse<NativeComposeGraph> QueryComposeGraph(long composeId);
    }
}
