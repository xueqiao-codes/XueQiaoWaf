using business_foundation_lib.xq_thriftlib_config;
using ContainerShell.Interfaces.Applications;
using ContainerShell.Interfaces.Events;
using ContainerShell.Interfaces.Services;
using lib.xqclient_base.logger;
using lib.xqclient_base.thriftapi_mediation;
using lib.xqclient_base.thriftapi_mediation.Interface;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Schedulers;
using System.Windows.Threading;
using Thrift.Collections;
using Thrift.Protocol;
using xueqiao.mailbox.user.message.thriftapi;
using xueqiao.trade.hosting.proxy;
using xueqiao.trade.hosting.terminal.ao;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;

namespace ContainerShell.Applications.ServiceControllers
{
    /// <summary>
    /// 职责：维护用户消息列表。
    /// 1.定时加载更新的消息，如果存在新消息，则发布一个通知。
    /// 2.提供刷新、获取更新、获取更旧的消息的方法。
    /// </summary>
    [Export(typeof(IUserMessageServiceCtrl)), Export(typeof(IContainerShellModuleServiceCtrl)), PartCreationPolicy(CreationPolicy.Shared)]
    internal class UserMessageServiceCtrl : IUserMessageServiceCtrl, IContainerShellModuleServiceCtrl
    {
        private const int MESSAGE_QUERY_SIZE = 50;

        private readonly IUserMessageService userMessageService;
        private readonly IEventAggregator eventAggregator;
        private readonly ILoginDataService loginDataService;
        private readonly ILoginUserManageService loginUserManageService;
        

        /// <summary>
        /// 消息列表同步任务工厂。该工厂生产的任务是顺序执行，将增删、刷新包装成任务执行，才可保证同步的准确性
        /// </summary>
        private readonly TaskFactory messageListSyncTaskFactory = new TaskFactory(new OrderedTaskScheduler());

        /// <summary>
        /// 消息列表
        /// </summary>
        private readonly Dictionary<long, UserMessage> messageList = new Dictionary<long, UserMessage>();
        private readonly object messageListLock = new object();

        /// <summary>
        /// 消息列表加载的可取消控制
        /// </summary>
        private CancellationTokenSource messageListLoadCTS;
        /// <summary>
        /// 是否正在加载数据
        /// </summary>
        private bool isLoadMessageList;
        private readonly object messageListLoadLock = new object();

        // 加载更新消息的定时器
        private System.Timers.Timer loadNewMessagesTimer;

        [ImportingConstructor]
        public UserMessageServiceCtrl(
            IUserMessageService userMessageService,
            IEventAggregator eventAggregator,
            ILoginDataService loginDataService,
            ILoginUserManageService loginUserManageService)
        {
            this.userMessageService = userMessageService;
            this.eventAggregator = eventAggregator;
            this.loginDataService = loginDataService;
            this.loginUserManageService = loginUserManageService;
            
        }

        public void Initialize()
        {
            loginUserManageService.HasLogined += RecvUserLogined;
            loginUserManageService.HasLogouted += RecvUserHasLogouted;
        }
        
        public void Shutdown()
        {
            loginUserManageService.HasLogined -= RecvUserLogined;
            loginUserManageService.HasLogouted -= RecvUserHasLogouted;

            StopLoadNewMessagesTimer();
            CancelMessageListLoad();
            ClearMessageList();
        }
        
        public Task<IInterfaceInteractResponse<QueryItemsByPageResult<UserMessage>>> LoadNewMessages(long? referenceTimestamp = null)
        {
            return messageListSyncTaskFactory.StartNew(() => 
            {
                var landingInfo = loginDataService.LandingInfo;
                if (landingInfo == null) return null;
                if (SafeGetIsLoadMessageList()) return null;
                var clt = AcquireMessageListLoadCLT();
                var resp = InternalLoadNewMessages(referenceTimestamp, landingInfo, clt,
                    out IEnumerable<UserMessage> _addItems, out IEnumerable<UserMessage> _updateItems);
                return resp;
            });
        }
        
