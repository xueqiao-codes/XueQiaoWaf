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
    /// 预分配持仓预览项以合约分组的概况 data model
    /// </summary>
    public class PAItemPreviewItemsContractSummary : Model
    {
        public PAItemPreviewItemsContractSummary(int contractId)
        {
            this.ContractId = contractId;
        }

        public int ContractId { get; private set; }

        // 合约信息容器
        public TargetContract_TargetContractDetail ContractDetailContainer { get; set; }

        // 买量
        public int BuyTotalVolume { get; set; }

        // 卖量
        public int SellTotalVolume { get; set; }

        // 净量
        public int NetTotalVolume
        {
            get { return BuyTotalVolume - SellTotalVolume; }
        }
    }
}
