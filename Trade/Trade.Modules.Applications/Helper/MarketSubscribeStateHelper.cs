using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Helper
{
    public static class MarketSubscribeStateHelper
    {
        public static string DefaultStateMsgForSubscribeState(MarketSubscribeState subState)
        {
            switch (subState)
            {
                case MarketSubscribeState.Unkown:
                    return Properties.Resources.DefaultMsg_MarketSubscribeStateUnkown;
                case MarketSubscribeState.Unsubscribed:
                    return Properties.Resources.DefaultMsg_MarketSubscribeStateUnsubscribed;
                case MarketSubscribeState.Subscribed:
                    return Properties.Resources.DefaultMsg_MarketSubscribeStateSubscribed;
                case MarketSubscribeState.Subscribing:
                    return Properties.Resources.DefaultMsg_MarketSubscribeStateSubscribing;
                case MarketSubscribeState.Unsubscribing:
                    return Properties.Resources.DefaultMsg_MarketSubscribeStateUnsubscribing;
            }
            return Properties.Resources.DefaultMsg_MarketSubscribeStateUnkown;
        }
    }
}
