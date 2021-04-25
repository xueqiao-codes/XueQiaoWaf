﻿using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using XueQiaoFoundation.Interfaces.Applications;
using XueQiaoFoundation.Interfaces.Event.business;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;
using XueQiaoWaf.Trade.Modules.Applications.Services;
using xueqiao.trade.hosting.push.protocol;
using xueqiao.trade.hosting.events;
using System.Threading.Tasks.Schedulers;
using IDLAutoGenerated.Util;
using System.Threading;
using XueQiaoFoundation.Shared.Helper;
using xueqiao.trade.hosting.arbitrage.thriftapi;
using XueQiaoWaf.Trade.Modules.Applications.Helper;
using XueQiaoWaf.Trade.Modules.Applications.ServiceControllers.Events;
using NativeModel.Trade;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoWaf.Trade.Interfaces.Applications;
using XueQiaoWaf.Trade.Interfaces.DataModels;
using XueQiaoWaf.Trade.Interfaces.Events;
using xueqiao.trade.hosting.proxy;
using System.Diagnostics;
using System.Collections.Concurrent;
using lib.xqclient_base.thriftapi_mediation.Interface;
using System.Windows.Threading;
using XueQiaoFoundation.BusinessResources.Helpers;
using XueQiaoFoundation.BusinessResources.Models;
using XueQiaoFoundation.Shared.Model;

namespace XueQiaoWaf.Trade.Modules.Applications.ServiceControllers
{
    /// <summary>
    /// 成交控制器。
    ///  
    /// 1.承担拉取成交列表的职责。
    /// 2.处理成交相关的 push event。
    ///
    /// </summary>
    [Export(typeof(ITradeItemsController)), Export(typeof(ITradeModuleSingletonController)),
        PartCreationPolicy(CreationPolicy.Shared)]
    internal class TradeItemsController : ITradeItemsController, ITradeModuleSingletonController
    {
        private delegate TradeItemUpdateTemplate TradeItemUpdateTemplateFactory(XQTradeItemKey itemKey, TradeItemDataModel existItem);

        /// <summary>
        /// 成交项更新模板
        /// </summary>
        private class TradeItemUpdateTemplate
        {
            public Tuple<ClientTradeDirection> Direction;

            public Tuple<double> TradePrice;

            public Tuple<int> TradeVolume;

            public Tuple<HostingXQTarget> SourceOrderTarget;

            public Tuple<long> CreateTimestampMs;
        }
        
        private readonly TradeItemsService tradeItemsService;
        private readonly IEventAggregator eventAggregator;
        private readonly ILoginDataService loginDataService;
        private readonly Lazy<ILoginUserManageService> loginUserManageService;
        private readonly ISubAccountXQEffectOrderQueryController subAccountXQEffectOrderQueryCtrl;
        private readonly IRelatedSubAccountItemsController relatedSubAccountItemsCtrl;
        private readonly IUserSubAccountRelatedItemQueryController subAccountRelatedItemQueryCtrl;
        private readonly IUserSubAccountRelatedItemCacheController subAccountRelatedItemCacheCtrl;
        private readonly IHostingUserQueryController hostingUserQueryCtrl;
        private readonly IHostingUserCacheController hostingUserCacheCtrl;
        private readonly IComposeGraphCacheController composeGraphCacheCtrl;
        private readonly IComposeGraphQueryController composeGraphQueryCtrl;
        private readonly IUserComposeViewCacheController userComposeViewCacheCtrl;
        private readonly IUserComposeViewQueryController userComposeViewQueryCtrl;
        private readonly IContractItemTreeQueryController contractItemTreeQueryCtrl;

        
        /// <summary>
        /// 成交列表同步任务工厂。该工厂生产的任务是顺序执行，将增删、刷新包装成任务执行，以保证同步的准确性
        /// </summary>
        private readonly TaskFactory tradeItemsSyncTaskFactory = new TaskFactory(new OrderedTaskScheduler());
        /// <summary>
        /// 成交列表
        /// </summary>
        private readonly Dictionary<XQTradeItemKey, TradeItemDataModel> tradeItems = new Dictionary<XQTradeItemKey, TradeItemDataModel>();
        private readonly object tradeItemsLock = new object();


