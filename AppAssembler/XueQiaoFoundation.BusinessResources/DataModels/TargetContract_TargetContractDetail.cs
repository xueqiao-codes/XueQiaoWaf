using NativeModel.Contract;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;

namespace XueQiaoFoundation.BusinessResources.DataModels
{
    /// <summary>
    /// 合约标的类型数据的合约信息容器
    /// </summary>
    public class TargetContract_TargetContractDetail : Model
    {
        public TargetContract_TargetContractDetail(int contractId)
        {
            this.ContractId = contractId;
        }

        /// <summary>
        /// 合约 id
        /// </summary>
        public int ContractId { get; private set; }

        // 合约详情
        private NativeContract contractDetail;
        public NativeContract ContractDetail
        {
            get { return contractDetail; }
            set { SetProperty(ref contractDetail, value); }
        }

        // 商品详情
        private NativeCommodity commodityDetail;
        public NativeCommodity CommodityDetail
        {
            get { return commodityDetail; }
            set { SetProperty(ref commodityDetail, value); }
        }

        // 交易所详情
        private NativeExchange exchangeDetail;
        public NativeExchange ExchangeDetail
        {
            get { return exchangeDetail; }
            set { SetProperty(ref exchangeDetail, value); }
        }

        private ObservableCollection<TargetContract_TargetContractDetail> relatedContractDetails;
        /// <summary>
        /// 跨期、跨品种合约的相关合约列表
        /// </summary>
        public ObservableCollection<TargetContract_TargetContractDetail> RelatedContractDetails
        {
            get { return relatedContractDetails; }
            set { SetProperty(ref relatedContractDetails, value); }
        }
        
        private string engDisplayName;
        // 英文显示名称
        public string EngDisplayName
        {
            get { return engDisplayName; }
            set { SetProperty(ref engDisplayName, value); }
        }

        private string cnDisplayName;
        // 简体显示名称
        public string CnDisplayName
        {
            get { return cnDisplayName; }
            set { SetProperty(ref cnDisplayName, value); }
        }

        private string tcDisplayName;
        // 繁体显示名称
        public string TcDisplayName
        {
            get { return tcDisplayName; }
            set { SetProperty(ref tcDisplayName, value); }
        }
    }
}
