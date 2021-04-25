using Manage.Applications.DataModels;
using Manage.Applications.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Waf.Foundation;
using System.Windows.Data;

namespace Manage.Applications.ViewModels
{
    /// <summary>
    /// 未分配持仓的未预分配列表视图 model
    /// </summary>
    public class UATUnpreviewAssignItemsVM : Model
    {
        private readonly UATPAService UATPAService;

        /// <summary>
        /// 是否忽略改变 <see cref="IsBatchPreviewAssignEnabled"/>
        /// </summary>
        private bool __isIgnoreInvalidateIsBatchPreviewAssignEnabled;

        public UATUnpreviewAssignItemsVM(UATPAService UATPAService)
        {
            this.UATPAService = UATPAService;
            var synchUATOptItems = new SynchronizingCollection<UATItemCheckItem, UnAssignTradeDM>(UATPAService.UATItems, i => new UATItemCheckItem(i));
            UPAOptItemsCollectionView = CollectionViewSource.GetDefaultView(synchUATOptItems) as ListCollectionView;

            BatchPreviewAssignCmd = new DelegateCommand(BatchPreviewAssign, CanBatchPreviewAssign);
            UPAOptItemCheckedCmd = new DelegateCommand(UPAOptItemChecked);
            UPAOptItemUncheckedCmd = new DelegateCommand(UPAOptItemUnchecked);
            PreviewAssignUATItemCmd = new DelegateCommand(PreviewAssignUATItem);

            InitUPAOptItemsCollectionView();
            RefreshUPAOptItemsCollectionView();
            IsCheckedBatchAllUPAOptItems = false;
        }
        
        public long? FilterFundAccountId { get; private set; }

        public void UpdateFilterFundAccountId(long? filterFundAccountId)
        {
            if (this.FilterFundAccountId == filterFundAccountId) return;
            this.FilterFundAccountId = filterFundAccountId;

            this.IsCheckedBatchAllUPAOptItems = false;
            RefreshUPAOptItemsCollectionView();
        }

        public int? FilterContractId { get; private set; }

        public void UpdateFilterContractId(int? filterContractId)
        {
            if (this.FilterContractId == filterContractId) return;
            this.FilterContractId = filterContractId;

            this.IsCheckedBatchAllUPAOptItems = false;
            RefreshUPAOptItemsCollectionView();
        }

        /// <summary>
        /// 批量预分配操作
        /// </summary>
        public Action<TriggerBatchPreviewAssignArgs> TriggerBatchPreviewAssignHandler { get; set; }

        /// <summary>
        /// 预分配单个未分配的操作
        /// </summary>
        public Action<TriggerSingleUATItemPreviewAssignArgs> TriggerSingleUATItemPreviewAssignHandler { get; set; }

        // 未预分配的未分配持仓项列表
        public ListCollectionView UPAOptItemsCollectionView { get; private set; }

        // 批量预分配选中的未成交项 command
        public DelegateCommand BatchPreviewAssignCmd { get; private set; }

        // 未预分配项选中的 command
        public DelegateCommand UPAOptItemCheckedCmd { get; private set; }

        // 未预分配项不选中的 command
        public DelegateCommand UPAOptItemUncheckedCmd { get; private set; }
        
        // 预分配某个未分配项 command
        public DelegateCommand PreviewAssignUATItemCmd { get; private set; }

        private bool isCheckedBatchAllUPAOptItems;
        /// <summary>
        /// 是否勾选批量选中所有未预分配项
        /// </summary>
        public bool IsCheckedBatchAllUPAOptItems
        {
            get { return isCheckedBatchAllUPAOptItems; }
            set
            {
                if (SetProperty(ref isCheckedBatchAllUPAOptItems, value))
                {
                    this.__isIgnoreInvalidateIsBatchPreviewAssignEnabled = true;

                    var currentUPAOptItems = UPAOptItemsCollectionView?.OfType<UATItemCheckItem>().ToArray();
                    if (currentUPAOptItems?.Any() == true)
                    {
                        foreach (var optItem in currentUPAOptItems)
                            optItem.IsChecked = value;
                    }

                    this.__isIgnoreInvalidateIsBatchPreviewAssignEnabled = false;
                    InvalidateIsBatchPreviewAssignEnabled();
                }
            }
        }