        private CancellationTokenSource tradeItemsRefreshCTS;
        private readonly object tradeItemsRefreshLock = new object();
        private readonly Dictionary<long, SubAccountAnyDataRefreshStateHolder> tradeItemsRefreshStateHolders
            = new Dictionary<long, SubAccountAnyDataRefreshStateHolder>();
        
        private readonly IDIncreaser clientDataForceSyncTimesIncreaser = new IDIncreaser(0);

        [ImportingConstructor]
        public TradeItemsController(
            TradeItemsService tradeItemsService,
            IEventAggregator eventAggregator,
            ILoginDataService loginDataService,
            Lazy<ILoginUserManageService> loginUserManageService,
            ISubAccountXQEffectOrderQueryController subAccountXQEffectOrderQueryCtrl,
            IRelatedSubAccountItemsController relatedSubAccountItemsCtrl,
            IUserSubAccountRelatedItemQueryController subAccountRelatedItemQueryCtrl,
            IUserSubAccountRelatedItemCacheController subAccountRelatedItemCacheCtrl,
            IHostingUserQueryController hostingUserQueryCtrl,
            IHostingUserCacheController hostingUserCacheCtrl,
            IComposeGraphCacheController composeGraphCacheCtrl,
            IComposeGraphQueryController composeGraphQueryCtrl,
            IUserComposeViewCacheController userComposeViewCacheCtrl,
            IUserComposeViewQueryController userComposeViewQueryCtrl,
            IContractItemTreeQueryController contractItemTreeQueryCtrl)
        {
            this.tradeItemsService = tradeItemsService;
            this.eventAggregator = eventAggregator;
            this.loginDataService = loginDataService;
            this.loginUserManageService = loginUserManageService;
            this.subAccountXQEffectOrderQueryCtrl = subAccountXQEffectOrderQueryCtrl;
            this.relatedSubAccountItemsCtrl = relatedSubAccountItemsCtrl;

            this.subAccountRelatedItemQueryCtrl = subAccountRelatedItemQueryCtrl;
            this.subAccountRelatedItemCacheCtrl = subAccountRelatedItemCacheCtrl;
            this.hostingUserQueryCtrl = hostingUserQueryCtrl;
            this.hostingUserCacheCtrl = hostingUserCacheCtrl;
            this.composeGraphCacheCtrl = composeGraphCacheCtrl;
            this.composeGraphQueryCtrl = composeGraphQueryCtrl;
            this.userComposeViewCacheCtrl = userComposeViewCacheCtrl;
            this.userComposeViewQueryCtrl = userComposeViewQueryCtrl;
            this.contractItemTreeQueryCtrl = contractItemTreeQueryCtrl;

            loginUserManageService.Value.HasLogouted += RecvUserHasLogouted;
            eventAggregator.GetEvent<ClientDataForceSync>().Subscribe(ReceivedClientDataForceSyncEvent);
            eventAggregator.GetEvent<XQTradeListChanged>().Subscribe(ReceivedXQTradeListChangedEvent);
            eventAggregator.GetEvent<XQOrderExpired>().Subscribe(ReceivedXQOrderExpiredEvent);
            eventAggregator.GetEvent<UserComposeViewUpdatedEvent>().Subscribe(ReceivedUserComposeViewUpdatedEvent);
            eventAggregator.GetEvent<UserRelatedSubAccountItemsRefreshEvent>().Subscribe(RecvUserRelatedSubAccountItemsRefreshEvent);

            //WeakEventManager<TradeItemsController, ItemListRefreshingStateChangedArgs>.AddHandler(this, "TradeItemsRefreshingChanged", OnTradeItemsRefreshingChanged);

        }

        //private void OnTradeItemsRefreshingChanged(object sender, ItemListRefreshingStateChangedArgs e)
        //{

        //}

