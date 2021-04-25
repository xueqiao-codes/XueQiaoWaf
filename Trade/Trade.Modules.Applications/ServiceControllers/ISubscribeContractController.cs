using NativeModel.Contract;
using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;

namespace XueQiaoWaf.Trade.Modules.Applications.ServiceControllers
{
    /// <summary>
    /// 合约订阅管理协议
    /// </summary>
    public interface ISubscribeContractController
    {
        /// <summary>
        /// 添加或修改某个订阅合约
        /// </summary>
        /// <param name="contractId">合约 id</param>
        /// <param name="subscribeGroupKey">订阅 group key</param>
        /// <param name="sameContractIdItemsUpdateAction">相同组合id的订阅合约项修改方法。arg1:是否存在该新加的订阅合约，arg2:返回与新加的id相同的订阅合约的修改模板</param>
        SubscribeContractDataModel AddOrUpdateSubscribeContract(int contractId, string subscribeGroupKey,
            Func<bool, SubscribeContractUpdateTemplate> sameContractIdItemsUpdateAction);

        /// <summary>
        /// 删除某个订阅合约
        /// </summary>
        /// <param name="contractId">合约 id</param>
        /// <param name="subscribeGroupKey">订阅 group key。当subscribeGroupKey为 null 时，删除该合约 id 的所有订阅</param>
        void RemoveSubscribeConract(int contractId, string subscribeGroupKey = null);

        /// <summary>
        /// 修改相同 id 的订阅合约
        /// </summary>
        /// <param name="contractId">合约 id</param>
        /// <param name="orSymbol">或者合约symbol</param>
        /// <param name="updateAction">修改方法。返回修改模板</param>
        void UpdateSubscribeContractsWithSameId(int? contractId,
            ContractSymbol orSymbol,
            Func<SubscribeContractUpdateTemplate> updateAction);

        /// <summary>
        /// 获取当前用户订阅的所有key是shared的合约
        /// </summary>
        /// <returns></returns>
        IEnumerable<SubscribeContractDataModel> GetSharedGroupKeySubscribeContracts();

        /// <summary>
        /// 订阅指定合约行情。
        /// </summary>
        /// <param name="contractId">指定合约id</param>
        /// <param name="orSymbol">或指定合约的 symbol</param>
        void SubscribeContractQuotationIfNeed(int? contractId, ContractSymbol orSymbol);

        /// <summary>
        /// 退订指定合约行情。
        /// </summary>
        /// <param name="contractId">指定合约id</param>
        /// <param name="orSymbol">或指定合约的 symbol</param>
        void UnsubscribeContractQuotationIfNeed(int? contractId, ContractSymbol orSymbol);
    }

    /// <summary>
    /// 订阅的合约修改模板
    /// </summary>
    public class SubscribeContractUpdateTemplate
    {
        /// <summary>
        /// 要修改的行情
        /// </summary>
        public Tuple<NativeQuotationItem> Quotation;

        /// <summary>
        /// 要修改的订阅状态
        /// </summary>
        public Tuple<MarketSubscribeState> SubscribeState;

        /// <summary>
        /// 要修改的订阅信息
        /// </summary>
        public Tuple<string> SubscribeStateMsg;

        /// <summary>
        /// 添加时间
        /// </summary>
        public Tuple<long> AddTimestamp;

        /// <summary>
        /// 自定义分组列表
        /// </summary>
        public Tuple<IEnumerable<string>> CustomGroupKeys;

        /// <summary>
        /// 该合约正进行交易中的子账户id列表
        /// </summary>
        public Tuple<IEnumerable<long>> OnTradingSubAccountIds;

        /// <summary>
        /// 该合约存在持仓的子账户id列表
        /// </summary>
        public Tuple<IEnumerable<long>> ExistPositionSubAccountIds;

        /// <summary>
        /// 是否组合相关
        /// </summary>
        public Tuple<bool> IsComposeRelated;

    }
}
