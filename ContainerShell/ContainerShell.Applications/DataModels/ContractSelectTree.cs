using NativeModel.Contract;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Model;

namespace ContainerShell.Applications.DataModels
{
    public class ContractSelectTree
    {
        public ContractSelectTree()
        {
            Exchanges = new ObservableCollection<ExchangeSelectNode>();
        }

        public ObservableCollection<ExchangeSelectNode> Exchanges { get; private set; }

        /// <summary>
        /// 更新 Tree 数据
        /// </summary>
        /// <param name="exchanges"></param>
        /// <param name="commodityListGetter"></param>
        public void UpdateTreeData(IEnumerable<NativeExchange> exchanges, 
            Func<NativeExchange, IEnumerable<NativeCommodity>> commodityListGetter)
        {
            Debug.Assert(commodityListGetter != null);
            if (exchanges == null) exchanges = new NativeExchange[] { };

            var nodeList = new List<ExchangeSelectNode>();
            foreach (var exc in exchanges)
            {
                var excNode = new ExchangeSelectNode { Node = exc };
                nodeList.Add(excNode);

                var comds = commodityListGetter(exc);
                if (comds?.Any() != true) continue;

                var comdTypeGroups = comds.GroupBy(i => i.SledCommodityType);
                foreach (IGrouping<int, NativeCommodity> typeGroup in comdTypeGroups)
                {
                    var commodityTypeNode = new CommodityTypeSelectNode { Node = typeGroup.Key };
                    excNode.Children.Add(commodityTypeNode);

                    foreach (var comd in typeGroup)
                    {
                        var comdNode = new CommoditySelectNode { Node = comd };
                        commodityTypeNode.Children.Add(comdNode);
                    }
                }
            }

            this.Exchanges.Clear();
            this.Exchanges.AddRange(nodeList);
        }
    }

    /// <summary>
    /// 交易所选择
    /// </summary>
    public class ExchangeSelectNode : NodeWithChildren<NativeExchange, CommodityTypeSelectNode>
    {
        public ExchangeSelectNode() : base()
        {
        }

        private bool isVisiable = true;
        public bool IsVisiable
        {
            get { return isVisiable; }
            set { SetProperty(ref isVisiable, value); }
        }
    }

    /// <summary>
    /// 产品类别选择
    /// 类别参考 <seealso cref="xueqiao.contract.standard.SledCommodityType"/>
    /// </summary>
    public class CommodityTypeSelectNode : NodeWithChildren<int, CommoditySelectNode>
    {
        public CommodityTypeSelectNode() : base()
        {
        }
    }

    /// <summary>
    /// 产品选择
    /// </summary>
    public class CommoditySelectNode : NodeWithChildren<NativeCommodity, ContractSelectNode>
    {
        public CommoditySelectNode() : base()
        {
        }

        private bool isVisiable = true;
        public bool IsVisiable
        {
            get { return isVisiable; }
            set { SetProperty(ref isVisiable, value); }
        }
    }

    public class ContractSelectNode : Model
    {
        public ContractSelectNode(int contractId)
        {
            this.ContractDetailContainer = new TargetContract_TargetContractDetail(contractId);
        }

        public TargetContract_TargetContractDetail ContractDetailContainer { get; private set; }

        private bool isVisiable = true;
        public bool IsVisiable
        {
            get { return isVisiable; }
            set { SetProperty(ref isVisiable, value); }
        }
    }
}