        public void Shutdown()
        {
            clientDataForceSyncTimesIncreaser?.Reset();
            CancelSubAccountTradeItemsRefresh(true);

            loginUserManageService.Value.HasLogouted -= RecvUserHasLogouted;
            eventAggregator.GetEvent<ClientDataForceSync>().Unsubscribe(ReceivedClientDataForceSyncEvent);
            eventAggregator.GetEvent<XQTradeListChanged>().Unsubscribe(ReceivedXQTradeListChangedEvent);
            eventAggregator.GetEvent<XQOrderExpired>().Unsubscribe(ReceivedXQOrderExpiredEvent);
            eventAggregator.GetEvent<UserComposeViewUpdatedEvent>().Unsubscribe(ReceivedUserComposeViewUpdatedEvent);
            eventAggregator.GetEvent<UserRelatedSubAccountItemsRefreshEvent>().Unsubscribe(RecvUserRelatedSubAccountItemsRefreshEvent);
        }

        public event SubAccountAnyDataRefreshStateChanged TradeItemsRefreshStateChanged;

        public void RefreshTradeItemsIfNeed(long subAccountId)
        {
            RefreshSubAccountTradeItems(new long[] { subAccountId }, false);
        }
        
        public void RefreshTradeItemsForce(long subAccountId)
        {
            RefreshSubAccountTradeItems(new long[] { subAccountId }, true);
        }

        private void RecvUserHasLogouted(ProxyLoginResp lastLoginResp)
        {
            clientDataForceSyncTimesIncreaser?.Reset();
            CancelSubAccountTradeItemsRefresh(true);
            RemoveSubAccountTradeItems(null);
        }

        private void ReceivedClientDataForceSyncEvent(ClientForceSyncEvent obj)
        {
            if (loginDataService.ProxyLoginResp == null) return;
            var forceSyncTimes = clientDataForceSyncTimesIncreaser.RequestIncreasedId();
            
            // 同步成交列表
            var userRelatedSubAccountIds = relatedSubAccountItemsCtrl.RelatedSubAccountItems?.Select(i => i.SubAccountId).Distinct();
            if (userRelatedSubAccountIds?.Any() != true) return;
            
            CancelSubAccountTradeItemsRefresh(false);
            RefreshSubAccountTradeItems(userRelatedSubAccountIds, forceSyncTimes != 1);
        }

        private void ReceivedXQTradeListChangedEvent(XQTradeListChangedEvent obj)
        {
            if (loginDataService.ProxyLoginResp == null) return;
            if (obj?.TradeList == null) return;

            lock (tradeItemsLock)
            {
                if (loginDataService.ProxyLoginResp == null) return;

                UnsafeAddOrUpdateTradeItems(obj.TradeList,
                    out IEnumerable<TradeItemDataModel> addedItems, out IEnumerable<TradeItemDataModel> updatedItems);
                if (addedItems?.Any() == true)
                {
                    foreach (var t in addedItems)
                    {
                        if (t.SourceType != ClientTradeItemSourceType.ComposeTargetLame)
                        {
                            eventAggregator.GetEvent<OrderTradedEventDrivedFromPush>().Publish(new OrderTradedEventPayload(t));
                        }
                    }
                }
            }
        }
        
        private void ReceivedXQOrderExpiredEvent(XQOrderExpiredEvent msg)
        {
            if (msg == null) return;
            var orderId = msg.OrderId;
            tradeItemsSyncTaskFactory.StartNew(() =>
            {
                lock (tradeItemsLock)
                {
                    if (loginDataService.ProxyLoginResp == null) return;
                    var rmKeys = tradeItems.Where(i => i.Value.OrderId == orderId).Select(i=>i.Key).ToArray();
                    UnsafeRemoveTradeItems(rmKeys);
                }
            });
        }

