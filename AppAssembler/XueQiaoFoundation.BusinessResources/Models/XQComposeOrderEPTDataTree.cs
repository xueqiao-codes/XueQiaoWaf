using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.BusinessResources.Models
{
    /// <summary>
    /// 雪橇组合订单执行参数模板（Execute parameters template）数据树 
    /// </summary>
    public class XQComposeOrderEPTDataTree
    {
        /// <summary>
        /// 模板列表
        /// </summary>
        public ComposeOrderEPT[] EPTs { get; set; }
    }

    public class ComposeOrderEPT
    {
        /// <summary>
        /// 模板 key
        /// </summary>
        public string Key { get; set; }
        
        /// <summary>
        /// 模板类型
        /// </summary>
        public int TemplateType { get; set; }


        /// <summary>
        /// 创建时间戳
        /// </summary>
        public long CreateTimestampMs { get; set; }

        /// <summary>
        /// 模板名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// <see cref="TemplateType"/>为`到价并发`类型的参数详情
        /// </summary>
        public ParallelLegParams ParallelLegTypeParams { get; set; }

        /// <summary>
        /// <see cref="TemplateType"/>为`逐腿到价`类型的参数详情
        /// </summary>
        public SerialLegPriceBestEPTParams SerialLegPriceBestTypeParams { get; set; }

        /// <summary>
        /// <see cref="TemplateType"/>为`逐腿挂单`类型的参数详情
        /// </summary>
        public SerialLegPriceTryingEPTParams SerialLegPriceTryingTypeParams { get; set; }
    }

    public class ParallelLegParams
    {
        /// <summary>
        /// 发单条件参数：盘口系数
        /// 客户端表示：挂单量 <= 行情量 * 百分比
        /// 后台标识：总挂单量 * 百分比 <= 行情量
        /// 两者需要做一个转换
        /// </summary>
        public double? LegSendOrderParam_QuantityRatio { get; set; }

        /// <summary>
        /// 腿追价参数：下单追价 tick 数
        /// </summary>
        public int? LegChaseParam_Ticks { get; set; }

        /// <summary>
        /// 内盘各腿追单（撤单）次数限制
        /// </summary>
        public int? InnerLegChaseTimes { get; set; }

        /// <summary>
        /// 各腿保护价格比例：即追加价格不超过初次目标价格的比例
        /// </summary>
        public double? LegChaseProtectPriceRatio { get; set; }

        /// <summary>
        /// 提前收市暂停的秒数
        /// </summary>
        public int? EarlySuspendedForMarketSeconds { get; set; }
    }

    public class SerialLegEPTParams
    {
        /// <summary>
        /// 是否优先把外盘作为先手腿
        /// </summary>
        public bool PreferOuterLegAsFirstLeg { get; set; }
        
        /// <summary>
        /// 外盘先手撤单参数：撤单偏离价格容忍度，当目标价偏离原有挂单价多少Tick时进行撤单
        /// </summary>
        public int? OutterFirstLegRevokeDeviatePriceTicks { get; set; }

        /// <summary>
        /// 内盘先手撤单参数：撤单偏离价格容忍度
        /// </summary>
        public int? InnerFirstLegRevokeDeviatePriceTicks { get; set; }

        /// <summary>
        /// 后手追价参数：下单追价 tick 数
        /// </summary>
        public int? AfterLegChaseParam_Ticks { get; set; }

        /// <summary>
        /// 内盘各腿追单（撤单）次数限制
        /// </summary>
        public int? InnerLegChaseTimes { get; set; }

        /// <summary>
        /// 各腿保护价格比例：即追加价格不超过初次目标价格的比例
        /// </summary>
        public double? LegChaseProtectPriceRatio { get; set; }

        /// <summary>
        /// 提前收市暂停的秒数
        /// </summary>
        public int? EarlySuspendedForMarketSeconds { get; set; }
    }
    
    public class SerialLegPriceBestEPTParams : SerialLegEPTParams
    {
        /// <summary>
        /// 发单条件参数：腿盘口系数
        /// 客户端表示：挂单量 <= 行情量 * 百分比
        /// 后台标识：总挂单量 * 百分比 <= 行情量
        /// 两者需要做一个转换
        /// </summary>
        public double? LegSendOrderParam_QuantityRatio { get; set; }
    }

    public class SerialLegPriceTryingEPTParams : SerialLegEPTParams
    {
        /// <summary>
        /// 价格优于排队价多少个Tick时尝试发单
        /// </summary>
        public int? BeyondInPriceTicks { get; set; }

        /// <summary>
        /// 发单条件参数：后手腿盘口系数
        /// 客户端表示：挂单量 <= 行情量 * 百分比
        /// 后台标识：总挂单量 * 百分比 <= 行情量
        /// 两者需要做一个转换
        /// </summary>
        public double? AfterLegSendOrderParam_QuantityRatio { get; set; }
    }

}
