using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NativeModel.Trade;
using XueQiaoFoundation.Shared.Helper.WeakDelegate;
using System.Threading;
using lib.xqclient_base.thriftapi_mediation.Interface;

namespace XueQiaoFoundation.Interfaces.Applications
{
    /// <summary>
    /// 登录用户的组合视图查询控制器
    /// </summary>
    public interface IUserComposeViewQueryController
    {
        /// <summary>
        /// 异步查询当前登录用户的当前组合视图列表
        /// </summary>
        /// <param name="handler">结果处理</param>
        /// <returns>请求 id</returns>
        long QueryCurrentComposeViewList(ActionDelegateReference<IInterfaceInteractResponse<IEnumerable<NativeComposeViewDetail>>> handler);

        /// <summary>
        /// 删除查询所有组合视图列表的某个 handler
        /// </summary>
        /// <param name="reqId">请求id</param>
        void RemoveQueryCurrentComposeViewListHandler(long reqId);

        /// <summary>
        /// 同步查询当前用户的当前组合视图列表
        /// </summary>
        /// <param name="ct">取消token</param>
        /// <returns></returns>
        IInterfaceInteractResponse<IEnumerable<NativeComposeViewDetail>> QueryCurrentComposeViewList(CancellationToken ct);


        /// <summary>
        /// 异步查询当前登录用户的现有组合视图
        /// </summary>
        /// <param name="composeId">组合id</param>
        /// <param name="handler">结果处理</param>
        /// <returns>请求 id</returns>
        long QueryCurrentComposeView(long composeId, ActionDelegateReference<IInterfaceInteractResponse<NativeComposeViewDetail>> handler);

        /// <summary>
        /// 删除查询单个组合视图的某个 handler
        /// <param name="composeId">组合id</param>
        /// <param name="reqId">请求id</param>
        /// </summary>
        void RemoveQueryCurrentComposeViewHandler(long composeId, long reqId);

        /// <summary>
        /// 同步查询当前登录用户的现有组合视图
        /// </summary>
        /// <param name="composeId">组合id</param>
        /// <returns></returns>
        IInterfaceInteractResponse<NativeComposeViewDetail> QueryCurrentComposeView(long composeId);


        /// <summary>
        /// 异步查询当前登录用户的历史（包含当前已存在和过去逻辑删除的）组合视图
        /// </summary>
        /// <param name="composeId">组合id</param>
        /// <param name="handler">结果处理</param>
        /// <returns>请求 id</returns>
        long QueryHistoryComposeView(long composeId, ActionDelegateReference<IInterfaceInteractResponse<NativeComposeViewDetail>> handler);

        /// <summary>
        /// 删除查询单个历史（包含当前已存在和过去逻辑删除的）组合视图的某个 handler
        /// <param name="composeId">组合id</param>
        /// <param name="reqId">请求id</param>
        /// </summary>
        void RemoveQueryHistoryComposeViewHandler(long composeId, long reqId);

        /// <summary>
        /// 同步查询当前登录用户的某个组合视图历史（包含当前已存在和过去逻辑删除的）
        /// </summary>
        /// <param name="composeId">组合id</param>
        /// <returns></returns>
        IInterfaceInteractResponse<NativeComposeViewDetail> QueryHistoryComposeView(long composeId);
    }
}
