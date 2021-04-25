using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using XueQiaoFoundation.BusinessResources.DataModels;

namespace XueQiaoWaf.Trade.Modules.Applications.DataModels
{
    /// <summary>
    /// 订阅的合约 data model
    /// </summary>
    public class SubscribeContractDataModel : Model, IMarketSubscribeData
    {
        /// <summary>
        /// 共享的合约列表的合约 Group key
        /// </summary>
        public const string SharedListContractGroupKey = "9f4b628a-32cb-415b-bf80-43c3b490af0c";

        /// <summary>
        /// 获取唯一的订阅合约 Group key
        /// </summary>
        /// <returns></returns>
        public static String UniqueSubscribeContractGroupKey()
        {
            return Guid.NewGuid().ToString();
        }

        public SubscribeContractDataModel(int contractId, string subscribeGroupKey)
        {
            if (subscribeGroupKey == null) throw new ArgumentNullException("subscribeGroupKey can't be null.");
            this.ContractId = contractId;
            this.SubscribeGroupKey = subscribeGroupKey;
            this.ContractDetailContainer = new TargetContract_TargetContractDetail(contractId);
        }

        /// <summary>
        /// 合约 id
        /// </summary>
        public int ContractId { get; private set; }

        /// <summary>
        /// 订阅组 key
        /// </summary>
        public string SubscribeGroupKey { get; private set; }

        /// <summary>
        /// 合约详情容器
        /// </summary>
        public TargetContract_TargetContractDetail ContractDetailContainer { get; private set; }
        
        private NativeQuotationItem quotation;
        /// <summary>
        /// 行情信息
        /// </summary>
        public NativeQuotationItem Quotation
        {
            get { return quotation; }
            set { SetProperty(ref quotation, value); }
        }

        private MarketSubscribeState subscribeState;
        public MarketSubscribeState SubscribeState
        {
            get { return subscribeState; }
            set { SetProperty(ref subscribeState, value); }
        }

        private string subscribeStateMsg;
        public string SubscribeStateMsg
        {
            get { return subscribeStateMsg; }
            set { SetProperty(ref subscribeStateMsg, value); }
        }

        private long addTimestamp;
        public long AddTimestamp
        {
            get { return addTimestamp; }
            set { SetProperty(ref addTimestamp, value); }
        }

        private IEnumerable<string> customGroupKeys;
        // 自定义分组列表
        public IEnumerable<string> CustomGroupKeys
        {
            get { return customGroupKeys; }
            set { SetProperty(ref customGroupKeys, value); }
        }
        
        private IEnumerable<long> onTradingSubAccountIds;
        /// <summary>
        /// 该合约正进行交易中的子账户id列表
        /// </summary>
        public IEnumerable<long> OnTradingSubAccountIds
        {
            get { return onTradingSubAccountIds; }
            set { SetProperty(ref onTradingSubAccountIds, value); }
        }

        private IEnumerable<long> existPositionSubAccountIds;
        /// <summary>
        /// 该合约存在持仓的子账户id列表
        /// </summary>
        public IEnumerable<long> ExistPositionSubAccountIds
        {
            get { return existPositionSubAccountIds; }
            set { SetProperty(ref existPositionSubAccountIds, value); }
        }

        private bool isComposeRelated;
        // 是否组合相关
        public bool IsComposeRelated
        {
            get { return isComposeRelated; }
            set { SetProperty(ref isComposeRelated, value); }
        }
        
        private bool isXqTargetExpired;
        /// <summary>
        /// 标的是否已过期
        /// </summary>
        public bool IsXqTargetExpired
        {
            get { return isXqTargetExpired; }
            set { SetProperty(ref isXqTargetExpired, value); }
        }

        private string xqTargetName;
        /// <summary>
        /// 标的名称（第一层级，方便用于列表排序）
        /// </summary>
        public string XqTargetName
        {
            get { return xqTargetName; }
            set { SetProperty(ref xqTargetName, value); }
        }

    }
}
