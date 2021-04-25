using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using xueqiao.graph.xiaoha.chart.thriftapi;

namespace Touyan.Interface.datamodel
{
    /// <summary>
    /// 投研图表浏览历史项 model
    /// </summary>
    public class ChartViewHistoryDM : Model
    {
        public ChartViewHistoryDM(long chartId)
        {
            this.ChartId = chartId;
        }
        
        public long ChartId { get; private set; }
        
        private long viewTimestamp;
        public long ViewTimestamp
        {
            get { return viewTimestamp; }
            set { SetProperty(ref viewTimestamp, value); }
        }

        private Chart chartInfo;
        public Chart ChartInfo
        {
            get { return chartInfo; }
            set { SetProperty(ref chartInfo, value); }
        }
    }
}
