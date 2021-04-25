using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NativeModel.Trade;
using xueqiao.trade.hosting.arbitrage.thriftapi;
using XueQiaoFoundation.Shared.Helper;
using Thrift.Protocol;
using XueQiaoWaf.Trade.Modules.Applications.Helper;

namespace XueQiaoWaf.Trade.Modules.Applications.DataModels
{
    /// <summary>
    /// 合约限价单类型的下单视图创建剧本
    /// </summary>
    public class PlaceOrderViewCreateDrama_ContractLimitPrice : PlaceOrderViewCreateDramaBase
    {
        public PlaceOrderViewCreateDrama_ContractLimitPrice() 
            : base(ClientPlaceOrderType.PRICE_LIMIT, ClientXQOrderTargetType.CONTRACT_TARGET)
        {
        }

        protected override void InitDatas()
        {
            // 设置 order price types
            this.SupportPriceTypeValues.Clear();
            this.SupportPriceTypeValues.AddRange(new HostingXQOrderPrice[]
            {
                new HostingXQOrderPrice
                {
                    PriceType = HostingXQOrderPriceType.ACTION_PRICE_LIMIT,
                    LimitPrice = 0,
                    ChasePriceTicks = 0,
                    ChasePriceValue = 0,
                }
            });
            this.SelectedPriceTypeValue = SupportPriceTypeValues.FirstOrDefault();

            // 设置有效时间
            this.SupportEffectDateTypeValues.Clear();
            this.SupportEffectDateTypeValues.AddRange(new HostingXQEffectDate[]
            {
                new HostingXQEffectDate { Type = HostingXQEffectDateType.XQ_EFFECT_TODAY, StartEffectTimestampS = 0, EndEffectTimestampS = 0 },
                new HostingXQEffectDate { Type = HostingXQEffectDateType.XQ_EFFECT_FOREVER, StartEffectTimestampS = 0, EndEffectTimestampS = 0 },
                new HostingXQEffectDate { Type = HostingXQEffectDateType.XQ_EFFECT_PERIOD, StartEffectTimestampS = 0, EndEffectTimestampS = 0 },
            });
            this.SelectedEffectDateTypeValue = SupportEffectDateTypeValues.FirstOrDefault();


            // 设置触发条件
            this.OrderTriggerDisplayModel = new XQOrderTriggerDisplayModel(XQOrderTriggerDisplayType.TextDescription)
            {
                TextTriggerDescription = "立即触发"
            };

            // 设置可下单时段说明
            this.PlaceOrderAvailableTimeDescription = "开市时段可下单";
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
            
            if (!XQPlaceOrderDramaValidateHelper.ValidatePlaceOrderDrama_LimitPrice(this, out outOrderValidateErrorMsg))
            {
                return;
            }
            
            var orderPriceTypeValue = this.SelectedPriceTypeValue;
            var effectDateTypeValue = this.SelectedEffectDateTypeValue;

            var limitPriceOrder = new HostingXQContractLimitOrderDetail
            {
                Direction = orderDirection,
                Quantity = orderQuantity,
                LimitPrice = orderPriceTypeValue.LimitPrice,
                EffectDate = effectDateTypeValue
            };

            // 深拷贝 detail
            var tmpDetail = new HostingXQOrderDetail { ContractLimitOrderDetail = limitPriceOrder };
            TBase refDetail = new HostingXQOrderDetail();
            ThriftHelper.UnserializeBytesToTBase(ref refDetail, ThriftHelper.SerializeTBaseToBytes(tmpDetail));

            outOrderDetail = refDetail as HostingXQOrderDetail;
            outHostingOrderType = HostingXQOrderType.XQ_ORDER_CONTRACT_LIMIT;
        }
    }
}
