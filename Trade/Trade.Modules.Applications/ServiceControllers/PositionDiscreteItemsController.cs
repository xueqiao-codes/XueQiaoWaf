using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Schedulers;
using xueqiao.trade.hosting.asset.thriftapi;
using xueqiao.trade.hosting.events;
using xueqiao.trade.hosting.push.protocol;
using xueqiao.trade.hosting.terminal.ao;
using XueQiaoFoundation.Interfaces.Applications;
using XueQiaoFoundation.BusinessResources.Constants;
using XueQiaoFoundation.Interfaces.Event.business;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Helper;
using XueQiaoWaf.Trade.Modules.Applications.ServiceControllers.Events;
using XueQiaoWaf.Trade.Modules.Applications.Services;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.Interfaces.Helper;
using XueQiaoWaf.Trade.Interfaces.DataModels;
using xueqiao.trade.hosting.proxy;
using XueQiaoFoundation.BusinessResources.Models;
using lib.xqclient_base.thriftapi_mediation;
using lib.xqclient_base.logger;
using System.Collections.Concurrent;
using lib.xqclient_base.thriftapi_mediation.Interface;
using System.Windows.Threading;
using System.Diagnostics;
using business_foundation_lib.xq_thriftlib_config;
using XueQiaoFoundation.Shared.Model;

namespace XueQiaoWaf.Trade.Modules.Applications.ServiceControllers
{
    /// <summary>
    /// 散列持仓控制器
    /// 
    /// 1.承担拉取散列持仓列表的职责。
    /// 2.处理散列持仓相关的 push event。
    /// </summary>
    [Export(typeof(IPositionDiscreteItemsController)), Export(typeof(ITradeModuleSingletonController)),
        PartCreationPolicy(CreationPolicy.Shared)]
    internal class PositionDiscreteItemsController : IPositionDiscreteItemsController, ITradeModuleSingletonController
    {
        private delegate PositionDiscreteItemUpdateTemplate PositionUpdateTemplateFactory(PositionDiscreteItemKey itemKey, PositionDiscreteItemDataModel existItem);

        // 持仓更新模板
        private class PositionDiscreteItemUpdateTemplate
        {
            // 创建时间
            public Tuple<long?> UpdateTimestamp;

            // 上日持仓
            public Tuple<long?> PrevPosition;

            // 今日长持
            public Tuple<long?> LongPosition;

            // 今日短持
            public Tuple<long?> ShortPosition;

            // 占用手续费
            public Tuple<double?> UseCommission;

            // 净仓
            public Tuple<long?> NetPosition;

            // 平仓盈亏
            public Tuple<double?> CloseProfit;

            // 持仓盈亏
            public Tuple<double?> PositionProfit;

            // 计算价格
            public Tuple<double?> CalculatePrice;

            // 持仓均价
            public Tuple<double?> PositionAvgPrice;

            // 占用保证金
            public Tuple<double?> UseMargin;

            // 冻结保证金
            public Tuple<double?> FrozenMargin;

            // 冻结手续费
            public Tuple<double?> FrozenCommission;

            // 币种
            public Tuple<string> Currency;

            // 市值
            public Tuple<double?> GoodsValue;

            // 杠杆
            public Tuple<double?> Leverage;

            // 标的是否过期
            public Tuple<bool?> IsXqTargetExpired;
        }

        private readonly PositionDiscreteItemsService positionDiscreteItemsService;
        private readonly IEventAggregator eventAggregator;
        private readonly ILoginDataService loginDataService;
        private readonly Lazy<ILoginUserManageService> loginUserManageService;
        
        private readonly IRelatedSubAccountItemsController relatedSubAccountItemsCtrl;
        private readonly IUserSubAccountRelatedItemQueryController subAccountRelatedItemQueryCtrl;
        private readonly IUserSubAccountRelatedItemCacheController subAccountRelatedItemCacheCtrl;
        private readonly IHostingUserQueryController hostingUserQueryCtrl;
        private readonly IHostingUserCacheController hostingUserCacheCtrl;
        private readonly IContractItemTreeQueryController contractItemTreeQueryCtrl;

