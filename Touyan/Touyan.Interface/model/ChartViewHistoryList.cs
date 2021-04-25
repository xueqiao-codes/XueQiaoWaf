using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Touyan.Interface.model
{
    /// <summary>
    /// 包装 <see cref="ChartViewHistory"/> 类型的数组 model
    /// </summary>
    public class ChartViewHistoryList
    {
        public ChartViewHistory[] HistoryList { get; set; }
    }
}
