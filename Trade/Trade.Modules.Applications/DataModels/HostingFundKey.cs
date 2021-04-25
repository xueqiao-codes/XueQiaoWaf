using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoWaf.Trade.Modules.Applications.DataModels
{
    public class HostingFundKey : Tuple<long, bool, string>
    {
        public HostingFundKey(long subAccountId, bool isBaseCurrency, string currency) : base(subAccountId, isBaseCurrency, currency)
        {
            this.SubAccountId = subAccountId;
            this.IsBaseCurrency = isBaseCurrency;
            this.Currency = currency;
        }

        public long SubAccountId { get; private set; }

        public bool IsBaseCurrency { get; private set; }

        public string Currency { get; private set; }
    }
}
