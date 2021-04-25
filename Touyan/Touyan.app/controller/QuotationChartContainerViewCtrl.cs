using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Touyan.app.viewmodel;
using Touyan.Interface.application;
using Touyan.Interface.model;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.Shared.Model;

namespace Touyan.app.controller
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class QuotationChartContainerViewCtrl : IController
    {
        private readonly QuotationChartContainerVM contentVM;
        private readonly ITouyanChartViewHistoryServiceCtrl chartViewHistoryServiceCtrl;
        private readonly ExportFactory<ChartFolderListContainerViewCtrl> folderListContainerViewCtrlFactory;
        private readonly ExportFactory<ChartFavoriateListViewCtrl> favoriateListViewCtrlFactory;
        private readonly ExportFactory<ChartViewHistoryListViewCtrl> chartViewHistoryListViewCtrlFactory;
        private readonly ChartDetailContainerViewCtrl chartDetailContainerViewCtrl;
        
        private SimpleTabItem chartFolderListTabItem;
        private SimpleTabItem chartFavoriateListTabItem;
        private SimpleTabItem chartViewHistoryTabItem;

        private ChartFolderListContainerViewCtrl folderListContainerViewCtrl;
        private ChartFavoriateListViewCtrl favoriateListViewCtrl;
        private ChartViewHistoryListViewCtrl chartViewHistoryListViewCtrl;

        private long? currentSelectedChartId;

        [ImportingConstructor]
        public QuotationChartContainerViewCtrl(QuotationChartContainerVM contentVM,
            ITouyanChartViewHistoryServiceCtrl chartViewHistoryServiceCtrl,
            ExportFactory<ChartFolderListContainerViewCtrl> folderListContainerViewCtrlFactory,
            ExportFactory<ChartFavoriateListViewCtrl> favoriateListViewCtrlFactory,
            ExportFactory<ChartViewHistoryListViewCtrl> chartViewHistoryListViewCtrlFactory,
            ChartDetailContainerViewCtrl chartDetailContainerViewCtrl)
        {
            this.contentVM = contentVM;
            this.chartViewHistoryServiceCtrl = chartViewHistoryServiceCtrl;
            this.folderListContainerViewCtrlFactory = folderListContainerViewCtrlFactory;
            this.favoriateListViewCtrlFactory = favoriateListViewCtrlFactory;
            this.chartViewHistoryListViewCtrlFactory = chartViewHistoryListViewCtrlFactory;
            this.chartDetailContainerViewCtrl = chartDetailContainerViewCtrl;
        }
        
        public object ContentView => contentVM.View;

        public void Initialize()
        {
            chartFolderListTabItem = new SimpleTabItem { Header = "图表" };
            chartFavoriateListTabItem = new SimpleTabItem { Header = "收藏夹" };
            chartViewHistoryTabItem = new SimpleTabItem { Header = "浏览历史" };

            chartDetailContainerViewCtrl.ChartIdFactory = _ctrl => this.currentSelectedChartId;
            chartDetailContainerViewCtrl.SearchChartSelectedHandler = (_ctrl, _chartId) =>
            {
                this.currentSelectedChartId = _chartId;
                _ctrl?.InvalidatePageContent();
            };

            chartDetailContainerViewCtrl.Initialize();
            chartDetailContainerViewCtrl.Run();

            contentVM.ChartDetailContentView = chartDetailContainerViewCtrl.ContentView;

            PropertyChangedEventManager.AddHandler(contentVM, ContentVMPropChanged_SelectedTabItem, nameof(QuotationChartContainerVM.SelectedTabItem));
        }

        public void Run()
        {
            contentVM.TabItems.Add(chartFolderListTabItem);
            contentVM.TabItems.Add(chartFavoriateListTabItem);
            contentVM.TabItems.Add(chartViewHistoryTabItem);
            contentVM.SelectedTabItem = contentVM.TabItems.First();
        }

        public void Shutdown()
        {
            PropertyChangedEventManager.RemoveHandler(contentVM, ContentVMPropChanged_SelectedTabItem, nameof(QuotationChartContainerVM.SelectedTabItem));

            folderListContainerViewCtrl?.Shutdown();
            folderListContainerViewCtrl = null;

            chartDetailContainerViewCtrl?.Shutdown();
        }

        private void ContentVMPropChanged_SelectedTabItem(object sender, PropertyChangedEventArgs e)
        {
            var selTabItem = contentVM.SelectedTabItem;
            if (selTabItem == chartFolderListTabItem)
            {
                ShowTabContent_chartFolderList();
            }
            else if (selTabItem == chartFavoriateListTabItem)
            {
                ShowTabContent_chartFavoriateList();
            }
            else if (selTabItem == chartViewHistoryTabItem)
            {
                ShowTabContent_chartViewHistory();
            }
        }

        private void ShowTabContent_chartFolderList()
        {
            if (folderListContainerViewCtrl == null)
            {
                var ctrl = folderListContainerViewCtrlFactory.CreateExport().Value;
                ctrl.SelectedChartHandler = _chartId => 
                {
                    this.currentSelectedChartId = _chartId;
                    chartDetailContainerViewCtrl.InvalidatePageContent();
                    AddOrUpdateChartViewHistory(_chartId);
                };

                ctrl.Initialize();
                ctrl.Run();

                this.folderListContainerViewCtrl = ctrl;
            }

            if (chartFolderListTabItem != null && chartFolderListTabItem.ContentView == null)
            {
                chartFolderListTabItem.ContentView = folderListContainerViewCtrl.ContentView;
            }
        }

        private void ShowTabContent_chartFavoriateList()
        {
            if (favoriateListViewCtrl == null)
            {
                var ctrl = favoriateListViewCtrlFactory.CreateExport().Value;
                ctrl.SelectedChartHandler = _chartId =>
                {
                    this.currentSelectedChartId = _chartId;
                    chartDetailContainerViewCtrl.InvalidatePageContent();
                    AddOrUpdateChartViewHistory(_chartId);
                };

                ctrl.Initialize();
                ctrl.Run();

                this.favoriateListViewCtrl = ctrl;
            }

            if (chartFavoriateListTabItem != null && chartFavoriateListTabItem.ContentView == null)
            {
                chartFavoriateListTabItem.ContentView = favoriateListViewCtrl.ContentView;
            }
        }

        private void ShowTabContent_chartViewHistory()
        {
            if (chartViewHistoryListViewCtrl == null)
            {
                var ctrl = chartViewHistoryListViewCtrlFactory.CreateExport().Value;
                ctrl.SelectedHistoryChartHandler = _chartId => 
                {
                    this.currentSelectedChartId = _chartId;
                    chartDetailContainerViewCtrl.InvalidatePageContent();
                    AddOrUpdateChartViewHistory(_chartId);
                };

                ctrl.Initialize();
                ctrl.Run();

                this.chartViewHistoryListViewCtrl = ctrl;
            }

            if (chartViewHistoryTabItem != null && chartViewHistoryTabItem.ContentView == null)
            {
                chartViewHistoryTabItem.ContentView = chartViewHistoryListViewCtrl.ContentView;
            }
        }

        /// <summary>
        /// 添加或更新图表浏览历史
        /// </summary>
        /// <param name="chartId"></param>
        /// <param name="viewTimestamp"></param>
        private void AddOrUpdateChartViewHistory(long chartId, long? viewTimestamp = null)
        {
            long viewTS = viewTimestamp ?? (long)DateHelper.NowUnixTimeSpan().TotalSeconds;
            chartViewHistoryServiceCtrl.RequestAddOrUpdateHistoryItem(new ChartViewHistory[] { new ChartViewHistory { ChartId = chartId, ViewTimestamp = viewTS } }, true);
        }
    }
}
