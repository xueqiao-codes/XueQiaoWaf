using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoWaf.Trade.Modules.Applications.DataModels
{
    /// <summary>
    /// 组合持仓合并腿信息同步器
    /// </summary>
    public class CPMergeLegInfoSynchronizer
    {
        public CPMergeLegInfoSynchronizer(CPMergeLegInfoItem mergeLegInfoItem, ObservableCollection<XqTargetDetailPositionDM_MergeEdit> legPositionMergeEditItems)
        {
            Debug.Assert(mergeLegInfoItem != null);
            Debug.Assert(legPositionMergeEditItems != null);
            this.MergeLegInfoItem = mergeLegInfoItem;
            this.LegPositionMergeEditItems = legPositionMergeEditItems;

            foreach (var mergeEditItem in legPositionMergeEditItems)
            {
                PropertyChangedEventManager.AddHandler(mergeEditItem, LegPositionMergeEditItemPropChanged, "");
            }

            CollectionChangedEventManager.AddHandler(legPositionMergeEditItems, LegPositionMergeEditItemsChanged);
            PropertyChangedEventManager.AddHandler(mergeLegInfoItem, MergeLegInfoItemPropChanged, "");

            InvalidateCPMergeLegInfo();
            InvalidateLegPositionMergeEditItemsEditInfos();
        }

        public CPMergeLegInfoItem MergeLegInfoItem { get; private set; }

        public ObservableCollection<XqTargetDetailPositionDM_MergeEdit> LegPositionMergeEditItems { get; private set; }

        private void LegPositionMergeEditItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var oldItems = e.OldItems?.OfType<XqTargetDetailPositionDM_MergeEdit>().ToArray();
            var newItems = e.NewItems?.OfType<XqTargetDetailPositionDM_MergeEdit>().ToArray();
            if (oldItems?.Any() == true)
            {
                foreach (var item in oldItems)
                {
                    PropertyChangedEventManager.RemoveHandler(item, LegPositionMergeEditItemPropChanged, "");
                }
            }

            if (newItems?.Any() == true)
            {
                foreach (var item in newItems)
                {
                    PropertyChangedEventManager.RemoveHandler(item, LegPositionMergeEditItemPropChanged, "");
                    PropertyChangedEventManager.AddHandler(item, LegPositionMergeEditItemPropChanged, "");
                }
            }

            InvalidateCPMergeLegInfo();
            InvalidateLegPositionMergeEditItemsEditInfos();
        }

        private void LegPositionMergeEditItemPropChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(XqTargetDetailPositionDM_MergeEdit.Price))
            {
                InvalidateCPMergeLegInfo();
            }

            if (e.PropertyName == nameof(XqTargetDetailPositionDM_MergeEdit.InputQuantity))
            {
                InvalidateCPMergeLegInfo();
                InvalidateLegPositionMergeEditItemsEditInfos();
            }
        }

        private void MergeLegInfoItemPropChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CPMergeLegInfoItem.DemandVolume))
            {
                var item = sender as CPMergeLegInfoItem;
                MergeLegInfoItem_DemandVolumeChanged(item);
            }
        }
        
        private void MergeLegInfoItem_DemandVolumeChanged(CPMergeLegInfoItem item)
        {
            if (item == null) return;

            var demandVolume = item.DemandVolume;
            var currentVolume = item.CurrentVolume;
            if (demandVolume < currentVolume)
            {
                // 需求量小于当前选择量时，直接设置所有的输入项的输入数量为 0
                var tarEditItems = LegPositionMergeEditItems.Where(i => i.InputQuantity != 0).ToArray();
                if (tarEditItems.Any())
                {
                    foreach (var editItem in tarEditItems)
                    {
                        PropertyChangedEventManager.RemoveHandler(editItem, LegPositionMergeEditItemPropChanged, "");
                        editItem.InputQuantity = 0;
                        PropertyChangedEventManager.AddHandler(editItem, LegPositionMergeEditItemPropChanged, "");
                    }
                    InvalidateCPMergeLegInfo();
                }
            }

            InvalidateLegPositionMergeEditItemsEditInfos();
        }

        private void InvalidateCPMergeLegInfo()
        {
            var tarEditItems = LegPositionMergeEditItems.Where(i => i.InputQuantity > 0).ToArray();
            var currentVolume = tarEditItems.Sum(i => i.InputQuantity);

            MergeLegInfoItem.CurrentVolume = currentVolume;
            if (currentVolume > 0)
            {
                MergeLegInfoItem.CurrentAvgPrice = tarEditItems.Sum(i => i.InputQuantity * i.Price) / currentVolume;
            }
            else
            {
                MergeLegInfoItem.CurrentAvgPrice = null;
            }
        }

        private void InvalidateLegPositionMergeEditItemsEditInfos()
        {
            var mergeEditItems = this.LegPositionMergeEditItems;
            var currentChooseTotalVolume = MergeLegInfoItem.CurrentVolume;
            var availableVolume = Math.Min(MergeLegInfoItem.DemandVolume, mergeEditItems.Sum(i=>i.Quantity));
            if (currentChooseTotalVolume <= availableVolume)
            {
                foreach (var editItem in mergeEditItems)
                {
                    PropertyChangedEventManager.RemoveHandler(editItem, LegPositionMergeEditItemPropChanged, "");

                    editItem.MaximumInputQuantity = Math.Min(editItem.InputQuantity + (availableVolume - currentChooseTotalVolume), editItem.Quantity);
                    editItem.InputQuantity = Math.Min(editItem.InputQuantity, editItem.MaximumInputQuantity);

                    PropertyChangedEventManager.AddHandler(editItem, LegPositionMergeEditItemPropChanged, "");
                }
            }
        }
    }
}
