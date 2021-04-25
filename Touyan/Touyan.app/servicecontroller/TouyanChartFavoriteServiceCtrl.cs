using business_foundation_lib.xq_thriftlib_config;
using lib.xqclient_base.thriftapi_mediation.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Schedulers;
using Thrift.Collections;
using Touyan.app.datamodel;
using Touyan.app.helper;
using Touyan.Interface.application;
using Touyan.Interface.datamodel;
using xueqiao.graph.xiaoha.chart.terminal.ao.thriftapi;
using xueqiao.personal.user.thriftapi;
using XueQiaoFoundation.Shared.Model;
using XueQiaoFoundation.Shared.Helper;
using lib.xqclient_base.thriftapi_mediation;

namespace Touyan.app.servicecontroller
{
    [Export(typeof(ITouyanChartFavoriteServiceCtrl)), Export(typeof(ITouyanModuleServiceCtrl)), PartCreationPolicy(CreationPolicy.Shared)]
    internal class TouyanChartFavoriteServiceCtrl : ITouyanChartFavoriteServiceCtrl, ITouyanModuleServiceCtrl
    {
        private readonly ITouyanAuthUserLoginService authUserLoginService;
        private readonly ITouyanChartQueryCtrl chartQueryCtrl;

        /// <summary>
        /// 列表同步任务工厂。该工厂生产的任务是顺序执行，将增删、刷新包装成任务执行，以保证同步的准确性
        /// </summary>
        private readonly TaskFactory favorListSyncTaskFactory = new TaskFactory(new OrderedTaskScheduler());

        private readonly Dictionary<ChartFavoriteListItemKey, ChartFavoriteListItem> favoriteList
            = new Dictionary<ChartFavoriteListItemKey, ChartFavoriteListItem>();
        private readonly object favoriteListLock = new object();

        private CancellationTokenSource favoriteListRefreshCTS;
        private readonly object favoriteListRefreshLock = new object();
        private DataRefreshState favoriteListRefreshState = DataRefreshState.NotRefresh;

        [ImportingConstructor]
        public TouyanChartFavoriteServiceCtrl(ITouyanAuthUserLoginService authUserLoginService,
            ITouyanChartQueryCtrl chartQueryCtrl)
        {
            this.authUserLoginService = authUserLoginService;
            this.chartQueryCtrl = chartQueryCtrl;
        }

        public void Initialize()
        {
            authUserLoginService.TouyanAuthUserHasLogined += RecvTouyanAuthUserHasLogined;
            authUserLoginService.TouyanAuthUserHasLogouted += RecvTouyanAuthUserHasLogouted;
        }
        
        public void Shutdown()
        {
            authUserLoginService.TouyanAuthUserHasLogined -= RecvTouyanAuthUserHasLogined;
            authUserLoginService.TouyanAuthUserHasLogouted -= RecvTouyanAuthUserHasLogouted;
        }

        #region ITouyanChartFavoriteServiceCtrl
        
        public event TouyanChartFavoriteListRefreshStateChanged FavoriteListRefreshStateChanged;

        public Task<ServerDataRefreshResultWrapper<IEnumerable<ChartFavoriteListItem>>> RefreshFavoriteListIfNeed()
        {
            return RefreshFavoriteList(false);
        }

        public Task<ServerDataRefreshResultWrapper<IEnumerable<ChartFavoriteListItem>>> RefreshFavoriteListForce()
        {
            return RefreshFavoriteList(true);
        }

        public event TouyanChartFavoriteItemAdded TouyanChartFavoriteItemAdded;
        
