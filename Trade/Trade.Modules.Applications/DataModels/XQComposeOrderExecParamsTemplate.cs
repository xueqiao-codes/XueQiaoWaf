using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using XueQiaoFoundation.Shared.Model;

namespace XueQiaoWaf.Trade.Modules.Applications.DataModels
{
    /// <summary>
    /// 组合订单执行参数模板
    /// </summary>
    public class XQComposeOrderExecParamsTemplate : ValidationModel, IValidatableObject
    {
        public XQComposeOrderExecParamsTemplate(string key, XQComposeOrderExecParamsSendType templateType)
        {
            this.Key = key;
            this.TemplateType = templateType;
            this.ParallelLegTypeParams = new XQComposeOrderParallelLegEPTParams();
            this.SerialLegPriceBestTypeParams = new XQComposeOrderSerialLegPriceBestEPTParams();
            this.SerialLegPriceTryingTypeParams = new XQComposeOrderSerialLegPriceTryingEPTParams();
        }

        /// <summary>
        /// 模板 key
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// 模板类型
        /// </summary>
        public XQComposeOrderExecParamsSendType TemplateType { get; private set; }

        private long createTimestampMs;
        /// <summary>
        /// 创建时间戳
        /// </summary>
        public long CreateTimestampMs
        {
            get { return createTimestampMs; }
            set { SetProperty(ref createTimestampMs, value); }
        }

        private string name;
        /// <summary>
        /// 模板名称
        /// </summary>
        [Required(ErrorMessage = "Name 必填")]
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        private bool isArchived;
        /// <summary>
        /// 是否已归档
        /// </summary>
        public bool IsArchived
        {
            get { return isArchived; }
            set { SetProperty(ref isArchived, value); }
        }

        private bool isInEditMode;
        /// <summary>
        /// 是否处于编辑模式
        /// </summary>
        public bool IsInEditMode
        {
            get { return isInEditMode; }
            set { SetProperty(ref isInEditMode, value); }
        }

        /// <summary>
        /// <see cref="TemplateType"/>为<see cref="XQComposeOrderExecParamsSendType.PARALLEL_LEG"/>类型的参数详情
        /// </summary>
        public XQComposeOrderParallelLegEPTParams ParallelLegTypeParams { get; private set; }

        /// <summary>
        /// <see cref="TemplateType"/>为<see cref="XQComposeOrderExecParamsSendType.SERIAL_LEG_PRICE_BEST"/>类型的参数详情
        /// </summary>
        public XQComposeOrderSerialLegPriceBestEPTParams SerialLegPriceBestTypeParams { get; private set; }

        /// <summary>
        /// <see cref="TemplateType"/>为<see cref="XQComposeOrderExecParamsSendType.SERIAL_LEG_PRICE_TRYING"/>类型的参数详情
        /// </summary>
        public XQComposeOrderSerialLegPriceTryingEPTParams SerialLegPriceTryingTypeParams { get; private set; }

        /// <summary>
        /// 设置成默认参数
        /// </summary>
        public void ConfigParamsAsDefault()
        {
            if (this.TemplateType == XQComposeOrderExecParamsSendType.PARALLEL_LEG)
                this.ParallelLegTypeParams.ConfigParamsWithDefault();
            else if (this.TemplateType == XQComposeOrderExecParamsSendType.SERIAL_LEG_PRICE_BEST)
                this.SerialLegPriceBestTypeParams.ConfigParamsWithDefault();
            else if (this.TemplateType == XQComposeOrderExecParamsSendType.SERIAL_LEG_PRICE_TRYING)
                this.SerialLegPriceTryingTypeParams.ConfigParamsWithDefault();
        }

