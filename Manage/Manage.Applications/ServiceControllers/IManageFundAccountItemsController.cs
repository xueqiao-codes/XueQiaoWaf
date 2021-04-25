using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.trade.hosting;

namespace Manage.Applications.ServiceControllers
{
    /// <summary>
    /// 管理的资金账户列表 controller
    /// </summary>
    internal interface IManageFundAccountItemsController
    {
        /// <summary>
        /// 刷新资金账户列表
        /// </summary>
        void RefreshFundAccountItemsIfNeed();

        /// <summary>
        /// 强制刷新资金账户列表
        /// </summary>
        void RefreshFundAccountItemsForce();

        /// <summary>
        /// 所有的资金账户
        /// </summary>
        IEnumerable<HostingTradeAccount> AllFundAccountItems { get; }
    }
}
