using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.Shared.Model
{
    /// <summary>
    /// 本地的数据刷新结果包装
    /// </summary>
    public class LocalDataRefreshResultWrapper
    {
        // 是否真正地发起数据刷新请求
        public bool HasRequestRefresh;

        // 本次刷新后的数据刷新状态
        public DataRefreshState RefreshState;

        // 请求异常列表
        public Exception[] RequestExceptions;
    }

    public class LocalDataRefreshResultWrapper<TData> : LocalDataRefreshResultWrapper
    {
        /// <summary>
        /// 刷新结果数据。如果<see cref="HasRequestRefresh"/>为 False，则该值可能为缓存数据
        /// </summary>
        public TData ResultData;
    }
}
