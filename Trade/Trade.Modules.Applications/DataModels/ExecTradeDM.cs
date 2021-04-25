using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using xueqiao.trade.hosting.arbitrage.thriftapi;
using XueQiaoFoundation.BusinessResources.DataModels;

namespace XueQiaoWaf.Trade.Modules.Applications.DataModels
{
    /// <summary>
    /// 执行成交 data model
    /// </summary>
    public class ExecTradeDM : Model
    {
        public ExecTradeDM(long execTradeId, long execOrderId, int contractId, long subAccountId, HostingXQTargetType xqTargetType, long? xqComposeId)
        {
            this.ExecTradeId = execTradeId;
            this.ExecOrderId = execOrderId;
            this.ContractId = contractId;
            this.SubAccountId = subAccountId;
            this.XqTargetType = XqTargetType;
            this.XqComposeId = xqComposeId;
            this.ContractDetailContainer = new TargetContract_TargetContractDetail(contractId);
        }

        public long ExecTradeId { get; private set; }

        public long ExecOrderId { get; private set; }

        public int ContractId { get; private set; }

        public long SubAccountId { get; private set; }

        public HostingXQTargetType XqTargetType { get; private set; }

        public long? XqComposeId { get; private set; }

        /// <summary>
        /// 合约详情容器
        /// </summary>
        public TargetContract_TargetContractDetail ContractDetailContainer { get; private set; }

        private NativeComposeLeg xqComposeLegMeta;
        /// <summary>
        /// 雪橇组合腿的简明信息
        /// </summary>
        public NativeComposeLeg XqComposeLegMeta
        {
            get { return xqComposeLegMeta; }
            set { SetProperty(ref xqComposeLegMeta, value); }
        }

        private int direction;
        public int Direction
        {
            get { return direction; }
            set { SetProperty(ref direction, value); }
        }

        private int tradeVolume;
        public int TradeVolume
        {
            get { return tradeVolume; }
            set { SetProperty(ref tradeVolume, value); }
        }

        private double tradePrice;
        public double TradePrice
        {
            get { return tradePrice; }
            set { SetProperty(ref tradePrice, value); }
        }

        private long createTimestampMs;
        public long CreateTimestampMs
        {
            get { return createTimestampMs; }
            set { SetProperty(ref createTimestampMs, value); }
        }
    }
}
