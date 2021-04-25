using NativeModel.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.trade.hosting;
using XueQiaoFoundation.Shared.Helper;

namespace Manage.Applications.DataModels
{
    public static class Extensions
    {
        /// <summary>
        /// 将路由树打散
        /// </summary>
        /// <param name="tree"></param>
        /// <returns></returns>
        public static IEnumerable<OrderRouteRuleDiscreteModel> HostingOrderRouteTree2DiscreteRules(this Dictionary<string, HostingOrderRouteExchangeNode> orderRouteTree)
        {
            var discreteRuleList = new List<OrderRouteRuleDiscreteModel>();
            // 交易所层级的计算
            var sortedExhcangeNodePairs = orderRouteTree.OrderBy(i => i.Key).ToArray();
            foreach (var excNodePair in sortedExhcangeNodePairs)
            {
                var exchangeMic = excNodePair.Key;
                var exchangeNode = excNodePair.Value;
                var commodityTypeNodes = exchangeNode.SubCommodityTypeNodes;
                var exchangeLevelRule = exchangeNode.RelatedInfo;
                if (exchangeLevelRule != null)
                {
                    // 交易所层级存在路由规则
                    discreteRuleList.Add(new OrderRouteRuleDiscreteModel
                    {
                        RouteLevelType = OrderRouteRuleLevelType.Exchange,
                        IsForbiddenTradeRule = exchangeLevelRule.Forbidden,
                        RouteToTradeAccountId = exchangeLevelRule.MainTradeAccountId,
                        RouteExchangeMic = exchangeMic
                    });
                }

                // 下一个层级(商品类型)的计算
                if (commodityTypeNodes != null)
                {
                    var commodityTypeNodePairs = commodityTypeNodes.OrderBy(i => i.Key).ToArray();
                    foreach (var typeNodePair in commodityTypeNodePairs)
                    {
                        var commType = typeNodePair.Key;
                        var commTypeNode = typeNodePair.Value;
                        var commodityNodes = commTypeNode.SubCommodityCodeNodes;
                        var commTypeLevelRule = commTypeNode.RelatedInfo;
                        if (commTypeLevelRule != null)
                        {
                            discreteRuleList.Add(new OrderRouteRuleDiscreteModel
                            {
                                RouteLevelType = OrderRouteRuleLevelType.CommodityType,
                                IsForbiddenTradeRule = commTypeLevelRule.Forbidden,
                                RouteToTradeAccountId = commTypeLevelRule.MainTradeAccountId,
                                RouteExchangeMic = exchangeMic,
                                RouteCommodityType = commType
                            });
                        }

                        // 下一个层级(商品类型)的计算
                        if (commodityNodes != null)
                        {
                            var commodityNodePairs = commodityNodes.OrderBy(i => i.Key).ToArray();
                            foreach (var commodityNodePair in commodityNodePairs)
                            {
                                var commodityCode = commodityNodePair.Key;
                                var commodityNode = commodityNodePair.Value;
                                var commodityLevelRule = commodityNode.RelatedInfo;
                                if (commodityLevelRule != null)
                                {
                                    discreteRuleList.Add(new OrderRouteRuleDiscreteModel
                                    {
                                        RouteLevelType = OrderRouteRuleLevelType.Commodity,
                                        IsForbiddenTradeRule = commodityLevelRule.Forbidden,
                                        RouteToTradeAccountId = commodityLevelRule.MainTradeAccountId,
                                        RouteExchangeMic = exchangeMic,
                                        RouteCommodityType = commType,
                                        RouteCommoditycode = commodityCode
                                    });
                                }
                            } 
                        }
                    }
                }
            }
            return discreteRuleList.ToArray();
        }

