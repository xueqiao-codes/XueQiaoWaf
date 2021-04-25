using business_foundation_lib.xq_thriftlib_config;
using lib.xqclient_base.thriftapi_mediation.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Schedulers;
using System.Waf.Applications;
using Thrift.Collections;
using Touyan.app.viewmodel;
using Touyan.Interface.application;
using Touyan.Interface.datamodel;
using xueqiao.graph.xiaoha.chart.terminal.ao.thriftapi;
using xueqiao.graph.xiaoha.chart.thriftapi;
using xueqiao.personal.user.thriftapi;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;

namespace Touyan.app.controller
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class ChartDetailContainerViewCtrl : IController
    {
        private const string _404HtmlContent = "<!DOCTYPE HTML PUBLIC \" -//W3C//DTD HTML 4.0 Transitional//EN\">" +
            "<html><head><title>404</title></head><body><H1>Not Found</H1></body></html>";

        private const string _EmptyHtmlContent = "<!DOCTYPE HTML PUBLIC \" -//W3C//DTD HTML 4.0 Transitional//EN\">" +
            "<html><head></head><body></body></html>";

        private readonly ChartDetailContainerVM contentVM;
        private readonly ITouyanAuthUserLoginService authUserLoginService;
        private readonly ITouyanChartQueryCtrl chartQueryCtrl;
        private readonly ITouyanChartFavoriteServiceCtrl chartFavoriteServiceCtrl;
        private readonly ExportFactory<AddFavoriteItemDialogCtrl> addFavoriteItemDialogCtrlFactory;

        private readonly DelegateCommand toggleChartFavoriteCmd;
        private readonly TaskFactory pageDataRefreshTF = new TaskFactory(new OrderedTaskScheduler());

        private CancellationTokenSource pageDataRefreshCLTS;
        private readonly object pageDataRefreshCLTSLock = new object();

        private long? currentChartId;
        private Chart queriedChart;
        private FavoriteChart queriedFavoriteInfo;
        private bool isLoadingPageData;
        private bool isRefreshingFavorInfo;

        [ImportingConstructor]
        public ChartDetailContainerViewCtrl(
            ChartDetailContainerVM contentVM,
            ITouyanAuthUserLoginService authUserLoginService,
            ITouyanChartQueryCtrl chartQueryCtrl,
            ITouyanChartFavoriteServiceCtrl chartFavoriteServiceCtrl,
            ExportFactory<AddFavoriteItemDialogCtrl> addFavoriteItemDialogCtrlFactory)
        {
            this.contentVM = contentVM;
            this.authUserLoginService = authUserLoginService;
            this.chartQueryCtrl = chartQueryCtrl;
            this.chartFavoriteServiceCtrl = chartFavoriteServiceCtrl;
            this.addFavoriteItemDialogCtrlFactory = addFavoriteItemDialogCtrlFactory;

            toggleChartFavoriteCmd = new DelegateCommand(ToggleChartFavorite, CanToggleChartFavorite);
        }
        
        /// <summary>
        /// 图表 id 获取方法
        /// </summary>
        public Func<ChartDetailContainerViewCtrl, long?> ChartIdFactory { get; set; }

        public Action<ChartDetailContainerViewCtrl, long> SearchChartSelectedHandler { get; set; }

        public object ContentView => contentVM.View;

        public void Initialize()
        {
            authUserLoginService.TouyanAuthUserHasLogined += RevcTouyanAuthUserHasLogined;
            authUserLoginService.TouyanAuthUserHasLogouted += RecvTouyanAuthUserHasLogouted;

            contentVM.ToggleChartFavoriteCmd = toggleChartFavoriteCmd;
            contentVM.ChartSearchSuggestionGetter = _filter => QueryChartListByFilter(_filter);

            PropertyChangedEventManager.AddHandler(contentVM, ContentVMPropChanged_SelectedSuggestionChart, nameof(ChartDetailContainerVM.SelectedSuggestionChart));
        }
        
        public void Run()
        {
            InvalidatePageContent();
        }

        public void Shutdown()
        {
            authUserLoginService.TouyanAuthUserHasLogined -= RevcTouyanAuthUserHasLogined;
            authUserLoginService.TouyanAuthUserHasLogouted -= RecvTouyanAuthUserHasLogouted;

            CancelPageDataRefresh();
            PropertyChangedEventManager.RemoveHandler(contentVM, ContentVMPropChanged_SelectedSuggestionChart, nameof(ChartDetailContainerVM.SelectedSuggestionChart));
        }
        
        public void InvalidatePageContent()
        {
            CancelPageDataRefresh();

            var queryChartId = this.ChartIdFactory?.Invoke(this);
            UpdateCurrentChartId(queryChartId);

            if (queryChartId == null || queryChartId <= 0)
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    contentVM.LoadChartWebContentWithHtmlContent(_EmptyHtmlContent);
                });
                return;
            }
            else
            {
                var clt = AcquirePageDataRefreshCLT();
                RefreshPageData(queryChartId.Value, clt);
            }
        }

        private void ContentVMPropChanged_SelectedSuggestionChart(object sender, PropertyChangedEventArgs e)
        {
            var selSuggestionChart = contentVM.SelectedSuggestionChart;
            if (selSuggestionChart != null)
            {
                SearchChartSelectedHandler?.Invoke(this, selSuggestionChart.ChartId);
            }
        }
        
        private void RevcTouyanAuthUserHasLogined()
        {
            RefreshChartFavoriteInfo();
        }

        private void RecvTouyanAuthUserHasLogouted(XiaohaChartLandingInfo lastLoginLandingInfo)
        {
            RefreshChartFavoriteInfo();
        }
        
        private Chart[] QueryChartListByFilter(string filterText)
        {
            filterText = filterText?.Trim();
            if (string.IsNullOrEmpty(filterText)) return null;

            // query suggestion
            var keywords = Regex.Split(filterText, @"\s+").Where(s => s != string.Empty).ToArray();
            var reqOption = new ReqChartOption { Name = filterText, State = ChartState.ENABLE };
            if (keywords?.Any() == true)
            {
                reqOption.KeyWords = new THashSet<string>();
                reqOption.KeyWords.AddRange(keywords);
            }

            // 只查询 200 条记录
            var resp = chartQueryCtrl.RequestQueryChart(reqOption, new IndexedPageOption { PageIndex = 0, PageSize = 200 });
            return resp?.CorrectResult?.Page.ToArray();
        }

        private bool CanToggleChartFavorite()
        {
            return currentChartId != null && currentChartId > 0 && !isLoadingPageData && !isRefreshingFavorInfo;
        }

        private void ToggleChartFavorite()
        {
            if (isRefreshingFavorInfo) return;
            if (authUserLoginService.TouyanAuthUserLoginLandingInfo == null)
            {
                // 弹出登录框
                authUserLoginService.ShowTouyanAuthUserLoginDialog(UIHelper.GetWindowOfUIElement(contentVM.View));
                return;
            }

            var favorInfo = this.queriedFavoriteInfo;
            if (contentVM.ChartIsFavorited && favorInfo != null)
            {
                // 取消收藏操作
                chartFavoriteServiceCtrl.RequestRemoveFavoriteChart(favorInfo.FavoriteChartId)
                .ContinueWith(t => 
                {
                    RefreshChartFavoriteInfo();
                });
            }
            else
            {
                var chartInfo = this.queriedChart;
                if (chartInfo != null)
                {
                    // 添加收藏
                    var dialogCtrl = addFavoriteItemDialogCtrlFactory.CreateExport().Value;
                    dialogCtrl.DialogOwner = UIHelper.GetWindowOfUIElement(contentVM.View);
                    dialogCtrl.FavoriteItemType = ChartFolderListItemType.Chart;
                    dialogCtrl.ChartId = chartInfo.ChartId;
                    dialogCtrl.InitialFavoriteItemName = chartInfo.ChartName;

                    dialogCtrl.Initialize();
                    dialogCtrl.Run();
                    dialogCtrl.Shutdown();

                    if (dialogCtrl.AddOrUpdatedFavoriteItem != null && dialogCtrl.IsFavoriteItemNewAdded == true)
                    {
                        RefreshChartFavoriteInfo();
                    }
                }
            }
        }

        private void UpdateIsLoadingPageData(bool value)
        {
            this.isLoadingPageData = value;
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                contentVM.IsLoadingPageData = value;
                toggleChartFavoriteCmd?.RaiseCanExecuteChanged();
            });
        }

        private void UpdateIsRefreshingFavorInfo(bool value)
        {
            this.isRefreshingFavorInfo = value;
            DispatcherHelper.CheckBeginInvokeOnUI(() => 
            {
                toggleChartFavoriteCmd?.RaiseCanExecuteChanged();
            });
        }

        private void UpdateCurrentChartId(long? value)
        {
            this.currentChartId = value;
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                toggleChartFavoriteCmd?.RaiseCanExecuteChanged();
            });
        }

        private CancellationToken AcquirePageDataRefreshCLT()
        {
            CancellationToken clt = CancellationToken.None;
            lock (pageDataRefreshCLTSLock)
            {
                if (pageDataRefreshCLTS == null)
                {
                    pageDataRefreshCLTS = new CancellationTokenSource();
                }
                clt = pageDataRefreshCLTS.Token;
            }
            return clt;
        }

        private void CancelPageDataRefresh()
        {
            lock (pageDataRefreshCLTSLock)
            {
                if (pageDataRefreshCLTS != null)
                {
                    pageDataRefreshCLTS.Cancel();
                    pageDataRefreshCLTS.Dispose();
                    pageDataRefreshCLTS = null;
                }
            }
        }

        private void RefreshPageData(long queryChartId, CancellationToken clt)
        {
            pageDataRefreshTF.StartNew(() => 
            {
                if (clt.IsCancellationRequested) return;
                
                UpdateIsLoadingPageData(true);

                Chart qChart = null;
                FavoriteChart qChartFavorInfo = null;

                var t1 = Task.Run(() =>
                {
                    qChart = chartQueryCtrl.QueryChart(queryChartId, false, out IInterfaceInteractResponse _resp);
                });

                var t2 = Task.Run(() => 
                {
                    qChartFavorInfo = QueryChartFavoriteInfo(queryChartId);
                });

                try
                {
                    Task.WaitAll(t1, t2);
                }
                catch (AggregateException)
                {
                    // 失败或取消
                }

                this.queriedChart = qChart;
                this.queriedFavoriteInfo = qChartFavorInfo;
                UpdateIsLoadingPageData(false);

                if (!clt.IsCancellationRequested)
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        var chartUrl = qChart?.Url;
                        if (!string.IsNullOrEmpty(chartUrl))
                            contentVM.LoadChartWebContentWithUrl(chartUrl);
                        else
                            contentVM.LoadChartWebContentWithHtmlContent(_404HtmlContent);

                        contentVM.ChartIsFavorited = (qChartFavorInfo != null);
                    });
                }
            });
        }

        private void RefreshChartFavoriteInfo()
        {
            if (isRefreshingFavorInfo) return;
            pageDataRefreshTF.StartNew(() => 
            {
                UpdateIsRefreshingFavorInfo(true);
                FavoriteChart qChartFavorInfo = null;
                var queryChartId = this.ChartIdFactory?.Invoke(this);
                if (queryChartId != null)
                {
                    qChartFavorInfo = QueryChartFavoriteInfo(queryChartId.Value);
                }

                this.queriedFavoriteInfo = qChartFavorInfo;
                UpdateIsRefreshingFavorInfo(false);
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    contentVM.ChartIsFavorited = (qChartFavorInfo != null);
                });
            });
        }

        private FavoriteChart QueryChartFavoriteInfo(long chartId)
        {
            var landingInfo = authUserLoginService.TouyanAuthUserLoginLandingInfo;
            if (landingInfo == null) return null;
            
            var option = new ReqFavoriteChartOption { XiaohaChartId = chartId };
            var resp = XqThriftLibConfigurationManager.SharedInstance.XiaohaChartTerminalAoHttpStub.reqFavoriteChart(landingInfo, option);
            return resp?.CorrectResult?.FirstOrDefault(i => i.XiaohaChartId == chartId);
        }
    }
}
