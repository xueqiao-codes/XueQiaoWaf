using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.trade.hosting;

namespace XueQiaoWaf.Trade.Modules.Applications.ServiceControllers
{
    /// <summary>
    /// 登录用户的操作账户项管理协议
    /// </summary>
    internal interface IRelatedSubAccountItemsController
    {
        /// <summary>
        /// 刷新子账户列表
        /// </summary>
        void RefreshRelatedSubAccountItemsIfNeed();

        /// <summary>
        /// 刷新子账户列表，然后用一个子账户列表用于填充刷新后的列表
        /// </summary>
        /// <param name="fillRelatedSubAccountItems">用于填充刷新后列表的子账户列表</param>
        void RefreshRelatedSubAccountItemsIfNeed(IEnumerable<HostingSubAccountRelatedItem> fillRelatedSubAccountItems);

        /// <summary>
        /// 强制刷新子账户列表
        /// </summary>
        void RefreshRelatedSubAccountItemsForce();
        
        /// <summary>
        /// 获取当前子账户列表
        /// </summary>
        IEnumerable<HostingSubAccountRelatedItem> RelatedSubAccountItems { get; }
    }
}