        public static Dictionary<string, HostingOrderRouteExchangeNode> DiscreteOrderRouteRules2OrderRouteTree(this IEnumerable<OrderRouteRuleDiscreteModel> discreteRules)
        {
            var tarOrderRoutetree = new Dictionary<string, HostingOrderRouteExchangeNode>();

            var exchangeGroupedRules = discreteRules.GroupBy(i => i.RouteExchangeMic).ToArray();
            foreach (var excGroupItem in exchangeGroupedRules)
            {
                var exchangeMic = excGroupItem.Key;
                if (tarOrderRoutetree.ContainsKey(exchangeMic))
                {
                    continue;
                }

                var exchangeNode = new HostingOrderRouteExchangeNode
                {
                    SledExchangeCode = excGroupItem.Key,
                    SubCommodityTypeNodes = new Dictionary<short, HostingOrderRouteCommodityTypeNode>()
                };
                tarOrderRoutetree[exchangeMic] = exchangeNode;

                var excLevelRuleItem = excGroupItem
                    .FirstOrDefault(i => i.RouteLevelType == OrderRouteRuleLevelType.Exchange 
                        && !string.IsNullOrEmpty(i.RouteExchangeMic)
                        && i.RouteToTradeAccountId > 0);
                if (excLevelRuleItem != null)
                {
                    exchangeNode.RelatedInfo = new HostingOrderRouteRelatedInfo
                    {
                        MainTradeAccountId = excLevelRuleItem.RouteToTradeAccountId,
                        Forbidden = excLevelRuleItem.IsForbiddenTradeRule
                    };
                }

                // 下一个层级（商品类型）计算
                var commTypeGroupedRules = excGroupItem.Where(i => i.RouteLevelType != OrderRouteRuleLevelType.Exchange)
                    .GroupBy(i => i.RouteCommodityType).ToArray();
                foreach (var commTypeItem in commTypeGroupedRules)
                {
                    var commType = (short)commTypeItem.Key;
                    if (exchangeNode.SubCommodityTypeNodes.ContainsKey(commType))
                    {
                        continue;
                    }
                    
                    var commTypeNode = new HostingOrderRouteCommodityTypeNode
                    {
                        SledCommodityType = commType,
                        SubCommodityCodeNodes = new Dictionary<string, HostingOrderRouteCommodityCodeNode>()
                    };
                    exchangeNode.SubCommodityTypeNodes[commType] = commTypeNode;

                    var commTypeLevelRuleItem = commTypeItem.FirstOrDefault(i => i.RouteLevelType == OrderRouteRuleLevelType.CommodityType
                                                                && i.RouteToTradeAccountId > 0);
                    if (commTypeLevelRuleItem != null)
                    {
                        commTypeNode.RelatedInfo = new HostingOrderRouteRelatedInfo
                        {
                            MainTradeAccountId = commTypeLevelRuleItem.RouteToTradeAccountId,
                            Forbidden = commTypeLevelRuleItem.IsForbiddenTradeRule
                        };
                    }

                    // 下一个层级（商品）计算
                    var commodityRules = commTypeItem.Where(i => i.RouteLevelType == OrderRouteRuleLevelType.Commodity).ToArray();
                    foreach (var commodityItem in commodityRules)
                    {
                        var commodityCode = commodityItem.RouteCommoditycode;
                        if (commTypeNode.SubCommodityCodeNodes.ContainsKey(commodityCode))
                        {
                            continue;
                        }

                        var commodityNode = new HostingOrderRouteCommodityCodeNode
                        {
                            SledCommodityCode = commodityCode
                        };
                        commTypeNode.SubCommodityCodeNodes[commodityCode] = commodityNode;

                        if (commodityItem.RouteToTradeAccountId > 0)
                        {
                            commodityNode.RelatedInfo = new HostingOrderRouteRelatedInfo
                            {
                                MainTradeAccountId = commodityItem.RouteToTradeAccountId,
                                Forbidden = commodityItem.IsForbiddenTradeRule
                            };
                        }
                    }
                }
            }
            return tarOrderRoutetree;
        }

        public static IEnumerable<OrderRouteRuleListItemModel> OrderRouteDiscreteRules2ListItems(this IEnumerable<OrderRouteRuleDiscreteModel> discreteRules)
        {
            // 转化为显示项目
            var tarListItems = new List<OrderRouteRuleListItemModel>();
            var exchangeGroupedRules = discreteRules.GroupBy(i => i.RouteExchangeMic).ToArray();
            foreach (var exchangeGroupedItem in exchangeGroupedRules)
            {
                var exchangeMic = exchangeGroupedItem.Key;
                var allRulesInThisExchange = exchangeGroupedItem.ToArray();
                var exchangeLevelRules = allRulesInThisExchange.Where(i => i.RouteLevelType == OrderRouteRuleLevelType.Exchange
                            && !string.IsNullOrEmpty(i.RouteExchangeMic)
                            && i.RouteToTradeAccountId > 0).ToArray();
                tarListItems.AddRange(exchangeLevelRules
                    .Select(i => new OrderRouteRuleListItemModel().FillOrderRouteRuleListItemModelByDiscreteItem(i))
                    .ToArray());

                var commTypeGroupedRules = allRulesInThisExchange
                    .Where(i=>i.RouteLevelType != OrderRouteRuleLevelType.Exchange)
                    .GroupBy(i => i.RouteCommodityType).ToArray();
                foreach (var commTypeGroupedItem in commTypeGroupedRules)
                {
                    var commodityType = commTypeGroupedItem.Key;
                    var allRulesInThisCommType = commTypeGroupedItem.ToArray();
                    var commTypeLevelRules = allRulesInThisCommType.Where(i => i.RouteLevelType == OrderRouteRuleLevelType.CommodityType
                                && i.RouteToTradeAccountId > 0).ToArray();
                    tarListItems.AddRange(commTypeLevelRules
                        .Select(i => new OrderRouteRuleListItemModel().FillOrderRouteRuleListItemModelByDiscreteItem(i))
                        .ToArray());

                    var commodityLevelRules = allRulesInThisCommType.Where(i => i.RouteLevelType == OrderRouteRuleLevelType.Commodity
                                                    && i.RouteToTradeAccountId > 0).ToArray();

                    var forbiddenGroupedRules = commodityLevelRules.GroupBy(i => i.IsForbiddenTradeRule).ToArray();
                    foreach (var forbiddenGroupedItem in forbiddenGroupedRules)
                    {
                        var isForbiddenTradeRule = forbiddenGroupedItem.Key;
                        var allRulesInThisForbiddenState = forbiddenGroupedItem.ToArray();
                        var routeToAccountGroupedRules = allRulesInThisForbiddenState.GroupBy(i => i.RouteToTradeAccountId).ToArray();
                        foreach (var routeToAccountGroupedItem in routeToAccountGroupedRules)
                        {
                            var routeToTradeAccountId = routeToAccountGroupedItem.Key;
                            var allRulesInThisRoutedAccount = routeToAccountGroupedItem.ToArray();
                            if (allRulesInThisRoutedAccount.Count() == 0)
                            {
                                continue;
                            }
                            var tarListItemModel = new OrderRouteRuleListItemModel
                            {
                                RouteLevelType = OrderRouteRuleLevelType.Commodity,
                                IsForbiddenTradeRule = isForbiddenTradeRule,
                                RouteToTradeAccountId = routeToTradeAccountId,
                                RouteExchangeMic = exchangeMic,
                                RouteCommodityType = commodityType
                            };
                            tarListItemModel
                                .GroupedCommodities
                                .AddRange(allRulesInThisRoutedAccount.Select(i => new NativeCommodity
                                {
                                    SledCommodityCode = i.RouteCommoditycode,
                                    ExchangeMic = exchangeMic,
                                    SledCommodityType = commodityType
                                }).ToArray());
                            tarListItems.Add(tarListItemModel);
                        }
                    }
                }
            }
            return tarListItems.ToArray();
        }

