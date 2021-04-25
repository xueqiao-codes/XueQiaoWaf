using NativeModel.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using XueQiaoFoundation.BusinessResources.Models;

namespace ContainerShell.Applications.Helper
{
    public static class ContractQuickSearchHelper
    {
        /// <summary>
        /// 获取字符串中的字母片段。
        /// 参考资料<see cref="https://www.dotnetperls.com/regex-split-numbers"/>
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetLetterPieces(string source)
        {
            if (source == null) return null;
            return Regex.Split(source, @"[^A-Za-z]+").Where(i => !string.IsNullOrEmpty(i)).ToArray();            
        }

        /// <summary>
        /// 获取合约快速搜索的语义图
        /// </summary>
        /// <param name="srcSearcText">源搜索文字</param>
        /// <returns></returns>
        public static ContractQuickSearchTextGraph GetContractSearchTextGraph(string srcSearcText)
        {
            if (srcSearcText == null) return null;
            // 正则Group的命名访问
            // 利用 (?<xxx>子表达式) 定义分组别名，这样就可以利用 Groups["xxx"] 进行访问分组/子表达式内容。
            var match = Regex.Match(srcSearcText, @"^(?<PART1>[^0-9]+)(?<PART2>[0-9]*)$");
            if (!match.Success)
            {
                return null;
            }
            return new ContractQuickSearchTextGraph
            {
                CommoditySearchPart = match.Groups["PART1"].Value.Trim(),
                ContractCodeSearchPart = match.Groups["PART2"]?.Value.Trim(),
            };
        }

        /// <summary>
        /// 搜索最匹配的商品。优先搜索商品 code，再搜简称
        /// </summary>
        /// <param name="sourceCommodities">源商品列表</param>
        /// <param name="searchCommodityCodeOrAcronym">搜索商品 code 或简称</param>
        /// <param name="searchAcronymLang">搜索商品简称的语言，不提供则不搜索任何语言的简称</param>
        /// <param name="resultListMaxLimit">最大结果条数限制</param>
        /// <returns></returns>
        public static IEnumerable<NativeCommodity> SearchMostMatchedCommodities(IEnumerable<NativeCommodity> sourceCommodities, 
            string searchCommodityCodeOrAcronym,
            XqAppLanguages? searchAcronymLang,
            int? resultListMaxLimit = null)
        {
            if (sourceCommodities == null) return null;

            IEnumerable<NativeCommodity> destItems = null;
            if (string.IsNullOrEmpty(searchCommodityCodeOrAcronym))
            {
                destItems = sourceCommodities.ToArray();
            }
            else
            {
                destItems = sourceCommodities.Where(i =>
                {
                    if (i.SledCommodityCode.Contains(searchCommodityCodeOrAcronym)) return true;
                    if (searchAcronymLang != null)
                    {
                        switch (searchAcronymLang.Value)
                        {
                            case XqAppLanguages.ENG:
                                return i.EngAcronym?.Contains(searchCommodityCodeOrAcronym) ?? false;
                            case XqAppLanguages.CN:
                                return i.CnAcronym?.Contains(searchCommodityCodeOrAcronym) ?? false;
                            case XqAppLanguages.TC:
                                return i.TcAcronym?.Contains(searchCommodityCodeOrAcronym) ?? false;
                        }
                    }
                    return false;
                }).ToArray();

                var orderedItems = destItems.OrderBy(i => 
                {
                    QuickSearchCommodityMatchedItem matchedItem = null;
                    int? idx_commodityCode = i.SledCommodityCode.IndexOf(searchCommodityCodeOrAcronym);
                    if (idx_commodityCode < 0) idx_commodityCode = null;
                    
                    if (searchAcronymLang != null)
                    {
                        switch (searchAcronymLang.Value)
                        {
                            case XqAppLanguages.ENG:
                                {
                                    var idx_engAcronym = i.EngAcronym?.IndexOf(searchCommodityCodeOrAcronym);
                                    if (idx_engAcronym < 0) idx_engAcronym = null;
                                    matchedItem = new QuickSearchCommodityMatchedItem(idx_commodityCode, idx_engAcronym);
                                    break;
                                }
                            case XqAppLanguages.CN:
                                {
                                    var idx_cnAcronym = i.CnAcronym?.IndexOf(searchCommodityCodeOrAcronym);
                                    if (idx_cnAcronym < 0) idx_cnAcronym = null;
                                    matchedItem = new QuickSearchCommodityMatchedItem(idx_commodityCode, idx_cnAcronym);
                                    break;
                                }
                            case XqAppLanguages.TC:
                                {
                                    var idx_tcAcronym = i.TcAcronym?.IndexOf(searchCommodityCodeOrAcronym);
                                    if (idx_tcAcronym < 0) idx_tcAcronym = null;
                                    matchedItem = new QuickSearchCommodityMatchedItem(idx_commodityCode, idx_tcAcronym);
                                    break;
                                }
                        }
                    }
                    if (matchedItem == null)
                    {
                        matchedItem = new QuickSearchCommodityMatchedItem(idx_commodityCode, null);
                    }
                    return matchedItem;
                }, new QuickSearchCommodityMatchedItemComparer()).ToArray();

                destItems = orderedItems;
            }
            if (resultListMaxLimit != null)
            {
                if (destItems.Count() > resultListMaxLimit)
                {
                    destItems = destItems.Take(resultListMaxLimit.Value);
                }
            }
            return destItems;
        }
    }

    /// <summary>
    /// 合约快速搜索的语义图
    /// </summary>
    public class ContractQuickSearchTextGraph
    {
        /// <summary>
        /// 商品语义部分
        /// </summary>
        public string CommoditySearchPart { get; set; }

        /// <summary>
        /// 合约code语义部分
        /// </summary>
        public string ContractCodeSearchPart { get; set; }

        public override string ToString()
        {
            return $"ContractQuickSearchTextGraph{{CommoditySearchPart:{CommoditySearchPart}, ContractCodeSearchPart:{ContractCodeSearchPart}}}";
        }
    }
    
    public class QuickSearchCommodityMatchedItem 
    {
        public QuickSearchCommodityMatchedItem(int? searchTextInCommodityCodeIdx, int? searchTextInAcronymIdx)
        {
            this.SearchTextInCommodityCodeIdx = searchTextInCommodityCodeIdx;
            this.SearchTextInAcronymIdx = searchTextInAcronymIdx;
        }

        public readonly int? SearchTextInCommodityCodeIdx;
        public readonly int? SearchTextInAcronymIdx;
    }

    public class QuickSearchCommodityMatchedItemComparer : IComparer<QuickSearchCommodityMatchedItem>
    {
        public int Compare(QuickSearchCommodityMatchedItem x, QuickSearchCommodityMatchedItem y)
        {
            if (x.SearchTextInCommodityCodeIdx != null && y.SearchTextInCommodityCodeIdx != null)
            {
                if (x.SearchTextInCommodityCodeIdx != y.SearchTextInCommodityCodeIdx)
                {
                    return (x.SearchTextInCommodityCodeIdx.Value - y.SearchTextInCommodityCodeIdx.Value);
                }
            }
            else if (x.SearchTextInCommodityCodeIdx != null || y.SearchTextInCommodityCodeIdx != null)
            {
                return (x.SearchTextInCommodityCodeIdx ?? int.MaxValue) - (y.SearchTextInCommodityCodeIdx ?? int.MaxValue);
            }

            return (x.SearchTextInAcronymIdx ?? int.MaxValue) - (y.SearchTextInAcronymIdx ?? int.MaxValue);
        }
    }
}
