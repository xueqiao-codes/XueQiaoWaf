﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.Composition;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;
using System.Threading;
using System.Threading.Tasks;
using NativeModel.Contract;
using XueQiaoFoundation.Interfaces.Applications;
using xueqiao.contract.standard;
using IDLAutoGenerated.Util;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Helper.WeakDelegate;
using lib.xqclient_base.thriftapi_mediation.Interface;
using lib.xqclient_base.thriftapi_mediation;
using lib.xqclient_base.logger;
using business_foundation_lib.xq_thriftlib_config;

namespace XueQiaoFoundation.Applications.Controllers
{
    [Export(typeof(ICommodityQueryController)), Export(typeof(IXueQiaoFoundationSingletonController)), PartCreationPolicy(CreationPolicy.Shared)]
    internal class CommodityQueryController : ICommodityQueryController, IXueQiaoFoundationSingletonController
    {
        
        private readonly ILoginDataService loginDataService;
        private readonly ICommodityCacheController commodityCacheController;

        private readonly IDIncreaser queryAllCommoditiesReqIdIncreaser = new IDIncreaser();
        private readonly object queryAllCommoditiesLock = new object();
        private readonly Dictionary<long, ActionDelegateReference<IInterfaceInteractResponse<IEnumerable<NativeCommodity>>>> allCommoditiesQueriedCallbackHandlers;
        private CancellationTokenSource queryAllCommoditiesCts;
        private Task<IInterfaceInteractResponse<IEnumerable<NativeCommodity>>> queryAllCommoditiesTask;

        [ImportingConstructor]
        public CommodityQueryController(
            ILoginDataService loginDataService,
            ICommodityCacheController commodityCacheController)
        {
            
            this.loginDataService = loginDataService;
            this.commodityCacheController = commodityCacheController;

            allCommoditiesQueriedCallbackHandlers = new Dictionary<long, ActionDelegateReference<IInterfaceInteractResponse<IEnumerable<NativeCommodity>>>>();
        }

        public void Shutdown()
        {
            lock (queryAllCommoditiesLock)
            {
                if (queryAllCommoditiesCts != null)
                {
                    queryAllCommoditiesCts.Cancel();
                    queryAllCommoditiesCts.Dispose();
                    queryAllCommoditiesCts = null;
                }

                allCommoditiesQueriedCallbackHandlers.Clear();
            }
        }
        
        public long QueryAllCommodities(ActionDelegateReference<IInterfaceInteractResponse<IEnumerable<NativeCommodity>>> handler)
        {
            lock (queryAllCommoditiesLock)
            {
                long reqId = 0;
                if (handler != null)
                {
                    reqId = queryAllCommoditiesReqIdIncreaser.RequestIncreasedId();
                    allCommoditiesQueriedCallbackHandlers.Add(reqId, handler);
                }

                if (queryAllCommoditiesTask != null
                    && queryAllCommoditiesTask?.IsCompleted == false
                    && queryAllCommoditiesTask?.IsCanceled == false
                    && queryAllCommoditiesTask?.IsFaulted == false)
                {
                    return reqId;
                }

                if (queryAllCommoditiesCts != null)
                {
                    queryAllCommoditiesCts.Cancel();
                    queryAllCommoditiesCts.Dispose();
                    queryAllCommoditiesCts = null;
                }
                queryAllCommoditiesCts = new CancellationTokenSource();
                var cancelToken = queryAllCommoditiesCts.Token;
                queryAllCommoditiesTask = Task.Run(() => QueryAllCommoditiesSync(cancelToken, true),
                    cancelToken);
                queryAllCommoditiesTask.ContinueWith(task =>
                {
                    if (cancelToken.IsCancellationRequested) return;
                    CallbackForAllCommoditiesQueriedResponse(task.Result);
                });

                return reqId;
            }
        }

        public void RemoveQueryAllCommoditiesHandler(long reqId)
        {
            lock (queryAllCommoditiesLock)
            {
                allCommoditiesQueriedCallbackHandlers.Remove(reqId);
            }
        }

        public IInterfaceInteractResponse<IEnumerable<NativeCommodity>> QueryAllCommodities(CancellationToken cancelToken)
        {
            var resp = QueryAllCommoditiesSync(cancelToken, true);
            if (cancelToken.IsCancellationRequested) return null;

            CallbackForAllCommoditiesQueriedResponse(resp);
            return resp;
        }

        public IInterfaceInteractResponse<NativeCommodity> QueryCommodity(int commodityId)
        {
            var serviceStub = XqThriftLibConfigurationManager.SharedInstance.ContractOnlineServiceHttpStub;
            var option = new ReqSledCommodityOption
            {
                PlatformEnv = loginDataService.ProxyLoginResp?.HostingRunningMode.Contract_HostingRunningMode2TechPlatformEnv() ?? TechPlatformEnv.REAL,
                SledCommodityIdList = new List<int> { commodityId }
            };
            // first time
            var resp = serviceStub.reqSledCommodity(option, 0, 1);
            var tarResp = GenerateCommodityFirstItemQueryResponse(resp, resp?.CorrectResult?.Page);

            if (tarResp?.CorrectResult != null)
            {
                // cache commodities
                CacheCommodityItems(new NativeCommodity[] { tarResp.CorrectResult });
            }

            return tarResp;
        }

