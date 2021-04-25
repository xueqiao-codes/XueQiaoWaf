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
    /// 订单执行项 data model
    /// </summary>
    public class ExecOrderItemDataModel : Model
    {
        public ExecOrderItemDataModel(long execOrderId, int contractId, long subAccountId)
        {
            this.ExecOrderId = execOrderId;
            this.ContractId = contractId;
            this.SubAccountId = subAccountId;
        }

        public long ExecOrderId { get; private set; }

        public int ContractId { get; private set; }

        public long SubAccountId { get; private set; }

        private TargetContract_TargetContractDetail contractDetail;
        /// <summary>
        /// 执行订单对应合约的详情
        /// </summary>
        public TargetContract_TargetContractDetail ContractDetail
        {
            get { return contractDetail; }
            set { SetProperty(ref contractDetail, value); }
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

        private int tradeVolume;
        public int TradeVolume
        {
            get { return tradeVolume; }
            set { SetProperty(ref tradeVolume, value); }
        }

        private double tradeAvgPrice;
        public double TradeAvgPrice
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
