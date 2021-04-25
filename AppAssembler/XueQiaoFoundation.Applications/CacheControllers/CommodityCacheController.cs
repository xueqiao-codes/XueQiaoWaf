using NativeModel.Contract;
using Prism.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.trade.hosting.proxy;
using XueQiaoFoundation.Interfaces.Applications;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;


namespace XueQiaoFoundation.Applications.CacheControllers
{
    [Export(typeof(ICommodityCacheController)),
     Export(typeof(IXueQiaoFoundationSingletonController)),
     PartCreationPolicy(CreationPolicy.Shared)]
    internal class CommodityCacheController : ICommodityCacheController, IXueQiaoFoundationSingletonController, IInternalCacheController<int>
    {
        private readonly Lazy<ILoginUserManageService> loginUserManageService;
        private readonly ILoginDataService loginDataService;
        private readonly ConcurrentDictionary<int, NativeCommodity> cacheCommodityDictionary;

        [ImportingConstructor]
        public CommodityCacheController(Lazy<ILoginUserManageService> loginUserManageService,
            ILoginDataService loginDataService)
        {
            this.loginUserManageService = loginUserManageService;
            this.loginDataService = loginDataService;
            cacheCommodityDictionary = new ConcurrentDictionary<int, NativeCommodity>();

            loginUserManageService.Value.HasLogouted += RecvUserHasLogouted;
        }

        public void Cache(int key, NativeCommodity value)
        {
            if (value == null) return;
            if (loginDataService.ProxyLoginResp != null)
            {
                cacheCommodityDictionary.AddOrUpdate(key, value, (_id, oldItem) => value);
            }
        }

        public void RemoveCache(int key)
        {
            cacheCommodityDictionary.TryRemove(key, out NativeCommodity ignore);
        }

        public void ClearAllCaches()
        {
            cacheCommodityDictionary.Clear();
        }

        public Dictionary<int, NativeCommodity> AllCaches()
        {
            var tars = cacheCommodityDictionary.ToArray().ToDictionary(p => p.Key, p => p.Value);
            return tars;
        }

        public NativeCommodity GetCache(int key)
        {
            cacheCommodityDictionary.TryGetValue(key, out NativeCommodity tar);
            return tar;
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
