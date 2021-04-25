using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.graph.xiaoha.chart.thriftapi;
using XueQiaoFoundation.Shared.Model;

namespace Touyan.Interface.application
{
    public delegate void TouyanChartFolderListRefreshStateChanged(DataRefreshState refreshState);

    /// <summary>
    /// 投研图表文件夹服务管理协议
    /// </summary>
    public interface ITouyanChartFolderServiceCtrl
    {
        event TouyanChartFolderListRefreshStateChanged FolderListRefreshStateChanged;

        Task<ServerDataRefreshResultWrapper<IEnumerable<ChartFolder>>> RefreshFolderListIfNeed();

        /// <summary>
        /// 强制刷新列表
        /// </summary>
        /// <returns></returns>
        Task<ServerDataRefreshResultWrapper<IEnumerable<ChartFolder>>> RefreshFolderListForce();

    }
}
