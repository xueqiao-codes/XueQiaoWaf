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
using XueQiaoFoundation.Shared.Model;

namespace Touyan.app.controller
{
    [Export, Export(typeof(ITouyanModuleRootViewCtrl)), PartCreationPolicy(CreationPolicy.NonShared)]
    internal class TouyanModuleRootViewCtrl : ITouyanModuleRootViewCtrl
    {
        private readonly TouyanModuleRootVM contentVM;
        private readonly ExportFactory<QuotationChartContainerViewCtrl> quotationChartContainerViewCtrlFactory;

        private readonly Dictionary<SimpleTabItem, IController> tabWorkspaceControllers
            = new Dictionary<SimpleTabItem, IController>();

        private SimpleTabItem workspaceItem_QuotationChart;

        [ImportingConstructor]
        public TouyanModuleRootViewCtrl(TouyanModuleRootVM contentVM,
            ExportFactory<QuotationChartContainerViewCtrl> quotationChartContainerViewCtrlFactory)
        {
            this.contentVM = contentVM;
            this.quotationChartContainerViewCtrlFactory = quotationChartContainerViewCtrlFactory;
        }

        public ChromeWindowCaptionDataHolder EmbedInWindowCaptionDataHolder { get; set; }

        public object ContentView => contentVM.View;

        public void Initialize()
        {
            contentVM.EmbedInWindowCaptionDataHolder = this.EmbedInWindowCaptionDataHolder;
            PropertyChangedEventManager.AddHandler(contentVM, ContentVMPropChanged, "");
        }

        public void Run()
        {
            workspaceItem_QuotationChart = new SimpleTabItem { Header = "行情图表" };
            contentVM.WorkspaceItems.Add(workspaceItem_QuotationChart);
            contentVM.ActiveWorkspaceItem = contentVM.WorkspaceItems.FirstOrDefault();
        }

        public void Shutdown()
        {
            PropertyChangedEventManager.RemoveHandler(contentVM, ContentVMPropChanged, "");

            foreach (var i in tabWorkspaceControllers.ToArray())
            {
                i.Value.Shutdown();
            }
            tabWorkspaceControllers.Clear();
        }

        private void ContentVMPropChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TouyanModuleRootVM.ActiveWorkspaceItem))
            {
                var activeWorkspaceItem = contentVM.ActiveWorkspaceItem;
                if (activeWorkspaceItem == workspaceItem_QuotationChart)
                {
                    ShowWorkspaceContent_QuotationChart();
                }
                else
                {
                    // do other  
                }
            }
        }

        private void ShowWorkspaceContent_QuotationChart()
        {
            if (workspaceItem_QuotationChart == null) return;

            tabWorkspaceControllers.TryGetValue(workspaceItem_QuotationChart, out IController controller);
            var tabWorkspaceCtrl = controller as QuotationChartContainerViewCtrl;
            if (tabWorkspaceCtrl == null)
            {
                tabWorkspaceCtrl = quotationChartContainerViewCtrlFactory.CreateExport().Value;
                tabWorkspaceCtrl.Initialize();
                tabWorkspaceCtrl.Run();

                tabWorkspaceControllers[workspaceItem_QuotationChart] = tabWorkspaceCtrl;
            }

            if (workspaceItem_QuotationChart.ContentView == null)
            {
                workspaceItem_QuotationChart.ContentView = tabWorkspaceCtrl.ContentView;
            }
        }
    }
}
