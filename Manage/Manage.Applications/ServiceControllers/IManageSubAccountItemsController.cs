using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.trade.hosting;

namespace Manage.Applications.ServiceControllers
{
    /// <summary>
    /// 管理的操作账户列表 controller
    /// </summary>
    internal interface IManageSubAccountItemsController
    {
        /// <summary>
        /// 刷新操作账户列表
        /// </summary>
        void RefreshSubAccountItemsIfNeed();

        /// <summary>
        /// 强制刷新操作账户列表
        /// </summary>
        void RefreshSubAccountItemsForce();

        /// <summary>
        /// 所有的操作账户
        /// </summary>
        IEnumerable<HostingSubAccount> AllSubAccountItems { get; }
    }
}
