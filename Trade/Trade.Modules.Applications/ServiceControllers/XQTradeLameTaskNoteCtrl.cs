using business_foundation_lib.xq_thriftlib_config;
using lib.xqclient_base.logger;
using lib.xqclient_base.thriftapi_mediation;
using lib.xqclient_base.thriftapi_mediation.Interface;
using Prism.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Schedulers;
using System.Windows.Threading;
using Thrift.Collections;
using Thrift.Protocol;
using xueqiao.trade.hosting.arbitrage.thriftapi;
using xueqiao.trade.hosting.events;
using xueqiao.trade.hosting.proxy;
using xueqiao.trade.hosting.push.protocol;
using xueqiao.trade.hosting.tasknote.thriftapi;
using xueqiao.trade.hosting.terminal.ao;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.Interfaces.Applications;
using XueQiaoFoundation.Interfaces.Event.business;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Model;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;
using XueQiaoWaf.Trade.Interfaces.Applications;
using XueQiaoWaf.Trade.Interfaces.Events;
using XueQiaoWaf.Trade.Interfaces.Services;
using XueQiaoWaf.Trade.Modules.Applications.Helper;
using XueQiaoWaf.Trade.Modules.Applications.ServiceControllers.Events;

namespace XueQiaoWaf.Trade.Modules.Applications.ServiceControllers
{
    /// <summary>
    /// 雪橇瘸腿成交 Task Note 管理协议的实现。
    ///  
    /// 1.同步雪橇瘸腿成交 TaskNote 列表；
    /// 2.处理雪橇瘸腿成交 TaskNote 相关的 push event；
    /// 3.提供雪橇瘸腿成交本地 TaskNote 的获取方法；
    /// 4.发布雪橇瘸腿成交的本地 event。
    ///
    /// </summary>
    [Export(typeof(IXQTradeLameTaskNoteCtrl)), Export(typeof(ITradeModuleSingletonController)),
        PartCreationPolicy(CreationPolicy.Shared)]
    internal class XQTradeLameTaskNoteCtrl : IXQTradeLameTaskNoteCtrl, ITradeModuleSingletonController
    {
        private class XQTradeLameTNUpdateTemplate
        {
            public Tuple<TradeItemDataModel> TradeInfo;
            public Tuple<long> CreateTimestampMs;
        }

        private delegate XQTradeLameTNUpdateTemplate XQTradeLameTNUpdateTemplateFactory(XQTradeItemKey itemKey, XQTradeLameTaskNote existItem);


        private readonly IXQTradeLameTaskNoteService lameTaskNoteService;
        private readonly IEventAggregator eventAggregator;
        private readonly ILoginDataService loginDataService;
        private readonly ILoginUserManageService loginUserManageService;
        
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
        /// 瘸腿成交 Task Note 同步任务工厂。该工厂生产的任务是顺序执行，将增删、刷新包装成任务执行，才可保证同步的准确性
        /// </summary>
        private readonly TaskFactory lameTaskNoteSyncTaskFactory = new TaskFactory(new OrderedTaskScheduler());
        
        /// <summary>
        /// 瘸腿成交 Task Note 项列表
        /// </summary>
        private readonly Dictionary<XQTradeItemKey, XQTradeLameTaskNote> lameTaskNotes = new Dictionary<XQTradeItemKey, XQTradeLameTaskNote>();
        
        // 瘸腿成交 Task Note 项列表 lock
        private readonly object lameTaskNotesLock = new object();


        private CancellationTokenSource subAccountLameTaskNotesRefreshCTS;
        private readonly Dictionary<long, SubAccountAnyDataRefreshStateHolder> subAccountLameTaskNotesRefreshStateHolders
            = new Dictionary<long, SubAccountAnyDataRefreshStateHolder>();
        private readonly object subAccountLameTaskNotesRefreshLock = new object();

        private readonly IDIncreaser clientDataForceSyncTimesIncreaser = new IDIncreaser(0);

        [ImportingConstructor]
        public XQTradeLameTaskNoteCtrl(
            IXQTradeLameTaskNoteService lameTaskNoteService,
            IEventAggregator eventAggregator,
            ILoginDataService loginDataService,
            ILoginUserManageService loginUserManageService,
            
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
            this.lameTaskNoteService = lameTaskNoteService;
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

            loginUserManageService.HasLogouted += RecvUserHasLogouted;
            eventAggregator.GetEvent<ClientDataForceSync>().Subscribe(ReceivedClientDataForceSyncEvent);
            eventAggregator.GetEvent<XQTaskNoteCreated>().Subscribe(ReceivedXQTaskNoteCreatedEvent);
            eventAggregator.GetEvent<XQTaskNoteDeleted>().Subscribe(ReceivedXQTaskNoteDeletedEvent);
            eventAggregator.GetEvent<UserRelatedSubAccountItemsRefreshEvent>().Subscribe(RecvUserRelatedSubAccountItemsRefreshEvent);
        }

