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
    /// 未分配成交以合约分组的概况
    /// </summary>
    public class UATContractGroupedSummary : Model
    {
        public UATContractGroupedSummary(long fundAccountId, int contractId)
        {
            this.FundAccountId = fundAccountId;
            this.ContractId = contractId;
            this.ContractDetailContainer = new TargetContract_TargetContractDetail(contractId);

            this.BuyVolumeSummary = new UATVolumeSummaryDM();
            this.SellVolumeSummary = new UATVolumeSummaryDM();
        }

        public long FundAccountId { get; private set; }

        public int ContractId { get; private set; }

        public TargetContract_TargetContractDetail ContractDetailContainer { get; private set; }

        /// <summary>
        /// 买方向的数量概要
        /// </summary>
        public UATVolumeSummaryDM BuyVolumeSummary { get; private set; }

        /// <summary>
        /// 卖方向的数量概要
        /// </summary>
        public UATVolumeSummaryDM SellVolumeSummary { get; private set; }
    }

    public class UATContractGroupedSummaryKey : Tuple<long, int>
    {
        public UATContractGroupedSummaryKey(long fundAccountId, int contractId) : base(fundAccountId, contractId)
        {
            this.FundAccountId = fundAccountId;
            this.ContractId = contractId;
        }

        public long FundAccountId { get; private set; }

        public int ContractId { get; private set; }
    }
}
