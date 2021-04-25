using NativeModel.Trade;
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
    /// 登录用户的雪橇持仓管理协议
    /// </summary>
    internal interface IXqTargetPositionItemsController
    {
        /// <summary>
        /// 刷新某个子账户的持仓列表
        /// <paramref name="subAccountId">子账户 id</paramref>
        /// </summary>
        void RefreshPositionItemsIfNeed(long subAccountId);

        /// <summary>
        /// 强制刷新某个子账户的持仓列表
        /// <paramref name="subAccountId">子账户 id</paramref>
        /// </summary>
        void RefreshPositionItemsForce(long subAccountId);

        /// <summary>
        /// 子账户的散列持仓列表刷新变化事件
        /// </summary>
        event SubAccountAnyDataRefreshStateChanged PositionItemsRefreshStateChanged;

        /// <summary>
        /// 获取某个持仓项
        /// </summary>
        /// <param name="itemKey"></param>
        /// <returns></returns>
        TargetPositionDataModel GetPositionItem(TargetPositionKey itemKey);

        /// <summary>
        /// 某个标的在持仓中的子账户列表
        /// </summary>
        /// <param name="targetKey">标的 key</param>
        /// <param name="targetType">标的类型</param>
        /// <returns></returns>
        IEnumerable<long> ExistPositionSubAccountIdsOfTarget(string targetKey, ClientXQOrderTargetType targetType);
        
        /// <summary>
        /// 请求删除过期持仓
        /// </summary>
        /// <param name="itemKey"></param>
        void RequestDeleteExpiredXqTargetPosition(TargetPositionKey itemKey);
    }
}
