using business_foundation_lib.xq_thriftlib_config;
using lib.xqclient_base.logger;
using lib.xqclient_base.thriftapi_mediation;
using lib.xqclient_base.thriftapi_mediation.Interface;
using NativeModel.Trade;
using Prism.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Schedulers;
using System.Windows.Threading;
using xueqiao.trade.hosting.events;
using xueqiao.trade.hosting.position.statis;
using xueqiao.trade.hosting.proxy;
using xueqiao.trade.hosting.push.protocol;
using xueqiao.trade.hosting.terminal.ao;
using XueQiaoFoundation.BusinessResources.Constants;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.BusinessResources.Helpers;
using XueQiaoFoundation.BusinessResources.Models;
using XueQiaoFoundation.Interfaces.Applications;
using XueQiaoFoundation.Interfaces.Event.business;
using XueQiaoFoundation.Interfaces.Helper;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Model;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;
using XueQiaoWaf.Trade.Interfaces.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Helper;
using XueQiaoWaf.Trade.Modules.Applications.ServiceControllers.Events;
using XueQiaoWaf.Trade.Modules.Applications.Services;

namespace XueQiaoWaf.Trade.Modules.Applications.ServiceControllers
{
    /// <summary>
    /// 标的持仓管理 controller
    /// 
    /// 1.承担拉取标的持仓列表的职责。
    /// 2.处理标的持仓相关的 push event。
    /// </summary>
    [Export(typeof(IXqTargetPositionItemsController)), Export(typeof(ITradeModuleSingletonController)),
        PartCreationPolicy(CreationPolicy.Shared)]
    internal class XqTargetPositionItemsController : IXqTargetPositionItemsController, ITradeModuleSingletonController
    {
        private delegate TargetPositionUpdateTemplate TargetPositionUpdateTemplateFactory(TargetPositionKey itemKey, TargetPositionDataModel existItem);

        // 持仓更新模板
        private class TargetPositionUpdateTemplate
        {
            // 今日长持
            public Tuple<long?> LongPosition;

            // 今日短持
            public Tuple<long?> ShortPosition;

            // 净仓
            public Tuple<long?> NetPosition;

            // 持仓均价
            public Tuple<double?> PositionAvgPrice;

            // 最后修改时间
            public long? LastModifyTimestampMs;

            // 动态新修改时间
            public long? DynamicInfoModifyTimestampMs;

            // 动态信息
            public Tuple<TargetPositionDynamicInfo> DynamicInfo;

            // 标的是否过期
            public Tuple<bool?> IsXqTargetExpired;
        }

        private readonly TargetPositionService targetPositionService;
        private readonly IEventAggregator eventAggregator;
        private readonly ILoginDataService loginDataService;
        private readonly Lazy<ILoginUserManageService> loginUserManageService;
        
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
        /// 持仓同步任务工厂。该工厂生产的任务是顺序执行，将增删、刷新包装成任务执行，才可保证同步的准确性
        /// </summary>
        private readonly TaskFactory positionSyncTaskFactory = new TaskFactory(new OrderedTaskScheduler());
        /// <summary>
        /// 持仓列表
        /// </summary>
        private readonly Dictionary<TargetPositionKey, TargetPositionDataModel> positionItems = new Dictionary<TargetPositionKey, TargetPositionDataModel>();
        private readonly object positionItemsLock = new object();

        
        private CancellationTokenSource positionItemsRefreshCTS;
        private readonly object positionItemsRefreshLock = new object();
        private readonly Dictionary<long, SubAccountAnyDataRefreshStateHolder> positionItemsRefreshStateHolders
            = new Dictionary<long, SubAccountAnyDataRefreshStateHolder>();
        
        private readonly IDIncreaser clientDataForceSyncTimesIncreaser = new IDIncreaser(0);
        
        // 检查标的是否过期的定时器
        private System.Timers.Timer checkTargetExpiredTimer;

        [ImportingConstructor]
        public XqTargetPositionItemsController(
            TargetPositionService targetPositionService,
            IEventAggregator eventAggregator,
            ILoginDataService loginDataService,
            Lazy<ILoginUserManageService> loginUserManageService,
            
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
            this.targetPositionService = targetPositionService;
            this.eventAggregator = eventAggregator;
            this.loginDataService = loginDataService;
            this.loginUserManageService = loginUserManageService;
            
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

            loginUserManageService.Value.HasLogined += RecvUserLogined;
            loginUserManageService.Value.HasLogouted += RecvUserHasLogouted;
            eventAggregator.GetEvent<ClientDataForceSync>().Subscribe(ReceivedClientDataForceSyncEvent);
            eventAggregator.GetEvent<UserRelatedSubAccountItemsRefreshEvent>().Subscribe(RecvUserRelatedSubAccountItemsRefreshEvent);
            eventAggregator.GetEvent<XQTargetPositionSummaryChanged>().Subscribe(RecvXQTargetPositionSummaryChangedEvent);
            eventAggregator.GetEvent<XQTargetPositionDynamicInfoEvent>().Subscribe(RecvXQTargetPositionDynamicInfoEvent);
            eventAggregator.GetEvent<UserComposeViewUpdatedEvent>().Subscribe(ReceivedUserComposeViewUpdatedEvent);
        }