        public Task<RequestAddFavoriteFolderResp> RequestAddFavoriteFolder(FavoriteFolder favoriteInfo)
        {
            return Task.Run(() => 
            {
                var landingInfo = authUserLoginService.TouyanAuthUserLoginLandingInfo;
                if (landingInfo == null) return null;

                var respResult = new RequestAddFavoriteFolderResp();

                var siip = new StubInterfaceInteractParams { TransportConnectTimeoutMS = 2000, TransportReadTimeoutMS = 2000 };
                var resp = XqThriftLibConfigurationManager.SharedInstance.XiaohaChartTerminalAoHttpStub
                    .addFavoriteFolder(landingInfo, favoriteInfo, siip);
                respResult.Resp = resp;

                var returnInfo = resp?.CorrectResult;
                if (resp == null || resp.SourceException != null || returnInfo == null)
                {
                    return respResult;
                }

                lock (favoriteListLock)
                {
                    if (authUserLoginService.TouyanAuthUserLoginLandingInfo != null)
                    {
                        UnsafeAddOrUpdateFavoriteFolders(new FavoriteFolder[] { returnInfo }, false,
                            out IEnumerable<ChartFavoriteListItem_Folder> _addList,
                            out IEnumerable<ChartFavoriteListItem_Folder> _updateList);

                        var addedItem = _addList?.FirstOrDefault(i => i.FavoriteId == returnInfo.FolderId);
                        var tarFavorItem = addedItem ?? _updateList?.FirstOrDefault(i => i.FavoriteId == returnInfo.FolderId);

                        respResult.FavoriteItem = tarFavorItem;
                        respResult.IsNewAdded = addedItem != null;

                        if (addedItem != null)
                        {
                            TouyanChartFavoriteItemAdded?.Invoke(addedItem);
                        }
                    }
                }
                
                return respResult;
            });
        }

        public Task<RequestAddFavoriteChartResp> RequestAddFavoriteChart(FavoriteChart favoriteInfo)
        {
            return Task.Run(() =>
            {
                var landingInfo = authUserLoginService.TouyanAuthUserLoginLandingInfo;
                if (landingInfo == null) return null;

                var respResult = new RequestAddFavoriteChartResp();

                var siip = new StubInterfaceInteractParams { TransportConnectTimeoutMS = 2000, TransportReadTimeoutMS = 2000 };
                var resp = XqThriftLibConfigurationManager.SharedInstance.XiaohaChartTerminalAoHttpStub
                    .addFavoriteChart(landingInfo, favoriteInfo, siip);
                respResult.Resp = resp;

                var returnInfo = resp?.CorrectResult;
                if (resp == null || resp.SourceException != null || returnInfo == null)
                {
                    return respResult;
                }
                
                lock (favoriteListLock)
                {
                    if (authUserLoginService.TouyanAuthUserLoginLandingInfo != null)
                    {
                        UnsafeAddOrUpdateFavoriteCharts(new FavoriteChart[] { returnInfo }, false,
                            out IEnumerable<ChartFavoriteListItem_Chart> _addList,
                            out IEnumerable<ChartFavoriteListItem_Chart> _updateList);

                        var addedItem = _addList?.FirstOrDefault(i => i.FavoriteId == returnInfo.FavoriteChartId);
                        var tarFavorItem = addedItem ?? _updateList?.FirstOrDefault(i => i.FavoriteId == returnInfo.FavoriteChartId);

                        respResult.FavoriteItem = tarFavorItem;
                        respResult.IsNewAdded = addedItem != null;

                        if (addedItem != null)
                        {
                            TouyanChartFavoriteItemAdded?.Invoke(addedItem);
                        }
                    }
                }
                
                return respResult;
            });
        }

        public event TouyanChartFavoriteItemRemoved TouyanChartFavoriteItemRemoved;

        public Task<IInterfaceInteractResponse> RequestRemoveFavoriteFolder(long folderFavorId)
        {
            return Task.Run(() =>
            {
                var landingInfo = authUserLoginService.TouyanAuthUserLoginLandingInfo;
                if (landingInfo == null) return null;

                var siip = new StubInterfaceInteractParams { TransportConnectTimeoutMS = 2000, TransportReadTimeoutMS = 2000 };
                var resp = XqThriftLibConfigurationManager.SharedInstance.XiaohaChartTerminalAoHttpStub
                    .removeFavoriteFolder(landingInfo, folderFavorId, siip);
                
                if (resp != null && resp.SourceException == null)
                {
                    lock (favoriteListLock)
                    {
                        var currentItem = UnsafeGetFavoriteItem(ChartFolderListItemType.Folder, folderFavorId);
                        var recursiveChildren = UnsafeGetChildrenItems(folderFavorId, true);

                        // remove self
                        UnsafeRemoveFavoriteItem(ChartFolderListItemType.Folder, folderFavorId);
                        // remove children
                        foreach (var child in recursiveChildren)
                        {
                            UnsafeRemoveFavoriteItem(child.ItemType, child.FavoriteId);
                        }
                        
                        if (currentItem != null)
                        {
                            TouyanChartFavoriteItemRemoved?.Invoke(currentItem);
                        }
                    }
                }

                return resp;
            });
        }

