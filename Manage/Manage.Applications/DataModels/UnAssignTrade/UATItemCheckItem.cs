using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;

namespace Manage.Applications.DataModels
{
    /// <summary>
    /// 未分配成交预可选择 data model
    /// </summary>
    public class UATItemCheckItem : Model
    {
        public UATItemCheckItem(UnAssignTradeDM UATItem)
        {
            if (UATItem == null) throw new ArgumentNullException("UATItem");
            this.UATItem = UATItem;
            PropertyChangedEventManager.AddHandler(UATItem, UATItemPropChanged, "");
        }

        public UnAssignTradeDM UATItem { get; private set; }

        private bool isChecked;
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsChecked
        {
            get { return isChecked; }
            set { SetProperty(ref isChecked, value); }
        }

        /// <summary>
        /// 已预分配的数量
        /// </summary>
        public int PreviewAssignVolume
        {
            get { return this.UATItem.PreviewAssignVolume; }
        }

        /// <summary>
        /// 未预分配的数量
        /// </summary>
        public int UnpreviewAssignVolume
        {
            get { return this.UATItem.UnpreviewAssignVolume; }
        }
        
        public long TradeTimestampMs
        {
            get { return this.UATItem.TradeTimestampMs; }
        }

        public int ContractId
        {
            get { return UATItem.ContractId; }
        }

        public string UATItemKey
        {
            get { return UATItem.ItemKey; }
        }

        private void UATItemPropChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(UnAssignTradeDM.PreviewAssignVolume))
            {
                RaisePropertyChanged(nameof(PreviewAssignVolume));
            }
            else if (e.PropertyName == nameof(UnAssignTradeDM.UnpreviewAssignVolume))
            {
                RaisePropertyChanged(nameof(UnpreviewAssignVolume));
            }
            else if (e.PropertyName == nameof(UnAssignTradeDM.TradeTimestampMs))
            {
                RaisePropertyChanged(nameof(TradeTimestampMs));
            }
        }
    }
}