        /// <summary>
        /// 持仓同步任务工厂。该工厂生产的任务是顺序执行，将增删、刷新包装成任务执行，才可保证同步的准确性
        /// </summary>
        private readonly TaskFactory positionSyncTaskFactory = new TaskFactory(new OrderedTaskScheduler());
        /// <summary>
        /// 持仓列表
        /// </summary>
        private readonly Dictionary<PositionDiscreteItemKey, PositionDiscreteItemDataModel> positionItems = new Dictionary<PositionDiscreteItemKey, PositionDiscreteItemDataModel>();
        private readonly object positionItemsLock = new object();

        private CancellationTokenSource positionItemsRefreshCTS;
        private readonly object positionItemsRefreshLock = new object();
        private readonly Dictionary<long, SubAccountAnyDataRefreshStateHolder> positionItemsRefreshStateHolders
            = new Dictionary<long, SubAccountAnyDataRefreshStateHolder>();

        private readonly IDIncreaser clientDataForceSyncTimesIncreaser = new IDIncreaser(0);

        // 检查标的是否过期的定时器
        private System.Timers.Timer checkTargetExpiredTimer;

        [ImportingConstructor]
        public PositionDiscreteItemsController(
            PositionDiscreteItemsService positionDiscreteItemsService,
            IEventAggregator eventAggregator,
            ILoginDataService loginDataService,
            Lazy<ILoginUserManageService> loginUserManageService,
            
            IRelatedSubAccountItemsController relatedSubAccountItemsCtrl,
            IUserSubAccountRelatedItemQueryController subAccountRelatedItemQueryCtrl,
            IUserSubAccountRelatedItemCacheController subAccountRelatedItemCacheCtrl,
            IHostingUserQueryController hostingUserQueryCtrl,
            IHostingUserCacheController hostingUserCacheCtrl,
            IContractItemTreeQueryController contractItemTreeQueryCtrl)
        {
            this.positionDiscreteItemsService = positionDiscreteItemsService;
            this.eventAggregator = eventAggregator;
            this.loginDataService = loginDataService;
            this.loginUserManageService = loginUserManageService;
            
            this.relatedSubAccountItemsCtrl = relatedSubAccountItemsCtrl;

            this.subAccountRelatedItemQueryCtrl = subAccountRelatedItemQueryCtrl;
            this.subAccountRelatedItemCacheCtrl = subAccountRelatedItemCacheCtrl;
            this.hostingUserQueryCtrl = hostingUserQueryCtrl;
            this.hostingUserCacheCtrl = hostingUserCacheCtrl;
            this.contractItemTreeQueryCtrl = contractItemTreeQueryCtrl;

            loginUserManageService.Value.HasLogined += RecvUserLogined;
            loginUserManageService.Value.HasLogouted += RecvUserHasLogouted;
            eventAggregator.GetEvent<ClientDataForceSync>().Subscribe(ReceivedClientDataForceSyncEvent);
            eventAggregator.GetEvent<HostingPositionVolumeChanged>().Subscribe(ReceivedHostingPositionVolumeEvent);
            eventAggregator.GetEvent<HostingPositionFundChanged>().Subscribe(ReceivedHostingPositionFundEvent);
            eventAggregator.GetEvent<UserRelatedSubAccountItemsRefreshEvent>().Subscribe(RecvUserRelatedSubAccountItemsRefreshEvent);
        }
        
        public void Shutdown()
        {
            StopCheckTargetExpiredTimer();
            clientDataForceSyncTimesIncreaser?.Reset();
            CancelSubAccountPositionItemsRefresh(true);

            loginUserManageService.Value.HasLogined -= RecvUserLogined;
            loginUserManageService.Value.HasLogouted -= RecvUserHasLogouted;
            eventAggregator.GetEvent<ClientDataForceSync>().Unsubscribe(ReceivedClientDataForceSyncEvent);
            eventAggregator.GetEvent<HostingPositionVolumeChanged>().Unsubscribe(ReceivedHostingPositionVolumeEvent);
            eventAggregator.GetEvent<HostingPositionFundChanged>().Subscribe(ReceivedHostingPositionFundEvent);
            eventAggregator.GetEvent<UserRelatedSubAccountItemsRefreshEvent>().Unsubscribe(RecvUserRelatedSubAccountItemsRefreshEvent);

        }

