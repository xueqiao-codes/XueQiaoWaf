using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoWaf.Trade.Modules.Applications.DataModels
{
    /// <summary>
    /// 关联订单显示类型
    /// </summary>
    public enum RelatedOrderShowType
    {
        Entrusted = 1,  // 委托单
        Parked = 2,     // 预埋单
        Condition = 3,  // 条件单
    }
}
