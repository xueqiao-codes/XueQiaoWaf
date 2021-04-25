using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.trade.hosting;
using XueQiaoFoundation.Shared.Interface;

namespace XueQiaoFoundation.Interfaces.Applications
{
    public interface IHostingUserCacheController : ICacheController<int, HostingUser>
    {
    }
}
