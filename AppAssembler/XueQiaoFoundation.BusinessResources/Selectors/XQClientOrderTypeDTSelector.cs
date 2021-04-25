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
    /// 传入<see cref="XQClientOrderType"/>，并且根据该值决定 data template 选择器
    /// </summary>
    public class XQClientOrderTypeDTSelector : DataTemplateSelector
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
        /// <param name="item">必须为<see cref="XueQiaoFoundation.BusinessResources.DataModels.XQClientOrderType"/>类型的数据</param>
        /// <param name="container"></param>
        /// <returns></returns>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;

            var dataItem = item as XQClientOrderType?;
            if (dataItem == null) return null;

            if (dataItem == XQClientOrderType.Entrusted)
                return this.EntrustedOrderDT;
            else if (dataItem == XQClientOrderType.Condition)
                return this.ConditionOrderDT;
            else if (dataItem == XQClientOrderType.Parked)
                return this.ParkedOrderDT;

            return null;
        }
    }
}
