using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.Shared.Model;

namespace XueQiaoWaf.Trade.Modules.Applications.ServiceControllers
{
    /// <summary>
    /// 子账户相关的数据刷新状态 holder
    /// </summary>
    public class SubAccountAnyDataRefreshStateHolder
    {
        public SubAccountAnyDataRefreshStateHolder(long subAccountId)
        {
            this.SubAccountId = subAccountId;
        }

        public long SubAccountId { get; private set; }

        /// <summary>
        /// 数据列表刷新状态
        /// </summary>
        public DataRefreshState DataRefreshState { get; set; }
    }
}
