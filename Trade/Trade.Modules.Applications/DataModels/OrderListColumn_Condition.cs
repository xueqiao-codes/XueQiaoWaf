using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoWaf.Trade.Modules.Applications.DataModels
{
    public enum OrderListColumn_Condition
    {
        OrderState = 1,         // 状态
        TargetType = 3,         // 标的类型 
        ConditionOrderLabel = 4, // 条件单类型
        Name = 5,               // 标的名称
        TriggerConditionInfo = 6,    // 条件信息。比如：“对价>=1805”“最新价<=1806”
        CreateTime = 9,     // 下单时间
        EffectDateType = 10,    // 有效期类型
        OrderId = 11,       // 订单号
        CreatorUserName = 12,   // 下单用户名
        SubAccountName = 13,    // 操作账户名
        StateMsg = 14,      // 状态信息

        UpdateTime = 40,        // 更新时间
        CreatorUserId = 41,     // 下单用户 id
        SubAccountId = 42,      // 操作账户 id
        DataVersion = 43,       // 数据版本

        ChildOrder = 44,      // 子订单ID
        EffectEndTime = 45,    // 有效结束时间
    }
}
