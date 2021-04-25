using Manage.Applications.DataModels;
using Manage.Applications.Services;
using Manage.Applications.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Data;
using System.Windows.Input;
using xueqiao.trade.hosting;

namespace Manage.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class UATPreviewTabContentVM : ViewModel<IUATPreviewTabContentView>
    {
        private readonly UATPAService UATPAService;

        [ImportingConstructor]
        protected UATPreviewTabContentVM(IUATPreviewTabContentView view,
            UATPAService UATPAService,
            ManageFundAccountItemsService manFundAccountItemsService,
            ManageSubAccountItemsService manSubAccountItemsService) : base(view)
        {
            this.UATPAService = UATPAService;

            var synchFundAccountItems = new SynchronizingCollection<HostingTradeAccount, HostingTradeAccount>(manFundAccountItemsService.AccountItems, i => i);
            FundAccountItemsCollectionView = CollectionViewSource.GetDefaultView(synchFundAccountItems) as ListCollectionView;

            var synchSubAccountItems = new SynchronizingCollection<HostingSubAccount, HostingSubAccount>(manSubAccountItemsService.AccountItems, i => i);
            SubAccountItemsCollectionView = CollectionViewSource.GetDefaultView(synchSubAccountItems) as ListCollectionView;

            SearchedPreviewItems = new ObservableCollection<PAItemPreviewItem>();
            SelectedPAItemsPreviewType = PAItemsPreviewType.ByFundAccount;
            
            CollectionChangedEventManager.AddHandler(UATPAService.UATItems, UATItemsCollectionChanged);
            CollectionChangedEventManager.AddHandler(UATPAService.PAItems, PAItemsCollectionChanged);

            InitFundAccountItemsCollectionView();
            RefreshFundAccountItemsCollectionView();
            InitSubAccountItemsCollectionView();
            RefreshSubAccountItemsCollectionView();

            SelectedFundAccountId = FundAccountItemsCollectionView.OfType<HostingTradeAccount>().FirstOrDefault()?.TradeAccountId;
            SelectedSubAccountId = SubAccountItemsCollectionView.OfType<HostingSubAccount>().FirstOrDefault()?.SubAccountId;
        }

        private PAItemsPreviewType selectedPAItemsPreviewType;
        /// <summary>
        /// 选中的预览方式
        /// </summary>
        public PAItemsPreviewType SelectedPAItemsPreviewType
        {
            get { return selectedPAItemsPreviewType; }
            set { SetProperty(ref selectedPAItemsPreviewType, value); }
        }

        // 资金账户列表
        public ListCollectionView FundAccountItemsCollectionView { get; private set; }

        // 操作账户列表
        public ListCollectionView SubAccountItemsCollectionView { get; private set; }

        private long? selectedFundAccountId;
        public long? SelectedFundAccountId
        {
            get { return selectedFundAccountId; }
            set { SetProperty(ref selectedFundAccountId, value); }
        }

        private long? selectedSubAccountId;
        public long? SelectedSubAccountId
        {
            get { return selectedSubAccountId; }
            set { SetProperty(ref selectedSubAccountId, value); }
        }

        private ICommand refreshSearchPAItemsCmd;
        /// <summary>
        /// 按条件查询预分配项 command
        /// </summary>
        public ICommand RefreshSearchPAItemsCmd
        {
            get { return refreshSearchPAItemsCmd; }
            set { SetProperty(ref refreshSearchPAItemsCmd, value); }
        }

        private ICommand submitCurrentPAItemsCmd;
        /// <summary>
        /// 提交当前浏览的与分配项 command
        /// </summary>
        public ICommand SubmitCurrentPAItemsCmd
        {
            get { return submitCurrentPAItemsCmd; }
            set { SetProperty(ref submitCurrentPAItemsCmd, value); }
        }

        /// <summary>
        /// 预分配预览项列表
        /// </summary>
        public ObservableCollection<PAItemPreviewItem> SearchedPreviewItems { get; private set; }
        

        private void InitFundAccountItemsCollectionView()
        {
            var listView = FundAccountItemsCollectionView;
            if (listView == null) return;

            // 只显示存在预分配的资金账户
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

        private void InitSubAccountItemsCollectionView()
        {
            var listView = SubAccountItemsCollectionView;
            if (listView == null) return;

            // 只显示存在预分配的资金账户
            listView.Filter = i =>
            {
                var accItem = i as HostingSubAccount;
                if (accItem == null) return false;
                return UATPAService.PAItems.Any(_PA => _PA.SubAccountId == accItem.SubAccountId);
            };
        }

        private void RefreshSubAccountItemsCollectionView()
        {
            SubAccountItemsCollectionView?.Refresh();
        }

        private void UATItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems?.Count > 0 || e.NewItems?.Count > 0)
            {
                RefreshFundAccountItemsCollectionView();
            }
        }

        private void PAItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems?.Count > 0 || e.NewItems?.Count > 0)
            {
                RefreshSubAccountItemsCollectionView();
            }
        }
    }

    /// <summary>
    /// 预分配项预览方式
    /// </summary>
    public enum PAItemsPreviewType
    {
        ByFundAccount = 1,
        BySubAccount  = 2,
    }
}
