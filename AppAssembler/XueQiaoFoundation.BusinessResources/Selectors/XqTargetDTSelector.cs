using NativeModel.Trade;
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
    public class XqTargetDTSelector : DataTemplateSelector
    {
        // 合约标的类型的订单数据模板
        public DataTemplate ContractTargetDT { get; set; }

        // 组合标的类型的订单数据模板
        public DataTemplate ComposeTargetDT { get; set; }

        /// <summary>
        /// 选择模板方法
        /// </summary>
        /// <param name="item">必须为<see cref="XueQiaoFoundation.BusinessResources.DataModels.IXqTargetDM"/>类型的数据</param>
        /// <param name="container"></param>
        /// <returns></returns>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;

            var dataItem = item as IXqTargetDM;
            if (dataItem == null) return null;

            if (dataItem.TargetType == ClientXQOrderTargetType.CONTRACT_TARGET)
                return this.ContractTargetDT;
            else if (dataItem.TargetType == ClientXQOrderTargetType.COMPOSE_TARGET)
                return this.ComposeTargetDT;

            return null;
        }
    }
}
