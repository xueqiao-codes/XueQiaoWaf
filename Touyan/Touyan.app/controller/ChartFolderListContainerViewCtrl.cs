using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Touyan.app.datamodel;
using Touyan.app.helper;
using Touyan.app.viewmodel;
using Touyan.Interface.application;
using xueqiao.graph.xiaoha.chart.thriftapi;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;

namespace Touyan.app.controller
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class ChartFolderListContainerViewCtrl : IController
    {
        private readonly ChartFolderContainerVM contentVM;
        private readonly ITouyanChartFolderServiceCtrl chartFolderServiceCtrl;
        private readonly ExportFactory<ChartFolderSubItemListViewCtrl> folderSubItemListViewCtrlFactory;

        private readonly Dictionary<TopChartFolderTabItem, ChartFolderSubItemListViewCtrl> topFolderSubItemListViewCtrls
            = new Dictionary<TopChartFolderTabItem, ChartFolderSubItemListViewCtrl>();
        
        private IEnumerable<ChartFolderListTreeNode_Folder> loadedFolderNodeTree;

        [ImportingConstructor]
        public ChartFolderListContainerViewCtrl(
            ChartFolderContainerVM contentVM,
            ITouyanChartFolderServiceCtrl chartFolderServiceCtrl,
            ExportFactory<ChartFolderSubItemListViewCtrl> folderSubItemListViewCtrlFactory)
        {
            this.contentVM = contentVM;
            this.chartFolderServiceCtrl = chartFolderServiceCtrl;
            this.folderSubItemListViewCtrlFactory = folderSubItemListViewCtrlFactory;
        }

        public Action<long> SelectedChartHandler { get; set; }

        public object ContentView => contentVM.View;
        
        public void Initialize()
        {
            AddContentVMPropChangedHandler_SelectedTopFolderTabItem();
        }

        public void Run()
        {
            LoadChartFolderList();
        }

        public void Shutdown()
        {
            RemoveContentVMPropChangedHandler_SelectedTopFolderTabItem();
            ClearAndShutdownAllTopFolderSubItemListViewCtrls();
            loadedFolderNodeTree = null;
        }

        private void ContentVMPropChanged_SelectedTopFolderTabItem(object sender, PropertyChangedEventArgs e)
        {
            OnChanged_SelectedTopFolderTabItem();
        }

        private void OnChanged_SelectedTopFolderTabItem()
        {
            var selTabItem = contentVM.SelectedTopFolderTabItem;
            if (selTabItem != null)
            {
                ChartFolderSubItemListViewCtrl viewCtrl = null;
                if (!topFolderSubItemListViewCtrls.TryGetValue(selTabItem, out viewCtrl))
                {
                    viewCtrl = folderSubItemListViewCtrlFactory.CreateExport().Value;
                    viewCtrl.SubItemNodeList = loadedFolderNodeTree.FirstOrDefault(i => i.FolderId == selTabItem.Folder.FolderId)?.Children;
                    viewCtrl.SelectedChartHandler = _id => this.SelectedChartHandler?.Invoke(_id);

                    viewCtrl.Initialize();
                    viewCtrl.Run();

                    topFolderSubItemListViewCtrls[selTabItem] = viewCtrl;
                }

                if (selTabItem.ContentView == null)
                {
                    selTabItem.ContentView = viewCtrl.ContentView;
                }
            }
        }

        private void AddContentVMPropChangedHandler_SelectedTopFolderTabItem()
        {
            RemoveContentVMPropChangedHandler_SelectedTopFolderTabItem();
            PropertyChangedEventManager.AddHandler(contentVM, ContentVMPropChanged_SelectedTopFolderTabItem, nameof(ChartFolderContainerVM.SelectedTopFolderTabItem));
        }

        private void RemoveContentVMPropChangedHandler_SelectedTopFolderTabItem()
        {
            PropertyChangedEventManager.RemoveHandler(contentVM, ContentVMPropChanged_SelectedTopFolderTabItem, nameof(ChartFolderContainerVM.SelectedTopFolderTabItem));
        }

        private void ClearAndShutdownAllTopFolderSubItemListViewCtrls()
        {
            foreach (var ctrl in topFolderSubItemListViewCtrls)
            {
                ctrl.Value?.Shutdown();
            }
            topFolderSubItemListViewCtrls.Clear();
        }

        private async void LoadChartFolderList()
        {
            var resultWrapper = await chartFolderServiceCtrl.RefreshFolderListIfNeed();
            var resultData = resultWrapper?.ResultData?.ToArray() ?? new ChartFolder[] { };

            var rootFolder = DMConstructHelper.GetMeaningfulRootChartFolder(resultData);
            IEnumerable<ChartFolderListTreeNode_Folder> folderNodeTree = null;
            if (rootFolder != null)
            {
                var folderRootNode = DMConstructHelper.GenerateChartFolderTreeNodeByRootFolderId(resultData, rootFolder.FolderId);
                folderNodeTree = folderRootNode?.Children.OfType<ChartFolderListTreeNode_Folder>().ToArray();
            }
            var topFolderTabItems = folderNodeTree?.Select(i => new TopChartFolderTabItem { Folder = i.Folder }).ToArray();

            DispatcherHelper.CheckBeginInvokeOnUI(() => 
            {
                RemoveContentVMPropChangedHandler_SelectedTopFolderTabItem();

                ClearAndShutdownAllTopFolderSubItemListViewCtrls();

                this.loadedFolderNodeTree = folderNodeTree;
                contentVM.TopFolderTabItems.Clear();
                contentVM.TopFolderTabItems.AddRange(topFolderTabItems);
                contentVM.SelectedTopFolderTabItem = topFolderTabItems?.FirstOrDefault();
                OnChanged_SelectedTopFolderTabItem();

                AddContentVMPropChangedHandler_SelectedTopFolderTabItem();
            });
        }
    }
}
