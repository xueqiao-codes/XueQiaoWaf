using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.BusinessResources.DataModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Views
{
    public interface IPageAvailableOrders
    {
        /// <summary>
        /// 订单页面可用订单过滤器
        /// </summary>
        Predicate<OrderItemDataModel> PageAvailableOrdersFilter { get; set; }
    }
}
