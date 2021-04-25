using lib.xqclient_base.thriftapi_mediation.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.graph.xiaoha.chart.thriftapi;

namespace Touyan.Interface.application
{
    /// <summary>
    /// 投研图表项查询控制器 协议
    /// </summary>
    public interface ITouyanChartQueryCtrl
    {
        /// <summary>
        /// 请求查询投研图表。
        /// </summary>
        /// <param name="queryOption">查询选项</param>
        /// <param name="pageOption">分页选项</param>
        /// <returns></returns>
        IInterfaceInteractResponse<ChartPage> RequestQueryChart(ReqChartOption queryOption, IndexedPageOption pageOption);

        /// <summary>
        /// 查询图表。如果 <see cref="ignoreCache"/> 为 false，则先查询本地缓存，存在则返回缓存，否则继续向服务端查询
        /// </summary>
        /// <param name="chartId">图表 id</param>
        /// <param name="ignoreCache">是否忽略缓存</param>
        /// <param name="serverQueryResp">服务端查询结果</param>
        /// <returns></returns>
        Chart QueryChart(long chartId, bool ignoreCache, out IInterfaceInteractResponse serverQueryResp);
    }
}
