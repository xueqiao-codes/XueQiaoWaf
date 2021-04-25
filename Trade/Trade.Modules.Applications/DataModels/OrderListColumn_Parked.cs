using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoWaf.Trade.Modules.Applications.DataModels
{
    /// <summary>
    /// 预埋单列表显示列
    /// </summary>
    public enum OrderListColumn_Parked
    {
        OrderState = 1,         // 状态
        Direction = 2,          // 买/卖方向
        TargetType = 3,         // 标的类型 
        OrderType = 4,          // 订单类型
        Name = 5,               // 标的名称
        TriggerOrderPrice = 6,  // 触发订单价格
        Quantity = 7,       // 数量
        CreateTime = 8,     // 下单时间
        TriggerTime = 9,    // 触发时间
        OrderId = 10,       // 订单号
        CreatorUserName = 11,   // 下单用户名
        SubAccountName = 12,    // 操作账户名
        StateMsg = 13,      // 状态信息

        UpdateTime = 40,        // 更新时间
        CreatorUserId = 41,     // 下单用户 id
        SubAccountId = 42,      // 操作账户 id
        DataVersion = 43,       // 数据版本

        ChildOrder = 44,      // 子订单
    }
}
