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
    /// <see cref="ComposePositionPriceInputItem"/>的数据模板选择器
    /// </summary>
    public class ComposePositionPriceInputItemDTS : DataTemplateSelector
    {
        /// <summary>
        /// <see cref="ComposePositionPriceType.PriceDiff"/>类型的名称模板
        /// </summary>
        public DataTemplate PriceDiffTypeDataTemplate { get; set; }

        /// <summary>
        /// <see cref="ComposePositionPriceType.LegPrice"/>类型的名称模板
        /// </summary>
        public DataTemplate LegPriceTypeDataTemplate { get; set; }

        /// <summary>
        /// 选择模板方法
        /// </summary>
        /// <param name="item">必须为<see cref="ComposePositionPriceInputItem"/>类型的数据</param>
        /// <param name="container"></param>
        /// <returns></returns>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var tarItem = item as ComposePositionPriceInputItem;
            if (tarItem == null) return null;

            if (tarItem.PriceType == ComposePositionPriceType.PriceDiff)
                return this.PriceDiffTypeDataTemplate;
            else if (tarItem.PriceType == ComposePositionPriceType.LegPrice)
                return this.LegPriceTypeDataTemplate;

            return null;
        }
    }
}
