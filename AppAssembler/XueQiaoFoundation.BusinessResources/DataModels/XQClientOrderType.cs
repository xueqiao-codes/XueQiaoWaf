using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.BusinessResources.DataModels
{
    /// <summary>
    /// 客户端的订单类型
    /// </summary>
    public enum XQClientOrderType
    {
        Entrusted = 1,  // 委托单
        Condition = 2,  // 条件单
        Parked = 3,     // 预埋单
    }
}
