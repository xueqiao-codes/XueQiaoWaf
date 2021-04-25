using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business_foundation_lib.quotationpush
{
    /// <summary>
    /// 订阅行情 key
    /// </summary>
    public class SubscribeQuotationKey
    {
        public SubscribeQuotationKey(string platform, string contractSymbol)
        {
            this.Platform = platform;
            this.ContractSymbol = contractSymbol;
        }

        public string Platform { get; private set; }

        public string ContractSymbol { get; private set; }

        public readonly Dictionary<string, string> CustomInfos = new Dictionary<string, string>();

        public override string ToString()
        {
            return $"SubscribeQuotationKey{{Platform:{Platform}, ContractSymbol:{ContractSymbol}}}";
        }
    }
}
