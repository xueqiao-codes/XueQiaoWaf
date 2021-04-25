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
    /// 雪橇成交详情item data model
    /// </summary>
    public class XqTradeDetailDM : Model
    {
        public XqTradeDetailDM(XqTradeDetailDMType itemType)
        {
            this.ItemType = itemType;
        }

        public XqTradeDetailDMType ItemType { get; private set; }

        private long? tradeId;
        public long? TradeId
        {
            get { return tradeId; }
            set { SetProperty(ref tradeId, value); }
        }

        private ClientTradeDirection? direction;
        public ClientTradeDirection? Direction
        {
            get { return direction; }
            set { SetProperty(ref direction, value); }
        }

        private int? tradeVolume;
        public int? TradeVolume
        {
            get { return tradeVolume; }
            set { SetProperty(ref tradeVolume, value); }
        }

        private double? tradePrice;
        public double? TradePrice
        {
            get { return tradePrice; }
            set { SetProperty(ref tradePrice, value); }
        }

        private string orderId;
        public string OrderId
        {
            get { return orderId; }
            set { SetProperty(ref orderId, value); }
        }

        private long? createTimestampMs;
        public long? CreateTimestampMs
        {
            get { return createTimestampMs; }
            set { SetProperty(ref createTimestampMs, value); }
        }

        private TargetComposeLegTradeSummarysContainer composeLegTradeSummarysContainer;
        /// <summary>
        /// 标的为组合的组合腿成交概要容器
        /// </summary>
        public TargetComposeLegTradeSummarysContainer TargetComposeLegTradeSummarysContainer
        {
            get { return composeLegTradeSummarysContainer; }
            set { SetProperty(ref composeLegTradeSummarysContainer, value); }
        }

        private TargetContract_TargetContractDetail targetContractDetailContainer;
        /// <summary>
        /// 标的为合约的合约详情容器
        /// </summary>
        public TargetContract_TargetContractDetail TargetContractDetailContainer
        {
            get { return targetContractDetailContainer; }
            set { SetProperty(ref targetContractDetailContainer, value); }
        }
    }

    /// <summary>
    /// 雪橇成交详情 item 类型
    /// </summary>
    public enum XqTradeDetailDMType
    {
        NormalTraded = 1,               // 已成交
        ComposeLameTraded = 2,          // 已成交（组合瘸腿）
        WaitingTrade = 3,               // 待成交
    }
}
