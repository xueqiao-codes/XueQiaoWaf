using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using XueQiaoFoundation.BusinessResources.DataModels;

namespace Manage.Applications.DataModels
{
    /// <summary>
    /// 雪橇预览成交项 data model
    /// </summary>
    public class PositionVerifyTradeInputDM : Model
    {
        public PositionVerifyTradeInputDM(string key, long fundAccountId, long? belongVerifyDailySec)
        {
            this.Key = key;
            this.FundAccountId = fundAccountId;
            this.BelongVerifyDailySec = belongVerifyDailySec;
            InvalidateIsPreservable();
        }

        public string Key { get; private set; }

        public long FundAccountId { get; private set; }

        /// <summary>
        /// 所属核对日期时间点(秒)
        /// </summary>
        public long? BelongVerifyDailySec { get; private set; }

        private int? contractId;
        public int? ContractId
        {
            get { return contractId; }
            set
            {
                if (SetProperty(ref contractId, value))
                {
                    InvalidateIsPreservable();
                }
            }
        }

        private TargetContract_TargetContractDetail contractDetailContainer;
        public TargetContract_TargetContractDetail ContractDetailContainer
        {
            get { return contractDetailContainer; }
            set { SetProperty(ref contractDetailContainer, value); }
        }

        private long? tradeTimestamp;
        public long? TradeTimestamp
        {
            get { return tradeTimestamp; }
            set
            {
                if (SetProperty(ref tradeTimestamp, value))
                {
                    InvalidateIsPreservable();
                }
            }
        }

        private ClientTradeDirection? direction;
        public ClientTradeDirection? Direction
        {
            get { return direction; }
            set
            {
                if (SetProperty(ref direction, value))
                {
                    InvalidateIsPreservable();
                }
            }
        }

        private double? price;
        public double? Price
        {
            get { return price; }
            set
            {
                if (SetProperty(ref price, value))
                {
                    InvalidateIsPreservable();
                }
            }
        }

        private int? quantity;
        public int? Quantity
        {
            get { return quantity; }
            set
            {
                if (SetProperty(ref quantity, value))
                {
                    InvalidateIsPreservable();
                }
            }
        }

        /// <summary>
        /// 是否可保存
        /// </summary>
        private bool isPreservable;
        public bool IsPreservable
        {
            get { return isPreservable; }
            private set { SetProperty(ref isPreservable, value); }
        }

        private void InvalidateIsPreservable()
        {
            if (ContractId == null)
            {
                IsPreservable = false;
                return;
            }
            if (TradeTimestamp == null && Direction == null && Price == null && Quantity == null)
            {
                IsPreservable = false;
                return;
            }

            IsPreservable = true;
        }
    }
}