        public Task<IInterfaceInteractResponse> RequestRemoveFavoriteChart(long chartFavorId)
        {
            return Task.Run(() =>
            {
                var landingInfo = authUserLoginService.TouyanAuthUserLoginLandingInfo;
                if (landingInfo == null) return null;

                var siip = new StubInterfaceInteractParams { TransportConnectTimeoutMS = 2000, TransportReadTimeoutMS = 2000 };
                var resp = XqThriftLibConfigurationManager.SharedInstance.XiaohaChartTerminalAoHttpStub
                    .removeFavoriteChart(landingInfo, chartFavorId, siip);

                if (resp != null && resp.SourceException == null)
                {
                    lock (favoriteListLock)
                    {
                        var currentItem = UnsafeGetFavoriteItem(ChartFolderListItemType.Chart, chartFavorId);

                        // remove self
                        UnsafeRemoveFavoriteItem(ChartFolderListItemType.Chart, chartFavorId);

                        if (currentItem != null)
                        {
                            TouyanChartFavoriteItemRemoved?.Invoke(currentItem);
                        }
                    }
                }

                return resp;
            });
        }

        public event TouyanChartFavoriteItemMoved TouyanChartFavoriteItemMoved;

        public Task<IInterfaceInteractResponse> RequestMoveFavoriteFolder(long folderFavorId, long targetParentFolderId)
        {
            return Task.Run(() =>
            {
                var landingInfo = authUserLoginService.TouyanAuthUserLoginLandingInfo;
                if (landingInfo == null) return null;

                var siip = new StubInterfaceInteractParams { TransportConnectTimeoutMS = 5000, TransportReadTimeoutMS = 5000 };
                var resp = XqThriftLibConfigurationManager.SharedInstance.XiaohaChartTerminalAoHttpStub
                    .moveFavoriteFolder(landingInfo, folderFavorId, targetParentFolderId, siip);

                if (resp != null && resp.SourceException == null)
                {
                    long? originParentFolderId = null;
                    lock (favoriteListLock)
                    {
                        originParentFolderId = UnsafeGetFavoriteItem(ChartFolderListItemType.Folder, folderFavorId)?.ParentFavoriteFolderId;
                    }

                    var refreshTask = RefreshSpecifiedFolderFavoriteItems(folderFavorId);
                    refreshTask.Wait();

                    ChartFavoriteListItem currentItem = null;
                    lock (favoriteListLock)
                    {
                        currentItem = UnsafeGetFavoriteItem(ChartFolderListItemType.Folder, folderFavorId);
                    }
                    if (originParentFolderId != null && currentItem != null)
                    {
                        TouyanChartFavoriteItemMoved?.Invoke(currentItem, originParentFolderId.Value);
                    }
                }
                return resp;
            });
        }

        public Task<IInterfaceInteractResponse> RequestMoveFavoriteChart(long chartFavorId, long targetParentFolderId)
        {
            return Task.Run(() =>
            {
                var landingInfo = authUserLoginService.TouyanAuthUserLoginLandingInfo;
                if (landingInfo == null) return null;

                var siip = new StubInterfaceInteractParams { TransportConnectTimeoutMS = 2000, TransportReadTimeoutMS = 2000 };
                var resp = XqThriftLibConfigurationManager.SharedInstance.XiaohaChartTerminalAoHttpStub
                    .moveFavoriteChart(landingInfo, chartFavorId, targetParentFolderId, siip);

                if (resp != null && resp.SourceException == null)
                {
                    long? originParentFolderId = null;
                    lock (favoriteListLock)
                    {
                        originParentFolderId = UnsafeGetFavoriteItem(ChartFolderListItemType.Chart, chartFavorId)?.ParentFavoriteFolderId;
                    }

                    var refreshTask = RefreshSpecifiedChartFavoriteItems(chartFavorId);
                    refreshTask.Wait();

                    ChartFavoriteListItem currentItem = null;
                    lock (favoriteListLock)
                    {
                        currentItem = UnsafeGetFavoriteItem(ChartFolderListItemType.Chart, chartFavorId);
                    }
                    if (originParentFolderId != null && currentItem != null)
                    {
                        TouyanChartFavoriteItemMoved?.Invoke(currentItem, originParentFolderId.Value);
                    }
                }
                return resp;
            });
        }

