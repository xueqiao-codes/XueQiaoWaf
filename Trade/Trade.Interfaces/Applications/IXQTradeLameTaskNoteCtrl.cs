using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.BusinessResources.DataModels;

namespace XueQiaoWaf.Trade.Interfaces.Applications
{

    /// <summary>
    /// 登录用户的雪橇瘸腿成交 Task Note 管理协议
    /// </summary>
    public interface IXQTradeLameTaskNoteCtrl
    {
        /// <summary>
        /// 若上次该操作账户的瘸腿成交 Task Note 列表刷新不成功，则本次刷新
        /// </summary>
        /// <param name="subAccountId"></param>
        void RefreshTaskNotesIfNeed(long subAccountId);

        /// <summary>
        /// 强制刷新某个操作账户的瘸腿成交 Task Note 列表
        /// </summary>
        /// <param name="subAccountId"></param>
        void RefreshTaskNotesForce(long subAccountId);

        /// <summary>
        /// 获取某项瘸腿成交 Task Note
        /// </summary>
        /// <param name="itemKey"></param>
        /// <returns></returns>
        XQTradeLameTaskNote GetTaskNote(XQTradeItemKey itemKey);

        /// <summary>
        /// 请求删除某项瘸腿成交 Task Note
        /// </summary>
        /// <param name="itemKey"></param>
        void RequestDeleteTaskNote(XQTradeItemKey itemKey);
    }
}
