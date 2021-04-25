using lib.xqclient_base.logger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Schedulers;
using System.Windows.Threading;
using System.Xml.Serialization;
using Touyan.app.helper;
using Touyan.Interface.application;
using Touyan.Interface.datamodel;
using Touyan.Interface.model;
using Touyan.Interface.service;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Model;

namespace Touyan.app.servicecontroller
{
    [Export(typeof(ITouyanChartViewHistoryServiceCtrl)), Export(typeof(ITouyanModuleServiceCtrl)), PartCreationPolicy(CreationPolicy.Shared)]
    internal class TouyanChartViewHistoryServiceCtrl : ITouyanChartViewHistoryServiceCtrl, ITouyanModuleServiceCtrl
    {
        private const int ChartViewHistoryLimitSize = 500;

        private static readonly string ChartViewHistroyFilePath = Path.Combine(PathHelper.AppSetupDirectoryPath, "touyan_chart_view_history.json");
       
        private readonly ITouyanChartViewHistoryService chartViewHistoryService;
        private readonly ITouyanChartQueryCtrl chartQueryCtrl;

        /// <summary>
        /// 列表同步任务工厂。该工厂生产的任务是顺序执行，将增删、刷新包装成任务执行，以保证同步的准确性
        /// </summary>
        private readonly TaskFactory historyListSyncTaskFactory = new TaskFactory(new OrderedTaskScheduler());

        private readonly Dictionary<long, ChartViewHistoryDM> historyList = new Dictionary<long, ChartViewHistoryDM>();
        private readonly object historyListLock = new object();

        private CancellationTokenSource historyListSyncCTS;
        private readonly object historyListSyncLock = new object();
        private DataRefreshState historyListRefreshState = DataRefreshState.NotRefresh;

        private readonly object diskHistoryFileOpreateLock = new object();
        
        [ImportingConstructor]
        public TouyanChartViewHistoryServiceCtrl(ITouyanChartViewHistoryService chartViewHistoryService,
            ITouyanChartQueryCtrl chartQueryCtrl)
        {
            this.chartViewHistoryService = chartViewHistoryService;
            this.chartQueryCtrl = chartQueryCtrl;
        }

        public void Initialize()
        {

        }

        public void Shutdown()
        {
            CancelHistoryListSycn();
        }

        #region ITouyanChartViewHistoryServiceCtrl
        
        public event TouyanChartViewHistoryListRefreshStateChanged HistoryListRefreshStateChanged;

        public Task<LocalDataRefreshResultWrapper<ChartViewHistoryDM[]>> RefreshHistoryListIfNeed()
        {
            return RefreshHistoryList(false);
        }

        public Task<LocalDataRefreshResultWrapper<ChartViewHistoryDM[]>> RefreshHistoryListForce()
        {
            return RefreshHistoryList(true);
        }

        public event TouyanChartViewHistoryAdded ViewHistoryAdded;

        public Task<ChartViewHistoryDM[]> RequestAddOrUpdateHistoryItem(ChartViewHistory[] historyItems, bool resetContentWhenOriginContentLost)
        {
            var clt = AcquireHistoryListSyncCLT();
            return historyListSyncTaskFactory.StartNew(() => 
            {
                if (clt.IsCancellationRequested) return null;
                if (historyItems?.Any() != true) return null;

                List<ChartViewHistoryDM> addOrUpdateItems = null;

                var diskOptExp = AddOrUpdateHistoryInDiskFile(historyItems,
                    ChartViewHistoryLimitSize, resetContentWhenOriginContentLost, 
                    out ChartViewHistory[] _abandonItems);
                if (diskOptExp != null)
                {
                    AppLog.Error("Failed to AddOrUpdateHistoryInDiskFile.", diskOptExp);
                }
                else
                {
                    lock (historyListLock)
                    {
                        // 删除舍弃的项
                        if (_abandonItems?.Any() == true)
                        {
                            UnsafeRemoveHistoryItem(_abandonItems.Select(i => i.ChartId).ToArray());
                        }

                        addOrUpdateItems = new List<ChartViewHistoryDM>();

                        UnsafeAddOrUpdateHistoryItems(historyItems, true,
                            out ChartViewHistoryDM[] _addList, out ChartViewHistoryDM[] _updateList);

                        if (_addList?.Any() == true) addOrUpdateItems.AddRange(_addList);
                        if (_updateList?.Any() == true) addOrUpdateItems.AddRange(_updateList);

                        if (_addList?.Any() == true)
                        {
                            ViewHistoryAdded?.Invoke(_addList);
                        }
                    }
                }

                return addOrUpdateItems?.ToArray();
            });
        }

