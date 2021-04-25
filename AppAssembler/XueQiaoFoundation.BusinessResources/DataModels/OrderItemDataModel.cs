using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using xueqiao.trade.hosting.arbitrage.thriftapi;
using XueQiaoFoundation.BusinessResources.Constants;

namespace XueQiaoFoundation.BusinessResources.DataModels
{
    public class OrderItemDataModel : Model, IXqTargetDM
    {
        protected OrderItemDataModel(ClientXQOrderTargetType targetType, 
            HostingXQOrderType orderType, 
            SubAccountFieldsForTradeData subAccountFields, 
            string orderId, 
            string targetKey)
        {
            this.TargetType = targetType;
            this.OrderType = orderType;
            this.SubAccountFields = subAccountFields;
            this.OrderId = orderId;
            this.TargetKey = targetKey;

            InvalidateClientOrderType();
        }
        
        protected virtual void OnUpdatedOrderState()
        {
            InvalidateIsSuspendedWithError();
            InvalidateIsStateAmbiguous();
        }

        protected virtual void OnUpdatedOrderDetail()
        {
            InvalidateIsSuspendedWithError();
        }
        
        /// <summary>
        /// 标的类型
        /// </summary>
        public ClientXQOrderTargetType TargetType { get; private set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public HostingXQOrderType OrderType { get; private set; }

        /// <summary>
        /// 操作账户相关数据
        /// </summary>
        public SubAccountFieldsForTradeData SubAccountFields { get; private set; }

        /// <summary>
        /// 订单 id
        /// </summary>
        public string OrderId { get; private set; }

        /// <summary>
        /// 标的key。合约类型标的为合约 id，组合类型标的为组合 id
        /// </summary>
        public string TargetKey { get; private set; }


        private TargetContract_TargetContractDetail targetContractDetailContainer;
        /// <summary>
        /// 合约标的的容器
        /// </summary>
        public TargetContract_TargetContractDetail TargetContractDetailContainer
        {
            get { return targetContractDetailContainer; }
            set { SetProperty(ref targetContractDetailContainer, value); }
        }

        private TargetCompose_ComposeDetail targetComposeDetailContainer;
        /// <summary>
        /// 组合标的的组合详情
        /// </summary>
        public TargetCompose_ComposeDetail TargetComposeDetailContainer
        {
            get { return targetComposeDetailContainer; }
            set { SetProperty(ref targetComposeDetailContainer, value); }
        }

        private UserComposeViewContainer targetComposeUserComposeViewContainer;
        /// <summary>
        /// 组合标的的用户组合视图容器
        /// </summary>
        public UserComposeViewContainer TargetComposeUserComposeViewContainer
        {
            get { return targetComposeUserComposeViewContainer; }
            set { SetProperty(ref targetComposeUserComposeViewContainer, value); }
        }
        
        private string targetName;
        /// <summary>
        /// 雪橇标的名称。根据其他数据设置而来的，避免在界面中拼接影响性能
        /// </summary>
        public string TargetName
        {
            get { return targetName; }
            set { SetProperty(ref targetName, value); }
        }

        private long? orderTimestampMs;
        public long? OrderTimestampMs
        {
            get { return orderTimestampMs; }
            set { SetProperty(ref orderTimestampMs, value); }
        }

        private long? updateTimestampMs;
        public long? UpdateTimestampMs
        {
            get { return updateTimestampMs; }
            set { SetProperty(ref updateTimestampMs, value); }
        }
        
        private int version;
        public int Version
        {
            get { return version; }
            set { SetProperty(ref version, value); }
        }

        private ClientXQOrderState orderState;
        public ClientXQOrderState OrderState
        {
            get { return orderState; }
            set
            {
                if (SetProperty(ref orderState, value))
                {
                    OnUpdatedOrderState();
                }
            }
        }
        
        private HostingXQOrderState orderStateDetail;
        // 状态详情
        public HostingXQOrderState OrderStateDetail
        {
            get { return orderStateDetail; }
            set
            {
                if (SetProperty(ref orderStateDetail, value))
                {
                    OnUpdatedOrderDetail();
                }
            }
        }
        

        private string sourceOrderId;
        /// <summary>
        /// 引向的条件单 id。(父订单id, 存在时表明当前订单由该条件单触发)
        /// </summary>
        public string SourceOrderId
        {
            get { return sourceOrderId; }
            set { SetProperty(ref sourceOrderId, value); }
        }

        private string actionOrderId;
        /// <summary>
        /// 触发后产生的订单 id。(子订单id,如果当前条件单未触发则该字段为空)
        /// </summary>
        public string ActionOrderId
        {
            get { return actionOrderId; }
            set { SetProperty(ref actionOrderId, value); }
        }

        private long? orderEffectEndTimestampMs;
        /// <summary>
        /// 有效时间结束时间
        /// </summary>
        public long? OrderEffectEndTimestampMs
        {
            get { return orderEffectEndTimestampMs; }
            set { SetProperty(ref orderEffectEndTimestampMs, value); }
        }

        /// <summary>
        /// 是否异常暂停
        /// </summary>
        private bool isSuspendedWithError;
        public bool IsSuspendedWithError
        {
            get { return isSuspendedWithError; }
            private set { SetProperty(ref isSuspendedWithError, value); }
        }

        /// <summary>
        /// 是否状态不明确
        /// </summary>
        private bool isStateAmbiguous;
        public bool IsStateAmbiguous
        {
            get { return isStateAmbiguous; }
            private set { SetProperty(ref isStateAmbiguous, value); }
        }

        /// <summary>
        /// 客户端订单类型
        /// </summary>
        private XQClientOrderType? clientOrderType;
        public XQClientOrderType? ClientOrderType
        {
            get { return clientOrderType; }
            private set { SetProperty(ref clientOrderType, value); }
        }

        /// <summary>
        /// 更新<see cref="IsSuspendedWithError"/>
        /// </summary>
        /// <returns></returns>
        private void InvalidateIsSuspendedWithError()
        {
            var _orderState = this.OrderState;
            var _stateDetail = this.OrderStateDetail;
            if (_orderState == ClientXQOrderState.XQ_ORDER_SUSPENDED && _stateDetail?.SuspendReason == HostingXQSuspendReason.SUSPENDED_ERROR_OCCURS)
                this.IsSuspendedWithError = true;
            else
                this.IsSuspendedWithError = false;
        }

        /// <summary>
        /// 更新<see cref="IsStateAmbiguous"/>
        /// </summary>
        private void InvalidateIsStateAmbiguous()
        {
            var _orderState = this.OrderState;
            this.IsStateAmbiguous = XueQiaoConstants.AmbiguousOrderStates?.Contains(_orderState) ?? false;
        }

        /// <summary>
        /// 更新<see cref="ClientOrderType"/>
        /// </summary>
        private void InvalidateClientOrderType()
        {
            XQClientOrderType? _clientOrderType = null;
            var _orderType = this.OrderType;
            if (_orderType == HostingXQOrderType.XQ_ORDER_CONTRACT_LIMIT
                || _orderType == HostingXQOrderType.XQ_ORDER_COMPOSE_LIMIT)
                _clientOrderType = XQClientOrderType.Entrusted;
            else if (_orderType == HostingXQOrderType.XQ_ORDER_CONDITION)
                _clientOrderType = XQClientOrderType.Condition;
            else if (_orderType == HostingXQOrderType.XQ_ORDER_CONTRACT_PARKED)
                _clientOrderType = XQClientOrderType.Parked;

            this.ClientOrderType = _clientOrderType;
        }

    }
}
