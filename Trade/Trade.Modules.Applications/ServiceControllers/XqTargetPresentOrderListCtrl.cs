using NativeModel.Trade;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.trade.hosting.proxy;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;

using XueQiaoWaf.Trade.Modules.Applications.Services;

namespace XueQiaoWaf.Trade.Modules.Applications.ServiceControllers
{
    public class XqTargetPresentOrderListKey : Tuple<long, ClientXQOrderTargetType, string, XQClientOrderType?>
    {
        public XqTargetPresentOrderListKey(long subAccountId, 
            ClientXQOrderTargetType targetType, string targetKey, XQClientOrderType? clientOrderType) 
            : base(subAccountId, targetType, targetKey, clientOrderType)
        {
            this.SubAccountId = subAccountId;
            this.TargetType = targetType;
            this.TargetKey = targetKey;
            this.ClientOrderType = clientOrderType;
        }

        public long SubAccountId { get; private set; }

        public ClientXQOrderTargetType TargetType { get; private set; }

        public string TargetKey { get; private set; }

        public XQClientOrderType? ClientOrderType { get; private set; }
    }

    /// <summary>
    /// 雪橇标的的订单列表服务控制器
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.Shared)]
    public sealed class XqTargetPresentOrderListCtrl : ITradeModuleSingletonController
    {
        private readonly OrderItemsService orderItemsService;
        private readonly Lazy<ILoginUserManageService> loginUserManageService;
        private readonly IEventAggregator eventAggregator;

        private Dictionary<XqTargetPresentOrderListKey, ObservableCollection<OrderItemDataModel>> presentOrderLists
            = new Dictionary<XqTargetPresentOrderListKey, ObservableCollection<OrderItemDataModel>>();

        [ImportingConstructor]
        public XqTargetPresentOrderListCtrl(OrderItemsService orderItemsService,
            Lazy<ILoginUserManageService> loginUserManageService,
            IEventAggregator eventAggregator)
        {
            this.orderItemsService = orderItemsService;
            this.loginUserManageService = loginUserManageService;
            this.eventAggregator = eventAggregator;

            var orders = orderItemsService.OrderItems.ToArray();
            AddOrderItems(orders);

            CollectionChangedEventManager.AddHandler(orderItemsService.OrderItems, OrderItemsChanged);
            loginUserManageService.Value.HasLogouted += RecvUserHasLogouted;
        }

        public void Shutdown()
        {
            CollectionChangedEventManager.RemoveHandler(orderItemsService.OrderItems, OrderItemsChanged);
            loginUserManageService.Value.HasLogouted -= RecvUserHasLogouted;
            presentOrderLists.Clear();
        }

        /// <summary>
        /// 获取标的的订单展现列表
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ObservableCollection<OrderItemDataModel> TryGetXqTargetPresentOrderList(XqTargetPresentOrderListKey key)
        {
            if (presentOrderLists.TryGetValue(key, out ObservableCollection<OrderItemDataModel> list))
            {
                return list;
            }
            else
            {
                var _list = new ObservableCollection<OrderItemDataModel>();
                presentOrderLists.Add(key, _list);
                return _list;
            }
        }

        private void RecvUserHasLogouted(ProxyLoginResp lastLoginResp)
        {
            presentOrderLists.Clear();
        }

        private void RemoveOrderItems(OrderItemDataModel[] orders)
        {
            if (orders == null) return;
            var groupedOrders = orders.GroupBy(i => new XqTargetPresentOrderListKey(i.SubAccountFields.SubAccountId, i.TargetType, i.TargetKey, i.ClientOrderType));
            foreach (var group in groupedOrders)
            {
                var list = TryGetXqTargetPresentOrderList(group.Key);
                if (list != null)
                {
                    foreach (var o in group)
                    {
                        list.Remove(o);
                    }
                }
            }
        }

        private void AddOrderItems(OrderItemDataModel[] orders)
        {
            if (orders == null) return;
            var groupedOrders = orders.GroupBy(i => new XqTargetPresentOrderListKey(i.SubAccountFields.SubAccountId, i.TargetType, i.TargetKey, i.ClientOrderType));
            foreach (var group in groupedOrders)
            {
                var list = TryGetXqTargetPresentOrderList(group.Key);
                if (list != null)
                {
                    foreach (var o in group)
                    {
                        if (!list.Contains(o))
                            list.Add(o);
                    }
                }
            }
        }
        
        private void ClearOrderItems()
        {
            var keys = presentOrderLists.Keys.ToArray();
            foreach (var k in keys)
            {
                presentOrderLists[k].Clear();
            }
        }

        private void OrderItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                    var oldItems = e.OldItems?.OfType<OrderItemDataModel>().ToArray();
                    var newItems = e.NewItems?.OfType<OrderItemDataModel>().ToArray();
                    RemoveOrderItems(oldItems);
                    AddOrderItems(newItems);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    ClearOrderItems();
                    break;
                case NotifyCollectionChangedAction.Move:
                    // do nothing
                    break;
            }
        }
    }
}
