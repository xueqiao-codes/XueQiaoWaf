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
    /// 未分配持仓的已预分配列表视图 model
    /// </summary>
    public class UATPreviewAssignItemsVM : Model
    {
        private readonly UATPAService UATPAService;

        /// <summary>
        /// 是否忽略改变 <see cref="IsBatchRemovePAItemsEnabled"/>
        /// </summary>
        private bool __isIgnoreInvalidateIsBatchRemovePAItemsEnabled;

        public UATPreviewAssignItemsVM(UATPAService UATPAService)
        {
            this.UATPAService = UATPAService;
            var synchPAOptItems = new SynchronizingCollection<PAItemCheckItem, PositionPreviewAssignDM>(UATPAService.PAItems, i => new PAItemCheckItem(i));
            PAOptItemsCollectionView = CollectionViewSource.GetDefaultView(synchPAOptItems) as ListCollectionView;

            BatchRemovePAItemsCmd = new DelegateCommand(BatchRemovePAItems, CanBatchRemovePAItems);
            PAOptItemCheckedCmd = new DelegateCommand(PAOptItemChecked);
            PAOptItemUncheckedCmd = new DelegateCommand(PAOptItemUnchecked);
            RemovePAItemCmd = new DelegateCommand(RemovePAItem);

            ConfigPAOptItemsCollectionView();
            RefreshPAOptItemsCollectionView();
            IsCheckedBatchAllPAOptItems = false;
        }

        public long? FilterFundAccountId { get; private set; }

        public void UpdateFilterFundAccountId(long? filterFundAccountId)
        {
            if (this.FilterFundAccountId == filterFundAccountId) return;
            this.FilterFundAccountId = filterFundAccountId;

            this.IsCheckedBatchAllPAOptItems = false;
            RefreshPAOptItemsCollectionView();
        }

        public int? FilterContractId { get; private set; }

        public void UpdateFilterContractId(int? filterContractId)
        {
            if (this.FilterContractId == filterContractId) return;
            this.FilterContractId = filterContractId;

            this.IsCheckedBatchAllPAOptItems = false;
            RefreshPAOptItemsCollectionView();
        }

        /// <summary>
        ///移除批量预分配操作
        /// </summary>
        public Action<TriggerBatchRemovePAItemsArgs> TriggerBatchRemovePAItemsHandler { get; set; }

        /// <summary>
        ///移除单个预分配操作
        /// </summary>
        public Action<TriggerSingleRemovePAItemArgs> TriggerSingleRemovePAItemHandler { get; set; }

        // 未预分配的已分配持仓项列表
        public ListCollectionView PAOptItemsCollectionView { get; private set; }

        // 批量删除选中的已预分配项 command
        public DelegateCommand BatchRemovePAItemsCmd { get; private set; }

        // 预分配项选中的 command
        public DelegateCommand PAOptItemCheckedCmd { get; private set; }

        // 预分配项不选中的 command
        public DelegateCommand PAOptItemUncheckedCmd { get; private set; }

        // 移除某个预分配 command
        public DelegateCommand RemovePAItemCmd { get; private set; }

        private bool isCheckedBatchAllPAOptItems;
        /// <summary>
        /// 是否勾选批量选中所有预分配项
        /// </summary>
        public bool IsCheckedBatchAllPAOptItems
        {
            get { return isCheckedBatchAllPAOptItems; }
            set
            {
                if (SetProperty(ref isCheckedBatchAllPAOptItems, value))
                {
                    this.__isIgnoreInvalidateIsBatchRemovePAItemsEnabled = true;
                    
                    var currentUPAOptItems = PAOptItemsCollectionView?.OfType<PAItemCheckItem>().ToArray();
                    if (currentUPAOptItems?.Any() == true)
                    {
                        foreach (var optItem in currentUPAOptItems)
                            optItem.IsChecked = value;
                    }

                    this.__isIgnoreInvalidateIsBatchRemovePAItemsEnabled = false;
                    InvalidateIsBatchRemovePAItemsEnabled();
                }
            }
        }

        private bool isBatchRemovePAItemsEnabled;
        // 是否可批量删除选中的未成交项的预分配
        private bool IsBatchRemovePAItemsEnabled
        {
            get { return isBatchRemovePAItemsEnabled; }
            set
            {
                if (SetProperty(ref isBatchRemovePAItemsEnabled, value))
                {
                    BatchRemovePAItemsCmd?.RaiseCanExecuteChanged();
                }
            }
        }

        private bool CanBatchRemovePAItems(object triggerElement)
        {
            return IsBatchRemovePAItemsEnabled;
        }

        private void BatchRemovePAItems(object triggerElement)
        {
            // 获取选中的项
            var checkedPAOptItems = PAOptItemsCollectionView.OfType<PAItemCheckItem>()
                .Where(i => i.IsChecked).ToArray();
            if (checkedPAOptItems?.Any() == true)
            {
                var args = new TriggerBatchRemovePAItemsArgs
                {
                    TriggerElement = triggerElement,
                    PAItems = checkedPAOptItems.Select(i => i.PAItem).ToArray()
                };
                TriggerBatchRemovePAItemsHandler?.Invoke(args);
                if (args.Handled)
                {
                    this.__isIgnoreInvalidateIsBatchRemovePAItemsEnabled = true;

                    foreach (var optPAItem in checkedPAOptItems)
                    {
                        optPAItem.IsChecked = false;
                    }

                    this.__isIgnoreInvalidateIsBatchRemovePAItemsEnabled = false;
                    InvalidateIsBatchRemovePAItemsEnabled();
                }
            }
        }

        private void PAOptItemChecked(object param)
        {
            var item = param as PAItemCheckItem;
            if (item == null) return;

            item.IsChecked = true;
            InvalidateIsBatchRemovePAItemsEnabled();
        }

        private void PAOptItemUnchecked(object param)
        {
            var item = param as PAItemCheckItem;
            if (item == null) return;

            item.IsChecked = false;
            InvalidateIsBatchRemovePAItemsEnabled();
        }

        private void RemovePAItem(object param)
        {
            var args = param as object[];
            if (args?.Length != 2) return;
            var optPAItem = args[0] as PAItemCheckItem;
            var triggerElement = args[1];
            
            if (optPAItem == null) return;

            var rmArgs = new TriggerSingleRemovePAItemArgs { TriggerElement = triggerElement, PAItem = optPAItem.PAItem };
            TriggerSingleRemovePAItemHandler?.Invoke(rmArgs);
            if (rmArgs.Handled)
            {
                optPAItem.IsChecked = false;
            }
        }

        private void InvalidateIsBatchRemovePAItemsEnabled()
        {
            if (this.__isIgnoreInvalidateIsBatchRemovePAItemsEnabled) return;
            this.IsBatchRemovePAItemsEnabled = PAOptItemsCollectionView?.OfType<PAItemCheckItem>().Any(i => i.IsChecked) ?? false;
        }

        private void ConfigPAOptItemsCollectionView()
        {
            var listView = PAOptItemsCollectionView;
            if (listView == null) return;

            listView.SortDescriptions.Add(new SortDescription(nameof(PAItemCheckItem.UATItemTradeTimestampMs), ListSortDirection.Ascending));
            listView.SortDescriptions.Add(new SortDescription(nameof(PAItemCheckItem.ContractId), ListSortDirection.Ascending));
            listView.SortDescriptions.Add(new SortDescription(nameof(PAItemCheckItem.UATItemKey), ListSortDirection.Ascending));

            listView.Filter = i =>
            {
                var item = i as PAItemCheckItem;
                if (item == null) return false;
                return item.PAItem.FundAccountId == this.FilterFundAccountId
                    && item.PAItem.ContractId == this.FilterContractId;
            };
            listView.IsLiveFiltering = true;
        }

        private void RefreshPAOptItemsCollectionView()
        {
            var listView = PAOptItemsCollectionView;
            if (listView == null) return;
            listView.Refresh();
        }
    }
}
