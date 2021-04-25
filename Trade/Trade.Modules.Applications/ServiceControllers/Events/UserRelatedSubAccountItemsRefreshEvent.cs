using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.trade.hosting;

namespace XueQiaoWaf.Trade.Modules.Applications.ServiceControllers.Events
{
    /// <summary>
    /// 用户的子账户列表已刷新 event
    /// </summary>
    public class UserRelatedSubAccountItemsRefreshEvent : PubSubEvent<UserRelatedSubAccountItemsRefreshEventArgs>
    {
    }

    public class UserRelatedSubAccountItemsRefreshEventArgs
    {
        public UserRelatedSubAccountItemsRefreshEventArgs(string loginUserToken, IEnumerable<HostingSubAccountRelatedItem> relatedSubAccountItems)
        {
            this.LoginUserToken = loginUserToken;
            this.RelatedSubAccountItems = relatedSubAccountItems;
        }

        public readonly string LoginUserToken;
        public readonly IEnumerable<HostingSubAccountRelatedItem> RelatedSubAccountItems;
    }
}
