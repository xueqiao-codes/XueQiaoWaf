using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib.xqclient_base.thriftapi_mediation.Interface
{
    /// <summary>
    /// 接口交互回报
    /// </summary>
    public interface IInterfaceInteractResponse
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        string Servant { get; }

        /// <summary>
        /// 方法名称
        /// </summary>
        string InterfaceName { get; }

        /// <summary>
        /// 接口交互信息
        /// </summary>
        IInterfaceInteractInformation InteractInformation { get; }

        /// <summary>
        /// 原始异常信息
        /// </summary>
        Exception SourceException { get; }

        /// <summary>
        /// 是否存在传输异常
        /// </summary>
        bool HasTransportException { get; }

        /// <summary>
        /// http response 状态码
        /// </summary>
        int? HttpResponseStatusCode { get; }

        /// <summary>
        /// 自定义解析的异常结果
        /// </summary>
        IInterfaceCustomParsedExceptionResult CustomParsedExceptionResult { get; }
    }

    /// <summary>
    /// 带正确结果的接口交互回报
    /// </summary>
    /// <typeparam name="T">正确结果的类型</typeparam>
    public interface IInterfaceInteractResponse<T> : IInterfaceInteractResponse
    {
        /// <summary>
        /// 正确的结果信息
        /// </summary>
        T CorrectResult { get; }
    }
}
