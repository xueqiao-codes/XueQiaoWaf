using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.BusinessResources.Constants;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoWaf.Trade.Interfaces.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Helper
{
    public static class TradeWorkspaceDataDisplayHelper
    {
        public static void ConfigureWorkspaceDataRootDisplayDefaultIfNeed(TradeWorkspaceDataRoot dataRoot)
        {
            if (dataRoot == null) return;

            var mainWindowWorkspaces = dataRoot.MainWindowWorkspaceListContainer.Workspaces;

            // 配置主窗口
            ConfigureDisplayDefaultIfNeedForTradeWorkspaces(mainWindowWorkspaces);

            // 配置次交易窗口
            foreach (var interWindow in dataRoot.InterTabWorkspaceWindowListContainer.Windows)
            {
                ConfigureDisplayDefaultIfNeedForTradeWorkspaces(interWindow.WorkspaceListContainer.Workspaces);
            }
        }

        /// <summary>
        /// 配置交易组件的默认 title
        /// </summary>
        /// <param name="component"></param>
        public static void ConfigureComponentDescTitleDefaultIfNeed(TradeComponent component)
        {
            if (component == null) return;
            if (!string.IsNullOrEmpty(component.ComponentDescTitle)) return;
            component.ComponentDescTitle = GetTradeComponentTypeDisplayName(component.ComponentType);
        }
        
        /// <summary>
        /// 获取交易组件类型的显示名称
        /// </summary>
        /// <param name="tradeComponentType"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static string GetTradeComponentTypeDisplayName(int tradeComponentType, CultureInfo culture = null)
        {
            var key = string.Format("TradeComponentType_{0}", tradeComponentType);
            string name = Properties.Resources.ResourceManager.GetString(key, culture);
            return name;
        }

        /// <summary>
        /// 默认订阅合约列表的显示列
        /// </summary>
        public static readonly IEnumerable<SubscribeContractListDisplayColumn> DefaultContractListDisplayColumns
            = new SubscribeContractListDisplayColumn[]
            {
                SubscribeContractListDisplayColumn.Name,
                SubscribeContractListDisplayColumn.LastPrice,
                SubscribeContractListDisplayColumn.LastQty,
                SubscribeContractListDisplayColumn.BidQty,
                SubscribeContractListDisplayColumn.BidPrice,
                SubscribeContractListDisplayColumn.AskPrice,
                SubscribeContractListDisplayColumn.AskQty,
                SubscribeContractListDisplayColumn.IncreasePrice,
                SubscribeContractListDisplayColumn.IncreasePriceRate,
                SubscribeContractListDisplayColumn.Volume,
                SubscribeContractListDisplayColumn.OpenInterest,
                SubscribeContractListDisplayColumn.DailyIncrementOpenInterest,
                SubscribeContractListDisplayColumn.UpdateTime,
                SubscribeContractListDisplayColumn.SubscribeState,
            };


        /// <summary>
        /// 默认组合列表的显示列
        /// </summary>
        public static readonly IEnumerable<SubscribeComposeListDisplayColumn> DefaultComposeListDisplayColumns
            = new SubscribeComposeListDisplayColumn[]
            {
                SubscribeComposeListDisplayColumn.Name,
                SubscribeComposeListDisplayColumn.BidQty,
                SubscribeComposeListDisplayColumn.BidPrice,
                SubscribeComposeListDisplayColumn.AskPrice,
                SubscribeComposeListDisplayColumn.AskQty,
                SubscribeComposeListDisplayColumn.UpdateTime,
                SubscribeComposeListDisplayColumn.SubscribeState,
            };

        /// <summary>
        /// 委托单列表的默认显示列
        /// </summary>
        public static readonly IEnumerable<OrderListColumn_Entrusted> DefaultOrderListEntrustedDisplayColumns
            = new OrderListColumn_Entrusted[]
            {
                OrderListColumn_Entrusted.Name,
                OrderListColumn_Entrusted.Direction,
                OrderListColumn_Entrusted.TargetType,
                OrderListColumn_Entrusted.OrderState,
                OrderListColumn_Entrusted.Price,
                OrderListColumn_Entrusted.Quantity,
                OrderListColumn_Entrusted.TradeVolume,
                OrderListColumn_Entrusted.TradeAvgPrice,
                OrderListColumn_Entrusted.ComposeOrderSendType,
                OrderListColumn_Entrusted.ExecuteDetail,
                OrderListColumn_Entrusted.OrderType,
                OrderListColumn_Entrusted.CreateTime,
                OrderListColumn_Entrusted.EffectDateType,
                OrderListColumn_Entrusted.StateMsg,
                OrderListColumn_Entrusted.OrderId,
                OrderListColumn_Entrusted.ParentOrder,
                OrderListColumn_Entrusted.CreatorUserName,
                OrderListColumn_Entrusted.SubAccountName
            };

        /// <summary>
        /// 预埋单列表的默认显示列
        /// </summary>
        public static readonly IEnumerable<OrderListColumn_Parked> DefaultOrderListParkedDisplayColumns
            = new OrderListColumn_Parked[] 
            {
                OrderListColumn_Parked.Name,
                OrderListColumn_Parked.Direction,
                OrderListColumn_Parked.TargetType,
                OrderListColumn_Parked.OrderState,
                OrderListColumn_Parked.OrderType,
                OrderListColumn_Parked.TriggerOrderPrice,
                OrderListColumn_Parked.Quantity,
                OrderListColumn_Parked.CreateTime,
                OrderListColumn_Parked.TriggerTime,
                OrderListColumn_Parked.OrderId,
                OrderListColumn_Parked.ChildOrder,
                OrderListColumn_Parked.CreatorUserName,
                OrderListColumn_Parked.SubAccountName
            };

        /// <summary>
        /// 条件单列表的默认显示列
        /// </summary>
        public static readonly IEnumerable<OrderListColumn_Condition> DefaultOrderListConditionDisplayColumns
            = new OrderListColumn_Condition[]
            {
                OrderListColumn_Condition.Name,
                OrderListColumn_Condition.TargetType,
                OrderListColumn_Condition.OrderState,
                OrderListColumn_Condition.ConditionOrderLabel,
                OrderListColumn_Condition.TriggerConditionInfo,
                OrderListColumn_Condition.CreateTime,
                OrderListColumn_Condition.EffectDateType,
                OrderListColumn_Condition.OrderId,
                OrderListColumn_Condition.ChildOrder,
                OrderListColumn_Condition.CreatorUserName,
                OrderListColumn_Condition.SubAccountName
            };

        /// <summary>
        /// 默认成交列表的显示列
        /// </summary>
        public static readonly IEnumerable<TradeListColumn> DefaultTradeListDisplayColumns
            = new TradeListColumn[]
            {
                TradeListColumn.Name,
                TradeListColumn.Direction,
                TradeListColumn.TargetType,
                TradeListColumn.TradePrice,
                TradeListColumn.TradeVolume,
                TradeListColumn.SourceType,
                TradeListColumn.CreateTime,
                TradeListColumn.TradeId,
                TradeListColumn.OrderId,
                TradeListColumn.CreatorUserName,
                TradeListColumn.SubAccountName
            };

        /// <summary>
        /// 某个组件类型是否属于`订阅数据`容器中的组件
        /// </summary>
        public static bool IsBelong2SubscribeDataContainerComponent(int componentType)
        {
            return ComponentsInSubscribeDataContainer.Contains(componentType);
        }

        public static readonly int[] ComponentsInSubscribeDataContainer
            = new[] 
            {
                XueQiaoConstants.TradeCompType_CONTRACT_LIST,
                XueQiaoConstants.TradeCompType_COMPOSE_LIST
            };

        /// <summary>
        /// 某个组件类型是否属于`下单`容器中的组件
        /// </summary>
        public static bool IsBelong2PlaceOrderContainerComponent(int componentType)
        {
            return ComponentsInPlaceOrderContainer.Contains(componentType);
        }

        public static readonly int[] ComponentsInPlaceOrderContainer
            = new[] { XueQiaoConstants.TradeCompType_PLACE_ORDER };

        /// <summary>
        /// 某个组件类型是否属于`账号`容器中的组件
        /// </summary>
        public static bool IsBelong2AccountContainerComponent(int componentType)
        {
            return ComponentsInAccountContainer.Contains(componentType);
        }

        public static readonly int[] ComponentsInAccountContainer
            = new[] 
            {
                XueQiaoConstants.TradeCompType_ENTRUSTED_ORDER_LIST,
                XueQiaoConstants.TradeCompType_PARKED_ORDER_LIST,
                XueQiaoConstants.TradeCompType_CONDITION_ORDER_LIST,
                XueQiaoConstants.TradeCompType_TRADE_LIST,
                XueQiaoConstants.TradeCompType_POSITION_LIST,
                XueQiaoConstants.TradeCompType_POSITION_ASSISTANT,
                XueQiaoConstants.TradeCompType_ORDER_HISTORY,
                XueQiaoConstants.TradeCompType_TRADE_HISTORY,
                XueQiaoConstants.TradeCompType_POSITION_ASSIGN_HISTORY,
                XueQiaoConstants.TradeCompType_FUND
            };


        private static void ConfigureDisplayDefaultIfNeedForTradeWorkspaces(IEnumerable<TabWorkspace> workspaces)
        {
            foreach (var workspace in workspaces)
            {
                foreach (var component in workspace.TradeComponents)
                {
                    ConfigureComponentDescTitleDefaultIfNeed(component);
                }
            }
        }
    }
}
