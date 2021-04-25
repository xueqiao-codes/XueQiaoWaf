using lib.xqclient_base.thriftapi_mediation.Interface;
using NativeModel.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.contract.standard;
using XueQiaoFoundation.Shared.Helper.WeakDelegate;

namespace XueQiaoFoundation.Interfaces.Applications
{
    /// <summary>
    /// 合约查询管理
    /// </summary>
    public interface IContractQueryController
    {   
        /// <summary>
        /// 异步查询某个合约
        /// </summary>
        /// <param name="contractId">合约id</param>
        /// <param name="handler">结果处理</param>
        /// <returns>请求 id</returns>
        long QueryContract(int contractId, ActionDelegateReference<IInterfaceInteractResponse<NativeContract>> handler);
        
        /// <summary>
        /// 删除查询单个合约的某个 handler
        /// </summary>
        /// <param name="contractId">合约id</param>
        /// <param name="reqId">请求 id</param>
        void RemoveQueryContractHandler(int contractId, long reqId);

        /// <summary>
        /// 同步查询某个合约
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        IInterfaceInteractResponse<NativeContract> QueryContract(int contractId);

        /// <summary>
        /// 查询某个商品下的合约
        /// </summary>
        /// <param name="queryReqKey">查询请求信息</param>
        /// <param name="handler">结果处理</param>
        /// <returns>请求 id</returns>
        long QueryContracts(QueryContractsByCommodityReqKey queryReqKey,
            ActionDelegateReference<IInterfaceInteractResponse<IEnumerable<NativeContract>>> handler);

        /// <summary>
        /// 删除查询某个商品下的合约的某个 handler
        /// </summary>
        /// <param name="queryReqKey">查询请求信息</param>
        /// <param name="reqId">请求 id</param>
        void RemoveQueryContractsHandler(QueryContractsByCommodityReqKey queryReqKey,
            long reqId);

        /// <summary>
        /// 同步查询某个商品下的合约
        /// </summary>
        /// <param name="queryReqKey">查询请求信息</param>
        /// <returns></returns>
        IInterfaceInteractResponse<IEnumerable<NativeContract>> QueryContracts(QueryContractsByCommodityReqKey queryReqKey);
    }

    /// <summary>
    /// 查询某商品的合约请求 Key
    /// </summary>
    public class QueryContractsByCommodityReqKey : Tuple<int, ContractStatus, int?/*, IEnumerable<string>*/>
    {
        public QueryContractsByCommodityReqKey(int commodityId, ContractStatus status, int? limitSize/*, string[] contractCodes*/)
            : base(commodityId, status, limitSize/*, contractCodes */)
        {
            this.CommodityId = commodityId;
            this.Status = status;
            this.LimitSize = limitSize;
            //this.ContractCodes = contractCodes.ToArray();
        }

        // 合约 id
        public readonly int CommodityId;

        // 合约状态
        public readonly ContractStatus Status;

        // 限定数量
        public readonly int? LimitSize;

        // 特定合约的 code 
        //public readonly string[] ContractCodes;

        //public override bool Equals(object obj)
        //{
        //    var result = base.Equals(obj);
        //    if (result == false)
        //    {
        //        var thisContractCodes = this.ContractCodes;
        //        if (obj is QueryContractsByCommodityReqKey compareReq && thisContractCodes != null)
        //        {
        //            result = thisContractCodes.SequenceEqual(compareReq.ContractCodes);
        //        }
        //    }
        //    return result;
        //}
    }
}
