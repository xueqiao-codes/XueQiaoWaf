﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NativeModel.Trade;
using XueQiaoFoundation.Interfaces.Applications;
using XueQiaoFoundation.Shared.Helper.WeakDelegate;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;
using System.Collections.Concurrent;
using XueQiaoFoundation.Shared.Helper;
using System.Threading;
using IDLAutoGenerated.Util;
using Thrift.Collections;
using xueqiao.trade.hosting;
using lib.xqclient_base.thriftapi_mediation.Interface;
using lib.xqclient_base.thriftapi_mediation;
using business_foundation_lib.xq_thriftlib_config;

namespace XueQiaoFoundation.Applications.Controllers
{
    [Export(typeof(IComposeGraphQueryController)), Export(typeof(IXueQiaoFoundationSingletonController)), PartCreationPolicy(CreationPolicy.Shared)]
    internal class ComposeGraphQueryController : IComposeGraphQueryController, IXueQiaoFoundationSingletonController
    {
        
        private readonly ILoginDataService loginDataService;
        private readonly IComposeGraphCacheController composeGraphCacheController;

        private readonly ConcurrentDictionary<long, ComposeGraphItemQueryControl> queryComposeGraphByIdControlDictionary;

        [ImportingConstructor]
        public ComposeGraphQueryController(
            ILoginDataService loginDataService,
            IComposeGraphCacheController composeGraphCacheController)
        {
            
            this.loginDataService = loginDataService;
            this.composeGraphCacheController = composeGraphCacheController;

            queryComposeGraphByIdControlDictionary = new ConcurrentDictionary<long, ComposeGraphItemQueryControl>();
        }

        public void Shutdown()
        {
            var queryGraphByIdControls = queryComposeGraphByIdControlDictionary.Values.ToArray();
            queryComposeGraphByIdControlDictionary.Clear();
            foreach (var i in queryGraphByIdControls)
            {
                i.ComposeGraphQueried -= QueryComposeGraphByIdControl_Queried;
                i.Dispose();
            }
        }

        public long QueryComposeGraph(long composeId, ActionDelegateReference<IInterfaceInteractResponse<NativeComposeGraph>> handler)
        {
            return CreateQueryComposeGraphByIdControl(composeId).QueryComposeGraph(handler);
        }
        
        public void RemoveQueryComposeGraphHandler(long composeId, long reqId)
        {
            if (queryComposeGraphByIdControlDictionary.TryGetValue(composeId, out ComposeGraphItemQueryControl queryItem))
            {
                queryItem.RemoveQueryCallbackHandler(reqId);
            }
        }

        public IInterfaceInteractResponse<NativeComposeGraph> QueryComposeGraph(long composeId)
        {
            return CreateQueryComposeGraphByIdControl(composeId).QueryComposeGraph(CancellationToken.None);
        }

        private ComposeGraphItemQueryControl CreateQueryComposeGraphByIdControl(long composeId)
        {
            var optItem = queryComposeGraphByIdControlDictionary.AddOrUpdate(composeId,
                    (k) =>
                    {
                        var newItem = new ComposeGraphItemQueryControl(composeId,
                            loginDataService,
                            composeGraphCacheController);
                        newItem.ComposeGraphQueried += QueryComposeGraphByIdControl_Queried;
                        return newItem;
                    },
                    (k, oldValue) =>
                    {
                        return oldValue;
                    });
            return optItem;
        }

        private void QueryComposeGraphByIdControl_Queried(ComposeGraphItemQueryControl arg1, NativeComposeGraph arg2)
        {
            if (arg1.ComposeId is long composeId)
            {
                if (queryComposeGraphByIdControlDictionary.TryRemove(composeId,
                    out ComposeGraphItemQueryControl _queryControl))
                {
                    _queryControl.ComposeGraphQueried -= QueryComposeGraphByIdControl_Queried;
                    _queryControl.Dispose();
                }
            }
        }

        /// <summary>
        /// 某个组合图查询的控制
        /// </summary>
        class ComposeGraphItemQueryControl : IDisposable
        {
            
