using Manage.Applications.DataModels;
using Manage.Applications.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Data;
using System.Windows.Input;
using xueqiao.trade.hosting.position.adjust.thriftapi;
using XueQiaoFoundation.BusinessResources.DataModels;

namespace Manage.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class PositionDiffOverviewVM : ViewModel<IPositionDiffOverviewView>
    {
        [ImportingConstructor]
        protected PositionDiffOverviewVM(IPositionDiffOverviewView view) : base(view)
        {
            RealtimeDiffItems = new ObservableCollection<SettlementDiffItem>();
            RealtimeDiffCollectionView = CollectionViewSource.GetDefaultView(RealtimeDiffItems) as ListCollectionView;
            ConfigRealtimeDiffCollectionView();

            DailyDiffs = new ObservableCollection<DailyPositionDifference>();
        }

        #region 实时持仓差异对比属性

        /// <summary>
        /// 实时差异项列表
        /// </summary>
        public ObservableCollection<SettlementDiffItem> RealtimeDiffItems { get; private set; }
        public ListCollectionView RealtimeDiffCollectionView { get; private set; }
        
        /// <summary>
        /// 选中的实时差异项
        /// </summary>
        private SettlementDiffItem selectedRealtimeDiffItem;
        public SettlementDiffItem SelectedRealtimeDiffItem
        {
            get { return selectedRealtimeDiffItem; }
            set { SetProperty(ref selectedRealtimeDiffItem, value); }
        }
        
        /// <summary>
        /// 实时差异刷新时间
        /// </summary>
        private long? realtimeDiffRefreshTimestamp;
        public long? RealtimeDiffRefreshTimestamp
        {
            get { return realtimeDiffRefreshTimestamp; }
            set { SetProperty(ref realtimeDiffRefreshTimestamp, value); }
        }

        /// <summary>
        /// 实时差异刷新 command
        /// </summary>
        private ICommand refreshRealtimeDiffCmd;
        public ICommand RefreshRealtimeDiffCmd
        {
            get { return refreshRealtimeDiffCmd; }
            set { SetProperty(ref refreshRealtimeDiffCmd, value); }
        }

        #endregion

        /// <summary>
        /// 触发每日持仓对比选择合约 command
        /// </summary>
        private ICommand triggerDailyDiffSelectedContractCmd;
        public ICommand TriggerDailyDiffSelectedContractCmd
        {
            get { return triggerDailyDiffSelectedContractCmd; }
            set { SetProperty(ref triggerDailyDiffSelectedContractCmd, value); }
        }

        /// <summary>
        /// 刷新每日持仓对比列表 command
        /// </summary>
        private ICommand refreshDailyDiffListCmd;
        public ICommand RefreshDailyDiffListCmd
        {
            get { return refreshDailyDiffListCmd; }
            set { SetProperty(ref refreshDailyDiffListCmd, value); }
        }

        /// <summary>
        /// 每日持仓对比选中的合约
        /// </summary>
        private TargetContract_TargetContractDetail dailyDiffSelectedContractContainer;
        public TargetContract_TargetContractDetail DailyDiffSelectedContractContainer
        {
            get { return dailyDiffSelectedContractContainer; }
            set { SetProperty(ref dailyDiffSelectedContractContainer, value); }
        }

        /// <summary>
        /// 所选合约的每日持仓差异
        /// </summary>
        public ObservableCollection<DailyPositionDifference> DailyDiffs { get; private set; }
        
        /// <summary>
        /// 每日持仓核对 command
        /// </summary>
        private ICommand toVerifyDailyDiffCmd;
        public ICommand ToVerifyDailyDiffCmd
        {
            get { return toVerifyDailyDiffCmd; }
            set { SetProperty(ref toVerifyDailyDiffCmd, value); }
        }
        
        private void ConfigRealtimeDiffCollectionView()
        {
            var collectionView = RealtimeDiffCollectionView;
            if (collectionView == null) return;
            collectionView.SortDescriptions.Add(new SortDescription(nameof(SettlementDiffItem.NetPositionDiff), ListSortDirection.Descending));
        }
    }
}
