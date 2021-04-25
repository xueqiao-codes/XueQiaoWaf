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
    [Export(typeof(IManageFundAccountItemsController)), Export(typeof(IManageModuleSingletonController)),
        PartCreationPolicy(CreationPolicy.Shared)]
    internal class ManageFundAccountItemsController : IManageFundAccountItemsController, IManageModuleSingletonController
    {
        private readonly IEventAggregator eventAggregator;
        private readonly ILoginDataService loginDataService;
        private readonly Lazy<ILoginUserManageService> loginUserManageService;
        private readonly ManageFundAccountItemsService manFundAccountItemsService;
        private readonly IManageFundAccountQueryCtrl manFundAccountQueryCtrl;
        
        private readonly object fundAccountItemsRefreshLock = new object();
        private readonly TaskFactory fundAccountItemsRefreshTaskFactory = new TaskFactory(new OrderedTaskScheduler());
        private CancellationTokenSource fundAccountItemsRefreshCancelTokenSource;
        private DataRefreshState fundAccountItemsRefreshState = DataRefreshState.NotRefresh;

        private readonly Dictionary<long, HostingTradeAccount> fundAccountItemsDictionary
            = new Dictionary<long, HostingTradeAccount>();
        private readonly object fundAccountItemsDictionaryLock = new object();

        [ImportingConstructor]
        public ManageFundAccountItemsController(IEventAggregator eventAggregator,
            ILoginDataService loginDataService,
            Lazy<ILoginUserManageService> loginUserManageService,
            ManageFundAccountItemsService manFundAccountItemsService,
            IManageFundAccountQueryCtrl manFundAccountQueryCtrl)
        {
            this.eventAggregator = eventAggregator;
            this.loginDataService = loginDataService;
            this.loginUserManageService = loginUserManageService;
            this.manFundAccountItemsService = manFundAccountItemsService;
            this.manFundAccountQueryCtrl = manFundAccountQueryCtrl;

            loginUserManageService.Value.HasLogouted += RecvUserHasLogouted;
        }

        public void Shutdown()
        {
            loginUserManageService.Value.HasLogouted -= RecvUserHasLogouted;
            CancelFundAccountItemsRefresh();
            RemoveAllFundAccountItems();
        }

        public void RefreshFundAccountItemsIfNeed()
        {
            lock (fundAccountItemsRefreshLock)
            {
                if (fundAccountItemsRefreshState == DataRefreshState.NotRefresh
                    || fundAccountItemsRefreshState == DataRefreshState.FailedRefreshed)
                {
                    InternalRefreshFundAccountItems();
                }
            }
        }

        public void RefreshFundAccountItemsForce()
        {
            lock (fundAccountItemsRefreshLock)
            {
                InternalRefreshFundAccountItems();
            }
        }

        public IEnumerable<HostingTradeAccount> AllFundAccountItems
        {
            get
            {
                IEnumerable<HostingTradeAccount> values = null;
                lock (fundAccountItemsDictionaryLock)
                {
                    values = fundAccountItemsDictionary.Values.ToArray();
                }
                return values;
            }
        }

        private void RecvUserHasLogouted(ProxyLoginResp lastLoginResp)
        {
            CancelFundAccountItemsRefresh();
            RemoveAllFundAccountItems();
        }

        private void RemoveAllFundAccountItems()
        {
            lock (fundAccountItemsDictionaryLock)
            {
                fundAccountItemsDictionary.Clear();
                DispatcherHelper.CheckBeginInvokeOnUI(() => 
                {
                    manFundAccountItemsService.AccountItems.Clear();
                });
            }
        }

        private void RemoveFundAccountItemsWithId(IEnumerable<long> rmFundAccountIds)
        {
            if (rmFundAccountIds?.Any() != true) return;
            lock (fundAccountItemsDictionaryLock)
            {
                foreach (var rmId in rmFundAccountIds)
                {
                    fundAccountItemsDictionary.Remove(rmId);
                }
                DispatcherHelper.CheckBeginInvokeOnUI(() => 
                {
                    manFundAccountItemsService.AccountItems.RemoveAll(i => rmFundAccountIds.Contains(i.TradeAccountId));
                });
            }
        }

        private void CancelFundAccountItemsRefresh()
        {
            lock (fundAccountItemsRefreshLock)
            {
                if (fundAccountItemsRefreshCancelTokenSource != null)
                {
                    fundAccountItemsRefreshCancelTokenSource.Cancel();
                    fundAccountItemsRefreshCancelTokenSource.Dispose();
                    fundAccountItemsRefreshCancelTokenSource = null;
                }
                fundAccountItemsRefreshState = DataRefreshState.NotRefresh;
            }
        }

        private void InternalRefreshFundAccountItems()
        {
            CancelFundAccountItemsRefresh();

            fundAccountItemsRefreshCancelTokenSource = new CancellationTokenSource();
            var cancelToken = fundAccountItemsRefreshCancelTokenSource.Token;

            fundAccountItemsRefreshTaskFactory.StartNew(() =>
            {
                if (cancelToken.IsCancellationRequested) return;
                var loginSession = loginDataService.ProxyLoginResp?.HostingSession;
                if (loginSession == null) return;

                fundAccountItemsRefreshState = DataRefreshState.Refreshing;

                var resp = manFundAccountQueryCtrl.QueryAllFundAccounts(cancelToken);
                
                fundAccountItemsRefreshState = (resp?.CorrectResult != null) ? DataRefreshState.SuccessRefreshed : DataRefreshState.FailedRefreshed;

                if (resp == null) return;
                if (cancelToken.IsCancellationRequested) return;

                var fillItems = resp.CorrectResult?.ToArray() ?? new HostingTradeAccount[] { };
                lock (fundAccountItemsDictionaryLock)
                {
                    var rmItemIds = fundAccountItemsDictionary.Keys
                        .Except(fillItems.Select(i => i.TradeAccountId).ToArray());
                    RemoveFundAccountItemsWithId(rmItemIds);
                    AddOrUpdateFundAccountItems(fillItems);
                }
                eventAggregator.GetEvent<ManageFundAccountItemsRefreshEvent>()
                            .Publish(new ManageFundAccountItemsRefreshEventArgs(loginSession.Token, fillItems));
            }, cancelToken);
        }

        private void AddOrUpdateFundAccountItems(IEnumerable<HostingTradeAccount> newItems)
        {
            if (newItems?.Any() != true) return;
            var loginSession = loginDataService.ProxyLoginResp?.HostingSession;
            if (loginSession == null) return;

            lock (fundAccountItemsDictionaryLock)
            {
                foreach (var item in newItems)
                {
                    var fundAccountId = item.TradeAccountId;
                    HostingTradeAccount existItem = null;
                    if (fundAccountItemsDictionary.TryGetValue(fundAccountId, out existItem))
                    {
                        // 存在，改变内容
                        TBase refExistItem = existItem;
                        ThriftHelper.UnserializeBytesToTBase(ref refExistItem, ThriftHelper.SerializeTBaseToBytes(item));
                    }
                    else
                    {
                        // 不存在，则添加
                        fundAccountItemsDictionary[fundAccountId] = item;
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            manFundAccountItemsService.AccountItems.Add(item);
                        });
                    }
                }
            }
        }
    }
}
