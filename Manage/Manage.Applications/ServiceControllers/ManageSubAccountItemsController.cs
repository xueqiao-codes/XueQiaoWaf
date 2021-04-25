using Manage.Applications.ServiceControllers.Events;
using Manage.Applications.Services;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Schedulers;
using Thrift.Protocol;
using xueqiao.trade.hosting;
using xueqiao.trade.hosting.proxy;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Model;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;

namespace Manage.Applications.ServiceControllers
{
    [Export(typeof(IManageSubAccountItemsController)), Export(typeof(IManageModuleSingletonController)),
        PartCreationPolicy(CreationPolicy.Shared)]
    internal class ManageSubAccountItemsController : IManageSubAccountItemsController, IManageModuleSingletonController
    {
        private readonly IEventAggregator eventAggregator;
        private readonly ILoginDataService loginDataService;
        private readonly Lazy<ILoginUserManageService> loginUserManageService;
        private readonly ManageSubAccountItemsService manageSubAccountItemsService;
        private readonly IManageSubAccountQueryCtrl manageSubAccountQueryCtrl;

        private readonly object subAccountItemsRefreshLock = new object();
        private readonly TaskFactory subAccountItemsRefreshTaskFactory = new TaskFactory(new OrderedTaskScheduler());
        private CancellationTokenSource subAccountItemsRefreshCancelTokenSource;
        private DataRefreshState subAccountItemsRefreshState = DataRefreshState.NotRefresh;

        private readonly Dictionary<long, HostingSubAccount> subAccountItemsDictionary
            = new Dictionary<long, HostingSubAccount>();
        private readonly object subAccountItemsDictionaryLock = new object();

        [ImportingConstructor]
        public ManageSubAccountItemsController(IEventAggregator eventAggregator,
            ILoginDataService loginDataService,
            Lazy<ILoginUserManageService> loginUserManageService,
            ManageSubAccountItemsService manageSubAccountItemsService,
            IManageSubAccountQueryCtrl manageSubAccountQueryCtrl)
        {
            this.eventAggregator = eventAggregator;
            this.loginDataService = loginDataService;
            this.loginUserManageService = loginUserManageService;
            this.manageSubAccountItemsService = manageSubAccountItemsService;
            this.manageSubAccountQueryCtrl = manageSubAccountQueryCtrl;

            loginUserManageService.Value.HasLogouted += RecvUserHasLogouted;
        }

        public void Shutdown()
        {
            loginUserManageService.Value.HasLogouted -= RecvUserHasLogouted;
            CancelSubAccountItemsRefresh();
            RemoveAllSubAccountItems();
        }

        public void RefreshSubAccountItemsIfNeed()
        {
            lock (subAccountItemsRefreshLock)
            {
                if (subAccountItemsRefreshState == DataRefreshState.NotRefresh
                    || subAccountItemsRefreshState == DataRefreshState.FailedRefreshed)
                {
                    InternalRefreshSubAccountItems();
                }
            }
        }

        public void RefreshSubAccountItemsForce()
        {
            lock (subAccountItemsRefreshLock)
            {
                InternalRefreshSubAccountItems();
            }
        }

        public IEnumerable<HostingSubAccount> AllSubAccountItems
        {
            get
            {
                IEnumerable<HostingSubAccount> values = null;
                lock (subAccountItemsDictionaryLock)
                {
                    values = subAccountItemsDictionary.Values.ToArray();
                }
                return values;
            }
        }

        private void RecvUserHasLogouted(ProxyLoginResp lastLoginResp)
        {
            CancelSubAccountItemsRefresh();
            RemoveAllSubAccountItems();
        }

        private void RemoveAllSubAccountItems()
        {
            lock (subAccountItemsDictionaryLock)
            {
                subAccountItemsDictionary.Clear();
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    manageSubAccountItemsService.AccountItems.Clear();
                });
            }
        }

        private void RemoveSubAccountItemsWithId(IEnumerable<long> rmSubAccountIds)
        {
            if (rmSubAccountIds?.Any() != true) return;
            lock (subAccountItemsDictionaryLock)
            {
                foreach (var rmId in rmSubAccountIds)
                {
                    subAccountItemsDictionary.Remove(rmId);
                }
                DispatcherHelper.CheckBeginInvokeOnUI(() => 
                {
                    manageSubAccountItemsService.AccountItems.RemoveAll(i => rmSubAccountIds.Contains(i.SubAccountId));
                });
            }
        }

        private void CancelSubAccountItemsRefresh()
        {
            lock (subAccountItemsRefreshLock)
            {
                if (subAccountItemsRefreshCancelTokenSource != null)
                {
                    subAccountItemsRefreshCancelTokenSource.Cancel();
                    subAccountItemsRefreshCancelTokenSource.Dispose();
                    subAccountItemsRefreshCancelTokenSource = null;
                }
                subAccountItemsRefreshState = DataRefreshState.NotRefresh;
            }
        }

        private void InternalRefreshSubAccountItems()
        {
            CancelSubAccountItemsRefresh();

            subAccountItemsRefreshCancelTokenSource = new CancellationTokenSource();
            var cancelToken = subAccountItemsRefreshCancelTokenSource.Token;

            subAccountItemsRefreshTaskFactory.StartNew(() =>
            {
                if (cancelToken.IsCancellationRequested) return;
                var loginSession = loginDataService.ProxyLoginResp?.HostingSession;
                if (loginSession == null) return;

                subAccountItemsRefreshState = DataRefreshState.Refreshing;

                var resp = manageSubAccountQueryCtrl.QueryAllSubAccounts(cancelToken);
                
                subAccountItemsRefreshState = (resp?.CorrectResult != null) ? DataRefreshState.SuccessRefreshed : DataRefreshState.FailedRefreshed;

                if (resp == null) return;
                if (cancelToken.IsCancellationRequested) return;

                var fillItems = resp.CorrectResult?.ToArray() ?? new HostingSubAccount[] { };
                lock (subAccountItemsDictionaryLock)
                {
                    var rmItemIds = subAccountItemsDictionary.Keys
                        .Except(fillItems.Select(i => i.SubAccountId).ToArray());
                    RemoveSubAccountItemsWithId(rmItemIds);
                    AddOrUpdateSubAccountItems(fillItems);
                }
                eventAggregator.GetEvent<ManageSubAccountItemsRefreshEvent>()
                            .Publish(new ManageSubAccountItemsRefreshEventArgs(loginSession.Token, fillItems));
            }, cancelToken);
        }
        
        private void AddOrUpdateSubAccountItems(IEnumerable<HostingSubAccount> newItems)
        {
            if (newItems?.Any() != true) return;
            var loginSession = loginDataService.ProxyLoginResp?.HostingSession;
            if (loginSession == null) return;

            lock (subAccountItemsDictionaryLock)
            {
                foreach (var item in newItems)
                {
                    var subAccountId = item.SubAccountId;
                    HostingSubAccount existItem = null;
                    if (subAccountItemsDictionary.TryGetValue(subAccountId, out existItem))
                    {
                        // 存在，改变内容
                        TBase refExistItem = existItem;
                        ThriftHelper.UnserializeBytesToTBase(ref refExistItem, ThriftHelper.SerializeTBaseToBytes(item));
                    }
                    else
                    {
                        // 不存在，则添加
                        subAccountItemsDictionary[subAccountId] = item;
                        DispatcherHelper.CheckBeginInvokeOnUI(() => 
                        {
                            manageSubAccountItemsService.AccountItems.Add(item);
                        });
                    }
                }
            }
        }
    }
}