        public Task<IInterfaceInteractResponse> RequestRenameFavoriteFolder(long folderFavorId, string newName)
        {
            return Task.Run(() =>
            {
                var landingInfo = authUserLoginService.TouyanAuthUserLoginLandingInfo;
                if (landingInfo == null) return null;

                var siip = new StubInterfaceInteractParams { TransportConnectTimeoutMS = 2000, TransportReadTimeoutMS = 2000 };
                var resp = XqThriftLibConfigurationManager.SharedInstance.XiaohaChartTerminalAoHttpStub
                    .renameFavoriteFolder(landingInfo, folderFavorId, newName, siip);

                if (resp != null && resp.SourceException == null)
                {
                    var refreshTask = RefreshSpecifiedFolderFavoriteItems(folderFavorId);
                    refreshTask.Wait();
                }
                return resp;
            });
        }

        #endregion

        private void RecvTouyanAuthUserHasLogouted(XiaohaChartLandingInfo lastLoginLandingInfo)
        {
            CancelFavoriteListRefresh();
            ClearFavoriteList();
        }

        private void RecvTouyanAuthUserHasLogined()
        {
            RefreshFavoriteListIfNeed();
        }
        
        private Task RefreshSpecifiedFolderFavoriteItems(params long[] folderFavorIds)
        {
            var refreshFavorIds = folderFavorIds?.ToArray();
            return favorListSyncTaskFactory.StartNew(() =>
            {
                if (refreshFavorIds?.Any() != true) return;
                var landingInfo = authUserLoginService.TouyanAuthUserLoginLandingInfo;
                if (landingInfo == null) return;
                
                var queryOption = new ReqFavoriteFolderOption { FolderIds = new THashSet<long>() };
                queryOption.FolderIds.AddRange(refreshFavorIds);

                var clt = AcquireFavoriteListRefreshCLT();
                var queryResp = XqThriftLibConfigurationManager.SharedInstance.XiaohaChartTerminalAoHttpStub
                    .reqFavoriteFolder(landingInfo, queryOption);
                
                lock (favoriteListLock)
                {
                    if (!clt.IsCancellationRequested)
                    {
                        var queryList = queryResp?.CorrectResult;
                        if (queryList != null)
                        {
                            UnsafeAddOrUpdateFavoriteFolders(queryList, true,
                                out IEnumerable<ChartFavoriteListItem_Folder> _addList,
                                out IEnumerable<ChartFavoriteListItem_Folder> _updateList);
                        }
                    }
                }
            });
        }

        private Task RefreshSpecifiedChartFavoriteItems(params long[] chartFavorIds)
        {
            var refreshFavorIds = chartFavorIds?.ToArray();
            return favorListSyncTaskFactory.StartNew(() =>
            {
                if (refreshFavorIds?.Any() != true) return;
                var landingInfo = authUserLoginService.TouyanAuthUserLoginLandingInfo;
                if (landingInfo == null) return;

                var queryOption = new ReqFavoriteChartOption { FavoriteChartIds = new THashSet<long>() };
                queryOption.FavoriteChartIds.AddRange(refreshFavorIds);

                var clt = AcquireFavoriteListRefreshCLT();
                var queryResp = XqThriftLibConfigurationManager.SharedInstance.XiaohaChartTerminalAoHttpStub
                    .reqFavoriteChart(landingInfo, queryOption);

                lock (favoriteListLock)
                {
                    if (!clt.IsCancellationRequested)
                    {
                        var queryList = queryResp?.CorrectResult;
                        if (queryList != null)
                        {
                            UnsafeAddOrUpdateFavoriteCharts(queryList, true,
                                out IEnumerable<ChartFavoriteListItem_Chart> _addList,
                                out IEnumerable<ChartFavoriteListItem_Chart> _updateList);
                        }
                    }
                }
            });
        }

