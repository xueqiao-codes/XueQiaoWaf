using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Touyan.Interface.application;
using xueqiao.graph.xiaoha.chart.thriftapi;

namespace Touyan.app.servicecontroller
{
    [   Export, 
        Export(typeof(ITouyanChartCacheCtrl)), 
        Export(typeof(ITouyanModuleServiceCtrl)),
        PartCreationPolicy(CreationPolicy.Shared)]
    internal class TouyanChartCacheCtrl : ITouyanChartCacheCtrl, ITouyanModuleServiceCtrl
    {
        private readonly ConcurrentDictionary<long, Chart> cacheDictionary = new ConcurrentDictionary<long, Chart>();

        [ImportingConstructor]
        public TouyanChartCacheCtrl()
        {
        }

        public void Initialize()
        {
        }

        public void Shutdown()
        {
            ClearAllCaches();
        }

        public void Cache(long key, Chart value)
        {
            if (value == null) return;
            cacheDictionary.AddOrUpdate(key, value, (_id, oldItem) => value);
        }

        public void RemoveCache(long key)
        {
            cacheDictionary.TryRemove(key, out Chart ignore);
        }

        public void ClearAllCaches()
        {
            cacheDictionary.Clear();
        }

        public Dictionary<long, Chart> AllCaches()
        {
            var tars = cacheDictionary.ToArray().ToDictionary(p => p.Key, p => p.Value);
            return tars;
        }

        public Chart GetCache(long key)
        {
            cacheDictionary.TryGetValue(key, out Chart tar);
            return tar;
        }

    }
}
