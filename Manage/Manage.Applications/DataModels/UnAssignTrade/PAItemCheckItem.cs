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
    /// 预分配项可选择 data model
    /// </summary>
    public class PAItemCheckItem : Model
    {
        public PAItemCheckItem(PositionPreviewAssignDM PAItem)
        {
            if (PAItem == null) throw new ArgumentNullException("PAItem");
            this.PAItem = PAItem;
            PropertyChangedEventManager.AddHandler(PAItem, PAItemPropChanged, "");
        }

        public PositionPreviewAssignDM PAItem { get; private set; }

        private bool isChecked;
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsChecked
        {
            get { return isChecked; }
            set { SetProperty(ref isChecked, value); }
        }
        
        public long UATItemTradeTimestampMs
        {
            get { return this.PAItem.UATItemTradeTimestampMs; }
        }

        public int ContractId
        {
            get { return PAItem.ContractId; }
        }

        public string UATItemKey
        {
            get { return PAItem.UATItemKey; }
        }

        private void PAItemPropChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PositionPreviewAssignDM.UATItemTradeTimestampMs))
            {
                RaisePropertyChanged(nameof(UATItemTradeTimestampMs));
            }
        }
    }
}
