using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoWaf.Trade.Modules.Applications.DataModels
{
    /// <summary>
    /// 委托订单列表显示列
    /// </summary>
    public enum OrderListColumn_Entrusted
    {
        OrderState = 1,         // 状态
        Direction = 2,          // 买/卖方向
        TargetType = 3,         // 标的类型 
        Name = 4,               // 标的名称
        Price = 5,          // 价格
        Quantity = 6,       // 数量
        TradeVolume = 7,    // 成交数量
        TradeAvgPrice = 8,  // 成交均价
        ExecuteDetail = 9,  // 执行详情
        OrderType = 10,     // 订单类型
        CreateTime = 11,    // 下单时间
        EffectDateType = 12,    // 有效期类型
        StateMsg = 13,      // 状态信息
        OrderId = 14,       // 订单号
        CreatorUserName = 15,   // 下单用户名
        SubAccountName = 16,    // 操作账户名

        UpdateTime = 40,        // 更新时间
        CreatorUserId = 41,     // 下单用户 id
        SubAccountId = 42,      // 操作账户 id
        DataVersion = 43,       // 数据版本
        ComposeLegTradeSummary = 44,    // 组合腿成交概要

        ParentOrder = 45,             // 父订单
        ComposeOrderSendType = 46,      // （组合订单）触发执行方式
        EffectEndTime = 47,    // 有效结束时间
    }
}
