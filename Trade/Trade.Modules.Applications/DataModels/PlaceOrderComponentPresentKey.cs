using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoWaf.Trade.Modules.Applications.DataModels
{
    public class PlaceOrderComponentPresentKey : Tuple<long?, ClientXQOrderTargetType?, string>
    {
        public PlaceOrderComponentPresentKey(long? subAccountId, ClientXQOrderTargetType? targetType, string targetKey) : base(subAccountId, targetType, targetKey)
        {
            this.SubAccountId = subAccountId;
            this.TargetType = targetType;
            this.TargetKey = targetKey;
        }

        public long? SubAccountId { get; private set; }

        public ClientXQOrderTargetType? TargetType { get; private set; }

        public string TargetKey { get; private set; }
    }
}
