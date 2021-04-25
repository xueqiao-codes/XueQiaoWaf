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
    /// 持仓的成交详情项 data model
    /// </summary>
    public class PositionTradeDetailItemDM : Model
    {
        public PositionTradeDetailItemDM(int contractId, ClientXQOrderTargetType parentTradeTargetType, string parentTradeTargetKey)
        {
            this.ContractId = contractId;
            this.ParentTradeTargetType = parentTradeTargetType;
            this.ParentTradeTargetKey = parentTradeTargetKey;
            this.ContractDetailContainer = new TargetContract_TargetContractDetail(contractId);
        }

        public int ContractId { get; private set; }

        public ClientXQOrderTargetType ParentTradeTargetType { get; private set; }

        public string ParentTradeTargetKey { get; private set; }

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

        private ClientTradeDirection direction;
        public ClientTradeDirection Direction
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

        private long dataTimestampMs;
        public long DataTimestampMs
        {
            get { return dataTimestampMs; }
            set { SetProperty(ref dataTimestampMs, value); }
        }
    }
}
