using lib.xqclient_base.thriftapi_mediation.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.Shared.Model
{
    /// <summary>
    /// 服务端数据刷新结果包装
    /// </summary>
    public class ServerDataRefreshResultWrapper
    {
        // 是否真正地发起服务端数据刷新请求
        public bool HasRequestRefresh;

        // 本次刷新后的数据刷新状态
        public DataRefreshState RefreshState;

        // 请求刷新的结果信息。可以保存多个接口请求结果信息
        public IInterfaceInteractResponse[] RefreshRequestResp;
    }

    public class ServerDataRefreshResultWrapper<TData> : ServerDataRefreshResultWrapper
    {
        /// <summary>
        /// 刷新结果数据。如果<see cref="HasRequestRefresh"/>为 False，则该值可能为缓存数据
        /// </summary>
        public TData ResultData;
    }
}
