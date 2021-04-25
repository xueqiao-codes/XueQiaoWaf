using ContainerShell.Interfaces.Applications;
using Research.app.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using XueQiaoFoundation.BusinessResources.Constants;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.Shared.ControllerBase;
using XueQiaoFoundation.Shared.Model;

namespace Research.app.Controller
{
    /// <summary>
    /// 投研工作空间页管理
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class ResearchWorkspaceItemCtrl : SwitchablePageControllerBase, IWorkspaceItemViewCtrl
    {
        private const double UrlComponentWidthDefault = 800d;
        private const double UrlComponentHeightDefault = 600d;

        private readonly ResearchWorkspaceVM contentVM;
        private readonly IDraggableComponentPanelContextCtrl componentPanelContextCtrl;
        private readonly ExportFactory<ResearchUrlComponentCtrl> urlComponentCtrlFactory;

        private readonly DelegateCommand addUrlComponentCmd;
        private readonly DelegateCommand loadAllUrlsCmd;
        private readonly DelegateCommand hideAllUrlsCmd;
        private readonly DelegateCommand showAllUrlsCmd;
        
        private readonly Dictionary<DraggableComponentUIDM, IResearchComponentCtrl>
            componentControllers = new Dictionary<DraggableComponentUIDM, IResearchComponentCtrl>();

        [ImportingConstructor]
        public ResearchWorkspaceItemCtrl(
            ResearchWorkspaceVM contentVM,
            IDraggableComponentPanelContextCtrl componentPanelContextCtrl,
            ExportFactory<ResearchUrlComponentCtrl> urlComponentCtrlFactory)
        {
            this.contentVM = contentVM;
            this.componentPanelContextCtrl = componentPanelContextCtrl;
            this.urlComponentCtrlFactory = urlComponentCtrlFactory;

            addUrlComponentCmd = new DelegateCommand(AddUrlComponent);
            loadAllUrlsCmd = new DelegateCommand(LoadAllUrls);
            hideAllUrlsCmd = new DelegateCommand(HideAllUrls);
            showAllUrlsCmd = new DelegateCommand(ShowAllUrls);
        }

        public TabWorkspace Workspace { get; set; }
        
        public object ContentView => contentVM.View;

        protected override void DoInitialize()
        {
            if (Workspace == null) throw new ArgumentNullException("Property `Workspace` must be setted value before Initialize.");

            componentPanelContextCtrl.ComponentPanelContext = contentVM.DraggableComponentPanelContext;
            componentPanelContextCtrl.Initialize();

            contentVM.AddUrlComponentCmd = addUrlComponentCmd;
            contentVM.LoadAllUrlsCmd = loadAllUrlsCmd;
            contentVM.HideAllUrlsCmd = hideAllUrlsCmd;
            contentVM.ShowAllUrlsCmd = showAllUrlsCmd;
        }

        protected override void DoRun()
        {
            InvalidateComponentsDisplay();
        }

        protected override void DoShutdown()
        {
            componentPanelContextCtrl?.Shutdown();
            ShutdownAllComponents();
        }
        
        private void AddUrlComponent()
        {
            var newComp = new ResearchComponent
            {
                ComponentType = XueQiaoConstants.ResearchCompType_URL,
                Width = UrlComponentWidthDefault,
                Height = UrlComponentHeightDefault
            };

            // add into research component list 
            Workspace.ResearchComponents.Add(newComp);
            AddViewForUrlComponent(newComp, out DraggableComponentUIDM addedItem);

            componentPanelContextCtrl.PositionComponent(addedItem, null);
            componentPanelContextCtrl.FocusOnComponent(addedItem);
        }

        private ResearchUrlComponentCtrl[] GetAllResearchUrlComponentCtrls()
        {
            return componentControllers.Values.OfType<ResearchUrlComponentCtrl>().ToArray();
        }

        private void LoadAllUrls()
        {
            foreach (var ctrl in GetAllResearchUrlComponentCtrls())
            {
                var url = ctrl.Component?.UrlCompDetail?.Url;
                if(url != null)
                    ctrl.LoadUrl(url);
            }
        }

        private void HideAllUrls()
        {
            foreach (var ctrl in GetAllResearchUrlComponentCtrls())
            {
                ctrl.HideUrlInfo();
            }
        }

        private void ShowAllUrls()
        {
            foreach (var ctrl in GetAllResearchUrlComponentCtrls())
            {
                ctrl.ShowUrlInfo();
            }
        }

        private void InvalidateComponentsDisplay()
        {
            if (Workspace != null)
            {
                foreach (var comp in Workspace.ResearchComponents)
                {
                    if (comp.ComponentType == XueQiaoConstants.ResearchCompType_URL)
                    {
                        AddViewForUrlComponent(comp, out DraggableComponentUIDM ignore);
                    }
                }
            }
        }

        private void ShutdownAllComponents()
        {
            foreach (var ctrlItem in componentControllers)
            {
                componentPanelContextCtrl.RemoveComponent(ctrlItem.Key);
                ctrlItem.Value.Shutdown();
            }
            componentControllers.Clear();
        }

        private void CloseComponentWithDM(DraggableComponentUIDM compDataModel)
        {
            if (compDataModel == null) return;

            if (compDataModel.Component is ResearchComponent)
            {
                Workspace.ResearchComponents.Remove((ResearchComponent)compDataModel.Component);
            }

            if (componentControllers.TryGetValue(compDataModel, out IResearchComponentCtrl controller))
            {
                this.componentControllers.Remove(compDataModel);
                controller.Shutdown();
                componentPanelContextCtrl.RemoveComponent(compDataModel);
            }
        }

        private void AddViewForUrlComponent(ResearchComponent urlComp,
            out DraggableComponentUIDM addedComponentItem)
        {
            var ctrl = urlComponentCtrlFactory.CreateExport().Value;
            ctrl.Component = urlComp;
            ctrl.ParentWorkspace = this.Workspace;
            ctrl.CloseComponentHandler = (_ctrl) => {
                CloseComponentWithDM(ctrl.ComponentItemDataModel);
            };

            ctrl.Initialize();
            ctrl.Run();

            componentControllers[ctrl.ComponentItemDataModel] = ctrl;
            componentPanelContextCtrl.AddComponent(ctrl.ComponentItemDataModel);

            addedComponentItem = ctrl.ComponentItemDataModel;
        }
    }
}
