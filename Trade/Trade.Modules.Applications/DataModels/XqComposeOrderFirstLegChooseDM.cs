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
    /// 雪橇组合订单的先手腿选择 data model
    /// </summary>
    public class XqComposeOrderFirstLegChooseDM : Model
    {
        public XqComposeOrderFirstLegChooseDM(HostingXQComposeLimitOrderFirstLegChooseMode chooseMode, int appointSledContractId)
        {
            this.ChooseMode = chooseMode;
            this.AppointSledContractId = appointSledContractId;
        }

        /// <summary>
        /// 先手选择模式
        /// </summary>
        public HostingXQComposeLimitOrderFirstLegChooseMode ChooseMode { get; private set; }

        /// <summary>
        /// 指定先手腿模式下指定的先手合约 id
        /// </summary>
        public int AppointSledContractId { get; private set; }

        private TargetContract_TargetContractDetail appointContractDetailContainer;
        /// <summary>
        /// 指定先手的合约详情容器
        /// </summary>
        public TargetContract_TargetContractDetail AppointContractDetailContainer
        {
            get { return appointContractDetailContainer; }
            set { SetProperty(ref appointContractDetailContainer, value); }
        }
    }
}
