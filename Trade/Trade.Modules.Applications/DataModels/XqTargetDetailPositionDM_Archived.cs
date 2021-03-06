using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoWaf.Trade.Modules.Applications.DataModels
{
    /// <summary>
    /// 已归档的雪橇标的持仓详情项 data model
    /// </summary>
    public class XqTargetDetailPositionDM_Archived : XqTargetDetailPositionDM
    {
        public XqTargetDetailPositionDM_Archived(string targetKey, ClientXQOrderTargetType targetType, long subAccountId,
            long detailItemId) : base(targetKey, targetType, subAccountId)
        {
            this.DetailItemId = detailItemId;
        }

        /// <summary>
        /// 明细项 id
        /// </summary>
        public long DetailItemId { get; private set; }
    }
}
