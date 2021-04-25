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
    [Export(typeof(IExchangeCacheController)), 
     Export(typeof(IXueQiaoFoundationSingletonController)),
     PartCreationPolicy(CreationPolicy.Shared)]
    internal class ExchangeCacheController : IExchangeCacheController, IXueQiaoFoundationSingletonController, IInternalCacheController<string>
    {
        private readonly Lazy<ILoginUserManageService> loginUserManageService;
        private readonly ILoginDataService loginDataService;
        private readonly ConcurrentDictionary<string, NativeExchange> cacheExchangeDictionary;

        [ImportingConstructor]
        public ExchangeCacheController(Lazy<ILoginUserManageService> loginUserManageService,
            ILoginDataService loginDataService)
        {
            this.loginUserManageService = loginUserManageService;
            this.loginDataService = loginDataService;
            cacheExchangeDictionary = new ConcurrentDictionary<string, NativeExchange>();

            loginUserManageService.Value.HasLogouted += RecvUserHasLogouted;
        }

        public void Cache(string key, NativeExchange value)
        {
            if (value == null) return;
            if (loginDataService.ProxyLoginResp != null)
            {
                cacheExchangeDictionary.AddOrUpdate(key, value, (_id, oldItem) => value);
            }
        }

        public void RemoveCache(string key)
        {
            cacheExchangeDictionary.TryRemove(key, out NativeExchange ignore);
        }

        public void ClearAllCaches()
        {
            cacheExchangeDictionary.Clear();
        }

        public Dictionary<string, NativeExchange> AllCaches()
        {
            var tars = cacheExchangeDictionary.ToArray().ToDictionary(p=>p.Key, p=>p.Value);
            return tars;
        }

        public NativeExchange GetCache(string key)
        {
            cacheExchangeDictionary.TryGetValue(key, out NativeExchange tar);
            return tar;
        }

        public void Shutdown()
        {
            loginUserManageService.Value.HasLogouted -= RecvUserHasLogouted;
            ClearAllCaches();
        }

        private void RecvUserHasLogouted(ProxyLoginResp lastLoginResp)
        {
            ClearAllCaches();
        }
    }
}
