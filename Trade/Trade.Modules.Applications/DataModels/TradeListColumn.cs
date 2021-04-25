using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoWaf.Trade.Modules.Applications.DataModels
{
    public enum TradeListColumn
    {
        Direction = 1,
        TargetType = 2,
        Name = 3,
        TradePrice = 4,
        TradeVolume = 5,
        SourceType = 6,  // 来源类型
        CreateTime = 7,
        TradeId = 8,
        OrderId = 9,
        CreatorUserName = 10,
        SubAccountName = 11,

        SubUserId = 30,
        SubAccountId = 31,
    }
}