        public event TouyanChartViewHistoryRemoved ViewHistoryRemoved;

        public Task<Exception> RequestRemoveHistoryItem(params long[] chartIds)
        {
            var clt = AcquireHistoryListSyncCLT();
            return historyListSyncTaskFactory.StartNew(() =>
            {
                if (clt.IsCancellationRequested) return null;
                if (chartIds?.Any() != true) return null;

                Exception rmException = null;

                var diskOptExp = RemoveHistoryInDiskFile(chartIds);
                if (diskOptExp != null)
                {
                    AppLog.Error("Failed to RemoveHistoryInDiskFile.", diskOptExp);
                    rmException = diskOptExp;
                }
                else
                {
                    lock (historyListLock)
                    {
                        UnsafeRemoveHistoryItem(chartIds);
                        ViewHistoryRemoved?.Invoke(chartIds);
                    }
                }

                return rmException;
            });
        }

        public event TouyanChartViewHistoryListCleared ViewHistoryListCleared;

        public Task<Exception> RequestClearHistoryList()
        {
            var clt = AcquireHistoryListSyncCLT();
            return historyListSyncTaskFactory.StartNew(() =>
            {
                if (clt.IsCancellationRequested) return null;

                Exception clearException = null;

                var diskOptExp = ClearHistoryInDiskFile();
                if (diskOptExp != null)
                {
                    AppLog.Error("Failed to ClearHistoryInDiskFile.", diskOptExp);
                    clearException = diskOptExp;
                }
                else
                {
                    lock (historyListLock)
                    {
                        UnsafeClearHistoryList();
                        ViewHistoryListCleared?.Invoke();
                    }
                }

                return clearException;
            });
        }

        #endregion
        
        private void SaveChartViewHistoryListToDisk(ChartViewHistoryList historyListObj, out Exception _e)
        {
            _e = null;
            var filePath = ChartViewHistroyFilePath;

            // xml solution
            /*
            try
            {
                // Insert code to set properties and fields of the object.  
                var xmlSerializer = new XmlSerializer(typeof(ChartViewHistoryList));
                using (var writeStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    xmlSerializer.Serialize(writeStream, historyListObj);
                }
            }
            catch (Exception e)
            {
                _e = e;
            }
            */

            // json solution
            // serialize JSON to a file
            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        var serializeErrors = new List<Exception>();
                        var serializer = new JsonSerializer();
                        serializer.Error += (s, args) =>
                        {
                            // only log an error once
                            if (args.CurrentObject == args.ErrorContext.OriginalObject)
                            {
                                serializeErrors.Add(args.ErrorContext.Error);
                            }
                        };

                        serializer.Serialize(writer, historyListObj);

                        if (serializeErrors.Count > 0)
                        {
                            _e = serializeErrors.First();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _e = e;
            }
        }

