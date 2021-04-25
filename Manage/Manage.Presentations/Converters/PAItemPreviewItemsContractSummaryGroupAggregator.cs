using Manage.Applications.DataModels;
using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Manage.Presentations.Converters
{
    /// <summary>
    /// <see cref="PAItemPreviewItem"/> 数据列表以合约分组的聚合转化器。转化为<see cref="PAItemPreviewItemsContractSummary"/>
    /// </summary>
    public class PAItemPreviewItemsContractSummaryGroupAggregator : IValueConverter
    {
        public object Convert(object value, Type type, object arg, CultureInfo culture)
        {
            var groupData = value as CollectionViewGroup;
            if (groupData != null)
            {
                var items = groupData.Items.Cast<PAItemPreviewItem>();
                if (items.Any())
                {
                    int buyTotalVolume = 0, sellTotalVolume = 0;
                    foreach (var i in items)
                    {
                        if (i.Direction == ClientTradeDirection.BUY)
                            buyTotalVolume += i.Volume;
                        else
                            sellTotalVolume += i.Volume;
                    }
                    var firstItem = items.First();
                    return new PAItemPreviewItemsContractSummary(firstItem.ContractId)
                    {
                        ContractDetailContainer = firstItem.ContractDetailContainer,
                        BuyTotalVolume = buyTotalVolume,
                        SellTotalVolume = sellTotalVolume
                    };
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type type, object arg, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
