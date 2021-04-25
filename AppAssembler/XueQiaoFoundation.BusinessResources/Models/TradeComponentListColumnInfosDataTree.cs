using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.BusinessResources.Models
{
    /// <summary>
    /// 交易组件中的列表列信息数据树
    /// </summary>
    public class TradeComponentListColumnInfosDataTree
    {
        /// <summary>
        /// 全局应用的合约列表显示的列
        /// </summary>
        public ListColumnInfo[] GlobalAppliedContractListDisplayColumns { get; set; }

        /// <summary>
        /// 全局应用的组合列表显示的列
        /// </summary>
        public ListColumnInfo[] GlobalAppliedComposeListDisplayColumns { get; set; }

        /// <summary>
        /// 全局应用的委托单列表显示的列
        /// </summary>
        public ListColumnInfo[] GlobalAppliedOrderListEntrustedDisplayColumns { get; set; }

        /// <summary>
        /// 全局应用的预埋单列表显示的列
        /// </summary>
        public ListColumnInfo[] GlobalAppliedOrderListParkedDisplayColumns { get; set; }

        /// <summary>
        /// 全局应用的条件单列表显示的列
        /// </summary>
        public ListColumnInfo[] GlobalAppliedOrderListConditionDisplayColumns { get; set; }

        /// <summary>
        /// 全局应用的成交列表显示的列
        /// </summary>
        public ListColumnInfo[] GlobalAppliedTradeListDisplayColumns { get; set; }
    }
}
