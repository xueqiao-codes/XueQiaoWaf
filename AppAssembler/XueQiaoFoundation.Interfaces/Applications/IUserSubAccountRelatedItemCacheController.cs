using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.trade.hosting;
using XueQiaoFoundation.Shared.Interface;

namespace XueQiaoFoundation.Interfaces.Applications
{
    public interface IUserSubAccountRelatedItemCacheController : ICacheController<UserSubAccountRelatedCacheKey, HostingSubAccountRelatedItem>
    {
    }

    public class UserSubAccountRelatedCacheKey : Tuple<int, long>
    {
        public UserSubAccountRelatedCacheKey(int subUserId, long subAccountId) : base(subUserId, subAccountId)
        {
        }

        public int SubUserId => this.Item1;

        public long SubAccountId => this.Item2;        
    }
}
