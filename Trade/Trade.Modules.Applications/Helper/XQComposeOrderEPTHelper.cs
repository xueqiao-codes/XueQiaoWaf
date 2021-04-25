using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.BusinessResources.Models;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Helper
{
    /// <summary>
    /// 雪橇组合订单执行参数模板 helper
    /// </summary>
    public static class XQComposeOrderEPTHelper
    {
        /// <summary>
        /// 使用 datamodel 模板配置clr 项模板
        /// </summary>
        /// <param name="clrItem">clr 项</param>
        /// <param name="dmItem">data model 项</param>
        /// <returns></returns>
        public static void ConfigCLREPTWithDM(ComposeOrderEPT clrItem, XQComposeOrderExecParamsTemplate dmItem)
        {
            if (clrItem == null || dmItem == null) return;

            clrItem.Key = dmItem.Key;
            clrItem.TemplateType = dmItem.TemplateType.GetHashCode();
            clrItem.Name = dmItem.Name;
            clrItem.CreateTimestampMs = dmItem.CreateTimestampMs;

            if (dmItem.ParallelLegTypeParams != null)
            {
                var srcParams = dmItem.ParallelLegTypeParams;
                clrItem.ParallelLegTypeParams = new ParallelLegParams
                {
                    LegSendOrderParam_QuantityRatio = srcParams.LegSendOrderParam_QuantityRatio,
                    LegChaseParam_Ticks = srcParams.LegChaseParam_Ticks,
                    InnerLegChaseTimes = srcParams.InnerLegChaseTimes,
                    LegChaseProtectPriceRatio = srcParams.LegChaseProtectPriceRatio,
                    EarlySuspendedForMarketSeconds = srcParams.EarlySuspendedForMarketSeconds,
                };
            }

            if (dmItem.SerialLegPriceBestTypeParams != null)
            {
                var srcParams = dmItem.SerialLegPriceBestTypeParams;
                clrItem.SerialLegPriceBestTypeParams = new SerialLegPriceBestEPTParams
                {
                    PreferOuterLegAsFirstLeg = srcParams.PreferOuterLegAsFirstLeg,
                    LegSendOrderParam_QuantityRatio = srcParams.LegSendOrderParam_QuantityRatio,
                    OutterFirstLegRevokeDeviatePriceTicks = srcParams.OutterFirstLegRevokeDeviatePriceTicks,
                    InnerFirstLegRevokeDeviatePriceTicks = srcParams.InnerFirstLegRevokeDeviatePriceTicks,
                    AfterLegChaseParam_Ticks = srcParams.AfterLegChaseParam_Ticks,
                    InnerLegChaseTimes = srcParams.InnerLegChaseTimes,
                    LegChaseProtectPriceRatio = srcParams.LegChaseProtectPriceRatio,
                    EarlySuspendedForMarketSeconds = srcParams.EarlySuspendedForMarketSeconds,
                };
            }

            if (dmItem.SerialLegPriceTryingTypeParams != null)
            {
                var srcParams = dmItem.SerialLegPriceTryingTypeParams;
                clrItem.SerialLegPriceTryingTypeParams = new SerialLegPriceTryingEPTParams
                {
                    PreferOuterLegAsFirstLeg = srcParams.PreferOuterLegAsFirstLeg,
                    AfterLegSendOrderParam_QuantityRatio = srcParams.AfterLegSendOrderParam_QuantityRatio,
                    OutterFirstLegRevokeDeviatePriceTicks = srcParams.OutterFirstLegRevokeDeviatePriceTicks,
                    InnerFirstLegRevokeDeviatePriceTicks = srcParams.InnerFirstLegRevokeDeviatePriceTicks,
                    AfterLegChaseParam_Ticks = srcParams.AfterLegChaseParam_Ticks,
                    InnerLegChaseTimes = srcParams.InnerLegChaseTimes,
                    LegChaseProtectPriceRatio = srcParams.LegChaseProtectPriceRatio,
                    BeyondInPriceTicks = srcParams.BeyondInPriceTicks,
                    EarlySuspendedForMarketSeconds = srcParams.EarlySuspendedForMarketSeconds,
                };
            }
        }

        /// <summary>
        /// 使用clr 模板配置 datamodel 模板
        /// </summary>
        /// <param name="dmItem">data model 项</param>
        /// <param name="clrItem">clr 项</param>
        /// <returns></returns>
        public static void ConfigDMEPTWithCLR(XQComposeOrderExecParamsTemplate dmItem, ComposeOrderEPT clrItem)
        {
            if (dmItem == null || clrItem == null) return;
            if (!Enum.TryParse(clrItem.TemplateType.ToString(), out XQComposeOrderExecParamsSendType templateType))
                return;

            dmItem.Name = clrItem.Name;
            dmItem.CreateTimestampMs = clrItem.CreateTimestampMs;

            if (clrItem.ParallelLegTypeParams != null)
            {
                var srcParams = clrItem.ParallelLegTypeParams;
                dmItem.ParallelLegTypeParams.LegSendOrderParam_QuantityRatio = srcParams.LegSendOrderParam_QuantityRatio;
                dmItem.ParallelLegTypeParams.LegChaseParam_Ticks = srcParams.LegChaseParam_Ticks;
                dmItem.ParallelLegTypeParams.InnerLegChaseTimes = srcParams.InnerLegChaseTimes;
                dmItem.ParallelLegTypeParams.LegChaseProtectPriceRatio = srcParams.LegChaseProtectPriceRatio;
                dmItem.ParallelLegTypeParams.EarlySuspendedForMarketSeconds = srcParams.EarlySuspendedForMarketSeconds;
            }

            if (clrItem.SerialLegPriceBestTypeParams != null)
            {
                var srcParams = clrItem.SerialLegPriceBestTypeParams;
                dmItem.SerialLegPriceBestTypeParams.PreferOuterLegAsFirstLeg = srcParams.PreferOuterLegAsFirstLeg;
                dmItem.SerialLegPriceBestTypeParams.LegSendOrderParam_QuantityRatio = srcParams.LegSendOrderParam_QuantityRatio;
                dmItem.SerialLegPriceBestTypeParams.OutterFirstLegRevokeDeviatePriceTicks = srcParams.OutterFirstLegRevokeDeviatePriceTicks;
                dmItem.SerialLegPriceBestTypeParams.InnerFirstLegRevokeDeviatePriceTicks = srcParams.InnerFirstLegRevokeDeviatePriceTicks;
                dmItem.SerialLegPriceBestTypeParams.AfterLegChaseParam_Ticks = srcParams.AfterLegChaseParam_Ticks;
                dmItem.SerialLegPriceBestTypeParams.InnerLegChaseTimes = srcParams.InnerLegChaseTimes;
                dmItem.SerialLegPriceBestTypeParams.LegChaseProtectPriceRatio = srcParams.LegChaseProtectPriceRatio;
                dmItem.SerialLegPriceBestTypeParams.EarlySuspendedForMarketSeconds = srcParams.EarlySuspendedForMarketSeconds;
            }

            if (clrItem.SerialLegPriceTryingTypeParams != null)
            {
                var srcParams = clrItem.SerialLegPriceTryingTypeParams;
                dmItem.SerialLegPriceTryingTypeParams.PreferOuterLegAsFirstLeg = srcParams.PreferOuterLegAsFirstLeg;
                dmItem.SerialLegPriceTryingTypeParams.AfterLegSendOrderParam_QuantityRatio = srcParams.AfterLegSendOrderParam_QuantityRatio;
                dmItem.SerialLegPriceTryingTypeParams.OutterFirstLegRevokeDeviatePriceTicks = srcParams.OutterFirstLegRevokeDeviatePriceTicks;
                dmItem.SerialLegPriceTryingTypeParams.InnerFirstLegRevokeDeviatePriceTicks = srcParams.InnerFirstLegRevokeDeviatePriceTicks;
                dmItem.SerialLegPriceTryingTypeParams.AfterLegChaseParam_Ticks = srcParams.AfterLegChaseParam_Ticks;
                dmItem.SerialLegPriceTryingTypeParams.InnerLegChaseTimes = srcParams.InnerLegChaseTimes;
                dmItem.SerialLegPriceTryingTypeParams.LegChaseProtectPriceRatio = srcParams.LegChaseProtectPriceRatio;
                dmItem.SerialLegPriceTryingTypeParams.BeyondInPriceTicks = srcParams.BeyondInPriceTicks;
                dmItem.SerialLegPriceTryingTypeParams.EarlySuspendedForMarketSeconds = srcParams.EarlySuspendedForMarketSeconds;
            }
        }

        /// <summary>
        /// 将客户端显示的盘口系数转换为服务端端的要求的值。
        /// </summary>
        /// <param name="clientRatio">客户端显示的盘口系数值</param>
        public static double Convert2ServerQuantityRatio(double clientRatio)
        {
            return 1d / clientRatio;
        }

        /// <summary>
        /// 将服务端的盘口系数转换为客户端的显示数值。
        /// </summary>
        /// <param name="serverRatio">服务端要求的盘口系数值</param>
        public static double Convert2ClientQuantityRatio(double serverRatio)
        {
            return 1d / serverRatio;
        }
    }
}
