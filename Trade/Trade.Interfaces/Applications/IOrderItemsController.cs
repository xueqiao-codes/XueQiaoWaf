using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.trade.hosting.arbitrage.thriftapi;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoWaf.Trade.Interfaces.DataModels;

namespace XueQiaoWaf.Trade.Interfaces.Applications
{
    /// <summary>
    /// 登录用户的订单项管理协议
    /// </summary>
    public interface IOrderItemsController
    {
        /// <summary>
        /// 刷新某个子账户的订单列表
        /// <paramref name="subAccountId">子账户 id</paramref>
        /// </summary>
        void RefreshOrderItemsIfNeed(long subAccountId);
        
        /// <summary>
        /// 强制刷新某个子账户的订单列表
        /// <paramref name="subAccountId">子账户 id</paramref>
        /// </summary>
        void RefreshOrderItemsForce(long subAccountId);

        /// <summary>
        /// 子账户的订单列表刷新状态变化事件
        /// </summary>
        event SubAccountAnyDataRefreshStateChanged OrderItemsRefreshStateChanged;

        /// <summary>
        /// 添加或修改委托单
        /// </summary>
        /// <param name="subAccountId"></param>
        /// <param name="orderId"></param>
        /// <param name="orderType"></param>
        /// <param name="targetType"></param>
        /// <param name="targetKey"></param>
        /// <param name="newOrderDetail"></param>
        /// <param name="creatorUserId"></param>
        /// <param name="orderUpdateTemplateFactory">更新数据的模板工厂方法。arg1：该订单号的存在项，arg2：修改模板</param>
        /// <returns></returns>
        OrderItemDataModel_Entrusted AddOrUpdateOrder_Entrusted(long subAccountId, string orderId,
            HostingXQOrderType orderType, ClientXQOrderTargetType targetType, string targetKey,
            HostingXQOrderDetail newOrderDetail, int creatorUserId,
            Func<OrderItemDataModel_Entrusted, OrderUpdateTemplate_Entrusted> orderUpdateTemplateFactory);

        /// <summary>
        /// 更新委托单
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderUpdateTemplateFactory">更新数据的模板工厂方法。arg1：该订单号的存在项，arg2：修改模板</param>
        void UpdateOrder_Entrusted(string orderId,
            Func<OrderItemDataModel_Entrusted, OrderUpdateTemplate_Entrusted> orderUpdateTemplateFactory);


        /// <summary>
        /// 添加或修改预埋单
        /// </summary>
        /// <param name="subAccountId"></param>
        /// <param name="orderId"></param>
        /// <param name="orderType"></param>
        /// <param name="targetType"></param>
        /// <param name="targetKey"></param>
        /// <param name="newOrderDetail"></param>
        /// <param name="creatorUserId"></param>
        /// <param name="orderUpdateTemplateFactory">更新数据的模板工厂方法。arg1：该订单号的存在项，arg2：修改模板</param>
        /// <returns></returns>
        OrderItemDataModel_Parked AddOrUpdateOrder_Parked(long subAccountId, string orderId,
            HostingXQOrderType orderType, ClientXQOrderTargetType targetType, string targetKey,
            HostingXQOrderDetail newOrderDetail, int creatorUserId,
            Func<OrderItemDataModel_Parked, OrderUpdateTemplate_Parked> orderUpdateTemplateFactory);

        /// <summary>
        /// 更新预埋单
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderUpdateTemplateFactory">更新数据的模板工厂方法。arg1：该订单号的存在项，arg2：修改模板</param>
        void UpdateOrder_Parked(string orderId,
            Func<OrderItemDataModel_Parked, OrderUpdateTemplate_Parked> orderUpdateTemplateFactory);

        /// <summary>
        /// 添加或修改条件单
        /// </summary>
        /// <param name="subAccountId"></param>
        /// <param name="orderId"></param>
        /// <param name="orderType"></param>
        /// <param name="targetType"></param>
        /// <param name="targetKey"></param>
        /// <param name="newOrderDetail"></param>
        /// <param name="creatorUserId"></param>
        /// <param name="orderUpdateTemplateFactory">更新数据的模板工厂方法。arg1：该订单号的存在项，arg2：修改模板</param>
        /// <returns></returns>
        OrderItemDataModel_Condition AddOrUpdateOrder_Condition(long subAccountId, string orderId,
            HostingXQOrderType orderType, ClientXQOrderTargetType targetType, string targetKey,
            HostingXQOrderDetail newOrderDetail, int creatorUserId,
            Func<OrderItemDataModel_Condition, OrderUpdateTemplate_Condition> orderUpdateTemplateFactory);

        /// <summary>
        /// 更新条件单
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderUpdateTemplateFactory">更新数据的模板工厂方法。arg1：该订单号的存在项，arg2：修改模板</param>
        void UpdateOrder_Condition(string orderId,
            Func<OrderItemDataModel_Condition, OrderUpdateTemplate_Condition> orderUpdateTemplateFactory);
        
        /// <summary>
        /// 请求撤单
        /// </summary>
        /// <param name="orderIds">撤单订单 id 列表</param>
        void RequestRevokeOrders(IEnumerable<string> revokeOrderIds);

        /// <summary>
        /// 请求暂停订单
        /// </summary>
        /// <param name="orderIds">暂停订单 id 列表</param>
        void RequestSuspendOrders(IEnumerable<string> suspendOrderIds);

        /// <summary>
        /// 请求启动订单
        /// </summary>
        /// <param name="orderIds">恢复订单 id 列表</param>
        void RequestResumeOrders(IEnumerable<string> resumeOrderIds);

        /// <summary>
        /// 请求强追
        /// </summary>
        /// <param name="strongChaseOrderIds"></param>
        void RequestStrongChaseOrders(IEnumerable<string> strongChaseOrderIds);


        /// <summary>
        /// 某个标的在交易中的子账户列表
        /// </summary>
        /// <param name="targetKey">标的 key</param>
        /// <param name="targetType">标的类型</param>
        /// <returns></returns>
        IEnumerable<long> OnTradingSubAccountIdsOfTarget(string targetKey, ClientXQOrderTargetType targetType);
    }
}