        public void Shutdown()
        {
            clientDataForceSyncTimesIncreaser?.Reset();
            CancelSubAccountLameTaskNotesRefresh(true);

            loginUserManageService.HasLogouted -= RecvUserHasLogouted;
            eventAggregator.GetEvent<ClientDataForceSync>().Unsubscribe(ReceivedClientDataForceSyncEvent);
            eventAggregator.GetEvent<XQTaskNoteCreated>().Unsubscribe(ReceivedXQTaskNoteCreatedEvent);
            eventAggregator.GetEvent<XQTaskNoteDeleted>().Unsubscribe(ReceivedXQTaskNoteDeletedEvent);
            eventAggregator.GetEvent<UserRelatedSubAccountItemsRefreshEvent>().Unsubscribe(RecvUserRelatedSubAccountItemsRefreshEvent);
        }

        public void RefreshTaskNotesIfNeed(long subAccountId)
        {
            RefreshSubAccountLameTaskNotes(new long[] { subAccountId }, false);
        }

        public void RefreshTaskNotesForce(long subAccountId)
        {
            RefreshSubAccountLameTaskNotes(new long[] { subAccountId }, true);
        }
        
        public XQTradeLameTaskNote GetTaskNote(XQTradeItemKey itemKey)
        {
            XQTradeLameTaskNote tn = null;
            lock (lameTaskNotesLock)
            {
                lameTaskNotes.TryGetValue(itemKey, out tn);
            }
            return tn;
        }

        public void RequestDeleteTaskNote(XQTradeItemKey itemKey)
        {
            if (itemKey == null) return;
            var landingInfo = loginDataService.LandingInfo;
            if (landingInfo == null) return;
            
            var siip = new StubInterfaceInteractParams { TransportConnectTimeoutMS = 2000, TransportReadTimeoutMS = 2000 };
            XqThriftLibConfigurationManager.SharedInstance.TradeHostingTerminalAoHttpStub.batchDeleteXQTradeLameTaskNotesAsync(landingInfo,
                itemKey.SubAccountId, new THashSet<long> { itemKey.XQTradeId}, CancellationToken.None, siip)
                .ContinueWith(t =>
                {
                    // 不需要手动删除本地记录，会通过 push 推送删除事件，然后再删除

                    var resp = t.Result;
                    if (resp == null)
                    {
                        AppLog.Error($"Failed to RequestDeleteTaskNote(itemKey:{itemKey}).");
                    }
                    else if (resp?.SourceException != null)
                    {
                        AppLog.Error($"Failed to RequestDeleteTaskNote(itemKey:{itemKey}).", resp?.SourceException);
                    }
                    else
                    {
                        var errResults = resp.CorrectResult?.Where(i=>i.Value.ErrorCode != 0).ToArray();
                        if (errResults != null)
                        {
                            foreach (var errResult in errResults)
                            {
                                AppLog.Error($"Failed to RequestDeleteTaskNote. key:{errResult.Key}, errInfo:{errResult.Value}");
                            }
                        }
                    }
                });
        }
        
        private void RecvUserHasLogouted(ProxyLoginResp lastLoginResp)
        {
            clientDataForceSyncTimesIncreaser?.Reset();
            CancelSubAccountLameTaskNotesRefresh(true);
            RemoveSubAccountTaskNotes(null);
        }

        private void ReceivedClientDataForceSyncEvent(ClientForceSyncEvent msg)
        {
            if (loginDataService.ProxyLoginResp == null) return;
            var forceSyncTimes = clientDataForceSyncTimesIncreaser.RequestIncreasedId();
            
            // 同步持仓列表
            var userRelatedSubAccountIds = relatedSubAccountItemsCtrl.RelatedSubAccountItems?.Select(i => i.SubAccountId).Distinct();
            if (userRelatedSubAccountIds?.Any() != true) return;

            CancelSubAccountLameTaskNotesRefresh(false);
            RefreshSubAccountLameTaskNotes(userRelatedSubAccountIds, forceSyncTimes != 1);
        }