        private void ReceivedUserComposeViewUpdatedEvent(NativeComposeView composeView)
        {
            if (composeView == null) return;
            var composeId = composeView.ComposeGraphId;

            IEnumerable<TradeItemDataModel> tarTradeItems = null;
            lock (tradeItemsLock)
            {
                var targetKey = $"{composeId}";
                tarTradeItems = tradeItems.Values
                    .Where(i => i.TargetType == ClientXQOrderTargetType.COMPOSE_TARGET && i.TargetKey == targetKey)
                    .ToArray();
            }

            if (tarTradeItems == null) return;
            foreach (var item in tarTradeItems)
            {
                var composeViewContainer = item.TargetComposeUserComposeViewContainer;
                if (composeViewContainer != null)
                {
                    composeViewContainer.UserComposeView = composeView;

                    // Rectify related price properties
                    TradeItemDataModelCreateHelper.RectifyTradeItemRelatedPriceProps(item);
                    
                    // Update target name
                    XqTargetDMHelper.InvalidateTargetNameWithAppropriate(item, XqAppLanguages.CN);
                }
            }
        }

        private void RecvUserRelatedSubAccountItemsRefreshEvent(UserRelatedSubAccountItemsRefreshEventArgs args)
        {
            var currentLoginToken = loginDataService?.ProxyLoginResp?.HostingSession?.Token;
            if (currentLoginToken == null || currentLoginToken != args.LoginUserToken) return;

            var relatedSubAccountIds = args.RelatedSubAccountItems?.Select(i => i.SubAccountId).Distinct();
            if (relatedSubAccountIds?.Any() != true) return;

            foreach (var subAccountId in relatedSubAccountIds)
            {
                RefreshTradeItemsIfNeed(subAccountId);
            }
        }


        private void UnsafeRemoveTradeItems(IEnumerable<XQTradeItemKey> itemKeys)
        {
            if (itemKeys?.Any() != true) return;

            var rmItems = new List<TradeItemDataModel>();
            foreach (var _rmKey in itemKeys)
            {
                if (tradeItems.TryGetValue(_rmKey, out TradeItemDataModel rmItem))
                {
                    rmItems.Add(rmItem);
                    tradeItems.Remove(_rmKey);
                }
            }

            if (rmItems.Any())
            {
                UIItemsAddOrRemoveDispatcherBeginInvoke(() =>
                {
                    foreach (var rmItem in rmItems)
                    {
                        tradeItemsService.TradeItems.Remove(rmItem);
                    }
                });
            }
        }

        /// <summary>
        /// 删除某个操作账户的所有成交项。如果 subAccountId 为 null，则删除所有
        /// </summary>
        /// <param name="subAccountId"></param>
        private void RemoveSubAccountTradeItems(long? subAccountId)
        {
            lock (tradeItemsLock)
            {
                UnsafeRemoveSubAccountTradeItems(subAccountId);
            }
        }

        /// <summary>
        /// 非线程安全地删除某个操作账户的所有成交项。如果 subAccountId 为 null，则删除所有
        /// </summary>
        /// <param name="subAccountId"></param>
        private void UnsafeRemoveSubAccountTradeItems(long? subAccountId)
        {
            KeyValuePair<XQTradeItemKey, TradeItemDataModel>[] rmItems = null;
            if (subAccountId == null)
                rmItems = tradeItems.ToArray();
            else
                rmItems = tradeItems.Where(i => i.Key.SubAccountId == subAccountId).ToArray();

            foreach (var rmItem in rmItems)
            {
                tradeItems.Remove(rmItem.Key);
            }
            UIItemsAddOrRemoveDispatcherBeginInvoke(() =>
            {
                foreach (var rmItem in rmItems)
                {
                    tradeItemsService.TradeItems.Remove(rmItem.Value);
                }
            });
        }

        private static bool IsRefreshingOrSuccessRefreshed(DataRefreshState subAccountLastRefreshState)
        {
            return subAccountLastRefreshState == DataRefreshState.Refreshing || subAccountLastRefreshState == DataRefreshState.SuccessRefreshed;
        }

