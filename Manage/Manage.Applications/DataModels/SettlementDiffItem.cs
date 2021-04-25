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
    /// 核对差异项 datamodel
    /// </summary>
    public class SettlementDiffItem : Model
    {
        public SettlementDiffItem(long verifyId, long tradeAccountId, int contractId)
        {
            this.VerifyId = verifyId;
            this.TradeAccountId = tradeAccountId;
            this.ContractId = contractId;
            this.ContractDetailContainer = new TargetContract_TargetContractDetail(contractId);
            InvalidateNetPositionDiff();
        }
        
        public long VerifyId { get; private set; }

        public long TradeAccountId { get; private set; }

        public int ContractId { get; private set; }

        public TargetContract_TargetContractDetail ContractDetailContainer { get; private set; }

        private int sledNetPosition;
        /// <summary>
        /// 雪橇净仓
        /// </summary>
        public int SledNetPosition
        {
            get { return sledNetPosition; }
            set
            {
                if (SetProperty(ref sledNetPosition, value))
                {
                    InvalidateNetPositionDiff();
                }
            }
        }

        private int upsideNetPosition;
        /// <summary>
        /// 上手净仓
        /// </summary>
        public int UpsideNetPosition
        {
            get { return upsideNetPosition; }
            set
            {
                if (SetProperty(ref upsideNetPosition, value))
                {
                    InvalidateNetPositionDiff();
                }
            }
        }
        
        private int netPositionDiff;
        /// <summary>
        /// 雪橇和上手的持仓差异
        /// </summary>
        public int NetPositionDiff
        {
            get { return netPositionDiff; }
            private set { SetProperty(ref netPositionDiff, value); }
        }

        private void InvalidateNetPositionDiff()
        {
            this.NetPositionDiff = this.SledNetPosition - this.UpsideNetPosition;
        }
    }
}