        private Task<ServerDataRefreshResultWrapper<IEnumerable<ChartFavoriteListItem>>> 
            RefreshFavoriteList(bool isForceRefresh)
        {
            if (isForceRefresh)
            {
                CancelFavoriteListRefresh();
            }

            var clt = AcquireFavoriteListRefreshCLT();
            return favorListSyncTaskFactory.StartNew(() => 
            {
                var resultWrapper = new ServerDataRefreshResultWrapper<IEnumerable<ChartFavoriteListItem>>();
                if (clt.IsCancellationRequested)
                {
                    resultWrapper.RefreshState = DataRefreshState.CanceledRefresh;
                    return resultWrapper;
                }

                bool needRefresh = true;
                if (!isForceRefresh && IsRefreshingOrSuccessRefreshed(SafeGetFavoriteListRefreshState()))
                {
                    needRefresh = false;
                }

                resultWrapper.HasRequestRefresh = needRefresh;
                if (needRefresh == false)
                {
                    lock (favoriteListLock)
                    {
                        resultWrapper.ResultData = favoriteList.Values.ToArray();
                    }
                }
                else
                {
                    DataRefreshState change2State = DataRefreshState.Refreshing;
                    SafeSetFavoriteListRefreshState(change2State);
                    FavoriteListRefreshStateChanged?.Invoke(change2State);

                    QueryAllFavoriteItems(
                        out IInterfaceInteractResponse _queryFavorFolderResp,
                        out IEnumerable<FavoriteFolder> _favorFolderList,
                        out IInterfaceInteractResponse _queryFavorChartResp,
                        out IEnumerable<FavoriteChart> _favorChartList);

                    var querySuccess = (_queryFavorFolderResp != null && _queryFavorFolderResp.SourceException == null
                            && _queryFavorChartResp != null && _queryFavorChartResp.SourceException == null);

                    change2State = DataRefreshState.NotRefresh;
                    if (clt.IsCancellationRequested)
                    {
                        change2State = DataRefreshState.CanceledRefresh;
                    }
                    else
                    {
                        change2State = querySuccess ? DataRefreshState.SuccessRefreshed : DataRefreshState.FailedRefreshed;
                    }

                    var respList = new List<IInterfaceInteractResponse>();
                    if (_queryFavorFolderResp != null) respList.Add(_queryFavorFolderResp);
                    if (_queryFavorChartResp != null) respList.Add(_queryFavorChartResp);

                    resultWrapper.RefreshRequestResp = respList.ToArray();

                    var newFavorList = new List<ChartFavoriteListItem>();
                    lock (favoriteListLock)
                    {
                        // AddOrUpdate
                        if (!clt.IsCancellationRequested)
                        {
                            UnsafteClearFavoriteList();
                            if (_favorFolderList != null)
                            {
                                UnsafeAddOrUpdateFavoriteFolders(_favorFolderList, true,
                                    out IEnumerable<ChartFavoriteListItem_Folder> _addList,
                                    out IEnumerable<ChartFavoriteListItem_Folder> _updateList);
                                if (_addList != null) newFavorList.AddRange(_addList);
                                if (_updateList != null) newFavorList.AddRange(_updateList);
                            }

                            if (_favorChartList != null)
                            {
                                UnsafeAddOrUpdateFavoriteCharts(_favorChartList, true,
                                    out IEnumerable<ChartFavoriteListItem_Chart> _addList,
                                    out IEnumerable<ChartFavoriteListItem_Chart> _updateList);
                                if (_addList != null) newFavorList.AddRange(_addList);
                                if (_updateList != null) newFavorList.AddRange(_updateList);
                            }
                        }
                    }

                    resultWrapper.ResultData = newFavorList;

                    SafeSetFavoriteListRefreshState(change2State);
                    FavoriteListRefreshStateChanged?.Invoke(change2State);
                }

                resultWrapper.RefreshState = SafeGetFavoriteListRefreshState();

                return resultWrapper;
            });
        }

