using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;

namespace XueQiaoWaf.Trade.Modules.Presentation.Selector
{
    /// <summary>
    /// <see cref="CPMergePriceInputItem"/>的数据模板选择器
    /// </summary>
    public class CPMergePriceInputItemDTS : DataTemplateSelector
    {
        /// <summary>
        /// <see cref="CPMergePriceInputItemType.PriceDiffInputItem"/>类型的数据模板
        /// </summary>
        public DataTemplate DataTemplate_PriceDiff { get; set; }

        /// <summary>
        /// <see cref="CPMergePriceInputItemType.LegPriceInputItem"/>类型的数据模板
        /// </summary>
        public DataTemplate DataTemplate_LegPrice { get; set; }

        /// <summary>
        /// 选择模板方法
        /// </summary>
        /// <param name="item">必须为<see cref="CPMergePriceInputItem"/>类型的数据</param>
        /// <param name="container"></param>
        /// <returns></returns>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var tarItem = item as CPMergePriceInputItem;
            if (tarItem == null) return null;

            if (tarItem.ItemType == CPMergePriceInputItemType.PriceDiffInputItem)
                return this.DataTemplate_PriceDiff;
            else if (tarItem.ItemType == CPMergePriceInputItemType.LegPriceInputItem)
                return this.DataTemplate_LegPrice;

            return null;
        }
    }
}
