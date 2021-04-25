using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeModel.Trade
{
    /// <summary>
    /// 组合行情
    /// </summary>
    public class NativeCombQuotationItem
    {
        public NativeCombQuotationItem(long composeGraphId)
        {
            this.ComposeGraphId = composeGraphId;
        }

        // 组合 id
        public readonly long ComposeGraphId;

        // 组合的行情
        public NativeQuotationItem CombQuotation { get; set; }

        // 组合的各腿行情
        public IEnumerable<NativeQuotationItem> LegQuotations { get; set; }
    }
}
