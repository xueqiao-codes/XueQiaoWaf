using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Helper
{
    /// <summary>
    /// 列表列 live properties 辅助
    /// </summary>
    public static class ListColumnLivePropertiesHelper
    {

#region EntrustedOrderList

        /// <summary>
        /// 委托单列表的列<see cref="OrderListColumn_Entrusted"/>对应于订单项<see cref="OrderItemDataModel_Entrusted"/>的 live properties 字典
        /// </summary>
        public static Dictionary<OrderListColumn_Entrusted, string[]> EntrustedOrderColumnKeyedOrderLiveProperties
        {
            get
            {
                var map = new Dictionary<OrderListColumn_Entrusted, string[]>();
                map.Add(OrderListColumn_Entrusted.OrderState, new string[] { nameof(OrderItemDataModel_Entrusted.OrderState) });
                map.Add(OrderListColumn_Entrusted.Direction, new string[] { nameof(OrderItemDataModel_Entrusted.Direction) });
                map.Add(OrderListColumn_Entrusted.TargetType, new string[] { nameof(OrderItemDataModel_Entrusted.TargetType) });
                map.Add(OrderListColumn_Entrusted.Name, new string[] { nameof(OrderItemDataModel_Entrusted.TargetKey) });
                map.Add(OrderListColumn_Entrusted.Price, new string[] { nameof(OrderItemDataModel_Entrusted.Price) });
                map.Add(OrderListColumn_Entrusted.Quantity, new string[] { nameof(OrderItemDataModel_Entrusted.Quantity) });
                map.Add(OrderListColumn_Entrusted.TradeVolume, new string[] { nameof(OrderItemDataModel_Entrusted.TradeVolume) });
                map.Add(OrderListColumn_Entrusted.TradeAvgPrice, new string[] { nameof(OrderItemDataModel_Entrusted.TradeAvgPrice) });
                map.Add(OrderListColumn_Entrusted.OrderType, new string[] { nameof(OrderItemDataModel_Entrusted.OrderType) });
                map.Add(OrderListColumn_Entrusted.CreateTime, new string[] { nameof(OrderItemDataModel_Entrusted.OrderTimestampMs) });
                map.Add(OrderListColumn_Entrusted.OrderId, new string[] { nameof(OrderItemDataModel_Entrusted.OrderId) });
                map.Add(OrderListColumn_Entrusted.UpdateTime, new string[] { nameof(OrderItemDataModel_Entrusted.UpdateTimestampMs) });
                map.Add(OrderListColumn_Entrusted.CreatorUserName, new string[] { nameof(OrderItemDataModel_Entrusted.SubAccountFields) });
                map.Add(OrderListColumn_Entrusted.CreatorUserId, new string[] { nameof(OrderItemDataModel_Entrusted.SubAccountFields) });
                return map;
            }
        }

        /// <summary>
        /// 委托单列表可排序的列
        /// </summary>
        public static OrderListColumn_Entrusted[] EntrustedOrderListSortableColumns
        {
            get
            {
                return new OrderListColumn_Entrusted[] 
                {
                    OrderListColumn_Entrusted.OrderState, OrderListColumn_Entrusted.Direction,
                    OrderListColumn_Entrusted.TargetType, OrderListColumn_Entrusted.Name,
                    OrderListColumn_Entrusted.CreateTime, OrderListColumn_Entrusted.CreatorUserName,
                    OrderListColumn_Entrusted.CreatorUserId,
                };
            }
        }

        /// <summary>
        /// 委托单列表可过滤的列
        /// </summary>
        public static OrderListColumn_Entrusted[] EntrustedOrderListFilterableColumns
        {
            get
            {
                return new OrderListColumn_Entrusted[]
                {
                    OrderListColumn_Entrusted.OrderState, OrderListColumn_Entrusted.Direction,
                    OrderListColumn_Entrusted.TargetType, OrderListColumn_Entrusted.Name,
                    OrderListColumn_Entrusted.CreateTime, OrderListColumn_Entrusted.OrderId,
                    OrderListColumn_Entrusted.CreatorUserName
                };
            }
        }

        #endregion


#region ParkedOrderList
        /// <summary>
        /// 预埋单列表的列<see cref="OrderListColumn_Parked"/>对应于订单项<see cref="OrderItemDataModel_Parked"/>的 live properties 字典
        /// </summary>
        public static Dictionary<OrderListColumn_Parked, string[]> ParkedOrderColumnKeyedOrderLiveProperties
        {
            get
            {
                var map = new Dictionary<OrderListColumn_Parked, string[]>();
                map.Add(OrderListColumn_Parked.OrderState, new string[] { nameof(OrderItemDataModel_Parked.OrderState) });
                map.Add(OrderListColumn_Parked.Direction, new string[] { nameof(OrderItemDataModel_Parked.Direction) });
                map.Add(OrderListColumn_Parked.TargetType, new string[] { nameof(OrderItemDataModel_Parked.TargetType) });
                map.Add(OrderListColumn_Parked.OrderType, new string[] { nameof(OrderItemDataModel_Parked.OrderType) });
                map.Add(OrderListColumn_Parked.Name, new string[] { nameof(OrderItemDataModel_Parked.TargetKey) });
                map.Add(OrderListColumn_Parked.Quantity, new string[] { nameof(OrderItemDataModel_Parked.Quantity) });
                map.Add(OrderListColumn_Parked.OrderId, new string[] { nameof(OrderItemDataModel_Parked.OrderId) });
                map.Add(OrderListColumn_Parked.CreateTime, new string[] { nameof(OrderItemDataModel_Parked.OrderTimestampMs) });
                map.Add(OrderListColumn_Parked.UpdateTime, new string[] { nameof(OrderItemDataModel_Parked.UpdateTimestampMs) });
                map.Add(OrderListColumn_Parked.CreatorUserName, new string[] { nameof(OrderItemDataModel_Parked.SubAccountFields) });
                map.Add(OrderListColumn_Parked.CreatorUserId, new string[] { nameof(OrderItemDataModel_Parked.SubAccountFields) });
                return map;
            }
        }

        /// <summary>
        /// 预埋单列表可排序的列
        /// </summary>
        public static OrderListColumn_Parked[] ParkedOrderListSortableColumns
        {
            get
            {
                return new OrderListColumn_Parked[]
                {
                    OrderListColumn_Parked.OrderState, OrderListColumn_Parked.Direction,
                    OrderListColumn_Parked.TargetType, OrderListColumn_Parked.Name,
                    OrderListColumn_Parked.CreateTime, OrderListColumn_Parked.CreatorUserName,
                    OrderListColumn_Parked.CreatorUserId,
                };
            }
        }

        /// <summary>
        /// 预埋单列表可过滤的列
        /// </summary>
        public static OrderListColumn_Parked[] ParkedOrderListFilterableColumns
        {
            get
            {
                return new OrderListColumn_Parked[]
                {
                    OrderListColumn_Parked.OrderState, OrderListColumn_Parked.Direction,
                    OrderListColumn_Parked.TargetType, OrderListColumn_Parked.Name,
                    OrderListColumn_Parked.CreateTime, OrderListColumn_Parked.OrderId,
                    OrderListColumn_Parked.CreatorUserName
                };
            }
        }

        #endregion

        #region ConditionOrderList
        /// <summary>
        /// 条件单列表的列<see cref="OrderListColumn_Condition"/>对应于订单项<see cref="OrderItemDataModel_Condition"/>的 live properties 字典
        /// </summary>
        public static Dictionary<OrderListColumn_Condition, string[]> ConditionOrderColumnKeyedOrderLiveProperties
        {
            get
            {
                var map = new Dictionary<OrderListColumn_Condition, string[]>();
                map.Add(OrderListColumn_Condition.OrderState, new string[] { nameof(OrderItemDataModel_Condition.OrderState) });
                map.Add(OrderListColumn_Condition.TargetType, new string[] { nameof(OrderItemDataModel_Condition.TargetType) });
                map.Add(OrderListColumn_Condition.ConditionOrderLabel, new string[] { nameof(OrderItemDataModel_Condition.OrderLabel) });
                map.Add(OrderListColumn_Condition.Name, new string[] { nameof(OrderItemDataModel_Condition.TargetKey) });
                map.Add(OrderListColumn_Condition.CreateTime, new string[] { nameof(OrderItemDataModel_Condition.OrderTimestampMs) });
                map.Add(OrderListColumn_Condition.OrderId, new string[] { nameof(OrderItemDataModel_Condition.OrderId) });
                map.Add(OrderListColumn_Condition.UpdateTime, new string[] { nameof(OrderItemDataModel_Condition.UpdateTimestampMs) });
                map.Add(OrderListColumn_Condition.CreatorUserName, new string[] { nameof(OrderItemDataModel_Condition.SubAccountFields) });
                map.Add(OrderListColumn_Condition.CreatorUserId, new string[] { nameof(OrderItemDataModel_Condition.SubAccountFields) });
                return map;
            }
        }

        /// <summary>
        /// 条件单列表可排序的列
        /// </summary>
        public static OrderListColumn_Condition[] ConditionOrderListSortableColumns
        {
            get
            {
                return new OrderListColumn_Condition[]
                {
                    OrderListColumn_Condition.OrderState, 
                    OrderListColumn_Condition.TargetType, OrderListColumn_Condition.ConditionOrderLabel,
                    OrderListColumn_Condition.Name, OrderListColumn_Condition.CreateTime,
                    OrderListColumn_Condition.CreatorUserName, OrderListColumn_Condition.CreatorUserId,
                };
            }
        }

        /// <summary>
        /// 条件单列表可过滤的列
        /// </summary>
        public static OrderListColumn_Condition[] ConditionOrderListFilterableColumns
        {
            get
            {
                return new OrderListColumn_Condition[]
                {
                    OrderListColumn_Condition.OrderState, 
                    OrderListColumn_Condition.TargetType, OrderListColumn_Condition.Name,
                    OrderListColumn_Condition.ConditionOrderLabel, OrderListColumn_Condition.CreateTime,
                    OrderListColumn_Condition.OrderId, OrderListColumn_Condition.CreatorUserName,
                };
            }
        }
        #endregion

        #region TradeList
        /// <summary>
        /// 成交列表的列<see cref="TradeListColumn"/>对应于成交项<see cref="TradeItemDataModel"/>的 live properties 字典
        /// </summary>
        public static Dictionary<TradeListColumn, string[]> TradeItemColumnKeyedLiveProperties
        {
            get
            {
                var map = new Dictionary<TradeListColumn, string[]>();
                map.Add(TradeListColumn.Direction, new string[] { nameof(TradeItemDataModel.Direction) });
                map.Add(TradeListColumn.TargetType, new string[] { nameof(TradeItemDataModel.TargetType) });
                map.Add(TradeListColumn.Name, new string[] { nameof(TradeItemDataModel.TargetKey) });
                map.Add(TradeListColumn.TradePrice, new string[] { nameof(TradeItemDataModel.TradePrice) });
                map.Add(TradeListColumn.TradeVolume, new string[] { nameof(TradeItemDataModel.TradeVolume) });
                map.Add(TradeListColumn.SourceType, new string[] { nameof(TradeItemDataModel.SourceType) });
                map.Add(TradeListColumn.CreateTime, new string[] { nameof(TradeItemDataModel.CreateTimestampMs) });
                map.Add(TradeListColumn.TradeId, new string[] { nameof(TradeItemDataModel.TradeId) });
                map.Add(TradeListColumn.OrderId, new string[] { nameof(TradeItemDataModel.OrderId) });
                map.Add(TradeListColumn.CreatorUserName, new string[] { nameof(TradeItemDataModel.SubAccountFields) });
                map.Add(TradeListColumn.SubUserId, new string[] { nameof(TradeItemDataModel.SubAccountFields) });
                return map;
            }
        }

        /// <summary>
        /// 成交列表可排序的列
        /// </summary>
        public static TradeListColumn[] TradeListSortableColumns
        {
            get
            {
                return new TradeListColumn[]
                {
                    TradeListColumn.Direction, TradeListColumn.TargetType,
                    TradeListColumn.Name, TradeListColumn.SourceType,
                    TradeListColumn.CreateTime, TradeListColumn.OrderId,
                    TradeListColumn.CreatorUserName, TradeListColumn.SubUserId,
                };
            }
        }

        /// <summary>
        /// 成交列表可过滤的列
        /// </summary>
        public static TradeListColumn[] TradeListFilterableColumns
        {
            get
            {
                return new TradeListColumn[]
                {
                    TradeListColumn.Direction, TradeListColumn.TargetType,
                    TradeListColumn.Name, TradeListColumn.SourceType,
                    TradeListColumn.CreateTime, TradeListColumn.TradeId,
                    TradeListColumn.OrderId, TradeListColumn.CreatorUserName
                };
            }
        }
        #endregion

        #region SubscribeContractList
        
        /// <summary>
        /// 合约订阅列表可过滤的列
        /// </summary>
        public static SubscribeContractListDisplayColumn[] SubscribeContractFilterableColumns
        {
            get
            {
                return new SubscribeContractListDisplayColumn[] 
                {
                    // TODO: 填写可过滤的列
                };
            }
        }
        #endregion

        #region SubscribeComposeList
        
        /// <summary>
        /// 组合订阅列表可过滤的列
        /// </summary>
        public static SubscribeComposeListDisplayColumn[] SubscribeComposeFilterableColumns
        {
            get
            {
                return new SubscribeComposeListDisplayColumn[]
                {
                    // TODO: 填写可过滤的列
                };
            }
        }
        #endregion

    }
}
