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
    // 委托单列表过滤类型
    public enum EntrustedOrderListFilterType
    {
        All,            // 全部
        Hanging,        // 已挂单
        SuspendedWithError,      // 异常暂停
    }

    /// <summary>
    /// 委托单 data model
    /// </summary>
    public class OrderItemDataModel_Entrusted : OrderItemDataModel
    {
        public OrderItemDataModel_Entrusted(ClientXQOrderTargetType targetType,
            HostingXQOrderType orderType,
            SubAccountFieldsForTradeData subAccountFields,
            string orderId,
            string targetKey) : base(targetType, orderType, subAccountFields, orderId, targetKey)
        {
            if (!IsOrder_Entrusted(orderType))
            {
                throw new ArgumentException("order type is not Entrusted.");
            }

            InvalidateBelongListFilterTypes();
        }

        protected override void OnUpdatedOrderState()
        {
            base.OnUpdatedOrderState();
            InvalidateBelongListFilterTypes();
        }

        protected override void OnUpdatedOrderDetail()
        {
            base.OnUpdatedOrderDetail();
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
            set
            {
                if (SetProperty(ref quantity, value))
                {
                    InvalidateBelongListFilterTypes();
                }
            }
        }

        private double price;
        public double Price
        {
            get { return price; }
            set { SetProperty(ref price, value); }
        }

        private int tradeVolume;
        public int TradeVolume
        {
            get { return tradeVolume; }
            set
            {
                if (SetProperty(ref tradeVolume, value))
                {
                    InvalidateBelongListFilterTypes();
                }
            }
        }

        private double tradeAvgPrice;
        public double TradeAvgPrice
        {
            get { return tradeAvgPrice; }
            set { SetProperty(ref tradeAvgPrice, value); }
        }

        private HostingXQEffectDate effectDate;
        // 有效期类型及其值
        public HostingXQEffectDate EffectDate
        {
            get { return effectDate; }
            set { SetProperty(ref effectDate, value); }
        }

        private HostingXQComposeLimitOrderExecParams composeOrderExecParams;
        /// <summary>
        /// 雪橇组合标的订单的执行参数
        /// </summary>
        public HostingXQComposeLimitOrderExecParams ComposeOrderExecParams
        {
            get { return composeOrderExecParams; }
            set
            {
                SetProperty(ref composeOrderExecParams, value);
                this.ComposeOrderSendType = composeOrderExecParams?.ParseComposeOrderExecParamsSendType();
            }
        }

        private XQComposeOrderExecParamsSendType? composeOrderSendType;
        /// <summary>
        /// 雪橇组合标的订单发单方式
        /// </summary>
        public XQComposeOrderExecParamsSendType? ComposeOrderSendType
        {
            get { return composeOrderSendType; }
            private set { SetProperty(ref composeOrderSendType, value); }
        }

        private TargetComposeLegTradeSummarysContainer composeLegTradeSummarysContainer;
        /// <summary>
        /// 组合标的的腿成交概要容器
        /// </summary>
        public TargetComposeLegTradeSummarysContainer TargetComposeLegTradeSummarysContainer
        {
            get { return composeLegTradeSummarysContainer; }
            set { SetProperty(ref composeLegTradeSummarysContainer, value); }
        }
        
        private ObservableCollection<EntrustedOrderListFilterType> belongListFilterTypes;
        /// <summary>
        /// 订单所属列表过滤类型
        /// </summary>
        public ObservableCollection<EntrustedOrderListFilterType> BelongListFilterTypes
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
            var belongTypes = new List<EntrustedOrderListFilterType> { EntrustedOrderListFilterType.All };
            var orderState = this.OrderState;
            var stateDetail = this.OrderStateDetail;
            var quantity = this.Quantity;
            var tradeVolume = this.TradeVolume;

            if (orderState != ClientXQOrderState.XQ_ORDER_FINISHED
                && orderState != ClientXQOrderState.XQ_ORDER_CANCELLED
                && orderState != ClientXQOrderState.XQ_ORDER_FINISHING)
            {
                belongTypes.Add(EntrustedOrderListFilterType.Hanging);
            }

            if (orderState == ClientXQOrderState.XQ_ORDER_SUSPENDED && stateDetail?.SuspendReason == HostingXQSuspendReason.SUSPENDED_ERROR_OCCURS)
            {
                belongTypes.Add(EntrustedOrderListFilterType.SuspendedWithError);
            }

            this.BelongListFilterTypes = new ObservableCollection<EntrustedOrderListFilterType>(belongTypes);
        }

        /// <summary>
        /// 判断<see cref="HostingXQOrderType"/>订单类型的订单是否为委托单
        /// </summary>
        /// <param name="hostingXQOrderType"><see cref="HostingXQOrderType"/>订单类型</param>
        /// <returns></returns>
        public static bool IsOrder_Entrusted(HostingXQOrderType hostingXQOrderType)
        {
            return hostingXQOrderType == HostingXQOrderType.XQ_ORDER_CONTRACT_LIMIT
                || hostingXQOrderType == HostingXQOrderType.XQ_ORDER_COMPOSE_LIMIT;
        }

        /// <summary>
        /// 解析委托单详情，输出一些信息
        /// </summary>
        /// <param name="orderDetail"></param>
        /// <param name="hostingXQOrderType"></param>
        /// <param name="_effectDate"></param>
        /// <param name="_composeOrderExecParams"></param>
        public static void ParseEntrustedOrderDetail(HostingXQOrderDetail orderDetail, 
            HostingXQOrderType hostingXQOrderType,
            out HostingXQEffectDate _effectDate,
            out HostingXQComposeLimitOrderExecParams _composeOrderExecParams)
        {
            _effectDate = null;
            _composeOrderExecParams = null;

            if (hostingXQOrderType == HostingXQOrderType.XQ_ORDER_CONTRACT_LIMIT)
            {
                _effectDate = orderDetail?.ContractLimitOrderDetail?.EffectDate;
            }
            else if (hostingXQOrderType == HostingXQOrderType.XQ_ORDER_COMPOSE_LIMIT)
            {
                _effectDate = orderDetail?.ComposeLimitOrderDetail?.EffectDate;
                _composeOrderExecParams = orderDetail?.ComposeLimitOrderDetail?.ExecParams;
            }
        }
    }
}
