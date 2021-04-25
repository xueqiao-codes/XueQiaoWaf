using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.BusinessResources.Models
{
    // 雪橇应用首选项
    public class XqAppPreference
    {
        // 语言
        public string Language { get; set; }

        // 主题
        public string AppTheme { get; set; }

        // 订单出错音频提示文件名
        public string OrderErrAudioFileName { get; set; }

        // 订单状态不明音频提示文件名
        public string OrderAmbiguousAudioFileName { get; set; }

        // 订单触发音频提示文件名
        public string OrderTriggeredAudioFileName { get; set; }
        
        // 订单成交音频提示文件名
        public string OrderTradedAudioFileName { get; set; }

        // 瘸腿音频提示文件名
        public string LameTradedAudioFileName { get; set; }
        
        // 订单其他提示音频文件名
        public string OrderOtherNotifyAudioFileName { get; set; }

        // 下单时是否要进行确认
        public bool? PlaceOrderNeedConfirm { get; set; }

        // 订单暂停是否要进行确认
        public bool? SuspendOrderNeedConfirm { get; set; }

        // 订单启动是否要进行确认
        public bool? ResumeOrderNeedConfirm { get; set; }

        // 订单强追是否要进行确认
        public bool? StrongChaseOrderNeedConfirm { get; set; }

        // 订单撤单是否要进行确认
        public bool? RevokeOrderNeedConfirm { get; set; }

        public override string ToString()
        {
            return $"{{" +
                $"Language:{Language}, " +
                $"AppTheme:{AppTheme}, " +
                $"OrderErrAudioFileName:{OrderErrAudioFileName}, " +
                $"OrderAmbiguousAudioFileName:{OrderAmbiguousAudioFileName}, " +
                $"OrderTriggeredAudioFileName:{OrderTriggeredAudioFileName}, " +
                $"OrderTradedAudioFileName:{OrderTradedAudioFileName}, " +
                $"LameTradedAudioFileName:{LameTradedAudioFileName}, " +
                $"OrderOtherNotifyAudioFileName:{OrderOtherNotifyAudioFileName}, " +
                $"PlaceOrderNeedConfirm:{PlaceOrderNeedConfirm}, " +
                $"SuspendOrderNeedConfirm:{SuspendOrderNeedConfirm}, " +
                $"ResumeOrderNeedConfirm:{ResumeOrderNeedConfirm}, " +
                $"StrongChaseOrderNeedConfirm:{StrongChaseOrderNeedConfirm}, " +
                $"RevokeOrderNeedConfirm:{RevokeOrderNeedConfirm}" +
                $"}}";
        }
    }
}
