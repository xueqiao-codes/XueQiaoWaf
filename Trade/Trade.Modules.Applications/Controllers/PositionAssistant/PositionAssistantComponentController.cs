using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoWaf.Trade.Modules.Applications.ViewModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers
{
    /// <summary>
    /// 持仓助手组件 controller
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class PositionAssistantComponentController : IController
    {
        private readonly PositionAssistantComponentContainerModel containerViewModel;
        private readonly ExportFactory<PositionOfContractController> positionOfContractCtrlFactory;
        private readonly ExportFactory<PositionOfComposeController> positionOfComposeCtrlFactory;
        private readonly ExportFactory<PositionOfComposeAddDialogCtrl> composePositionAddCtrlFactory;
        private readonly ExportFactory<XqTargetClosePositionSearchDialogCtrl> xqTargetClosePositionSearchDialogCtrlFactory;
        private readonly ExportFactory<PositionOfComposeMergeWindowCtrl> _CPMergeWindowCtrlFactory;

        private PositionOfComposeMergeWindowCtrl currentCPMergeWindowCtrl;

        private PositionOfContractController contractPositionCtrl;
        private PositionOfComposeController composePositionCtrl;

        private readonly DelegateCommand toInputComposePositionCmd;
        private readonly DelegateCommand toMerge2ComposePositionCmd;
        private readonly DelegateCommand showClosePositionSearchPageCmd;

        [ImportingConstructor]
        public PositionAssistantComponentController(
            PositionAssistantComponentContainerModel containerViewModel,
            ExportFactory<PositionOfContractController> positionOfContractCtrlFactory,
            ExportFactory<PositionOfComposeController> positionOfComposeCtrlFactory,
            ExportFactory<PositionOfComposeAddDialogCtrl> composePositionAddCtrlFactory,
            ExportFactory<XqTargetClosePositionSearchDialogCtrl> xqTargetClosePositionSearchDialogCtrlFactory,
            ExportFactory<PositionOfComposeMergeWindowCtrl> _CPMergeWindowCtrlFactory)
        {
            this.containerViewModel = containerViewModel;
            this.positionOfContractCtrlFactory = positionOfContractCtrlFactory;
            this.positionOfComposeCtrlFactory = positionOfComposeCtrlFactory;
            this.composePositionAddCtrlFactory = composePositionAddCtrlFactory;
            this.xqTargetClosePositionSearchDialogCtrlFactory = xqTargetClosePositionSearchDialogCtrlFactory;
            this._CPMergeWindowCtrlFactory = _CPMergeWindowCtrlFactory;

            toInputComposePositionCmd = new DelegateCommand(ToInputComposePosition);
            toMerge2ComposePositionCmd = new DelegateCommand(ToMerge2ComposePosition);
            showClosePositionSearchPageCmd = new DelegateCommand(ShowClosePositionSearchPage);
        }

        public XueQiaoFoundation.BusinessResources.DataModels.TabWorkspace ParentWorkspace { get; set; }
        public ITradeComponentController ParentComponentCtrl { get; set; }

        public object ComponentContentView => containerViewModel.View;

        public void Initialize()
        {
            if (ParentWorkspace == null) throw new ArgumentNullException("ParentWorkspace");
            if (ParentComponentCtrl == null) throw new ArgumentNullException("ParentComponentCtrl");

            PropertyChangedEventManager.AddHandler(containerViewModel, ContainerViewModelPropertyChanged, "");

            containerViewModel.ToInputComposePositionCmd = toInputComposePositionCmd;
            containerViewModel.ToMerge2ComposePositionCmd = toMerge2ComposePositionCmd;
            containerViewModel.ShowClosePositionSearchPageCmd = showClosePositionSearchPageCmd;
            containerViewModel.SelectedContentType = PositionAssistantContentType.CONTRACT;            
        }

        public void Run()
        {

        }

        public void Shutdown()
        {
            PropertyChangedEventManager.RemoveHandler(containerViewModel, ContainerViewModelPropertyChanged, "");
            currentCPMergeWindowCtrl?.Shutdown();
        }

        private void ContainerViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PositionAssistantComponentContainerModel.SelectedContentType))
            {
                var selectedContentType = containerViewModel.SelectedContentType;

                if (selectedContentType == PositionAssistantContentType.CONTRACT)
                {
                    ShowPositionOfContractInContainer();
                }
                else if (selectedContentType == PositionAssistantContentType.COMPOSE)
                {
                    ShowPositionOfComposeInContainer();
                }
                return;
            }
        }
        
        private void ToInputComposePosition()
        {
            var dialogCtrl = composePositionAddCtrlFactory.CreateExport().Value;
            dialogCtrl.DialogOwner = UIHelper.GetWindowOfUIElement(containerViewModel.View);
            dialogCtrl.SubAccountId = this.ParentWorkspace.SubAccountId;

            dialogCtrl.Initialize();
            dialogCtrl.Run();
            dialogCtrl.Shutdown();
        }

        private void ToMerge2ComposePosition()
        {
            this.currentCPMergeWindowCtrl?.Shutdown();

            var windowCtrl = _CPMergeWindowCtrlFactory.CreateExport().Value;
            windowCtrl.WindowOwner = UIHelper.GetWindowOfUIElement(containerViewModel.View);
            windowCtrl.SubAccountId = this.ParentWorkspace.SubAccountId;

            windowCtrl.Initialize();
            windowCtrl.Run();

            this.currentCPMergeWindowCtrl = windowCtrl;
        }

        private void ShowClosePositionSearchPage(object param)
        {
            var contentType = param as PositionAssistantContentType?;
            if (contentType == null) return;

            ClientXQOrderTargetType? xqTargetType = null;
            if (contentType == PositionAssistantContentType.CONTRACT)
                xqTargetType = ClientXQOrderTargetType.CONTRACT_TARGET;
            else if (contentType == PositionAssistantContentType.COMPOSE)
                xqTargetType = ClientXQOrderTargetType.COMPOSE_TARGET;
            if (xqTargetType == null) return;

            var dialogCtrl = xqTargetClosePositionSearchDialogCtrlFactory.CreateExport().Value;
            dialogCtrl.DialogOwner = UIHelper.GetWindowOfUIElement(containerViewModel.View);
            dialogCtrl.SubAccountId = ParentWorkspace.SubAccountId;
            dialogCtrl.InitialSelectedXqTargetType = xqTargetType.Value;

            dialogCtrl.Initialize();
            dialogCtrl.Run();
            dialogCtrl.Shutdown();
        }

        private void ShowPositionOfContractInContainer()
        {
            if (contractPositionCtrl == null)
            {
                var ctrl = positionOfContractCtrlFactory.CreateExport().Value;
                ctrl.ParentWorkspace = this.ParentWorkspace;
                ctrl.ParentComponentCtrl = this.ParentComponentCtrl;
                ctrl.Initialize();
                ctrl.Run();

                this.contractPositionCtrl = ctrl;
            }
            // change the content display
            containerViewModel.AssistantContentView = contractPositionCtrl.ContentView;
        }

        private void ShowPositionOfComposeInContainer()
        {
            if (composePositionCtrl == null)
            {
                var ctrl = positionOfComposeCtrlFactory.CreateExport().Value;
                ctrl.ParentWorkspace = this.ParentWorkspace;
                ctrl.ParentComponentCtrl = this.ParentComponentCtrl;
                ctrl.Initialize();
                ctrl.Run();

                this.composePositionCtrl = ctrl;
            }
            // change the content display
            containerViewModel.AssistantContentView = composePositionCtrl.ContentView;
        }
    }
}
