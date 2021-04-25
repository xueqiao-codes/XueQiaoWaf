using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;
using System.Threading;
using NativeModel.Contract;
using XueQiaoFoundation.Interfaces.Applications;
using xueqiao.contract.standard;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Helper.WeakDelegate;
using lib.xqclient_base.thriftapi_mediation.Interface;
using lib.xqclient_base.thriftapi_mediation;
using lib.xqclient_base.logger;
using business_foundation_lib.xq_thriftlib_config;

namespace XueQiaoFoundation.Applications.Controllers
{
    [Export(typeof(IExchangeQueryController)), Export(typeof(IXueQiaoFoundationSingletonController)), PartCreationPolicy(CreationPolicy.Shared)]
    internal class ExchangeQueryController : IExchangeQueryController, IXueQiaoFoundationSingletonController
    {
        
        private readonly ILoginDataService loginDataService;
        private readonly IExchangeCacheController exchangeCacheController;

        private readonly IDIncreaser queryAllExchangesReqIdIncreaser = new IDIncreaser();
        private readonly object queryAllExchangesLock = new object();
        private Task<IInterfaceInteractResponse<IEnumerable<NativeExchange>>> queryAllExchangesTask;
        private CancellationTokenSource queryAllExchangesCts;
        private readonly Dictionary<long, ActionDelegateReference<IInterfaceInteractResponse<IEnumerable<NativeExchange>>>> allExchangesQueriedCallbackHandlers;

        [ImportingConstructor]
        public ExchangeQueryController( 
            ILoginDataService loginDataService,
            IExchangeCacheController exchangeCacheController)
        {
            
            this.loginDataService = loginDataService;
            this.exchangeCacheController = exchangeCacheController;

            allExchangesQueriedCallbackHandlers = new Dictionary<long, ActionDelegateReference<IInterfaceInteractResponse<IEnumerable<NativeExchange>>>>();
        }

        public void Shutdown()
        {
            lock (queryAllExchangesLock)
            {
                if (queryAllExchangesCts != null)
                {
                    queryAllExchangesCts.Cancel();
                    queryAllExchangesCts.Dispose();
                    queryAllExchangesCts = null;
                }
                allExchangesQueriedCallbackHandlers.Clear();
            }
        }

        public long QueryAllExchanges(ActionDelegateReference<IInterfaceInteractResponse<IEnumerable<NativeExchange>>> handler)
        {
            lock (queryAllExchangesLock)
            {
                long reqId = queryAllExchangesReqIdIncreaser.RequestIncreasedId();
                if (handler != null)
                {
                    allExchangesQueriedCallbackHandlers.Add(reqId, handler);
                }

                if (queryAllExchangesTask != null
                    && queryAllExchangesTask.IsCanceled == false
                    && queryAllExchangesTask.IsCompleted == false
                    && queryAllExchangesTask.IsFaulted == false)
                {
                    return reqId;
                }

                if (queryAllExchangesCts != null)
                {
                    queryAllExchangesCts.Cancel();
                    queryAllExchangesCts.Dispose();
                    queryAllExchangesCts = null;
                }
                queryAllExchangesCts = new CancellationTokenSource();
                var cancelToken = queryAllExchangesCts.Token;
                queryAllExchangesTask = Task.Run(() => QueryAllExchanges(cancelToken, true), cancelToken);
                queryAllExchangesTask.ContinueWith(task =>
                {
                    if (cancelToken.IsCancellationRequested) return;
                    CallbackForAllExchangesQueriedResponse(task.Result);
                }, cancelToken);

                return reqId;
            }
        }

        public void RemoveQueryAllExchangesHandler(long reqId)
        {
            lock (queryAllExchangesLock)
            {
                allExchangesQueriedCallbackHandlers.Remove(reqId);
            }
        }

        public IInterfaceInteractResponse<IEnumerable<NativeExchange>> QueryAllExchanges(CancellationToken cancelToken)
        {
            var resp = QueryAllExchanges(cancelToken, true);
            if (cancelToken.IsCancellationRequested) return null;
            CallbackForAllExchangesQueriedResponse(resp);
            return resp;
        }

