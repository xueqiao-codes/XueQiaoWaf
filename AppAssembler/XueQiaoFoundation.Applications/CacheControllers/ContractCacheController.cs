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
    [Export(typeof(IContractCacheController)), Export(typeof(IXueQiaoFoundationSingletonController)),PartCreationPolicy(CreationPolicy.Shared)]
    internal class ContractCacheController : IContractCacheController, IXueQiaoFoundationSingletonController, IInternalCacheController<int>
    {
        private readonly Lazy<ILoginUserManageService> loginUserManageService;
        private readonly ILoginDataService loginDataService;
        private readonly ConcurrentDictionary<int, NativeContract> cacheContractDictionary;

        [ImportingConstructor]
        public ContractCacheController(Lazy<ILoginUserManageService> loginUserManageService,
            ILoginDataService loginDataService)
        {
            this.loginUserManageService = loginUserManageService;
            this.loginDataService = loginDataService;
            cacheContractDictionary = new ConcurrentDictionary<int, NativeContract>();

            loginUserManageService.Value.HasLogouted += RecvUserHasLogouted;
        }

        public void Cache(int key, NativeContract value)
        {
            if (value == null) return;
            if (loginDataService.ProxyLoginResp != null)
            {
                cacheContractDictionary.AddOrUpdate(key, value, (_id, oldItem) => value);
            }
        }

        public void RemoveCache(int key)
        {
            cacheContractDictionary.TryRemove(key, out NativeContract ignore);
        }

        public void ClearAllCaches()
        {
            cacheContractDictionary.Clear();
        }

        public Dictionary<int, NativeContract> AllCaches()
        {
            var tars = cacheContractDictionary.ToArray().ToDictionary(p => p.Key, p => p.Value);
            return tars;
        }

        public NativeContract GetCache(int key)
        {
            cacheContractDictionary.TryGetValue(key, out NativeContract tar);
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
