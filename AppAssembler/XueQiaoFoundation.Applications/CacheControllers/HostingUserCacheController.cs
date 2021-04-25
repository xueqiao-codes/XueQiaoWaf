using Prism.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.trade.hosting;
using xueqiao.trade.hosting.proxy;
using XueQiaoFoundation.Interfaces.Applications;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;


namespace XueQiaoFoundation.Applications.CacheControllers
{
    [Export(typeof(IHostingUserCacheController)), Export(typeof(IXueQiaoFoundationSingletonController)), PartCreationPolicy(CreationPolicy.Shared)]
    internal class HostingUserCacheController : IHostingUserCacheController, IXueQiaoFoundationSingletonController, IInternalCacheController<int>
    {
        private readonly Lazy<ILoginUserManageService> loginUserManageService;
        private readonly ILoginDataService loginDataService;
        private readonly ConcurrentDictionary<int, HostingUser> cacheItemDictionary;

        [ImportingConstructor]
        public HostingUserCacheController(Lazy<ILoginUserManageService> loginUserManageService,
            ILoginDataService loginDataService)
        {
            this.loginUserManageService = loginUserManageService;
            this.loginDataService = loginDataService;
            cacheItemDictionary = new ConcurrentDictionary<int, HostingUser>();

            loginUserManageService.Value.HasLogouted += RecvUserHasLogouted;
        }

        public void Cache(int key, HostingUser value)
        {
            if (value == null) return;
            if (loginDataService.ProxyLoginResp != null)
            {
                cacheItemDictionary.AddOrUpdate(key, value, (_id, oldItem) => value);
            }
        }

        public HostingUser GetCache(int key)
        {
            cacheItemDictionary.TryGetValue(key, out HostingUser tar);
            return tar;
        }

        public Dictionary<int, HostingUser> AllCaches()
        {
            var tars = cacheItemDictionary.ToArray().ToDictionary(p => p.Key, p => p.Value);
            return tars;
        }

        public void RemoveCache(int key)
        {
            cacheItemDictionary.TryRemove(key, out HostingUser ignore);
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
