using business_foundation_lib.quotationpush;
using NativeModel.Trade;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.quotation;
using xueqiao.trade.hosting.events;
using xueqiao.trade.hosting.push.protocol;

namespace XueQiaoFoundation.Interfaces.Event.business
{
    /// <summary>
    /// 投机合约行情已订阅的事件。message 为合约 symbol
    /// </summary>
    public class SpecQuotationSubscribedEvent : PubSubEvent<SpecQuotationSubscribedEventArgs> { }

    public class SpecQuotationSubscribedEventArgs
    {
        public SpecQuotationSubscribedEventArgs(SubscribeQuotationKey subKey, bool IsSubscribedSuccess)
        {
            this.SubKey = subKey;
            this.IsSubscribedSuccess = IsSubscribedSuccess;
        }

        public SubscribeQuotationKey SubKey { get; private set; }

        public bool IsSubscribedSuccess { get; private set; }
    }

    /// <summary>
    /// 投机合约行情已取消订阅的事件。message 为合约 symbol
    /// </summary>
    public class SpecQuotationUnsubscribedEvent : PubSubEvent<SpecQuotationUnsubscribedEventArgs> { }

    public class SpecQuotationUnsubscribedEventArgs
    {
        public SpecQuotationUnsubscribedEventArgs(SubscribeQuotationKey subKey, bool IsUnsubscribedSuccess)
        {
            this.SubKey = subKey;
            this.IsUnsubscribedSuccess = IsUnsubscribedSuccess;
        }

        public SubscribeQuotationKey SubKey { get; private set; }

        public bool IsUnsubscribedSuccess { get; private set; }
    }

    /// <summary>
    /// 合约行情更新的事件
    /// </summary>
    public class SpecQuotationUpdated : PubSubEvent<NativeQuotationItem> { }

    /// <summary>
    /// 客户端数据强制同步的事件
    /// </summary>
    public class ClientDataForceSync : PubSubEvent<ClientForceSyncEvent> { }

    /// <summary>
    /// 雪橇单已创建的事件
    /// </summary>
    public class XQOrderCreated : PubSubEvent<XQOrderCreatedEvent> { }

    /// <summary>
    /// 雪橇订单更新的事件
    /// </summary>
    public class XQOrderChanged : PubSubEvent<XQOrderChangedEvent> { }

    /// <summary>
    /// 雪橇订单过期事件
    /// </summary>
    public class XQOrderExpired : PubSubEvent<XQOrderExpiredEvent> { }

    /// <summary>
    /// 雪橇成交列表变更事件
    /// </summary>
    public class XQTradeListChanged : PubSubEvent<XQTradeListChangedEvent> { }
    

    /// <summary>
    /// 组合行情更新的事件
    /// </summary>
    public class CombQuotationUpdated : PubSubEvent<NativeCombQuotationItem> { }


    /// <summary>
    /// 某个持仓的持仓量信息事件
    /// </summary>
    public class HostingPositionVolumeChanged : PubSubEvent<HostingPositionVolumeEvent> { }

    /// <summary>
    /// 某个持仓的资金变化信息事件
    /// </summary>
    public class HostingPositionFundChanged : PubSubEvent<HostingPositionFundEvent> { }

    /// <summary>
    /// 子账户资金变化信息事件
    /// </summary>
    public class HostingFundChanged : PubSubEvent<HostingFundEvent> { }

    /// <summary>
    /// 雪橇标的的持仓 summary 变化事件
    /// </summary>
    public class XQTargetPositionSummaryChanged : PubSubEvent<StatPositionSummaryChangedEvent> { }

    /// <summary>
    /// 雪橇标的的持仓动态信息变化事件
    /// </summary>
    public class XQTargetPositionDynamicInfoEvent : PubSubEvent<StatPositionDynamicInfoEvent> { }

    /// <summary>
    /// 雪橇任务笔记创建事件
    /// </summary>
    public class XQTaskNoteCreated : PubSubEvent<TaskNoteCreatedEvent> { }

    /// <summary>
    /// 雪橇任务笔记删除事件
    /// </summary>
    public class XQTaskNoteDeleted : PubSubEvent<TaskNoteDeletedEvent> { }

}
