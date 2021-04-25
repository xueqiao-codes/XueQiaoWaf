using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.Shared.Model;

namespace Manage.Applications.ServiceControllers
{
    /// <summary>
    /// 资金账户相关的数据刷新状态变化参数
    /// </summary>
    public class FundAccountRelatedDataRefreshStateChangedArgs
    {
        public FundAccountRelatedDataRefreshStateChangedArgs(int userId, long fundAccountId, DataRefreshState refreshState)
        {
            this.UserId = userId;
            this.FundAccountId = fundAccountId;
            this.RefreshState = refreshState;
        }

        public int UserId { get; private set; }

        public long FundAccountId { get; private set; }

        public DataRefreshState RefreshState { get; private set; }
    }

    /// <summary>
    /// 资金账户相关的数据刷新状态变化 delegate
    /// </summary>
    /// <param name="args"></param>
    public delegate void FundAccountRelatedDataRefreshStateChanged(FundAccountRelatedDataRefreshStateChangedArgs args);
}
