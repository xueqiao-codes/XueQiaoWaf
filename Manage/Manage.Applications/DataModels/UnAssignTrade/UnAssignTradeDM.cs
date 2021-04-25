using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using XueQiaoFoundation.BusinessResources.DataModels;

namespace Manage.Applications.DataModels
{
    /// <summary>
    /// 未分配成交项 data model
    /// </summary>
    public class UnAssignTradeDM : Model, IUATVolumeSummaryDM
    {
        private readonly UATVolumeSummaryDM internalVolumeSummary;
        private readonly PreviewAssignVolumesSynchronizer previewAssignVolumesSynchronizer;
        
        public UnAssignTradeDM(string itemKey, long fundAccountId, int contractId)
        {
            this.ItemKey = itemKey;
            this.FundAccountId = fundAccountId;
            this.ContractId = contractId;
            this.ContractDetailContainer = new TargetContract_TargetContractDetail(contractId);
            this.PAItems = new ObservableCollection<PositionPreviewAssignDM>();

            this.internalVolumeSummary = new UATVolumeSummaryDM();
            this.previewAssignVolumesSynchronizer = new PreviewAssignVolumesSynchronizer(this);
        }
        
        public string ItemKey { get; private set; }

        public long FundAccountId { get; private set; }

        public int ContractId { get; private set; }

        public TargetContract_TargetContractDetail ContractDetailContainer { get; private set; }

        /// <summary>
        /// 预分配的成交
        /// </summary>
        public ObservableCollection<PositionPreviewAssignDM> PAItems { get; private set; }
        
#region IUATVolumeSummartDM
        /// <summary>
        /// 总数
        /// </summary>
        public int Volume
        {
            get { return internalVolumeSummary.Volume; }
            set
            {
                var originVal = internalVolumeSummary.Volume;
                if (!Equals(originVal, value))
                {
                    internalVolumeSummary.Volume = value;
                    RaisePropertyChanged(nameof(Volume));
                }
            }
        }

        /// <summary>
        /// 已预分配的数量
        /// </summary>
        public int PreviewAssignVolume
        {
            get { return internalVolumeSummary.PreviewAssignVolume; }
            set
            {
                var originVal = internalVolumeSummary.PreviewAssignVolume;
                if (!Equals(originVal, value))
                {
                    internalVolumeSummary.PreviewAssignVolume = value;
                    RaisePropertyChanged(nameof(PreviewAssignVolume));
                }
            }
        }

        /// <summary>
        /// 未预分配的数量
        /// </summary>
        public int UnpreviewAssignVolume
        {
            get { return internalVolumeSummary.UnpreviewAssignVolume; }
            set
            {
                var originVal = internalVolumeSummary.UnpreviewAssignVolume;
                if (!Equals(originVal, value))
                {
                    internalVolumeSummary.UnpreviewAssignVolume = value;
                    RaisePropertyChanged(nameof(UnpreviewAssignVolume));
                }
            }
        }
#endregion
        
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

    public class UnAssignTradeItemKey : Tuple<string, long, int>
    {
        public UnAssignTradeItemKey(string itemKey, long fundAccountId, int contractId) 
            : base(itemKey, fundAccountId, contractId)
        {
            this.ItemKey = itemKey;
            this.FundAccountId = fundAccountId;
            this.ContractId = contractId;
        }

        public string ItemKey { get; private set; }

        public long FundAccountId { get; private set; }

        public int ContractId { get; private set; }

        public override string ToString()
        {
            return $"UnAssignTradeItemKey:{{ItemKey:{ItemKey},FundAccountId:{FundAccountId},ContractId:{ContractId}}}";
        }
    }
}
