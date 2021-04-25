using business_foundation_lib.xq_thriftlib_config;
using lib.xqclient_base.thriftapi_mediation;
using lib.xqclient_base.thriftapi_mediation.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Schedulers;
using Thrift.Protocol;
using Touyan.Interface.application;
using xueqiao.graph.xiaoha.chart.thriftapi;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Model;

namespace Touyan.app.servicecontroller
{
    [Export(typeof(ITouyanChartFolderServiceCtrl)), Export(typeof(ITouyanModuleServiceCtrl)), PartCreationPolicy(CreationPolicy.Shared)]
    internal class TouyanChartFolderServiceCtrl : ITouyanChartFolderServiceCtrl, ITouyanModuleServiceCtrl
    {
        /// <summary>
        /// 列表同步任务工厂。该工厂生产的任务是顺序执行，将增删、刷新包装成任务执行，以保证同步的准确性
        /// </summary>
        private readonly TaskFactory folderListSyncTaskFactory = new TaskFactory(new OrderedTaskScheduler());
        /// <summary>
        /// 成交列表
        /// </summary>
        private readonly Dictionary<long, ChartFolder> folderList = new Dictionary<long, ChartFolder>();
        private readonly object folderListLock = new object();

        private CancellationTokenSource folderListRefreshCTS;
        private readonly object folderListRefreshLock = new object();
        private DataRefreshState folderListRefreshState = DataRefreshState.NotRefresh;

        [ImportingConstructor]
        public TouyanChartFolderServiceCtrl()
        {
        }

        public void Initialize()
        {

        }

        public void Shutdown()
        {
            CancelFolderListRefresh();
        }

        public event TouyanChartFolderListRefreshStateChanged FolderListRefreshStateChanged;

        public Task<ServerDataRefreshResultWrapper<IEnumerable<ChartFolder>>> RefreshFolderListIfNeed()
        {
            return RefreshFolderList(false);
        }

        public Task<ServerDataRefreshResultWrapper<IEnumerable<ChartFolder>>> RefreshFolderListForce()
        {
            return RefreshFolderList(true);
        }

        private Task<ServerDataRefreshResultWrapper<IEnumerable<ChartFolder>>> RefreshFolderList(bool isForceRefresh)
        {
            if (isForceRefresh)
            {
                CancelFolderListRefresh();
            }

            return folderListSyncTaskFactory.StartNew(() => 
            {
                var resultWrapper = new ServerDataRefreshResultWrapper<IEnumerable<ChartFolder>>();

                bool needRefresh = true;
                if (!isForceRefresh && IsRefreshingOrSuccessRefreshed(SafeGetFolderListRefreshState()))
                {
                    needRefresh = false;
                }

                resultWrapper.HasRequestRefresh = needRefresh;

                if (needRefresh == false)
                {
                    lock (folderListLock)
                    {
                        resultWrapper.ResultData = folderList.Values.ToArray();
                    }
                }
                else
                {
                    var clt = AcquireFolderListRefreshCLT();

                    DataRefreshState change2State = DataRefreshState.Refreshing;
                    SafeSetFolderListRefreshState(change2State);
                    FolderListRefreshStateChanged?.Invoke(change2State);

                    var resp = QueryAllChartFolders(clt);

                    change2State = DataRefreshState.NotRefresh;
                    if (clt.IsCancellationRequested)
                    {
                        change2State = DataRefreshState.CanceledRefresh;
                    }
                    else
                    {
                        change2State = (resp != null && resp.SourceException == null) ? DataRefreshState.SuccessRefreshed : DataRefreshState.FailedRefreshed;
                    }

                    if (resp != null)
                    {
                        resultWrapper.RefreshRequestResp = new IInterfaceInteractResponse[] { resp };
                        resultWrapper.ResultData = resp.CorrectResult;
                    }

                    lock (folderListLock)
                    {
                        if (!clt.IsCancellationRequested)
                        {
                            UnsafteClearChartFolders();
                            if (resp?.CorrectResult != null)
                            {
                                UnsafeAddOrUpdateChartFolders(resp?.CorrectResult, false,
                                    out IEnumerable<ChartFolder> _addItems, out IEnumerable<ChartFolder> _updateItems);
                            }
                        }
                    }

                    SafeSetFolderListRefreshState(change2State);
                    FolderListRefreshStateChanged?.Invoke(change2State);
                }

                resultWrapper.RefreshState = SafeGetFolderListRefreshState();

                return resultWrapper;
            });
        }

