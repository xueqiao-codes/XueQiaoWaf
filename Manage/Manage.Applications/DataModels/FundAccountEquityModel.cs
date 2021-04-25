using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using System.Windows.Input;
using xueqiao.trade.hosting.terminal.ao;
using xueqiao.trade.hosting.tradeaccount.data;

namespace Manage.Applications.DataModels
{
    /// <summary>
    /// 资金账户的权益 data model
    /// </summary>
    public class FundAccountEquityModel : Model
    {
        public FundAccountEquityModel(TradeAccountFund equityData)
        {
            this.EquityData = equityData;
        }

        public TradeAccountFund EquityData { get; private set; }

        private ICommand showDetailCmd;
        /// <summary>
        /// 显示权益详情 cmd。paramter type is <see cref="FundAccountEquityModel"/>。
        /// </summary>
        public ICommand ShowDetailCmd
        {
            get { return showDetailCmd; }
            set { SetProperty(ref showDetailCmd, value); }
        }

        public static void GenerateEquityModels(HostingTAFundItem srcEquityData,
            out IEnumerable<FundAccountEquityModel> totalEquityItems, 
            out IEnumerable<FundAccountEquityModel> currencyGroupedEquityItems,
            Action<FundAccountEquityModel> equityModelConfigFactory)
        {
            totalEquityItems = null;
            currencyGroupedEquityItems = null;

            if (srcEquityData == null) return;

            totalEquityItems = new FundAccountEquityModel[] { new FundAccountEquityModel(srcEquityData.TotalFund) };
            currencyGroupedEquityItems = srcEquityData.GroupFunds.Values?
                .OrderBy(i => i.CurrencyNo)
                .Select(i => new FundAccountEquityModel(i.GroupTotalFund))
                .ToArray();

            if (equityModelConfigFactory != null)
            {
                foreach (var totalItem in totalEquityItems)
                {
                    equityModelConfigFactory?.Invoke(totalItem);
                }

                foreach (var currencyEquityItem in currencyGroupedEquityItems)
                {
                    equityModelConfigFactory?.Invoke(currencyEquityItem);
                }
            }
        }


        #region 资金账户权益列表列名称
        public const string FundAccountEquityColumn_DynamicBenefit = "FundAccountEquityColumn_DynamicBenefit";  // 动态权益
        public const string FundAccountEquityColumn_Currency = "FundAccountEquityColumn_Currency";  // 币种
        public const string FundAccountEquityColumn_BaseCurrency = "FundAccountEquityColumn_BaseCurrency"; // 基准货币
        public const string FundAccountEquityColumn_Available = "FundAccountEquityColumn_Available"; // 可用资金

        public const string FundAccountEquityColumn_CloseProfit = "FundAccountEquityColumn_CloseProfit"; // 平仓盈亏
        public const string FundAccountEquityColumn_PositionProfit = "FundAccountEquityColumn_PositionProfit"; // 持仓盈亏
        public const string FundAccountEquityColumn_TotalProfit = "FundAccountEquityColumn_TotalProfit"; // 总盈亏
        public const string FundAccountEquityColumn_Commission = "FundAccountEquityColumn_Commission"; // 已扣手续费
        public const string FundAccountEquityColumn_FrozenCash = "FundAccountEquityColumn_FrozenCash"; // 冻结手续费

        public const string FundAccountEquityColumn_CurrMargin = "FundAccountEquityColumn_CurrMargin"; // 持仓保证金
        public const string FundAccountEquityColumn_FrozenMargin = "FundAccountEquityColumn_FrozenMargin"; // 冻结保证金
        public const string FundAccountEquityColumn_Deposit = "FundAccountEquityColumn_Deposit"; // 入金金额
        public const string FundAccountEquityColumn_Withdraw = "FundAccountEquityColumn_Withdraw"; // 出金金额
        public const string FundAccountEquityColumn_PreBalance = "FundAccountEquityColumn_PreBalance"; // 上日结算资金

        public const string FundAccountEquityColumn_RiskRate = "FundAccountEquityColumn_RiskRate"; // 风险度
        public const string FundAccountEquityColumn_Credit = "FundAccountEquityColumn_Credit"; // 信用额度
        public const string FundAccountEquityColumn_Detail = "FundAccountEquityColumn_Detail"; // 详情

        public const string FundAccountEquityColumn_CurrencyChannel = "FundAccountEquityColumn_CurrencyChannel"; // 分类
#endregion

        /// <summary>
        /// 总权益列表显示列
        /// </summary>
        public static readonly string[] TotalEquityListDisplayColumns = new string[]
        {
            FundAccountEquityColumn_DynamicBenefit, FundAccountEquityColumn_BaseCurrency, FundAccountEquityColumn_Available,

            FundAccountEquityColumn_PositionProfit,FundAccountEquityColumn_CloseProfit, FundAccountEquityColumn_TotalProfit,

            FundAccountEquityColumn_Commission, FundAccountEquityColumn_FrozenCash, FundAccountEquityColumn_CurrMargin, FundAccountEquityColumn_FrozenMargin, 

            FundAccountEquityColumn_Deposit, FundAccountEquityColumn_Withdraw, FundAccountEquityColumn_PreBalance,

            FundAccountEquityColumn_RiskRate, FundAccountEquityColumn_Credit
        };


        /// <summary>
        /// 各币种权益列表显示列
        /// </summary>
        public static readonly string[] CurrencyGroupedEquityListDisplayColumns = new string[]
        {
            FundAccountEquityColumn_Currency, FundAccountEquityColumn_DynamicBenefit, FundAccountEquityColumn_Available,

            FundAccountEquityColumn_PositionProfit,FundAccountEquityColumn_CloseProfit, FundAccountEquityColumn_TotalProfit,

            FundAccountEquityColumn_Commission, FundAccountEquityColumn_FrozenCash, FundAccountEquityColumn_CurrMargin, FundAccountEquityColumn_FrozenMargin,

            FundAccountEquityColumn_Deposit, FundAccountEquityColumn_Withdraw, FundAccountEquityColumn_PreBalance,

            FundAccountEquityColumn_RiskRate, FundAccountEquityColumn_Credit,

            FundAccountEquityColumn_Detail
        };

        /// <summary>
        /// 币种权益详情列表显示列
        /// </summary>
        public static readonly string[] CurrencyEquityDetailListDisplayColumns = new string[]
        {
            FundAccountEquityColumn_CurrencyChannel,

            FundAccountEquityColumn_DynamicBenefit, FundAccountEquityColumn_Currency, FundAccountEquityColumn_Available,

            FundAccountEquityColumn_PositionProfit,FundAccountEquityColumn_CloseProfit, FundAccountEquityColumn_TotalProfit,

            FundAccountEquityColumn_Commission, FundAccountEquityColumn_FrozenCash, FundAccountEquityColumn_CurrMargin, FundAccountEquityColumn_FrozenMargin,

            FundAccountEquityColumn_Deposit, FundAccountEquityColumn_Withdraw, FundAccountEquityColumn_PreBalance,

            FundAccountEquityColumn_RiskRate, FundAccountEquityColumn_Credit
        };
    }
}
