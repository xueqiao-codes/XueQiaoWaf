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
using XueQiaoFoundation.Interfaces.Applications;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Model;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;
using XueQiaoWaf.Trade.Modules.Applications.ServiceControllers.Events;

namespace XueQiaoWaf.Trade.Modules.Applications.ServiceControllers
{
    /// <summary>
    /// 子账户列表控制器
    /// </summary>
    [Export(typeof(IRelatedSubAccountItemsController)), Export(typeof(ITradeModuleSingletonController)),
        PartCreationPolicy(CreationPolicy.Shared)]
    internal class RelatedSubAccountItemsController : IRelatedSubAccountItemsController, ITradeModuleSingletonController
    {
        class RelatedSubAccountItemKey : Tuple<int, long>
        {
            public RelatedSubAccountItemKey(int userId, long subAccountId) : base(userId, subAccountId)
            {
                this.UserId = userId;
                this.SubAccountId = subAccountId;
            }

            public readonly int UserId;
            public readonly long SubAccountId;
        }

        private readonly IUserSubAccountRelatedItemQueryController subAccountRelatedItemQueryCtrl;
        private readonly ILoginDataService loginDataService;
        private readonly Lazy<ILoginUserManageService> loginUserManageService;
        private readonly IEventAggregator eventAggregator;

        /// <summary>
        /// 相关操作账户同步任务工厂。该工厂生产的任务是顺序执行，将增删、刷新包装成任务执行，才可保证同步的准确性
        /// </summary>
        private readonly TaskFactory relatedSubAccountItemsSyncTaskFactory = new TaskFactory(new OrderedTaskScheduler());
        /// <summary>
        /// 相关操作账户列表
        /// </summary>
        private readonly Dictionary<RelatedSubAccountItemKey, HostingSubAccountRelatedItem> relatedSubAccountItemsDictionary
            = new Dictionary<RelatedSubAccountItemKey, HostingSubAccountRelatedItem>();
        private readonly object relatedSubAccountItemsDictionaryLock = new object();


        private CancellationTokenSource relatedSubAccountItemsRefreshCTS;
        private readonly object relatedSubAccountItemsRefreshLock = new object();
        private DataRefreshState relatedSubAccountItemsRefreshState = DataRefreshState.NotRefresh;
        

        [ImportingConstructor]
        public RelatedSubAccountItemsController(
            IUserSubAccountRelatedItemQueryController subAccountRelatedItemQueryCtrl,
            ILoginDataService loginDataService,
            Lazy<ILoginUserManageService> loginUserManageService,
            IEventAggregator eventAggregator)
        {
            this.subAccountRelatedItemQueryCtrl = subAccountRelatedItemQueryCtrl;
            this.loginDataService = loginDataService;
            this.loginUserManageService = loginUserManageService;
            this.eventAggregator = eventAggregator;

            loginUserManageService.Value.HasLogouted += RecvUserHasLogouted;
        }

        public void Shutdown()
        {
            loginUserManageService.Value.HasLogouted -= RecvUserHasLogouted;
            CancelRelatedSubAccountItemsRefresh(true);
        }

        public void RefreshRelatedSubAccountItemsIfNeed()
        {
            RefreshRelatedSubAccountItems(false);
        }

        public void RefreshRelatedSubAccountItemsIfNeed(IEnumerable<HostingSubAccountRelatedItem> fillRelatedSubAccountItems)
        {
            relatedSubAccountItemsSyncTaskFactory.StartNew(() => 
            {
                var landingInfo = loginDataService.LandingInfo;
                if (landingInfo == null) return;

                bool needRefresh = !IsRefreshingOrSuccessRefreshed(SafeGetRelatedSubAccountItemsRefreshState());
                if (needRefresh)
                {
                    SafeSetRelatedSubAccountItemsRefreshState(DataRefreshState.SuccessRefreshed);

                    var fillList = fillRelatedSubAccountItems ?? new HostingSubAccountRelatedItem[] { };
                    lock (relatedSubAccountItemsDictionaryLock)
                    {
                        if (loginDataService.LandingInfo?.Token != landingInfo.Token) return;
                        UnsafeRemoveAllRelatedSubAccountItems();
                        UnsafeAddOrUpdateRelatedSubAccountItems(fillList);
                    }
                    eventAggregator.GetEvent<UserRelatedSubAccountItemsRefreshEvent>().Publish(new UserRelatedSubAccountItemsRefreshEventArgs(landingInfo.Token, fillList?.ToArray()));
                }
            });
        }
        
        public void RefreshRelatedSubAccountItemsForce()
        {
            RefreshRelatedSubAccountItems(true);
        }

        public IEnumerable<HostingSubAccountRelatedItem> RelatedSubAccountItems
        {
            get
            {
                IEnumerable<HostingSubAccountRelatedItem> values = null; 
                lock (relatedSubAccountItemsDictionaryLock)
                {
                    values = relatedSubAccountItemsDictionary.Values.ToArray();
                }
                return values;
            }
        }


        private void RecvUserHasLogouted(ProxyLoginResp lastLoginResp)
        {
            CancelRelatedSubAccountItemsRefresh(true);
            RemoveAllRelatedSubAccountItems();
        }

