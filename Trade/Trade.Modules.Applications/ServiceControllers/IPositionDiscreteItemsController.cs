using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoWaf.Trade.Interfaces.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;

namespace XueQiaoWaf.Trade.Modules.Applications.ServiceControllers
{
    /// <summary>
    /// 登录用户的散列持仓项管理协议
    /// </summary>
    public interface IPositionDiscreteItemsController
    {
        /// <summary>
        /// 刷新某个子账户的散列持仓列表
        /// <paramref name="subAccountId">子账户 id</paramref>
        /// </summary>
        void RefreshDiscretePositionItemsIfNeed(long subAccountId);

        /// <summary>
        /// 强制刷新某个子账户的散列持仓列表
        /// <paramref name="subAccountId">子账户 id</paramref>
        /// </summary>
        void RefreshDiscretePositionItemsForce(long subAccountId);

        /// <summary>
        /// 子账户的散列持仓列表刷新变化事件
        /// </summary>
        event SubAccountAnyDataRefreshStateChanged DiscretePositionItemsRefreshStateChanged;
        
        /// <summary>
        /// 某个合约存在持仓的子账户列表
        /// </summary>
        /// <param name="contractId">合约id</param>
        /// <returns></returns>
        IEnumerable<long> ExistPositionSubAccountIdsOfContract(int contractId);
        
        /// <summary>
        /// 请求删除过期持仓
        /// </summary>
        /// <param name="itemKey"></param>
        void RequestDeleteExpiredPosition(PositionDiscreteItemKey itemKey);
    }
}
