using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib.xqclient_base.thriftapi_mediation
{
    /// <summary>
    /// 接口交互参数
    /// </summary>
    public class StubInterfaceInteractParams
    {
        /// <summary>
        /// 是否需要经过通过器
        /// </summary>
        public bool IsPassThroughResponse = true;

        /// <summary>
        /// 传输的连接超时时间
        /// </summary>
        public int TransportConnectTimeoutMS = 10000;

        /// <summary>
        /// 传输的读取超时时间
        /// </summary>
        public int TransportReadTimeoutMS = 10000;

        /// <summary>
        /// 是否记录接口交互信息
        /// </summary>
        public bool LogInterfaceInteractInfo = true;

        /// <summary>
        /// 是否记录接口请求参数
        /// </summary>
        public bool LogInterfaceRequestArgs = true;

        /// <summary>
        /// 是否记录接口返回结果信息
        /// </summary>
        public bool LogInterfaceReturnResult = true;

        /// <summary>
        /// 自定义信息，在请求生命周期中流通，可用于存放请求的唯一标识等信息
        /// </summary>
        public readonly Dictionary<string, object> CustomInfos = new Dictionary<string, object>();
    }
}
