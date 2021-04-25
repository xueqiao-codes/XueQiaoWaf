using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using XueQiaoFoundation.BusinessResources.DataModels;

namespace XueQiaoFoundation.BusinessResources.Selectors
{
    /// <summary>
    /// 传入<see cref="OrderItemDataModel"/>，然后根据订单类型<see cref="XQClientOrderType"/>决定 data template 选择器
    /// </summary>
    public class OrderItemDTSelectorByClientOrderType : DataTemplateSelector
    {
        // `委托单`类型的订单数据模板
        public DataTemplate EntrustedOrderDT { get; set; }

        // `条件单`类型的订单数据模板
        public DataTemplate ConditionOrderDT { get; set; }

        // `预埋单`类型的订单数据模板
        public DataTemplate ParkedOrderDT { get; set; }


        /// <summary>
        /// 选择模板方法
        /// </summary>
        /// <param name="item">必须为<see cref="XueQiaoFoundation.BusinessResources.DataModels.OrderItemDataModel"/>类型的数据</param>
        /// <param name="container"></param>
        /// <returns></returns>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;
            var dataItem = item as OrderItemDataModel;
            if (dataItem == null) return null;

            if (dataItem.ClientOrderType == XQClientOrderType.Entrusted)
                return this.EntrustedOrderDT;
            else if (dataItem.ClientOrderType == XQClientOrderType.Condition)
                return this.ConditionOrderDT;
            else if (dataItem.ClientOrderType == XQClientOrderType.Parked)
                return this.ParkedOrderDT;

            return null;
        }
    }
}
