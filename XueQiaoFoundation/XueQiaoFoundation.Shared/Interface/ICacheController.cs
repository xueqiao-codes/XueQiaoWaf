using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.Shared.Interface
{
    public interface ICacheController<TKey, TValue>
    {
        void Cache(TKey key, TValue value);
        
        Dictionary<TKey, TValue> AllCaches();

        TValue GetCache(TKey key);
    }
}