        private void UnsafeAddOrUpdateFavoriteFolders(
            IEnumerable<FavoriteFolder> favorFolders,
            bool updateIfItemExist,
            out IEnumerable<ChartFavoriteListItem_Folder> _addedItems,
            out IEnumerable<ChartFavoriteListItem_Folder> _updatedItems)
        {
            _addedItems = null;
            _updatedItems = null;

            if (favorFolders?.Any() != true) return;

            var addList = new List<ChartFavoriteListItem_Folder>();
            var updateList = new List<ChartFavoriteListItem_Folder>();
            foreach (var item in favorFolders)
            {
                var itemKey = new ChartFavoriteListItemKey(ChartFolderListItemType.Folder, item.FolderId);
                if (favoriteList.TryGetValue(itemKey, out ChartFavoriteListItem _existItem)
                    && _existItem is ChartFavoriteListItem_Folder folderItem)
                {
                    // 存在，则改变内容
                    if (updateIfItemExist)
                    {
                        folderItem.FavoriteInfo = item;
                        folderItem.ParentFavoriteFolderId = item.ParentFolderId;
                    }
                    updateList.Add(folderItem);
                }
                else
                {
                    // 不存在，则添加
                    var newItem = new ChartFavoriteListItem_Folder(item.FolderId)
                    {
                        FavoriteInfo = item,
                        ParentFavoriteFolderId = item.ParentFolderId
                    };

                    favoriteList[itemKey] = newItem;
                    addList.Add(newItem);
                }
            }

            _addedItems = addList;
            _updatedItems = updateList;
        }

        private void UnsafeAddOrUpdateFavoriteCharts(
            IEnumerable<FavoriteChart> favorCharts,
            bool updateIfItemExist,
            out IEnumerable<ChartFavoriteListItem_Chart> _addedItems,
            out IEnumerable<ChartFavoriteListItem_Chart> _updatedItems)
        {
            _addedItems = null;
            _updatedItems = null;

            if (favorCharts?.Any() != true) return;

            var addList = new List<ChartFavoriteListItem_Chart>();
            var updateList = new List<ChartFavoriteListItem_Chart>();
            foreach (var item in favorCharts)
            {
                var itemKey = new ChartFavoriteListItemKey(ChartFolderListItemType.Chart, item.FavoriteChartId);
                if (favoriteList.TryGetValue(itemKey, out ChartFavoriteListItem _existItem)
                    && _existItem is ChartFavoriteListItem_Chart chartItem)
                {
                    // 存在，则改变内容
                    if (updateIfItemExist)
                    {
                        chartItem.FavoriteInfo = item;
                        chartItem.ParentFavoriteFolderId = item.ParentFolderId;
                    }
                    updateList.Add(chartItem);
                }
                else
                {
                    // 不存在，则添加
                    var newItem = new ChartFavoriteListItem_Chart(item.FavoriteChartId, item.XiaohaChartId)
                    {
                        FavoriteInfo = item,
                        ParentFavoriteFolderId = item.ParentFolderId
                    };

                    // 获取 Chart info
                    DMDataLoadHelper.LoadChartInfo(newItem, chartQueryCtrl, false);

                    favoriteList[itemKey] = newItem;
                    addList.Add(newItem);
                }
            }

            _addedItems = addList;
            _updatedItems = updateList;
        }
        
        private void UnsafteClearFavoriteList()
        {
            favoriteList.Clear();
        }

        private void ClearFavoriteList()
        {
            lock (favoriteListLock)
            {
                UnsafteClearFavoriteList();
            }
        }

        private bool UnsafeRemoveFavoriteItem(ChartFolderListItemType itemType, long favoriteId)
        {
            return favoriteList.Remove(new ChartFavoriteListItemKey(itemType, favoriteId));
        }

