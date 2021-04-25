using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.BusinessResources.Models
{
    /// <summary>
    /// 货币单位 model
    /// </summary>
    public class CurrencyUnitInfo
    {
        // 货币
        public string Currency { get; set; }

        // 货币名称
        public string CurrencyName { get; set; }

        // 单位名称
        public string UnitName { get; set; }

        // 0.1倍的单位名称
        public string UnitName0_1 { get; set; }

        // 0.01倍的单位名称
        public string UnitName0_0_1 { get; set; }

        // 0.001倍的单位名称
        public string UnitName0_0_0_1 { get; set; }
    }
}
