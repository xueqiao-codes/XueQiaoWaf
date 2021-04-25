using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.trade.hosting.asset.thriftapi;
using XueQiaoFoundation.BusinessResources.Constants;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.Shared.Helper;

namespace XueQiaoFoundation.BusinessResources.Helpers
{
    public static class DiscretePosition_ModelHelper
    {
        /// <summary>
        /// 修改散列持仓 data model
        /// </summary>
        /// <param name="positionDM">目标项</param>
        /// <param name="srcPosition">修改数据源</param>
        public static void UpdateDiscretePositionDM(DiscretePositionDM positionDM,
            HostingSledContractPosition srcPosition)
        {
            if (positionDM == null || srcPosition == null) return;
            var positionVolume = srcPosition.PositionVolume;
            var positionFund = srcPosition.PositionFund;

            var template = new DiscretePositionDM((int)srcPosition.SledContractId);
            if (srcPosition.__isset.currency)
                template.Currency = srcPosition.Currency;

            if (positionVolume != null)
            {
                if (positionVolume.__isset.prevPosition)
                    template.PrevPosition = positionVolume.PrevPosition;
                if (positionVolume.__isset.longPosition)
                    template.LongPosition = positionVolume.LongPosition;
                if (positionVolume.__isset.shortPosition)
                    template.ShortPosition = positionVolume.ShortPosition;
                if (positionVolume.__isset.netPosition)
                    template.NetPosition = positionVolume.NetPosition;
                if (positionVolume.__isset.useCommission)
                    template.UseCommission = positionVolume.UseCommission;
                if (positionVolume.__isset.closeProfit)
                    template.CloseProfit = positionVolume.CloseProfit;
                if (positionVolume.__isset.positionAvgPrice)
                    template.PositionAvgPrice = positionVolume.PositionAvgPrice;
            }
            if (positionFund != null)
            {
                if (positionFund.__isset.positionProfit)
                    template.PositionProfit = positionFund.PositionProfit;
                if (positionFund.__isset.calculatePrice)
                    template.CalculatePrice = positionFund.CalculatePrice;
                if (positionFund.__isset.useMargin)
                    template.UseMargin = positionFund.UseMargin;
                if (positionFund.__isset.frozenMargin)
                    template.FrozenMargin = positionFund.FrozenMargin;
                if (positionFund.__isset.frozenCommission)
                    template.FrozenCommission = positionFund.FrozenCommission;
                if (positionFund.__isset.goodsValue)
                    template.GoodsValue = positionFund.GoodsValue;
                if (positionFund.__isset.leverage)
                    template.Leverage = positionFund.Leverage;
            }

            UpdateDiscretePositionDM(positionDM, template);
        }

        /// <summary>
        /// 修改散列持仓 data model
        /// </summary>
        /// <param name="positionDM">目标项</param>
        /// <param name="updateTemplate">修改模板</param>
        public static void UpdateDiscretePositionDM(DiscretePositionDM positionDM,
            DiscretePositionDM updateTemplate)
        {
            if (positionDM == null || updateTemplate == null) return;

            if (updateTemplate.CreateTimestampMs != null)
                positionDM.CreateTimestampMs = updateTemplate.CreateTimestampMs;

            if (updateTemplate.UpdateTimestampMs != null)
                positionDM.UpdateTimestampMs = updateTemplate.UpdateTimestampMs;

            if (updateTemplate.PrevPosition != null)
                positionDM.PrevPosition = updateTemplate.PrevPosition;

            if (updateTemplate.LongPosition != null)
                positionDM.LongPosition = updateTemplate.LongPosition;

            if (updateTemplate.ShortPosition != null)
                positionDM.ShortPosition = updateTemplate.ShortPosition;

            if (updateTemplate.NetPosition != null)
                positionDM.NetPosition = updateTemplate.NetPosition;

            if (updateTemplate.CalculatePrice != null)
                positionDM.CalculatePrice = updateTemplate.CalculatePrice;

            if (updateTemplate.CloseProfit != null)
                positionDM.CloseProfit = updateTemplate.CloseProfit;

            if (updateTemplate.PositionProfit != null)
                positionDM.PositionProfit = updateTemplate.PositionProfit;

            if (updateTemplate.PositionAvgPrice != null)
                positionDM.PositionAvgPrice = updateTemplate.PositionAvgPrice;

            if (updateTemplate.UseMargin != null)
                positionDM.UseMargin = updateTemplate.UseMargin;

            if (updateTemplate.FrozenMargin != null)
                positionDM.FrozenMargin = updateTemplate.FrozenMargin;

            if (updateTemplate.UseCommission != null)
                positionDM.UseCommission = updateTemplate.UseCommission;

            if (updateTemplate.FrozenCommission != null)
                positionDM.FrozenCommission = updateTemplate.FrozenCommission;

            if (updateTemplate.Currency != null)
                positionDM.Currency = updateTemplate.Currency;

            if (updateTemplate.GoodsValue != null)
                positionDM.GoodsValue = updateTemplate.GoodsValue;

            if (updateTemplate.Leverage != null)
                positionDM.Leverage = updateTemplate.Leverage;

        }

        /// <summary>
        /// 精确化<see cref="DiscretePositionDM"/>的相关价格
        /// </summary>
        /// <param name="positionDM"></param>
        /// <param name="commodityTickSize"></param>
        public static void RectifyPositionPriceRelatedProps(DiscretePositionDM positionDM)
        {
            if (positionDM == null) return;

            var tickSize = positionDM.ContractDetailContainer?.CommodityDetail?.TickSize;
            if (tickSize == null) return;

            // 计算价特殊处理
            var calculatePrice = positionDM.CalculatePrice;
            if (calculatePrice != null)
            {
                positionDM.CalculatePrice = MathHelper.MakeValuePrecise(calculatePrice.Value, tickSize.Value);
            }

            // 持仓均价特殊处理
            var positionAvgPrice = positionDM.PositionAvgPrice;
            if (positionAvgPrice != null)
            {
                positionDM.PositionAvgPrice = MathHelper.MakeValuePrecise(positionAvgPrice.Value,
                    tickSize.Value * XueQiaoConstants.XQMultipleCalculatedPriceTickRate);
            }
        }
    }
}
