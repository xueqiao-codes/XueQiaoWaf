using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoWaf.Trade.Interfaces.DataModels;

namespace XueQiaoWaf.Trade.Modules.Applications.ServiceControllers
{
    /// <summary>
    /// 资金项管理协议
    /// </summary>
    public interface IFundItemsController
    {
        /// <summary>
        /// 刷新某个子账户的资金列表
        /// <paramref name="subAccountId">子账户 id</paramref>
        /// </summary>
        void RefreshFundItemsIfNeed(long subAccountId);

        /// <summary>
        /// 强制刷新某个子账户的资金列表
        /// <paramref name="subAccountId">子账户 id</paramref>
        /// </summary>
        void RefreshFundItemsForce(long subAccountId);

        /// <summary>
        /// 资金列表刷新变化事件
        /// </summary>
        event SubAccountAnyDataRefreshStateChanged FundItemsRefreshStateChanged;
    }
}
