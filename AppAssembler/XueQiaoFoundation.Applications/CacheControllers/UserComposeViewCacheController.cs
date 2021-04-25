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
    [Export(typeof(IUserComposeViewCacheController)), Export(typeof(IXueQiaoFoundationSingletonController)), PartCreationPolicy(CreationPolicy.Shared)]
    internal class UserComposeViewCacheController : IUserComposeViewCacheController, IXueQiaoFoundationSingletonController, IInternalCacheController<UserComposeViewCacheKey>
    {
        private readonly Lazy<ILoginUserManageService> loginUserManageService;
        private readonly ILoginDataService loginDataService;
        private readonly ConcurrentDictionary<UserComposeViewCacheKey, NativeComposeViewDetail> cacheItemDictionary;

        [ImportingConstructor]
        public UserComposeViewCacheController(Lazy<ILoginUserManageService> loginUserManageService,
            ILoginDataService loginDataService)
        {
            this.loginUserManageService = loginUserManageService;
            this.loginDataService = loginDataService;
            cacheItemDictionary = new ConcurrentDictionary<UserComposeViewCacheKey, NativeComposeViewDetail>();

            loginUserManageService.Value.HasLogouted += RecvUserHasLogouted;
        }

        public void Cache(UserComposeViewCacheKey key, NativeComposeViewDetail value)
        {
            if (value == null) return;
            if (loginDataService.ProxyLoginResp != null)
            {
                cacheItemDictionary.AddOrUpdate(key, value, (_id, oldItem) => value);
            }
        }
        
        public NativeComposeViewDetail GetCache(UserComposeViewCacheKey key)
        {
            cacheItemDictionary.TryGetValue(key, out NativeComposeViewDetail tar);
            return tar;
        }

        public Dictionary<UserComposeViewCacheKey, NativeComposeViewDetail> AllCaches()
        {
            var tars = cacheItemDictionary.ToArray().ToDictionary(p => p.Key, p => p.Value);
            return tars;
        }

        public void RemoveCache(UserComposeViewCacheKey key)
        {
            cacheItemDictionary.TryRemove(key, out NativeComposeViewDetail ignore);
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
