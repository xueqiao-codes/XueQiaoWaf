using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.trade.hosting.arbitrage.thriftapi;
using XueQiaoFoundation.BusinessResources.Models;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Helper
{
    /// <summary>
    /// 雪橇组合订单执行参数生成器
    /// </summary>
    public class XQComposeOrderExecParamsGenerator
    {
        public XQComposeOrderExecParamsGenerator(ComposeOrderEPT srcOrderEPT,
            IEnumerable<XQOrderComposeLegMeta> legMetas, int execEveryQty)
        {
            this.SrcOrderEPT = srcOrderEPT;
            this.LegMetas = legMetas;
            this.ExecEveryQty = execEveryQty;
        }

        public XQComposeOrderExecParamsGenerator(XQComposeOrderExecParamsTemplate srcOrderEPT,
            IEnumerable<XQOrderComposeLegMeta> legMetas, int execEveryQty)
        {
            var tarEPT = new ComposeOrderEPT();
            XQComposeOrderEPTHelper.ConfigCLREPTWithDM(tarEPT, srcOrderEPT);
            this.SrcOrderEPT = tarEPT;
            this.LegMetas = legMetas;
            this.ExecEveryQty = execEveryQty;
        }

        /// <summary>
        /// 下单执行参数模板
        /// </summary>
        public readonly ComposeOrderEPT SrcOrderEPT;

        /// <summary>
        /// 组合腿信息
        /// </summary>
        public readonly IEnumerable<XQOrderComposeLegMeta> LegMetas;

        /// <summary>
        /// 单次发单数量
        /// </summary>
        public readonly int ExecEveryQty;

        /// <summary>
        /// 验证并生成订单执行参数。如果验证通过则生成订单执行参数，否则返回错误信息
        /// </summary>
        /// <param name="outOrderExecParams">生成的订单执行参数</param>
        /// <param name="outOrderValidateErrorMsg">验证不通过的错误信息</param>
        public void ValidateAndGenerateOrderExecParams(out HostingXQComposeLimitOrderExecParams outOrderExecParams,
            out string outValidateErrorMsg)
        {
            outOrderExecParams = null;
            outValidateErrorMsg = null;

            var orderEPT = this.SrcOrderEPT;
            var legMetas = this.LegMetas?.ToArray();

            if (orderEPT == null)
            {
                outValidateErrorMsg = "未提供触发执行方式";
                return;
            }

            if (legMetas?.Any() != true)
            {
                outValidateErrorMsg = "不存在组合腿信息";
                return;
            }

            var orderEPTType = orderEPT.TemplateType;
            var execEveryQty = this.ExecEveryQty;

            var orderExecParams = new HostingXQComposeLimitOrderExecParams { ExecEveryQty = execEveryQty };
            if (orderEPTType == XQComposeOrderExecParamsSendType.PARALLEL_LEG.GetHashCode())
            {
                // 到价并发
                var srcTypeParams = orderEPT.ParallelLegTypeParams;
                ValidateAndGenerateXQComposeOrderParallelLegEPTParams(out HostingXQComposeLimitOrderParallelParams destParallelLegEPTParams,
                    out string _validateErrMsg, srcTypeParams, legMetas);
                if (_validateErrMsg != null)
                {
                    outValidateErrorMsg = _validateErrMsg;
                    return;
                }

                orderExecParams.ExecType = HostingXQComposeLimitOrderExecType.PARALLEL_LEG;
                orderExecParams.ExecParallelParams = destParallelLegEPTParams;
                
                var earlySuspendedForMarketSeconds = srcTypeParams.EarlySuspendedForMarketSeconds;
                if (earlySuspendedForMarketSeconds != null)
                {
                    orderExecParams.EarlySuspendedForMarketSeconds = earlySuspendedForMarketSeconds.Value;
                }
            }
            else
            {
                string _validateErrMsg = null;
                int? earlySuspendedForMarketSeconds = null;
                HostingXQComposeLimitOrderLegByParams destSerialLegParams = null;
                if (orderEPTType == XQComposeOrderExecParamsSendType.SERIAL_LEG_PRICE_BEST.GetHashCode())
                {
                    earlySuspendedForMarketSeconds = SrcOrderEPT.SerialLegPriceBestTypeParams?.EarlySuspendedForMarketSeconds;
                    ValidateAndGenerateXQComposeOrderSerialLegPriceBestEPTParams(out _validateErrMsg,
                        out destSerialLegParams, SrcOrderEPT.SerialLegPriceBestTypeParams, legMetas);
                }
                else if (orderEPTType == XQComposeOrderExecParamsSendType.SERIAL_LEG_PRICE_TRYING.GetHashCode())
                {
                    earlySuspendedForMarketSeconds = SrcOrderEPT.SerialLegPriceTryingTypeParams?.EarlySuspendedForMarketSeconds;
                    ValidateAndGenerateXQComposeOrderSerialLegPriceTryingEPTParams(out _validateErrMsg,
                        out destSerialLegParams, SrcOrderEPT.SerialLegPriceTryingTypeParams, legMetas);
                }
                else
                {
                    outValidateErrorMsg = $"当前不支持该触发执行方式(type code:{orderEPTType})";
                    return;
                }

                if (_validateErrMsg != null)
                {
                    outValidateErrorMsg = _validateErrMsg;
                    return;
                }

                orderExecParams.ExecType = HostingXQComposeLimitOrderExecType.LEG_BY_LEG;
                orderExecParams.ExecLegByParams = destSerialLegParams;
                
                if (earlySuspendedForMarketSeconds != null)
                {
                    orderExecParams.EarlySuspendedForMarketSeconds = earlySuspendedForMarketSeconds.Value;
                }
            }

            outOrderExecParams = orderExecParams;
        }
        
        private static void ValidateAndGenerateXQComposeOrderParallelLegEPTParams(
            out HostingXQComposeLimitOrderParallelParams out_DestParallelLegEPTParams,
            out string out_validateErrorMsg,
            ParallelLegParams srcParallelLegEPTParams,
            IEnumerable<XQOrderComposeLegMeta> composeLegMetas)
        {
            out_DestParallelLegEPTParams = null;
            out_validateErrorMsg = null;
            
            var destParallelLegEPTParams = new HostingXQComposeLimitOrderParallelParams
            {
                LegSendOrderExtraParam = new Dictionary<long, HostingXQComposeOrderLimitLegSendOrderExtraParam>(),
                LegChaseParams = new Dictionary<long, HostingXQComposeLimitOrderLegChaseParam>()
            };
            foreach (var legMeta in composeLegMetas)
            {
                var legSendOrderParam = new HostingXQComposeOrderLimitLegSendOrderExtraParam();
                var legChaseParam = new HostingXQComposeLimitOrderLegChaseParam();

                if (srcParallelLegEPTParams.LegSendOrderParam_QuantityRatio != null)
                {
                    legSendOrderParam.QuantityRatio = XQComposeOrderEPTHelper.Convert2ServerQuantityRatio(srcParallelLegEPTParams.LegSendOrderParam_QuantityRatio.Value);
                }

                if (srcParallelLegEPTParams.LegChaseParam_Ticks != null)
                    legChaseParam.Ticks = srcParallelLegEPTParams.LegChaseParam_Ticks.Value;

                if (srcParallelLegEPTParams.InnerLegChaseTimes != null)
                {
                    if (!legMeta.IsBelongOutterExchange)
                        legChaseParam.Times = srcParallelLegEPTParams.InnerLegChaseTimes.Value;
                }

                if (srcParallelLegEPTParams.LegChaseProtectPriceRatio != null)
                    legChaseParam.ProtectPriceRatio = srcParallelLegEPTParams.LegChaseProtectPriceRatio.Value;

                destParallelLegEPTParams.LegSendOrderExtraParam.Add(legMeta.ContractId, legSendOrderParam);
                destParallelLegEPTParams.LegChaseParams.Add(legMeta.ContractId, legChaseParam);
            }

            out_DestParallelLegEPTParams = destParallelLegEPTParams;
        }

        private static void ValidateAndFillXQComposeOrderSerialLegEPTParamsBase(
            out string outValidateErrorMsg,
            HostingXQComposeLimitOrderLegByParams fillParamsTarget,
            SerialLegEPTParams srcEPTParamsFiller,
            IEnumerable<XQOrderComposeLegMeta> composeLegMetas)
        {
            outValidateErrorMsg = null;

            if (fillParamsTarget == null) throw new ArgumentNullException("fillParamsTarget");
            if (srcEPTParamsFiller == null) throw new ArgumentNullException("srcEPTParamsFiller");
            if (composeLegMetas == null) throw new ArgumentNullException("composeLegMetas");

            // 设置 legChaseParams
            {
                if (fillParamsTarget.LegChaseParams == null)
                    fillParamsTarget.LegChaseParams = new Dictionary<long, HostingXQComposeLimitOrderLegChaseParam>();

                foreach (var legMeta in composeLegMetas)
                {
                    var chaseParam = new HostingXQComposeLimitOrderLegChaseParam();
                    if (srcEPTParamsFiller.AfterLegChaseParam_Ticks != null)
                        chaseParam.Ticks = srcEPTParamsFiller.AfterLegChaseParam_Ticks.Value;

                    if (srcEPTParamsFiller.LegChaseProtectPriceRatio != null)
                        chaseParam.ProtectPriceRatio = srcEPTParamsFiller.LegChaseProtectPriceRatio.Value;

                    if (srcEPTParamsFiller.InnerLegChaseTimes != null)
                    {
                        if (!legMeta.IsBelongOutterExchange)
                            chaseParam.Times = srcEPTParamsFiller.InnerLegChaseTimes.Value;
                    }

                    fillParamsTarget.LegChaseParams.Add(legMeta.ContractId, chaseParam);
                }
            }

            // 设置 firstLegChooseParam
            {
                if (fillParamsTarget.FirstLegChooseParam == null)
                    fillParamsTarget.FirstLegChooseParam = new HostingXQComposeLimitOrderFirstLegChooseParam();

                // 外盘先手腿优先时，应该判断是否有外盘腿，如果存在则使用外盘优先模式，否则使用默认模式
                if (srcEPTParamsFiller.PreferOuterLegAsFirstLeg
                    && (true == composeLegMetas?.Any(i => i.IsBelongOutterExchange)))
                {
                    fillParamsTarget.FirstLegChooseParam.Mode = HostingXQComposeLimitOrderFirstLegChooseMode.FIRST_LEG_CHOOSE_MODE_OUTER_DISC;
                } else
                {
                    fillParamsTarget.FirstLegChooseParam.Mode = HostingXQComposeLimitOrderFirstLegChooseMode.FIRST_LEG_CHOOSE_MODE_DEFAULT;
                }
            }


            // 设置 firstLegExtraParams
            {
                if (fillParamsTarget.FirstLegExtraParams == null)
                    fillParamsTarget.FirstLegExtraParams = new Dictionary<long, HostingXQComposeLimitOrderLegByFirstLegExtraParam>();

                foreach (var legMeta in composeLegMetas)
                {
                    var extraParam = new HostingXQComposeLimitOrderLegByFirstLegExtraParam();
                    if (legMeta.IsBelongOutterExchange && srcEPTParamsFiller.OutterFirstLegRevokeDeviatePriceTicks != null)
                        extraParam.RevokeDeviatePriceTicks = srcEPTParamsFiller.OutterFirstLegRevokeDeviatePriceTicks.Value;

                    if (!legMeta.IsBelongOutterExchange && srcEPTParamsFiller.InnerFirstLegRevokeDeviatePriceTicks != null)
                        extraParam.RevokeDeviatePriceTicks = srcEPTParamsFiller.InnerFirstLegRevokeDeviatePriceTicks.Value;

                    fillParamsTarget.FirstLegExtraParams.Add(legMeta.ContractId, extraParam);
                }
            }
        }

        private static void ValidateAndGenerateXQComposeOrderSerialLegPriceTryingEPTParams(
            out string outValidateErrorMsg,
            out HostingXQComposeLimitOrderLegByParams outDestSerialLegParams,
            SerialLegPriceTryingEPTParams srcEPTParams,
            IEnumerable<XQOrderComposeLegMeta> composeLegMetas)
        {
            outDestSerialLegParams = null;
            outValidateErrorMsg = null;
            
            var destSerialLegParams = new HostingXQComposeLimitOrderLegByParams
            {
                LegByTriggerType = HostingXQComposeLimitOrderLegByTriggerType.PRICE_TRYING,
                FirstLegTryingParams = new Dictionary<long, HostingXQComposeLimitOrderLegByPriceTryingParam>(),
                LegSendOrderExtraParam = new Dictionary<long, HostingXQComposeOrderLimitLegSendOrderExtraParam>(),
                LegChaseParams = new Dictionary<long, HostingXQComposeLimitOrderLegChaseParam>(),
                FirstLegChooseParam = new HostingXQComposeLimitOrderFirstLegChooseParam(),
                FirstLegExtraParams = new Dictionary<long, HostingXQComposeLimitOrderLegByFirstLegExtraParam>()
            };

            ValidateAndFillXQComposeOrderSerialLegEPTParamsBase(out outValidateErrorMsg, destSerialLegParams, srcEPTParams, composeLegMetas);
            if (!string.IsNullOrEmpty(outValidateErrorMsg)) return;

            // 设置 FirstLegTryingParams
            {
                foreach (var legMeta in composeLegMetas)
                {
                    var priceTryingParam = new HostingXQComposeLimitOrderLegByPriceTryingParam();
                    if (srcEPTParams.BeyondInPriceTicks != null)
                        priceTryingParam.BeyondInPriceTicks = srcEPTParams.BeyondInPriceTicks.Value;

                    destSerialLegParams.FirstLegTryingParams.Add(legMeta.ContractId, priceTryingParam);
                }
            }

            // 设置 LegSendOrderExtraParam
            {
                foreach (var legMeta in composeLegMetas)
                {
                    var sendParam = new HostingXQComposeOrderLimitLegSendOrderExtraParam();
                    if (srcEPTParams.AfterLegSendOrderParam_QuantityRatio != null)
                    {
                        sendParam.QuantityRatio = XQComposeOrderEPTHelper.Convert2ServerQuantityRatio(srcEPTParams.AfterLegSendOrderParam_QuantityRatio.Value);
                    }

                    destSerialLegParams.LegSendOrderExtraParam.Add(legMeta.ContractId, sendParam);
                }
            }

            outDestSerialLegParams = destSerialLegParams;
        }

        private static void ValidateAndGenerateXQComposeOrderSerialLegPriceBestEPTParams(
            out string outValidateErrorMsg,
            out HostingXQComposeLimitOrderLegByParams outDestSerialLegParams,
            SerialLegPriceBestEPTParams srcEPTParams,
            IEnumerable<XQOrderComposeLegMeta> composeLegMetas)
        {
            outDestSerialLegParams = null;
            outValidateErrorMsg = null;
            
            var destSerialLegParams = new HostingXQComposeLimitOrderLegByParams
            {
                LegByTriggerType = HostingXQComposeLimitOrderLegByTriggerType.PRICE_BEST,
                LegSendOrderExtraParam = new Dictionary<long, HostingXQComposeOrderLimitLegSendOrderExtraParam>(),
                LegChaseParams = new Dictionary<long, HostingXQComposeLimitOrderLegChaseParam>(),
                FirstLegChooseParam = new HostingXQComposeLimitOrderFirstLegChooseParam(),
                FirstLegExtraParams = new Dictionary<long, HostingXQComposeLimitOrderLegByFirstLegExtraParam>()
            };

            ValidateAndFillXQComposeOrderSerialLegEPTParamsBase(out outValidateErrorMsg, destSerialLegParams, srcEPTParams, composeLegMetas);
            if (!string.IsNullOrEmpty(outValidateErrorMsg)) return;

            // 设置 LegSendOrderExtraParam
            {
                foreach (var legMeta in composeLegMetas)
                {
                    var sendParam = new HostingXQComposeOrderLimitLegSendOrderExtraParam();
                    if (srcEPTParams.LegSendOrderParam_QuantityRatio != null)
                    {
                        sendParam.QuantityRatio = XQComposeOrderEPTHelper.Convert2ServerQuantityRatio(srcEPTParams.LegSendOrderParam_QuantityRatio.Value);
                    }

                    destSerialLegParams.LegSendOrderExtraParam.Add(legMeta.ContractId, sendParam);
                }
            }

            outDestSerialLegParams = destSerialLegParams;
        }
    }
}
