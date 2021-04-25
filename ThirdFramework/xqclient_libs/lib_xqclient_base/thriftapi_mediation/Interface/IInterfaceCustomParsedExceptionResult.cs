using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib.xqclient_base.thriftapi_mediation.Interface
{
    /// <summary>
    /// 接口定义解析的异常结果信息
    /// </summary>
    public interface IInterfaceCustomParsedExceptionResult
    {
        /// <summary>
        /// 业务错误码
        /// </summary>
        int? BusinessErrorCode { get; }

        /// <summary>
        /// 业务错误信息
        /// </summary>
        string BusinessErrorMessage { get; }

        /// <summary>
        /// 是否 Session 无效
        /// </summary>
        bool SessionInvalid { get; }
        
    }
}