        private bool isBatchPreviewAssignEnabled;
        // 是否可批量预分配选中的未成交项
        private bool IsBatchPreviewAssignEnabled
        {
            get { return isBatchPreviewAssignEnabled; }
            set
            {
                if (SetProperty(ref isBatchPreviewAssignEnabled, value))
                {
                    BatchPreviewAssignCmd?.RaiseCanExecuteChanged();
                }
            }
        }
        
        private bool CanBatchPreviewAssign(object triggerElement)
        {
            return IsBatchPreviewAssignEnabled;
        }

        private void BatchPreviewAssign(object triggerElemant)
        {
            // 获取选中的项
            var checkedOptUATItems = UPAOptItemsCollectionView.OfType<UATItemCheckItem>()
                .Where(i => i.IsChecked).ToArray();
            if (checkedOptUATItems?.Any() == true)
            {
                var handlerArgs = new TriggerBatchPreviewAssignArgs
                {
                    TriggerElement = triggerElemant,
                    UATItems = checkedOptUATItems.Select(i => i.UATItem).ToArray()
                };
                TriggerBatchPreviewAssignHandler?.Invoke(handlerArgs);

                if (handlerArgs.Handled)
                {
                    this.__isIgnoreInvalidateIsBatchPreviewAssignEnabled = true;

                    foreach (var optItem in checkedOptUATItems)
                    {
                        optItem.IsChecked = false;
                    }

                    this.__isIgnoreInvalidateIsBatchPreviewAssignEnabled = false;
                    InvalidateIsBatchPreviewAssignEnabled();
                }
            }
        }

        private void UPAOptItemChecked(object param)
        {
            var item = param as UATItemCheckItem;
            if (item == null) return;

            item.IsChecked = true;
            InvalidateIsBatchPreviewAssignEnabled();
        }

        private void UPAOptItemUnchecked(object param)
        {
            var item = param as UATItemCheckItem;
            if (item == null) return;

            item.IsChecked = false;
            InvalidateIsBatchPreviewAssignEnabled();
        }

        private void PreviewAssignUATItem(object param)
        {
            var cmdArgs = param as object[];
            if (cmdArgs?.Length != 2) return;
            var optUATItem = cmdArgs[0] as UATItemCheckItem;
            var triggerElement = cmdArgs[1];

            if (optUATItem == null) return;

            var handlerArgs = new TriggerSingleUATItemPreviewAssignArgs { TriggerElement = triggerElement, UATItem = optUATItem.UATItem };
            TriggerSingleUATItemPreviewAssignHandler?.Invoke(handlerArgs);
            if (handlerArgs.Handled)
            {
                optUATItem.IsChecked = false;
            }
        }

        private void InvalidateIsBatchPreviewAssignEnabled()
        {
            if (this.__isIgnoreInvalidateIsBatchPreviewAssignEnabled) return;
            this.IsBatchPreviewAssignEnabled = UPAOptItemsCollectionView?.OfType<UATItemCheckItem>().Any(i => i.IsChecked) ?? false;
        }

        private void InitUPAOptItemsCollectionView()
        {
            var listView = UPAOptItemsCollectionView;
            if (listView == null) return;
            
            listView.SortDescriptions.Add(new SortDescription(nameof(UATItemCheckItem.TradeTimestampMs), ListSortDirection.Ascending));
            listView.SortDescriptions.Add(new SortDescription(nameof(UATItemCheckItem.ContractId), ListSortDirection.Ascending));
            listView.SortDescriptions.Add(new SortDescription(nameof(UATItemCheckItem.UATItemKey), ListSortDirection.Ascending));


            listView.Filter = i =>
            {
                var item = i as UATItemCheckItem;
                if (item == null) return false;
                return item.UATItem.FundAccountId == this.FilterFundAccountId
                    && item.UATItem.ContractId == this.FilterContractId
                    && item.UnpreviewAssignVolume > 0;
            };
            listView.LiveFilteringProperties.Add(nameof(UATItemCheckItem.UnpreviewAssignVolume));
            listView.IsLiveFiltering = true;
        }

        private void RefreshUPAOptItemsCollectionView()
        {
            var listView = UPAOptItemsCollectionView;
            if (listView == null) return;
            listView.Refresh();
        }
    }
}
