using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.Composition;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using xueqiao.trade.hosting.terminal.ao;
using System.Threading;
using NativeModel.Trade;
using XueQiaoFoundation.Interfaces.Applications;
using XueQiaoFoundation.Shared.Helper.WeakDelegate;
using XueQiaoFoundation.Shared.Helper;
using lib.xqclient_base.thriftapi_mediation.Interface;
using lib.xqclient_base.thriftapi_mediation;
using Thrift.Collections;
using business_foundation_lib.xq_thriftlib_config;

namespace XueQiaoFoundation.Applications.Controllers
{
    [Export(typeof(IUserComposeViewQueryController)), Export(typeof(IXueQiaoFoundationSingletonController)), PartCreationPolicy(CreationPolicy.Shared)]
    internal class UserComposeViewQueryController : IUserComposeViewQueryController, IXueQiaoFoundationSingletonController
    {
        
        private readonly ILoginDataService loginDataService;
        private readonly IUserComposeViewCacheController userComposeViewCacheController;

        private readonly ConcurrentDictionary<long, UserCurrentComposeViewItemsQueryControl> queryCurrentViewByIdCtrls
            = new ConcurrentDictionary<long, UserCurrentComposeViewItemsQueryControl>();
        private readonly UserCurrentComposeViewItemsQueryControl queryCurrentComposeViewListCtrl;

        private readonly ConcurrentDictionary<long, UserHistoryComposeViewItemQueryControl> queryHistoryViewByIdCtrls
            = new ConcurrentDictionary<long, UserHistoryComposeViewItemQueryControl>();

        [ImportingConstructor]
        public UserComposeViewQueryController(
            ILoginDataService loginDataService,
            IUserComposeViewCacheController userComposeViewCacheController)
        {
            
            this.loginDataService = loginDataService;
            this.userComposeViewCacheController = userComposeViewCacheController;

            queryCurrentComposeViewListCtrl = new UserCurrentComposeViewItemsQueryControl(null, loginDataService, userComposeViewCacheController);
        }

        public void Shutdown()
        {
            queryCurrentComposeViewListCtrl.Dispose();

            var _currentQueryCtrls = queryCurrentViewByIdCtrls.Values;
            queryCurrentViewByIdCtrls.Clear();
            foreach (var i in _currentQueryCtrls)
            {
                i.ComposeItemsQueried -= QueryCurrentComposeView_Queried;
                i.Dispose();
            }

            var _histroyQueryCtrls = queryHistoryViewByIdCtrls.Values;
            queryHistoryViewByIdCtrls.Clear();
            foreach (var i in _histroyQueryCtrls)
            {
                i.ComposeItemsQueried -= QueryHistoryComposeView_Queried;
                i.Dispose();
            }
        }

        public long QueryCurrentComposeViewList(ActionDelegateReference<IInterfaceInteractResponse<IEnumerable<NativeComposeViewDetail>>> handler)
        {
            return queryCurrentComposeViewListCtrl.QueryComposeViewItems(handler);
        }

        public void RemoveQueryCurrentComposeViewListHandler(long reqId)
        {
            queryCurrentComposeViewListCtrl.RemoveQueryCallbackHandler(reqId);
        }

        public IInterfaceInteractResponse<IEnumerable<NativeComposeViewDetail>> QueryCurrentComposeViewList(CancellationToken ct)
        {
            return queryCurrentComposeViewListCtrl.QueryComposeViewItems(ct);
        }

        public long QueryCurrentComposeView(long composeId, ActionDelegateReference<IInterfaceInteractResponse<NativeComposeViewDetail>> handler)
        {
            var optItem = CreateQueryCurrentViewByIdCtrl(composeId);
            var wrapHandler = new Action<IInterfaceInteractResponse<IEnumerable<NativeComposeViewDetail>>>(_resp =>
            {
                handler?.Target?.Invoke(GetTargetComposeQueridResponse(composeId, _resp));
            });
            return optItem.QueryComposeViewItems(new ActionDelegateReference<IInterfaceInteractResponse<IEnumerable<NativeComposeViewDetail>>>(wrapHandler, true));
        }

