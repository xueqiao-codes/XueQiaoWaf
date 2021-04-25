using lib.xqclient_base.thriftapi_mediation.Interface;
using NativeModel.Contract;
using System;
using System.Collections.Generic;
using System.Threading;
using XueQiaoFoundation.Shared.Helper.WeakDelegate;

namespace XueQiaoFoundation.Interfaces.Applications
{
    /// <summary>
    /// 商品查询管理
    /// </summary>
    public interface ICommodityQueryController
    {   
        /// <summary>
        /// 异步查询所有商品数据
        /// </summary>
        /// <param name="handler">结果处理</param>
        /// <returns>请求 id</returns>
        long QueryAllCommodities(ActionDelegateReference<IInterfaceInteractResponse<IEnumerable<NativeCommodity>>> handler);

        /// <summary>
        /// 删除查询所有商品数据的某个请求
        /// </summary>
        /// <param name="reqId">请求的 id</param>
        void RemoveQueryAllCommoditiesHandler(long reqId);

        /// <summary>
        /// 同步查询所有商品
        /// </summary>
        /// <param name="ct">取消token</param>
        /// <returns></returns>
        IInterfaceInteractResponse<IEnumerable<NativeCommodity>> QueryAllCommodities(CancellationToken ct);

        /// <summary>
        /// 使用 commodity id 同步查询某个商品
        /// </summary>
        /// <param name="commodityId">商品 id</param>
        /// <returns></returns>
        IInterfaceInteractResponse<NativeCommodity> QueryCommodity(int commodityId);

        /// <summary>
        /// 使用 commoditySymbol 同步查询某个商品
        /// </summary>
        /// <param name="commoditySymbol">商品标识，包含三部分:交易所代码、商品类型、商品代码</param>
        /// <returns></returns>
        IInterfaceInteractResponse<NativeCommodity> QueryCommodity(Tuple<string/*exchangeMic*/,int/*commodityType*/,string/*commodityCode*/> commoditySymbol);
    }
}
