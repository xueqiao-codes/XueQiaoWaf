using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.trade.hosting;
using XueQiaoFoundation.Shared.Interface;

namespace Manage.Applications.ServiceControllers
{
    internal interface IManageFundAccountCacheCtrl : ICacheController<long, HostingTradeAccount>
    {
        void RemoveCache(long key);

        void ClearAllCaches();
    }
}
