using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using XueQiaoFoundation.Interfaces.Applications;
using System.Threading;
using System.Threading.Tasks.Schedulers;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Helper.WeakDelegate;
using lib.xqclient_base.logger;

namespace XueQiaoFoundation.Applications.Controllers
{
    /// <summary>
    /// 合约树(contract-commodity-exchange)的查询 controller
    /// </summary>
    [Export(typeof(IContractItemTreeQueryController)), Export(typeof(IXueQiaoFoundationSingletonController)), PartCreationPolicy(CreationPolicy.Shared)]
    internal class ContractItemTreeQueryController : IContractItemTreeQueryController, IXueQiaoFoundationSingletonController
    {
        private readonly IExchangeQueryController exchangeQueryController;
        private readonly ICommodityQueryController commodityQueryController;
        private readonly IContractQueryController contractQueryController;
        private readonly IExchangeCacheController exchangeCacheController;
        private readonly ICommodityCacheController commodityCacheController;
        private readonly IContractCacheController contractCacheController;

        private readonly ConcurrentDictionary<long, CancellationTokenSource> queryCancellationTokenSources;
        private readonly IDIncreaser queryReqIdIncreaser = new IDIncreaser();
        private readonly ConcurrentDictionary<long, ActionDelegateReference<Dictionary<int, ContractItemTree>>> queryCallbackHandlers;
        private readonly TaskFactory queryTaskFactory = new TaskFactory(new LimitedConcurrencyLevelTaskScheduler(2));
        
        [ImportingConstructor]
        public ContractItemTreeQueryController(IExchangeQueryController exchangeQueryController,
            ICommodityQueryController commodityQueryController,
            IContractQueryController contractQueryController,
            IExchangeCacheController exchangeCacheController,
            ICommodityCacheController commodityCacheController,
            IContractCacheController contractCacheController)
        {
            this.exchangeQueryController = exchangeQueryController;
            this.commodityQueryController = commodityQueryController;
            this.contractQueryController = contractQueryController;
            this.exchangeCacheController = exchangeCacheController;
            this.commodityCacheController = commodityCacheController;
            this.contractCacheController = contractCacheController;

            queryCancellationTokenSources = new ConcurrentDictionary<long, CancellationTokenSource>();
            queryCallbackHandlers = new ConcurrentDictionary<long, ActionDelegateReference<Dictionary<int, ContractItemTree>>>();
        }

        public long QueryTreeItems(IEnumerable<int> contractIds,
            bool queryParentCommodity, 
            bool queryParentExchange,
            bool ignoreCache,
            ActionDelegateReference<Dictionary<int, ContractItemTree>> handler)
        {
            if (contractIds == null || contractIds.Count() == 0)
            {
                throw new ArgumentException("contractIds can't be empty or null.");
            }
            long reqId = queryReqIdIncreaser.RequestIncreasedId();
            var cancellationSource = new CancellationTokenSource();
            var cancelToken = cancellationSource.Token;
            if (handler != null)
            {
                queryCancellationTokenSources.TryAdd(reqId, cancellationSource);
                queryCallbackHandlers.TryAdd(reqId, handler);
            }

            queryTaskFactory.StartNew(() => 
            {
                var tarResult = QueryTreeItems(contractIds, queryParentCommodity, queryParentExchange, ignoreCache, cancelToken);
                if (queryCallbackHandlers.TryRemove(reqId, out ActionDelegateReference<Dictionary<int, ContractItemTree>> callback))
                {
                    callback.Target?.Invoke(tarResult);
                }

                if (queryCancellationTokenSources.TryRemove(reqId, out CancellationTokenSource _cts))
                {
                    _cts.Dispose();
                }

            }, cancelToken);

            return reqId;
        }

        public void RemoveQueryTreeItemsHandler(long reqId)
        {
            queryCallbackHandlers.TryRemove(reqId, out ActionDelegateReference<Dictionary<int, ContractItemTree>> ignore);
            if (queryCancellationTokenSources.TryRemove(reqId, out CancellationTokenSource _cts))
            {
                _cts.Cancel();
                _cts.Dispose();
            }
        }