        #region IValidatableObject
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();
            if (this.TemplateType == XQComposeOrderExecParamsSendType.PARALLEL_LEG)
            {
                var valErrMsg = this.ParallelLegTypeParams.Validate();
                if (!string.IsNullOrEmpty(valErrMsg))
                {
                    validationResults.Add(new ValidationResult(valErrMsg, new[] { nameof(ParallelLegTypeParams) }));
                }
            }
            else if (this.TemplateType == XQComposeOrderExecParamsSendType.SERIAL_LEG_PRICE_BEST)
            {
                var valErrMsg = this.SerialLegPriceBestTypeParams.Validate();
                if (!string.IsNullOrEmpty(valErrMsg))
                {
                    validationResults.Add(new ValidationResult(valErrMsg, new[] { nameof(SerialLegPriceBestTypeParams) }));
                }
            }
            else if (this.TemplateType == XQComposeOrderExecParamsSendType.SERIAL_LEG_PRICE_TRYING)
            {
                var valErrMsg = this.SerialLegPriceTryingTypeParams.Validate();
                if (!string.IsNullOrEmpty(valErrMsg))
                {
                    validationResults.Add(new ValidationResult(valErrMsg, new[] { nameof(SerialLegPriceTryingTypeParams) }));
                }
            }
            return validationResults;
        }
        #endregion
    }
    
    /// <summary>
    /// 到价并发类型模板的执行参数详情
    /// </summary>
    public class XQComposeOrderParallelLegEPTParams : ValidationModel
    {
        private double? legSendOrderParam_QuantityRatio;
        /// <summary>
        /// 发单条件参数：盘口系数
        /// </summary>
        [Range(0.0, double.MaxValue, ErrorMessage = "LegSendOrderParam_QuantityRatio 必须 >=0")]
        [Required(ErrorMessage = "LegSendOrderParam_QuantityRatio 必填")]
        public double? LegSendOrderParam_QuantityRatio
        {
            get { return legSendOrderParam_QuantityRatio; }
            set { SetProperty(ref legSendOrderParam_QuantityRatio, value); }
        }

        private int? legChaseParam_Ticks;
        /// <summary>
        /// 腿追价参数：下单追价 tick 数
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "LegChaseParam_Ticks 必须 >=0")]
        [Required(ErrorMessage = "LegChaseParam_Ticks 必填")]
        public int? LegChaseParam_Ticks
        {
            get { return legChaseParam_Ticks; }
            set { SetProperty(ref legChaseParam_Ticks, value); }
        }

        private int? innerLegChaseTimes;
        /// <summary>
        /// 内盘各腿追单（撤单）次数限制
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "InnerLegChaseTimes 必须 >=0")]
        [Required(ErrorMessage = "InnerLegChaseTimes 必填")]
        public int? InnerLegChaseTimes
        {
            get { return innerLegChaseTimes; }
            set { SetProperty(ref innerLegChaseTimes, value); }
        }

        private double? legChaseProtectPriceRatio;
        /// <summary>
        /// 各腿保护价格比例：即追加价格不超过初次目标价格的比例
        /// </summary>
        [Range(0.0, 1.0, ErrorMessage = "LegChaseProtectPriceRatio 必须 >=0 且 <=1")]
        [Required(ErrorMessage = "LegChaseProtectPriceRatio 必填")]
        public double? LegChaseProtectPriceRatio
        {
            get { return legChaseProtectPriceRatio; }
            set { SetProperty(ref legChaseProtectPriceRatio, value); }
        }

        private int? earlySuspendedForMarketSeconds;
        /// <summary>
        /// 提前收市暂停的秒数
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "提前收市暂停秒数必须 >= 0")]
        [Required(ErrorMessage = "提前收市暂停秒数未填写")]
        public int? EarlySuspendedForMarketSeconds
        {
            get { return earlySuspendedForMarketSeconds; }
            set { SetProperty(ref earlySuspendedForMarketSeconds, value); }
        }

        /// <summary>
        /// 默认参数
        /// </summary>
        private static readonly XQComposeOrderParallelLegEPTParams defaultEPTParams 
            = new XQComposeOrderParallelLegEPTParams
        {
            LegSendOrderParam_QuantityRatio = 0.6,
            LegChaseParam_Ticks = 0,
            InnerLegChaseTimes = 10,
            LegChaseProtectPriceRatio = 0.001,
            EarlySuspendedForMarketSeconds = 5
        };

        /// <summary>
        /// 设置成默认参数
        /// </summary>
        public void ConfigParamsWithDefault()
        {
            this.LegSendOrderParam_QuantityRatio = defaultEPTParams.LegSendOrderParam_QuantityRatio;
            this.LegChaseParam_Ticks = defaultEPTParams.LegChaseParam_Ticks;
            this.InnerLegChaseTimes = defaultEPTParams.InnerLegChaseTimes;
            this.LegChaseProtectPriceRatio = defaultEPTParams.LegChaseProtectPriceRatio;
            this.EarlySuspendedForMarketSeconds = defaultEPTParams.EarlySuspendedForMarketSeconds;
        }
    }

    /// <summary>
    /// 逐腿类型模板的执行参数详情基类
    /// </summary>
    public class XQComposeOrderSerialLegEPTParamsBase : ValidationModel
    {
        private bool preferOuterLegAsFirstLeg;
        /// <summary>
        /// 是否优先把外盘作为先手腿
        /// </summary>
        public bool PreferOuterLegAsFirstLeg
        {
            get { return preferOuterLegAsFirstLeg; }
            set { SetProperty(ref preferOuterLegAsFirstLeg, value); }
        }
        
        private int? outterFirstLegRevokeDeviatePriceTicks;
        /// <summary>
        /// 外盘先手撤单参数：撤单偏离价格容忍度，当目标价偏离原有挂单价多少Tick时进行撤单
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "OutterFirstLegRevokeDeviatePriceTicks 必须 >=0")]
        [Required(ErrorMessage = "OutterFirstLegRevokeDeviatePriceTicks 必填")]
        public int? OutterFirstLegRevokeDeviatePriceTicks
        {
            get { return outterFirstLegRevokeDeviatePriceTicks; }
            set { SetProperty(ref outterFirstLegRevokeDeviatePriceTicks, value); }
        }

        private int? innerFirstLegRevokeDeviatePriceTicks;
        /// <summary>
        /// 内盘先手撤单参数：撤单偏离价格容忍度
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "InnerFirstLegRevokeDeviatePriceTicks 必须 >=0")]
        [Required(ErrorMessage = "InnerFirstLegRevokeDeviatePriceTicks 必填")]
        public int? InnerFirstLegRevokeDeviatePriceTicks
        {
            get { return innerFirstLegRevokeDeviatePriceTicks; }
            set { SetProperty(ref innerFirstLegRevokeDeviatePriceTicks, value); }
        }

        private int? afterLegChaseParam_Ticks;
        /// <summary>
        /// 后手追价参数：下单追价 tick 数
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "AfterLegChaseParam_Ticks 必须 >=0")]
        [Required(ErrorMessage = "AfterLegChaseParam_Ticks 必填")]
        public int? AfterLegChaseParam_Ticks
        {
            get { return afterLegChaseParam_Ticks; }
            set { SetProperty(ref afterLegChaseParam_Ticks, value); }
        }

        private int? innerLegChaseTimes;
        /// <summary>
        /// 内盘各腿追单（撤单）次数限制
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "InnerLegChaseTimes 必须 >=0")]
        [Required(ErrorMessage = "InnerLegChaseTimes 必填")]
        public int? InnerLegChaseTimes
        {
            get { return innerLegChaseTimes; }
            set { SetProperty(ref innerLegChaseTimes, value); }
        }

        private double? legChaseProtectPriceRatio;
        /// <summary>
        /// 各腿保护价格比例：即追加价格不超过初次目标价格的比例
        /// </summary>
        [Range(0.0, 1.0, ErrorMessage = "LegChaseProtectPriceRatio 必须 >=0 且 <=1")]
        [Required(ErrorMessage = "LegChaseProtectPriceRatio 必填")]
        public double? LegChaseProtectPriceRatio
        {
            get { return legChaseProtectPriceRatio; }
            set { SetProperty(ref legChaseProtectPriceRatio, value); }
        }

        private int? earlySuspendedForMarketSeconds;
        /// <summary>
        /// 提前收市暂停的秒数
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "提前收市暂停秒数必须 >= 0")]
        [Required(ErrorMessage = "提前收市暂停秒数未填写")]
        public int? EarlySuspendedForMarketSeconds
        {
            get { return earlySuspendedForMarketSeconds; }
            set { SetProperty(ref earlySuspendedForMarketSeconds, value); }
        }
    }

    /// <summary>
    /// 逐腿到价类型模板的执行参数详情
    /// </summary>
    public class XQComposeOrderSerialLegPriceBestEPTParams : XQComposeOrderSerialLegEPTParamsBase
    {
        private double? legSendOrderParam_QuantityRatio;
        /// <summary>
        /// 发单条件参数：各腿盘口系数
        /// </summary>
        [Range(0.0, double.MaxValue, ErrorMessage = "LegSendOrderParam_QuantityRatio 必须 >=0")]
        [Required(ErrorMessage = "LegSendOrderParam_QuantityRatio 必填")]
        public double? LegSendOrderParam_QuantityRatio
        {
            get { return legSendOrderParam_QuantityRatio; }
            set { SetProperty(ref legSendOrderParam_QuantityRatio, value); }
        }

        /// <summary>
        /// 默认参数
        /// </summary>
        private static readonly XQComposeOrderSerialLegPriceBestEPTParams defaultEPTParams = new XQComposeOrderSerialLegPriceBestEPTParams
        {
            PreferOuterLegAsFirstLeg = true,
            OutterFirstLegRevokeDeviatePriceTicks = 1,
            InnerFirstLegRevokeDeviatePriceTicks = 1,
            AfterLegChaseParam_Ticks = 0,
            InnerLegChaseTimes = 10,
            LegChaseProtectPriceRatio = 0.001,
            LegSendOrderParam_QuantityRatio = 0.6,
            EarlySuspendedForMarketSeconds = 5
        };

        /// <summary>
        /// 设置成默认参数
        /// </summary>
        public void ConfigParamsWithDefault()
        {
            this.PreferOuterLegAsFirstLeg = defaultEPTParams.PreferOuterLegAsFirstLeg;
            this.OutterFirstLegRevokeDeviatePriceTicks = defaultEPTParams.OutterFirstLegRevokeDeviatePriceTicks;
            this.InnerFirstLegRevokeDeviatePriceTicks = defaultEPTParams.InnerFirstLegRevokeDeviatePriceTicks;
            this.AfterLegChaseParam_Ticks = defaultEPTParams.AfterLegChaseParam_Ticks;
            this.InnerLegChaseTimes = defaultEPTParams.InnerLegChaseTimes;
            this.LegChaseProtectPriceRatio = defaultEPTParams.LegChaseProtectPriceRatio;
            this.LegSendOrderParam_QuantityRatio = defaultEPTParams.LegSendOrderParam_QuantityRatio;
            this.EarlySuspendedForMarketSeconds = defaultEPTParams.EarlySuspendedForMarketSeconds;
        }
    }

    /// <summary>
    /// 逐腿挂单类型模板的执行参数详情
    /// </summary>
    public class XQComposeOrderSerialLegPriceTryingEPTParams : XQComposeOrderSerialLegEPTParamsBase
    {
        private int? beyondInPriceTicks;
        /// <summary>
        /// 价格优于排队价多少个Tick时尝试发单
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "BeyondInPriceTicks 必须 >= 0")]
        [Required(ErrorMessage = "BeyondInPriceTicks 必填")]
        public int? BeyondInPriceTicks
        {
            get { return beyondInPriceTicks; }
            set { SetProperty(ref beyondInPriceTicks, value); }
        }

        private double? afterLegSendOrderParam_QuantityRatio;
        /// <summary>
        /// 发单条件参数：后手腿盘口系数
        /// </summary>
        [Range(0.0, double.MaxValue, ErrorMessage = "AfterLegSendOrderParam_QuantityRatio 必须 >=0")]
        [Required(ErrorMessage = "AfterLegSendOrderParam_QuantityRatio 必填")]
        public double? AfterLegSendOrderParam_QuantityRatio
        {
            get { return afterLegSendOrderParam_QuantityRatio; }
            set { SetProperty(ref afterLegSendOrderParam_QuantityRatio, value); }
        }

        /// <summary>
        /// 默认参数
        /// </summary>
        private static readonly XQComposeOrderSerialLegPriceTryingEPTParams defaultEPTParams 
            = new XQComposeOrderSerialLegPriceTryingEPTParams
        {
            PreferOuterLegAsFirstLeg = true,
            OutterFirstLegRevokeDeviatePriceTicks = 1,
            InnerFirstLegRevokeDeviatePriceTicks = 1,
            AfterLegChaseParam_Ticks = 0,
            InnerLegChaseTimes = 10,
            LegChaseProtectPriceRatio = 0.001,
            BeyondInPriceTicks = 1,
            AfterLegSendOrderParam_QuantityRatio = 0.6,
            EarlySuspendedForMarketSeconds = 5
        };

        /// <summary>
        /// 设置成默认参数
        /// </summary>
        public new void ConfigParamsWithDefault()
        {
            this.PreferOuterLegAsFirstLeg = defaultEPTParams.PreferOuterLegAsFirstLeg;
            this.OutterFirstLegRevokeDeviatePriceTicks = defaultEPTParams.OutterFirstLegRevokeDeviatePriceTicks;
            this.InnerFirstLegRevokeDeviatePriceTicks = defaultEPTParams.InnerFirstLegRevokeDeviatePriceTicks;
            this.AfterLegChaseParam_Ticks = defaultEPTParams.AfterLegChaseParam_Ticks;
            this.InnerLegChaseTimes = defaultEPTParams.InnerLegChaseTimes;
            this.LegChaseProtectPriceRatio = defaultEPTParams.LegChaseProtectPriceRatio;
            this.BeyondInPriceTicks = defaultEPTParams.BeyondInPriceTicks;
            this.AfterLegSendOrderParam_QuantityRatio = defaultEPTParams.AfterLegSendOrderParam_QuantityRatio;
            this.EarlySuspendedForMarketSeconds = defaultEPTParams.EarlySuspendedForMarketSeconds;
        }
    }
}
