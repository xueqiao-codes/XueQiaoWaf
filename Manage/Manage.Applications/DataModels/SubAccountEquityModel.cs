using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using xueqiao.trade.hosting.asset.thriftapi;

namespace Manage.Applications.DataModels
{
    /// <summary>
    /// 操作账户的权益 data model
    /// </summary>
    public class SubAccountEquityModel : Model
    {
        public SubAccountEquityModel(HostingFund equityData)
        {
            this.EquityData = equityData;
        }

        public HostingFund EquityData { get; private set; }


        #region 操作账户权益列表列名称
        public const string SubAccountEquityColumn_DynamicBenefit = "SubAccountEquityColumn_DynamicBenefit"; // 动态权益
        public const string SubAccountEquityColumn_Currency = "SubAccountEquityColumn_Currency"; // 币种
        public const string SubAccountEquityColumn_BaseCurrency = "SubAccountEquityColumn_BaseCurrency"; // 基准货币
        public const string SubAccountEquityColumn_AvailableFund = "SubAccountEquityColumn_AvailableFund"; // 可用资金

        public const string SubAccountEquityColumn_CloseProfit = "SubAccountEquityColumn_CloseProfit"; // 平仓盈亏
        public const string SubAccountEquityColumn_PositionProfit = "SubAccountEquityColumn_PositionProfit"; // 持仓盈亏
        public const string SubAccountEquityColumn_TotalProfit = "SubAccountEquityColumn_TotalProfit"; // 总盈亏
        public const string SubAccountEquityColumn_UseCommission = "SubAccountEquityColumn_UseCommission"; // 已扣手续费
        public const string SubAccountEquityColumn_FrozenCommission = "SubAccountEquityColumn_FrozenCommission"; // 冻结手续费

        public const string SubAccountEquityColumn_UseMargin = "SubAccountEquityColumn_UseMargin"; // 持仓保证金
        public const string SubAccountEquityColumn_FrozenMargin = "SubAccountEquityColumn_FrozenMargin"; // 冻结保证金
        public const string SubAccountEquityColumn_DepositAmount = "SubAccountEquityColumn_DepositAmount"; // 入金金额
        public const string SubAccountEquityColumn_WithdrawAmount = "SubAccountEquityColumn_WithdrawAmount"; // 出金金额
        public const string SubAccountEquityColumn_PreFund = "SubAccountEquityColumn_PreFund"; // 上次结算权益

        public const string SubAccountEquityColumn_RiskRate = "SubAccountEquityColumn_RiskRate"; // 风险度
        public const string SubAccountEquityColumn_GoodsValue = "SubAccountEquityColumn_GoodsValue"; // 市值
        public const string SubAccountEquityColumn_Leverage = "SubAccountEquityColumn_Leverage"; // 杠杆
        public const string SubAccountEquityColumn_CreditAmount = "SubAccountEquityColumn_CreditAmount"; // 信用额度
        #endregion

        /// <summary>
        /// 总权益列表显示列
        /// </summary>
        public static readonly string[] TotalEquityListDisplayColumns = new string[]
        {
            SubAccountEquityColumn_DynamicBenefit, SubAccountEquityColumn_BaseCurrency, SubAccountEquityColumn_AvailableFund,

            SubAccountEquityColumn_PositionProfit, SubAccountEquityColumn_CloseProfit, SubAccountEquityColumn_TotalProfit,

            SubAccountEquityColumn_UseCommission, SubAccountEquityColumn_FrozenCommission, SubAccountEquityColumn_UseMargin, SubAccountEquityColumn_FrozenMargin,

            SubAccountEquityColumn_DepositAmount, SubAccountEquityColumn_WithdrawAmount, SubAccountEquityColumn_PreFund,

            SubAccountEquityColumn_RiskRate, SubAccountEquityColumn_GoodsValue, SubAccountEquityColumn_Leverage, SubAccountEquityColumn_CreditAmount
        };


        /// <summary>
        /// 各币种权益列表显示列
        /// </summary>
        public static readonly string[] CurrencyGroupedEquityListDisplayColumns = new string[]
        {
            SubAccountEquityColumn_Currency, SubAccountEquityColumn_DynamicBenefit, SubAccountEquityColumn_AvailableFund,

            SubAccountEquityColumn_PositionProfit, SubAccountEquityColumn_CloseProfit, SubAccountEquityColumn_TotalProfit,

            SubAccountEquityColumn_UseCommission, SubAccountEquityColumn_FrozenCommission, SubAccountEquityColumn_UseMargin, SubAccountEquityColumn_FrozenMargin,

            SubAccountEquityColumn_DepositAmount, SubAccountEquityColumn_WithdrawAmount, SubAccountEquityColumn_PreFund,

            SubAccountEquityColumn_RiskRate, SubAccountEquityColumn_GoodsValue, SubAccountEquityColumn_Leverage, SubAccountEquityColumn_CreditAmount
        };

    }
}