        public void RemoveQueryCurrentComposeViewHandler(long composeId, long reqId)
        {
            if (queryCurrentViewByIdCtrls.TryGetValue(composeId, out UserCurrentComposeViewItemsQueryControl queryItem))
            {
                queryItem.RemoveQueryCallbackHandler(reqId);
            }
        }

        public IInterfaceInteractResponse<NativeComposeViewDetail> QueryCurrentComposeView(long composeId)
        {
            var resp = CreateQueryCurrentViewByIdCtrl(composeId).QueryComposeViewItems(CancellationToken.None);
            return GetTargetComposeQueridResponse(composeId, resp);
        }

        public long QueryHistoryComposeView(long composeId, ActionDelegateReference<IInterfaceInteractResponse<NativeComposeViewDetail>> handler)
        {
            var optItem = CreateQueryHistroyViewByIdCtrl(composeId);
            var wrapHandler = new Action<IInterfaceInteractResponse<IEnumerable<NativeComposeViewDetail>>>(_resp =>
            {
                handler?.Target?.Invoke(GetTargetComposeQueridResponse(composeId, _resp));
            });
            return optItem.QueryComposeViewItems(new ActionDelegateReference<IInterfaceInteractResponse<IEnumerable<NativeComposeViewDetail>>>(wrapHandler, true));
        }

        public void RemoveQueryHistoryComposeViewHandler(long composeId, long reqId)
        {
            if (queryHistoryViewByIdCtrls.TryGetValue(composeId, out UserHistoryComposeViewItemQueryControl queryItem))
            {
                queryItem.RemoveQueryCallbackHandler(reqId);
            }
        }

        public IInterfaceInteractResponse<NativeComposeViewDetail> QueryHistoryComposeView(long composeId)
        {
            var resp = CreateQueryHistroyViewByIdCtrl(composeId).QueryComposeViewItems(CancellationToken.None);
            return GetTargetComposeQueridResponse(composeId, resp);
        }

        private IInterfaceInteractResponse<NativeComposeViewDetail> GetTargetComposeQueridResponse(long tarComposeId, IInterfaceInteractResponse<IEnumerable<NativeComposeViewDetail>> sourceItemsResp)
        {
            IInterfaceInteractResponse<NativeComposeViewDetail> newResp = null;
            if (sourceItemsResp != null)
            {
                newResp = new InterfaceInteractResponse<NativeComposeViewDetail>(sourceItemsResp.Servant,
                    sourceItemsResp.InterfaceName,
                    sourceItemsResp.SourceException,
                    sourceItemsResp.HasTransportException,
                    sourceItemsResp.HttpResponseStatusCode,
                    sourceItemsResp.CorrectResult?.FirstOrDefault(i => i.UserComposeView?.ComposeGraphId == tarComposeId))
                {
                    CustomParsedExceptionResult = sourceItemsResp.CustomParsedExceptionResult,
                    InteractInformation = sourceItemsResp.InteractInformation
                };
            }
            return newResp;
        }

        private UserCurrentComposeViewItemsQueryControl CreateQueryCurrentViewByIdCtrl(long composeId)
        {
            var optItem = queryCurrentViewByIdCtrls.AddOrUpdate(composeId,
                    (k) =>
                    {
                        var newItem = new UserCurrentComposeViewItemsQueryControl(composeId,
                            loginDataService,
                            userComposeViewCacheController);
                        newItem.ComposeItemsQueried += QueryCurrentComposeView_Queried;
                        return newItem;
                    },
                    (k, oldValue) =>
                    {
                        return oldValue;
                    });
            return optItem;
        }

        private void QueryCurrentComposeView_Queried(ComposeViewQueryControlBase arg1, IEnumerable<NativeComposeViewDetail> arg2)
        {
            var ctrl = arg1 as UserCurrentComposeViewItemsQueryControl;
            if (ctrl == null) return;

            if (ctrl.ComposeId is long composeId)
            {
                if (queryCurrentViewByIdCtrls.TryRemove(composeId,
                    out UserCurrentComposeViewItemsQueryControl _queryControl))
                {
                    _queryControl.ComposeItemsQueried -= QueryCurrentComposeView_Queried;
                    _queryControl.Dispose();
                }
            }
        }