            private readonly ILoginDataService loginDataService;
            private readonly IComposeGraphCacheController composeGraphCacheController;

            private readonly IDIncreaser queryReqIdIncreaser = new IDIncreaser();
            private readonly Dictionary<long, ActionDelegateReference<IInterfaceInteractResponse<NativeComposeGraph>>> queriedCallbackHandlers;
            private readonly List<ActionDelegateReference<ComposeGraphItemQueryControl, NativeComposeGraph>> queriedEventHandlers;
            private readonly object queryLock = new object();
            private Task<IInterfaceInteractResponse<NativeComposeGraph>> queryTask;
            private CancellationTokenSource queryCts;
            private bool? isDisposed;

            public ComposeGraphItemQueryControl(long composeId,
                
                ILoginDataService loginDataService,
                IComposeGraphCacheController composeGraphCacheController)
            {
                this.ComposeId = composeId;
                
                this.loginDataService = loginDataService;
                this.composeGraphCacheController = composeGraphCacheController;

                queriedCallbackHandlers = new Dictionary<long, ActionDelegateReference<IInterfaceInteractResponse<NativeComposeGraph>>>();
                queriedEventHandlers = new List<ActionDelegateReference<ComposeGraphItemQueryControl, NativeComposeGraph>>();
            }

            public long ComposeId { get; private set; }

            public event Action<ComposeGraphItemQueryControl, NativeComposeGraph> ComposeGraphQueried
            {
                add
                {
                    lock (queryLock)
                    {
                        // 弱引用
                        queriedEventHandlers.Add(new ActionDelegateReference<ComposeGraphItemQueryControl, NativeComposeGraph>(value, false));
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

            public long QueryComposeGraph(ActionDelegateReference<IInterfaceInteractResponse<NativeComposeGraph>> handler)
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
                    queryTask = Task.Run(() => QueryComposeGraphSync(queryCancelToken, true), queryCancelToken);
                    queryTask.ContinueWith(task =>
                    {
                        if (queryCancelToken.IsCancellationRequested) return;
                        CallbackAndPublishEventForComposeGraphQueriedResponse(task.Result);
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

            public IInterfaceInteractResponse<NativeComposeGraph> QueryComposeGraph(CancellationToken cancelToken)
            {
                var resp = QueryComposeGraphSync(cancelToken, true);
                if (cancelToken.IsCancellationRequested) return null;

                CallbackAndPublishEventForComposeGraphQueriedResponse(resp);
                return resp;
            }

            private void CallbackAndPublishEventForComposeGraphQueriedResponse(IInterfaceInteractResponse<NativeComposeGraph> resp)
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

            private IInterfaceInteractResponse<NativeComposeGraph> QueryComposeGraphSync(CancellationToken cancelToken, bool cacheQueriedItem)
            {
                if (cancelToken.IsCancellationRequested) return null;
                var landingInfo = loginDataService.ProxyLoginResp?.HostingSession?.HostingSession2LandingInfo();
                if (landingInfo == null) return null;

                var resp = XqThriftLibConfigurationManager.SharedInstance.TradeHostingTerminalAoHttpStub.getComposeGraphs(landingInfo, new THashSet<long> { ComposeId });
                if (cancelToken.IsCancellationRequested) return null;
                if (resp == null) return null;

                HostingComposeGraph queriedCompose = null;
                resp.CorrectResult?.TryGetValue(ComposeId, out queriedCompose);

                var tarResp = new InterfaceInteractResponse<NativeComposeGraph>(resp.Servant,
                    resp.InterfaceName,
                    resp.SourceException,
                    resp.HasTransportException,
                    resp.HttpResponseStatusCode,
                    queriedCompose?.ToNativeComposeGraph())
                {
                    CustomParsedExceptionResult = resp.CustomParsedExceptionResult,
                    InteractInformation = resp.InteractInformation
                };

                if (cacheQueriedItem && tarResp.CorrectResult != null)
                {
                    if (tarResp?.CorrectResult != null)
                    {
                        composeGraphCacheController.Cache(ComposeId, tarResp?.CorrectResult);
                    }
                }
                return tarResp;
            }
        }
    }
}
