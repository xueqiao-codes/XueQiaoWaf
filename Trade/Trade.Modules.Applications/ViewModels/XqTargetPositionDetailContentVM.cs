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
    public class XqTargetPositionDetailContentVM : ViewModel<IXqTargetPositionDetailContentView>
    {
        [ImportingConstructor]
        protected XqTargetPositionDetailContentVM(IXqTargetPositionDetailContentView view) : base(view)
        {
            BuyDetailPositionItems = new ObservableCollection<XqTargetDetailPositionDM_Unarchived>();
            SellDetailPositionItems = new ObservableCollection<XqTargetDetailPositionDM_Unarchived>();
        }

        public void UpdateXqTargetType(ClientXQOrderTargetType xqTargetType)
        {
            ViewCore.UpdatePositionColumnsByPresentTarget(xqTargetType);
        }

        private StatClosedPositionDateSummary unarchivedClosedPositionSummary;
        /// <summary>
        /// 未归档配对概要
        /// </summary>
        public StatClosedPositionDateSummary UnarchivedClosedPositionSummary
        {
            get { return unarchivedClosedPositionSummary; }
            set
            {
                if (SetProperty(ref unarchivedClosedPositionSummary, value))
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
        public ObservableCollection<XqTargetDetailPositionDM_Unarchived> BuyDetailPositionItems { get; private set; }

        /// <summary>
        /// 卖方向的详细持仓项列表
        /// </summary>
        public ObservableCollection<XqTargetDetailPositionDM_Unarchived> SellDetailPositionItems { get; private set; }

        private ICommand positionItemDetailCmd;
        public ICommand PositionItemDetailCmd
        {
            get { return positionItemDetailCmd; }
            set { SetProperty(ref positionItemDetailCmd, value); }
        }
        
        private ICommand toRecoverUnarchivedClosedPositionCmd;
        /// <summary>
        /// 恢复未归档配对 command
        /// </summary>
        public ICommand ToRecoverUnarchivedClosedPositionCmd
        {
            get { return toRecoverUnarchivedClosedPositionCmd; }
            set { SetProperty(ref toRecoverUnarchivedClosedPositionCmd, value); }
        }

        private ICommand toQuickClosePositionCmd;
        /// <summary>
        /// 一键配对 command
        /// </summary>
        public ICommand ToQuickClosePositionCmd
        {
            get { return toQuickClosePositionCmd; }
            set { SetProperty(ref toQuickClosePositionCmd, value); }
        }
    }
}
