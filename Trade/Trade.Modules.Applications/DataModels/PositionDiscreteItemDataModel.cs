using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using XueQiaoFoundation.BusinessResources.DataModels;

namespace XueQiaoWaf.Trade.Modules.Applications.DataModels
{
    /// <summary>
    /// 散列的持仓 data model
    /// </summary>
    public class PositionDiscreteItemDataModel : Model
    {
        public PositionDiscreteItemDataModel(long contractId, SubAccountFieldsForTradeData subAccountFields)
        {
            this.ContractId = contractId;
            this.SubAccountFields = subAccountFields;

            InvalidIsExistPosition();
        }

        public long ContractId { get; private set; }

        public SubAccountFieldsForTradeData SubAccountFields { get; private set; }
        
        private TargetContract_TargetContractDetail contractDetailContainer;
        public TargetContract_TargetContractDetail ContractDetailContainer
        {
            get { return contractDetailContainer; }
            set { SetProperty(ref contractDetailContainer, value); }
        }

        private long? updateTimestamp;
        // 更新时间
        public long? UpdateTimestamp
        {
            get { return updateTimestamp; }
            set { SetProperty(ref updateTimestamp, value); }
        }

        private long? prevPosition;
        // 上日持仓
        public long? PrevPosition
        {
            get { return prevPosition; }
            set
            {
                if (SetProperty(ref prevPosition, value))
                {
                    InvalidIsExistPosition();
                }
            }
        }

        private long? longPosition;
        // 今日长持
        public long? LongPosition
        {
            get { return longPosition; }
            set
            {
                if (SetProperty(ref longPosition, value))
                {
                    InvalidIsExistPosition();
                }
            }
        }

        private long? shortPosition;
        // 今日短持
        public long? ShortPosition
        {
            get { return shortPosition; }
            set
            {
                if (SetProperty(ref shortPosition, value))
                {
                    InvalidIsExistPosition();
                }
            }
        }

        private long? netPosition;
        // 净仓
        public long? NetPosition
        {
            get { return netPosition; }
            set { SetProperty(ref netPosition, value); }
        }

        private double? calculatePrice;
        // 计算价
        public double? CalculatePrice
        {
            get { return calculatePrice; }
            set { SetProperty(ref calculatePrice, value); }
        }
        
        private double? closeProfit;
        // 平仓盈亏
        public double? CloseProfit
        {
            get { return closeProfit; }
            set
            {
                if (SetProperty(ref closeProfit, value))
                {
                    InvalidateTotalProfit();
                }
            }
        }

        private double? positionProfit;
        // 持仓盈亏
        public double? PositionProfit
        {
            get { return positionProfit; }
            set
            {
                if (SetProperty(ref positionProfit, value))
                {
                    InvalidateTotalProfit();
                }
            }
        }

        private double? positionAvgPrice;
        // 持仓均价
        public double? PositionAvgPrice
        {
            get { return positionAvgPrice; }
            set { SetProperty(ref positionAvgPrice, value); }
        }
        
        private double? useMargin;
        // 占用保证金
        public double? UseMargin
        {
            get { return useMargin; }
            set { SetProperty(ref useMargin, value); }
        }

        private double? frozenMargin;
        // 冻结保证金
        public double? FrozenMargin
        {
            get { return frozenMargin; }
            set { SetProperty(ref frozenMargin, value); }
        }

        private double? useCommission;
        // 占用手续费
        public double? UseCommission
        {
            get { return useCommission; }
            set { SetProperty(ref useCommission, value); }
        }

        private double? frozenCommission;
        // 冻结手续费
        public double? FrozenCommission
        {
            get { return frozenCommission; }
            set { SetProperty(ref frozenCommission, value); }
        }

        private string currency;
        // 币种
        public string Currency
        {
            get { return currency; }
            set { SetProperty(ref currency, value); }
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


        private bool isExistPosition;
        // 是否存在持仓
        public bool IsExistPosition
        {
            get { return isExistPosition; }
            private set { SetProperty(ref isExistPosition, value); }
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

        private bool? isXqTargetExpired;
        /// <summary>
        /// 标的是否已过期
        /// </summary>
        public bool? IsXqTargetExpired
        {
            get { return isXqTargetExpired; }
            set { SetProperty(ref isXqTargetExpired, value); }
        }

        private void InvalidIsExistPosition()
        {
            var compareValues = new long?[] { this.PrevPosition, this.LongPosition, this.ShortPosition };
            this.IsExistPosition = compareValues.Any(i => (i != null && i != 0));
        }
    }
}
