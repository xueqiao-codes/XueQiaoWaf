using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.trade.hosting.arbitrage.thriftapi;
using XueQiaoWaf.Trade.Interfaces.DataModels;

namespace XueQiaoWaf.Trade.Interfaces.Applications
{
    /// <summary>
    /// 登录用户的成交项管理协议
    /// </summary>
    public interface ITradeItemsController
    {
        /// <summary>
        /// 刷新某个子账户的成交列表
        /// <paramref name="subAccountId">子账户 id</paramref>
        /// </summary>
        void RefreshTradeItemsIfNeed(long subAccountId);
        
        /// <summary>
        /// 强制刷新某个子账户的成交列表
        /// <paramref name="subAccountId">子账户 id</paramref>
        /// </summary>
        void RefreshTradeItemsForce(long subAccountId);

        /// <summary>
        /// 子账户的成交列表刷新状态变化事件
        /// </summary>
        event SubAccountAnyDataRefreshStateChanged TradeItemsRefreshStateChanged;

    }
}
