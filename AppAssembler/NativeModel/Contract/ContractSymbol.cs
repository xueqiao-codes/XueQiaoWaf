using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeModel.Contract
{
    public class ContractSymbol : Tuple<string, int, string, string>
    {
        public ContractSymbol(string exchangeMic, int commodityType, string commodityCode, string contractCode) 
            : base(exchangeMic, commodityType, commodityCode, contractCode)
        {
            this.ExchangeMic = exchangeMic;
            this.CommodityType = commodityType;
            this.CommodityCode = commodityCode;
            this.ContractCode = contractCode;
        }

        public string ExchangeMic { get; private set; }
        public int CommodityType { get; private set; }
        public string CommodityCode { get; private set; }
        public string ContractCode { get; private set; }
    }
}
