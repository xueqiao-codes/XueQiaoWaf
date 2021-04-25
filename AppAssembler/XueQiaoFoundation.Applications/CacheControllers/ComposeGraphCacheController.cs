using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NativeModel.Trade;
using XueQiaoFoundation.Interfaces.Applications;
using Prism.Events;
using System.Collections.Concurrent;

using XueQiaoWaf.LoginUserManage.Interfaces.Applications;
using xueqiao.trade.hosting.proxy;

namespace XueQiaoFoundation.Applications.CacheControllers
{
    [Export(typeof(IComposeGraphCacheController)), Export(typeof(IXueQiaoFoundationSingletonController)), PartCreationPolicy(CreationPolicy.Shared)]
    internal class ComposeGraphCacheController : IComposeGraphCacheController, IXueQiaoFoundationSingletonController, IInternalCacheController<long>
    {
        private readonly Lazy<ILoginUserManageService> loginUserManageService;
        private readonly ILoginDataService loginDataService;
        private readonly ConcurrentDictionary<long, NativeComposeGraph> cacheItemDictionary;

        [ImportingConstructor]
        public ComposeGraphCacheController(Lazy<ILoginUserManageService> loginUserManageService,
            ILoginDataService loginDataService)
        {
            this.loginUserManageService = loginUserManageService;
            this.loginDataService = loginDataService;
            cacheItemDictionary = new ConcurrentDictionary<long, NativeComposeGraph>();

            loginUserManageService.Value.HasLogouted += RecvUserHasLogouted;
        }

        public void Cache(long key, NativeComposeGraph value)
        {
            if (value == null) return;
            if (loginDataService.ProxyLoginResp != null)
            {
                cacheItemDictionary.AddOrUpdate(key, value, (_id, oldItem) => value);
            }
        }
        
        public NativeComposeGraph GetCache(long key)
        {
            cacheItemDictionary.TryGetValue(key, out NativeComposeGraph tar);
            return tar;
        }

        public Dictionary<long, NativeComposeGraph> AllCaches()
        {
            var tars = cacheItemDictionary.ToArray().ToDictionary(p => p.Key, p => p.Value);
            return tars;
        }

        public void RemoveCache(long key)
        {
            cacheItemDictionary.TryRemove(key, out NativeComposeGraph ignore);
        }

        public void ClearAllCaches()
        {
            cacheItemDictionary.Clear();
        }

        public void Shutdown()
        {
            ClearAllCaches();
            loginUserManageService.Value.HasLogouted -= RecvUserHasLogouted;
        }

        private void RecvUserHasLogouted(ProxyLoginResp lastLoginResp)
        {
            ClearAllCaches();
        }
    }
}
