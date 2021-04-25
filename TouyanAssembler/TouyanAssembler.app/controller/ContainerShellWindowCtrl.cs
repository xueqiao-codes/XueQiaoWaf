using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows;
using System.Windows.Media;
using TouyanAssembler.app.viewmodel;
using TouyanAssembler.Interface.application;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.Shared.Model;

namespace TouyanAssembler.app.controller
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class ContainerShellWindowCtrl : IController
    {
        private readonly ContainerShellVM shellViewModel;
        private readonly IContainerShellModuleService containerShellModuleService;
        private readonly ExportFactory<AppInfoDialogCtrl> appInfoDialogCtrlFactory;

        private readonly ChromeWindowCaptionDataHolder windowCaptionDataHolder = new ChromeWindowCaptionDataHolder();
        private readonly DelegateCommand showAppInfoCmd;

        private AppModuleTabNode lastSelectedTabNode;

        [ImportingConstructor]
        public ContainerShellWindowCtrl(ContainerShellVM shellViewModel,
            IContainerShellModuleService containerShellModuleService,
            ExportFactory<AppInfoDialogCtrl> appInfoDialogCtrlFactory)
        {
            this.shellViewModel = shellViewModel;
            this.containerShellModuleService = containerShellModuleService;
            this.appInfoDialogCtrlFactory = appInfoDialogCtrlFactory;

            showAppInfoCmd = new DelegateCommand(ShowAppInfoView);
        }

        public event CancelEventHandler ShellClosing;

        public event EventHandler ShellClosed;

        /// <summary>
        /// 获取到当前 Shell 窗体
        /// </summary>
        public Window ShellWindow => shellViewModel.View as Window;

        public void Initialize()
        {
            shellViewModel.Closing += OnShellClosing;
            shellViewModel.Closed += OnShellClosed;

            shellViewModel.WindowCaptionDataHolder = this.windowCaptionDataHolder;
            shellViewModel.ShowAppInfoCmd = showAppInfoCmd;

            PropertyChangedEventManager.AddHandler(shellViewModel, ShellViewModelPropertyChanged, "");
        }

        public void Run()
        {
            shellViewModel.Show();
            SetupTabNodes();
        }

        public void Shutdown()
        {
            shellViewModel.Closing -= OnShellClosing;
            shellViewModel.Closed -= OnShellClosed;

            PropertyChangedEventManager.RemoveHandler(shellViewModel, ShellViewModelPropertyChanged, "");

            foreach (var node in shellViewModel.TabNodes)
            {
                node.ShowAction = null;
                node.CloseAction = null;
            }
            shellViewModel.TabNodes.Clear();
            shellViewModel.SelectedTabNode = null;
            shellViewModel.TabNodeContentView = null;
        }

        private void OnShellClosing(object sender, CancelEventArgs e)
        {
            ShellClosing?.Invoke(this, e);
        }

        private void OnShellClosed(object sender, EventArgs e)
        {
            ShellClosed?.Invoke(this, e);
        }

        private void ShellViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ContainerShellVM.SelectedTabNode))
            {
                var selectedTabNode = shellViewModel.SelectedTabNode;
                if (selectedTabNode != null)
                {
                    selectedTabNode.ShowAction?.Invoke(selectedTabNode);
                }

                if (this.lastSelectedTabNode != null)
                {
                    this.lastSelectedTabNode.CloseAction?.Invoke(lastSelectedTabNode);
                }
                this.lastSelectedTabNode = selectedTabNode;
            }
        }

        private void ShowTabNodeModuleRootView(AppModuleTabNode moduleTabNode, 
            object moduleRootView, Action showAction)
        {
            if (moduleRootView != null)
            {
                shellViewModel.TabNodeContentView = moduleRootView;
            }
            showAction?.Invoke();
        }
        
        private void SetupTabNodes()
        {
            shellViewModel.TabNodes.Add(new AppModuleTabNode
            {
                NodeTitle = "交易",
                NodeIconGeometry = CreateTradeTabNodeGeometry(),
                ShowAction = tab =>
                {
                    containerShellModuleService.GetTradeModuleRootView(() => this.windowCaptionDataHolder,
                        out object moduleRootView, out Action showAction, out Action closeAction);

                    tab.CloseAction = _tab => closeAction?.Invoke();
                    ShowTabNodeModuleRootView(tab, moduleRootView, showAction);
                }
            });

            shellViewModel.TabNodes.Add(new AppModuleTabNode
            {
                NodeTitle = "投研",
                NodeIconGeometry = CreateResearchTabNodeGeometry(),
                ShowAction = tab =>
                {
                    containerShellModuleService.GetTouyanModuleRootView(() => this.windowCaptionDataHolder,
                        out object moduleRootView, out Action showAction, out Action closeAction);

                    tab.CloseAction = _tab => closeAction?.Invoke();
                    ShowTabNodeModuleRootView(tab, moduleRootView, showAction);
                }
            });

            // 设置选中项
            shellViewModel.SelectedTabNode = shellViewModel.TabNodes.LastOrDefault();
        }
        
        private static Geometry CreateTradeTabNodeGeometry()
        {
            var iconGeometry = ResourceDictionaryHelper.FindResource("MainTabTradeIconGeometry");
            return iconGeometry as Geometry;
        }

        private static Geometry CreateResearchTabNodeGeometry()
        {
            var iconGeometry = ResourceDictionaryHelper.FindResource("MainTabResearchIconGeometry");
            return iconGeometry as Geometry;
        }

        private void ShowAppInfoView()
        {
            var dialogCtrl = appInfoDialogCtrlFactory.CreateExport().Value;
            dialogCtrl.DialogOwner = UIHelper.GetWindowOfUIElement(shellViewModel.View);

            dialogCtrl.Initialize();
            dialogCtrl.Run();
            dialogCtrl.Shutdown();
        }
    }
}
