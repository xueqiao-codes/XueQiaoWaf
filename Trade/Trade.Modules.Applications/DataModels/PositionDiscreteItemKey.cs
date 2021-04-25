using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoWaf.Trade.Modules.Applications.DataModels
{
    /// <summary>
    /// 散列持仓项 key
    /// </summary>
    public class PositionDiscreteItemKey : Tuple<long,long>
    {
        public PositionDiscreteItemKey(long contractId, long subAccountId) : base(contractId, subAccountId)
        {
            this.ContractId = contractId;
            this.SubAccountId = subAccountId;
        }

        public long ContractId { get; private set; }

        public long SubAccountId { get; private set; }
    }
}