        public Task<IInterfaceInteractResponse<QueryItemsByPageResult<UserMessage>>> LoadOldMessages(long? referenceTimestamp = null)
        {
            return messageListSyncTaskFactory.StartNew(() =>
            {
                if (SafeGetIsLoadMessageList()) return null;
                long? referTS = referenceTimestamp;
                if (referTS == null)
                {
                    lock (messageListLock)
                    {
                        referTS = UnsafeGetCurrentOldestMessage()?.CreateTimestamp;
                    }
                }
                if (referTS == null) referTS = (long)DateHelper.NowUnixTimeSpan().TotalSeconds;

                var queryOption = new xueqiao.trade.hosting.terminal.ao.ReqMailBoxMessageOption { EndTimstamp = referTS.Value };

                var landingInfo = loginDataService.LandingInfo;
                if (landingInfo == null) return null;

                var clt = AcquireMessageListLoadCLT();

                SafeSetIsLoadMessageList(true);

                AppLog.Debug($"Begin load old message list");
                var resp = QueryLimitedUserMessages(landingInfo, queryOption, clt, MESSAGE_QUERY_SIZE);
                AppLog.Debug($"End load old message list. count:{resp.CorrectResult?.Page?.Count()}");

                SafeSetIsLoadMessageList(false);
                lock (messageListLock)
                {
                    if (clt.IsCancellationRequested) return null;
                    UnsafeAddOrUpdateMessages(resp?.CorrectResult?.Page, false, false,
                        out IEnumerable<UserMessage> _addItems, out IEnumerable<UserMessage> _updateItems);
                }
                return resp;
            });
        }

        public Task<IInterfaceInteractResponse<QueryItemsByPageResult<UserMessage>>> RefreshMessageList(bool forceRefresh)
        {
            if (forceRefresh)
            {
                CancelMessageListLoad();
            }
            return messageListSyncTaskFactory.StartNew(() =>
            {
                if (SafeGetIsLoadMessageList()) return null;
                var queryOption = new xueqiao.trade.hosting.terminal.ao.ReqMailBoxMessageOption { };

                var landingInfo = loginDataService.LandingInfo;
                if (landingInfo == null) return null;

                var clt = AcquireMessageListLoadCLT();

                SafeSetIsLoadMessageList(true);

                AppLog.Debug($"Begin refresh message list");
                var resp = QueryLimitedUserMessages(landingInfo, queryOption, clt, MESSAGE_QUERY_SIZE);
                AppLog.Debug($"End refresh message list. count:{resp.CorrectResult?.Page?.Count()}");

                SafeSetIsLoadMessageList(false);
                lock (messageListLock)
                {
                    if (clt.IsCancellationRequested) return null;
                    UnsafeAddOrUpdateMessages(resp?.CorrectResult?.Page, true, false,
                        out IEnumerable<UserMessage> _addItems, out IEnumerable<UserMessage> _updateItems);
                }
                return resp;
            });
        }

        public Task<IInterfaceInteractResponse> MarkMessageAsRead(long messageId)
        {
            return Task.Run(() => 
            {
                UserMessage tarMsg = null;
                lock (messageListLock)
                {
                    messageList.TryGetValue(messageId, out tarMsg);
                }
                if (tarMsg == null) return null;
                var session = loginDataService.ProxyLoginResp?.HostingSession;
                if (session == null) return null;

                var sip = new StubInterfaceInteractParams { TransportConnectTimeoutMS = 2000, TransportReadTimeoutMS = 2000 };
                var resp = XqThriftLibConfigurationManager.SharedInstance.TradeHostingProxyHttpStub.markMessageAsRead(session, new THashSet<long> { messageId }, sip);
                if (resp == null) return null;
                if (resp.CorrectResult == true)
                {
                    tarMsg.State = MessageState.READ;
                }

                return (IInterfaceInteractResponse)resp;
            });
        }

        public UserMessage GetCurrentNewestMessage()
        {
            lock (messageListLock)
            {
                return UnsafeGetCurrentNewestMessage();
            }
        }

        public UserMessage GetCurrentOldestMessage()
        {
            lock (messageListLock)
            {
                return UnsafeGetCurrentOldestMessage();
            }
        }

        private void RecvUserLogined()
        {
            StartLoadNewMessagesTimer();
            RefreshMessageList(false);
        }

        private void RecvUserHasLogouted(ProxyLoginResp lastLoginResp)
        {
            StopLoadNewMessagesTimer();
            CancelMessageListLoad();
            ClearMessageList();
        }

        private UserMessage UnsafeGetCurrentNewestMessage()
        {
            return messageList.Values.OrderByDescending(i => i.CreateTimestamp).FirstOrDefault();
        }

        private UserMessage UnsafeGetCurrentOldestMessage()
        {
            return messageList.Values.OrderBy(i => i.CreateTimestamp).FirstOrDefault();
        }

