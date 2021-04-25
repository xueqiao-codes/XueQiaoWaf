using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.Shared.Interface;

namespace XueQiaoFoundation.Interfaces.Applications
{
    /// <summary>
    /// 登录用户组合视图缓存控制器
    /// </summary>
    public interface IUserComposeViewCacheController : ICacheController<UserComposeViewCacheKey, NativeComposeViewDetail>
    {
    }

    public class UserComposeViewCacheKey : Tuple<long>
    {
        public UserComposeViewCacheKey(long composeGraphId) : base(composeGraphId)
        {
        }

        public long ComposeGraphId => this.Item1;
    }
}
