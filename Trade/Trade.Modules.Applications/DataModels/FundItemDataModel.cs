using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using XueQiaoFoundation.BusinessResources.DataModels;

namespace XueQiaoWaf.Trade.Modules.Applications.DataModels
{
    public class FundItemDataModel : Model
    {
        private double? _preFund;
        private double? _depositAmount;
        private double? _withdrawAmount;
        private double? _closeProfit;
        private double? _positionProfit;
        private double? _useMargin;
        private double? _frozenMargin;
        private double? _useCommission;
        private double? _frozenCommission;
        private double? _availableFund;
        private double? _dynamicBenefit;
        private double? _riskRate;
        private double? _creditAmount;

        public FundItemDataModel(SubAccountFieldsForTradeData subAccountFields, bool isBaseCurrency, string currency)
        {
            this.SubAccountFields = subAccountFields;
            this.IsBaseCurrency = isBaseCurrency;
            this.Currency = currency;
            InvalidateIsExistFund();
        }

        public SubAccountFieldsForTradeData SubAccountFields { get; private set; }

        /// <summary>
        /// 是否是基币
        /// </summary>
        public bool IsBaseCurrency { get; private set; }

        /// <summary>
        /// 币种
        /// </summary>
        public string Currency { get; private set; }
        
        public double? PreFund
        {
            get
            {
                return _preFund;
            }
            set
            {
                SetProperty(ref _preFund, value);
            }
        }

        public double? DepositAmount
        {
            get
            {
                return _depositAmount;
            }
            set
            {
                SetProperty(ref _depositAmount, value);
            }
        }

        public double? WithdrawAmount
        {
            get
            {
                return _withdrawAmount;
            }
            set
            {
                SetProperty(ref _withdrawAmount, value);
            }
        }

        public double? CloseProfit
        {
            get
            {
                return _closeProfit;
            }
            set
            {
                if (SetProperty(ref _closeProfit, value))
                {
                    InvalidateTotalProfit();
                }
            }
        }

        public double? PositionProfit
        {
            get
            {
                return _positionProfit;
            }
            set
            {
                if (SetProperty(ref _positionProfit, value))
                {
                    InvalidateTotalProfit();
                }
            }
        }

        public double? UseMargin
        {
            get
            {
                return _useMargin;
            }
            set
            {
                SetProperty(ref _useMargin, value);
            }
        }

        public double? FrozenMargin
        {
            get
            {
                return _frozenMargin;
            }
            set
            {
                SetProperty(ref _frozenMargin, value);
            }
        }

        public double? UseCommission
        {
            get
            {
                return _useCommission;
            }
            set
            {
                SetProperty(ref _useCommission, value);
            }
        }

        public double? FrozenCommission
        {
            get
            {
                return _frozenCommission;
            }
            set
            {
                SetProperty(ref _frozenCommission, value);
            }
        }

        public double? AvailableFund
        {
            get
            {
                return _availableFund;
            }
            set
            {
                SetProperty(ref _availableFund, value);
            }
        }

        public double? DynamicBenefit
        {
            get
            {
                return _dynamicBenefit;
            }
            set
            {
                SetProperty(ref _dynamicBenefit, value);
            }
        }

        /// <summary>
        /// 风险度。已乘100
        /// </summary>
        public double? RiskRate
        {
            get
            {
                return _riskRate;
            }
            set
            {
                SetProperty(ref _riskRate, value);
            }
        }

        public double? CreditAmount
        {
            get
            {
                return _creditAmount;
            }
            set
            {
                SetProperty(ref _creditAmount, value);
            }
        }

        private double? goodsValue;
        // 市值
        public double? GoodsValue
        {
            get { return goodsValue; }
            set { SetProperty(ref goodsValue, value); }
        }

        private double? leverage;
        // 杠杆
        public double? Leverage
        {
            get { return leverage; }
            set { SetProperty(ref leverage, value); }
        }

        public void InvalidateIsExistFund()
        {
            var compareVars = new double?[] { DepositAmount, WithdrawAmount, CloseProfit, PositionProfit, UseMargin, FrozenMargin, UseCommission, FrozenCommission, AvailableFund, DynamicBenefit, RiskRate, CreditAmount };
            this.IsExistFund = compareVars.Any(i => (i != null && i != 0));
        }

        private bool isExistFund;
        public bool IsExistFund
        {
            get { return isExistFund; }
            private set { SetProperty(ref isExistFund, value); }
        }

        private double? totalProfit;
        /// <summary>
        /// 总盈亏
        /// </summary>
        public double? TotalProfit
        {
            get { return totalProfit; }
            private set { SetProperty(ref totalProfit, value); }
        }

        private void InvalidateTotalProfit()
        {
            this.TotalProfit = this.CloseProfit + this.PositionProfit;
        }
    }
}