        private IInterfaceInteractResponse<QueryItemsByPageResult<UserMessage>> InternalLoadNewMessages(
           long? referenceTimestamp,
           LandingInfo landingInfo,
           CancellationToken clt,
           out IEnumerable<UserMessage> _addItems,
           out IEnumerable<UserMessage> _updateItems)
        {
            _addItems = null;
            _updateItems = null;

            if (clt.IsCancellationRequested) return null;

            long? referTS = referenceTimestamp;
            if (referTS == null)
            {
                lock (messageListLock)
                {
                    referTS = UnsafeGetCurrentNewestMessage()?.CreateTimestamp;
                }
            }
            if (referTS == null)
                referTS = (long)DateHelper.NowUnixTimeSpan().TotalSeconds;
            
            var queryOption = new xueqiao.trade.hosting.terminal.ao.ReqMailBoxMessageOption { StartTimestamp = referTS.Value };

            SafeSetIsLoadMessageList(true);

            AppLog.Debug($"Begin load new message list");
            var resp = QueryLimitedUserMessages(landingInfo, queryOption, clt, null);
            AppLog.Debug($"End load new message list. count:{resp.CorrectResult?.Page?.Count()}");

            SafeSetIsLoadMessageList(false);
            lock (messageListLock)
            {
                if (clt.IsCancellationRequested) return null;
                UnsafeAddOrUpdateMessages(resp?.CorrectResult?.Page, false, true,
                    out _addItems, out _updateItems);
            }
            return resp;
        }

        private bool SafeGetIsLoadMessageList()
        {
            bool value;
            lock (messageListLoadLock)
            {
                value = isLoadMessageList;
            }
            return value;
        }

        private void SafeSetIsLoadMessageList(bool value)
        {
            lock (messageListLoadLock)
            {
                isLoadMessageList = value;
            }
        }

        private CancellationToken AcquireMessageListLoadCLT()
        {
            CancellationToken clt = CancellationToken.None;
            lock (messageListLoadLock)
            {
                if (messageListLoadCTS == null)
                {
                    messageListLoadCTS = new CancellationTokenSource();
                }
                clt = messageListLoadCTS.Token;
            }
            return clt;
        }

        private void CancelMessageListLoad()
        {
            lock (messageListLoadLock)
            {
                if (messageListLoadCTS != null)
                {
                    messageListLoadCTS.Cancel();
                    messageListLoadCTS.Dispose();
                    messageListLoadCTS = null;
                }
                isLoadMessageList = false;
            }
        }

        private void ClearMessageList()
        {
            lock (messageListLock)
            {
                messageList.Clear();
                UIItemsAddOrRemoveDispatcherBeginInvoke(() => 
                {
                    userMessageService.MessageItems.Clear();
                });
            }
        }

        /// <summary>
        /// 查询限定数目的消息。如果 limitSize 为 null，则查询条件内的全部
        /// </summary>
        /// <param name="session"></param>
        /// <param name="queryOption"></param>
        /// <param name="clt"></param>
        /// <param name="limitSize"></param>
        /// <returns></returns>
        private IInterfaceInteractResponse<QueryItemsByPageResult<UserMessage>>
            QueryLimitedUserMessages(LandingInfo landingInfo, xueqiao.trade.hosting.terminal.ao.ReqMailBoxMessageOption queryOption, 
            CancellationToken clt, int? limitSize)
        {
            if (clt.IsCancellationRequested) return null;
            if (landingInfo == null) return null;

            int queryPageSize = 0;
            if (limitSize != null)
            {
                queryPageSize = limitSize.Value < MESSAGE_QUERY_SIZE ? limitSize.Value : MESSAGE_QUERY_SIZE;
            } else
            {
                queryPageSize = MESSAGE_QUERY_SIZE;
            }

            IInterfaceInteractResponse<UserMessagePage> lastPageResp = null;
            var queryAllCtrl = new QueryAllItemsByPageHelper<UserMessage>(pageIndex => {
                if (clt.IsCancellationRequested) return null;
                var pageOption = new IndexedPageOption
                {
                    NeedTotalCount = true,
                    PageIndex = pageIndex,
                    PageSize = queryPageSize
                };

                var resp = XqThriftLibConfigurationManager.SharedInstance.TradeHostingTerminalAoHttpStub
                    .queryMailBoxMessage(landingInfo, queryOption, pageOption);
                lastPageResp = resp;

                if (resp == null) return null;

                var pageInfo = resp.CorrectResult;
                var pageResult = new QueryItemsByPageResult<UserMessage>(resp.SourceException != null)
                {
                    TotalCount = pageInfo?.Total,
                    Page = pageInfo?.Page
                };
                return pageResult;
            });

            if (limitSize != null)
            {
                queryAllCtrl.ContinueQueryNextPageFunc = (totalQueriedItems, lastTimeQueridResult) =>
                {
                    if (lastTimeQueridResult == null) return false;
                    if (lastTimeQueridResult?.Page?.Any() != true) return false;

                    if (totalQueriedItems != null)
                    {
                        var idGroupedItems = totalQueriedItems.GroupBy(i => i.MessageId);
                        if (idGroupedItems.Count() >= limitSize) return false;
                    }
                    return true;
                };
            }
            else
            {
                queryAllCtrl.ContinueQueryNextPageFunc = (totalQueriedItems, lastTimeQueridResult) =>
                {
                    if (lastTimeQueridResult == null) return false;
                    if (lastTimeQueridResult?.Page?.Any() != true) return false;
                    return true;
                };
            }
            
            queryAllCtrl.RemoveDuplicateFunc = _items =>
            {
                if (_items == null) return null;
                var idGroupedItems = _items.GroupBy(i => i.MessageId);
                return idGroupedItems.Select(i => i.LastOrDefault()).ToArray();
            };

            var queriedItems = queryAllCtrl.QueryAllItems();
            if (lastPageResp == null) return null;
            if (clt.IsCancellationRequested) return null;

            QueryItemsByPageResult<UserMessage> tarCorrectResult = null;
            if (lastPageResp.SourceException == null)
            {
                tarCorrectResult = new QueryItemsByPageResult<UserMessage>(false)
                {
                    TotalCount = lastPageResp.CorrectResult?.Total,
                    Page = queriedItems
                };
            }

            var tarResp = new InterfaceInteractResponse<QueryItemsByPageResult<UserMessage>>(lastPageResp.Servant,
                lastPageResp.InterfaceName,
                lastPageResp.SourceException,
                lastPageResp.HasTransportException,
                lastPageResp.HttpResponseStatusCode,
                tarCorrectResult)
            {
                CustomParsedExceptionResult = lastPageResp.CustomParsedExceptionResult,
                InteractInformation = lastPageResp.InteractInformation
            };
            return tarResp;
        }
        