        private SubAccountAnyDataRefreshStateHolder AcquireSubAccountTradeItemsRefreshStateHolder(long subAccountId)
        {
            lock (tradeItemsRefreshLock)
            {
                SubAccountAnyDataRefreshStateHolder dataHolder = null;
                if (!tradeItemsRefreshStateHolders.TryGetValue(subAccountId, out dataHolder))
                {
                    dataHolder = new SubAccountAnyDataRefreshStateHolder(subAccountId);
                    tradeItemsRefreshStateHolders.Add(subAccountId, dataHolder);
                }
                return dataHolder;
            }
        }

        private CancellationToken AcquireSubAccountTradeItemsRefreshCLT()
        {
            CancellationToken clt = CancellationToken.None;
            lock (tradeItemsRefreshLock)
            {
                if (tradeItemsRefreshCTS == null)
                {
                    tradeItemsRefreshCTS = new CancellationTokenSource();
                }
                clt = tradeItemsRefreshCTS.Token;
            }
            return clt;
        }

        private void CancelSubAccountTradeItemsRefresh(bool clearRefreshStateHolders)
        {
            lock (tradeItemsRefreshLock)
            {
                if (tradeItemsRefreshCTS != null)
                {
                    tradeItemsRefreshCTS.Cancel();
                    tradeItemsRefreshCTS.Dispose();
                    tradeItemsRefreshCTS = null;
                }

                if (clearRefreshStateHolders)
                {
                    tradeItemsRefreshStateHolders.Clear();
                }
            }
        }

        /// <summary>
        /// 刷新操作账号的成交列表
        /// </summary>
        /// <param name="subAccountIds">需要刷新的操作账号 id 列表</param>
        /// <param name="isForceRefresh">是否强制刷新。YES 表示强制刷新， NO 表示必要时才刷新</param>
        private void RefreshSubAccountTradeItems(IEnumerable<long> subAccountIds, bool isForceRefresh)
        {
            if (subAccountIds?.Any() != true) return;
            
            tradeItemsSyncTaskFactory.StartNew(() =>
            {
                var landingInfo = loginDataService.LandingInfo;
                if (landingInfo == null) return;

                var queryTsks = new List<Task>();
                var queryResps = new ConcurrentDictionary<long, IInterfaceInteractResponse<IEnumerable<HostingXQOrderWithTradeList>>>();
                var refreshStateHolders = new List<SubAccountAnyDataRefreshStateHolder>();

                var clt = AcquireSubAccountTradeItemsRefreshCLT();
                // 查询所有 subaccount 的数据
                foreach (var subAccountId in subAccountIds)
                {
                    var stateHolder = AcquireSubAccountTradeItemsRefreshStateHolder(subAccountId);

                    bool needRefresh = true;
                    if (!isForceRefresh && IsRefreshingOrSuccessRefreshed(stateHolder.DataRefreshState))
                    {
                        needRefresh = false;
                    }

                    if (needRefresh && !clt.IsCancellationRequested)
                    {
                        refreshStateHolders.Add(stateHolder);
                        stateHolder.DataRefreshState = DataRefreshState.Refreshing;
                        TradeItemsRefreshStateChanged?.Invoke(new SubAccountAnyDataRefreshStateChangedArgs(landingInfo.SubUserId, stateHolder.SubAccountId, stateHolder.DataRefreshState));
                        var tsk = Task.Run(() =>
                        {
                            clt.ThrowIfCancellationRequested();

                            var resp = subAccountXQEffectOrderQueryCtrl.SubAccountQueryXQOrdersSync(stateHolder.SubAccountId, clt);
                            queryResps.TryAdd(subAccountId, resp);

                            clt.ThrowIfCancellationRequested();
                        }, clt);
                        queryTsks.Add(tsk);
                    }
                }

                if (queryTsks.Count == 0) return;
                try
                {
                    Task.WaitAll(queryTsks.ToArray());
                }
                catch (AggregateException)
                {
                    if (loginDataService.ProxyLoginResp == null) return;

                    foreach (var stateHolder in refreshStateHolders)
                    {
                        // 取消或失败。修改刷新状态
                        stateHolder.DataRefreshState = clt.IsCancellationRequested ? DataRefreshState.CanceledRefresh : DataRefreshState.FailedRefreshed;
                        TradeItemsRefreshStateChanged?.Invoke(new SubAccountAnyDataRefreshStateChangedArgs(landingInfo.SubUserId, stateHolder.SubAccountId, stateHolder.DataRefreshState));
                    }
                    return;
                }
                
                lock (tradeItemsLock)
                {
                    if (loginDataService.ProxyLoginResp == null) return;

                    foreach (var stateHolder in refreshStateHolders)
                    {
                        IInterfaceInteractResponse<IEnumerable<HostingXQOrderWithTradeList>> resp = null;
                        queryResps.TryGetValue(stateHolder.SubAccountId, out resp);
                        stateHolder.DataRefreshState = (resp?.CorrectResult != null) ? DataRefreshState.SuccessRefreshed : DataRefreshState.FailedRefreshed;
                        TradeItemsRefreshStateChanged?.Invoke(new SubAccountAnyDataRefreshStateChangedArgs(landingInfo.SubUserId, stateHolder.SubAccountId, stateHolder.DataRefreshState));

                        if (clt.IsCancellationRequested) continue;

                        if (resp != null)
                        {
                            var qTradeItems = resp.CorrectResult?.JoinAllTradeItems() ?? new HostingXQTrade[] { };

                            // 这里不删除 sub account 的原来的成交记录。因为 push 一直在进行，所以只添加或修改才不会将数据丢失。而在退出登录时，才进行清理

                            // 添加或更新查询到的成交项
                            UnsafeAddOrUpdateTradeItems(qTradeItems,
                                    out IEnumerable<TradeItemDataModel> _addList,
                                    out IEnumerable<TradeItemDataModel> _updateList);
                        }
                    }
                }
            });
        }
        