        private UserHistoryComposeViewItemQueryControl CreateQueryHistroyViewByIdCtrl(long composeId)
        {
            var optItem = queryHistoryViewByIdCtrls.AddOrUpdate(composeId,
                    (k) =>
                    {
                        var newItem = new UserHistoryComposeViewItemQueryControl(composeId,
                            loginDataService,
                            userComposeViewCacheController);
                        newItem.ComposeItemsQueried += QueryHistoryComposeView_Queried;
                        return newItem;
                    },
                    (k, oldValue) =>
                    {
                        return oldValue;
                    });
            return optItem;
        }

        private void QueryHistoryComposeView_Queried(ComposeViewQueryControlBase arg1, IEnumerable<NativeComposeViewDetail> arg2)
        {
            var ctrl = arg1 as UserHistoryComposeViewItemQueryControl;
            if (ctrl == null) return;

            if (queryHistoryViewByIdCtrls.TryRemove(ctrl.ComposeId,
                    out UserHistoryComposeViewItemQueryControl _queryControl))
            {
                _queryControl.ComposeItemsQueried -= QueryHistoryComposeView_Queried;
                _queryControl.Dispose();
            }
        }

        /// <summary>
        /// 查询用户组合视图控制基类。查询某个组合视图，设置 ComposeId；查询全部组合视图，ComposeId 设置为null
        /// </summary>
        abstract class ComposeViewQueryControlBase
        {
            private readonly IUserComposeViewCacheController userComposeViewCacheController;
            
            private readonly IDIncreaser queryReqIdIncreaser = new IDIncreaser();
            private readonly Dictionary<long, ActionDelegateReference<IInterfaceInteractResponse<IEnumerable<NativeComposeViewDetail>>>> queriedCallbackHandlers;
            private readonly List<ActionDelegateReference<ComposeViewQueryControlBase, IEnumerable<NativeComposeViewDetail>>> queriedEventHandlers;
            private readonly object queryLock = new object();
            private Task<IInterfaceInteractResponse<IEnumerable<NativeComposeViewDetail>>> queryTask;
            private CancellationTokenSource queryCts;
            private bool? isDisposed;

            public ComposeViewQueryControlBase(IUserComposeViewCacheController userComposeViewCacheController)
            {
                this.userComposeViewCacheController = userComposeViewCacheController;

                queriedCallbackHandlers = new Dictionary<long, ActionDelegateReference<IInterfaceInteractResponse<IEnumerable<NativeComposeViewDetail>>>>();
                queriedEventHandlers = new List<ActionDelegateReference<ComposeViewQueryControlBase, IEnumerable<NativeComposeViewDetail>>>();
            }
            
            public event Action<ComposeViewQueryControlBase, IEnumerable<NativeComposeViewDetail>> ComposeItemsQueried
            {
                add
                {
                    lock (queryLock)
                    {
                        // 弱引用
                        queriedEventHandlers.Add(new ActionDelegateReference<ComposeViewQueryControlBase, IEnumerable<NativeComposeViewDetail>>(value, false));
                    }
                }
                remove
                {
                    lock (queryLock)
                    {
                        queriedEventHandlers.RemoveAll(i => i.Target == value);
                    }
                }
            }

            public long QueryComposeViewItems(ActionDelegateReference<IInterfaceInteractResponse<IEnumerable<NativeComposeViewDetail>>> handler)
            {
                lock (queryLock)
                {
                    long reqId = 0;
                    if (handler != null)
                    {
                        reqId = queryReqIdIncreaser.RequestIncreasedId();
                        queriedCallbackHandlers.Add(reqId, handler);
                    }

                    if (queryTask != null
                        && queryTask.IsCanceled == false
                        && queryTask.IsCompleted == false
                        && queryTask.IsFaulted == false)
                    {
                        return reqId;
                    }

                    if (queryCts != null)
                    {
                        queryCts.Cancel();
                        queryCts.Dispose();
                        queryCts = null;
                    }
                    queryCts = new CancellationTokenSource();
                    var queryCancelToken = queryCts.Token;
                    queryTask = Task.Run(() => QueryComposeViewItemsSync(queryCancelToken), queryCancelToken);
                    queryTask.ContinueWith(task =>
                    {
                        if (queryCancelToken.IsCancellationRequested) return;

                        var resp = task.Result;
                        CacheComposeViewItems(resp?.CorrectResult);
                        CallbackAndPublishEventForQueriedResponse(resp);

                    }, queryCancelToken);
                    return reqId;
                }
            }