        private void UnsafeAddOrUpdateChartFolders(
            IEnumerable<ChartFolder> chartFolders,
            bool updateIfItemExist,
            out IEnumerable<ChartFolder> _addedItems, 
            out IEnumerable<ChartFolder> _updatedItems)
        {
            _addedItems = null;
            _updatedItems = null;

            if (chartFolders?.Any() != true) return;

            var addList = new List<ChartFolder>();
            var updateList = new List<ChartFolder>();
            foreach (var item in chartFolders)
            {
                var folderId = item.FolderId;
                if (folderList.TryGetValue(folderId, out ChartFolder existItem))
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
                    folderList[folderId] = item;
                    addList.Add(item);
                }
            }

            _addedItems = addList;
            _updatedItems = updateList;
        }

        private void UnsafteClearChartFolders()
        {
            folderList.Clear();
        }
        
        private IInterfaceInteractResponse<IEnumerable<ChartFolder>> QueryAllChartFolders(CancellationToken clt)
        {
            if (clt.IsCancellationRequested) return null;
            var queryPageSize = 50;
            IInterfaceInteractResponse<ChartFolderPage> lastPageResp = null;
            var queryOption = new ReqChartFolderOption { };

            var queryAllCtrl = new QueryAllItemsByPageHelper<ChartFolder>(pageIndex => 
            {
                if (clt.IsCancellationRequested) return null;
                var pageOption = new IndexedPageOption
                {
                    NeedTotalCount = true,
                    PageIndex = pageIndex,
                    PageSize = queryPageSize
                };

                var resp = XqThriftLibConfigurationManager.SharedInstance.XiaohaChartTerminalAoHttpStub
                    .reqChartFolder(queryOption, pageOption);
                lastPageResp = resp;

                if (resp == null) return null;

                var pageInfo = resp.CorrectResult;
                var pageResult = new QueryItemsByPageResult<ChartFolder>(resp.SourceException != null)
                {
                    TotalCount = pageInfo?.Total,
                    Page = pageInfo?.Page
                };
                return pageResult;
            });

            queryAllCtrl.RemoveDuplicateFunc = _items =>
            {
                if (_items == null) return null;
                var idGroupedItems = _items.GroupBy(i => i.FolderId);
                return idGroupedItems.Select(i => i.LastOrDefault()).ToArray();
            };

            var queriedItems = queryAllCtrl.QueryAllItems();
            if (lastPageResp == null) return null;
            if (clt.IsCancellationRequested) return null;

            var tarResp = new InterfaceInteractResponse<IEnumerable<ChartFolder>>(lastPageResp.Servant,
                lastPageResp.InterfaceName,
                lastPageResp.SourceException,
                lastPageResp.HasTransportException,
                lastPageResp.HttpResponseStatusCode,
                queriedItems)
            {
                CustomParsedExceptionResult = lastPageResp.CustomParsedExceptionResult,
                InteractInformation = lastPageResp.InteractInformation
            };
            return tarResp;
        }

        private DataRefreshState SafeGetFolderListRefreshState()
        {
            DataRefreshState state;
            lock (folderListRefreshLock)
            {
                state = folderListRefreshState;
            }
            return state;
        }

        private void SafeSetFolderListRefreshState(DataRefreshState state)
        {
            lock (folderListRefreshLock)
            {
                folderListRefreshState = state;
            }
        }

        private static bool IsRefreshingOrSuccessRefreshed(DataRefreshState subAccountLastRefreshState)
        {
            return subAccountLastRefreshState == DataRefreshState.Refreshing || subAccountLastRefreshState == DataRefreshState.SuccessRefreshed;
        }
        
        private CancellationToken AcquireFolderListRefreshCLT()
        {
            CancellationToken clt = CancellationToken.None;
            lock (folderListRefreshLock)
            {
                if (folderListRefreshCTS == null)
                {
                    folderListRefreshCTS = new CancellationTokenSource();
                }
                clt = folderListRefreshCTS.Token;
            }
            return clt;
        }

        private void CancelFolderListRefresh()
        {
            lock (folderListRefreshLock)
            {
                if (folderListRefreshCTS != null)
                {
                    folderListRefreshCTS.Cancel();
                    folderListRefreshCTS.Dispose();
                    folderListRefreshCTS = null;
                }
            }
        }
    }
}
