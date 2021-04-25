using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XueQiaoFoundation.BusinessResources.DataModels;

namespace Manage.Applications.DataModels
{
    /// <summary>
    /// 子账号历史持仓 data model
    /// </summary>
    public class SubAccHistoryPositionDM : PositionManageDM
    {
        public SubAccHistoryPositionDM(DiscretePositionDM positionContent, ICommand showDetailCmd, long historySettlementId) : base(positionContent, showDetailCmd)
        {
            this.HistorySettlementId = historySettlementId;
        }

        public long HistorySettlementId { get; private set; }
    }
}