            public void RemoveQueryCallbackHandler(long reqId)
            {
                lock (queryLock)
                {
                    queriedCallbackHandlers.Remove(reqId);
                }
            }

            public void Dispose()
            {
                if (isDisposed == true) return;
                isDisposed = true;
                if (queryCts != null)
                {
                    queryCts.Cancel();
                    queryCts.Dispose();
                    queryCts = null;
                }
            }

            public IInterfaceInteractResponse<IEnumerable<NativeComposeViewDetail>> QueryComposeViewItems(CancellationToken cancelToken)
            {
                var resp = QueryComposeViewItemsSync(cancelToken);
                if (cancelToken.IsCancellationRequested) return null;

                CacheComposeViewItems(resp?.CorrectResult);
                CallbackAndPublishEventForQueriedResponse(resp);

                return resp;
            }
            
            /// <summary>
            /// 同步查询组合视图项
            /// </summary>
            /// <param name="cancelToken"></param>
            /// <param name="cacheQueriedItems"></param>
            /// <returns></returns>
            protected abstract IInterfaceInteractResponse<IEnumerable<NativeComposeViewDetail>>
                QueryComposeViewItemsSync(CancellationToken cancelToken);


            private void CallbackAndPublishEventForQueriedResponse(IInterfaceInteractResponse<IEnumerable<NativeComposeViewDetail>> resp)
            {
                lock (queryLock)
                {
                    var callbackHandlers = queriedCallbackHandlers.Values.ToArray();
                    var eventHandlers = queriedEventHandlers.ToArray();
                    queriedCallbackHandlers.Clear();

                    foreach (var callbackHandlerItem in callbackHandlers)
                    {
                        callbackHandlerItem.Target?.Invoke(resp);
                    }

                    if (resp?.CorrectResult != null)
                    {
                        foreach (var eventHandlerItem in eventHandlers)
                        {
                            eventHandlerItem.Target?.Invoke(this, resp?.CorrectResult);
                        }
                    }
                }
            }
            
            private void CacheComposeViewItems(IEnumerable<NativeComposeViewDetail> composeViewItems)
            {
                if (composeViewItems?.Any() != true) return;
                foreach (var composeView in composeViewItems)
                {
                    if (composeView.UserComposeView != null)
                    {
                        userComposeViewCacheController.Cache(new UserComposeViewCacheKey(composeView.UserComposeView.ComposeGraphId), composeView);
                    }
                }
            }
        }


        /// <summary>
        /// 用户当前(未删除)组合视图查询的控制。查询某个组合视图，设置 ComposeId；查询全部组合视图，ComposeId 设置为null
        /// </summary>
        class UserCurrentComposeViewItemsQueryControl : ComposeViewQueryControlBase
        {
            
            private readonly ILoginDataService loginDataService;
            
            public UserCurrentComposeViewItemsQueryControl(
                long? composeId,
                
                ILoginDataService loginDataService, 
                IUserComposeViewCacheController userComposeViewCacheCtrl) 
                : base(userComposeViewCacheCtrl)
            {
                this.ComposeId = composeId;
                
                this.loginDataService = loginDataService;
            }

            public long? ComposeId { get; private set; }

