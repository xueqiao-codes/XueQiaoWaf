using business_foundation_lib.xq_thriftlib_config;
using lib.xqclient_base.thriftapi_mediation;
using lib.xqclient_base.thriftapi_mediation.Interface;
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
using Thrift.Collections;
using xueqiao.trade.hosting.asset.thriftapi;
using xueqiao.trade.hosting.events;
using xueqiao.trade.hosting.proxy;
using xueqiao.trade.hosting.push.protocol;
using xueqiao.trade.hosting.terminal.ao;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.Interfaces.Applications;
using XueQiaoFoundation.Interfaces.Event.business;
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
    /// 资金列表控制器
    /// 
    /// 1.承担拉取资金列表的职责。
    /// 2.处理资金列表相关的 push event。
    /// </summary>
    [Export(typeof(IFundItemsController)), Export(typeof(ITradeModuleSingletonController)),
        PartCreationPolicy(CreationPolicy.Shared)]
    internal class FundItemsController : IFundItemsController, ITradeModuleSingletonController
    {
        private delegate FundItemUpdateTemplate FundItemUpdateTemplateFactory(HostingFundKey itemKey, FundItemDataModel existItem);

        /// <summary>
        /// 资金项修改模板
        /// </summary>
        private class FundItemUpdateTemplate
        {
            public Tuple<double?> PreFund;

            public Tuple<double?> DepositAmount;

            public Tuple<double?> WithdrawAmount;

            public Tuple<double?> CloseProfit;

            public Tuple<double?> PositionProfit;

            public Tuple<double?> UseMargin;

            public Tuple<double?> FrozenMargin;

            public Tuple<double?> UseCommission;

            public Tuple<double?> FrozenCommission;

            public Tuple<double?> AvailableFund;

            public Tuple<double?> DynamicBenefit;

            public Tuple<double?> RiskRate;

            public Tuple<double?> CreditAmount;

            public Tuple<double?> GoodsValue;

            public Tuple<double?> Leverage;
        }

        private readonly FundItemsService fundItemsService;
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
        /// 资金列表同步任务工厂。该工厂生产的任务是顺序执行，将增删、刷新包装成任务执行，才可保证同步的准确性
        /// </summary>
        private readonly TaskFactory fundItemsSyncTaskFactory = new TaskFactory(new OrderedTaskScheduler());
        /// <summary>
        /// 资金列表
        /// </summary>
        private readonly Dictionary<HostingFundKey, FundItemDataModel> fundItems = new Dictionary<HostingFundKey, FundItemDataModel>();
        private readonly object fundItemsLock = new object();


        private CancellationTokenSource fundItemsRefreshCTS;
        private readonly object fundItemsRefreshLock = new object();
        private readonly Dictionary<long, SubAccountAnyDataRefreshStateHolder> fundItemsRefreshStateHolders
            = new Dictionary<long, SubAccountAnyDataRefreshStateHolder>();
        
        private readonly IDIncreaser clientDataForceSyncTimesIncreaser = new IDIncreaser(0);

        [ImportingConstructor]
        public FundItemsController(
            FundItemsService fundItemsService,
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
            this.fundItemsService = fundItemsService;
            this.eventAggregator = eventAggregator;
            this.loginDataService = loginDataService;
            this.loginUserManageService = loginUserManageService;
            this.relatedSubAccountItemsCtrl = relatedSubAccountItemsCtrl;

            this.subAccountRelatedItemQueryCtrl = subAccountRelatedItemQueryCtrl;
            this.subAccountRelatedItemCacheCtrl = subAccountRelatedItemCacheCtrl;
            this.hostingUserQueryCtrl = hostingUserQueryCtrl;
            this.hostingUserCacheCtrl = hostingUserCacheCtrl;
            this.contractItemTreeQueryCtrl = contractItemTreeQueryCtrl;

            loginUserManageService.Value.HasLogouted += RecvUserHasLogouted;
            eventAggregator.GetEvent<ClientDataForceSync>().Subscribe(ReceivedClientDataForceSyncEvent);
            eventAggregator.GetEvent<HostingFundChanged>().Subscribe(ReceivedHostingFundChangedEvent);
            eventAggregator.GetEvent<UserRelatedSubAccountItemsRefreshEvent>().Subscribe(RecvUserRelatedSubAccountItemsRefreshEvent);
        }

        public void Shutdown()
        {
            clientDataForceSyncTimesIncreaser?.Reset();
            CancelSubAccountFundItemsRefresh(true);
            RemoveSubAccountFundItems(null);

            loginUserManageService.Value.HasLogouted -= RecvUserHasLogouted;
            eventAggregator.GetEvent<ClientDataForceSync>().Unsubscribe(ReceivedClientDataForceSyncEvent);
            eventAggregator.GetEvent<HostingFundChanged>().Unsubscribe(ReceivedHostingFundChangedEvent);
            eventAggregator.GetEvent<UserRelatedSubAccountItemsRefreshEvent>().Unsubscribe(RecvUserRelatedSubAccountItemsRefreshEvent);
        }

        public event SubAccountAnyDataRefreshStateChanged FundItemsRefreshStateChanged;

        public void RefreshFundItemsForce(long subAccountId)
        {
            RefreshSubAccountFundItems(new long[] { subAccountId }, true);
        }

        public void RefreshFundItemsIfNeed(long subAccountId)
        {
            RefreshSubAccountFundItems(new long[] { subAccountId }, false);
        }

        private void RecvUserHasLogouted(ProxyLoginResp lastLoginResp)
        {
            clientDataForceSyncTimesIncreaser?.Reset();
            CancelSubAccountFundItemsRefresh(true);
            RemoveSubAccountFundItems(null);
        }

        private void ReceivedClientDataForceSyncEvent(ClientForceSyncEvent obj)
        {
            if (loginDataService.ProxyLoginResp == null) return;
            var forceSyncTimes = clientDataForceSyncTimesIncreaser.RequestIncreasedId();
            
            // 同步资金列表
            var userRelatedSubAccountIds = relatedSubAccountItemsCtrl.RelatedSubAccountItems?.Select(i => i.SubAccountId).Distinct();
            if (userRelatedSubAccountIds?.Any() != true) return;
            
            CancelSubAccountFundItemsRefresh(false);
            RefreshSubAccountFundItems(userRelatedSubAccountIds, forceSyncTimes != 1);
        }

        private void ReceivedHostingFundChangedEvent(HostingFundEvent payload)
        {
            if (payload == null || payload.HostingFund == null) return;

            lock (fundItemsLock)
            {
                if (loginDataService.ProxyLoginResp == null) return;
                UnsafeAddOrUpdateFundItems(new HostingFund[] { payload.HostingFund }, _item => payload.BaseCurrency, 
                    out IEnumerable<FundItemDataModel> addList, out IEnumerable<FundItemDataModel> updateList);
            }
        }

        private void RecvUserRelatedSubAccountItemsRefreshEvent(UserRelatedSubAccountItemsRefreshEventArgs args)
        {
            var currentLoginToken = loginDataService?.ProxyLoginResp?.HostingSession?.Token;
            if (currentLoginToken == null || currentLoginToken != args.LoginUserToken) return;

            var relatedSubAccountIds = args.RelatedSubAccountItems?.Select(i => i.SubAccountId).Distinct();
            if (relatedSubAccountIds?.Any() != true) return;

            RefreshSubAccountFundItems(relatedSubAccountIds, false);
        }

        /// <summary>
        /// 删除某个操作账户的所有资金项。如果 subAccountId 为 null，则删除所有
        /// </summary>
        /// <param name="subAccountId"></param>
        private void RemoveSubAccountFundItems(long? subAccountId)
        {
            lock (fundItemsLock)
            {
                UnsafeRemoveSubAccountFundItems(subAccountId);
            }
        }

        /// <summary>
        /// 非线程安全地删除某个操作账户的所有资金项。如果 subAccountId 为 null，则删除所有
        /// </summary>
        /// <param name="subAccountId"></param>
        private void UnsafeRemoveSubAccountFundItems(long? subAccountId)
        {
            KeyValuePair<HostingFundKey, FundItemDataModel>[] rmItems = null;
            if (subAccountId == null)
                rmItems = fundItems.ToArray();
            else
                rmItems = fundItems.Where(i => i.Key.SubAccountId == subAccountId).ToArray();

            foreach (var rmItem in rmItems)
            {
                fundItems.Remove(rmItem.Key);
            }
            UIItemsAddOrRemoveDispatcherBeginInvoke(() =>
            {
                foreach (var rmItem in rmItems)
                {
                    fundItemsService.FundItems.Remove(rmItem.Value);
                }
            });
        }

        private static bool IsRefreshingOrSuccessRefreshed(DataRefreshState refreshState)
        {
            return refreshState == DataRefreshState.Refreshing || refreshState == DataRefreshState.SuccessRefreshed;
        }

        private SubAccountAnyDataRefreshStateHolder AcquireSubAccountFundItemsRefreshStateHolder(long subAccountId)
        {
            lock (fundItemsRefreshLock)
            {
                SubAccountAnyDataRefreshStateHolder dataHolder = null;
                if (!fundItemsRefreshStateHolders.TryGetValue(subAccountId, out dataHolder))
                {
                    dataHolder = new SubAccountAnyDataRefreshStateHolder(subAccountId);
                    fundItemsRefreshStateHolders.Add(subAccountId, dataHolder);
                }
                return dataHolder;
            }
        }

        private CancellationToken AcquireSubAccountFundItemsRefreshCLT()
        {
            CancellationToken clt = CancellationToken.None;
            lock (fundItemsRefreshLock)
            {
                if (fundItemsRefreshCTS == null)
                {
                    fundItemsRefreshCTS = new CancellationTokenSource();
                }
                clt = fundItemsRefreshCTS.Token;
            }
            return clt;
        }

        private void CancelSubAccountFundItemsRefresh(bool clearRefreshStateHolders)
        {
            lock (fundItemsRefreshLock)
            {
                if (fundItemsRefreshCTS != null)
                {
                    fundItemsRefreshCTS.Cancel();
                    fundItemsRefreshCTS.Dispose();
                    fundItemsRefreshCTS = null;
                }

                if (clearRefreshStateHolders)
                {
                    fundItemsRefreshStateHolders.Clear();
                }
            }
        }
        
        private void UnsafeAddOrUpdateFundItems(
            IEnumerable<HostingFund> addOrUpdatefundItems,
            Func<HostingFund, bool> isBaseCurrencyGetter,
            out IEnumerable<FundItemDataModel> _addedItems,
            out IEnumerable<FundItemDataModel> _updatedItems)
        {
            _addedItems = null;
            _updatedItems = null;

            Debug.Assert(isBaseCurrencyGetter != null);

            if (addOrUpdatefundItems?.Any() != true) return;

            var itemMap = addOrUpdatefundItems.ToDictionary(i => new HostingFundKey(i.SubAccountId, isBaseCurrencyGetter(i), i.Currency));

            UnsafeAddOrUpdateFundItems(itemMap.Keys,
                _key =>
                {
                    return GenerateFundItem(_key);
                },
                (_key, _existItem) =>
                {
                    var hostingItem = itemMap[_key];
                    return GenerateUpdateTemplate(hostingItem);
                },
                out _addedItems,
                out _updatedItems);
        }
        
        private void UnsafeAddOrUpdateFundItems(
            IEnumerable<HostingFundKey> newItemKeys,
            Func<HostingFundKey, FundItemDataModel> newItemFactory,
            FundItemUpdateTemplateFactory updateTemplateFactory,
            out IEnumerable<FundItemDataModel> _addedItems,
            out IEnumerable<FundItemDataModel> _updatedItems)
        {
            _addedItems = null;
            _updatedItems = null;

            Debug.Assert(newItemFactory != null);

            if (newItemKeys?.Any() != true) return;

            var addList = new List<FundItemDataModel>();
            var updateList = new List<FundItemDataModel>();
            foreach (var itemKey in newItemKeys)
            {
                FundItemDataModel implItem = null;
                FundItemDataModel existItem = null;
                fundItems.TryGetValue(itemKey, out existItem);
                if (existItem == null)
                {
                    var newItem = newItemFactory.Invoke(itemKey);
                    if (newItem != null)
                    {
                        fundItems[itemKey] = newItem;
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
                    UpdateFundItemWithTemplate(implItem, temp);
                }
            }

            if (addList.Any())
            {
                UIItemsAddOrRemoveDispatcherBeginInvoke(() =>
                {
                    fundItemsService.FundItems.AddRange(addList);
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

        private FundItemDataModel GenerateFundItem(HostingFundKey itemKey)
        {
            var subAccountFields = new SubAccountFieldsForTradeData(null, itemKey.SubAccountId);
            TradeDMLoadHelper.SetupSubAccountFields(subAccountFields,
                subAccountRelatedItemQueryCtrl, subAccountRelatedItemCacheCtrl,
                hostingUserQueryCtrl, hostingUserCacheCtrl);

            var newItem = new FundItemDataModel(subAccountFields, itemKey.IsBaseCurrency, itemKey.Currency);
            return newItem;
        }

        private IInterfaceInteractResponse<IEnumerable<HostingFund>> QuerySubAccountFundItems(LandingInfo landingInfo,
            long subAccountId, bool queryBaseCurrency)
        {
            if (landingInfo == null) return null;

            var option = new ReqHostingFundOption
            {
                SubAccountIds = new THashSet<long> { subAccountId },
                BaseCurrency = queryBaseCurrency
            };

            var resp = XqThriftLibConfigurationManager.SharedInstance.TradeHostingTerminalAoHttpStub.getHostingSubAccountFund(landingInfo, option);
            if (resp == null) return null;

            var queryItems = resp.CorrectResult?.Page?.Where(i => i.SubAccountId == subAccountId).ToArray();
            var tarResp = new InterfaceInteractResponse<IEnumerable<HostingFund>>(resp.Servant,
                resp.InterfaceName,
                resp.SourceException,
                resp.HasTransportException,
                resp.HttpResponseStatusCode,
                queryItems)
            {
                CustomParsedExceptionResult = resp.CustomParsedExceptionResult,
                InteractInformation = resp.InteractInformation
            };
            return tarResp;
        }

        private void QuerySubAccountFundItems(LandingInfo landingInfo, long subAccountId,
            out IInterfaceInteractResponse<IEnumerable<HostingFund>> _baseCurrencyFundResp,
            out IInterfaceInteractResponse<IEnumerable<HostingFund>> _childrenCurrencyFundResp)
        {
            var taskFactory = new TaskFactory();
            var tasks = new List<Task>();

            // query base currency fund
            IInterfaceInteractResponse<IEnumerable<HostingFund>> baseCurrencyFundResp = null;
            var task1 = taskFactory.StartNew(() =>
            {
                baseCurrencyFundResp = QuerySubAccountFundItems(landingInfo, subAccountId, true);
            });
            tasks.Add(task1);

            // query children currency funds
            IInterfaceInteractResponse<IEnumerable<HostingFund>> childrenCurrencyFundResp = null;
            var task2 = taskFactory.StartNew(() =>
            {
                childrenCurrencyFundResp = QuerySubAccountFundItems(landingInfo, subAccountId, false);
            });
            tasks.Add(task2);

            Task.WaitAll(tasks.ToArray());

            _baseCurrencyFundResp = baseCurrencyFundResp;
            _childrenCurrencyFundResp = childrenCurrencyFundResp;
        }

        /// <summary>
        /// 刷新操作账号的资金列表
        /// </summary>
        /// <param name="subAccountIds">需要刷新的操作账号 id 列表</param>
        /// <param name="isForceRefresh">是否强制刷新。YES 表示强制刷新， NO 表示必要时才刷新</param>
        private void RefreshSubAccountFundItems(IEnumerable<long> subAccountIds, bool isForceRefresh)
        {
            if (subAccountIds?.Any() != true) return;

            fundItemsSyncTaskFactory.StartNew(() =>
            {
                var landingInfo = loginDataService.LandingInfo;
                if (landingInfo == null) return;

                var queryTsks = new List<Task>();
                var baseCurrencyQueryResps = new ConcurrentDictionary<long, IInterfaceInteractResponse<IEnumerable<HostingFund>>>();
                var childrenCurrencyQueryResps = new ConcurrentDictionary<long, IInterfaceInteractResponse<IEnumerable<HostingFund>>>();
                var refreshStateHolders = new List<SubAccountAnyDataRefreshStateHolder>();

                var clt = AcquireSubAccountFundItemsRefreshCLT();
                // 查询所有 subaccount 的数据
                foreach (var subAccountId in subAccountIds)
                {
                    var stateHolder = AcquireSubAccountFundItemsRefreshStateHolder(subAccountId);

                    bool needRefresh = true;
                    if (!isForceRefresh && IsRefreshingOrSuccessRefreshed(stateHolder.DataRefreshState))
                    {
                        needRefresh = false;
                    }

                    if (needRefresh && !clt.IsCancellationRequested)
                    {
                        refreshStateHolders.Add(stateHolder);
                        stateHolder.DataRefreshState = DataRefreshState.Refreshing;
                        FundItemsRefreshStateChanged?.Invoke(new SubAccountAnyDataRefreshStateChangedArgs(landingInfo.SubUserId, stateHolder.SubAccountId, stateHolder.DataRefreshState));
                        var tsk = Task.Run(() =>
                        {
                            clt.ThrowIfCancellationRequested();

                            QuerySubAccountFundItems(landingInfo, subAccountId, 
                                out IInterfaceInteractResponse<IEnumerable<HostingFund>> _baseResp,
                                out IInterfaceInteractResponse<IEnumerable<HostingFund>> _childrenResp);
                            baseCurrencyQueryResps.TryAdd(subAccountId, _baseResp);
                            childrenCurrencyQueryResps.TryAdd(subAccountId, _childrenResp);

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
                        FundItemsRefreshStateChanged?.Invoke(new SubAccountAnyDataRefreshStateChangedArgs(landingInfo.SubUserId, stateHolder.SubAccountId, stateHolder.DataRefreshState));
                    }
                    return;
                }
                
                lock (fundItemsLock)
                {
                    if (loginDataService.ProxyLoginResp == null) return;

                    foreach (var stateHolder in refreshStateHolders)
                    {
                        IInterfaceInteractResponse<IEnumerable<HostingFund>> baseCurrencyResp = null;
                        IInterfaceInteractResponse<IEnumerable<HostingFund>> childrenCurrencyResp = null;
                        baseCurrencyQueryResps.TryGetValue(stateHolder.SubAccountId, out baseCurrencyResp);
                        childrenCurrencyQueryResps.TryGetValue(stateHolder.SubAccountId, out childrenCurrencyResp);

                        var isQueryFailed = baseCurrencyResp?.CorrectResult == null || childrenCurrencyResp?.CorrectResult == null;

                        stateHolder.DataRefreshState = isQueryFailed ? DataRefreshState.FailedRefreshed : DataRefreshState.SuccessRefreshed;
                        FundItemsRefreshStateChanged?.Invoke(new SubAccountAnyDataRefreshStateChangedArgs(landingInfo.SubUserId, stateHolder.SubAccountId, stateHolder.DataRefreshState));


                        if (clt.IsCancellationRequested) continue;

                        // 清除 subAccount 的资金项
                        UnsafeRemoveSubAccountFundItems(stateHolder.SubAccountId);

                        // 添加新的资金项
                        if (baseCurrencyResp?.CorrectResult != null)
                        {
                            UnsafeAddOrUpdateFundItems(baseCurrencyResp?.CorrectResult, _item => true, 
                                out IEnumerable<FundItemDataModel> addList, out IEnumerable<FundItemDataModel> updateList);
                        }

                        if (childrenCurrencyResp?.CorrectResult != null)
                        {
                            UnsafeAddOrUpdateFundItems(childrenCurrencyResp?.CorrectResult, _item => false,
                                out IEnumerable<FundItemDataModel> addList, out IEnumerable<FundItemDataModel> updateList);
                        }
                    }
                }
            });
        }
        
        private static FundItemUpdateTemplate GenerateUpdateTemplate(HostingFund srcFund)
        {
            var temp = new FundItemUpdateTemplate();
            if (srcFund != null)
            {
                temp.PreFund = new Tuple<double?>(srcFund.__isset.preFund ? (double?)srcFund.PreFund : null);
                temp.DepositAmount = new Tuple<double?>(srcFund.__isset.depositAmount ? (double?)srcFund.DepositAmount : null);
                temp.WithdrawAmount = new Tuple<double?>(srcFund.__isset.withdrawAmount ? (double?)srcFund.WithdrawAmount : null);
                temp.CloseProfit = new Tuple<double?>(srcFund.__isset.closeProfit ? (double?)srcFund.CloseProfit : null);
                temp.PositionProfit = new Tuple<double?>(srcFund.__isset.positionProfit ? (double?)srcFund.PositionProfit : null);
                temp.UseMargin = new Tuple<double?>(srcFund.__isset.useMargin ? (double?)srcFund.UseMargin : null);
                temp.FrozenMargin = new Tuple<double?>(srcFund.__isset.frozenMargin ? (double?)srcFund.FrozenMargin : null);
                temp.UseCommission = new Tuple<double?>(srcFund.__isset.useCommission ? (double?)srcFund.UseCommission : null);
                temp.FrozenCommission = new Tuple<double?>(srcFund.__isset.frozenCommission ? (double?)srcFund.FrozenCommission : null);
                temp.AvailableFund = new Tuple<double?>(srcFund.__isset.availableFund ? (double?)srcFund.AvailableFund : null);
                temp.DynamicBenefit = new Tuple<double?>(srcFund.__isset.dynamicBenefit ? (double?)srcFund.DynamicBenefit : null);
                temp.RiskRate = new Tuple<double?>(srcFund.__isset.riskRate ? (double?)srcFund.RiskRate : null);
                temp.CreditAmount = new Tuple<double?>(srcFund.__isset.creditAmount ? (double?)srcFund.CreditAmount : null);
                temp.GoodsValue = new Tuple<double?>(srcFund.__isset.goodsValue ? (double?)srcFund.GoodsValue : null);
                temp.Leverage = new Tuple<double?>(srcFund.__isset.leverage ? (double?)srcFund.Leverage : null);
            }
            return temp;
        }

        private void UpdateFundItemWithTemplate(FundItemDataModel fundItem, FundItemUpdateTemplate updateTemplate)
        {
            if (fundItem == null || updateTemplate == null) return;

            if (updateTemplate.PreFund != null) fundItem.PreFund = updateTemplate.PreFund.Item1;
            if (updateTemplate.DepositAmount != null) fundItem.DepositAmount = updateTemplate.DepositAmount.Item1;
            if (updateTemplate.WithdrawAmount != null) fundItem.WithdrawAmount = updateTemplate.WithdrawAmount.Item1;
            if (updateTemplate.CloseProfit != null) fundItem.CloseProfit = updateTemplate.CloseProfit.Item1;
            if (updateTemplate.PositionProfit != null) fundItem.PositionProfit = updateTemplate.PositionProfit.Item1;
            if (updateTemplate.UseMargin != null) fundItem.UseMargin = updateTemplate.UseMargin.Item1;
            if (updateTemplate.FrozenMargin != null) fundItem.FrozenMargin = updateTemplate.FrozenMargin.Item1;
            if (updateTemplate.UseCommission != null) fundItem.UseCommission = updateTemplate.UseCommission.Item1;
            if (updateTemplate.FrozenCommission != null) fundItem.FrozenCommission = updateTemplate.FrozenCommission.Item1;
            if (updateTemplate.AvailableFund != null) fundItem.AvailableFund = updateTemplate.AvailableFund.Item1;
            if (updateTemplate.DynamicBenefit != null) fundItem.DynamicBenefit = updateTemplate.DynamicBenefit.Item1;
            if (updateTemplate.RiskRate != null) fundItem.RiskRate = updateTemplate.RiskRate.Item1;
            if (updateTemplate.CreditAmount != null) fundItem.CreditAmount = updateTemplate.CreditAmount.Item1;
            if (updateTemplate.GoodsValue != null) fundItem.GoodsValue = updateTemplate.GoodsValue.Item1;
            if (updateTemplate.Leverage != null) fundItem.Leverage = updateTemplate.Leverage.Item1;

            fundItem.InvalidateIsExistFund();
        }
    }
}