        public IInterfaceInteractResponse<NativeExchange> QueryExchange(string exchangeMic)
        {
            if (string.IsNullOrEmpty(exchangeMic)) throw new ArgumentException("exchangeMic can't be empty.");
            var serviceStub = XqThriftLibConfigurationManager.SharedInstance.ContractOnlineServiceHttpStub;
            var option = new ReqSledExchangeOption
            {
                ExchangeMic = exchangeMic
            };
            var resp = serviceStub.reqSledExchange(option, 0, 1);
            if (resp == null) return null;
            var tarResp = new InterfaceInteractResponse<NativeExchange>(resp.Servant,
                    resp.InterfaceName,
                    resp.SourceException,
                    resp.HasTransportException,
                    resp.HttpResponseStatusCode,
                    resp.CorrectResult?.Page.FirstOrDefault()?.ToNativeExchange())
            {
                InteractInformation = resp.InteractInformation,
                CustomParsedExceptionResult = resp.CustomParsedExceptionResult
            };

            // Cache exchange
            if (tarResp.CorrectResult != null)
            {
                CacheExchangeItems(new NativeExchange[] { tarResp.CorrectResult });
            }

            return tarResp;
        }

        private void CallbackForAllExchangesQueriedResponse(IInterfaceInteractResponse<IEnumerable<NativeExchange>> resp)
        {
            if (resp == null) return;
            lock (queryAllExchangesLock)
            {
                AppLog.Info($"已查询所有交易所. exchanges.count:{(resp.CorrectResult?.Count() ?? 0)}");
                var callbackHandlers = allExchangesQueriedCallbackHandlers.Values.ToArray();
                allExchangesQueriedCallbackHandlers.Clear();

                foreach (var item in callbackHandlers)
                {
                    item.Target?.Invoke(resp);
                }
            }
        }

        private void CacheExchangeItems(IEnumerable<NativeExchange> exchanges)
        {
            if (exchanges == null) return;
            foreach (var item in exchanges)
            {
                exchangeCacheController.Cache(item.ExchangeMic, item);
            }
        }
        
        private IInterfaceInteractResponse<IEnumerable<NativeExchange>> QueryAllExchanges(CancellationToken ct, bool cacheQueriedItems)
        {
            var queryPageSize = 50;
            IInterfaceInteractResponse<SledExchangePage> firstPageResp = null;
            var queryAllCtrl = new QueryAllItemsByPageHelper<SledExchange>(pageIndex => {
                if (ct.IsCancellationRequested) return null;

                var option = new ReqSledExchangeOption();
                var resp = XqThriftLibConfigurationManager.SharedInstance.ContractOnlineServiceHttpStub.reqSledExchange(option, pageIndex, queryPageSize);
                if (resp == null) return null;
                if (pageIndex == 0)
                {
                    firstPageResp = resp;
                }
                var pageInfo = resp.CorrectResult;
                var pageResult = new QueryItemsByPageResult<SledExchange>(resp.SourceException != null)
                {
                    TotalCount = pageInfo?.Total,
                    Page = pageInfo?.Page?.ToArray()
                };
                return pageResult;
            });
            
            queryAllCtrl.RemoveDuplicateFunc = _items =>
            {
                if (_items == null) return null;
                var idGroupedItems = _items.GroupBy(i => i.ExchangeMic);
                return idGroupedItems.Select(i => i.LastOrDefault()).ToArray();
            };

            var queriedItems = queryAllCtrl.QueryAllItems();
            if (firstPageResp == null) return null;

            var tarResp = new InterfaceInteractResponse<IEnumerable<NativeExchange>>(firstPageResp.Servant,
                firstPageResp.InterfaceName,
                firstPageResp.SourceException,
                firstPageResp.HasTransportException,
                firstPageResp.HttpResponseStatusCode,
                queriedItems?.Select(i => i.ToNativeExchange()).ToArray())
            {
                CustomParsedExceptionResult = firstPageResp.CustomParsedExceptionResult,
                InteractInformation = firstPageResp.InteractInformation
            };

            if (cacheQueriedItems)
            {
                // cache commodities
                CacheExchangeItems(tarResp.CorrectResult);
            }

            return tarResp;
        }
    }
 }
