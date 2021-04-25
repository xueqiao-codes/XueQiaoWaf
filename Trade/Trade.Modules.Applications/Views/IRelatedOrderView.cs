using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Views
{
    public interface IRelatedOrderView : IView
    {
        /// <summary>
        /// 更新当前订单列表显示列
        /// </summary>
        /// <param name="orderShowType">订单显示类型</param>
        void UpdateCurrentOrderListColumnsWithShowType(RelatedOrderShowType orderShowType);

        /// <summary>
        /// 更新父订单列表显示列
        /// </summary>
        /// <param name="orderShowType">订单显示类型</param>
        void UpdateParentOrderListColumnsWithShowType(RelatedOrderShowType orderShowType);

        /// <summary>
        /// 更新子订单列表显示列
        /// </summary>
        /// <param name="orderShowType">订单显示类型</param>
        void UpdateChildOrderListColumnsWithShowType(RelatedOrderShowType orderShowType);
    }
}