        public static OrderRouteRuleListItemModel FillOrderRouteRuleListItemModelByDiscreteItem(this OrderRouteRuleListItemModel ruleListItemModel, OrderRouteRuleDiscreteModel discreteModel)
        {
            ruleListItemModel.RouteLevelType = discreteModel.RouteLevelType;
            ruleListItemModel.IsForbiddenTradeRule = discreteModel.IsForbiddenTradeRule;
            ruleListItemModel.RouteToTradeAccountId = discreteModel.RouteToTradeAccountId;
            ruleListItemModel.RouteExchangeMic = discreteModel.RouteExchangeMic;
            ruleListItemModel.RouteCommodityType = discreteModel.RouteCommodityType;
            return ruleListItemModel;
        }


        /// <summary>
        /// 填充订单路由选择树
        /// </summary>
        /// <param name="exchanges">交易所列表</param>
        /// <param name="commodities">商品列表</param>
        /// <param name="commodityIsCheckedFactory">商品是否选中获取方法</param>
        /// <returns></returns>
        public static OrderRouteRuleSelectTree FillDataIntoOrderRouteSelectTree(this OrderRouteRuleSelectTree tree,
            IEnumerable<NativeExchange> exchanges,
            IEnumerable<NativeCommodity> commodities,
            Func<NativeCommodity, bool> commodityIsCheckedFactory)
        {
            var ruleExchangeNodes = new List<RuleExchangeSelectNode>();
            var exchangeGroups = commodities.GroupBy(i => i.ExchangeMic).ToArray();
            if (exchangeGroups.Any())
            {
                foreach (IGrouping<string, NativeCommodity> exchangeGroup in exchangeGroups)
                {
                    var exchangeMic = exchangeGroup.Key;
                    RuleExchangeSelectNode exchangeNode = ruleExchangeNodes.FirstOrDefault(i => i.Node.ExchangeMic == exchangeMic);
                    if (exchangeNode == null)
                    {
                        var exchange = exchanges.FirstOrDefault(i => i.ExchangeMic == exchangeMic);
                        if (exchange == null)
                        {
                            continue;
                        }
                        exchangeNode = new RuleExchangeSelectNode
                        {
                            Node = exchange
                        };
                        ruleExchangeNodes.Add(exchangeNode);
                    }

                    var commodityTypeGroups = exchangeGroup.GroupBy(i => i.SledCommodityType).ToArray();
                    if (!commodityTypeGroups.Any())
                    {
                        continue;
                    }

                    foreach (IGrouping<int, NativeCommodity> typeGroup in commodityTypeGroups)
                    {
                        var commodityTypeNode = new RuleCommodityTypeSelectNode
                        {
                            Node = typeGroup.Key
                        };
                        exchangeNode.Children.Add(commodityTypeNode);

                        foreach (var commodity in typeGroup)
                        {
                            var commoditySelectModel = new OrderRouteRuleCommoditySelectModel
                            {
                                Commodity = commodity
                            };
                            if (commodityIsCheckedFactory != null)
                            {
                                commoditySelectModel.IsChecked = commodityIsCheckedFactory(commodity);
                            }
                            commodityTypeNode.Children.Add(commoditySelectModel);
                        }
                    }
                }
            }

            tree.Exchanges.Clear();
            tree.Exchanges.AddRange(ruleExchangeNodes);

            return tree;
        }
    }
}
