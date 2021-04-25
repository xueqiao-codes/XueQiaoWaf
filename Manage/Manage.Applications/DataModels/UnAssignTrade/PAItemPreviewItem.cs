using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using xueqiao.trade.hosting;
using XueQiaoFoundation.BusinessResources.DataModels;

namespace Manage.Applications.DataModels
{
    /// <summary>
    /// 预分配项的预览项 data model
    /// </summary> 
    public class PAItemPreviewItem : Model
    {
        public PAItemPreviewItem(long subAccountId,
            string UATItemKey, long fundAccountId, int contractId)
        {
            this.SubAccountId = subAccountId;
            this.UATItemKey = UATItemKey;
            this.FundAccountId = fundAccountId;
            this.ContractId = contractId;
        }

        /// <summary>
        /// 分配的操作账户 id
        /// </summary>
        public long SubAccountId { get; private set; }

        // 未分配成交项的 key
        public string UATItemKey { get; private set; }

        /// <summary>
        /// 所属资账户 id
        /// </summary>
        public long FundAccountId { get; private set; }

        /// <summary>
        /// 所属合约 id
        /// </summary>
        public int ContractId { get; private set; }

        private TargetContract_TargetContractDetail contractDetailContainer;
        /// <summary>
        /// 合约信息容器
        /// </summary>
        public TargetContract_TargetContractDetail ContractDetailContainer
        {
            get { return contractDetailContainer; }
            set { SetProperty(ref contractDetailContainer, value); }
        }

        private PAItemAccountInfoContainer _PAAccountInfoContainer;
        /// <summary>
        /// 预分配账户信息容器 
        /// </summary>
        public PAItemAccountInfoContainer PAAccountInfoContainer
        {
            get { return _PAAccountInfoContainer; }
            set { SetProperty(ref _PAAccountInfoContainer, value); }
        }

        private int volume;
        // 分配数量
        public int Volume
        {
            get { return volume; }
            set { SetProperty(ref volume, value); }
        }

        private ClientTradeDirection direction;
        public ClientTradeDirection Direction
        {
            get { return direction; }
            set { SetProperty(ref direction, value); }
        }

        private double price;
        public double Price
        {
            get { return price; }
            set { SetProperty(ref price, value); }
        }

        private long tradeTimestampMs;
        public long TradeTimestampMs
        {
            get { return tradeTimestampMs; }
            set { SetProperty(ref tradeTimestampMs, value); }
        }
    }
}