        private void UnsafeAddOrUpdateTradeItems(
            IEnumerable<HostingXQTrade> hostingTradeItems,
            out IEnumerable<TradeItemDataModel> _addedTradeItems,
            out IEnumerable<TradeItemDataModel> _updatedTradeItems)
        {
            _addedTradeItems = null;
            _updatedTradeItems = null;

            if (hostingTradeItems == null) return;

            var qTradeItemMap = hostingTradeItems.ToDictionary(i => new XQTradeItemKey(i.SubAccountId, i.TradeId));
            UnsafeAddOrUpdateTradeItems(qTradeItemMap.Keys,
                _key =>
                {
                    var tradeData = qTradeItemMap[_key];
                    var newItem = TradeItemDataModelCreateHelper.CreateTradeItem(tradeData,
                        subAccountRelatedItemQueryCtrl, subAccountRelatedItemCacheCtrl,
                        hostingUserQueryCtrl, hostingUserCacheCtrl,
                        composeGraphCacheCtrl, composeGraphQueryCtrl,
                        userComposeViewCacheCtrl, userComposeViewQueryCtrl,
                        contractItemTreeQueryCtrl);
                    return newItem;
                },
                (_key, _existItem) =>
                {
                    // 新添加的不需要通过更新模板进行更新
                    if (_existItem == null) return null;

                    var tradeData = qTradeItemMap[_key];

                    var temp = new TradeItemUpdateTemplate();
                    if (tradeData.__isset.tradeDiretion)
                        temp.Direction = new Tuple<ClientTradeDirection>(tradeData.TradeDiretion.ToClientTradeDirection());
                    if (tradeData.__isset.tradePrice)
                        temp.TradePrice = new Tuple<double>(tradeData.TradePrice);
                    if (tradeData.__isset.tradeVolume)
                        temp.TradeVolume = new Tuple<int>(tradeData.TradeVolume);
                    if (tradeData.__isset.sourceOrderTarget)
                        temp.SourceOrderTarget = new Tuple<HostingXQTarget>(tradeData.SourceOrderTarget);
                    if (tradeData.__isset.createTimestampMs)
                        temp.CreateTimestampMs = new Tuple<long>(tradeData.CreateTimestampMs);
                    return temp;
                },
                out _addedTradeItems,
                out _updatedTradeItems);
        }
        
