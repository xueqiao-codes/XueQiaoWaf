using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using xueqiao.trade.hosting.arbitrage.thriftapi;

namespace XueQiaoFoundation.BusinessResources.DataModels
{
    /// <summary>
    /// 成交 data model 基类
    /// </summary>
    public class TradeItemDataModel : Model, IXqTargetDM
    {
        public TradeItemDataModel(ClientXQOrderTargetType targetType, string targetKey, SubAccountFieldsForTradeData subAccountFields, long tradeId, string orderId)
        {
            this.TargetType = targetType;
            this.TargetKey = targetKey;
            this.SubAccountFields = subAccountFields;
            this.TradeId = tradeId;
            this.OrderId = orderId;
            InvalidateSourceType();
        }

        /// <summary>
        /// 标的类型
        /// </summary>
        public ClientXQOrderTargetType TargetType { get; private set; }

        /// <summary>
        /// 标的key。合约类型标的为合约 id，组合类型标的为组合 id
        /// </summary>
        public string TargetKey { get; private set; }

        /// <summary>
        /// 子账户相关数据
        /// </summary>
        public SubAccountFieldsForTradeData SubAccountFields { get; private set; }
        
        /// <summary>
        /// 成交 id
        /// </summary>
        public long TradeId { get; private set; }

        /// <summary>
        /// 订单 id
        /// </summary>
        public string OrderId { get; private set; }


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

        private ClientTradeDirection direction;
        /// <summary>
        /// 方向
        /// </summary>
        public ClientTradeDirection Direction
        {
            get { return direction; }
            set { SetProperty(ref direction, value); }
        }
        
        private double tradePrice;
        /// <summary>
        /// 成交价格
        /// </summary>
        public double TradePrice
        {
            get { return tradePrice; }
            set { SetProperty(ref tradePrice, value); }
        }
        
        private int tradeVolume;
        /// <summary>
        /// 成交数量
        /// </summary>
        public int TradeVolume
        {
            get { return tradeVolume; }
            set { SetProperty(ref tradeVolume, value); }
        }

        private HostingXQTarget sourceOrderTarget;
        /// <summary>
        /// 源订单标的
        /// </summary>
        public HostingXQTarget SourceOrderTarget
        {
            get { return sourceOrderTarget; }
            set
            {
                SetProperty(ref sourceOrderTarget, value);
                InvalidateSourceType();
            }
        }

        private ClientTradeItemSourceType sourceType;
        /// <summary>
        /// 来源类型
        /// </summary>
        public ClientTradeItemSourceType SourceType
        {
            get { return sourceType; }
            private set { SetProperty(ref sourceType, value); }
        }

        private void InvalidateSourceType()
        {
            var sourceTarget = this.SourceOrderTarget;
            if (sourceTarget == null)
            {
                this.SourceType = ClientTradeItemSourceType.Unkown;
            }
            else
            {
                if (sourceTarget.TargetType == HostingXQTargetType.CONTRACT_TARGET)
                    this.SourceType = ClientTradeItemSourceType.ContractTarget;
                else if (sourceTarget.TargetType == HostingXQTargetType.COMPOSE_TARGET)
                {
                    if (this.TargetType == sourceTarget.TargetType.ToClientXQOrderTargetType())
                        this.SourceType = ClientTradeItemSourceType.ComposeTarget;
                    else
                        this.SourceType = ClientTradeItemSourceType.ComposeTargetLame;
                }
                else
                    this.SourceType = ClientTradeItemSourceType.Unkown;
            }
        }

        private long createTimestampMs;
        /// <summary>
        /// 创建时间
        /// </summary>
        public long CreateTimestampMs
        {
            get { return createTimestampMs; }
            set { SetProperty(ref createTimestampMs, value); }
        }
    }
}
