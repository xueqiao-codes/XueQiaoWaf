using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.trade.hosting;

namespace Manage.Applications.ServiceControllers.Events
{
    /// <summary>
    /// 管理的资金账户列表刷新事件
    /// </summary>
    internal class ManageFundAccountItemsRefreshEvent : PubSubEvent<ManageFundAccountItemsRefreshEventArgs>
    {
    }

    internal class ManageFundAccountItemsRefreshEventArgs
    {
        public ManageFundAccountItemsRefreshEventArgs(string loginUserToken, IEnumerable<HostingTradeAccount> fundAccountItems)
        {
            this.LoginUserToken = loginUserToken;
            this.FundAccountItems = fundAccountItems;
        }

        public readonly string LoginUserToken;
        public readonly IEnumerable<HostingTradeAccount> FundAccountItems;
    }
}
