using ContainerShell.Interfaces.Applications;
using Research.Interface.Application;
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

namespace Research.app.Controller
{
    /// <summary>
    /// `投研` 模块根视图页面
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class ResearchModuleRootViewController : IController
    {
        private readonly IWorkspaceTabControlViewCtrl mainWinWorkspaceTabControlViewCtrl;
        private readonly ExportFactory<IWorkspaceInterTabWindowCtrl> wsInterTabWindowCtrlFactory;
        private readonly ExportFactory<ResearchWorkspaceItemCtrl> researchWSItemCtrlFactory;
        private readonly IResearchModuleService researchModuleService;

        private readonly List<IWorkspaceInterTabWindowCtrl> workspaceInterTabWindowCtrls
            = new List<IWorkspaceInterTabWindowCtrl>();

        private static readonly string RESEARCH_WORKSPACE_INTERTAB_KEY = UUIDHelper.CreateUUIDString(false);

        [ImportingConstructor]
        public ResearchModuleRootViewController(
            IWorkspaceTabControlViewCtrl mainWinWorkspaceTabControlViewCtrl,
            ExportFactory<IWorkspaceInterTabWindowCtrl> wsInterTabWindowCtrlFactory,
            ExportFactory<ResearchWorkspaceItemCtrl> researchWSItemCtrlFactory,
            IResearchModuleService researchModuleService)
        {
            this.mainWinWorkspaceTabControlViewCtrl = mainWinWorkspaceTabControlViewCtrl;
            this.wsInterTabWindowCtrlFactory = wsInterTabWindowCtrlFactory;
            this.researchWSItemCtrlFactory = researchWSItemCtrlFactory;
            this.researchModuleService = researchModuleService;
        }

        public object ContentView => mainWinWorkspaceTabControlViewCtrl.ContentView;

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
            var workspaceDataRoot = researchModuleService.ResearchWorkspaceDataRoot;
            if (workspaceDataRoot == null)
            {
                return;
            }

            // show main window workspace tab control view

            // 显示主窗口的投研 tab 视图
            var workspaceListContainer = workspaceDataRoot.MainWindowWorkspaceListContainer;
            if (!workspaceListContainer.Workspaces.Any())
            {
                var ws = new TabWorkspace(researchModuleService.GenerateResearchWorkspaceKey())
                { Name = "投研工作区", WorkspaceType = XueQiaoConstants.WORKSPACE_RESEARCH };
                workspaceListContainer.Workspaces.Add(ws);
            }
            mainWinWorkspaceTabControlViewCtrl.InterTabWindowListContainer = workspaceDataRoot.InterTabWorkspaceWindowListContainer;
            mainWinWorkspaceTabControlViewCtrl.WorkspaceListContainer = workspaceListContainer;
            mainWinWorkspaceTabControlViewCtrl.FixedItemsCount = 1;
            mainWinWorkspaceTabControlViewCtrl.CloseWindowWhenItemsEmptied = false;

            mainWinWorkspaceTabControlViewCtrl.NewItemViewCtrlFactory = (viewCtrl, ws) =>
            {
                var wsItemViewCtrl = researchWSItemCtrlFactory.CreateExport().Value;
                wsItemViewCtrl.Workspace = ws;
                return wsItemViewCtrl;
            };

            mainWinWorkspaceTabControlViewCtrl.NewWorkspaceFactory = (viewCtrl) =>
                new TabWorkspace(researchModuleService.GenerateResearchWorkspaceKey())
                {
                    Name = "新建工作区",
                    WorkspaceType = XueQiaoConstants.WORKSPACE_RESEARCH
                };

            mainWinWorkspaceTabControlViewCtrl.InterTabPartitionKey = RESEARCH_WORKSPACE_INTERTAB_KEY;
            mainWinWorkspaceTabControlViewCtrl.Initialize();
            mainWinWorkspaceTabControlViewCtrl.Run();

            // 显示拆分窗口的投研 tab 视图
            foreach (var interTabWindowContainer in workspaceDataRoot.InterTabWorkspaceWindowListContainer.Windows)
            {
                var interTabWindowCtrl = wsInterTabWindowCtrlFactory.CreateExport().Value;
                interTabWindowCtrl.InterTabWindowListContainer = workspaceDataRoot.InterTabWorkspaceWindowListContainer;
                interTabWindowCtrl.InterTabWindowContainer = interTabWindowContainer;

                interTabWindowCtrl.NewWorkspaceItemViewCtrlFactory = (viewCtrl, ws) =>
                {
                    var wsItemViewCtrl = researchWSItemCtrlFactory.CreateExport().Value;
                    wsItemViewCtrl.Workspace = ws;
                    return wsItemViewCtrl;
                };

                interTabWindowCtrl.NewWorkspaceFactory = (viewCtrl) =>
                    new TabWorkspace(researchModuleService.GenerateResearchWorkspaceKey())
                    {
                        Name = "新建工作区",
                        WorkspaceType = XueQiaoConstants.WORKSPACE_RESEARCH
                    };

                interTabWindowCtrl.InterTabPartitionKey = RESEARCH_WORKSPACE_INTERTAB_KEY;
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
