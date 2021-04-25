using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Touyan.app.viewmodel;
using Touyan.Interface.application;
using XueQiaoFoundation.Shared.Interface;

namespace Touyan.app.controller
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class ChartViewHistoryListViewCtrl : IController
    {
        private readonly ChartViewHistoryListVM contentVM;
        private readonly ITouyanChartViewHistoryServiceCtrl chartViewHistoryServiceCtrl;

        [ImportingConstructor]
        public ChartViewHistoryListViewCtrl(ChartViewHistoryListVM contentVM,
            ITouyanChartViewHistoryServiceCtrl chartViewHistoryServiceCtrl)
        {
            this.contentVM = contentVM;
            this.chartViewHistoryServiceCtrl = chartViewHistoryServiceCtrl;
        }

        public Action<long> SelectedHistoryChartHandler { get; set; }

        public object ContentView => contentVM.View;

        public void Initialize()
        {
            PropertyChangedEventManager.AddHandler(contentVM, ContentVMPropChanged_SelectedHistoryItem, nameof(ChartViewHistoryListVM.SelectedHistoryItem));
        }
        
        public void Run()
        {
            chartViewHistoryServiceCtrl?.RefreshHistoryListIfNeed();
        }

        public void Shutdown()
        {
            PropertyChangedEventManager.RemoveHandler(contentVM, ContentVMPropChanged_SelectedHistoryItem, nameof(ChartViewHistoryListVM.SelectedHistoryItem));
        }

        private void ContentVMPropChanged_SelectedHistoryItem(object sender, PropertyChangedEventArgs e)
        {
            var selItem = contentVM.SelectedHistoryItem;
            if (selItem != null)
            {
                SelectedHistoryChartHandler?.Invoke(selItem.ChartId);
            }
        }
    }
}