        private ChartFavoriteListItem UnsafeGetFavoriteItem(ChartFolderListItemType itemType, long favoriteId)
        {
            ChartFavoriteListItem favorItem = null;
            favoriteList.TryGetValue(new ChartFavoriteListItemKey(itemType, favoriteId), out favorItem);
            return favorItem;
        }

        private IEnumerable<ChartFavoriteListItem> UnsafeGetChildrenItems(long rootFavorFolderId, bool recursive)
        {
            var resultList = new List<ChartFavoriteListItem>();
            var children = favoriteList.Values.Where(i => i.ParentFavoriteFolderId == rootFavorFolderId).ToArray();
            resultList.AddRange(children);

            if (recursive && children.Any())
            {
                foreach (var c in children)
                {
                    if (c.ItemType == ChartFolderListItemType.Folder)
                    {
                        var grandChildren = UnsafeGetChildrenItems(c.FavoriteId, recursive);
                        resultList.AddRange(grandChildren);
                    }
                }
            }

            return resultList;
        }

        private DataRefreshState SafeGetFavoriteListRefreshState()
        {
            DataRefreshState state;
            lock (favoriteListRefreshLock)
            {
                state = favoriteListRefreshState;
            }
            return state;
        }

        private void SafeSetFavoriteListRefreshState(DataRefreshState state)
        {
            lock (favoriteListRefreshLock)
            {
                favoriteListRefreshState = state;
            }
        }

        private static bool IsRefreshingOrSuccessRefreshed(DataRefreshState subAccountLastRefreshState)
        {
            return subAccountLastRefreshState == DataRefreshState.Refreshing || subAccountLastRefreshState == DataRefreshState.SuccessRefreshed;
        }

        private CancellationToken AcquireFavoriteListRefreshCLT()
        {
            CancellationToken clt = CancellationToken.None;
            lock (favoriteListRefreshLock)
            {
                if (favoriteListRefreshCTS == null)
                {
                    favoriteListRefreshCTS = new CancellationTokenSource();
                }
                clt = favoriteListRefreshCTS.Token;
            }
            return clt;
        }

        private void CancelFavoriteListRefresh()
        {
            lock (favoriteListRefreshLock)
            {
                if (favoriteListRefreshCTS != null)
                {
                    favoriteListRefreshCTS.Cancel();
                    favoriteListRefreshCTS.Dispose();
                    favoriteListRefreshCTS = null;
                }
            }
        }

        private void QueryAllFavoriteItems(
            out IInterfaceInteractResponse _queryFavorFolderResp,
            out IEnumerable<FavoriteFolder> _favorFolderList,
            out IInterfaceInteractResponse _queryFavorChartResp,
            out IEnumerable<FavoriteChart> _favorChartList)
        {
            _queryFavorFolderResp = null;
            _favorFolderList = null;
            _queryFavorChartResp = null;
            _favorChartList = null;

            var landingInfo = authUserLoginService.TouyanAuthUserLoginLandingInfo;
            if (landingInfo == null) return;
            
            IInterfaceInteractResponse<List<FavoriteFolder>> favorFoldersResp = null;
            IInterfaceInteractResponse<List<FavoriteChart>> favorChartsResp = null;
           
            // 查询所有收藏夹
            var t1 = Task.Run(() => 
            {
                var option = new ReqFavoriteFolderOption();
                favorFoldersResp = XqThriftLibConfigurationManager.SharedInstance.XiaohaChartTerminalAoHttpStub
                    .reqFavoriteFolder(landingInfo, option);
            });

            // 查询所有收藏图片
            var t2 = Task.Run(() => 
            {
                var option = new ReqFavoriteChartOption();
                favorChartsResp = XqThriftLibConfigurationManager.SharedInstance.XiaohaChartTerminalAoHttpStub
                    .reqFavoriteChart(landingInfo, option);
            });

            Task.WaitAll(t1, t2);

            _queryFavorFolderResp = favorFoldersResp;
            _favorFolderList = favorFoldersResp?.CorrectResult;

            _queryFavorChartResp = favorChartsResp;
            _favorChartList = favorChartsResp?.CorrectResult;
        }
    }
}
