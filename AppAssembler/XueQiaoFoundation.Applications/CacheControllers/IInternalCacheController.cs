using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.Applications.CacheControllers
{
    internal interface IInternalCacheController<TKey>
    {
        void RemoveCache(TKey key);

        void ClearAllCaches();
    }
}
