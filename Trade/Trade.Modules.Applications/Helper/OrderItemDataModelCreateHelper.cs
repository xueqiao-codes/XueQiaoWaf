using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.trade.hosting.arbitrage.thriftapi;
using XueQiaoFoundation.BusinessResources.Constants;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.BusinessResources.Helpers;
using XueQiaoFoundation.BusinessResources.Models;
using XueQiaoFoundation.Interfaces.Applications;
using XueQiaoFoundation.Interfaces.Helper;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoWaf.Trade.Interfaces.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Helper
{
    public static class OrderItemDataModelCreateHelper
    {
        /// <summary>
        /// 通过 <see cref="HostingXQOrder"/> 类型数据更新<see cref="OrderItemDataModel"/>类型订单
        /// </summary>
        /// <param name="orderItem"></param>
        /// <param name="hostingXQOrder"></param>
        /// <param name="composeGraphCacheController"></param>
        /// <param name="composeGraphQueryController"></param>
        /// <param name="userComposeViewCacheController"></param>
        /// <param name="commodityCacheController"></param>
        public static void UpdateOrderItemWithHostingOrder(
            OrderItemDataModel orderItem, 
            HostingXQOrder hostingXQOrder,
            IComposeGraphCacheController composeGraphCacheController,
            IComposeGraphQueryController composeGraphQueryController,
            IUserComposeViewCacheController userComposeViewCacheController,
            ICommodityCacheController commodityCacheController)
        {
            if (orderItem == null || hostingXQOrder == null) return;

            OrderUpdateTemplateBase orderUpdateTemplate = null;

            if (OrderItemDataModel_Entrusted.IsOrder_Entrusted(orderItem.OrderType)
                && OrderItemDataModel_Entrusted.IsOrder_Entrusted(hostingXQOrder.OrderType))
            {
                OrderItemDataModel_Entrusted.ParseEntrustedOrderDetail(hostingXQOrder.OrderDetail, hostingXQOrder.OrderType,
                    out HostingXQEffectDate _effectDate,
                    out HostingXQComposeLimitOrderExecParams _composeOrderExecParams);

                var template = new OrderUpdateTemplate_Entrusted();

                template.TradeSummary = new Tuple<HostingXQTradeSummary>(hostingXQOrder.OrderTradeSummary);
                template.EffectDate = new Tuple<HostingXQEffectDate>(_effectDate);
                template.ComposeOrderExecParams = new Tuple<HostingXQComposeLimitOrderExecParams>(_composeOrderExecParams);

                // 设置有效期结束时间
                if (_effectDate != null)
                {
                    if (_effectDate.Type == HostingXQEffectDateType.XQ_EFFECT_PERIOD)
                    {
                        template.OrderEffectEndTimestampMs = new Tuple<long?>(_effectDate.EndEffectTimestampS * 1000);
                    }
                    else if (_effectDate.Type == HostingXQEffectDateType.XQ_EFFECT_TODAY
                        && hostingXQOrder.__isset.gfdOrderEndTimestampMs)
                    {
                        template.OrderEffectEndTimestampMs = new Tuple<long?>(hostingXQOrder.GfdOrderEndTimestampMs);
                    }
                }
                else
                {
                    template.OrderEffectEndTimestampMs = new Tuple<long?>(null);
                }

                orderUpdateTemplate = template;
            }
            else if (OrderItemDataModel_Parked.IsOrder_Parked(orderItem.OrderType)
                && OrderItemDataModel_Parked.IsOrder_Parked(hostingXQOrder.OrderType))
            {
                orderUpdateTemplate = new OrderUpdateTemplate_Parked();
            }
            else if (OrderItemDataModel_Condition.IsOrder_Condition(orderItem.OrderType)
                && OrderItemDataModel_Condition.IsOrder_Condition(hostingXQOrder.OrderType))
            {
                OrderItemDataModel_Condition.ParseConditionOrderDetail(hostingXQOrder.OrderDetail, hostingXQOrder.OrderType,
                                out IEnumerable<HostingXQCondition> _conditions,
                                out HostingXQConditionOrderLabel? _conditionOrderLabel,
                                out HostingXQEffectDate _effectDate);
                
                var template = new OrderUpdateTemplate_Condition();

                template.Conditions = new Tuple<IEnumerable<HostingXQCondition>>(_conditions);
                template.OrderLabel = new Tuple<HostingXQConditionOrderLabel?>(_conditionOrderLabel);
                template.EffectDate = new Tuple<HostingXQEffectDate>(_effectDate);
                
                // 设置有效期结束时间
                if (_effectDate != null)
                {
                    if (_effectDate.Type == HostingXQEffectDateType.XQ_EFFECT_PERIOD)
                    {
                        template.OrderEffectEndTimestampMs = new Tuple<long?>(_effectDate.EndEffectTimestampS * 1000);
                    }
                    else if (_effectDate.Type == HostingXQEffectDateType.XQ_EFFECT_TODAY
                        && hostingXQOrder.__isset.gfdOrderEndTimestampMs)
                    {
                        template.OrderEffectEndTimestampMs = new Tuple<long?>(hostingXQOrder.GfdOrderEndTimestampMs);
                    }
                }
                else
                {
                    template.OrderEffectEndTimestampMs = new Tuple<long?>(null);
                }

                orderUpdateTemplate = template;
            }


            if (orderUpdateTemplate != null)
            {
                if (hostingXQOrder.__isset.orderState)
                {
                    var orderState = hostingXQOrder.OrderState;
                    orderUpdateTemplate.OrderState = new Tuple<ClientXQOrderState>(orderState.StateValue.ToClientXQOrderState());
                    orderUpdateTemplate.OrderStateDetail = new Tuple<HostingXQOrderState>(orderState);
                    orderUpdateTemplate.UpdateTimestamMs = new Tuple<long>(orderState.StateTimestampMs);
                }
                if (hostingXQOrder.__isset.version)
                    orderUpdateTemplate.Version = new Tuple<int>(hostingXQOrder.Version);
                if (hostingXQOrder.__isset.createTimestampMs)
                    orderUpdateTemplate.CreateTimestampMs = new Tuple<long>(hostingXQOrder.CreateTimestampMs);
                if (hostingXQOrder.__isset.sourceOrderId)
                    orderUpdateTemplate.SourceOrderId = new Tuple<string>(hostingXQOrder.SourceOrderId);
                if (hostingXQOrder.__isset.actionOrderId)
                    orderUpdateTemplate.ActionOrderId = new Tuple<string>(hostingXQOrder.ActionOrderId);

                if (OrderItemDataModel_Entrusted.IsOrder_Entrusted(orderItem.OrderType)
                    && orderUpdateTemplate is OrderUpdateTemplate_Entrusted entrustedOUT)
                {
                    UpdateEntrustedOrderWithTemplate(orderItem as OrderItemDataModel_Entrusted,
                        entrustedOUT,
                        composeGraphCacheController, composeGraphQueryController,
                        userComposeViewCacheController, commodityCacheController);
                }
                else if (OrderItemDataModel_Parked.IsOrder_Parked(orderItem.OrderType)
                    && orderUpdateTemplate is OrderUpdateTemplate_Parked parkedOUT)
                {
                    UpdateParkedOrderWithTemplate(orderItem as OrderItemDataModel_Parked,
                        parkedOUT);
                }
                else if (OrderItemDataModel_Condition.IsOrder_Condition(orderItem.OrderType)
                    && orderUpdateTemplate is OrderUpdateTemplate_Condition conditionOUT)
                {
                    UpdateConditionOrderWithTemplate(orderItem as OrderItemDataModel_Condition,
                        conditionOUT);
                }
            }
        }
        
        public static OrderItemDataModel CreateOrderItem(
            this HostingXQOrder hostingXQOrder,
            IUserSubAccountRelatedItemQueryController subAccountRelatedItemQueryController,
            IUserSubAccountRelatedItemCacheController subAccountRelatedItemCacheController,
            IHostingUserQueryController hostingUserQueryController,
            IHostingUserCacheController hostingUserCacheController,
            IComposeGraphCacheController composeGraphCacheController,
            IComposeGraphQueryController composeGraphQueryController,
            IUserComposeViewCacheController userComposeViewCacheController,
            IUserComposeViewQueryController userComposeViewQueryController,
            IContractItemTreeQueryController contractItemTreeQueryController,
            ICommodityCacheController commodityCacheController)
        {
            if (hostingXQOrder == null) return null;

            var orderType = hostingXQOrder.OrderType;
            var orderCreateUserId = hostingXQOrder.SubUserId;
            var orderDetail = hostingXQOrder.OrderDetail;

            var orderKey = new DiscreteOrderItemKey(hostingXQOrder.SubAccountId, hostingXQOrder.OrderId, hostingXQOrder.OrderType,
                        hostingXQOrder.OrderTarget.TargetType.ToClientXQOrderTargetType(), hostingXQOrder.OrderTarget.TargetKey);

            OrderItemDataModel newOrderItem = null;
            if (OrderItemDataModel_Entrusted.IsOrder_Entrusted(orderType))
            {
                var entrustedOrder = GenerateOrder_Entrusted(orderKey, orderCreateUserId, orderDetail, 
                    subAccountRelatedItemQueryController, subAccountRelatedItemCacheController,
                    hostingUserQueryController, hostingUserCacheController,
                    composeGraphCacheController, composeGraphQueryController,
                    userComposeViewCacheController, userComposeViewQueryController, 
                    contractItemTreeQueryController);
                newOrderItem = entrustedOrder;
            }
            else if (OrderItemDataModel_Parked.IsOrder_Parked(orderType))
            {
                var parkedOrder = GenerateOrder_Parked(orderKey, orderCreateUserId, orderDetail,
                    subAccountRelatedItemQueryController, subAccountRelatedItemCacheController,
                    hostingUserQueryController, hostingUserCacheController,
                    contractItemTreeQueryController);
                newOrderItem = parkedOrder;
            }
            else if (OrderItemDataModel_Condition.IsOrder_Condition(orderType))
            {
                var conditionOrder = GenerateOrder_Condition(orderKey, orderCreateUserId, orderDetail,
                    subAccountRelatedItemQueryController, subAccountRelatedItemCacheController,
                    hostingUserQueryController, hostingUserCacheController,
                    composeGraphCacheController, composeGraphQueryController,
                    userComposeViewCacheController, userComposeViewQueryController,
                    contractItemTreeQueryController);
                newOrderItem = conditionOrder;
            }

            if (newOrderItem != null)
            {
                UpdateOrderItemWithHostingOrder(newOrderItem, hostingXQOrder,
                    composeGraphCacheController, composeGraphQueryController,
                    userComposeViewCacheController, commodityCacheController);
            }

            return newOrderItem;
        }

        public static OrderItemDataModel_Entrusted GenerateOrder_Entrusted(DiscreteOrderItemKey orderKey,
            int createUserId, HostingXQOrderDetail orderDetailFiller,
            IUserSubAccountRelatedItemQueryController subAccountRelatedItemQueryController,
            IUserSubAccountRelatedItemCacheController subAccountRelatedItemCacheController,
            IHostingUserQueryController hostingUserQueryController,
            IHostingUserCacheController hostingUserCacheController,
            IComposeGraphCacheController composeGraphCacheController,
            IComposeGraphQueryController composeGraphQueryController,
            IUserComposeViewCacheController userComposeViewCacheController,
            IUserComposeViewQueryController userComposeViewQueryController,
            IContractItemTreeQueryController contractItemTreeQueryController)
        {
            if (orderKey == null) return null;
            if (!OrderItemDataModel_Entrusted.IsOrder_Entrusted(orderKey.OrderType)) return null;

            var subAccountFields = new SubAccountFieldsForTradeData(createUserId, orderKey.SubAccountId);
            TradeDMLoadHelper.SetupSubAccountFields(subAccountFields,
                subAccountRelatedItemQueryController, subAccountRelatedItemCacheController,
                hostingUserQueryController, hostingUserCacheController);

            var entrustedOrder = new OrderItemDataModel_Entrusted(orderKey.TargetType,
                    orderKey.OrderType, subAccountFields, orderKey.OrderId, orderKey.TargetKey);

            OrderItemDataModel_Entrusted.ParseEntrustedOrderDetail(orderDetailFiller, orderKey.OrderType,
                out HostingXQEffectDate _effectDate,
                out HostingXQComposeLimitOrderExecParams _composeOrderExecParams);
            entrustedOrder.EffectDate = _effectDate;
            entrustedOrder.ComposeOrderExecParams = _composeOrderExecParams;
            entrustedOrder.OrderEffectEndTimestampMs = XueQiaoBusinessHelper.GetEffectDateEndTimestampMsDependOnType(_effectDate);

            if (orderKey.OrderType == HostingXQOrderType.XQ_ORDER_CONTRACT_LIMIT)
            {
                var target_ContractId = Convert.ToInt32(orderKey.TargetKey);

                var contractLimitOrderDetail = orderDetailFiller?.ContractLimitOrderDetail;
                if (contractLimitOrderDetail != null)
                {
                    entrustedOrder.Direction = contractLimitOrderDetail.Direction.ToClientTradeDirection();
                    entrustedOrder.Quantity = contractLimitOrderDetail.Quantity;
                    entrustedOrder.Price = contractLimitOrderDetail.LimitPrice;
                }

                entrustedOrder.TargetContractDetailContainer = new TargetContract_TargetContractDetail(target_ContractId);
                XueQiaoFoundationHelper.SetupTargetContract_ContractDetail(entrustedOrder.TargetContractDetailContainer, contractItemTreeQueryController,
                    XqContractNameFormatType.CommodityAcronym_Code_ContractCode,
                    _container =>
                    {
                        XqTargetDMHelper.InvalidateTargetNameWithAppropriate(entrustedOrder, XqAppLanguages.CN);
                        RectifyEntrustedOrderRelatedPriceProps(entrustedOrder);
                    });
            }
            else if (orderKey.OrderType == HostingXQOrderType.XQ_ORDER_COMPOSE_LIMIT)
            {
                var target_ComposeId = Convert.ToInt64(orderKey.TargetKey);

                var composeLimitOrderDetail = orderDetailFiller?.ComposeLimitOrderDetail;
                if (composeLimitOrderDetail != null)
                {
                    entrustedOrder.Direction = composeLimitOrderDetail.Direction.ToClientTradeDirection();
                    entrustedOrder.Quantity = composeLimitOrderDetail.Quantity;
                    entrustedOrder.Price = composeLimitOrderDetail.LimitPrice;
                }

                entrustedOrder.TargetComposeDetailContainer = new TargetCompose_ComposeDetail(target_ComposeId);
                XueQiaoFoundationHelper.SetupTargetCompose_ComposeDetail(entrustedOrder.TargetComposeDetailContainer,
                    composeGraphCacheController, composeGraphQueryController,
                    userComposeViewCacheController, contractItemTreeQueryController,
                    XqContractNameFormatType.CommodityAcronym_Code_ContractCode,
                    _container =>
                    {
                        XqTargetDMHelper.InvalidateTargetNameWithAppropriate(entrustedOrder, XqAppLanguages.CN);
                    });

                entrustedOrder.TargetComposeUserComposeViewContainer = new UserComposeViewContainer(target_ComposeId);
                XueQiaoFoundationHelper.SetupUserComposeView(entrustedOrder.TargetComposeUserComposeViewContainer,
                    userComposeViewCacheController, userComposeViewQueryController, false, true, 
                    _container => 
                    {
                        XqTargetDMHelper.InvalidateTargetNameWithAppropriate(entrustedOrder, XqAppLanguages.CN);
                        RectifyEntrustedOrderRelatedPriceProps(entrustedOrder);
                    });
            }

            return entrustedOrder;
        }
        
        public static OrderItemDataModel_Parked GenerateOrder_Parked(DiscreteOrderItemKey orderKey,
            int createUserId, HostingXQOrderDetail orderDetailFiller,
            IUserSubAccountRelatedItemQueryController subAccountRelatedItemQueryController,
            IUserSubAccountRelatedItemCacheController subAccountRelatedItemCacheController,
            IHostingUserQueryController hostingUserQueryController,
            IHostingUserCacheController hostingUserCacheController,
            IContractItemTreeQueryController contractItemTreeQueryController)
        {
            if (orderKey == null) return null;
            if (!OrderItemDataModel_Parked.IsOrder_Parked(orderKey.OrderType)) return null;

            var subAccountFields = new SubAccountFieldsForTradeData(createUserId, orderKey.SubAccountId);
            TradeDMLoadHelper.SetupSubAccountFields(subAccountFields,
                subAccountRelatedItemQueryController, subAccountRelatedItemCacheController,
                hostingUserQueryController, hostingUserCacheController);

            var parkedOrder = new OrderItemDataModel_Parked(orderKey.TargetType,
                    orderKey.OrderType, subAccountFields, orderKey.OrderId, orderKey.TargetKey);

            if (orderKey.OrderType == HostingXQOrderType.XQ_ORDER_CONTRACT_PARKED)
            {
                int target_ContractId = Convert.ToInt32(orderKey.TargetKey);

                var parkedOrderDetail = orderDetailFiller.ContractParkedOrderDetail;
                if (parkedOrderDetail != null)
                {
                    parkedOrder.Direction = parkedOrderDetail.Direction.ToClientTradeDirection();
                    parkedOrder.Quantity = parkedOrderDetail.Quantity;
                    parkedOrder.TriggerOrderPrice = parkedOrderDetail.Price;
                }

                parkedOrder.TargetContractDetailContainer = new TargetContract_TargetContractDetail(target_ContractId);
                XueQiaoFoundationHelper.SetupTargetContract_ContractDetail(parkedOrder.TargetContractDetailContainer, contractItemTreeQueryController, XqContractNameFormatType.CommodityAcronym_Code_ContractCode,
                    _container =>
                    {
                        XqTargetDMHelper.InvalidateTargetNameWithAppropriate(parkedOrder, XqAppLanguages.CN);
                        RectifyParkedOrderRelatedPriceProps(parkedOrder);
                    });
            }

            return parkedOrder;
        }

        public static OrderItemDataModel_Condition GenerateOrder_Condition(DiscreteOrderItemKey orderKey,
            int createUserId, HostingXQOrderDetail orderDetailFiller,
            IUserSubAccountRelatedItemQueryController subAccountRelatedItemQueryController,
            IUserSubAccountRelatedItemCacheController subAccountRelatedItemCacheController,
            IHostingUserQueryController hostingUserQueryController,
            IHostingUserCacheController hostingUserCacheController,
            IComposeGraphCacheController composeGraphCacheController,
            IComposeGraphQueryController composeGraphQueryController,
            IUserComposeViewCacheController userComposeViewCacheController,
            IUserComposeViewQueryController userComposeViewQueryController,
            IContractItemTreeQueryController contractItemTreeQueryController)
        {
            if (orderKey == null) return null;
            if (!OrderItemDataModel_Condition.IsOrder_Condition(orderKey.OrderType)) return null;

            var subAccountFields = new SubAccountFieldsForTradeData(createUserId, orderKey.SubAccountId);
            TradeDMLoadHelper.SetupSubAccountFields(subAccountFields,
                subAccountRelatedItemQueryController, subAccountRelatedItemCacheController,
                hostingUserQueryController, hostingUserCacheController);

            var conditionOrder = new OrderItemDataModel_Condition(orderKey.TargetType, orderKey.OrderType, subAccountFields, orderKey.OrderId, orderKey.TargetKey);

            OrderItemDataModel_Condition.ParseConditionOrderDetail(orderDetailFiller, orderKey.OrderType,
                                out IEnumerable<HostingXQCondition> _conditions,
                                out HostingXQConditionOrderLabel? _conditionOrderLabel,
                                out HostingXQEffectDate _effectDate);
            if (_conditions != null)
            {
                conditionOrder.Conditions = new ObservableCollection<HostingXQCondition>(_conditions);
            }
            conditionOrder.OrderLabel = _conditionOrderLabel;
            conditionOrder.EffectDate = _effectDate;
            conditionOrder.OrderEffectEndTimestampMs = XueQiaoBusinessHelper.GetEffectDateEndTimestampMsDependOnType(_effectDate);

            if (orderKey.TargetType == ClientXQOrderTargetType.CONTRACT_TARGET)
            {
                int target_ContractId = Convert.ToInt32(orderKey.TargetKey);
                conditionOrder.TargetContractDetailContainer = new TargetContract_TargetContractDetail(target_ContractId);
                XueQiaoFoundationHelper.SetupTargetContract_ContractDetail(conditionOrder.TargetContractDetailContainer, 
                    contractItemTreeQueryController, XqContractNameFormatType.CommodityAcronym_Code_ContractCode,
                    _container =>
                    {
                        XqTargetDMHelper.InvalidateTargetNameWithAppropriate(conditionOrder, XqAppLanguages.CN);
                    });
            }
            else if (orderKey.TargetType == ClientXQOrderTargetType.COMPOSE_TARGET)
            {
                long target_ComposeId = Convert.ToInt64(orderKey.TargetKey);
                conditionOrder.TargetComposeDetailContainer = new TargetCompose_ComposeDetail(target_ComposeId);
                XueQiaoFoundationHelper.SetupTargetCompose_ComposeDetail(conditionOrder.TargetComposeDetailContainer,
                    composeGraphCacheController, composeGraphQueryController,
                    userComposeViewCacheController, contractItemTreeQueryController,
                    XqContractNameFormatType.CommodityAcronym_Code_ContractCode,
                    _container =>
                    {
                        XqTargetDMHelper.InvalidateTargetNameWithAppropriate(conditionOrder, XqAppLanguages.CN);
                    });

                conditionOrder.TargetComposeUserComposeViewContainer = new UserComposeViewContainer(target_ComposeId);
                XueQiaoFoundationHelper.SetupUserComposeView(conditionOrder.TargetComposeUserComposeViewContainer,
                    userComposeViewCacheController, userComposeViewQueryController, false, true,
                    _container =>
                    {
                        XqTargetDMHelper.InvalidateTargetNameWithAppropriate(conditionOrder, XqAppLanguages.CN);
                    });
            }

            return conditionOrder;
        }

        public static void UpdateEntrustedOrderWithTemplate(OrderItemDataModel_Entrusted entrustedOrder, 
            OrderUpdateTemplate_Entrusted updateTemplate,
            IComposeGraphCacheController composeGraphCacheController,
            IComposeGraphQueryController composeGraphQueryController,
            IUserComposeViewCacheController userComposeViewCacheController,
            ICommodityCacheController commodityCacheController)
        {
            UpdateOrderWithTemplateBase(entrustedOrder, updateTemplate);
            if (entrustedOrder == null || updateTemplate == null) return;

            var rectifyReletedPriceProps = false;

            if (updateTemplate.TradeSummary != null)
            {
                var tradeSummary = updateTemplate.TradeSummary.Item1;
                if (tradeSummary == null)
                {
                    entrustedOrder.TradeVolume = 0;
                    entrustedOrder.TradeAvgPrice = 0;
                    entrustedOrder.TargetComposeLegTradeSummarysContainer = null;
                }
                else
                {
                    rectifyReletedPriceProps = true;
                    
                    // 设置 TradeAvgPrice
                    entrustedOrder.TradeAvgPrice = tradeSummary.AveragePrice;

                    entrustedOrder.TradeVolume = tradeSummary.TotalVolume;
                    
                    // 设置 TargetComposeOrderLegTradeSummarys
                    if (entrustedOrder.TargetType == ClientXQOrderTargetType.COMPOSE_TARGET)
                    {
                        if (tradeSummary.SubTradeSummaries?.Any() != true)
                        {
                            entrustedOrder.TargetComposeLegTradeSummarysContainer = null;
                        }
                        else
                        {
                            var MakeLegSummaryPricePrecise = new Action<TargetComposeLegTradeSummary>(_legSummary =>
                            {
                                if (_legSummary == null) return;
                                var commodityId = _legSummary.LegMeta?.SledCommodityId;
                                if (commodityId == null) return;

                                var originPrice = _legSummary.SummaryPrice;
                                if (originPrice != null)
                                {
                                    var pricePreciseTick = commodityCacheController.GetCache((int)commodityId.Value)?.TickSize;
                                    if (pricePreciseTick != null)
                                        pricePreciseTick *= XueQiaoConstants.XQMultipleCalculatedPriceTickRate;

                                    _legSummary.SummaryPrice = XueQiaoBusinessHelper.MakePreciseXQContractRelatedPrice(originPrice.Value, pricePreciseTick);
                                }
                            });

                            long composeId = Convert.ToInt64(entrustedOrder.TargetKey);
                            var metaLegTradeSummarys = tradeSummary.SubTradeSummaries.Values.ToArray();
                            var legTradeSummarys = TradeDMLoadHelper.CreateTargetComposeLegTradeSummarys(
                                    metaLegTradeSummarys.Select(i => Convert.ToInt32(i.SubTarget.TargetKey)).ToArray(),
                                    composeId, _legContractId => LegTradeSummaryPriceType.TradeAvgPrice,
                                    composeGraphCacheController, composeGraphQueryController, userComposeViewCacheController,
                                    _summs =>
                                    {
                                        if (_summs == null) return;
                                        foreach (var _summ in _summs)
                                            MakeLegSummaryPricePrecise(_summ);
                                    });

                            foreach (var legTradeSummary in legTradeSummarys)
                            {
                                var metaLegTradeSummary = metaLegTradeSummarys?.FirstOrDefault(i => i.SubTarget.TargetKey == $"{legTradeSummary.LegContractId}");
                                if (metaLegTradeSummary != null)
                                {
                                    legTradeSummary.TradeVolume = metaLegTradeSummary.SubTradeVolume;
                                    legTradeSummary.SummaryPrice = metaLegTradeSummary.SubTradeAveragePrice;
                                    MakeLegSummaryPricePrecise(legTradeSummary);
                                }
                            }

                            entrustedOrder.TargetComposeLegTradeSummarysContainer = new TargetComposeLegTradeSummarysContainer(composeId)
                            {
                                LegTradeSummarys = new ObservableCollection<TargetComposeLegTradeSummary>(legTradeSummarys)
                            };
                        }
                    }
                }
            }

            if (updateTemplate.EffectDate != null)
            {
                entrustedOrder.EffectDate = updateTemplate.EffectDate.Item1;
            }

            if (updateTemplate.ComposeOrderExecParams != null)
            {
                entrustedOrder.ComposeOrderExecParams = updateTemplate.ComposeOrderExecParams.Item1;
            }

            if (rectifyReletedPriceProps)
            {
                RectifyEntrustedOrderRelatedPriceProps(entrustedOrder);
            }
        }

        public static void UpdateParkedOrderWithTemplate(OrderItemDataModel_Parked parkedOrder, OrderUpdateTemplate_Parked updateTemplate)
        {
            UpdateOrderWithTemplateBase(parkedOrder, updateTemplate);
        }

        public static void UpdateConditionOrderWithTemplate(OrderItemDataModel_Condition conditionOrder, OrderUpdateTemplate_Condition updateTemplate)
        {
            UpdateOrderWithTemplateBase(conditionOrder, updateTemplate);

            if (conditionOrder == null || updateTemplate == null) return;
            if (updateTemplate.EffectDate != null)
            {
                conditionOrder.EffectDate = updateTemplate.EffectDate.Item1;
            }
        }

        public static void UpdateOrderWithTemplateBase(OrderItemDataModel orderItem, OrderUpdateTemplateBase orderUpdateTemplate)
        {
            if (orderItem == null || orderUpdateTemplate == null) return;
            if (orderUpdateTemplate.OrderState != null)
            {
                orderItem.OrderState = orderUpdateTemplate.OrderState.Item1;
            }

            if (orderUpdateTemplate.OrderStateDetail != null)
            {
                orderItem.OrderStateDetail = orderUpdateTemplate.OrderStateDetail.Item1;
            }

            if (orderUpdateTemplate.Version != null)
            {
                orderItem.Version = orderUpdateTemplate.Version.Item1;
            }

            if (orderUpdateTemplate.CreateTimestampMs != null)
            {
                orderItem.OrderTimestampMs = orderUpdateTemplate.CreateTimestampMs.Item1;
            }

            if (orderUpdateTemplate.UpdateTimestamMs != null)
            {
                orderItem.UpdateTimestampMs = orderUpdateTemplate.UpdateTimestamMs.Item1;
            }

            if (orderUpdateTemplate.SourceOrderId != null)
            {
                orderItem.SourceOrderId = orderUpdateTemplate.SourceOrderId.Item1;
            }

            if (orderUpdateTemplate.ActionOrderId != null)
            {
                orderItem.ActionOrderId = orderUpdateTemplate.ActionOrderId.Item1;
            }

            if (orderUpdateTemplate.OrderEffectEndTimestampMs != null)
            {
                orderItem.OrderEffectEndTimestampMs = orderUpdateTemplate.OrderEffectEndTimestampMs.Item1;
            }
        }

        /// <summary>
        /// 纠正订单的相关价格属性数据，使其更精确
        /// </summary>
        /// <param name="entrustedOrder">委托单</param>
        public static void RectifyEntrustedOrderRelatedPriceProps(OrderItemDataModel_Entrusted entrustedOrder)
        {
            if (entrustedOrder == null) return;

            var tickSize = XueQiaoConstants.XQContractPriceMinimumPirceTick;
            if (entrustedOrder.TargetType == ClientXQOrderTargetType.CONTRACT_TARGET)
            {
                var commodityTickSize = entrustedOrder.TargetContractDetailContainer?.CommodityDetail?.TickSize;
                if (commodityTickSize != null)
                {
                    tickSize = commodityTickSize.Value;
                }
            }
            else if (entrustedOrder.TargetType == ClientXQOrderTargetType.COMPOSE_TARGET)
            {
                tickSize = XueQiaoBusinessHelper.CalculteXqTargetPriceTick(entrustedOrder.TargetComposeUserComposeViewContainer?.UserComposeView?.PrecisionNumber,
                    XueQiaoConstants.XQComposePriceMaximumPirceTick);
            }

            // TradeAvgPrice
            entrustedOrder.TradeAvgPrice = MathHelper.MakeValuePrecise(entrustedOrder.TradeAvgPrice,
                        tickSize * XueQiaoConstants.XQMultipleCalculatedPriceTickRate);

            // Price
            entrustedOrder.Price = MathHelper.MakeValuePrecise(entrustedOrder.Price, tickSize);
        }

        /// <summary>
        /// 纠正订单的相关价格属性数据，使其更精确
        /// </summary>
        /// <param name="parkedOrder">预埋单</param>
        public static void RectifyParkedOrderRelatedPriceProps(OrderItemDataModel_Parked parkedOrder)
        {
            if (parkedOrder == null) return;

            var tickSize = XueQiaoConstants.XQContractPriceMinimumPirceTick;
            if (parkedOrder.TargetType == ClientXQOrderTargetType.CONTRACT_TARGET)
            {
                var commodityTickSize = parkedOrder.TargetContractDetailContainer?.CommodityDetail?.TickSize;
                if (commodityTickSize != null)
                {
                    tickSize = commodityTickSize.Value;
                }
            }
            else if (parkedOrder.TargetType == ClientXQOrderTargetType.COMPOSE_TARGET)
            {
                tickSize = XueQiaoBusinessHelper.CalculteXqTargetPriceTick(parkedOrder.TargetComposeUserComposeViewContainer?.UserComposeView?.PrecisionNumber,
                    XueQiaoConstants.XQComposePriceMaximumPirceTick);
            }

            // TriggerOrderPrice
            var triggerOrderPrice = parkedOrder.TriggerOrderPrice;
            if (triggerOrderPrice != null)
            {
                if (triggerOrderPrice.__isset.limitPrice)
                {
                    triggerOrderPrice.LimitPrice = MathHelper.MakeValuePrecise(triggerOrderPrice.LimitPrice, tickSize);
                }
            }
        }
    }
}
