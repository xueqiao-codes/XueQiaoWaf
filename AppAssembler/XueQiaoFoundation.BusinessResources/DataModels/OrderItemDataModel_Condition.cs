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
    /// 条件单列表过滤类型
    /// </summary>
    public enum ConditionOrderListFilterType
    {
        All,            // 全部
        Hanging,        // 已挂单(挂单中)
    }

    /// <summary>
    /// 条件单 data model
    /// </summary>
    public class OrderItemDataModel_Condition : OrderItemDataModel
    {
        public OrderItemDataModel_Condition(ClientXQOrderTargetType targetType,
            HostingXQOrderType orderType,
            SubAccountFieldsForTradeData subAccountFields,
            string orderId,
            string targetKey) : base(targetType, orderType, subAccountFields, orderId, targetKey)
        {
            if (!IsOrder_Condition(OrderType))
            {
                throw new ArgumentException("order type is not Condition.");
            }

            InvalidateBelongListFilterTypes();
        }

        protected override void OnUpdatedOrderState()
        {
            base.OnUpdatedOrderState();
            InvalidateBelongListFilterTypes();
        }

        private ObservableCollection<HostingXQCondition> conditions;
        // 条件
        public ObservableCollection<HostingXQCondition> Conditions
        {
            get { return conditions; }
            set { SetProperty(ref conditions, value); }
        }

        private HostingXQConditionOrderLabel? orderLabel;
        // 标签
        public HostingXQConditionOrderLabel? OrderLabel
        {
            get { return orderLabel; }
            set { SetProperty(ref orderLabel, value); }
        }

        private HostingXQEffectDate effectDate;
        // 有效期类型及其值
        public HostingXQEffectDate EffectDate
        {
            get { return effectDate; }
            set { SetProperty(ref effectDate, value); }
        }

        private ObservableCollection<ConditionOrderListFilterType> belongListFilterTypes;
        /// <summary>
        /// 订单所属列表过滤类型
        /// </summary>
        public ObservableCollection<ConditionOrderListFilterType> BelongListFilterTypes
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
            var belongTypes = new List<ConditionOrderListFilterType> { ConditionOrderListFilterType.All };
            var orderState = this.OrderState;
            var isTriggerWaiting = (orderState != ClientXQOrderState.XQ_ORDER_FINISHED
                && orderState != ClientXQOrderState.XQ_ORDER_CANCELLED
                && orderState != ClientXQOrderState.XQ_ORDER_FINISHING);
            var isSuspended = (orderState == ClientXQOrderState.XQ_ORDER_SUSPENDED);
            if (isTriggerWaiting || isSuspended)
            {
                belongTypes.Add(ConditionOrderListFilterType.Hanging);
            }
            
            this.BelongListFilterTypes = new ObservableCollection<ConditionOrderListFilterType>(belongTypes);
        }


        /// <summary>
        /// 判断<see cref="HostingXQOrderType"/>订单类型的订单是否为条件单
        /// </summary>
        /// <param name="hostingXQOrderType"><see cref="HostingXQOrderType"/>订单类型</param>
        /// <returns></returns>
        public static bool IsOrder_Condition(HostingXQOrderType hostingXQOrderType)
        {
            return hostingXQOrderType == HostingXQOrderType.XQ_ORDER_CONDITION;
        }

        /// <summary>
        /// 解析条件单详情，输出一些信息
        /// </summary>
        /// <param name="orderDetail"></param>
        /// <param name="hostingXQOrderType"></param>
        /// <param name="_conditions"></param>
        /// <param name="_conditionOrderLabel"></param>
        /// <param name="_effectDate"></param>
        public static void ParseConditionOrderDetail(HostingXQOrderDetail orderDetail,
            HostingXQOrderType hostingXQOrderType,
            out IEnumerable<HostingXQCondition> _conditions,
            out HostingXQConditionOrderLabel? _conditionOrderLabel,
            out HostingXQEffectDate _effectDate)
        {
            _conditions = null;
            _conditionOrderLabel = null;
            _effectDate = null;

            if (hostingXQOrderType == HostingXQOrderType.XQ_ORDER_CONDITION)
            {
                _conditions = orderDetail.ConditionOrderDetail?.Conditions?.ToArray();
                _conditionOrderLabel = orderDetail.ConditionOrderDetail?.Label;
                _effectDate = orderDetail.ConditionOrderDetail?.EffectDate;
            }
        }
    }
}
