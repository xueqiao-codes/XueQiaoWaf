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
using Touyan.app.datamodel;
using Touyan.app.viewmodel;
using Touyan.Interface.application;
using xueqiao.graph.xiaoha.chart.thriftapi;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;

namespace Touyan.app.controller
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class ChartFolderSubItemListViewCtrl : IController
    {
        private readonly ChartFolderSubItemListVM contentVM;
        private readonly ITouyanChartQueryCtrl chartQueryCtrl;

        private readonly TaskFactory loadFolderChildrenChartTF = new TaskFactory(new OrderedTaskScheduler());

        [ImportingConstructor]
        public ChartFolderSubItemListViewCtrl(
            ChartFolderSubItemListVM contentVM,
            ITouyanChartQueryCtrl chartQueryCtrl)
        {
            this.contentVM = contentVM;
            this.chartQueryCtrl = chartQueryCtrl;
        }
        
        public IEnumerable<ChartFolderListTreeNodeBase> SubItemNodeList { get; set; }

        public Action<long> SelectedChartHandler { get; set; }

        public object ContentView => contentVM.View;
        
        public void Initialize()
        {
            contentVM.NodeItemSelectedHandler = OnSelectedNodeItem;
            contentVM.NodeItemExpandedHandler = OnExpandedNodeItem;
        }
        
        public void Run()
        {
            InvalidateSubFolderNodeTree();
        }

        public void Shutdown()
        {
            contentVM.NodeItemSelectedHandler = null;
            contentVM.NodeItemExpandedHandler = null;
        }

        private void OnSelectedNodeItem(ChartFolderListTreeNodeBase nodeItem)
        {
            if (nodeItem is ChartFolderListTreeNode_Chart chartNode)
            {
                SelectedChartHandler?.Invoke(chartNode.ChartId);
            }
        }

        private void OnExpandedNodeItem(ChartFolderListTreeNodeBase nodeItem)
        {
            if (nodeItem is ChartFolderListTreeNode_Folder folderNode)
            {
                LoadChartChildrenIfNeed(folderNode);
            }
        }

        private void InvalidateSubFolderNodeTree()
        {
            contentVM.ChartFolderNodeTree.Clear();
            contentVM.ChartFolderNodeTree.AddRange(SubItemNodeList);
        }

        private void LoadChartChildrenIfNeed(ChartFolderListTreeNode_Folder folderNode)
        {
            if (folderNode == null) return;
            if (folderNode.IsLoadingChildren) return;
            if (folderNode.Children.Any()) return;

            folderNode.IsLoadingChildren = true;
            loadFolderChildrenChartTF.StartNew(() => 
            {
                var queryResp = QueryAllCharts(folderNode.FolderId, CancellationToken.None);
                var chartNodes = queryResp?.CorrectResult?
                    .Select(i => new ChartFolderListTreeNode_Chart(i.ChartId, i.ParentFolderId) { Chart = i })
                    .ToArray();
                DispatcherHelper.CheckBeginInvokeOnUI(() => 
                {
                    folderNode.Children.Clear();
                    folderNode.Children.AddRange(chartNodes);
                    folderNode.IsLoadingChildren = false;
                });
            });
        }

        private IInterfaceInteractResponse<IEnumerable<Chart>> QueryAllCharts(long parentFolderId, CancellationToken clt)
        {
            if (clt.IsCancellationRequested) return null;
            var queryPageSize = 50;
            IInterfaceInteractResponse<ChartPage> lastPageResp = null;
            var queryOption = new ReqChartOption { ParentFolderId = parentFolderId, State = ChartState.ENABLE };

            var queryAllCtrl = new QueryAllItemsByPageHelper<Chart>(pageIndex =>
            {
                if (clt.IsCancellationRequested) return null;
                var pageOption = new IndexedPageOption
                {
                    NeedTotalCount = true,
                    PageIndex = pageIndex,
                    PageSize = queryPageSize
                };

                var resp = chartQueryCtrl.RequestQueryChart(queryOption, pageOption);
                lastPageResp = resp;

                if (resp == null) return null;

                var pageInfo = resp.CorrectResult;
                var pageResult = new QueryItemsByPageResult<Chart>(resp.SourceException != null)
                {
                    TotalCount = pageInfo?.Total,
                    Page = pageInfo?.Page
                };
                return pageResult;
            });

            queryAllCtrl.RemoveDuplicateFunc = _items =>
            {
                if (_items == null) return null;
                var idGroupedItems = _items.GroupBy(i => i.ChartId);
                return idGroupedItems.Select(i => i.LastOrDefault()).ToArray();
            };

            var queriedItems = queryAllCtrl.QueryAllItems();
            if (lastPageResp == null) return null;
            if (clt.IsCancellationRequested) return null;

            var tarResp = new InterfaceInteractResponse<IEnumerable<Chart>>(lastPageResp.Servant,
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
    }
}
