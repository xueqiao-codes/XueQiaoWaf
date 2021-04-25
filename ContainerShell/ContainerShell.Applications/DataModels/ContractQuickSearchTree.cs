using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.Shared.Model;

namespace ContainerShell.Applications.DataModels
{
    public class ContractQuickSearchTree
    {
        public ContractQuickSearchTree()
        {
            Commodities = new ObservableCollection<QuickSearchCommodityNode>();
        }

        public ObservableCollection<QuickSearchCommodityNode> Commodities { get; private set; }
    }

    public class QuickSearchCommodityNode : NodeWithChildren<CommodityDetailContainer, QuickSearchContractNode>
    {
    }

    public class QuickSearchContractNode : Model
    {
        public QuickSearchContractNode(int contractId)
        {
            this.ContractDetailContainer = new TargetContract_TargetContractDetail(contractId);
        }

        public TargetContract_TargetContractDetail ContractDetailContainer { get; private set; }
    }
}