        /// <summary>
        /// 从磁盘中获取持久化的历史列表
        /// </summary>
        /// <param name="ignoreFileNotFoundException">是否忽略 FileNotFoundException。如果为 true，发生该异常时，该异常不会返回</param>
        /// <param name="_e">如果存在异常，则返回的异常</param>
        /// <param name="_existDeserializeException">是否存在反序列化异常</param>
        /// <returns></returns>
        private ChartViewHistoryList GetChartViewHistoryListFromDisk(
            bool ignoreFileNotFoundException, 
            out Exception _e,
            out bool _existDeserializeException)
        {
            _e = null;
            _existDeserializeException = false;
            
            var filePath = ChartViewHistroyFilePath;

            // xml solution
            /*
            try
            {
                // Warning:考虑文件已经是写锁住时，将 FileShare 设置为 ReadWrite，才能让锁住的写继续写。
                // By creating a FileStream object and setting FileShare.ReadWrite I am saying that I want to open the file in a mode that allows other files to Read and Write to/from the file while I have it opened.
                // Refer: https://stackoverflow.com/questions/6167136/how-to-copy-a-file-while-it-is-being-used-by-another-process
                // http://coding.infoconex.com/post/2009/04/21/How-do-I-open-a-file-that-is-in-use-in-C
                using (var readStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    var xmlSerializer = new XmlSerializer(typeof(ChartViewHistoryList));
                    try
                    {
                        var data = xmlSerializer.Deserialize(readStream);
                        return data as ChartViewHistoryList;
                    }
                    catch (Exception e)
                    {
                        _existDeserializeException = true;
                        _e = e;
                        return null;
                    }
                }
            }
            catch (FileNotFoundException fnfe)
            {
                if (!ignoreFileNotFoundException)
                {
                    _e = fnfe;
                }
                return null;
            }
            catch (Exception e)
            {
                _e = e;
                return null;
            }
            */

            // json solution
            try
            {
                // Warning:考虑文件已经是写锁住时，将 FileShare 设置为 ReadWrite，才能让锁住的写继续写。
                // By creating a FileStream object and setting FileShare.ReadWrite I am saying that I want to open the file in a mode that allows other files to Read and Write to/from the file while I have it opened.
                // Refer: https://stackoverflow.com/questions/6167136/how-to-copy-a-file-while-it-is-being-used-by-another-process
                // http://coding.infoconex.com/post/2009/04/21/How-do-I-open-a-file-that-is-in-use-in-C
                using (var readStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (var reader = new StreamReader(readStream))
                    {
                        var serializeErrors = new List<Exception>();
                        var serializer = new JsonSerializer();
                        serializer.Error += (s, args) =>
                        {
                            // only log an error once
                            if (args.CurrentObject == args.ErrorContext.OriginalObject)
                            {
                                serializeErrors.Add(args.ErrorContext.Error);
                            }
                        };

                        var data = serializer.Deserialize(reader, typeof(ChartViewHistoryList));

                        if (serializeErrors.Count > 0)
                        {
                            _existDeserializeException = true;
                            _e = serializeErrors.First();
                        }

                        return data as ChartViewHistoryList;
                    }
                }
            }
            catch (FileNotFoundException fnfe)
            {
                if (!ignoreFileNotFoundException)
                {
                    _e = fnfe;
                }
                return null;
            }
            catch (Exception e)
            {
                _e = e;
                return null;
            }
        }

        /// <summary>
        /// 增加或修改历史项
        /// </summary>
        /// <param name="addOrUpdateItems">要添加或修改的项</param>
        /// <param name="historyListLimitSize">历史数量限定。如果超出限定，则舍去时间更早的项。如果不限定，设置为小于 0 的数值</param>
        /// <param name="resetContentWhenDeserializeOriginContentFailed">当获取并反序列化原内容失败时，是否重置内容</param>
        /// <param name="abandonItems">由于数量限定，已舍弃的项<</param>
        /// <returns></returns>
        private Exception AddOrUpdateHistoryInDiskFile( 
            ChartViewHistory[] addOrUpdateItems,
            int historyListLimitSize,
            bool resetContentWhenDeserializeOriginContentFailed,
            out ChartViewHistory[] _abandonItems)
        {
            _abandonItems = null;

            if (addOrUpdateItems?.Any() != true) return null;
            Exception occurExp = null;
            lock (diskHistoryFileOpreateLock)
            {
                var historyListObj = GetChartViewHistoryListFromDisk(true, out Exception _getExp, out bool _existDeserializeExp);
                var continueAction = _getExp == null || (_existDeserializeExp && resetContentWhenDeserializeOriginContentFailed);
                if (!continueAction)
                {
                    occurExp = _getExp;
                }
                else
                {
                    var toSaveHistoryList = historyListObj?.HistoryList?.ToList() ?? new List<ChartViewHistory>();
                    foreach (var item in addOrUpdateItems)
                    {
                        toSaveHistoryList.RemoveAll(i => i.ChartId == item.ChartId);
                        toSaveHistoryList.Add(item);
                    }

                    var listCount = toSaveHistoryList.Count;
                    if (listCount > historyListLimitSize)
                    {
                        var rmIndex = historyListLimitSize;
                        toSaveHistoryList = toSaveHistoryList.OrderByDescending(i => i.ViewTimestamp).ToList();

                        _abandonItems = toSaveHistoryList.GetRange(rmIndex, listCount - rmIndex).ToArray();

                        toSaveHistoryList.RemoveRange(rmIndex, listCount - rmIndex);
                    }

                    SaveChartViewHistoryListToDisk(new ChartViewHistoryList { HistoryList = toSaveHistoryList.ToArray() }, out Exception _saveExp);
                    if (_saveExp != null)
                    {
                        occurExp = _saveExp;
                        _abandonItems = null;
                    }
                }
            }

            return occurExp;
        }

