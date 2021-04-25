﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using xueqiao.trade.hosting;
using XueQiaoFoundation.Interfaces.Applications;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Helper.WeakDelegate;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;
using IDLAutoGenerated.Util;
using xueqiao.trade.hosting.terminal.ao;
using lib.xqclient_base.thriftapi_mediation.Interface;
using lib.xqclient_base.thriftapi_mediation;
using business_foundation_lib.xq_thriftlib_config;

namespace Manage.Applications.ServiceControllers
{
    [Export(typeof(IManageSubAccountQueryCtrl)), Export(typeof(IManageModuleSingletonController)),
        PartCreationPolicy(CreationPolicy.Shared)]
    internal class ManageSubAccountQueryCtrl : IManageSubAccountQueryCtrl, IManageModuleSingletonController
    {
        private readonly ILoginDataService loginDataService;
        private readonly IManageSubAccountCacheCtrl manageSubAccountCacheCtrl;

        private readonly IDIncreaser queryAllSubAccountsReqIdIncreaser = new IDIncreaser();
        private readonly object queryAllSubAccountsLock = new object();
        private Task<IInterfaceInteractResponse<IEnumerable<HostingSubAccount>>> queryAllSubAccountsTask;
        private CancellationTokenSource queryAllSubAccountsCts;
        private readonly Dictionary<long, ActionDelegateReference<IInterfaceInteractResponse<IEnumerable<HostingSubAccount>>>>
            allSubAccountsQueriedCallbackHandlers = new Dictionary<long, ActionDelegateReference<IInterfaceInteractResponse<IEnumerable<HostingSubAccount>>>>();

        [ImportingConstructor]
        public ManageSubAccountQueryCtrl(
            ILoginDataService loginDataService,
            IManageSubAccountCacheCtrl manageSubAccountCacheCtrl)
        {
            this.loginDataService = loginDataService;
            this.manageSubAccountCacheCtrl = manageSubAccountCacheCtrl;
        }

        public void Shutdown()
        {
            lock (queryAllSubAccountsLock)
            {
                if (queryAllSubAccountsCts != null)
                {
                    queryAllSubAccountsCts.Cancel();
                    queryAllSubAccountsCts.Dispose();
                    queryAllSubAccountsCts = null;
                }
                allSubAccountsQueriedCallbackHandlers.Clear();
            }
        }

        public long QueryAllSubAccounts(ActionDelegateReference<IInterfaceInteractResponse<IEnumerable<HostingSubAccount>>> handler)
        {
            lock (queryAllSubAccountsLock)
            {
                long reqId = queryAllSubAccountsReqIdIncreaser.RequestIncreasedId();
                if (handler != null)
                {
                    allSubAccountsQueriedCallbackHandlers.Add(reqId, handler);
                }

                if (queryAllSubAccountsTask != null
                    && queryAllSubAccountsTask.IsCanceled == false
                    && queryAllSubAccountsTask.IsCompleted == false
                    && queryAllSubAccountsTask.IsFaulted == false)
                {
                    return reqId;
                }

                if (queryAllSubAccountsCts != null)
                {
                    queryAllSubAccountsCts.Cancel();
                    queryAllSubAccountsCts.Dispose();
                    queryAllSubAccountsCts = null;
                }
                queryAllSubAccountsCts = new CancellationTokenSource();
                var cancelToken = queryAllSubAccountsCts.Token;
                queryAllSubAccountsTask = Task.Run(() => QueryAllSubAccounts(cancelToken, true), cancelToken);
                queryAllSubAccountsTask.ContinueWith(task =>
                {
                    if (cancelToken.IsCancellationRequested) return;
                    CallbackForAllSubAccoiuntsQueriedResponse(task.Result);
                }, cancelToken);

                return reqId;
            }
        }

        public void RemoveQueryAllSubAccountsHandler(long reqId)
        {
            lock (queryAllSubAccountsLock)
            {
                allSubAccountsQueriedCallbackHandlers.Remove(reqId);
            }
        }

        public IInterfaceInteractResponse<IEnumerable<HostingSubAccount>> QueryAllSubAccounts(CancellationToken cancelToken)
        {
            var resp = QueryAllSubAccounts(cancelToken, true);
            if (cancelToken.IsCancellationRequested) return null;
            CallbackForAllSubAccoiuntsQueriedResponse(resp);
            return resp;
        }

