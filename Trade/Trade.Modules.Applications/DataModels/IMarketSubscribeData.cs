using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoWaf.Trade.Modules.Applications.DataModels
{
    public enum MarketSubscribeState
    {
        Unkown = 0,             // 未知
        Unsubscribed = 1,       // 未订阅
        Subscribed = 2,         // 已订阅
        Subscribing = 100,      // 订阅中
        Unsubscribing = 101,    // 退订中
    }
    
    public interface IMarketSubscribeData : INotifyPropertyChanged
    {
        /// <summary>
        /// 订阅状态
        /// </summary>
        MarketSubscribeState SubscribeState { get; }

        /// <summary>
        /// 订阅状态信息
        /// </summary>
        string SubscribeStateMsg { get; }
    }
}