        private void RecvUserRelatedSubAccountItemsRefreshEvent(UserRelatedSubAccountItemsRefreshEventArgs args)
        {
            var currentLoginToken = loginDataService?.ProxyLoginResp?.HostingSession?.Token;
            if (currentLoginToken == null || currentLoginToken != args.LoginUserToken) return;

            var relatedSubAccountIds = args.RelatedSubAccountItems?.Select(i => i.SubAccountId).Distinct();
            if (relatedSubAccountIds?.Any() != true) return;

            RefreshSubAccountLameTaskNotes(relatedSubAccountIds, false);
        }

        private void ReceivedXQTaskNoteCreatedEvent(TaskNoteCreatedEvent payload)
        {
            var createdNote = payload?.CreatedTaskNote;
            if (!ValidateXQTradeLameTaskNote(createdNote.NoteType))
                return;

            lameTaskNoteSyncTaskFactory.StartNew(() => 
            {
                lock (lameTaskNotesLock)
                {
                    if (loginDataService.ProxyLoginResp == null) return;
                    UnsafeAddOrUpdateTaskNotes(new HostingTaskNote[] { createdNote },
                        out IEnumerable<XQTradeLameTaskNote> _addItems, out IEnumerable<XQTradeLameTaskNote> _updateItems);
                    if (_addItems?.Any() == true)
                    {
                        // 发布本地创建事件
                        var addItem = _addItems.First();
                        var newEventPayload = new TradeLameTaskNoteNativeEventPayload(TradeLameTaskNoteNativeEventType.Create) { TaskNote = addItem };
                        eventAggregator.GetEvent<TradeLameTaskNoteNativeEvent>().Publish(newEventPayload);
                    }
                }
            });
        }
        
        private void ReceivedXQTaskNoteDeletedEvent(TaskNoteDeletedEvent payload)
        {
            if (payload == null) return;
            if (!ValidateXQTradeLameTaskNote(payload.NoteType) 
                || !ParseXQTradeLameTaskNoteKey(payload.NoteKey, out long subAccountId, out long XQTradeId))
                return;
            
            lameTaskNoteSyncTaskFactory.StartNew(() => 
            {
                if (loginDataService.ProxyLoginResp == null) return;

                var itemKey = new XQTradeItemKey(subAccountId, XQTradeId);
                if (RemoveTaskNote(itemKey))
                {
                    // 发布本地删除事件
                    var newEventPayload = new TradeLameTaskNoteNativeEventPayload(TradeLameTaskNoteNativeEventType.Delete) { ItemKey = itemKey };
                    eventAggregator.GetEvent<TradeLameTaskNoteNativeEvent>().Publish(newEventPayload);
                }
            });
        }

        private bool RemoveTaskNote(XQTradeItemKey itemKey)
        {
            lock (lameTaskNotesLock)
            {
                return UnsafeRemoveTaskNote(itemKey);
            }
        }

        private bool UnsafeRemoveTaskNote(XQTradeItemKey itemKey)
        {
            lameTaskNotes.TryGetValue(itemKey, out XQTradeLameTaskNote existItem);
            if (existItem != null)
            {
                lameTaskNotes.Remove(itemKey);
                UIItemsAddOrRemoveDispatcherBeginInvoke(() => 
                {
                    lameTaskNoteService.TaskNoteItems.Remove(existItem);
                });
                return true;
            }
            return false;
        }
        
        private XQTradeLameTaskNote GenerateXQTradeLameTaskNote(HostingTaskNote hostingTN)
        {
            var subAccountId = hostingTN.NoteKey.Key1;
            var xqTradeId = hostingTN.NoteKey.Key2;
            var newItem = new XQTradeLameTaskNote(subAccountId, xqTradeId)
            {
                TradeInfo = GenerateTradeInfo(hostingTN),
                CreateTimestampMs = hostingTN.CreateTimestampMs
            };
            return newItem;
        }

        private TradeItemDataModel GenerateTradeInfo(HostingTaskNote hostingTN)
        {
            var subAccountId = hostingTN.NoteKey.Key1;
            var xqTradeId = hostingTN.NoteKey.Key2;

            TBase refXQTrade = new HostingXQTrade();
            ThriftHelper.UnserializeThriftJson(ref refXQTrade, hostingTN.NoteContent);
            var tradeInfo = TradeItemDataModelCreateHelper.CreateTradeItem((HostingXQTrade)refXQTrade,
                subAccountRelatedItemQueryCtrl, subAccountRelatedItemCacheCtrl,
                hostingUserQueryCtrl, hostingUserCacheCtrl,
                composeGraphCacheCtrl, composeGraphQueryCtrl,
                userComposeViewCacheCtrl, userComposeViewQueryCtrl, contractItemTreeQueryCtrl);
            return tradeInfo;
        }

