using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.trade.hosting.arbitrage.thriftapi;

namespace XueQiaoFoundation.BusinessResources.DataModels
{
    /// <summary>
    /// 预埋单列表过滤类型
    /// </summary>
    public enum ParkedOrderListFilterType
    {
        All,            // 全部
        Hanging, // 已挂单（挂单中）
    }

    /// <summary>
    /// 预埋单 data model。
    /// </summary>
    public class OrderItemDataModel_Parked : OrderItemDataModel
    {
        public OrderItemDataModel_Parked(ClientXQOrderTargetType targetType,
            HostingXQOrderType orderType,
            SubAccountFieldsForTradeData subAccountFields,
            string orderId,
            string targetKey) : base(targetType, orderType, subAccountFields, orderId, targetKey)
        {
            if (!IsOrder_Parked(orderType))
            {
                throw new ArgumentException("order type is not parked.");
            }

            InvalidateBelongListFilterTypes();
        }

        protected override void OnUpdatedOrderState()
        {
            base.OnUpdatedOrderState();
            InvalidateBelongListFilterTypes();
        }

        private ClientTradeDirection direction;
        /// <summary>
        /// 方向
        /// </summary>
        public ClientTradeDirection Direction
        {
            get { return direction; }
            set { SetProperty(ref direction, value); }
        }

        private int quantity;
        public int Quantity
        {
            get { return quantity; }
            set { SetProperty(ref quantity, value); }
        }

        private HostingXQOrderPrice triggerOrderPrice;
        public HostingXQOrderPrice TriggerOrderPrice
        {
            get { return triggerOrderPrice; }
            set { SetProperty(ref triggerOrderPrice, value); }
        }

        private ObservableCollection<ParkedOrderListFilterType> belongListFilterTypes;
        /// <summary>
        /// 订单所属列表过滤类型
        /// </summary>
        public ObservableCollection<ParkedOrderListFilterType> BelongListFilterTypes
        {
            get { return belongListFilterTypes; }
            private set { SetProperty(ref belongListFilterTypes, value); }
        }

        /// <summary>
        /// 更新所属列表过滤类型
        /// </summary>
        /// <returns></returns>
        public void InvalidateBelongListFilterTypes()
        {
            var belongTypes = new List<ParkedOrderListFilterType> { ParkedOrderListFilterType.All };
            var orderState = this.OrderState;
            var isTriggerWaiting = (orderState != ClientXQOrderState.XQ_ORDER_FINISHED
                && orderState != ClientXQOrderState.XQ_ORDER_CANCELLED
                && orderState != ClientXQOrderState.XQ_ORDER_FINISHING);
            if (isTriggerWaiting)
            {
                belongTypes.Add(ParkedOrderListFilterType.Hanging);
            }

            this.BelongListFilterTypes = new ObservableCollection<ParkedOrderListFilterType>(belongTypes);
        }

        /// <summary>
        /// 判断<see cref="HostingXQOrderType"/>订单类型的订单是否为预埋单
        /// </summary>
        /// <param name="hostingXQOrderType"><see cref="HostingXQOrderType"/>订单类型</param>
        /// <returns></returns>
        public static bool IsOrder_Parked(HostingXQOrderType hostingXQOrderType)
        {
            return hostingXQOrderType == HostingXQOrderType.XQ_ORDER_CONTRACT_PARKED;
        }
    }
}