        public Dictionary<int, ContractItemTree> QueryTreeItems(IEnumerable<int> contractIds,
            bool queryParentCommodity,
            bool queryParentExchange,
            bool ignoreCache,
            CancellationToken cancelToken)
        {
            var tarTrees = new List<ContractItemTree>();
            var queryTasks = new List<Task<ContractItemTree>>();

            foreach (var contractId in contractIds.Distinct().ToArray())
            {
                queryTasks.Add(Task.Run(() =>
                {
                    cancelToken.ThrowIfCancellationRequested();
                    var result = QueryContractItemTree(contractId, queryParentCommodity, queryParentExchange, ignoreCache);
                    cancelToken.ThrowIfCancellationRequested();
                    return result;
                }, cancelToken));
            }

            try
            {
                Task.WaitAll(queryTasks.ToArray());
            }
            catch (AggregateException _ae)
            {
                if (!cancelToken.IsCancellationRequested)
                {
                    var exps = _ae.Flatten()?.InnerExceptions;
                    if (exps?.Any() == true)
                    {
                        foreach (var exp in exps)
                        {
                            AppLog.Error($"Failed QueryTreeItems in task, {exp.GetType().Name}");
                        }
                    }
                }
                return null;
            }
            
            foreach (var task in queryTasks)
            {
                var taskResult = task.Result;
                if (taskResult != null)
                {
                    tarTrees.Add(taskResult);
                }
            }

            return tarTrees.ToDictionary(i => i.Contract?.SledContractId ?? 0);
        }

        public ContractItemTree QueryContractItemTree(int contractId, 
            bool queryParentCommodity,
            bool queryParentExchange,
            bool ignoreCache)
        {
            var contractItemTree = new ContractItemTree();

            if (ignoreCache == false)
            {
                contractItemTree.Contract = contractCacheController.GetCache(contractId);
            }
            if (contractItemTree.Contract == null)
            {
                contractItemTree.Contract = contractQueryController.QueryContract(contractId)?.CorrectResult;
            }

            if (contractItemTree.Contract != null)
            {
                if (queryParentCommodity)
                {
                    var commodityId = contractItemTree.Contract.SledCommodityId;
                    var commodityTree = QueryCommodityItemTree(commodityId, queryParentExchange, ignoreCache);
                    contractItemTree.ParentCommodity = commodityTree?.Commodity;
                    contractItemTree.ParentExchange = commodityTree?.ParentExchange;
                }
            }
            return contractItemTree;
        }

        public CommodityItemTree QueryCommodityItemTree(int commodityId,
            bool queryParentExchange,
            bool ignoreCache)
        {
            var itemTree = new CommodityItemTree();
            if (ignoreCache == false)
            {
                itemTree.Commodity = commodityCacheController.GetCache(commodityId);
            }
            if (itemTree.Commodity == null)
            {
                itemTree.Commodity = commodityQueryController.QueryCommodity(commodityId)?.CorrectResult;
            }

            var exchangeMic = itemTree.Commodity?.ExchangeMic;
            if (queryParentExchange && exchangeMic != null)
            {
                if (ignoreCache == false)
                {
                    itemTree.ParentExchange = exchangeCacheController.GetCache(exchangeMic);
                }
                if (itemTree.ParentExchange == null)
                {
                    itemTree.ParentExchange = exchangeQueryController.QueryExchange(exchangeMic)?.CorrectResult;
                }
            }
            return itemTree;
        }

        public void Shutdown()
        {
            var queryCancellations = queryCancellationTokenSources.ToArray();
            foreach (var i in queryCancellations)
            {
                if (queryCancellationTokenSources.TryRemove(i.Key, out CancellationTokenSource tmp))
                {
                    tmp.Cancel();
                    tmp.Dispose();
                }
            }

            queryCallbackHandlers.Clear();
        }
    }
}
