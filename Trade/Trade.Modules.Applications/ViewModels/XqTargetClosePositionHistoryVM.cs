using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;
using xueqiao.trade.hosting.position.statis;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class XqTargetClosePositionHistoryVM : ViewModel<IXqTargetClosePositionHistoryView>
    {
        [ImportingConstructor]
        protected XqTargetClosePositionHistoryVM(IXqTargetClosePositionHistoryView view) : base(view)
        {
            BuyDetailPositionItems = new ObservableCollection<XqTargetDetailPositionDM_Archived>();
            SellDetailPositionItems = new ObservableCollection<XqTargetDetailPositionDM_Archived>();
        }
        
        public void UpdateXqTargetType(ClientXQOrderTargetType xqTargetType)
        {
            ViewCore.UpdatePositionColumnsByPresentTarget(xqTargetType);
        }

        private ICommand refreshDataCmd;
        public ICommand RefreshDataCmd
        {
            get { return refreshDataCmd; }
            set { SetProperty(ref refreshDataCmd, value); }
        }

        private long? refreshDataTimestamp;
        public long? RefreshDataTimestamp
        {
            get { return refreshDataTimestamp; }
            set { SetProperty(ref refreshDataTimestamp, value); }
        }
        
        private DateTime? selectedDate;
        public DateTime? SelectedDate
        {
            get { return selectedDate; }
            set { SetProperty(ref selectedDate, value); }
        }

        private StatClosedPositionDateSummary archivedClosedPositionSummary;
        /// <summary>
        /// 归档配对概要
        /// </summary>
        public StatClosedPositionDateSummary ArchivedClosedPositionSummary
        {
            get { return archivedClosedPositionSummary; }
            set
            {
                if (SetProperty(ref archivedClosedPositionSummary, value))
                {
                    this.SelectedClosedProfit = value?.ClosedProfits?.FirstOrDefault();
                }
            }
        }

        private ClosedProfit selectedClosedProfit;
        public ClosedProfit SelectedClosedProfit
        {
            get { return selectedClosedProfit; }
            set { SetProperty(ref selectedClosedProfit, value); }
        }
        
        /// <summary>
        /// 买方向的详细持仓项列表
        /// </summary>
        public ObservableCollection<XqTargetDetailPositionDM_Archived> BuyDetailPositionItems { get; private set; }

        /// <summary>
        /// 卖方向的详细持仓项列表
        /// </summary>
        public ObservableCollection<XqTargetDetailPositionDM_Archived> SellDetailPositionItems { get; private set; }

        private ICommand positionItemDetailCmd;
        public ICommand PositionItemDetailCmd
        {
            get { return positionItemDetailCmd; }
            set { SetProperty(ref positionItemDetailCmd, value); }
        }

    }
}
