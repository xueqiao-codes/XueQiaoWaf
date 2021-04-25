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
    /// 组合持仓合并的价格录入项
    /// </summary>
    public class CPMergePriceInputItem : Model
    {
        public CPMergePriceInputItem(CPMergePriceInputItemType itemType)
        {
            this.ItemType = itemType;
        }

        public CPMergePriceInputItemType ItemType { get; private set; }

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

    public enum CPMergePriceInputItemType
    {
        PriceDiffInputItem = 1,     // 价差录入项
        LegPriceInputItem = 2       // 腿价格录入项
    }
}