        public IInterfaceInteractResponse<HostingSubAccount> QuerySubAccount(long subAccountId)
        {
            var landingInfo = loginDataService.ProxyLoginResp?.HostingSession?.HostingSession2LandingInfo();
            if (landingInfo == null) return null;

            var option = new QueryHostingSAWRUItemListOption { SubAccountId = subAccountId };
            var pageOption = new IndexedPageOption
            {
                PageIndex = 0,
                PageSize = 1
            };
            var resp = XqThriftLibConfigurationManager.SharedInstance.TradeHostingTerminalAoHttpStub.getSAWRUTListPage(landingInfo, option, pageOption);
            if (resp == null) return null;

            var tarResp = new InterfaceInteractResponse<HostingSubAccount>(resp.Servant,
                    resp.InterfaceName,
                    resp.SourceException,
                    resp.HasTransportException,
                    resp.HttpResponseStatusCode,
                    resp.CorrectResult?.ResultList?.FirstOrDefault()?.SubAccount)
            {
                InteractInformation = resp.InteractInformation,
                CustomParsedExceptionResult = resp.CustomParsedExceptionResult
            };

            // Cache exchange
            if (tarResp.CorrectResult != null)
            {
                CacheSubAccountItems(new HostingSubAccount[] { tarResp.CorrectResult });
            }

            return tarResp;


        }

        private void CallbackForAllSubAccoiuntsQueriedResponse(IInterfaceInteractResponse<IEnumerable<HostingSubAccount>> resp)
        {
            if (resp == null) return;
            lock (queryAllSubAccountsLock)
            {
                var callbackHandlers = allSubAccountsQueriedCallbackHandlers.Values.ToArray();
                allSubAccountsQueriedCallbackHandlers.Clear();

                foreach (var item in callbackHandlers)
                {
                    item.Target?.Invoke(resp);
                }
            }
        }

        private void CacheSubAccountItems(IEnumerable<HostingSubAccount> subAccountItems)
        {
            if (subAccountItems == null) return;
            foreach (var item in subAccountItems)
            {
                manageSubAccountCacheCtrl.Cache(item.SubAccountId, item);
            }
        }

        private IInterfaceInteractResponse<IEnumerable<HostingSubAccount>> QueryAllSubAccounts(CancellationToken cancelToken, bool cacheQueriedItems)
        {
            var landingInfo = loginDataService.ProxyLoginResp?.HostingSession?.HostingSession2LandingInfo();
            if (landingInfo == null) return null;
            var queryPageSize = 50;
            IInterfaceInteractResponse<HostingSAWRUItemListPage> firstPageResp = null;
            var queryAllCtrl = new QueryAllItemsByPageHelper<HostingSAWRUItemList>(pageIndex => {

                if (cancelToken.IsCancellationRequested) return null;

                var option = new QueryHostingSAWRUItemListOption();
                var pageOption = new IndexedPageOption
                {
                    NeedTotalCount = true,
                    PageIndex = pageIndex,
                    PageSize = queryPageSize
                };
                var resp = XqThriftLibConfigurationManager.SharedInstance.TradeHostingTerminalAoHttpStub.getSAWRUTListPage(landingInfo, option, pageOption);
                if (resp == null) return null;
                if (pageIndex == 0)
                {
                    firstPageResp = resp;
                }
                var pageInfo = resp.CorrectResult;
                var pageResult = new QueryItemsByPageResult<HostingSAWRUItemList>(resp.SourceException != null)
                {
                    TotalCount = pageInfo?.TotalCount,
                    Page = pageInfo?.ResultList?.ToArray()
                };
                return pageResult;
            });

            queryAllCtrl.RemoveDuplicateFunc = _items =>
            {
                if (_items == null) return null;
                var idGroupedItems = _items.GroupBy(i => i.SubAccount.SubAccountId);
                return idGroupedItems.Select(i => i.LastOrDefault()).ToArray();
            };

            var queriedItems = queryAllCtrl.QueryAllItems();
            if (firstPageResp == null) return null;

            var tarResp = new InterfaceInteractResponse<IEnumerable<HostingSubAccount>>(firstPageResp.Servant,
                firstPageResp.InterfaceName,
                firstPageResp.SourceException,
                firstPageResp.HasTransportException,
                firstPageResp.HttpResponseStatusCode,
                queriedItems?.Select(i => i.SubAccount).ToArray())
            {
                CustomParsedExceptionResult = firstPageResp.CustomParsedExceptionResult,
                InteractInformation = firstPageResp.InteractInformation
            };

            if (cacheQueriedItems)
            {
                CacheSubAccountItems(tarResp.CorrectResult);
            }

            return tarResp;
        }
    }
}