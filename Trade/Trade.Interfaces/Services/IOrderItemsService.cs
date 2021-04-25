using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.BusinessResources.DataModels;

namespace XueQiaoWaf.Trade.Interfaces.Services
{
    public interface IOrderItemsService
    {
        /// <summary>
        /// 订单列表
        /// </summary>
        ObservableCollection<OrderItemDataModel> OrderItems { get; }
    }
}