        public IInterfaceInteractResponse<NativeCommodity> QueryCommodity(Tuple<string/*exchangeMic*/, int/*commodityType*/, string/*commodityCode*/> commoditySymbol)
        {
            if (commoditySymbol == null) return null;
            var serviceStub = XqThriftLibConfigurationManager.SharedInstance.ContractOnlineServiceHttpStub;
            var option = new ReqSledCommodityOption
            {
                PlatformEnv = loginDataService.ProxyLoginResp?.HostingRunningMode.Contract_HostingRunningMode2TechPlatformEnv() ?? TechPlatformEnv.REAL,
                ExchangeMic = commoditySymbol.Item1,
                SledCommodityType = (SledCommodityType)commoditySymbol.Item2,
                SledCommodityCode = commoditySymbol.Item3
            };
            // first time
            var resp = serviceStub.reqSledCommodity(option, 0, 1);
            var tarResp = GenerateCommodityFirstItemQueryResponse(resp, resp?.CorrectResult.Page);

            if (tarResp?.CorrectResult != null)
            {
                // cache commodities
                CacheCommodityItems(new NativeCommodity[] { tarResp.CorrectResult });
            }

            return tarResp;
        }

        private void CallbackForAllCommoditiesQueriedResponse(IInterfaceInteractResponse<IEnumerable<NativeCommodity>> resp)
        {
            if (resp == null) return;
            lock (queryAllCommoditiesLock)
            {
                AppLog.Info($"查询到所有商品. commodities.count:{(resp.CorrectResult?.Count() ?? 0)}");
                var callbackHandlers = allCommoditiesQueriedCallbackHandlers.Values.ToArray();
                allCommoditiesQueriedCallbackHandlers.Clear();

                foreach (var item in callbackHandlers)
                {
                    item.Target?.Invoke(resp);
                }
            }
        }

        private IInterfaceInteractResponse<IEnumerable<NativeCommodity>> QueryAllCommoditiesSync(CancellationToken ct, bool cacheQueriedItems)
        {
            var queryPageSize = 50;
            IInterfaceInteractResponse<SledCommodityPage> firstPageResp = null;
            var queryAllCtrl = new QueryAllItemsByPageHelper<SledCommodity>(pageIndex => {
                if (ct.IsCancellationRequested) return null;

                var option = new ReqSledCommodityOption
                {
                    PlatformEnv = loginDataService.ProxyLoginResp?.HostingRunningMode.Contract_HostingRunningMode2TechPlatformEnv()?? TechPlatformEnv.REAL
                };
                var resp = XqThriftLibConfigurationManager.SharedInstance.ContractOnlineServiceHttpStub.reqSledCommodity(option, pageIndex, queryPageSize);
                if (resp == null) return null;
                if (pageIndex == 0)
                {
                    firstPageResp = resp;
                }
                var pageInfo = resp.CorrectResult;
                var pageResult = new QueryItemsByPageResult<SledCommodity>(resp.SourceException != null)
                {
                    TotalCount = pageInfo?.Total,
                    Page = pageInfo?.Page?.ToArray()
                };
                return pageResult;
            });

            queryAllCtrl.RemoveDuplicateFunc = _items =>
            {
                if (_items == null) return null;
                var idGroupedItems = _items.GroupBy(i => i.SledCommodityId);
                return idGroupedItems.Select(i => i.LastOrDefault()).ToArray();
            };

            var queriedItems = queryAllCtrl.QueryAllItems();
            if (firstPageResp == null) return null;

            var tarResp = new InterfaceInteractResponse<IEnumerable<NativeCommodity>>(firstPageResp.Servant,
                firstPageResp.InterfaceName,
                firstPageResp.SourceException,
                firstPageResp.HasTransportException,
                firstPageResp.HttpResponseStatusCode,
                queriedItems?.Select(i => i.ToNativeCommodity()).ToArray())
            {
                CustomParsedExceptionResult = firstPageResp.CustomParsedExceptionResult,
                InteractInformation = firstPageResp.InteractInformation
            };

            if (cacheQueriedItems)
            {
                // cache commodities
                CacheCommodityItems(tarResp.CorrectResult);
            }

            return tarResp;
        }

        private void CacheCommodityItems(IEnumerable<NativeCommodity> commodities)
        {
            if (commodities == null) return;
            foreach (var item in commodities)
            {
                commodityCacheController.Cache(item.SledCommodityId, item);
            }
        }

        private IInterfaceInteractResponse<IEnumerable<NativeCommodity>> GenerateCommodityQueryResponse(IInterfaceInteractResponse<SledCommodityPage> srcResp, IEnumerable<SledCommodity> srcCommodities)
        {
            if (srcResp == null) return null;
            var tarResp = new InterfaceInteractResponse<IEnumerable<NativeCommodity>>(srcResp.Servant,
                    srcResp.InterfaceName,
                    srcResp.SourceException,
                    srcResp.HasTransportException,
                    srcResp.HttpResponseStatusCode,
                    srcCommodities?.Select(i => i.ToNativeCommodity()).ToArray())
            {
                InteractInformation = srcResp.InteractInformation,
                CustomParsedExceptionResult = srcResp.CustomParsedExceptionResult
            };
            return tarResp;
        }

        private IInterfaceInteractResponse<NativeCommodity> GenerateCommodityFirstItemQueryResponse(IInterfaceInteractResponse<SledCommodityPage> srcResp, IEnumerable<SledCommodity> srcCommodities)
        {
            if (srcResp == null) return null;
            var tarResp = new InterfaceInteractResponse<NativeCommodity>(srcResp.Servant,
                    srcResp.InterfaceName,
                    srcResp.SourceException,
                    srcResp.HasTransportException,
                    srcResp.HttpResponseStatusCode,
                    srcCommodities?.FirstOrDefault()?.ToNativeCommodity())
            {
                InteractInformation = srcResp.InteractInformation,
                CustomParsedExceptionResult = srcResp.CustomParsedExceptionResult
            };
            return tarResp;
        }
    }
}