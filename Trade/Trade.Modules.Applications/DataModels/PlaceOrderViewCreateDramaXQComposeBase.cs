using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NativeModel.Trade;
using XueQiaoWaf.Trade.Modules.Applications.Helper;
using xueqiao.trade.hosting.arbitrage.thriftapi;
using XueQiaoFoundation.BusinessResources.Models;

namespace XueQiaoWaf.Trade.Modules.Applications.DataModels
{
    /// <summary>
    /// 雪橇组合订单的组合腿 meta 信息
    /// </summary>
    public class XQOrderComposeLegMeta
    {
        public XQOrderComposeLegMeta(int contractId)
        {
            this.ContractId = contractId;
        }

        /// <summary>
        /// 合约 id
        /// </summary>
        public int ContractId { get; private set; }

        /// <summary>
        /// 是否属于外盘合约
        /// </summary>
        public bool IsBelongOutterExchange { get; set; }
    }

    /// <summary>
    /// 雪橇组合标的下单视图创建剧本基类
    /// </summary>
    public abstract class PlaceOrderViewCreateDramaXQComposeBase : PlaceOrderViewCreateDramaBase
    {
        protected PlaceOrderViewCreateDramaXQComposeBase(ClientPlaceOrderType viewPlaceOrderType) 
            : base(viewPlaceOrderType, ClientXQOrderTargetType.COMPOSE_TARGET)
        {
            this.ExecEveryQty = 1;
        }

        private int execEveryQty;
        /// <summary>
        /// 单次发单数量
        /// </summary>
        public int ExecEveryQty
        {
            get { return execEveryQty; }
            set { SetProperty(ref execEveryQty, value); }
        }

        private XQComposeOrderExecParamsTemplate orderExecParamsTemplate;
        /// <summary>
        /// 下单执行参数模板
        /// </summary>
        public XQComposeOrderExecParamsTemplate OrderExecParamsTemplate
        {
            get { return orderExecParamsTemplate; }
            set { SetProperty(ref orderExecParamsTemplate, value); }
        }

        /// <summary>
        /// 组合腿信息
        /// </summary>
        public IEnumerable<XQOrderComposeLegMeta> LegMetas { get; set; }

        /// <summary>
        /// 验证并生成订单执行参数。如果验证通过则生成订单执行参数，否则返回错误信息
        /// </summary>
        /// <param name="outOrderExecParams">生成的订单执行参数</param>
        /// <param name="outOrderValidateErrorMsg">验证不通过的错误信息</param>
        protected void ValidateAndGenerateOrderExecParams(out HostingXQComposeLimitOrderExecParams outOrderExecParams,
            out string outValidateErrorMsg)
        {
            var generator = new XQComposeOrderExecParamsGenerator(this.OrderExecParamsTemplate, 
                this.LegMetas, this.ExecEveryQty);
            generator.ValidateAndGenerateOrderExecParams(out outOrderExecParams, out outValidateErrorMsg);
        }
    }
}