        public void Shutdown()
        {
            StopCheckTargetExpiredTimer();
            clientDataForceSyncTimesIncreaser?.Reset();
            CancelSubAccountPositionItemsRefresh(true);

            loginUserManageService.Value.HasLogined -= RecvUserLogined;
            loginUserManageService.Value.HasLogouted -= RecvUserHasLogouted;
            eventAggregator.GetEvent<ClientDataForceSync>().Unsubscribe(ReceivedClientDataForceSyncEvent);
            eventAggregator.GetEvent<UserRelatedSubAccountItemsRefreshEvent>().Unsubscribe(RecvUserRelatedSubAccountItemsRefreshEvent);
            eventAggregator.GetEvent<XQTargetPositionSummaryChanged>().Unsubscribe(RecvXQTargetPositionSummaryChangedEvent);
            eventAggregator.GetEvent<XQTargetPositionDynamicInfoEvent>().Unsubscribe(RecvXQTargetPositionDynamicInfoEvent);
            eventAggregator.GetEvent<UserComposeViewUpdatedEvent>().Unsubscribe(ReceivedUserComposeViewUpdatedEvent);
        }
        
        public event SubAccountAnyDataRefreshStateChanged PositionItemsRefreshStateChanged;
        
        public void RefreshPositionItemsIfNeed(long subAccountId)
        {
            RefreshSubAccountPositionItems(new long[] { subAccountId }, false);
        }

        public void RefreshPositionItemsForce(long subAccountId)
        {
            RefreshSubAccountPositionItems(new long[] { subAccountId }, true);
        }

        public TargetPositionDataModel GetPositionItem(TargetPositionKey itemKey)
        {
            TargetPositionDataModel tarItem = null;
            lock (positionItemsLock)
            {
                positionItems.TryGetValue(itemKey, out tarItem);
            }
            return tarItem;
        }

        public IEnumerable<long> ExistPositionSubAccountIdsOfTarget(string targetKey, ClientXQOrderTargetType targetType)
        {
            IEnumerable<long> results = null;
            lock (positionItemsLock)
            {
                var tarItems = positionItems.Values.Where(i => targetKey == i.TargetKey && targetType == i.TargetType);
                results = tarItems.GroupBy(i => i.SubAccountFields.SubAccountId).Select(i => i.Key).ToArray();
            }
            return results;
        }
        
        public void RequestDeleteExpiredXqTargetPosition(TargetPositionKey itemKey)
        {
            if (itemKey == null) return;
            var landingInfo = loginDataService.LandingInfo;
            if (landingInfo == null) return;

            if (itemKey.TargetType != ClientXQOrderTargetType.CONTRACT_TARGET)
            {
                AppLog.Warn($"Not support delete expried position with target type({itemKey.TargetType.ToString()})");
                return;
            }

            var contractId = Convert.ToInt64(itemKey.TargetKey);
            var siip = new StubInterfaceInteractParams { TransportConnectTimeoutMS = 2000, TransportReadTimeoutMS = 2000 };
            XqThriftLibConfigurationManager.SharedInstance.TradeHostingTerminalAoHttpStub.deleteExpiredStatContractPositionAsync(landingInfo, itemKey.SubAccountId, contractId, CancellationToken.None, siip)
                .ContinueWith(t =>
                {
                    var resp = t.Result;
                    var currentLandingInfo = loginDataService.LandingInfo;
                    if (resp != null && resp.SourceException == null && currentLandingInfo?.Token == landingInfo.Token)
                    {
                        positionSyncTaskFactory.StartNew(() => 
                        {
                            lock (positionItemsLock)
                            {
                                if (loginDataService.ProxyLoginResp == null) return;
                                UnsafeRemovePositionItems(new TargetPositionKey[] { itemKey });
                            }
                        });
                    }
                    else
                    {
                        AppLog.Error($"Failed to deleteExpiredStatContractPosition.", resp?.SourceException);
                    }
                });
        }


        private void RecvUserLogined()
        {
            StartCheckTargetExpiredTimer();
        }

        private void RecvUserHasLogouted(ProxyLoginResp lastLoginResp)
        {
            StopCheckTargetExpiredTimer();
            clientDataForceSyncTimesIncreaser?.Reset();
            CancelSubAccountPositionItemsRefresh(true);
            RemoveSubAccountPositionItems(null);
        }
        
        private void ReceivedClientDataForceSyncEvent(ClientForceSyncEvent msg)
        {
            if (loginDataService.ProxyLoginResp == null) return;
            var forceSyncTimes = clientDataForceSyncTimesIncreaser.RequestIncreasedId();
            
            // 同步持仓列表
            var userRelatedSubAccountIds = relatedSubAccountItemsCtrl.RelatedSubAccountItems?.Select(i => i.SubAccountId).Distinct();
            if (userRelatedSubAccountIds?.Any() != true) return;

            CancelSubAccountPositionItemsRefresh(false);
            RefreshSubAccountPositionItems(userRelatedSubAccountIds, forceSyncTimes != 1);
        }
        
