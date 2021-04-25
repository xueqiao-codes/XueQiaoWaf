using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.BusinessResources.DataModels;

namespace XueQiaoWaf.Trade.Interfaces.Events
{
    /// <summary>
    /// 成交瘸腿任务笔记本地事件
    /// </summary>
    public class TradeLameTaskNoteNativeEvent : PubSubEvent<TradeLameTaskNoteNativeEventPayload>
    {

    }

    /// <summary>
    /// 事件消息体
    /// </summary>
    public class TradeLameTaskNoteNativeEventPayload
    {
        public TradeLameTaskNoteNativeEventPayload(TradeLameTaskNoteNativeEventType eventType)
        {
            this.EventType = eventType;
        }

        // 事件类型
        public readonly TradeLameTaskNoteNativeEventType EventType;

        /// <summary>
        /// 任务笔记信息。在 EventType 为 <see cref="TradeLameTaskNoteTipEventType.Create"/> 时需要赋值
        /// </summary>
        public XQTradeLameTaskNote TaskNote;

        /// <summary>
        /// 任务笔记 Key。在 EventType 为 <see cref="TradeLameTaskNoteNativeEventType.Delete"/> 时需要赋值
        /// </summary>
        public XQTradeItemKey ItemKey;
    }
    
    public enum TradeLameTaskNoteNativeEventType
    {
        Create = 1,     // 创建
        Delete = 2      // 删除
    }
}
