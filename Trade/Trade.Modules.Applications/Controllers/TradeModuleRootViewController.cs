using ContainerShell.Interfaces.Applications;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.BusinessResources.Constants;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.Shared.Model;
using XueQiaoWaf.Trade.Interfaces.Applications;
using XueQiaoWaf.Trade.Modules.Applications.ViewModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers
{
    /// <summary>
    /// `交易` 模块根视图页面
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class TradeModuleRootViewController : IController
    {
        private readonly TradeModuleRootVM contentVM;
        private readonly ITradeModuleService tradeModuleService;
        private readonly IWorkspaceTabControlViewCtrl mainWinWorkspaceTabControlViewCtrl;
        private readonly ExportFactory<IWorkspaceInterTabWindowCtrl> wsInterTabWindowCtrlFactory;
        private readonly ExportFactory<TradeWorkspaceItemController> tradeWSItemCtrlFactory;

        private readonly List<IWorkspaceInterTabWindowCtrl> workspaceInterTabWindowCtrls
            = new List<IWorkspaceInterTabWindowCtrl>();

        private static readonly string TRADE_WORKSPACE_INTERTAB_KEY = UUIDHelper.CreateUUIDString(false);

        [ImportingConstructor]
        public TradeModuleRootViewController(
            TradeModuleRootVM contentVM, 
            ITradeModuleService tradeModuleService,
            IWorkspaceTabControlViewCtrl mainWinWorkspaceTabControlViewCtrl,
            ExportFactory<IWorkspaceInterTabWindowCtrl> wsInterTabWindowCtrlFactory,
            ExportFactory<TradeWorkspaceItemController> tradeWSItemCtrlFactory)
        {
            this.contentVM = contentVM;
            this.tradeModuleService = tradeModuleService;
            this.mainWinWorkspaceTabControlViewCtrl = mainWinWorkspaceTabControlViewCtrl;
            this.wsInterTabWindowCtrlFactory = wsInterTabWindowCtrlFactory;
            this.tradeWSItemCtrlFactory = tradeWSItemCtrlFactory;
        }

        public ChromeWindowCaptionDataHolder EmbedInWindowCaptionDataHolder { get; set; }

        public object ContentView => contentVM.View;

        public void Initialize()
        {
            
        }

        public void Run()
        {
            ShowWorkspaceWindows();
        }

        public void Shutdown()
        {
            foreach (var ctrl in workspaceInterTabWindowCtrls.ToArray())
            {
                ctrl.Shutdown();
            }
            workspaceInterTabWindowCtrls.Clear();
            mainWinWorkspaceTabControlViewCtrl.Shutdown();
        }
        
        private void ShowWorkspaceWindows()
        {
            var workspaceDataRoot = tradeModuleService.TradeWorkspaceDataRoot;
            if (workspaceDataRoot == null)
            {
                return;
            }

            // show main window workspace tab control view

            // 显示主窗口的交易 tab 视图
            var workspaceListContainer = workspaceDataRoot.MainWindowWorkspaceListContainer;
            if (!workspaceListContainer.Workspaces.Any())
            {
                var ws = new TabWorkspace(tradeModuleService.GenerateTradeWorkspaceKey())
                { Name = "主工作区", WorkspaceType = XueQiaoConstants.WORKSPACE_TRADE_MAIN };
                workspaceListContainer.Workspaces.Add(ws);
            }
            mainWinWorkspaceTabControlViewCtrl.InterTabWindowListContainer = workspaceDataRoot.InterTabWorkspaceWindowListContainer;
            mainWinWorkspaceTabControlViewCtrl.WorkspaceListContainer = workspaceListContainer;
            mainWinWorkspaceTabControlViewCtrl.FixedItemsCount = 1;
            mainWinWorkspaceTabControlViewCtrl.CloseWindowWhenItemsEmptied = false;
            mainWinWorkspaceTabControlViewCtrl.IsEmbedInWindowCaption = true;
            mainWinWorkspaceTabControlViewCtrl.EmbedInWindowCaptionDataHolder = this.EmbedInWindowCaptionDataHolder;

            mainWinWorkspaceTabControlViewCtrl.NewItemViewCtrlFactory = (viewCtrl, ws) =>
            {
                var wsItemViewCtrl = tradeWSItemCtrlFactory.CreateExport().Value;
                wsItemViewCtrl.Workspace = ws;

                var isInitialFromDefaultTemplate = false;
                if (viewCtrl.ItemCount == 0
                    && true != ws.TradeComponents?.Any()
                    && ws.WorkspaceType == XueQiaoConstants.WORKSPACE_TRADE_MAIN)
                {
                    isInitialFromDefaultTemplate = true;
                }
                wsItemViewCtrl.IsInitialFromDefaultTemplate = isInitialFromDefaultTemplate;
                return wsItemViewCtrl;
            };

            mainWinWorkspaceTabControlViewCtrl.NewWorkspaceFactory = (viewCtrl) =>
                new TabWorkspace(tradeModuleService.GenerateTradeWorkspaceKey())
                {
                    Name = "新建工作区",
                    WorkspaceType = XueQiaoConstants.WORKSPACE_TRADE_SUB
                };

            mainWinWorkspaceTabControlViewCtrl.InterTabPartitionKey = TRADE_WORKSPACE_INTERTAB_KEY;

            mainWinWorkspaceTabControlViewCtrl.Initialize();
            mainWinWorkspaceTabControlViewCtrl.Run();

            contentVM.WorkspaceTabControlView = mainWinWorkspaceTabControlViewCtrl.ContentView;


            // 显示拆分窗口的交易 tab 视图
            foreach (var interTabWindowContainer in workspaceDataRoot.InterTabWorkspaceWindowListContainer.Windows)
            {
                var interTabWindowCtrl = wsInterTabWindowCtrlFactory.CreateExport().Value;
                interTabWindowCtrl.InterTabWindowListContainer = workspaceDataRoot.InterTabWorkspaceWindowListContainer;
                interTabWindowCtrl.InterTabWindowContainer = interTabWindowContainer;

                interTabWindowCtrl.NewWorkspaceItemViewCtrlFactory = (viewCtrl, ws)=> 
                {
                    var wsItemViewCtrl = tradeWSItemCtrlFactory.CreateExport().Value;
                    wsItemViewCtrl.Workspace = ws;
                    wsItemViewCtrl.IsInitialFromDefaultTemplate = false;
                    return wsItemViewCtrl;
                };

                interTabWindowCtrl.NewWorkspaceFactory = (viewCtrl) =>
                    new TabWorkspace(tradeModuleService.GenerateTradeWorkspaceKey())
                    {
                        Name = "新建工作区",
                        WorkspaceType = XueQiaoConstants.WORKSPACE_TRADE_SUB
                    };

                interTabWindowCtrl.InterTabPartitionKey = TRADE_WORKSPACE_INTERTAB_KEY;
                interTabWindowCtrl.ShowWindowWhenRun = true;

                interTabWindowCtrl.WindowCloseAction = ctrl => 
                {
                    ctrl.Shutdown();
                    workspaceInterTabWindowCtrls.Remove(ctrl);
                };

                workspaceInterTabWindowCtrls.Add(interTabWindowCtrl);

                interTabWindowCtrl.Initialize();
                interTabWindowCtrl.Run();
            }
        }
    }
}