        private void UnsafeAddOrUpdateMessages(
            IEnumerable<UserMessage> messages,
            bool updateIfItemExist,
            bool addItemsInsertOnBefore,
            out IEnumerable<UserMessage> _addedItems, out IEnumerable<UserMessage> _updatedItems)
        {
            _addedItems = null;
            _updatedItems = null;

            if (messages?.Any() != true) return;

            var addList = new List<UserMessage>();
            var updateList = new List<UserMessage>();
            foreach (var item in messages)
            {
                var msgId = item.MessageId;
                if (messageList.TryGetValue(msgId, out UserMessage existItem))
                {
                    // 存在，则改变内容
                    if (updateIfItemExist)
                    {
                        TBase refExistItem = existItem;
                        ThriftHelper.UnserializeBytesToTBase(ref refExistItem, ThriftHelper.SerializeTBaseToBytes(item));
                    }
                    updateList.Add(existItem);
                }
                else
                {
                    // 不存在，则添加
                    messageList[msgId] = item;
                    addList.Add(item);
                }
            }

            if (addList.Any())
            {
                UIItemsAddOrRemoveDispatcherBeginInvoke(() =>
                {
                    if (addItemsInsertOnBefore)
                    {
                        foreach (var item in addList.ToArray().Reverse())
                        {
                            userMessageService.MessageItems.Insert(0, item);
                        }
                    }
                    else
                    {
                        userMessageService.MessageItems.AddRange(addList);
                    }
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

        private void StartLoadNewMessagesTimer()
        {
            StopLoadNewMessagesTimer();

            // 6 minutes interval
            loadNewMessagesTimer = new System.Timers.Timer(1000 * 60 * 6);
            loadNewMessagesTimer.Elapsed += LoadNewMessagesTimer_Elapsed;
            loadNewMessagesTimer.Start();
        }

        private void StopLoadNewMessagesTimer()
        {
            if (loadNewMessagesTimer != null)
            {
                loadNewMessagesTimer.Elapsed -= LoadNewMessagesTimer_Elapsed;
                loadNewMessagesTimer.Stop();
                loadNewMessagesTimer.Dispose();
                loadNewMessagesTimer = null;
            }
        }

        private void LoadNewMessagesTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            messageListSyncTaskFactory.StartNew(() => 
            {
                var landingInfo = loginDataService.LandingInfo;
                if (landingInfo == null) return;

                var clt = AcquireMessageListLoadCLT();
                InternalLoadNewMessages(null, landingInfo, clt, 
                    out IEnumerable<UserMessage> _addedItems, out IEnumerable<UserMessage> _updateItems);
                if (_addedItems?.Any() == true)
                {
                    // 发布通知
                    eventAggregator.GetEvent<FindNewUserMessagesEvent>().Publish(new FindNewUserMessagesEventPayload { NewMessages = _addedItems.ToArray() });
                }
            });
        }
    }
}
