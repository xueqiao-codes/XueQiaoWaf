using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoWaf.Trade.Interfaces.Services;

namespace XueQiaoWaf.Trade.Modules.Applications.Services
{
    [Export, Export(typeof(IOrderItemsService)), PartCreationPolicy(CreationPolicy.Shared)]
    public class OrderItemsService : IOrderItemsService
    {
        public OrderItemsService()
        {
            OrderItems = new ObservableCollection<OrderItemDataModel>();
        }

        /// <summary>
        /// 订单列表
        /// </summary>
        public ObservableCollection<OrderItemDataModel> OrderItems { get; private set; }
    }
}
