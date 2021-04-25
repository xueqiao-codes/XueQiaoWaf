using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.graph.xiaoha.chart.thriftapi;
using XueQiaoFoundation.Shared.Interface;

namespace Touyan.Interface.application
{
    public interface ITouyanChartCacheCtrl : ICacheController<long, Chart>
    {
    }
}
