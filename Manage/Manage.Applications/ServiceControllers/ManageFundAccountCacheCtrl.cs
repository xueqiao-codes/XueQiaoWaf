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
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;

namespace Manage.Applications.ServiceControllers
{
    [Export(typeof(IManageFundAccountCacheCtrl)), Export(typeof(IManageModuleSingletonController)),
        PartCreationPolicy(CreationPolicy.Shared)]
    internal class ManageFundAccountCacheCtrl : IManageFundAccountCacheCtrl, IManageModuleSingletonController
    {
        private readonly IEventAggregator eventAggregator;
        private readonly ILoginDataService loginDataService;
        private readonly Lazy<ILoginUserManageService> loginUserManageService;
        private readonly ConcurrentDictionary<long, HostingTradeAccount> cachedItemsDictionary
            = new ConcurrentDictionary<long, HostingTradeAccount>();

        [ImportingConstructor]
        public ManageFundAccountCacheCtrl(IEventAggregator eventAggregator,
            ILoginDataService loginDataService,
            Lazy<ILoginUserManageService> loginUserManageService)
        {
            this.eventAggregator = eventAggregator;
            this.loginDataService = loginDataService;
            this.loginUserManageService = loginUserManageService;
            loginUserManageService.Value.HasLogouted += RecvUserHasLogouted;
        }

        public void Cache(long key, HostingTradeAccount value)
        {
            if (value == null) return;
            if (loginDataService.ProxyLoginResp != null)
            {
                cachedItemsDictionary.AddOrUpdate(key, value, (_id, oldItem) => value);
            }
        }

        public void RemoveCache(long key)
        {
            cachedItemsDictionary.TryRemove(key, out HostingTradeAccount ignore);
        }

        public void ClearAllCaches()
        {
            cachedItemsDictionary.Clear();
        }

        public Dictionary<long, HostingTradeAccount> AllCaches()
        {
            var tars = cachedItemsDictionary.ToArray().ToDictionary(p => p.Key, p => p.Value);
            return tars;
        }

        public HostingTradeAccount GetCache(long key)
        {
            cachedItemsDictionary.TryGetValue(key, out HostingTradeAccount tar);
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
