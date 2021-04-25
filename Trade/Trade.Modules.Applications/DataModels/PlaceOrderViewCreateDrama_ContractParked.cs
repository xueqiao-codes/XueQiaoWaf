using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thrift.Protocol;
using xueqiao.trade.hosting.arbitrage.thriftapi;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoWaf.Trade.Modules.Applications.Helper;

namespace XueQiaoWaf.Trade.Modules.Applications.DataModels
{
    /// <summary>
    /// 合约预埋单类型的下单视图创建剧本
    /// </summary>
    public class PlaceOrderViewCreateDrama_ContractParked : PlaceOrderViewCreateDramaBase
    {
        public PlaceOrderViewCreateDrama_ContractParked() 
            : base(ClientPlaceOrderType.PARKED, ClientXQOrderTargetType.CONTRACT_TARGET)
        {
        }

        protected override void InitDatas()
        {
            // 设置 order price types
            EnumHelper.GetAllTypesForEnum(typeof(HostingXQOrderPriceType), out IEnumerable<HostingXQOrderPriceType> allOrderPriceTypes);
            this.SupportPriceTypeValues.Clear();
            this.SupportPriceTypeValues.AddRange(allOrderPriceTypes
                .Select(i => new HostingXQOrderPrice
                {
                    PriceType = i,
                    LimitPrice = 0,
                    ChasePriceTicks = 0,
                    ChasePriceValue = 0,
                })
                .ToArray());
            this.SelectedPriceTypeValue = SupportPriceTypeValues.FirstOrDefault();
            
            // 设置有效时间，预埋单不显示有效时间

            // 设置触发条件
            this.OrderTriggerDisplayModel = new XQOrderTriggerDisplayModel(XQOrderTriggerDisplayType.TextDescription)
            {
                TextTriggerDescription = "下次开市触发"
            };

            // 设置可下单时段说明
            this.PlaceOrderAvailableTimeDescription = "非交易时段可下单";
        }

        public override void ValidateAndGenerateOrderDetail(out HostingXQOrderDetail outOrderDetail,
            out HostingXQOrderType? outHostingOrderType,
            out string outOrderValidateErrorMsg,
            int orderQuantity,
            HostingXQTradeDirection orderDirection)
        {
            outOrderDetail = null;
            outHostingOrderType = null;
            outOrderValidateErrorMsg = null;

            if (!XQPlaceOrderDramaValidateHelper.ValidatePlaceOrderDrama_Parked(this, out outOrderValidateErrorMsg))
            {
                return;
            }

            var orderPriceTypeValue = this.SelectedPriceTypeValue;
            var parkedOrder = new HostingXQContractParkedOrderDetail
            {
                Direction = orderDirection,
                Quantity = orderQuantity,
                Price = orderPriceTypeValue
            };
            
            // 深拷贝 detail
            var tmpDetail = new HostingXQOrderDetail { ContractParkedOrderDetail = parkedOrder };
            TBase refDetail = new HostingXQOrderDetail();
            ThriftHelper.UnserializeBytesToTBase(ref refDetail, ThriftHelper.SerializeTBaseToBytes(tmpDetail));

            outOrderDetail = refDetail as HostingXQOrderDetail;
            outHostingOrderType = HostingXQOrderType.XQ_ORDER_CONTRACT_PARKED;
        }
    }
}