            protected override IInterfaceInteractResponse<IEnumerable<NativeComposeViewDetail>>
                QueryComposeViewItemsSync(CancellationToken cancelToken)
            {
                if (cancelToken.IsCancellationRequested) return null;
                var landingInfo = loginDataService.LandingInfo;
                if (landingInfo == null) return null;

                var queryPageSize = 50;
                IInterfaceInteractResponse<QueryHostingComposeViewDetailPage> firstPageResp = null;
                var queryOption = new QueryHostingComposeViewDetailOption { SubUserId = landingInfo.SubUserId };
                if (this.ComposeId.HasValue)
                {
                    queryOption.ComposeGraphId = this.ComposeId.Value;
                }
                var queryAllCtrl = new QueryAllItemsByPageHelper<HostingComposeViewDetail>(pageIndex => {
                    if (cancelToken.IsCancellationRequested) return null;
                    var pageOption = new IndexedPageOption
                    {
                        NeedTotalCount = true,
                        PageIndex = pageIndex,
                        PageSize = queryPageSize
                    };

                    var resp = XqThriftLibConfigurationManager.SharedInstance.TradeHostingTerminalAoHttpStub
                    .getComposeViewDetailPage(landingInfo, queryOption, pageOption);
                    if (resp == null) return null;
                    if (pageIndex == 0)
                    {
                        firstPageResp = resp;
                    }
                    var pageInfo = resp.CorrectResult;
                    var pageResult = new QueryItemsByPageResult<HostingComposeViewDetail>(resp.SourceException != null)
                    {
                        TotalCount = pageInfo?.TotalCount,
                        Page = pageInfo?.ResultList?.ToArray()
                    };
                    return pageResult;
                });

                queryAllCtrl.RemoveDuplicateFunc = _items =>
                {
                    if (_items == null) return null;
                    var idGroupedItems = _items.GroupBy(i => i?.ViewDetail?.ComposeGraphId);
                    return idGroupedItems.Select(i => i.LastOrDefault()).ToArray();
                };

                var queriedItems = queryAllCtrl.QueryAllItems();
                if (firstPageResp == null) return null;
                if (cancelToken.IsCancellationRequested) return null;

                var tarResp = new InterfaceInteractResponse<IEnumerable<NativeComposeViewDetail>>(firstPageResp.Servant,
                    firstPageResp.InterfaceName,
                    firstPageResp.SourceException,
                    firstPageResp.HasTransportException,
                    firstPageResp.HttpResponseStatusCode,
                    queriedItems?.Select(i => i.ToNativeComposeViewDetail()).ToArray())
                {
                    CustomParsedExceptionResult = firstPageResp.CustomParsedExceptionResult,
                    InteractInformation = firstPageResp.InteractInformation
                };

                return tarResp;
            }
        }


        /// <summary>
        /// 用户历史（包含当前和逻辑删除）组合视图查询的控制
        /// </summary>
        class UserHistoryComposeViewItemQueryControl : ComposeViewQueryControlBase
        {
            
            private readonly ILoginDataService loginDataService;

            public UserHistoryComposeViewItemQueryControl(
                long composeId,
                
                ILoginDataService loginDataService,
                IUserComposeViewCacheController userComposeViewCacheCtrl)
                : base(userComposeViewCacheCtrl)
            {
                this.ComposeId = composeId;
                
                this.loginDataService = loginDataService;
            }

            public long ComposeId { get; private set; }

            protected override IInterfaceInteractResponse<IEnumerable<NativeComposeViewDetail>>
               QueryComposeViewItemsSync(CancellationToken cancelToken)
            {
                if (cancelToken.IsCancellationRequested) return null;
                var landingInfo = loginDataService.LandingInfo;
                if (landingInfo == null) return null;

                var idSet = new THashSet<long> { ComposeId };
                var resp = XqThriftLibConfigurationManager.SharedInstance.TradeHostingTerminalAoHttpStub
                        .getComposeViewDetails(landingInfo, idSet);

                if (resp == null) return null;
                var tarResp = new InterfaceInteractResponse<IEnumerable<NativeComposeViewDetail>>(resp.Servant,
                    resp.InterfaceName,
                    resp.SourceException,
                    resp.HasTransportException,
                    resp.HttpResponseStatusCode,
                    resp?.CorrectResult?.Values?.Select(i => i.ToNativeComposeViewDetail()))
                {
                    CustomParsedExceptionResult = resp.CustomParsedExceptionResult,
                    InteractInformation = resp.InteractInformation
                };

                return tarResp;
            }
        }
    }
}