        private void RecvUserRelatedSubAccountItemsRefreshEvent(UserRelatedSubAccountItemsRefreshEventArgs args)
        {
            var currentLoginToken = loginDataService?.ProxyLoginResp?.HostingSession?.Token;
            if (currentLoginToken == null || currentLoginToken != args.LoginUserToken) return;

            var relatedSubAccountIds = args.RelatedSubAccountItems?.Select(i => i.SubAccountId).Distinct();
            if (relatedSubAccountIds?.Any() != true) return;

            RefreshSubAccountPositionItems(relatedSubAccountIds, false);
        }
        
        private void RecvXQTargetPositionSummaryChangedEvent(StatPositionSummaryChangedEvent msg)
        {
            if (msg == null) return;
            var positionSummary = msg?.StatPositionSummary;
            if (positionSummary == null) return;
            if (loginDataService.ProxyLoginResp == null) return;

            if (msg.EventType == StatPositionEventType.STAT_ITEM_REMOVED)
            {
                positionSyncTaskFactory.StartNew(() =>
                {
                    lock (positionItemsLock)
                    {
                        if (loginDataService.ProxyLoginResp == null) return;
                        var rmKey = new TargetPositionKey(positionSummary.TargetType.ToClientXQOrderTargetType(),
                            positionSummary.SubAccountId, positionSummary.TargetKey);
                        if (positionItems.TryGetValue(rmKey, out TargetPositionDataModel rmItem))
                        {
                            positionItems.Remove(rmKey);
                            UIItemsAddOrRemoveDispatcherBeginInvoke(() =>
                            {
                                targetPositionService.PositionItems.Remove(rmItem);
                            });
                        }
                    }
                });
                return;
            }

            if (msg.EventType == StatPositionEventType.STAT_ITEM_CREATED || msg.EventType == StatPositionEventType.STAT_ITEM_UPDATED)
            {
                if (!positionSummary.__isset.lastmodifyTimestampMs)
                    return;

                lock (positionItemsLock)
                {
                    if (loginDataService.ProxyLoginResp == null) return;

                    var itemKey = new TargetPositionKey(positionSummary.TargetType.ToClientXQOrderTargetType(), positionSummary.SubAccountId, positionSummary.TargetKey);
                    UnsafeAddOrUpdatePositionItems(new TargetPositionKey[] { itemKey },
                        _key => GenerateTargetPositionItem(_key),
                        (_key, _existItem) =>
                        {
                            if (_existItem != null && _existItem.LastModifyTimestampMs > positionSummary.LastmodifyTimestampMs)
                                return null;

                            var template = new TargetPositionUpdateTemplate
                            {
                                LastModifyTimestampMs = positionSummary.LastmodifyTimestampMs
                            };

                            template.LongPosition = new Tuple<long?>(positionSummary.__isset.longPosition ? (long?)positionSummary.LongPosition : null);
                            template.ShortPosition = new Tuple<long?>(positionSummary.__isset.shortPosition ? (long?)positionSummary.ShortPosition : null);
                            template.NetPosition = new Tuple<long?>(positionSummary.__isset.netPosition ? (long?)positionSummary.NetPosition : null);
                            template.PositionAvgPrice = new Tuple<double?>(positionSummary.__isset.positionAvgPrice ? (double?)positionSummary.PositionAvgPrice : null);

                            return template;
                        },
                        out IEnumerable<TargetPositionDataModel> addedItems,
                        out IEnumerable<TargetPositionDataModel> updatedItems);
                }
            }
        }

        private void RecvXQTargetPositionDynamicInfoEvent(StatPositionDynamicInfoEvent payload)
        {
            if (payload == null) return;
            var srcDnamicInfo = payload.PositionDynamicInfo;
            if (srcDnamicInfo == null) return;

            if (!payload.__isset.eventCreateTimestampMs)
                return;

            lock (positionItemsLock)
            {
                if (loginDataService.ProxyLoginResp == null) return;

                var itemKey = new TargetPositionKey(payload.TargetType.ToClientXQOrderTargetType(), payload.SubAccountId, payload.TargetKey);
                UnsafeUpdatePositionItem(itemKey, (_key, _existItem) =>
                {
                    if (_existItem == null) return null;
                    if (_existItem.DynamicInfoModifyTimestampMs > payload.EventCreateTimestampMs) return null;

                    var template = new TargetPositionUpdateTemplate
                    {
                        DynamicInfoModifyTimestampMs = payload.EventCreateTimestampMs
                    };

                    var destDynamicInfo = new TargetPositionDynamicInfo();
                    if (srcDnamicInfo.__isset.lastPrice)
                        destDynamicInfo.LastPrice = srcDnamicInfo.LastPrice;
                    if (srcDnamicInfo.__isset.positionProfit)
                        destDynamicInfo.PositionProfit = srcDnamicInfo.PositionProfit;
                    if (srcDnamicInfo.__isset.closedProfit)
                        destDynamicInfo.ClosedProfit = srcDnamicInfo.ClosedProfit;
                    if (srcDnamicInfo.__isset.totalProfit)
                        destDynamicInfo.TotalProfit = srcDnamicInfo.TotalProfit;
                    if (srcDnamicInfo.__isset.positionValue)
                        destDynamicInfo.PositionValue = srcDnamicInfo.PositionValue;
                    if (srcDnamicInfo.__isset.leverage)
                        destDynamicInfo.Leverage = srcDnamicInfo.Leverage;
                    if (srcDnamicInfo.__isset.currency)
                        destDynamicInfo.Currency = srcDnamicInfo.Currency;

                    template.DynamicInfo = new Tuple<TargetPositionDynamicInfo>(destDynamicInfo);

                    return template;
                });
            }
        }
        
