using Prism.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using xueqiao.trade.hosting;
using xueqiao.trade.hosting.proxy;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;

namespace Manage.Applications.ServiceControllers
{
    [Export(typeof(IManageSubAccountCacheCtrl)), Export(typeof(IManageModuleSingletonController)),
        PartCreationPolicy(CreationPolicy.Shared)]
    internal class ManageSubAccountCacheCtrl : IManageSubAccountCacheCtrl, IManageModuleSingletonController
    {
        private readonly IEventAggregator eventAggregator;
        private readonly ILoginDataService loginDataService;
        private readonly Lazy<ILoginUserManageService> loginUserManageService;
        private readonly ConcurrentDictionary<long, HostingSubAccount> cachedItemsDictionary
            = new ConcurrentDictionary<long, HostingSubAccount>();

        [ImportingConstructor]
        public ManageSubAccountCacheCtrl(IEventAggregator eventAggregator,
            ILoginDataService loginDataService,
            Lazy<ILoginUserManageService> loginUserManageService)
        {
            this.eventAggregator = eventAggregator;
            this.loginDataService = loginDataService;
            this.loginUserManageService = loginUserManageService;

            loginUserManageService.Value.HasLogouted += RecvUserHasLogouted;
        }

        public void Cache(long key, HostingSubAccount value)
        {
            if (value == null) return;
            if (loginDataService.ProxyLoginResp != null)
            {
                cachedItemsDictionary.AddOrUpdate(key, value, (_id, oldItem) => value);
            }
        }

        public void RemoveCache(long key)
        {
            cachedItemsDictionary.TryRemove(key, out HostingSubAccount ignore);
        }

        public void ClearAllCaches()
        {
            cachedItemsDictionary.Clear();
        }

        public Dictionary<long, HostingSubAccount> AllCaches()
        {
            var tars = cachedItemsDictionary.ToArray().ToDictionary(p => p.Key, p => p.Value);
            return tars;
        }

        public HostingSubAccount GetCache(long key)
        {
            cachedItemsDictionary.TryGetValue(key, out HostingSubAccount tar);
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
