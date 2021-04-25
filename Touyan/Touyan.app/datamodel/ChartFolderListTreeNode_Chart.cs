using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.graph.xiaoha.chart.thriftapi;

namespace Touyan.app.datamodel
{
    /// <summary>
    /// 图表文件夹列表树 Chart 类型项
    /// </summary>
    public class ChartFolderListTreeNode_Chart : ChartFolderListTreeNodeBase
    {
        public ChartFolderListTreeNode_Chart(long chartId, long parentFolderId) : base(parentFolderId)
        {
            this.ChartId = chartId;
        }

        public long ChartId { get; private set; }

        private Chart chart;
        public Chart Chart
        {
            get { return chart; }
            set { SetProperty(ref chart, value); }
        }
    }
}
