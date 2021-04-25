using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoWaf.Trade.Modules.Applications.DataModels
{
    /// <summary>
    /// 组合列表显示的字段类型
    /// </summary>
    public enum SubscribeComposeListDisplayColumn
    {
        Name = 1,           // 名称
        BidQty = 2,         // 买量
        BidPrice = 3,       // 买价
        AskPrice = 4,       // 卖价
        AskQty = 5,         // 卖量
        UpdateTime = 6,     // 更新时间
        SubscribeState = 7,     //订阅状态

        SubscribeStateMsg = 30, //订阅状态信息
        ComposeId = 31,         // 组合 id
    }
}
