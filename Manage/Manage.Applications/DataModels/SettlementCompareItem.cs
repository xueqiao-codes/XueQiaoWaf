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
    /// 结算比对 data model
    /// </summary>
    public class SettlementCompareItem : Model
    {
        private TargetContract_TargetContractDetail contractDetailContainer;
        public TargetContract_TargetContractDetail ContractDetailContainer
        {
            get { return contractDetailContainer; }
            set { SetProperty(ref contractDetailContainer, value); }
        }

        private DateTime? selectedSettlementDate;
        /// <summary>
        /// 选择的交易所结算日期
        /// </summary>
        public DateTime? SelectedSettlementDate
        {
            get { return selectedSettlementDate; }
            set { SetProperty(ref selectedSettlementDate, value); }
        }

        private DateTime? selectedXqTradeDateBegin;
        /// <summary>
        /// 雪橇成交时段开始时间
        /// </summary>
        public DateTime? SelectedXqTradeDateBegin
        {
            get { return selectedXqTradeDateBegin; }
            set { SetProperty(ref selectedXqTradeDateBegin, value); }
        }

        private DateTime? selectedXqTradeDateEnd;
        /// <summary>
        /// 雪橇成交时段结束时间
        /// </summary>
        public DateTime? SelectedXqTradeDateEnd
        {
            get { return selectedXqTradeDateEnd; }
            set { SetProperty(ref selectedXqTradeDateEnd, value); }
        }

        private string settlementBillsContent;
        /// <summary>
        /// 结算单内容
        /// </summary>
        public string SettlementBillsContent
        {
            get { return settlementBillsContent; }
            set { SetProperty(ref settlementBillsContent, value); }
        }

        private ReadOnlyObservableCollection<AssetTradeDetailDM> xqTradeItems;
        /// <summary>
        /// 指定时间内的雪橇成交项
        /// </summary>
        public ReadOnlyObservableCollection<AssetTradeDetailDM> XqTradeItems
        {
            get { return xqTradeItems; }
            set { SetProperty(ref xqTradeItems, value); }
        }
    }
}
