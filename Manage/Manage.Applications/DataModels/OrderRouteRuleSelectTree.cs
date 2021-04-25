using NativeModel.Contract;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Model;

namespace Manage.Applications.DataModels
{
    public class OrderRouteRuleSelectTree
    {
        public OrderRouteRuleSelectTree()
        {
            Exchanges = new ObservableCollection<RuleExchangeSelectNode>();
        }

        public ObservableCollection<RuleExchangeSelectNode> Exchanges { get; private set; }

        /// <summary>
        /// 更新 Tree 数据
        /// </summary>
        /// <param name="exchanges"></param>
        /// <param name="commodityListGetter"></param>
        public void UpdateTreeData(IEnumerable<NativeExchange> exchanges,
            Func<NativeExchange, IEnumerable<NativeCommodity>> commodityListGetter,
            Func<NativeCommodity, bool> commodityItemIsCheckedGetter)
        {
            Debug.Assert(commodityListGetter != null);
            if (exchanges == null) exchanges = new NativeExchange[] { };

            var nodeList = new List<RuleExchangeSelectNode>();
            foreach (var exc in exchanges)
            {
                var excNode = new RuleExchangeSelectNode { Node = exc };
                nodeList.Add(excNode);

                var comds = commodityListGetter(exc);
                if (comds?.Any() != true) continue;

                var comdTypeGroups = comds.GroupBy(i => i.SledCommodityType);
                foreach (IGrouping<int, NativeCommodity> typeGroup in comdTypeGroups)
                {
                    var commodityTypeNode = new RuleCommodityTypeSelectNode { Node = typeGroup.Key };
                    excNode.Children.Add(commodityTypeNode);

                    foreach (var comd in typeGroup)
                    {
                        var comdNode = new OrderRouteRuleCommoditySelectModel { Commodity = comd };
                        if (commodityItemIsCheckedGetter != null)
                        {
                            comdNode.IsChecked = commodityItemIsCheckedGetter(comd);
                        }
                        commodityTypeNode.Children.Add(comdNode);
                    }
                }
            }

            this.Exchanges.Clear();
            this.Exchanges.AddRange(nodeList);
        }
    }

    public class RuleExchangeSelectNode : NodeWithChildren<NativeExchange, RuleCommodityTypeSelectNode>
    {
    }

    public class RuleCommodityTypeSelectNode : NodeWithChildren<int, OrderRouteRuleCommoditySelectModel>
    {
    }
}
