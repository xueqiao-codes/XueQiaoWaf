using Manage.Applications.DataModels;
using Manage.Applications.Services;
using Manage.Applications.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Data;
using xueqiao.trade.hosting;

namespace Manage.Applications.ViewModels
{
    /// <summary>
    /// 触发进行批量预分配的参数
    /// </summary>
    public class TriggerBatchPreviewAssignArgs
    {
        public object TriggerElement { get; set; }
        public IEnumerable<UnAssignTradeDM> UATItems { get; set; }
        // 是否处理
        public bool Handled { get; set; }
    }

    /// <summary>
    /// 触发进行单项预分配的参数
    /// </summary>
    public class TriggerSingleUATItemPreviewAssignArgs
    {
        public object TriggerElement { get; set; }
        public UnAssignTradeDM UATItem { get; set; }
        // 是否处理
        public bool Handled { get; set; }
    }

    /// <summary>
    /// 触发批量删除预分配的参数
    /// </summary>
    public class TriggerBatchRemovePAItemsArgs
    {
        public object TriggerElement { get; set; }
        public IEnumerable<PositionPreviewAssignDM> PAItems { get; set; }
        // 是否处理
        public bool Handled { get; set; }
    }

    /// <summary>
    /// 触发删除单项预分配的参数
    /// </summary>
    public class TriggerSingleRemovePAItemArgs
    {
        public object TriggerElement { get; set; }
        public PositionPreviewAssignDM PAItem { get; set; }
        // 是否处理
        public bool Handled { get; set; }
    }

    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class UATAssignTabContentVM : ViewModel<IUATAssignTabContentView>
    {
        private readonly ManageFundAccountItemsService manFundAccountItemsService;
        private readonly UATPAContractSummaryService UATPAContractSummaryService;
        private readonly UATPAService UATPAService;
        
        [ImportingConstructor]
        protected UATAssignTabContentVM(IUATAssignTabContentView view,
            ManageFundAccountItemsService manFundAccountItemsService,
            UATPAContractSummaryService UATPAContractSummaryService,
            UATPAService UATPAService) : base(view)
        {
            this.manFundAccountItemsService = manFundAccountItemsService;
            this.UATPAContractSummaryService = UATPAContractSummaryService;
            this.UATPAService = UATPAService;

            this.UPAItemsVM = new UATUnpreviewAssignItemsVM(UATPAService)
            {
                TriggerBatchPreviewAssignHandler = _args => this.TriggerBatchPreviewAssignHandler?.Invoke(_args),
                TriggerSingleUATItemPreviewAssignHandler = _args => this.TriggerSingleUATItemPreviewAssignHandler?.Invoke(_args)
            };

            this.PAItemsVM = new UATPreviewAssignItemsVM(UATPAService)
            {
                TriggerBatchRemovePAItemsHandler = _args => this.TriggerBatchRemovePAItemsHandler?.Invoke(_args),
                TriggerSingleRemovePAItemHandler = _args => this.TriggerSingleRemovePAItemHandler?.Invoke(_args)
            };

            var synchUATPAContractSummaries = new SynchronizingCollection<UATContractGroupedSummary, UATContractGroupedSummary>(UATPAContractSummaryService.ContractGroupedSummaries, i => i);
            UATPAContractSummaryCollectionView = CollectionViewSource.GetDefaultView(synchUATPAContractSummaries) as ListCollectionView;

            var synchFundAccountItems = new SynchronizingCollection<HostingTradeAccount, HostingTradeAccount>(manFundAccountItemsService.AccountItems, i=>i);
            FundAccountItemsCollectionView = CollectionViewSource.GetDefaultView(synchFundAccountItems) as ListCollectionView;
            
            InitUATPAContractSummaryCollectionView();
            RefreshUATPAContractSummaryCollectionView();

            CollectionChangedEventManager.AddHandler(UATPAService.UATItems, UATItemsCollectionChanged);

            InitFundAccountItemsCollectionView();
            RefreshFundAccountItemsCollectionView();

            this.SelectedFundAccountId = FundAccountItemsCollectionView.OfType<HostingTradeAccount>().FirstOrDefault()?.TradeAccountId;
        }

        /// <summary>
        /// 批量预分配操作
        /// </summary>
        public Action<TriggerBatchPreviewAssignArgs> TriggerBatchPreviewAssignHandler { get; set; }

        /// <summary>
        /// 预分配单个未分配的操作
        /// </summary>
        public Action<TriggerSingleUATItemPreviewAssignArgs> TriggerSingleUATItemPreviewAssignHandler { get; set; }

        /// <summary>
        ///移除批量预分配操作
        /// </summary>
        public Action<TriggerBatchRemovePAItemsArgs> TriggerBatchRemovePAItemsHandler { get; set; }

        /// <summary>
        ///移除单个预分配操作
        /// </summary>
        public Action<TriggerSingleRemovePAItemArgs> TriggerSingleRemovePAItemHandler { get; set; }

        /// <summary>
        /// 未分配区域视图 model
        /// </summary>
        public UATUnpreviewAssignItemsVM UPAItemsVM { get; private set; }

        /// <summary>
        /// 已分配区域视图 model
        /// </summary>
        public UATPreviewAssignItemsVM PAItemsVM { get; private set; }

        // 以合约分组的预分配概要列表
        public ListCollectionView UATPAContractSummaryCollectionView { get; private set; }
        
        // 资金账户列表
        public ListCollectionView FundAccountItemsCollectionView { get; private set; }
        
        private long? selectedFundAccountId;
        // 选择的资金账户 id
        public long? SelectedFundAccountId
        {
            get { return selectedFundAccountId; }
            set
            {
                if (SetProperty(ref selectedFundAccountId, value))
                {
                    RefreshUATPAContractSummaryCollectionView();
                    UPAItemsVM?.UpdateFilterFundAccountId(selectedFundAccountId);
                    PAItemsVM?.UpdateFilterFundAccountId(selectedFundAccountId);
                }
            }
        }

        private UATContractGroupedSummary selectedUATContractGroupedSummary;
        /// <summary>
        /// 选中的 summary
        /// </summary>
        public UATContractGroupedSummary SelectedUATContractGroupedSummary
        {
            get { return selectedUATContractGroupedSummary; }
            set
            {
                if (SetProperty(ref selectedUATContractGroupedSummary, value))
                {
                    var contractId = selectedUATContractGroupedSummary?.ContractId;
                    UPAItemsVM?.UpdateFilterContractId(contractId);
                    PAItemsVM?.UpdateFilterContractId(contractId);
                }
            }
        }
        
        private void InitUATPAContractSummaryCollectionView()
        {
            var listView = UATPAContractSummaryCollectionView;
            if (listView == null) return;

            listView.SortDescriptions.Add(new SortDescription(nameof(UATContractGroupedSummary.ContractId),
                ListSortDirection.Ascending));

            listView.Filter = i =>
            {
                var item = i as UATContractGroupedSummary;
                if (item == null) return false;
                return item.FundAccountId == this.SelectedFundAccountId;
            };
            listView.LiveFilteringProperties.Add(nameof(UATContractGroupedSummary.FundAccountId));
            listView.IsLiveFiltering = true;
        }
        
        private void RefreshUATPAContractSummaryCollectionView()
        {
            var listView = UATPAContractSummaryCollectionView;
            if (listView == null) return;
            listView.Refresh();
        }

        private void InitFundAccountItemsCollectionView()
        {
            var listView = FundAccountItemsCollectionView;
            if (listView == null) return;

            // 只显示存在未分配成交的资金账户
            listView.Filter = i =>
            {
                var accItem = i as HostingTradeAccount;
                if (accItem == null) return false;
                return UATPAService.UATItems.Any(_UAT => _UAT.FundAccountId == accItem.TradeAccountId);
            };
        }

        private void RefreshFundAccountItemsCollectionView()
        {
            FundAccountItemsCollectionView?.Refresh();
        }

        private void UATItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems?.Count > 0 || e.NewItems?.Count > 0)
            {
                RefreshFundAccountItemsCollectionView();
            }
        }
    }
}
