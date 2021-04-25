using NativeModel.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;

namespace XueQiaoFoundation.BusinessResources.DataModels
{
    public class CommodityDetailContainer : Model
    {
        public CommodityDetailContainer(int commodityId)
        {
            this.CommodityId = commodityId;
        }

        public int CommodityId { get; private set; }

        // 商品详情
        private NativeCommodity commodityDetail;
        public NativeCommodity CommodityDetail
        {
            get { return commodityDetail; }
            set { SetProperty(ref commodityDetail, value); }
        }

        // 交易所详情
        private NativeExchange exchangeDetail;
        public NativeExchange ExchangeDetail
        {
            get { return exchangeDetail; }
            set { SetProperty(ref exchangeDetail, value); }
        }
    }
}
