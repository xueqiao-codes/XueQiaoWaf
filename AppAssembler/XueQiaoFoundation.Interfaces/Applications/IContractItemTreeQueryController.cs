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
    public interface IContractItemTreeQueryController
    {
        /// <summary>
        /// 异步查询特定合约列表的树
        /// </summary>
        /// <param name="contractIds">要查询的合约id列表</param>
        /// <param name="queryParentCommodity">是否查询所属商品</param>
        /// <param name="queryParentExchange">是否查询所属交易所</param>
        /// <param name="ignoreCache">是否忽略缓存</param>
        /// <param name="handler">结果处理</param>
        /// <returns>请求 id</returns>
        long QueryTreeItems(IEnumerable<int> contractIds,
            bool queryParentCommodity,
            bool queryParentExchange,
            bool ignoreCache,
            ActionDelegateReference<Dictionary<int, ContractItemTree>> handler);

        /// <summary>
        /// 删除查询特定合约列表的树 handler
        /// </summary>
        /// <param name="reqId">请求id</param>
        void RemoveQueryTreeItemsHandler(long reqId);

        /// <summary>
        /// 同步查询特定合约列表的树
        /// </summary>
        /// <param name="contractIds">要查询的合约id列表</param>
        /// <param name="queryParentCommodity">是否查询所属商品</param>
        /// <param name="queryParentExchange">是否查询所属交易所</param>
        /// <param name="ignoreCache">是否忽略缓存</param>
        /// <param name="cancelToken">取消token</param>
        /// <returns>特定合约的合约树</returns>
        Dictionary<int, ContractItemTree> QueryTreeItems(IEnumerable<int> contractIds,
            bool queryParentCommodity,
            bool queryParentExchange,
            bool ignoreCache,
            CancellationToken cancelToken);

        /// <summary>
        /// 同步查询某个合约的数据树
        /// </summary>
        /// <param name="contractId">指定合约 id</param>
        /// <param name="queryParentCommodity">是否查询所属商品</param>
        /// <param name="queryParentExchange">是否查询所属交易所</param>
        /// <param name="ignoreCache">是否忽略缓存</param>
        /// <returns></returns>
        ContractItemTree QueryContractItemTree(int contractId,
            bool queryParentCommodity,
            bool queryParentExchange,
            bool ignoreCache);

        /// <summary>
        /// 同步查询某个商品的数据树
        /// </summary>
        /// <param name="commodityId">商品 id</param>
        /// <param name="queryParentExchange">是否查询所属交易所</param>
        /// <param name="ignoreCache">是否忽略缓存</param>
        /// <returns></returns>
        CommodityItemTree QueryCommodityItemTree(int commodityId,
            bool queryParentExchange,
            bool ignoreCache);
    }

    public class ContractItemTree
    {
        public ContractItemTree() { }

        public NativeExchange ParentExchange { get; set; }

        public NativeCommodity ParentCommodity { get; set; }

        public NativeContract Contract { get; set; }
    }

    public class CommodityItemTree
    {
        public CommodityItemTree() { }

        public NativeExchange ParentExchange { get; set; }

        public NativeCommodity Commodity { get; set; }
    }
}
