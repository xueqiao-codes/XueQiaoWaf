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
    [Export(typeof(IUserSubAccountRelatedItemCacheController)),
     Export(typeof(IXueQiaoFoundationSingletonController)),
     PartCreationPolicy(CreationPolicy.Shared)]
    internal class UserSubAccountRelatedItemCacheController : IUserSubAccountRelatedItemCacheController, IXueQiaoFoundationSingletonController, IInternalCacheController<UserSubAccountRelatedCacheKey>
    {
        private readonly Lazy<ILoginUserManageService> loginUserManageService;
        private readonly ILoginDataService loginDataService;
        private readonly ConcurrentDictionary<UserSubAccountRelatedCacheKey, HostingSubAccountRelatedItem> cacheItemDictionary;

        [ImportingConstructor]
        public UserSubAccountRelatedItemCacheController(Lazy<ILoginUserManageService> loginUserManageService,
            ILoginDataService loginDataService)
        {
            this.loginUserManageService = loginUserManageService;
            this.loginDataService = loginDataService;
            cacheItemDictionary = new ConcurrentDictionary<UserSubAccountRelatedCacheKey, HostingSubAccountRelatedItem>();

            loginUserManageService.Value.HasLogouted += RecvUserHasLogouted;
        }

        public void Cache(UserSubAccountRelatedCacheKey key, HostingSubAccountRelatedItem value)
        {
            if (value == null) return;
            if (loginDataService.ProxyLoginResp != null)
            {
                cacheItemDictionary.AddOrUpdate(key, value, (_id, oldItem) => value);
            }
        }

        public HostingSubAccountRelatedItem GetCache(UserSubAccountRelatedCacheKey key)
        {
            cacheItemDictionary.TryGetValue(key, out HostingSubAccountRelatedItem tar);
            return tar;
        }

        public Dictionary<UserSubAccountRelatedCacheKey, HostingSubAccountRelatedItem> AllCaches()
        {
            var tars = cacheItemDictionary.ToArray().ToDictionary(p => p.Key, p => p.Value);
            return tars;
        }

        public void RemoveCache(UserSubAccountRelatedCacheKey key)
        {
            cacheItemDictionary.TryRemove(key, out HostingSubAccountRelatedItem ignore);
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
