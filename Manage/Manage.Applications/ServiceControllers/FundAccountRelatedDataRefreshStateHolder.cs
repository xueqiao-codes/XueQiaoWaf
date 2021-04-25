using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.Shared.Model;

namespace Manage.Applications.ServiceControllers
{
    /// <summary>
    /// 资金账户相关的数据刷新状态 holder
    /// </summary>
    public class FundAccountRelatedDataRefreshStateHolder
    {
        public FundAccountRelatedDataRefreshStateHolder(long fundAccountId)
        {
            this.FundAccountId = fundAccountId;
        }

        public long FundAccountId { get; private set; }

        /// <summary>
        /// 数据列表刷新状态
        /// </summary>
        public DataRefreshState DataRefreshState { get; set; }
    }
}
