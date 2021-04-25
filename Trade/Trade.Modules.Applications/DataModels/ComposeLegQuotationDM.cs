using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using XueQiaoFoundation.BusinessResources.DataModels;

namespace XueQiaoWaf.Trade.Modules.Applications.DataModels
{
    /// <summary>
    /// 组合的腿行情 data model
    /// </summary>
    public class ComposeLegQuotationDM : Model
    {
        public ComposeLegQuotationDM(int contractId)
        {
            this.ContractId = contractId;
        }

        /// <summary>
        /// 合约 id
        /// </summary>
        public int ContractId { get; set; }

        private ComposeLegDetail legDetail;
        /// <summary>
        /// 腿详情
        /// </summary>
        public ComposeLegDetail LegDetail
        {
            get { return legDetail; }
            set { SetProperty(ref legDetail, value); }
        }

        private NativeQuotationItem quotation;
        /// <summary>
        /// 行情信息
        /// </summary>
        public NativeQuotationItem Quotation
        {
            get { return quotation; }
            set { SetProperty(ref quotation, value); }
        }
    }
}