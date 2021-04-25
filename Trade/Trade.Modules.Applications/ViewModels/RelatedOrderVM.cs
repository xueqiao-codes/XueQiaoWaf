using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class RelatedOrderVM : ViewModel<IRelatedOrderView>
    {
        [ImportingConstructor]
        protected RelatedOrderVM(IRelatedOrderView view) : base(view)
        {
        }

        /// <summary>
        /// 当前订单显示类型
        /// </summary>
        private RelatedOrderShowType? currentOrderShowType;
        public RelatedOrderShowType? CurrentOrderShowType
        {
            get { return currentOrderShowType; }
            private set
            {
                if (SetProperty(ref currentOrderShowType, value))
                {
                    if (value != null) ViewCore.UpdateCurrentOrderListColumnsWithShowType(value.Value);
                }
            }
        }
        
        /// <summary>
        /// 当前订单
        /// </summary>
        private OrderItemDataModel currentOrder;
        public OrderItemDataModel CurrentOrder
        {
            get { return currentOrder; }
            set
            {
                if (SetProperty(ref currentOrder, value))
                {
                    CurrentOrderShowType = (value != null) ? GetOrderShowType(value) : null;
                    CurrentOrderList = (value != null) ? new OrderItemDataModel[] { value } : null;
                }
            }
        }
        
        private OrderItemDataModel[] currentOrderList;
        public OrderItemDataModel[] CurrentOrderList
        {
            get { return currentOrderList; }
            private set { SetProperty(ref currentOrderList, value); }
        }


        /// <summary>
        /// 父订单显示类型
        /// </summary>
        private RelatedOrderShowType? parentOrderShowType;
        public RelatedOrderShowType? ParentorderShowType
        {
            get { return parentOrderShowType; }
            private set
            {
                if (SetProperty(ref parentOrderShowType, value))
                {
                    if (value != null) ViewCore.UpdateParentOrderListColumnsWithShowType(value.Value);
                }
            }
        }

        /// <summary>
        /// 父订单
        /// </summary>
        private OrderItemDataModel parentOrder;
        public OrderItemDataModel ParentOrder
        {
            get { return parentOrder; }
            set
            {
                if (SetProperty(ref parentOrder, value))
                {
                    ParentorderShowType = value != null ? GetOrderShowType(value) : null;
                    ParentOrderList = (value != null) ? new OrderItemDataModel[] { value } : null;
                }
            }
        }

        private OrderItemDataModel[] parentOrderList;
        public OrderItemDataModel[] ParentOrderList
        {
            get { return parentOrderList; }
            private set { SetProperty(ref parentOrderList, value); }
        }

        /// <summary>
        /// 子订单显示类型
        /// </summary>
        private RelatedOrderShowType? childOrderShowType;
        public RelatedOrderShowType? ChildOrderShowType
        {
            get { return childOrderShowType; }
            private set
            {
                if (SetProperty(ref childOrderShowType, value))
                {
                    if (value != null) ViewCore.UpdateChildOrderListColumnsWithShowType(value.Value);
                }
            }
        }

        /// <summary>
        /// 子订单
        /// </summary>
        private OrderItemDataModel childOrder;
        public OrderItemDataModel ChildOrder
        {
            get { return childOrder; }
            set
            {
                if (SetProperty(ref childOrder, value))
                {
                    ChildOrderShowType = (value != null) ? GetOrderShowType(value) : null;
                    ChildOrderList = (value != null) ? new OrderItemDataModel[] { value } : null;
                }
            }
        }

        private OrderItemDataModel[] childOrderList;
        public OrderItemDataModel[] ChildOrderList
        {
            get { return childOrderList; }
            private set { SetProperty(ref childOrderList, value); }
        }
        
        private RelatedOrderShowType? GetOrderShowType(OrderItemDataModel orderItem)
        {
            if (orderItem is OrderItemDataModel_Entrusted)
                return RelatedOrderShowType.Entrusted;
            if (orderItem is OrderItemDataModel_Parked)
                return RelatedOrderShowType.Parked;
            if (orderItem is OrderItemDataModel_Condition)
                return RelatedOrderShowType.Condition;
            return null;
        }
    }
}