        private static bool IsRefreshingOrSuccessRefreshed(DataRefreshState refreshState)
        {
            return refreshState == DataRefreshState.Refreshing || refreshState == DataRefreshState.SuccessRefreshed;
        }

        private SubAccountAnyDataRefreshStateHolder AcquireSubAccountLameTaskNotesRefreshStateHolder(long subAccountId)
        {
            lock (subAccountLameTaskNotesRefreshLock)
            {
                SubAccountAnyDataRefreshStateHolder dataHolder = null;
                if (!subAccountLameTaskNotesRefreshStateHolders.TryGetValue(subAccountId, out dataHolder))
                {
                    dataHolder = new SubAccountAnyDataRefreshStateHolder(subAccountId);
                    subAccountLameTaskNotesRefreshStateHolders.Add(subAccountId, dataHolder);
                }
                return dataHolder;
            }
        }

        private static bool ValidateXQTradeLameTaskNote(HostingTaskNoteType taskNoteType)
        {
            return taskNoteType == HostingTaskNoteType.XQ_TRADE_LAME;
        }

        private static bool ParseXQTradeLameTaskNoteKey(HostingTaskNoteKey taskNoteKey, out long _subAccountId, out long _XQTradeId)
        {
            _subAccountId = 0;
            _XQTradeId = 0;
            if (taskNoteKey == null) return false;

            _subAccountId = taskNoteKey.Key1;
            _XQTradeId = taskNoteKey.Key2;
            return true;
        }

