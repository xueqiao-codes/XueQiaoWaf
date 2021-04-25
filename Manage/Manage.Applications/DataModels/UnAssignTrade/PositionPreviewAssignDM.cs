using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using XueQiaoFoundation.BusinessResources.DataModels;

namespace Manage.Applications.DataModels
{
    /// <summary>
    /// 持仓预分配 data model
    /// </summary>
    public class PositionPreviewAssignDM : Model
    {
        public PositionPreviewAssignDM(long subAccountId, 
            string UATItemKey, long fundAccountId, int contractId)
        {
            this.SubAccountId = subAccountId;
            this.UATItemKey = UATItemKey;
            this.FundAccountId = fundAccountId;
            this.ContractId = contractId;
            this.ContractDetailContainer = new TargetContract_TargetContractDetail(contractId);
            this.PAAccountInfoContainer = new PAItemAccountInfoContainer(fundAccountId, subAccountId);
        }

        /// <summary>
        /// 分配的操作账户 id
        /// </summary>
        public long SubAccountId { get; private set; }

        // 未分配成交项的 key
        public string UATItemKey { get; private set; }

        /// <summary>
        /// 所属资账户 id
        /// </summary>
        public long FundAccountId { get; private set; }

        /// <summary>
        /// 所属合约 id
        /// </summary>
        public int ContractId { get; private set; }

        private ClientTradeDirection _UATItemDirection;
        public ClientTradeDirection UATItemDirection
        {
            get { return _UATItemDirection; }
            set { SetProperty(ref _UATItemDirection, value); }
        }

        private double _UATItemPrice;
        public double UATItemPrice
        {
            get { return _UATItemPrice; }
            set { SetProperty(ref _UATItemPrice, value); }
        }

        private long _UATItemTradeTimestampMs;
        public long UATItemTradeTimestampMs
        {
            get { return _UATItemTradeTimestampMs; }
            set { SetProperty(ref _UATItemTradeTimestampMs, value); }
        }

        /// <summary>
        /// 合约信息容器
        /// </summary>
        public TargetContract_TargetContractDetail ContractDetailContainer { get; private set; }

        /// <summary>
        /// 预分配账户信息容器 
        /// </summary>
        public PAItemAccountInfoContainer PAAccountInfoContainer { get; private set; }
        
        private int volume;
        // 分配数量
        public int Volume
        {
            get { return volume; }
            set { SetProperty(ref volume, value); }
        }
    }

    public class PositionPreviewAssignItemKey : Tuple<long, string, long, int>
    {
        public PositionPreviewAssignItemKey(long subAccountId, string UATItemKey, long fundAccountId, int contractId) 
            : base(subAccountId, UATItemKey, fundAccountId, contractId)
        {
            this.SubAccountId = subAccountId;
            this.UATItemKey = UATItemKey;
            this.FundAccountId = fundAccountId;
            this.ContractId = contractId;
        }

        /// <summary>
        /// 分配的操作账户 id
        /// </summary>
        public long SubAccountId { get; private set; }

        /// <summary>
        /// 所属资账户 id
        /// </summary>
        public long FundAccountId { get; private set; }

        // 未分配成交项的 key
        public string UATItemKey { get; private set; }

        /// <summary>
        /// 所属合约 id
        /// </summary>
        public int ContractId { get; private set; }
        
    }
}
