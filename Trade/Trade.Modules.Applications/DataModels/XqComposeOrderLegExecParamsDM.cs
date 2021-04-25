using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using xueqiao.trade.hosting.arbitrage.thriftapi;
using XueQiaoFoundation.BusinessResources.DataModels;

namespace XueQiaoWaf.Trade.Modules.Applications.DataModels
{
    /// <summary>
    /// 雪橇组合订单的腿执行参数显示 data model
    /// </summary>
    public class XqComposeOrderLegExecParamsDM : Model
    {
        public XqComposeOrderLegExecParamsDM(int contractId, XQComposeOrderExecParamsSendType orderSendType)
        {
            this.ContractId = contractId;
            this.OrderSendType = orderSendType;
            this.ParallelTypeParams = new XqComposeOrderLegParams_Parallel();
            this.SerialLegPriceBestTypeParams = new XqComposeOrderLegParams_SerialLeg();
            this.SerialLegPriceTryingTypeParams = new XqComposeOrderLegParams_SerialPriceTrying();
        }

        public int ContractId { get; private set; }

        /// <summary>
        /// 发单方式
        /// </summary>
        public XQComposeOrderExecParamsSendType OrderSendType { get; private set; }
        
        /// <summary>
        /// 腿详情
        /// </summary>
        private ComposeLegDetail legDetail;
        public ComposeLegDetail LegDetail
        {
            get { return legDetail; }
            set { SetProperty(ref legDetail, value); }
        }

        /// <summary>
        /// 并发执行类型组合腿的执行参数
        /// </summary>
        public XqComposeOrderLegParams_Parallel ParallelTypeParams { get; private set; }

        /// <summary>
        /// 逐腿到价类型组合腿的执行参数
        /// </summary>
        public XqComposeOrderLegParams_SerialLeg SerialLegPriceBestTypeParams { get; private set; }

        /// <summary>
        /// 逐腿挂单类型组合腿的执行参数
        /// </summary>
        public XqComposeOrderLegParams_SerialPriceTrying SerialLegPriceTryingTypeParams { get; private set; }
    }

    /// <summary>
    /// `并发执行`类型组合腿的执行参数
    /// </summary>
    public class XqComposeOrderLegParams_Parallel
    {
        // 腿的额外发单参数
        public HostingXQComposeOrderLimitLegSendOrderExtraParam SendOrderExtraParam { get; set; }

        // 腿的追单参数
        public HostingXQComposeLimitOrderLegChaseParam ChaseParam { get; set; }
    }

    /// <summary>
    /// `逐腿执行`类型组合腿的执行参数
    /// </summary>
    public class XqComposeOrderLegParams_SerialLeg
    {
        public HostingXQComposeOrderLimitLegSendOrderExtraParam SendOrderExtraParam { get; set; }

        public HostingXQComposeLimitOrderLegChaseParam ChaseParam { get; set; }

        public HostingXQComposeLimitOrderLegByFirstLegExtraParam FirstLegExtraParam { get; set; }
    }

    /// <summary>
    /// `逐腿执行`中的逐腿挂单的类型执行参数
    /// </summary>
    public class XqComposeOrderLegParams_SerialPriceTrying : XqComposeOrderLegParams_SerialLeg
    {
        public HostingXQComposeLimitOrderLegByPriceTryingParam FirstLegTryingParam { get; set; }
    }
}