        private void RemoveAllRelatedSubAccountItems()
        {
            lock (relatedSubAccountItemsDictionaryLock)
            {
                UnsafeRemoveAllRelatedSubAccountItems();
            }
        }

        private void UnsafeRemoveAllRelatedSubAccountItems()
        {
            relatedSubAccountItemsDictionary.Clear();
        }
        
        private static bool IsRefreshingOrSuccessRefreshed(DataRefreshState subAccountLastRefreshState)
        {
            return subAccountLastRefreshState == DataRefreshState.Refreshing || subAccountLastRefreshState == DataRefreshState.SuccessRefreshed;
        }

        private CancellationToken AcquireRelatedSubAccountItemsRefreshCLT()
        {
            CancellationToken clt = CancellationToken.None;
            lock (relatedSubAccountItemsRefreshLock)
            {
                if (relatedSubAccountItemsRefreshCTS == null)
                {
                    relatedSubAccountItemsRefreshCTS = new CancellationTokenSource();
                }
                clt = relatedSubAccountItemsRefreshCTS.Token;
            }
            return clt;
        }
        
        private void CancelRelatedSubAccountItemsRefresh(bool resetRefreshState)
        {
            lock (relatedSubAccountItemsRefreshLock)
            {
                if (relatedSubAccountItemsRefreshCTS != null)
                {
                    relatedSubAccountItemsRefreshCTS.Cancel();
                    relatedSubAccountItemsRefreshCTS.Dispose();
                    relatedSubAccountItemsRefreshCTS = null;
                }

                if (resetRefreshState)
                {
                    relatedSubAccountItemsRefreshState = DataRefreshState.NotRefresh;
                }
            }
        }

        private DataRefreshState SafeGetRelatedSubAccountItemsRefreshState()
        {
            DataRefreshState state;
            lock (relatedSubAccountItemsRefreshLock)
            {
                state = relatedSubAccountItemsRefreshState;
            }
            return state;
        }

        private void SafeSetRelatedSubAccountItemsRefreshState(DataRefreshState state)
        {
            lock (relatedSubAccountItemsRefreshLock)
            {
                relatedSubAccountItemsRefreshState = state;
            }
        }

        /// <summary>
        /// 刷新相关操作账户列表
        /// </summary>
        /// <param name="isForceRefresh">是否强制刷新。YES 表示强制刷新， NO 表示必要时才刷新</param>
        private void RefreshRelatedSubAccountItems(bool isForceRefresh)
        {
            relatedSubAccountItemsSyncTaskFactory.StartNew(() =>
            {
                var landingInfo = loginDataService.LandingInfo;
                if (landingInfo == null) return;

                bool needRefresh = true;
                if (!isForceRefresh && IsRefreshingOrSuccessRefreshed(SafeGetRelatedSubAccountItemsRefreshState()))
                {
                    needRefresh = false;
                }

                if (needRefresh)
                {
                    var clt = AcquireRelatedSubAccountItemsRefreshCLT();
                    SafeSetRelatedSubAccountItemsRefreshState(DataRefreshState.Refreshing);
                    var resp = subAccountRelatedItemQueryCtrl.QueryUserSubAccountRelatedItems(clt);

                    DataRefreshState newState = DataRefreshState.NotRefresh;
                    if (clt.IsCancellationRequested)
                    {
                        newState = DataRefreshState.CanceledRefresh;
                    }
                    else
                    {
                        newState = (resp != null && resp.SourceException == null) ? DataRefreshState.SuccessRefreshed : DataRefreshState.FailedRefreshed;
                    }
                    SafeSetRelatedSubAccountItemsRefreshState(newState);

                    if (clt.IsCancellationRequested) return;

                    var queriedList = resp.CorrectResult ?? new HostingSubAccountRelatedItem[] { };
                    lock (relatedSubAccountItemsDictionaryLock)
                    {
                        if (loginDataService.LandingInfo?.Token != landingInfo.Token) return;

                        UnsafeRemoveAllRelatedSubAccountItems();
                        UnsafeAddOrUpdateRelatedSubAccountItems(queriedList);
                    }
                    eventAggregator.GetEvent<UserRelatedSubAccountItemsRefreshEvent>().Publish(new UserRelatedSubAccountItemsRefreshEventArgs(landingInfo.Token, queriedList?.ToArray()));
                }
            });
        }        

        private void UnsafeAddOrUpdateRelatedSubAccountItems(IEnumerable<HostingSubAccountRelatedItem> newItems)
        {
            if (newItems?.Any() != true) return;

            foreach (var item in newItems)
            {
                var itemKey = new RelatedSubAccountItemKey(item.SubUserId, item.SubAccountId);
                HostingSubAccountRelatedItem existItem = null;
                if (relatedSubAccountItemsDictionary.TryGetValue(itemKey, out existItem))
                {
                    // 存在，改变内容
                    TBase refExistItem = existItem;
                    ThriftHelper.UnserializeBytesToTBase(ref refExistItem, ThriftHelper.SerializeTBaseToBytes(item));
                }
                else
                {
                    // 不存在，则添加
                    relatedSubAccountItemsDictionary[itemKey] = item;
                }
            }
        }
    }
}
