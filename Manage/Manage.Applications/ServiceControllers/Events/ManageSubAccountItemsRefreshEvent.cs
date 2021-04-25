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
    /// 管理的操作账户列表刷新事件
    /// </summary>
    internal class ManageSubAccountItemsRefreshEvent : PubSubEvent<ManageSubAccountItemsRefreshEventArgs>
    {
    }

    internal class ManageSubAccountItemsRefreshEventArgs
    {
        public ManageSubAccountItemsRefreshEventArgs(string loginUserToken, IEnumerable<HostingSubAccount> subAccountItems)
        {
            this.LoginUserToken = loginUserToken;
            this.SubAccountItems = subAccountItems;
        }

        public readonly string LoginUserToken;
        public readonly IEnumerable<HostingSubAccount> SubAccountItems;
    }
}