        private Exception RemoveHistoryInDiskFile(params long[] rmItemIds)
        {
            if (rmItemIds?.Any() != true) return null;
            Exception occurExp = null;
            lock (diskHistoryFileOpreateLock)
            {
                var historyListObj = GetChartViewHistoryListFromDisk(true, out Exception _getExp, out bool _existDeserializeException);
                var querySuccess = (_getExp == null);
                if (!querySuccess)
                {
                    occurExp = _getExp;
                }
                else
                {
                    var toSaveHistoryList = historyListObj?.HistoryList?.ToList() ?? new List<ChartViewHistory>();
                    foreach (var _id in rmItemIds)
                    {
                        toSaveHistoryList.RemoveAll(i => i.ChartId == _id);
                    }

                    SaveChartViewHistoryListToDisk(new ChartViewHistoryList { HistoryList = toSaveHistoryList.ToArray() }, out Exception _saveExp);
                    if (_saveExp != null)
                    {
                        occurExp = _saveExp;
                    }
                }
            }
            return occurExp;
        }

        private Exception ClearHistoryInDiskFile()
        {
            Exception occurExp = null;
            lock (diskHistoryFileOpreateLock)
            {
                SaveChartViewHistoryListToDisk(new ChartViewHistoryList(), out Exception _saveExp);
                if (_saveExp != null)
                {
                    occurExp = _saveExp;
                }
            }
            return occurExp;
        }

        private Task<LocalDataRefreshResultWrapper<ChartViewHistoryDM[]>> RefreshHistoryList(bool isForceRefresh)
        {
            if (isForceRefresh)
            {
                CancelHistoryListSycn();
            }

            var clt = AcquireHistoryListSyncCLT();
            return historyListSyncTaskFactory.StartNew(() =>
            {
                var resultWrapper = new LocalDataRefreshResultWrapper<ChartViewHistoryDM[]>();
                if (clt.IsCancellationRequested)
                {
                    resultWrapper.RefreshState = DataRefreshState.CanceledRefresh;
                    return resultWrapper;
                }

                bool needRefresh = true;
                if (!isForceRefresh && IsRefreshingOrSuccessRefreshed(SafeGetHistoryListRefreshState()))
                {
                    needRefresh = false;
                }

                resultWrapper.HasRequestRefresh = needRefresh;
                if (needRefresh == false)
                {
                    lock (historyListLock)
                    {
                        resultWrapper.ResultData = historyList.Values.ToArray();
                    }
                }
                else
                {
                    DataRefreshState change2State = DataRefreshState.Refreshing;
                    SafeSetHistoryListRefreshState(change2State);
                    HistoryListRefreshStateChanged?.Invoke(change2State);

                    ChartViewHistoryList historyListObj = null;
                    Exception _getExp = null;
                    lock (diskHistoryFileOpreateLock)
                    {
                        historyListObj = GetChartViewHistoryListFromDisk(true, out _getExp, out bool _existDeserializeException);
                        if (_getExp != null)
                        {
                            AppLog.Error("Failed to GetChartViewHistoryListFromDisk.", _getExp);
                        }
                    }
                    var querySuccess = (_getExp == null);
                    
                    change2State = DataRefreshState.NotRefresh;
                    if (clt.IsCancellationRequested)
                    {
                        change2State = DataRefreshState.CanceledRefresh;
                    }
                    else
                    {
                        change2State = querySuccess ? DataRefreshState.SuccessRefreshed : DataRefreshState.FailedRefreshed;
                    }

                    if (_getExp != null)
                    {
                        resultWrapper.RequestExceptions = new Exception[] { _getExp };
                    }

                    var newHistoryList = new List<ChartViewHistoryDM>();
                    lock (historyListLock)
                    {
                        // AddOrUpdate
                        if (!clt.IsCancellationRequested)
                        {
                            UnsafeClearHistoryList();
                            var historyList = historyListObj?.HistoryList?.ToArray();
                            if (historyList != null)
                            {
                                UnsafeAddOrUpdateHistoryItems(historyList, true,
                                    out ChartViewHistoryDM[] _addList,
                                    out ChartViewHistoryDM[] _updateList);
                                if (_addList != null) newHistoryList.AddRange(_addList);
                                if (_updateList != null) newHistoryList.AddRange(_updateList);
                            }
                        }
                    }

                    resultWrapper.ResultData = newHistoryList?.ToArray();

                    SafeSetHistoryListRefreshState(change2State);
                    HistoryListRefreshStateChanged?.Invoke(change2State);
                }

                resultWrapper.RefreshState = SafeGetHistoryListRefreshState();

                return resultWrapper;
            });
        }

