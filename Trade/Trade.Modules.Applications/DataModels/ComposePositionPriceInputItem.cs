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
    /// 组合持仓录入的价格录入项 data model
    /// </summary>
    public class ComposePositionPriceInputItem : Model
    {
        public ComposePositionPriceInputItem(ComposePositionPriceType priceType)
        {
            this.PriceType = priceType;
        }

        public ComposePositionPriceType PriceType { get; private set; }
        
        private double price;
        public double Price
        {
            get { return price; }
            set { SetProperty(ref price, value); }
        }

        private ComposeLegDetail legDetail;
        public ComposeLegDetail LegDetail
        {
            get { return legDetail; }
            set { SetProperty(ref legDetail, value); }
        }
    }

    public enum ComposePositionPriceType
    {
        PriceDiff = 1,  // 价差
        LegPrice = 2,   // 腿价格
    }
}
