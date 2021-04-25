using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Touyan.Interface.model
{
    /// <summary>
    /// 投研图表浏览历史项 model
    /// </summary>
    public class ChartViewHistory
    {
        public long ChartId { get; set; }

        public long ViewTimestamp { get; set; }
    }
}
