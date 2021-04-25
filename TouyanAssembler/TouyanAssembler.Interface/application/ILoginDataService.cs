using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.graph.xiaoha.chart.terminal.ao.thriftapi;

namespace TouyanAssembler.Interface.application
{
    /// <summary>
    /// 当前登录信息服务协议
    /// </summary>
    public interface ILoginDataService
    {
        /// <summary>
        /// 当前登录信息
        /// </summary>
        XiaohaChartLandingInfo LoginLandingInfo { get; }
    }
}
