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
    public class TargetPositionDataModel : Model, IXqTargetDM
    {
        public TargetPositionDataModel(ClientXQOrderTargetType targetType,
            SubAccountFieldsForTradeData subAccountFields,
            string targetKey)
        {
            this.TargetType = targetType;
            this.SubAccountFields = subAccountFields;
            this.TargetKey = targetKey;
        }

        /// <summary>
        /// 标的类型
        /// </summary>
        public ClientXQOrderTargetType TargetType { get; private set; }
        
        /// <summary>
        /// 子账户相关数据
        /// </summary>
        public SubAccountFieldsForTradeData SubAccountFields { get; private set; }
        
        /// <summary>
        /// 标的key。合约类型标的为合约 id，组合类型标的为组合 id
        /// </summary>
        public string TargetKey { get; private set; }


        private TargetContract_TargetContractDetail targetContractDetailContainer;
        /// <summary>
        /// 合约标的的容器
        /// </summary>
        public TargetContract_TargetContractDetail TargetContractDetailContainer
        {
            get { return targetContractDetailContainer; }
            set { SetProperty(ref targetContractDetailContainer, value); }
        }

        private TargetCompose_ComposeDetail targetComposeDetailContainer;
        /// <summary>
        /// 组合标的的组合详情
        /// </summary>
        public TargetCompose_ComposeDetail TargetComposeDetailContainer
        {
            get { return targetComposeDetailContainer; }
            set { SetProperty(ref targetComposeDetailContainer, value); }
        }

        private UserComposeViewContainer targetComposeUserComposeViewContainer;
        /// <summary>
        /// 组合标的的用户组合视图容器
        /// </summary>
        public UserComposeViewContainer TargetComposeUserComposeViewContainer
        {
            get { return targetComposeUserComposeViewContainer; }
            set { SetProperty(ref targetComposeUserComposeViewContainer, value); }
        }
        
        private string targetName;
        /// <summary>
        /// 雪橇标的名称。根据其他数据设置而来的，避免在界面中拼接影响性能
        /// </summary>
        public string TargetName
        {
            get { return targetName; }
            set { SetProperty(ref targetName, value); }
        }

        private long? longPosition;
        // 今日长持
        public long? LongPosition
        {
            get { return longPosition; }
            set { SetProperty(ref longPosition, value); }
        }

        private long? shortPosition;
        // 今日短持
        public long? ShortPosition
        {
            get { return shortPosition; }
            set { SetProperty(ref shortPosition, value); }
        }

        private long? netPosition;
        public long? NetPosition
        {
            get { return netPosition; }
            set { SetProperty(ref netPosition, value); }
        }

        private double? positionAvgPrice;
        public double? PositionAvgPrice
        {
            get { return positionAvgPrice; }
            set { SetProperty(ref positionAvgPrice, value); }
        }
        
        private long lastModifyTimestampMs;
        /// <summary>
        /// 最后修改时间戳(毫秒)
        /// </summary>
        public long LastModifyTimestampMs
        {
            get { return lastModifyTimestampMs; }
            set { SetProperty(ref lastModifyTimestampMs, value); }
        }

        private TargetPositionDynamicInfo dynamicInfo;
        public TargetPositionDynamicInfo DynamicInfo
        {
            get { return dynamicInfo; }
            set { SetProperty(ref dynamicInfo, value); }
        }

        private long dynamicInfoModifyTimestampMs;
        public long DynamicInfoModifyTimestampMs
        {
            get { return dynamicInfoModifyTimestampMs; }
            set { SetProperty(ref dynamicInfoModifyTimestampMs, value); }
        }
        
        private bool? isXqTargetExpired;
        /// <summary>
        /// 标的是否已过期
        /// </summary>
        public bool? IsXqTargetExpired
        {
            get { return isXqTargetExpired; }
            set { SetProperty(ref isXqTargetExpired, value); }
        }
    }

    public class TargetPositionDynamicInfo : Model
    {
        private double? lastPrice;
        /// <summary>
        /// 最新成交价
        /// </summary>
        public double? LastPrice
        {
            get { return lastPrice; }
            set { SetProperty(ref lastPrice, value); }
        }

        private double? positionProfit;
        /// <summary>
        /// 持仓盈亏
        /// </summary>
        public double? PositionProfit
        {
            get { return positionProfit; }
            set { SetProperty(ref positionProfit, value); }
        }

        private double? closedProfit;
        /// <summary>
        /// 平仓盈亏
        /// </summary>
        public double? ClosedProfit
        {
            get { return closedProfit; }
            set { SetProperty(ref closedProfit, value); }
        }

        private double? totalProfit;
        /// <summary>
        /// 总盈亏
        /// </summary>
        public double? TotalProfit
        {
            get { return totalProfit; }
            set { SetProperty(ref totalProfit, value); }
        }

        private double? positionValue;
        /// <summary>
        /// 持仓市值
        /// </summary>
        public double? PositionValue
        {
            get { return positionValue; }
            set { SetProperty(ref positionValue, value); }
        }

        private double? leverage;
        /// <summary>
        /// 杠杆
        /// </summary>
        public double? Leverage
        {
            get { return leverage; }
            set { SetProperty(ref leverage, value); }
        }

        private string currency;
        /// <summary>
        /// 币种
        /// </summary>
        public string Currency
        {
            get { return currency; }
            set { SetProperty(ref currency, value); }
        }
    }

    public class TargetPositionKey : Tuple<ClientXQOrderTargetType, long, string>
    {
        public TargetPositionKey(ClientXQOrderTargetType targetType,
            long subAccountId,
            string targetKey) : base(targetType, subAccountId, targetKey)
        {
            this.TargetType = targetType;
            this.SubAccountId = subAccountId;
            this.TargetKey = targetKey;
        }

        public ClientXQOrderTargetType TargetType { get; private set; }
        public long SubAccountId { get; private set; }
        public string TargetKey { get; private set; }
    }
}
