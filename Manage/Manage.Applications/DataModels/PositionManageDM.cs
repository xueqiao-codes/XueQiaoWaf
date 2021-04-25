using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using System.Windows.Input;
using XueQiaoFoundation.BusinessResources.DataModels;

namespace Manage.Applications.DataModels
{
    /// <summary>
    /// 持仓管理项 datamodel
    /// </summary>
    public class PositionManageDM : Model
    {
        public PositionManageDM(DiscretePositionDM positionContent, ICommand showDetailCmd)
        {
            this.PositionContent = positionContent;
            this.ShowDetailCmd = showDetailCmd;
        }

        public DiscretePositionDM PositionContent { get; private set; }

        public ICommand ShowDetailCmd { get; private set; }

        #region 持仓管理列表列名称
        public const string PositionManageColumn_Name = "PositionManageColumn_Name";
        public const string PositionManageColumn_PrevPosition = "PositionManageColumn_PrevPosition";
        public const string PositionManageColumn_LongPosition = "PositionManageColumn_LongPosition";
        public const string PositionManageColumn_ShortPosition = "PositionManageColumn_ShortPosition";
        public const string PositionManageColumn_LongPosition_CurrentDay = "PositionManageColumn_LongPosition_CurrentDay";
        public const string PositionManageColumn_ShortPosition_CurrentDay = "PositionManageColumn_ShortPosition_CurrentDay";
        public const string PositionManageColumn_NetPosition = "PositionManageColumn_NetPosition";
        public const string PositionManageColumn_PositionAvgPrice = "PositionManageColumn_PositionAvgPrice";
        public const string PositionManageColumn_CalculatePrice = "PositionManageColumn_CalculatePrice";
        public const string PositionManageColumn_PositionProfit = "PositionManageColumn_PositionProfit";
        public const string PositionManageColumn_CloseProfit = "PositionManageColumn_CloseProfit";
        public const string PositionManageColumn_TotalProfit = "PositionManageColumn_TotalProfit";
        public const string PositionManageColumn_UseCommission = "PositionManageColumn_UseCommission";
        public const string PositionManageColumn_FrozenCommission = "PositionManageColumn_FrozenCommission";
        public const string PositionManageColumn_UseMargin = "PositionManageColumn_UseMargin";
        public const string PositionManageColumn_FrozenMargin = "PositionManageColumn_FrozenMargin";
        public const string PositionManageColumn_GoodsValue = "PositionManageColumn_GoodsValue";
        public const string PositionManageColumn_Leverage = "PositionManageColumn_Leverage";
        public const string PositionManageColumn_Currency = "PositionManageColumn_Currency";
        public const string PositionManageColumn_SettlementTime = "PositionManageColumn_SettlementTime";
        public const string PositionManageColumn_Detail = "PositionManageColumn_Detail";
        #endregion

        /// <summary>
        /// 子账号持仓列表显示列
        /// </summary>
        public static readonly string[] PositionViewBySubAccountListColumns = new string[] 
        {
            PositionManageColumn_Name, PositionManageColumn_PrevPosition,PositionManageColumn_LongPosition,
            PositionManageColumn_ShortPosition, PositionManageColumn_NetPosition, PositionManageColumn_PositionAvgPrice,
            PositionManageColumn_CalculatePrice, PositionManageColumn_PositionProfit, PositionManageColumn_CloseProfit,
            PositionManageColumn_TotalProfit, PositionManageColumn_UseCommission, PositionManageColumn_FrozenCommission,
            PositionManageColumn_UseMargin, PositionManageColumn_FrozenMargin, PositionManageColumn_GoodsValue, PositionManageColumn_Leverage,
            PositionManageColumn_Currency, PositionManageColumn_Detail
        };

        /// <summary>
        /// 子账号历史持仓列表显示列
        /// </summary>
        public static readonly string[] HistoryPositionViewBySubAccountListColumns = new string[]
        {
            PositionManageColumn_Name, PositionManageColumn_PrevPosition,PositionManageColumn_LongPosition_CurrentDay,
            PositionManageColumn_ShortPosition_CurrentDay, PositionManageColumn_NetPosition, PositionManageColumn_PositionAvgPrice,
            PositionManageColumn_CalculatePrice, PositionManageColumn_PositionProfit, PositionManageColumn_CloseProfit,
            PositionManageColumn_TotalProfit, PositionManageColumn_UseCommission,
            PositionManageColumn_UseMargin, PositionManageColumn_GoodsValue, PositionManageColumn_Leverage,
            PositionManageColumn_Currency, PositionManageColumn_SettlementTime, PositionManageColumn_Detail
        };

        /// <summary>
        /// 资金账号持仓列表显示列
        /// </summary>
        public static readonly string[] PositionViewByFundAccountListColumns = new string[]
        {
           PositionManageColumn_Name, PositionManageColumn_PrevPosition,PositionManageColumn_LongPosition,
            PositionManageColumn_ShortPosition, PositionManageColumn_NetPosition, PositionManageColumn_PositionAvgPrice,
            PositionManageColumn_CalculatePrice, PositionManageColumn_PositionProfit, PositionManageColumn_CloseProfit,
            PositionManageColumn_TotalProfit, PositionManageColumn_UseCommission, PositionManageColumn_FrozenCommission,
            PositionManageColumn_UseMargin, PositionManageColumn_FrozenMargin, PositionManageColumn_GoodsValue, PositionManageColumn_Leverage,
            PositionManageColumn_Currency, PositionManageColumn_Detail
        };
    }
}
