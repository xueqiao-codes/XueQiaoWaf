using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib.xqclient_base.thriftapi_mediation.Interface
{
    /// <summary>
    /// 接口请求返回信息
    /// </summary>
    public interface IInterfaceInteractInformation
    {
        /// <summary>
        /// 耗时
        /// </summary>
        double CostTimeMS { get; }

        /// <summary>
        /// 开始请求时间
        /// </summary>
        double BeginRequestTimestampMS { get; }

        /// <summary>
        /// 服务接入 url
        /// </summary>
        string ServiceAccessUrl { get; }
        
    }
}
