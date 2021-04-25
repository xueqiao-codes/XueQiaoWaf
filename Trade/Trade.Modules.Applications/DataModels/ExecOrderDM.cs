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
    /// 执行订单 data model
    /// </summary>
    public class ExecOrderDM : Model
    {
        public ExecOrderDM(long execOrderId, int contractId, long subAccountId, HostingXQTargetType xqTargetType, long? xqComposeId)
        {
            this.ExecOrderId = execOrderId;
            this.ContractId = contractId;
            this.SubAccountId = subAccountId;
            this.XqTargetType = XqTargetType;
            this.XqComposeId = xqComposeId;
            this.ContractDetailContainer = new TargetContract_TargetContractDetail(contractId);
        }

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
        
        private ClientXQOrderState? mappedOrderState;
        public ClientXQOrderState? MappedOrderState
        {
            get { return mappedOrderState; }
            set { SetProperty(ref mappedOrderState, value); }
        }

        private string execStateMsg;
        public string ExecStateMsg
        {
            get { return execStateMsg; }
            set { SetProperty(ref execStateMsg, value); }
        }

        private int direction;
        public int Direction
        {
            get { return direction; }
            set { SetProperty(ref direction, value); }
        }

        private double price;
        public double Price
        {
            get { return price; }
            set { SetProperty(ref price, value); }
        }

        private int quantity;
        public int Quantity
        {
            get { return quantity; }
            set { SetProperty(ref quantity, value); }
        }

        private int? tradeVolume;
        public int? TradeVolume
        {
            get { return tradeVolume; }
            set { SetProperty(ref tradeVolume, value); }
        }

        private double? tradeAvgPrice;
        public double? TradeAvgPrice
        {
            get { return tradeAvgPrice; }
            set { SetProperty(ref tradeAvgPrice, value); }
        }

        private long createTimestampMs;
        public long CreateTimestampMs
        {
            get { return createTimestampMs; }
            set { SetProperty(ref createTimestampMs, value); }
        }
    }
}
