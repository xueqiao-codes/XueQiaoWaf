using NativeModel.Trade;
using System;
using System.Collections.Generic;
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

namespace XueQiaoWaf.Trade.Modules.Applications.Helper
{
    public static class TradeItemDataModelCreateHelper
    {
        public static TradeItemDataModel CreateTradeItem(
            this HostingXQTrade hostingXQTrade,
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
            if (hostingXQTrade == null) return null;

            var subUserId = hostingXQTrade.SubUserId;
            var subAccountId = hostingXQTrade.SubAccountId;
            var orderId = hostingXQTrade.OrderId;
            var tradeId = hostingXQTrade.TradeId;
            var tradeTargetKey = hostingXQTrade.TradeTarget.TargetKey;
            var targetType = hostingXQTrade.TradeTarget.TargetType;
            
            var subAccountFields = new SubAccountFieldsForTradeData(subUserId, subAccountId);
            TradeDMLoadHelper.SetupSubAccountFields(subAccountFields,
                subAccountRelatedItemQueryController, subAccountRelatedItemCacheController,
                hostingUserQueryController, hostingUserCacheController);

            var newItem = new TradeItemDataModel(targetType.ToClientXQOrderTargetType(), tradeTargetKey, subAccountFields, tradeId, orderId)
            {
                Direction = hostingXQTrade.TradeDiretion.ToClientTradeDirection(),
                TradePrice = hostingXQTrade.TradePrice,
                TradeVolume = hostingXQTrade.TradeVolume,
                SourceOrderTarget = hostingXQTrade.SourceOrderTarget,
                CreateTimestampMs = hostingXQTrade.CreateTimestampMs,
            };

            if (targetType == HostingXQTargetType.CONTRACT_TARGET)
            {
                // 合约
                int contractId = Convert.ToInt32(tradeTargetKey);
                newItem.TargetContractDetailContainer = new TargetContract_TargetContractDetail(contractId);
                XueQiaoFoundationHelper.SetupTargetContract_ContractDetail(newItem.TargetContractDetailContainer, contractItemTreeQueryController,
                    XqContractNameFormatType.CommodityAcronym_Code_ContractCode,
                    _detail => 
                    {
                        XqTargetDMHelper.InvalidateTargetNameWithAppropriate(newItem, XqAppLanguages.CN);
                        RectifyTradeItemRelatedPriceProps(newItem);
                    });
            }
            else if (targetType == HostingXQTargetType.COMPOSE_TARGET)
            {
                // 组合
                long target_ComposeId = Convert.ToInt64(tradeTargetKey);
                newItem.TargetComposeDetailContainer = new TargetCompose_ComposeDetail(target_ComposeId);
                XueQiaoFoundationHelper.SetupTargetCompose_ComposeDetail(newItem.TargetComposeDetailContainer,
                    composeGraphCacheController, composeGraphQueryController,
                    userComposeViewCacheController, contractItemTreeQueryController,
                    XqContractNameFormatType.CommodityAcronym_Code_ContractCode,
                    _detail => 
                    {
                        XqTargetDMHelper.InvalidateTargetNameWithAppropriate(newItem, XqAppLanguages.CN);
                    });

                newItem.TargetComposeUserComposeViewContainer = new UserComposeViewContainer(target_ComposeId);
                XueQiaoFoundationHelper.SetupUserComposeView(newItem.TargetComposeUserComposeViewContainer,
                    userComposeViewCacheController, userComposeViewQueryController, false, true, 
                    _detail => 
                    {
                        XqTargetDMHelper.InvalidateTargetNameWithAppropriate(newItem, XqAppLanguages.CN);
                        RectifyTradeItemRelatedPriceProps(newItem);
                    });
            }

            RectifyTradeItemRelatedPriceProps(newItem);
            return newItem;
        }


        /// <summary>
        /// 纠正成交项的相关价格属性数据，使其更精确
        /// </summary>
        /// <param name="tradeItem">成交项</param>
        public static void RectifyTradeItemRelatedPriceProps(TradeItemDataModel tradeItem)
        {
            if (tradeItem == null) return;

            var tickSize = XueQiaoConstants.XQContractPriceMinimumPirceTick;
            if (tradeItem.TargetType == ClientXQOrderTargetType.CONTRACT_TARGET)
            {
                var commodityTickSize = tradeItem.TargetContractDetailContainer?.CommodityDetail?.TickSize;
                if (commodityTickSize != null)
                {
                    tickSize = commodityTickSize.Value;
                }
            }
            else if (tradeItem.TargetType == ClientXQOrderTargetType.COMPOSE_TARGET)
            {
                tickSize = XueQiaoBusinessHelper.CalculteXqTargetPriceTick(tradeItem.TargetComposeUserComposeViewContainer?.UserComposeView?.PrecisionNumber,
                    XueQiaoConstants.XQComposePriceMaximumPirceTick);
            }

            // TradePrice
            tradeItem.TradePrice = MathHelper.MakeValuePrecise(tradeItem.TradePrice, tickSize);
        }
    }
}
