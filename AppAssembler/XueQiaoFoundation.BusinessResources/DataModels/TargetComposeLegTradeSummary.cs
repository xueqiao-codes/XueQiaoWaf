using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;

namespace XueQiaoFoundation.BusinessResources.DataModels
{
    /// <summary>
    /// 组合标的的腿成交概要
    /// </summary>
    public class TargetComposeLegTradeSummary : Model
    {
        public TargetComposeLegTradeSummary(long belongComposeId, int legContractId, LegTradeSummaryPriceType summaryPriceType)
        {
            this.BelongComposeId = belongComposeId;
            this.LegContractId = legContractId;
            this.SummaryPriceType = summaryPriceType;
        }

        /// <summary>
        /// 所属组合 id
        /// </summary>
        public long BelongComposeId { get; private set; }

        /// <summary>
        /// 腿合约 id 
        /// </summary>
        public int LegContractId { get; private set; }

        /// <summary>
        /// 概要的价格类型
        /// </summary>
        public LegTradeSummaryPriceType SummaryPriceType { get; private set; }

        private NativeComposeLeg legMeta;
        /// <summary>
        /// 腿的简明信息
        /// </summary>
        public NativeComposeLeg LegMeta
        {
            get { return legMeta; }
            set
            {
                SetProperty(ref legMeta, value);
                this.LegVariableName = legMeta?.VariableName;
            }
        }

        private string legVariableName;
        /// <summary>
        /// 腿变量名称
        /// </summary>
        public string LegVariableName
        {
            get { return legVariableName; }
            private set { SetProperty(ref legVariableName, value); }
        }
        
        private int? tradeVolume;
        /// <summary>
        /// 成交量
        /// </summary>
        public int? TradeVolume
        {
            get { return tradeVolume; }
            set { SetProperty(ref tradeVolume, value); }
        }

        private double? summaryPrice;
        /// <summary>
        /// 概要价格。根据概要价格类型<see cref="SummaryPriceType"/>设置价格
        /// </summary>
        public double? SummaryPrice
        {
            get { return summaryPrice; }
            set { SetProperty(ref summaryPrice, value); }
        }

        private int? totalVolume;
        /// <summary>
        /// 总量
        /// </summary>
        public int? TotalVolume
        {
            get { return totalVolume; }
            set { SetProperty(ref totalVolume, value); }
        }
    }

    /// <summary>
    /// 腿成交概要价格类型
    /// </summary>
    public enum LegTradeSummaryPriceType
    {
        TradeAvgPrice = 1,      // 成交均价
        CalculatePrice = 2,     // 计算价格
    }
}
