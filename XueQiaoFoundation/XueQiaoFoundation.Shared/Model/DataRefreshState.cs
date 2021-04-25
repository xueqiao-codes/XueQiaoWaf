using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.Shared.Model
{
    /// <summary>
    /// 数据刷新状态
    /// </summary>
    public enum DataRefreshState
    {
        NotRefresh = 0,
        Refreshing = 1,
        SuccessRefreshed = 2,
        FailedRefreshed = 3,
        CanceledRefresh = 4
    }
}