        /// <summary>
        /// 刷新操作账号的瘸腿 Task Notes 列表
        /// </summary>
        /// <param name="subAccountIds">需要刷新的操作账号 id 列表</param>
        /// <param name="isForceRefresh">是否强制刷新。YES 表示强制刷新， NO 表示必要时才刷新</param>
        private void RefreshSubAccountLameTaskNotes(IEnumerable<long> subAccountIds, bool isForceRefresh)
        {
            if (subAccountIds?.Any() != true) return;

            lameTaskNoteSyncTaskFactory.StartNew(() =>
            {
                var landingInfo = loginDataService.LandingInfo;
                if (landingInfo == null) return;

                var queryTsks = new List<Task>();
                var queryResps = new ConcurrentDictionary<long, IInterfaceInteractResponse<IEnumerable<HostingTaskNote>>>();
                var refreshStateHolders = new List<SubAccountAnyDataRefreshStateHolder>();

                var clt = AcquireSubAccountLameTaskNotesRefreshCLT();
                // 查询所有 subaccount 的数据
                foreach (var subAccountId in subAccountIds)
                {
                    var stateHolder = AcquireSubAccountLameTaskNotesRefreshStateHolder(subAccountId);
                    
                    bool needRefresh = true;
                    if (!isForceRefresh && IsRefreshingOrSuccessRefreshed(stateHolder.DataRefreshState))
                    {
                        needRefresh = false;
                    }

                    if (needRefresh && !clt.IsCancellationRequested)
                    {
                        refreshStateHolders.Add(stateHolder);
                        stateHolder.DataRefreshState = DataRefreshState.Refreshing;
                        var tsk = Task.Run(() =>
                        {
                            clt.ThrowIfCancellationRequested();

                            var resp = QueryXQTradeLameTaskNotes(subAccountId, landingInfo, clt);
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
                    }
                    return;
                }
                
                lock (lameTaskNotesLock)
                {
                    if (loginDataService.ProxyLoginResp == null) return;

                    foreach (var stateHolder in refreshStateHolders)
                    {
                        IInterfaceInteractResponse<IEnumerable<HostingTaskNote>> resp = null;
                        queryResps.TryGetValue(stateHolder.SubAccountId, out resp);
                        stateHolder.DataRefreshState = (resp?.CorrectResult != null) ? DataRefreshState.SuccessRefreshed : DataRefreshState.FailedRefreshed;
                        
                        if (clt.IsCancellationRequested) continue;

                        if (resp != null)
                        {
                            var queriedList = resp.CorrectResult ?? new HostingTaskNote[] { };

                            // 清除 SubAccountId 的所有 Lame task notes
                            UnSafeRemoveSubAccountTaskNotes(stateHolder.SubAccountId);

                            // 添加查询到的 Lame task notes
                            UnsafeAddOrUpdateTaskNotes(queriedList,
                                    out IEnumerable<XQTradeLameTaskNote> _addList,
                                    out IEnumerable<XQTradeLameTaskNote> _updateList);
                        }
                    }
                }
            });
        }

        private CancellationToken AcquireSubAccountLameTaskNotesRefreshCLT()
        {
            CancellationToken clt = CancellationToken.None;
            lock (subAccountLameTaskNotesRefreshLock)
            {
                if (subAccountLameTaskNotesRefreshCTS == null)
                {
                    subAccountLameTaskNotesRefreshCTS = new CancellationTokenSource();
                }
                clt = subAccountLameTaskNotesRefreshCTS.Token;
            }
            return clt;
        }

        private void CancelSubAccountLameTaskNotesRefresh(bool clearRefreshStateHolders)
        {
            lock (subAccountLameTaskNotesRefreshLock)
            {
                if (subAccountLameTaskNotesRefreshCTS != null)
                {
                    subAccountLameTaskNotesRefreshCTS.Cancel();
                    subAccountLameTaskNotesRefreshCTS.Dispose();
                    subAccountLameTaskNotesRefreshCTS = null;
                }

                if (clearRefreshStateHolders)
                {
                    subAccountLameTaskNotesRefreshStateHolders.Clear();
                }
            }
        }

        private void UnsafeAddOrUpdateTaskNotes(
            IEnumerable<XQTradeItemKey> newItemKeys,
            Func<XQTradeItemKey, XQTradeLameTaskNote> newItemFactory,
            XQTradeLameTNUpdateTemplateFactory updateTemplateFactory,
            out IEnumerable<XQTradeLameTaskNote> _addedItems,
            out IEnumerable<XQTradeLameTaskNote> _updatedItems)
        {
            _addedItems = null;
            _updatedItems = null;

            Debug.Assert(newItemFactory != null);

            if (newItemKeys?.Any() != true) return;

            var addList = new List<XQTradeLameTaskNote>();
            var updateList = new List<XQTradeLameTaskNote>();
            foreach (var itemKey in newItemKeys)
            {
                XQTradeLameTaskNote implItem = null;
                XQTradeLameTaskNote existItem = null;
                lameTaskNotes.TryGetValue(itemKey, out existItem);
                if (existItem == null)
                {
                    var newItem = newItemFactory.Invoke(itemKey);
                    if (newItem != null)
                    {
                        lameTaskNotes[itemKey] = newItem;
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
                    UpdateXQTradeLameTNWithTemplate(implItem, temp);
                }
            }

            if (addList.Any())
            {
                UIItemsAddOrRemoveDispatcherBeginInvoke(() =>
                {
                    lameTaskNoteService.TaskNoteItems.AddRange(addList);
                });
            }

            _addedItems = addList;
            _updatedItems = updateList;
        }

        private void UnsafeAddOrUpdateTaskNotes(
            IEnumerable<HostingTaskNote> taskNotes,
            out IEnumerable<XQTradeLameTaskNote> _addedItems,
            out IEnumerable<XQTradeLameTaskNote> _updatedItems)
        {
            _addedItems = null;
            _updatedItems = null;

            if (taskNotes?.Any() != true) return;

            var itemMap = new Dictionary<XQTradeItemKey, HostingTaskNote>();
            foreach (var tn in taskNotes)
            {
                if (ValidateXQTradeLameTaskNote(tn.NoteType)
                    && ParseXQTradeLameTaskNoteKey(tn.NoteKey, out long subAccountId, out long XQTradeId))
                {
                    itemMap[new XQTradeItemKey(subAccountId, XQTradeId)] = tn;
                }
            }
            
            UnsafeAddOrUpdateTaskNotes(itemMap.Keys,
                _key =>
                {
                    var newItem = GenerateXQTradeLameTaskNote(itemMap[_key]);
                    RectifyLameTNRelatedPriceProps(newItem);
                    return newItem;
                },
                (_key, _existItem) =>
                {
                    if (_existItem == null) return null;
                    var hostingTN = itemMap[_key];
                    return new XQTradeLameTNUpdateTemplate
                    {
                        TradeInfo = new Tuple<TradeItemDataModel>(GenerateTradeInfo(hostingTN)),
                        CreateTimestampMs = new Tuple<long>(hostingTN.CreateTimestampMs)
                    };
                },
                out _addedItems,
                out _updatedItems);
        }
        
        private void UpdateXQTradeLameTNWithTemplate(XQTradeLameTaskNote lameTN, XQTradeLameTNUpdateTemplate updateTemplate)
        {
            if (lameTN == null || updateTemplate == null) return;

            bool needRectifyPriceRelatedProps = false;

            if (updateTemplate.TradeInfo != null)
            {
                lameTN.TradeInfo = updateTemplate.TradeInfo.Item1;
                needRectifyPriceRelatedProps = true;
            }

            if (needRectifyPriceRelatedProps)
            {
                RectifyLameTNRelatedPriceProps(lameTN);
            }
        }

        /// <summary>
        /// 纠正瘸腿 Task Note 项的价格相关属性
        /// </summary>
        /// <param name="positionItem"></param>
        private static void RectifyLameTNRelatedPriceProps(XQTradeLameTaskNote positionItem)
        {
            var tradeInfo = positionItem.TradeInfo;
            if (tradeInfo != null)
            {
                TradeItemDataModelCreateHelper.RectifyTradeItemRelatedPriceProps(tradeInfo);
            }
        }
        
        /// <summary>
        /// 删除某个操作账户的所有 Lame task notes。如果 subAccountId 为 null，则删除所有
        /// </summary>
        /// <param name="subAccountId"></param>
        private void RemoveSubAccountTaskNotes(long? subAccountId)
        {
            lock (lameTaskNotesLock)
            {
                UnSafeRemoveSubAccountTaskNotes(subAccountId);
            }
        }

        /// <summary>
        /// 非线程安全地删除某个操作账户的所有 Lame task notes。如果 subAccountId 为 null，则删除所有
        /// </summary>
        /// <param name="subAccountId"></param>
        private void UnSafeRemoveSubAccountTaskNotes(long? subAccountId)
        {
            KeyValuePair<XQTradeItemKey, XQTradeLameTaskNote>[] rmItems = null;
            if (subAccountId == null)
                rmItems = lameTaskNotes.ToArray();
            else
                rmItems = lameTaskNotes.Where(i => i.Key.SubAccountId == subAccountId).ToArray();

            foreach (var rmItem in rmItems)
            {
                lameTaskNotes.Remove(rmItem.Key);
            }
            UIItemsAddOrRemoveDispatcherBeginInvoke(() =>
            {
                foreach (var rmItem in rmItems)
                {
                    lameTaskNoteService.TaskNoteItems.Remove(rmItem.Value);
                }
            });
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

        private IInterfaceInteractResponse<IEnumerable<HostingTaskNote>> 
            QueryXQTradeLameTaskNotes(long subAccountId, LandingInfo landingInfo, CancellationToken clt)
        {
            if (clt.IsCancellationRequested) return null;
            if (landingInfo == null) return null;

            const int queryPageSize = 50;
            IInterfaceInteractResponse<HostingTaskNotePage> firstPageResp = null;
            var queryOption = new QueryXQTradeLameTaskNotePageOption { SubAccountIds = new THashSet<long> { subAccountId } };

            var queryAllCtrl = new QueryAllItemsByPageHelper<HostingTaskNote>(pageIndex => {
                if (clt.IsCancellationRequested) return null;
                var pageOption = new IndexedPageOption
                {
                    NeedTotalCount = true,
                    PageIndex = pageIndex,
                    PageSize = queryPageSize
                };

                var resp = XqThriftLibConfigurationManager.SharedInstance.TradeHostingTerminalAoHttpStub
                    .getXQTradeLameTaskNotePage(landingInfo, queryOption, pageOption);
                if (resp == null) return null;
                if (pageIndex == 0)
                {
                    firstPageResp = resp;
                }
                var pageInfo = resp.CorrectResult;
                var pageResult = new QueryItemsByPageResult<HostingTaskNote>(resp.SourceException != null)
                {
                    TotalCount = pageInfo?.TotalNum,
                    Page = pageInfo?.ResultList
                };
                return pageResult;
            });

            queryAllCtrl.RemoveDuplicateFunc = _items =>
            {
                if (_items == null) return null;
                var idGroupedItems = _items.GroupBy(i => new Tuple<long,long>(i.NoteKey.Key1, i.NoteKey.Key2));
                return idGroupedItems.Select(i => i.LastOrDefault()).ToArray();
            };

            var queriedItems = queryAllCtrl.QueryAllItems();
            if (firstPageResp == null) return null;
            if (clt.IsCancellationRequested) return null;

            var tarResp = new InterfaceInteractResponse<IEnumerable<HostingTaskNote>>(firstPageResp.Servant,
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
    }
}