        public event SubAccountAnyDataRefreshStateChanged DiscretePositionItemsRefreshStateChanged;

        public void RefreshDiscretePositionItemsForce(long subAccountId)
        {
            RefreshSubAccountPositionItems(new long[] { subAccountId }, true);
        }
        
        public void RefreshDiscretePositionItemsIfNeed(long subAccountId)
        {
            RefreshSubAccountPositionItems(new long[] { subAccountId }, false);
        }

        public IEnumerable<long> ExistPositionSubAccountIdsOfContract(int contractId)
        {
            IEnumerable<long> results = null;
            lock (positionItemsLock)
            {
                var tarItems = positionItems.Values.Where(i => i.IsExistPosition && contractId == i.ContractId);
                results = tarItems.GroupBy(i => i.SubAccountFields.SubAccountId).Select(i => i.Key).ToArray();
            }
            return results;
        }
        
        public void RequestDeleteExpiredPosition(PositionDiscreteItemKey itemKey)
        {
            if (itemKey == null) return;
            var landingInfo = loginDataService.LandingInfo;
            if (landingInfo == null) return;

            var siip = new StubInterfaceInteractParams { TransportConnectTimeoutMS = 2000, TransportReadTimeoutMS = 2000 };
            XqThriftLibConfigurationManager.SharedInstance.TradeHostingTerminalAoHttpStub.deleteExpiredContractPositionAsync(landingInfo, itemKey.SubAccountId, itemKey.ContractId, CancellationToken.None, siip)
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
                                UnsafeRemovePositionItems(new PositionDiscreteItemKey[] { itemKey });
                            }
                        });
                    }
                    else
                    {
                        AppLog.Error($"Failed to deleteExpiredContractPosition.", resp?.SourceException);
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
        
        private void ReceivedHostingPositionVolumeEvent(HostingPositionVolumeEvent msg)
        {
            var positionVolume = msg?.PositionVolume;
            if (positionVolume == null) return;

            if (!msg.__isset.eventCreateTimestampMs) return;

            lock (positionItemsLock)
            {
                if (loginDataService.ProxyLoginResp == null) return;

                var itemKey = new PositionDiscreteItemKey(positionVolume.SledContractId, positionVolume.SubAccountId);
                UnsafeAddOrUpdatePositionItems(new PositionDiscreteItemKey[] { itemKey },
                    _key => GeneratePositionItem(_key),
                    (_key, _existItem) =>
                    {
                        var msgCreateTimestamp = msg.EventCreateTimestampMs / 1000;
                        if (_existItem != null && _existItem.UpdateTimestamp > msgCreateTimestamp)
                            return null;
                        
                        var template = new PositionDiscreteItemUpdateTemplate();

                        if (positionVolume.__isset.currency)
                            template.Currency = new Tuple<string>(positionVolume.Currency);

                        template.UpdateTimestamp = new Tuple<long?>(msgCreateTimestamp);

                        template.PrevPosition = new Tuple<long?>(positionVolume.__isset.prevPosition ? (long?)positionVolume.PrevPosition : null);
                        template.LongPosition = new Tuple<long?>(positionVolume.__isset.longPosition ? (long?)positionVolume.LongPosition : null);
                        template.ShortPosition = new Tuple<long?>(positionVolume.__isset.shortPosition ? (long?)positionVolume.ShortPosition : null);
                        template.NetPosition = new Tuple<long?>(positionVolume.__isset.netPosition ? (long?)positionVolume.NetPosition : null);
                        template.UseCommission = new Tuple<double?>(positionVolume.__isset.useCommission ? (double?)positionVolume.UseCommission : null);
                        template.CloseProfit = new Tuple<double?>(positionVolume.__isset.closeProfit ? (double?)positionVolume.CloseProfit : null);
                        template.PositionAvgPrice = new Tuple<double?>(positionVolume.__isset.positionAvgPrice ? (double?)positionVolume.PositionAvgPrice : null);

                        return template;
                    },
                    out IEnumerable<PositionDiscreteItemDataModel> addedItems,
                    out IEnumerable<PositionDiscreteItemDataModel> updatedItems);
            }
        }

        private void ReceivedHostingPositionFundEvent(HostingPositionFundEvent msg)
        {
            var positionFund = msg?.PositionFund;
            if (positionFund == null) return;

            if (!msg.__isset.eventCreateTimestampMs) return;

            lock (positionItemsLock)
            {
                if (loginDataService.ProxyLoginResp == null) return;

                var itemKey = new PositionDiscreteItemKey(positionFund.SledContractId, positionFund.SubAccountId);
                UnsafeAddOrUpdatePositionItems(new PositionDiscreteItemKey[] { itemKey },
                    _key => GeneratePositionItem(_key),
                    (_key, _existItem) =>
                    {
                        var msgCreateTimestamp = msg.EventCreateTimestampMs / 1000;
                        if (_existItem != null && _existItem.UpdateTimestamp > msgCreateTimestamp)
                            return null;

                        var template = new PositionDiscreteItemUpdateTemplate();

                        if (positionFund.__isset.currency)
                            template.Currency = new Tuple<string>(positionFund.Currency);

                        template.UpdateTimestamp = new Tuple<long?>(msgCreateTimestamp);

                        template.PositionProfit = new Tuple<double?>(positionFund.__isset.positionProfit ? (double?)positionFund.PositionProfit : null);
                        template.CalculatePrice = new Tuple<double?>(positionFund.__isset.calculatePrice ? (double?)positionFund.CalculatePrice : null);
                        template.UseMargin = new Tuple<double?>(positionFund.__isset.useMargin ? (double?)positionFund.UseMargin : null);
                        template.FrozenMargin = new Tuple<double?>(positionFund.__isset.frozenMargin ? (double?)positionFund.FrozenMargin : null);
                        template.FrozenCommission = new Tuple<double?>(positionFund.__isset.frozenCommission ? (double?)positionFund.FrozenCommission : null);
                        template.GoodsValue = new Tuple<double?>(positionFund.__isset.goodsValue ? (double?)positionFund.GoodsValue : null);
                        template.Leverage = new Tuple<double?>(positionFund.__isset.leverage ? (double?)positionFund.Leverage : null);

                        return template;
                    },
                    out IEnumerable<PositionDiscreteItemDataModel> addedItems,
                    out IEnumerable<PositionDiscreteItemDataModel> updatedItems);
            }
        }

        private void RecvUserRelatedSubAccountItemsRefreshEvent(UserRelatedSubAccountItemsRefreshEventArgs args)
        {
            var currentLoginToken = loginDataService?.ProxyLoginResp?.HostingSession?.Token;
            if (currentLoginToken == null || currentLoginToken != args.LoginUserToken) return;

            var relatedSubAccountIds = args.RelatedSubAccountItems?.Select(i => i.SubAccountId).Distinct();
            if (relatedSubAccountIds?.Any() != true) return;

            RefreshSubAccountPositionItems(relatedSubAccountIds, false);
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
                var queryResps = new ConcurrentDictionary<long, IInterfaceInteractResponse<HostingSledContractPositionPage>>();
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
                        DiscretePositionItemsRefreshStateChanged?.Invoke(new SubAccountAnyDataRefreshStateChangedArgs(landingInfo.SubUserId, stateHolder.SubAccountId, stateHolder.DataRefreshState));
                        var tsk = Task.Run(() =>
                        {
                            clt.ThrowIfCancellationRequested();

                            var resp = QueryAllPositions(subAccountId, landingInfo, clt);
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
                        DiscretePositionItemsRefreshStateChanged?.Invoke(new SubAccountAnyDataRefreshStateChangedArgs(landingInfo.SubUserId, stateHolder.SubAccountId, stateHolder.DataRefreshState));
                    }
                    return;
                }
                
                lock (positionItemsLock)
                {
                    if (loginDataService.ProxyLoginResp == null) return;

                    foreach (var stateHolder in refreshStateHolders)
                    {
                        IInterfaceInteractResponse<HostingSledContractPositionPage> resp = null;
                        queryResps.TryGetValue(stateHolder.SubAccountId, out resp);
                        stateHolder.DataRefreshState = (resp?.CorrectResult != null) ? DataRefreshState.SuccessRefreshed : DataRefreshState.FailedRefreshed;
                        DiscretePositionItemsRefreshStateChanged?.Invoke(new SubAccountAnyDataRefreshStateChangedArgs(landingInfo.SubUserId, stateHolder.SubAccountId, stateHolder.DataRefreshState));

                        if (clt.IsCancellationRequested) continue;

                        if (resp != null)
                        {
                            IEnumerable<HostingSledContractPosition> queriedList = resp.CorrectResult?.Page;
                            if (queriedList == null)
                                queriedList = new HostingSledContractPosition[] { };

                            // 清除 SubAccountId 的所有持仓
                            UnsafeRemoveSubAccountPositionItems(stateHolder.SubAccountId);

                            // 添加查询到的持仓
                            UnsafeAddOrUpdatePositionItems(queriedList,
                                    out IEnumerable<PositionDiscreteItemDataModel> _addList,
                                    out IEnumerable<PositionDiscreteItemDataModel> _updateList);
                        }
                    }
                }
            });
        }


        private void UnsafeRemovePositionItems(IEnumerable<PositionDiscreteItemKey> itemKeys)
        {
            if (itemKeys?.Any() != true) return;

            var rmItems = new List<PositionDiscreteItemDataModel>();
            foreach (var _rmKey in itemKeys)
            {
                if (positionItems.TryGetValue(_rmKey, out PositionDiscreteItemDataModel rmItem))
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
                        positionDiscreteItemsService.PositionItems.Remove(rmItem);
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
            KeyValuePair<PositionDiscreteItemKey, PositionDiscreteItemDataModel>[] rmItems = null;
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
                    positionDiscreteItemsService.PositionItems.Remove(rmItem.Value);
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

        private void UnsafeUpdatePositionItem(PositionDiscreteItemKey itemKey, PositionUpdateTemplateFactory updateTemplateFactory)
        {
            Debug.Assert(itemKey != null);
            if (positionItems.TryGetValue(itemKey, out PositionDiscreteItemDataModel existItem))
            {
                var temp = updateTemplateFactory?.Invoke(itemKey, existItem);
                UpdatePositionItemWithTemplate(existItem, temp);
            }
        }
        
        private void UnsafeAddOrUpdatePositionItems(
            IEnumerable<HostingSledContractPosition> positionItems,
            out IEnumerable<PositionDiscreteItemDataModel> _addedItems,
            out IEnumerable<PositionDiscreteItemDataModel> _updatedItems)
        {
            _addedItems = null;
            _updatedItems = null;

            if (positionItems?.Any() != true) return;

            var itemMap = positionItems.ToDictionary(i => new PositionDiscreteItemKey(i.SledContractId, i.SubAccountId));

            UnsafeAddOrUpdatePositionItems(itemMap.Keys,
                _key =>
                {
                    return GeneratePositionItem(_key);
                },
                (_key, _existItem) =>
                {
                    var hostingItem = itemMap[_key];
                    var positionVolume = hostingItem.PositionVolume;
                    var positionFund = hostingItem.PositionFund;

                    var template = new PositionDiscreteItemUpdateTemplate
                    {
                        UpdateTimestamp = null,
                        Currency = new Tuple<string>(hostingItem.Currency)
                    };
                    if (positionVolume != null)
                    {
                        template.PrevPosition = new Tuple<long?>(positionVolume.__isset.prevPosition ? (long?)positionVolume.PrevPosition : null);
                        template.LongPosition = new Tuple<long?>(positionVolume.__isset.longPosition ? (long?)positionVolume.LongPosition : null);
                        template.ShortPosition = new Tuple<long?>(positionVolume.__isset.shortPosition ? (long?)positionVolume.ShortPosition : null);
                        template.NetPosition = new Tuple<long?>(positionVolume.__isset.netPosition ? (long?)positionVolume.NetPosition : null);
                        template.UseCommission = new Tuple<double?>(positionVolume.__isset.useCommission ? (double?)positionVolume.UseCommission : null);
                        template.CloseProfit = new Tuple<double?>(positionVolume.__isset.closeProfit ? (double?)positionVolume.CloseProfit : null);
                        template.PositionAvgPrice = new Tuple<double?>(positionVolume.__isset.positionAvgPrice ? (double?)positionVolume.PositionAvgPrice : null);
                    }
                    if (positionFund != null)
                    {
                        template.PositionProfit = new Tuple<double?>(positionFund.__isset.positionProfit ? (double?)positionFund.PositionProfit : null);
                        template.CalculatePrice = new Tuple<double?>(positionFund.__isset.calculatePrice ? (double?)positionFund.CalculatePrice : null);
                        template.UseMargin = new Tuple<double?>(positionFund.__isset.useMargin ? (double?)positionFund.UseMargin : null);
                        template.FrozenMargin = new Tuple<double?>(positionFund.__isset.frozenMargin ? (double?)positionFund.FrozenMargin : null);
                        template.FrozenCommission = new Tuple<double?>(positionFund.__isset.frozenCommission ? (double?)positionFund.FrozenCommission : null);
                        template.GoodsValue = new Tuple<double?>(positionFund.__isset.goodsValue ? (double?)positionFund.GoodsValue : null);
                        template.Leverage = new Tuple<double?>(positionFund.__isset.leverage ? (double?)positionFund.Leverage : null);
                    }
                    return template;
                },
                out _addedItems,
                out _updatedItems);
        }
        
        private void UnsafeAddOrUpdatePositionItems(
           IEnumerable<PositionDiscreteItemKey> newItemKeys,
            Func<PositionDiscreteItemKey, PositionDiscreteItemDataModel> newItemFactory,
            PositionUpdateTemplateFactory updateTemplateFactory,
            out IEnumerable<PositionDiscreteItemDataModel> _addedItems,
            out IEnumerable<PositionDiscreteItemDataModel> _updatedItems)
        {
            _addedItems = null;
            _updatedItems = null;

            Debug.Assert(newItemFactory != null);

            if (newItemKeys?.Any() != true) return;

            var addList = new List<PositionDiscreteItemDataModel>();
            var updateList = new List<PositionDiscreteItemDataModel>();
            foreach (var itemKey in newItemKeys)
            {
                PositionDiscreteItemDataModel implItem = null;
                PositionDiscreteItemDataModel existItem = null;
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
                    positionDiscreteItemsService.PositionItems.AddRange(addList);
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

        private PositionDiscreteItemDataModel GeneratePositionItem(PositionDiscreteItemKey itemKey)
        {
            var subAccountFields = new SubAccountFieldsForTradeData(null, itemKey.SubAccountId);
            TradeDMLoadHelper.SetupSubAccountFields(subAccountFields,
                subAccountRelatedItemQueryCtrl, subAccountRelatedItemCacheCtrl,
                hostingUserQueryCtrl, hostingUserCacheCtrl);

            var newItem = new PositionDiscreteItemDataModel(itemKey.ContractId, subAccountFields);
            newItem.ContractDetailContainer = new TargetContract_TargetContractDetail((int)itemKey.ContractId);
            XueQiaoFoundationHelper.SetupTargetContract_ContractDetail(newItem.ContractDetailContainer, contractItemTreeQueryCtrl,
                XqContractNameFormatType.CommodityAcronym_Code_ContractCode,
                setupedDetail =>
                {
                    var nowTimestamp = (long)DateHelper.NowUnixTimeSpan().TotalSeconds;
                    InvalidatePositionIsExpired(newItem, nowTimestamp);
                    RectifyPositionRelatedPriceProps(newItem);
                });

            return newItem;
        }


        /// <summary>
        /// 更新持仓项的过期状态。和指定的时间比较，如果标的过期时间小于指定时间，则标识持仓过期
        /// </summary>
        /// <param name="positionItem"></param>
        /// <param name="compareTimestamp">指定的比较时间</param>
        private static void InvalidatePositionIsExpired(PositionDiscreteItemDataModel positionItem, long compareTimestamp)
        {
            if (positionItem == null) return;
            var contractExpDate = positionItem.ContractDetailContainer?.ContractDetail?.ContractExpDate;
            if (contractExpDate != null)
            {
                positionItem.IsXqTargetExpired = (compareTimestamp > contractExpDate.Value);
            }
        }

        /// <summary>
        /// 纠正持仓项的价格相关属性
        /// </summary>
        /// <param name="positionItem"></param>
        private static void RectifyPositionRelatedPriceProps(PositionDiscreteItemDataModel positionItem)
        {
            if (positionItem == null) return;

            var tickSize = XueQiaoConstants.XQContractPriceMinimumPirceTick;
            var commodityTickSize = positionItem.ContractDetailContainer?.CommodityDetail?.TickSize;
            if (commodityTickSize != null)
            {
                tickSize = commodityTickSize.Value;
            }

            // 纠正 CalculatePrice
            var calculatePrice = positionItem.CalculatePrice;
            if (calculatePrice != null)
            {
                positionItem.CalculatePrice = MathHelper.MakeValuePrecise(calculatePrice.Value, tickSize); ;
            }
            
            // 纠正 PositionAvgPrice
            var positionAvgPrice = positionItem.PositionAvgPrice;
            if (positionAvgPrice != null)
            {
                positionItem.PositionAvgPrice = MathHelper.MakeValuePrecise(positionAvgPrice.Value, tickSize * XueQiaoConstants.XQMultipleCalculatedPriceTickRate);
            }
        }
        

        private void UpdatePositionItemWithTemplate(PositionDiscreteItemDataModel positionItem,
            PositionDiscreteItemUpdateTemplate updateTemplate)
        {
            if (positionItem == null || updateTemplate == null) return;

            bool needRectifyPriceRelatedProps = false;

            if (updateTemplate.UpdateTimestamp != null) positionItem.UpdateTimestamp = updateTemplate.UpdateTimestamp.Item1;
            if (updateTemplate.PrevPosition != null) positionItem.PrevPosition = updateTemplate.PrevPosition.Item1;
            if (updateTemplate.LongPosition != null) positionItem.LongPosition = updateTemplate.LongPosition.Item1;
            if (updateTemplate.ShortPosition != null) positionItem.ShortPosition = updateTemplate.ShortPosition.Item1;
            if (updateTemplate.NetPosition != null) positionItem.NetPosition = updateTemplate.NetPosition.Item1;
            
            if (updateTemplate.UseCommission != null) positionItem.UseCommission = updateTemplate.UseCommission.Item1;
            if (updateTemplate.CloseProfit != null) positionItem.CloseProfit = updateTemplate.CloseProfit.Item1;

            if (updateTemplate.PositionProfit != null) positionItem.PositionProfit = updateTemplate.PositionProfit.Item1;
            if (updateTemplate.UseMargin != null) positionItem.UseMargin = updateTemplate.UseMargin.Item1;
            if (updateTemplate.FrozenMargin != null) positionItem.FrozenMargin = updateTemplate.FrozenMargin.Item1;
            if (updateTemplate.FrozenCommission != null) positionItem.FrozenCommission = updateTemplate.FrozenCommission.Item1;
            if (updateTemplate.Currency != null) positionItem.Currency = updateTemplate.Currency.Item1;
            if (updateTemplate.GoodsValue != null) positionItem.GoodsValue = updateTemplate.GoodsValue.Item1;
            if (updateTemplate.Leverage != null) positionItem.Leverage = updateTemplate.Leverage.Item1;
            if (updateTemplate.IsXqTargetExpired != null) positionItem.IsXqTargetExpired = updateTemplate.IsXqTargetExpired.Item1;

            // 计算价特殊处理
            if (updateTemplate.CalculatePrice != null)
            {
                positionItem.CalculatePrice = updateTemplate.CalculatePrice.Item1;
                needRectifyPriceRelatedProps = true;
            }

            // 持仓均价特殊处理
            if (updateTemplate.PositionAvgPrice != null)
            {
                positionItem.PositionAvgPrice = updateTemplate.PositionAvgPrice.Item1;
                needRectifyPriceRelatedProps = true;
            }

            if (needRectifyPriceRelatedProps)
            {
                RectifyPositionRelatedPriceProps(positionItem);
            }
        }
        
        private IInterfaceInteractResponse<HostingSledContractPositionPage> QueryAllPositions(long subAccountId,
            LandingInfo landingInfo, CancellationToken clt)
        {
            if (clt.IsCancellationRequested) return null;
            if (landingInfo == null) return null;
            var queryOption = new ReqHostingSledContractPositionOption { SubAccountId = subAccountId };
            var resp = XqThriftLibConfigurationManager.SharedInstance.TradeHostingTerminalAoHttpStub.getHostingSledContractPosition(landingInfo, queryOption);
            return resp;
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