        /// <summary>
        /// 添加或更新成交项
        /// </summary>
        /// <param name="newItemKeys">新的成交项 key 列表</param>
        /// <param name="newTradeItemFactory">新的成交项获取方法</param>
        /// <param name="updateTemplateFactory">更新模板获取方法</param>
        /// <param name="addedTradeItems">添加的成交项</param>
        /// <param name="updatedTradeItems">更新的成交项</param>
        private void UnsafeAddOrUpdateTradeItems(
            IEnumerable<XQTradeItemKey> newItemKeys,
            Func<XQTradeItemKey, TradeItemDataModel> newItemFactory,
            TradeItemUpdateTemplateFactory updateTemplateFactory,
            out IEnumerable<TradeItemDataModel> _addedTradeItems,
            out IEnumerable<TradeItemDataModel> _updatedTradeItems)
        {
            _addedTradeItems = null;
            _updatedTradeItems = null;

            Debug.Assert(newItemFactory != null);

            if (newItemKeys?.Any() != true) return;

            var addedTradeList = new List<TradeItemDataModel>();
            var updatedTradeList = new List<TradeItemDataModel>();
            foreach (var itemKey in newItemKeys)
            {
                TradeItemDataModel implTradeItem = null;
                TradeItemDataModel existTradeItem = null;
                tradeItems.TryGetValue(itemKey, out existTradeItem);
                if (existTradeItem == null)
                {
                    var newTradeItem = newItemFactory.Invoke(itemKey);
                    if (newTradeItem != null)
                    {
                        tradeItems[itemKey] = newTradeItem;
                        addedTradeList.Add(newTradeItem);
                        implTradeItem = newTradeItem;
                    }
                }
                else
                {
                    updatedTradeList.Add(existTradeItem);
                    implTradeItem = existTradeItem;
                }

                if (implTradeItem != null && updateTemplateFactory != null)
                {
                    var updateTemp = updateTemplateFactory.Invoke(itemKey, existTradeItem);
                    UpdateTradeItemWithTemplate(implTradeItem, updateTemp);
                }
            }

            // 添加到成交列表
            if (addedTradeList.Any())
            {
                UIItemsAddOrRemoveDispatcherBeginInvoke(() =>
                {
                    tradeItemsService.TradeItems.AddRange(addedTradeList);
                });
            }

            _addedTradeItems = addedTradeList;
            _updatedTradeItems = updatedTradeList;
        }

        /// <summary>
        /// 为了防止死锁，用于 UI 显示的列表的增加或删除需要在主线程执行。该方法能保证顺序执行
        /// </summary>
        /// <param name="action"></param>
        private static void UIItemsAddOrRemoveDispatcherBeginInvoke(Action action)
        {
            if (action == null) return;
            DispatcherHelper.CheckBeginInvokeOnUI(() => action(),
                DispatcherPriority.Normal);
        }

        private void UpdateTradeItemWithTemplate(TradeItemDataModel tradeItem, TradeItemUpdateTemplate updateTemplate)
        {
            if (tradeItem == null || updateTemplate == null) return;

            bool rectifyReletedPriceProps = false;

            if (updateTemplate.Direction != null) tradeItem.Direction = updateTemplate.Direction.Item1;
            
            if (updateTemplate.TradePrice != null)
            {
                rectifyReletedPriceProps = true;
                tradeItem.TradePrice = updateTemplate.TradePrice.Item1;
            }

            if (updateTemplate.TradeVolume != null) tradeItem.TradeVolume = updateTemplate.TradeVolume.Item1;
            if (updateTemplate.SourceOrderTarget != null) tradeItem.SourceOrderTarget = updateTemplate.SourceOrderTarget.Item1;
            if (updateTemplate.CreateTimestampMs != null) tradeItem.CreateTimestampMs = updateTemplate.CreateTimestampMs.Item1;

            if (rectifyReletedPriceProps)
            {
                TradeItemDataModelCreateHelper.RectifyTradeItemRelatedPriceProps(tradeItem);
            }
        }
    }
}