        private void ReceivedUserComposeViewUpdatedEvent(NativeComposeView composeView)
        {
            if (composeView == null) return;
            var composeId = composeView.ComposeGraphId;

            IEnumerable<TargetPositionDataModel> tarPositionItems = null;
            lock (positionItemsLock)
            {
                var targetKey = $"{composeId}";
                tarPositionItems = positionItems.Values
                    .Where(i => i.TargetType == ClientXQOrderTargetType.COMPOSE_TARGET && i.TargetKey == targetKey)
                    .ToArray();
            }

            if (tarPositionItems == null) return;
            foreach (var item in tarPositionItems)
            {
                var composeViewContainer = item.TargetComposeUserComposeViewContainer;
                if (composeViewContainer != null)
                {
                    composeViewContainer.UserComposeView = composeView;

                    // Rectify related price properties
                    RectifyPositionRelatedPriceProps(item);

                    // Update target name
                    XqTargetDMHelper.InvalidateTargetNameWithAppropriate(item, XqAppLanguages.CN);
                }
            }
        }
        

        /// <summary>
        /// 刷新操作账号的持仓列表
        /// </summary>
        /// <param name="subAccountIds">需要刷新的操作账号 id 列表</param>
        /// <param name="isForceRefresh">是否强制刷新。YES 表示强制刷新， NO 表示必要时才刷新</param>
        private void RefreshSubAccountPositionItems(IEnumerable<long> subAccountIds, bool isForceRefresh)
        {
            if (subAccountIds?.Any() != true) return;

            positionSyncTaskFactory.StartNew(() =>
            {
                var landingInfo = loginDataService.LandingInfo;
                if (landingInfo == null) return;
                
                var queryTsks = new List<Task>();
                var queryResps = new ConcurrentDictionary<long, IInterfaceInteractResponse<IEnumerable<StatPositionSummaryEx>>>();
                var refreshStateHolders = new List<SubAccountAnyDataRefreshStateHolder>();

                var clt = AcquireSubAccountPositionItemsRefreshCLT();
                // 查询所有 subaccount 的数据
                foreach (var subAccountId in subAccountIds)
                {
                    var stateHolder = AcquireSubAccountPositionItemsRefreshStateHolder(subAccountId);

                    bool needRefresh = true;
                    if (!isForceRefresh && IsRefreshingOrSuccessRefreshed(stateHolder.DataRefreshState))
                    {
                        needRefresh = false;
                    }

                    if (needRefresh && !clt.IsCancellationRequested)
                    {
                        refreshStateHolders.Add(stateHolder);
                        stateHolder.DataRefreshState = DataRefreshState.Refreshing;
                        PositionItemsRefreshStateChanged?.Invoke(new SubAccountAnyDataRefreshStateChangedArgs(landingInfo.SubUserId, stateHolder.SubAccountId, stateHolder.DataRefreshState));
                        var tsk = Task.Run(() =>
                        {
                            clt.ThrowIfCancellationRequested();

                            var resp = QueryAllTargetPositions(subAccountId, landingInfo, clt);
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
                        PositionItemsRefreshStateChanged?.Invoke(new SubAccountAnyDataRefreshStateChangedArgs(landingInfo.SubUserId, stateHolder.SubAccountId, stateHolder.DataRefreshState));
                    }
                    return;
                }
                
                lock (positionItemsLock)
                {
                    if (loginDataService.ProxyLoginResp == null) return;

                    foreach (var stateHolder in refreshStateHolders)
                    {
                        IInterfaceInteractResponse<IEnumerable<StatPositionSummaryEx>> resp = null;
                        queryResps.TryGetValue(stateHolder.SubAccountId, out resp);
                        stateHolder.DataRefreshState = (resp?.CorrectResult != null) ? DataRefreshState.SuccessRefreshed : DataRefreshState.FailedRefreshed;
                        PositionItemsRefreshStateChanged?.Invoke(new SubAccountAnyDataRefreshStateChangedArgs(landingInfo.SubUserId, stateHolder.SubAccountId, stateHolder.DataRefreshState));

                        if (clt.IsCancellationRequested) continue;

                        if (resp != null)
                        {
                            var queriedList = resp.CorrectResult ?? new StatPositionSummaryEx[] { };

                            // 清除 SubAccountId 的所有持仓
                            UnsafeRemoveSubAccountPositionItems(stateHolder.SubAccountId);

                            // 添加查询到的持仓
                            UnsafeAddOrUpdatePositionItems(queriedList,
                                    out IEnumerable<TargetPositionDataModel> _addList,
                                    out IEnumerable<TargetPositionDataModel> _updateList);
                        }
                    }
                }
            });
        }


        private void UnsafeRemovePositionItems(IEnumerable<TargetPositionKey> itemKeys)
        {
            if (itemKeys?.Any() != true) return; 

            var rmItems = new List<TargetPositionDataModel>();
            foreach (var _rmKey in itemKeys)
            {
                if (positionItems.TryGetValue(_rmKey, out TargetPositionDataModel rmItem))
                {
                    rmItems.Add(rmItem);
                    positionItems.Remove(_rmKey);
                }
            }

            if (rmItems.Any())
            {
                UIItemsAddOrRemoveDispatcherBeginInvoke(() =>
                {
                    foreach (var rmItem in rmItems)
                    {
                        targetPositionService.PositionItems.Remove(rmItem);
                    }
                });
            }
        }

        /// <summary>
        /// 删除某个操作账户的所有持仓。如果 subAccountId 为 null，则删除所有
        /// </summary>
        /// <param name="subAccountId"></param>
        private void RemoveSubAccountPositionItems(long? subAccountId)
        {
            lock (positionItemsLock)
            {
                UnsafeRemoveSubAccountPositionItems(subAccountId);
            }
        }

        /// <summary>
        /// 非线程安全地删除某个操作账户的所有持仓。如果 subAccountId 为 null，则删除所有
        /// </summary>
        /// <param name="subAccountId"></param>
        private void UnsafeRemoveSubAccountPositionItems(long? subAccountId)
        {
            KeyValuePair<TargetPositionKey, TargetPositionDataModel>[] rmItems = null;
            if (subAccountId == null)
                rmItems = positionItems.ToArray();
            else
                rmItems = positionItems.Where(i => i.Key.SubAccountId == subAccountId).ToArray();

            foreach (var rmItem in rmItems)
            {
                positionItems.Remove(rmItem.Key);
            }
            UIItemsAddOrRemoveDispatcherBeginInvoke(() =>
            {
                foreach (var rmItem in rmItems)
                {
                    targetPositionService.PositionItems.Remove(rmItem.Value);
                }
            });
        }

        private static bool IsRefreshingOrSuccessRefreshed(DataRefreshState refreshState)
        {
            return refreshState == DataRefreshState.Refreshing || refreshState == DataRefreshState.SuccessRefreshed;
        }

        private SubAccountAnyDataRefreshStateHolder AcquireSubAccountPositionItemsRefreshStateHolder(long subAccountId)
        {
            lock (positionItemsRefreshLock)
            {
                SubAccountAnyDataRefreshStateHolder dataHolder = null;
                if (!positionItemsRefreshStateHolders.TryGetValue(subAccountId, out dataHolder))
                {
                    dataHolder = new SubAccountAnyDataRefreshStateHolder(subAccountId);
                    positionItemsRefreshStateHolders.Add(subAccountId, dataHolder);
                }
                return dataHolder;
            }
        }

        private CancellationToken AcquireSubAccountPositionItemsRefreshCLT()
        {
            CancellationToken clt = CancellationToken.None;
            lock (positionItemsRefreshLock)
            {
                if (positionItemsRefreshCTS == null)
                {
                    positionItemsRefreshCTS = new CancellationTokenSource();
                }
                clt = positionItemsRefreshCTS.Token;
            }
            return clt;
        }

        private void CancelSubAccountPositionItemsRefresh(bool clearRefreshStateHolders)
        {
            lock (positionItemsRefreshLock)
            {
                if (positionItemsRefreshCTS != null)
                {
                    positionItemsRefreshCTS.Cancel();
                    positionItemsRefreshCTS.Dispose();
                    positionItemsRefreshCTS = null;
                }

                if (clearRefreshStateHolders)
                {
                    positionItemsRefreshStateHolders.Clear();
                }
            }
        }
        
        private void UnsafeUpdatePositionItem(TargetPositionKey itemKey, TargetPositionUpdateTemplateFactory updateTemplateFactory)
        {
            Debug.Assert(itemKey != null);
            if (positionItems.TryGetValue(itemKey, out TargetPositionDataModel existItem))
            {
                var temp = updateTemplateFactory?.Invoke(itemKey, existItem);
                UpdatePositionItemWithTemplate(existItem, temp);
            }
        }
        
        private void UnsafeAddOrUpdatePositionItems(
            IEnumerable<StatPositionSummaryEx> positionItems,
            out IEnumerable<TargetPositionDataModel> _addedItems,
            out IEnumerable<TargetPositionDataModel> _updatedItems)
        {
            _addedItems = null;
            _updatedItems = null;

            if (positionItems?.Any() != true) return;

            var itemMap = positionItems.ToDictionary(i => new TargetPositionKey(i.TargetType.ToClientXQOrderTargetType(), i.SubAccountId, i.TargetKey));

            UnsafeAddOrUpdatePositionItems(itemMap.Keys,
                _key =>
                {
                    return GenerateTargetPositionItem(_key);
                },
                (_key, _existItem) =>
                {
                    var hostingItem = itemMap[_key];

                    var pSummary = hostingItem.PositionSummary;

                    if (_existItem != null && pSummary != null)
                    {
                        if (_existItem.DynamicInfoModifyTimestampMs > pSummary.LastmodifyTimestampMs)
                        {
                            return null;
                        }
                    }

                    var template = new TargetPositionUpdateTemplate();
                    if (pSummary != null)
                    {
                        template.LongPosition = new Tuple<long?>(pSummary.__isset.longPosition ? (long?)pSummary.LongPosition : null);
                        template.ShortPosition = new Tuple<long?>(pSummary.__isset.shortPosition ? (long?)pSummary.ShortPosition : null);
                        template.NetPosition = new Tuple<long?>(pSummary.__isset.netPosition ? (long?)pSummary.NetPosition : null);
                        template.PositionAvgPrice = new Tuple<double?>(pSummary.__isset.positionAvgPrice ? (double?)pSummary.PositionAvgPrice : null);
                        if (pSummary.__isset.lastmodifyTimestampMs)
                            template.LastModifyTimestampMs = pSummary.LastmodifyTimestampMs;
                    }

                    TargetPositionDynamicInfo destDynamicInfo = null;
                    var srcDynamicInfo = hostingItem.PositionDynamicInfo;
                    if (srcDynamicInfo != null)
                    {
                        template.DynamicInfoModifyTimestampMs = template.LastModifyTimestampMs;
                        destDynamicInfo = new TargetPositionDynamicInfo();
                        if (srcDynamicInfo.__isset.lastPrice)
                            destDynamicInfo.LastPrice = srcDynamicInfo.LastPrice;
                        if (srcDynamicInfo.__isset.positionProfit)
                            destDynamicInfo.PositionProfit = srcDynamicInfo.PositionProfit;
                        if (srcDynamicInfo.__isset.closedProfit)
                            destDynamicInfo.ClosedProfit = srcDynamicInfo.ClosedProfit;
                        if (srcDynamicInfo.__isset.totalProfit)
                            destDynamicInfo.TotalProfit = srcDynamicInfo.TotalProfit;
                        if (srcDynamicInfo.__isset.positionValue)
                            destDynamicInfo.PositionValue = srcDynamicInfo.PositionValue;
                        if (srcDynamicInfo.__isset.leverage)
                            destDynamicInfo.Leverage = srcDynamicInfo.Leverage;
                        if (srcDynamicInfo.__isset.currency)
                            destDynamicInfo.Currency = srcDynamicInfo.Currency;
                    }
                    template.DynamicInfo = new Tuple<TargetPositionDynamicInfo>(destDynamicInfo);

                    return template;
                },
                out _addedItems,
                out _updatedItems);
        }
        
        private void UnsafeAddOrUpdatePositionItems(
            IEnumerable<TargetPositionKey> newItemKeys,
            Func<TargetPositionKey, TargetPositionDataModel> newItemFactory,
            TargetPositionUpdateTemplateFactory updateTemplateFactory,
            out IEnumerable<TargetPositionDataModel> _addedItems,
            out IEnumerable<TargetPositionDataModel> _updatedItems)
        {
            _addedItems = null;
            _updatedItems = null;

            Debug.Assert(newItemFactory != null);

            if (newItemKeys?.Any() != true) return;

            var addList = new List<TargetPositionDataModel>();
            var updateList = new List<TargetPositionDataModel>();
            foreach (var itemKey in newItemKeys)
            {
                TargetPositionDataModel implItem = null;
                TargetPositionDataModel existItem = null;
                positionItems.TryGetValue(itemKey, out existItem);
                if (existItem == null)
                {
                    var newItem = newItemFactory.Invoke(itemKey);
                    if (newItem != null)
                    {
                        positionItems[itemKey] = newItem;
                        addList.Add(newItem);
                        implItem = newItem;
                    }
                }
                else
                {
                    updateList.Add(existItem);
                    implItem = existItem;
                }

                if (implItem != null && updateTemplateFactory != null)
                {
                    var temp = updateTemplateFactory(itemKey, existItem);
                    UpdatePositionItemWithTemplate(implItem, temp);
                }
            }

            if (addList.Any())
            {
                UIItemsAddOrRemoveDispatcherBeginInvoke(() =>
                {
                    targetPositionService.PositionItems.AddRange(addList);
                });
            }

            _addedItems = addList;
            _updatedItems = updateList;
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

        private TargetPositionDataModel GenerateTargetPositionItem(TargetPositionKey itemKey)
        {
            var subAccountFields = new SubAccountFieldsForTradeData(null, itemKey.SubAccountId);
            TradeDMLoadHelper.SetupSubAccountFields(subAccountFields,
                subAccountRelatedItemQueryCtrl, subAccountRelatedItemCacheCtrl,
                hostingUserQueryCtrl, hostingUserCacheCtrl);

            var newItem = new TargetPositionDataModel(itemKey.TargetType,
                subAccountFields, itemKey.TargetKey);
            
            if (newItem.TargetType == ClientXQOrderTargetType.CONTRACT_TARGET)
            {
                // 合约标的持仓
                int contractId = Convert.ToInt32(newItem.TargetKey);
                newItem.TargetContractDetailContainer = new TargetContract_TargetContractDetail(contractId);
                XueQiaoFoundationHelper.SetupTargetContract_ContractDetail(newItem.TargetContractDetailContainer, contractItemTreeQueryCtrl,
                    XqContractNameFormatType.CommodityAcronym_Code_ContractCode,
                    setupedDetail =>
                    {
                        XqTargetDMHelper.InvalidateTargetNameWithAppropriate(newItem, XqAppLanguages.CN);
                        var nowTimestamp = (long)DateHelper.NowUnixTimeSpan().TotalSeconds;
                        InvalidatePositionIsExpired(newItem, nowTimestamp);
                        RectifyPositionRelatedPriceProps(newItem);
                    });
            }
            else if (newItem.TargetType == ClientXQOrderTargetType.COMPOSE_TARGET)
            {
                // 组合标的持仓
                long composeGraphId = Convert.ToInt64(newItem.TargetKey);

                newItem.TargetComposeDetailContainer = new TargetCompose_ComposeDetail(composeGraphId);
                XueQiaoFoundationHelper.SetupTargetCompose_ComposeDetail(newItem.TargetComposeDetailContainer,
                    composeGraphCacheCtrl, composeGraphQueryCtrl,
                    userComposeViewCacheCtrl, contractItemTreeQueryCtrl,
                    XqContractNameFormatType.CommodityAcronym_Code_ContractCode,
                    _det =>
                    {
                        XqTargetDMHelper.InvalidateTargetNameWithAppropriate(newItem, XqAppLanguages.CN);
                        var nowTimestamp = (long)DateHelper.NowUnixTimeSpan().TotalSeconds;
                        InvalidatePositionIsExpired(newItem, nowTimestamp);
                    });

                newItem.TargetComposeUserComposeViewContainer = new UserComposeViewContainer(composeGraphId);
                XueQiaoFoundationHelper.SetupUserComposeView(newItem.TargetComposeUserComposeViewContainer,
                    userComposeViewCacheCtrl, userComposeViewQueryCtrl, false, true,
                    _detail =>
                    {
                        XqTargetDMHelper.InvalidateTargetNameWithAppropriate(newItem, XqAppLanguages.CN);
                        RectifyPositionRelatedPriceProps(newItem);
                    });
            }

            return newItem;
        }
        
        /// <summary>
        /// 更新持仓项的过期状态。和指定的时间比较，如果标的过期时间小于指定时间，则标识持仓过期
        /// </summary>
        /// <param name="positionItem"></param>
        /// <param name="compareTimestamp">指定的比较时间</param>
        private static void InvalidatePositionIsExpired(TargetPositionDataModel positionItem, long compareTimestamp)
        {
            if (positionItem == null) return;
            if (positionItem.TargetType == ClientXQOrderTargetType.CONTRACT_TARGET)
            {
                var contractExpDate = positionItem.TargetContractDetailContainer?.ContractDetail?.ContractExpDate;
                if (contractExpDate != null)
                {
                    positionItem.IsXqTargetExpired = (compareTimestamp > contractExpDate.Value);
                }
            }
            else if (positionItem.TargetType == ClientXQOrderTargetType.COMPOSE_TARGET)
            {
                var existExpiredLeg = positionItem.TargetComposeDetailContainer?.DetailLegs?
                    .Any(i =>
                    {
                        var expDate = i.LegDetailContainer?.ContractDetail?.ContractExpDate;
                        return expDate != null && (compareTimestamp > expDate);
                    }) ?? null;
                if (existExpiredLeg != null)
                {
                    positionItem.IsXqTargetExpired = existExpiredLeg.Value;
                }
            }
        }

        /// <summary>
        /// 纠正持仓项的价格相关属性
        /// </summary>
        /// <param name="positionItem"></param>
        private static void RectifyPositionRelatedPriceProps(TargetPositionDataModel positionItem)
        {
            if (positionItem == null) return;

            var tickSize = XueQiaoConstants.XQContractPriceMinimumPirceTick;
            if (positionItem.TargetType == ClientXQOrderTargetType.CONTRACT_TARGET)
            {
                var commodityTickSize = positionItem.TargetContractDetailContainer?.CommodityDetail?.TickSize;
                if (commodityTickSize != null)
                {
                    tickSize = commodityTickSize.Value;
                }
            }
            else if (positionItem.TargetType == ClientXQOrderTargetType.COMPOSE_TARGET)
            {
                tickSize = XueQiaoBusinessHelper.CalculteXqTargetPriceTick(positionItem.TargetComposeUserComposeViewContainer?.UserComposeView?.PrecisionNumber,
                    XueQiaoConstants.XQComposePriceMaximumPirceTick);
            }

            // 纠正 PositionAvgPrice
            var positionAvgPrice = positionItem.PositionAvgPrice;
            if (positionAvgPrice != null)
            {
                positionItem.PositionAvgPrice = MathHelper.MakeValuePrecise(positionAvgPrice.Value, tickSize * XueQiaoConstants.XQMultipleCalculatedPriceTickRate);
            }

            // 纠正 DynamicInfo 中的价格
            var dynamicInfo = positionItem.DynamicInfo;
            if (dynamicInfo != null)
            {
                var lastPrice = dynamicInfo.LastPrice;
                if (lastPrice != null)
                {
                    dynamicInfo.LastPrice = MathHelper.MakeValuePrecise(lastPrice.Value, tickSize);
                }
            }
        }

        private void UpdatePositionItemWithTemplate(TargetPositionDataModel positionItem,
            TargetPositionUpdateTemplate updateTemplate)
        {
            if (positionItem == null || updateTemplate == null) return;

            bool needRectifyPriceRelatedProps = false;

            if (updateTemplate.LongPosition != null) positionItem.LongPosition = updateTemplate.LongPosition.Item1;
            if (updateTemplate.ShortPosition != null) positionItem.ShortPosition = updateTemplate.ShortPosition.Item1;
            if (updateTemplate.NetPosition != null) positionItem.NetPosition = updateTemplate.NetPosition.Item1;
            
            if (updateTemplate.LastModifyTimestampMs != null)
                positionItem.LastModifyTimestampMs = updateTemplate.LastModifyTimestampMs.Value;

            if (updateTemplate.DynamicInfoModifyTimestampMs != null)
                positionItem.DynamicInfoModifyTimestampMs = updateTemplate.DynamicInfoModifyTimestampMs.Value;

            if (updateTemplate.IsXqTargetExpired != null)
                positionItem.IsXqTargetExpired = updateTemplate.IsXqTargetExpired.Item1;
            
            // 持仓均价特殊处理
            if (updateTemplate.PositionAvgPrice != null)
            {
                positionItem.PositionAvgPrice = updateTemplate.PositionAvgPrice.Item1;
                needRectifyPriceRelatedProps = true;
            }

            if (updateTemplate.DynamicInfo != null)
            {
                positionItem.DynamicInfo = updateTemplate.DynamicInfo.Item1;
                needRectifyPriceRelatedProps = true;
            }

            if (needRectifyPriceRelatedProps)
            {
                RectifyPositionRelatedPriceProps(positionItem);
            }
        }

        private IInterfaceInteractResponse<IEnumerable<StatPositionSummaryEx>> QueryAllTargetPositions(long subAccountId,
            LandingInfo landingInfo,
            CancellationToken clt)
        {
            if (clt.IsCancellationRequested) return null;
            if (landingInfo == null) return null;

            var queryPageSize = 50;
            IInterfaceInteractResponse<StatPositionSummaryExPage> firstPageResp = null;
            var queryOption = new QueryStatPositionSummaryOption
            {
                SubAccountId = subAccountId
            };

            var queryAllCtrl = new QueryAllItemsByPageHelper<StatPositionSummaryEx>(pageIndex => {
                if (clt.IsCancellationRequested) return null;
                var pageOption = new IndexedPageOption
                {
                    NeedTotalCount = true,
                    PageIndex = pageIndex,
                    PageSize = queryPageSize
                };

                var resp = XqThriftLibConfigurationManager.SharedInstance.TradeHostingTerminalAoHttpStub
                    .queryStatPositionSummaryExPage(landingInfo, queryOption, pageOption);
                if (resp == null) return null;
                if (pageIndex == 0)
                {
                    firstPageResp = resp;
                }
                var pageInfo = resp.CorrectResult;
                var pageResult = new QueryItemsByPageResult<StatPositionSummaryEx>(resp.SourceException != null)
                {
                    TotalCount = pageInfo?.TotalNum,
                    Page = pageInfo?.StatPositionSummaryExList
                };
                return pageResult;
            });

            queryAllCtrl.RemoveDuplicateFunc = _items =>
            {
                if (_items == null) return null;
                var idGroupedItems = _items.GroupBy(i => new TargetPositionKey(i.TargetType.ToClientXQOrderTargetType(), i.SubAccountId, i.TargetKey));
                return idGroupedItems.Select(i => i.LastOrDefault()).ToArray();
            };

            var queriedItems = queryAllCtrl.QueryAllItems();
            if (firstPageResp == null) return null;
            if (clt.IsCancellationRequested) return null;

            var tarResp = new InterfaceInteractResponse<IEnumerable<StatPositionSummaryEx>>(firstPageResp.Servant,
                firstPageResp.InterfaceName,
                firstPageResp.SourceException,
                firstPageResp.HasTransportException,
                firstPageResp.HttpResponseStatusCode,
                queriedItems)
            {
                CustomParsedExceptionResult = firstPageResp.CustomParsedExceptionResult,
                InteractInformation = firstPageResp.InteractInformation
            };
            return tarResp;
        }

        private void StartCheckTargetExpiredTimer()
        {
            StopCheckTargetExpiredTimer();
            // 10 minute interval
            checkTargetExpiredTimer = new System.Timers.Timer(1000 * 60 * 10);
            checkTargetExpiredTimer.Elapsed += CheckTargetExpiredTimer_Elapsed;
            checkTargetExpiredTimer.Start();
        }

        private void CheckTargetExpiredTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var nowTimestamp = (long)DateHelper.NowUnixTimeSpan().TotalSeconds;
            lock (positionItemsLock)
            {
                var items = positionItems.Values;
                foreach (var item in items)
                {
                    InvalidatePositionIsExpired(item, nowTimestamp);
                }
            }
        }

        private void StopCheckTargetExpiredTimer()
        {
            if (checkTargetExpiredTimer != null)
            {
                checkTargetExpiredTimer.Elapsed -= CheckTargetExpiredTimer_Elapsed;
                checkTargetExpiredTimer.Stop();
                checkTargetExpiredTimer.Dispose();
                checkTargetExpiredTimer = null;
            }
        }
    }
}
