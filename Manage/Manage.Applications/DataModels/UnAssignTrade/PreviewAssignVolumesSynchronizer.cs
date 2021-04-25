using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.Shared.Helper;

namespace Manage.Applications.DataModels
{
    /// <summary>
    /// 分配数量同步器
    /// </summary>
    public class PreviewAssignVolumesSynchronizer
    {
        private readonly UnAssignTradeDM unAssignTradeItem;

        public PreviewAssignVolumesSynchronizer(UnAssignTradeDM unAssignTradeItem)
        {
            if (unAssignTradeItem == null) throw new ArgumentNullException("unAssignTradeItem");

            this.unAssignTradeItem = unAssignTradeItem;
            CollectionChangedEventManager.AddHandler(unAssignTradeItem.PAItems, PAItemsCollectionChanged);
            PropertyChangedEventManager.AddHandler(unAssignTradeItem, UATItemPropChanged, "");

            InvalidateUATItemVolumeSummaryPAVolumes();
        }

        private void UATItemPropChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(UnAssignTradeDM.Volume))
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    InvalidateUATItemVolumeSummaryPAVolumes();
                });
            }
        }

        private void PAItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var newItems = e.NewItems?.Cast<PositionPreviewAssignDM>().ToArray();
            var oldItems = e.OldItems?.Cast<PositionPreviewAssignDM>().ToArray();

            if (newItems?.Any() == true)
            {
                foreach (var newItem in newItems)
                {
                    PropertyChangedEventManager.RemoveHandler(newItem, PAItemPropChanged, "");
                    PropertyChangedEventManager.AddHandler(newItem, PAItemPropChanged, "");
                }
            }

            if (oldItems?.Any() == true)
            {
                foreach (var oldItem in oldItems)
                {
                    PropertyChangedEventManager.RemoveHandler(oldItem, PAItemPropChanged, "");
                }
            }

            InvalidateUATItemVolumeSummaryPAVolumes();
        }

        private void PAItemPropChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PositionPreviewAssignDM.Volume))
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    var item = sender as PositionPreviewAssignDM;
                    if (item != null && this.unAssignTradeItem.PAItems.Contains(item))
                    {
                        InvalidateUATItemVolumeSummaryPAVolumes();
                    }
                });
            }
        }

        private void InvalidateUATItemVolumeSummaryPAVolumes()
        {
            var _pAssignVolume = this.unAssignTradeItem.PAItems.Sum(i => i.Volume);
            this.unAssignTradeItem.PreviewAssignVolume = _pAssignVolume;
            this.unAssignTradeItem.UnpreviewAssignVolume = this.unAssignTradeItem.Volume - this.unAssignTradeItem.PreviewAssignVolume;
        }
    }
}
