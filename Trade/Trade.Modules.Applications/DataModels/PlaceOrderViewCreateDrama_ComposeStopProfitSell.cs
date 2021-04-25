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
    /// 组合卖出止盈单类型的下单视图创建剧本
    /// </summary>
    public class PlaceOrderViewCreateDrama_ComposeStopProfitSell : PlaceOrderViewCreateDramaXQComposeBase
    {
        public PlaceOrderViewCreateDrama_ComposeStopProfitSell() : base(ClientPlaceOrderType.SELL_STOP_PROFIT)
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

            // 设置有效时间
            var supportDateTypes = new HostingXQEffectDateType[] { HostingXQEffectDateType.XQ_EFFECT_FOREVER, HostingXQEffectDateType.XQ_EFFECT_PERIOD };
            this.SupportEffectDateTypeValues.Clear();
            this.SupportEffectDateTypeValues.AddRange(supportDateTypes
                            .Select(i => new HostingXQEffectDate { Type = i, StartEffectTimestampS = 0, EndEffectTimestampS = 0 })
                            .ToArray());
            this.SelectedEffectDateTypeValue = this.SupportEffectDateTypeValues.First();

            // 设置触发条件
            this.OrderTriggerDisplayModel = new XQOrderTriggerDisplayModel(XQOrderTriggerDisplayType.TriggerNeedConfig);
            this.OrderTriggerDisplayModel.ConfigTrigger.TriggerOperator = HostingXQConditionTriggerOperator.XQ_OP_GE;

            // 设置可下单时段说明
            this.PlaceOrderAvailableTimeDescription = "任意时段都可下单";

            // 设置买卖按钮区域显隐
            this.ShowBuyButtonAreaView = false;
            this.ShowSellButtonAreaView = true;
        }

        public override void ValidateAndGenerateOrderDetail(out HostingXQOrderDetail outOrderDetail,
            out HostingXQOrderType? outHostingOrderType,
            out string outOrderValidateErrorMsg,
            int orderQuantity,
            HostingXQTradeDirection orderDirection)
        {
            if (orderDirection != HostingXQTradeDirection.XQ_SELL)
            {
                throw new ArgumentException($"orderDirection must be `{HostingXQTradeDirection.XQ_SELL}`");
            }

            outOrderDetail = null;
            outHostingOrderType = null;
            outOrderValidateErrorMsg = null;


            if (!XQPlaceOrderDramaValidateHelper.ValidatePlaceOrderDrama_Condition(this, out outOrderValidateErrorMsg))
            {
                return;
            }

            HostingXQComposeLimitOrderExecParams orderExecParams = null;
            ValidateAndGenerateOrderExecParams(out orderExecParams, out string _validateExecParamsErrMsg);
            if (_validateExecParamsErrMsg != null)
            {
                outOrderValidateErrorMsg = _validateExecParamsErrMsg;
                return;
            }

            var effectDateTypeValue = this.SelectedEffectDateTypeValue;
            var triggerOrderPriceTypeValue = this.SelectedPriceTypeValue;

            var configedTrigger = this.OrderTriggerDisplayModel.ConfigTrigger;
            var condOrder = new HostingXQConditionOrderDetail();
            condOrder.EffectDate = effectDateTypeValue;

            var condition = new HostingXQCondition();
            condition.ConditionTrigger = configedTrigger;
            
            condition.ConditionAction = new HostingXQConditionAction
            {
                OrderType = HostingXQOrderType.XQ_ORDER_COMPOSE_LIMIT,
                TradeDirection = HostingXQTradeDirection.XQ_SELL,
                Quantity = orderQuantity,
                Price = triggerOrderPriceTypeValue,
                Extra = new HostingXQConditionActionExtra { ComposeLimitExecParams = orderExecParams }
            };
            condOrder.Conditions = new List<HostingXQCondition> { condition };

            condOrder.Label = HostingXQConditionOrderLabel.LABEL_STOP_PROFIT_SELL;

            // 深拷贝 detail
            var tmpDetail = new HostingXQOrderDetail { ConditionOrderDetail = condOrder };
            TBase refDetail = new HostingXQOrderDetail();
            ThriftHelper.UnserializeBytesToTBase(ref refDetail, ThriftHelper.SerializeTBaseToBytes(tmpDetail));

            outOrderDetail = refDetail as HostingXQOrderDetail;
            outHostingOrderType = HostingXQOrderType.XQ_ORDER_CONDITION;
        }
    }
}
