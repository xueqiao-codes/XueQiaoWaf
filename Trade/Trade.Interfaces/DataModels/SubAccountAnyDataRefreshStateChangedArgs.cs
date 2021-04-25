using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.Shared.Model;

namespace XueQiaoWaf.Trade.Interfaces.DataModels
{
    /// <summary>
    /// 子账户的某些数据刷新状态变化参数
    /// </summary>
    public class SubAccountAnyDataRefreshStateChangedArgs : EventArgs
    {
        public SubAccountAnyDataRefreshStateChangedArgs(int userId, long subAccountId, DataRefreshState refreshState)
        {
            this.UserId = userId;
            this.SubAccountId = subAccountId;
            this.RefreshState = refreshState;
        }

        public int UserId { get; private set; }

        public long SubAccountId { get; private set; }

        public DataRefreshState RefreshState { get; private set; }
    }

    /// <summary>
    /// 子账户的某些数据刷新状态变化 delegate
    /// </summary>
    /// <param name="args"></param>
    public delegate void SubAccountAnyDataRefreshStateChanged(SubAccountAnyDataRefreshStateChangedArgs args);
}