        private void UnsafeClearHistoryList()
        {
            historyList.Clear();
            DispatcherHelper.CheckBeginInvokeOnUI(() => 
            {
                chartViewHistoryService.HistoryItems.Clear();
            }, DispatcherPriority.Normal);
        }

        private void ClearHistoryList()
        {
            lock (historyListLock)
            {
                UnsafeClearHistoryList();
            }
        }

        private void UnsafeAddOrUpdateHistoryItems(ChartViewHistory[] histories,
            bool updateIfItemExist,
            out ChartViewHistoryDM[] _addedItems,
            out ChartViewHistoryDM[] _updatedItems)
        {
            _addedItems = null;
            _updatedItems = null;

            if (histories?.Any() != true) return;

            var addList = new List<ChartViewHistoryDM>();
            var updateList = new List<ChartViewHistoryDM>();
            foreach (var item in histories)
            {
                var chartId = item.ChartId;
                if (historyList.TryGetValue(chartId, out ChartViewHistoryDM existItem))
                {
                    // 存在，则改变内容
                    if (updateIfItemExist)
                    {
                        existItem.ViewTimestamp = item.ViewTimestamp;
                    }
                    updateList.Add(existItem);
                }
                else
                {
                    // 不存在，则添加
                    var newItem = new ChartViewHistoryDM(item.ChartId)
                    {
                        ViewTimestamp = item.ViewTimestamp
                    };
                    
                    // 获取 Chart info
                    DMDataLoadHelper.LoadChartInfo(newItem, chartQueryCtrl, false);

                    historyList[chartId] = newItem;
                    addList.Add(newItem);
                }
            }

            if (addList.Any())
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() => 
                {
                    chartViewHistoryService.HistoryItems.AddRange(addList);
                }, DispatcherPriority.Normal);
            }

            _addedItems = addList?.ToArray();
            _updatedItems = updateList?.ToArray();
        }

        private void UnsafeRemoveHistoryItem(params long[] chartIds)
        {
            if (chartIds?.Any() != true) return;

            var rmList = new List<ChartViewHistoryDM>();
            foreach (var rmId in chartIds)
            {
                if (historyList.TryGetValue(rmId, out ChartViewHistoryDM existItem))
                {
                    historyList.Remove(rmId);
                    rmList.Add(existItem);
                }
            }
            
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                foreach (var rmItem in rmList)
                {
                    chartViewHistoryService.HistoryItems.Remove(rmItem);
                }
            }, DispatcherPriority.Normal);
        }
        
        private DataRefreshState SafeGetHistoryListRefreshState()
        {
            DataRefreshState state;
            lock (historyListSyncLock)
            {
                state = historyListRefreshState;
            }
            return state;
        }

        private void SafeSetHistoryListRefreshState(DataRefreshState state)
        {
            lock (historyListSyncLock)
            {
                historyListRefreshState = state;
            }
        }

        private static bool IsRefreshingOrSuccessRefreshed(DataRefreshState subAccountLastRefreshState)
        {
            return subAccountLastRefreshState == DataRefreshState.Refreshing || subAccountLastRefreshState == DataRefreshState.SuccessRefreshed;
        }

        private CancellationToken AcquireHistoryListSyncCLT()
        {
            CancellationToken clt = CancellationToken.None;
            lock (historyListSyncLock)
            {
                if (historyListSyncCTS == null)
                {
                    historyListSyncCTS = new CancellationTokenSource();
                }
                clt = historyListSyncCTS.Token;
            }
            return clt;
        }

        private void CancelHistoryListSycn()
        {
            lock (historyListSyncLock)
            {
                if (historyListSyncCTS != null)
                {
                    historyListSyncCTS.Cancel();
                    historyListSyncCTS.Dispose();
                    historyListSyncCTS = null;
                }
            }
        }
    }
}
