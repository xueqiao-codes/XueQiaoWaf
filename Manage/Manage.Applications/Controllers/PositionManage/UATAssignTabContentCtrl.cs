using Manage.Applications.DataModels;
using Manage.Applications.ServiceControllers;
using Manage.Applications.Services;
using Manage.Applications.ViewModels;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using XueQiaoFoundation.BusinessResources.Helpers;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;

namespace Manage.Applications.Controllers
{
    /// <summary>
    /// 未分配的`分配`tab 内容 controller
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class UATAssignTabContentCtrl : IController
    {
        private readonly IManageFundAccountItemsController manageFundAccountItemsCtrl;
        private readonly UATPAContractSummaryService UATPAContractSummaryService;
        private readonly IUATPAController UATPAController;
        private readonly ILoginDataService loginDataService;
        private readonly IMessageWindowService messageWindowService;
        private readonly IEventAggregator eventAggregator;
        private readonly UATAssignTabContentVM contentViewModel;
        private readonly ExportFactory<UATPreviewAssignPopupCtrl> previewAssignPopupCtrlFactory;

        [ImportingConstructor]
        public UATAssignTabContentCtrl(
            IManageFundAccountItemsController manageFundAccountItemsCtrl,
            UATPAContractSummaryService UATPAContractSummaryService,
            IUATPAController UATPAController,
            ILoginDataService loginDataService,
            IMessageWindowService messageWindowService,
            IEventAggregator eventAggregator,
            UATAssignTabContentVM contentViewModel,
            ExportFactory<UATPreviewAssignPopupCtrl> previewAssignPopupCtrlFactory)
        {
            this.manageFundAccountItemsCtrl = manageFundAccountItemsCtrl;
            this.UATPAContractSummaryService = UATPAContractSummaryService;
            this.UATPAController = UATPAController;
            this.loginDataService = loginDataService;
            this.messageWindowService = messageWindowService;
            this.eventAggregator = eventAggregator;
            this.contentViewModel = contentViewModel;
            this.previewAssignPopupCtrlFactory = previewAssignPopupCtrlFactory;
        }

        public object ContentView => contentViewModel.View;

        public void Initialize()
        {
            manageFundAccountItemsCtrl.RefreshFundAccountItemsIfNeed();

            contentViewModel.TriggerBatchPreviewAssignHandler = _args => TriggerBatchPreviewAssign(_args);
            contentViewModel.TriggerSingleUATItemPreviewAssignHandler = _args => TriggerPreviewAssignUATItem(_args);
            contentViewModel.TriggerBatchRemovePAItemsHandler = _args => TriggerBatchRemovePAItems(_args);
            contentViewModel.TriggerSingleRemovePAItemHandler = _args => TriggerSingleRemovePAItem(_args);
        }

        public void Run()
        {
           
        }

        public void Shutdown()
        {
            contentViewModel.TriggerBatchPreviewAssignHandler = null;
            contentViewModel.TriggerSingleUATItemPreviewAssignHandler = null;
            contentViewModel.TriggerBatchRemovePAItemsHandler = null;
            contentViewModel.TriggerSingleRemovePAItemHandler = null;
        }

        private void TriggerBatchPreviewAssign(TriggerBatchPreviewAssignArgs args)
        {
            if (args == null) return;
            var popupCtrl = previewAssignPopupCtrlFactory.CreateExport().Value;
            popupCtrl.PopupPalcementTarget = args.TriggerElement;
            popupCtrl.IsShowAssignQuantityInputBox = false;

            popupCtrl.PopupCloseHandler = (_ctrl, _PAInfo) => 
            {
                if (_PAInfo != null && args.UATItems?.Any() == true)
                {
                    foreach (var _UATItem in args.UATItems)
                    {
                        var itemUnpreviewVolume = _UATItem.UnpreviewAssignVolume;
                        if (itemUnpreviewVolume > 0)
                        {
                            var _PAItemkey = new PositionPreviewAssignItemKey(_PAInfo.SelectedSubAccountId, _UATItem.ItemKey, _UATItem.FundAccountId, _UATItem.ContractId);
                            UATPAController.AddOrUpdatePAItem(_PAItemkey, existedPAItem => 
                            {
                                var updateTemplate = new PAItemUpdateTemplate();
                                if (existedPAItem != null)
                                    updateTemplate.Volume = existedPAItem.Volume + itemUnpreviewVolume;
                                else
                                    updateTemplate.Volume = itemUnpreviewVolume;
                                return updateTemplate;
                            });
                        }
                    }
                    args.Handled = true;
                }

                _ctrl.Shutdown();
            };

            popupCtrl.Initialize();
            popupCtrl.Run();
        }

        private void TriggerPreviewAssignUATItem(TriggerSingleUATItemPreviewAssignArgs args)
        {
            if (args == null) return;
            var optUATItem = args.UATItem;
            if (optUATItem == null) return;
            
            var popupCtrl = previewAssignPopupCtrlFactory.CreateExport().Value;
            popupCtrl.PopupPalcementTarget = args.TriggerElement;
            popupCtrl.IsShowAssignQuantityInputBox = true;
            popupCtrl.MaxAssignQuantity = optUATItem.UnpreviewAssignVolume;

            popupCtrl.PopupCloseHandler = (_ctrl, _PAInfo) =>
            {
                if (_PAInfo != null)
                {
                    var newAssignQuantity = Math.Min(_PAInfo.AssignQuantity, optUATItem.UnpreviewAssignVolume);
                    if (newAssignQuantity > 0)
                    {
                        var _PAItemkey = new PositionPreviewAssignItemKey(_PAInfo.SelectedSubAccountId, optUATItem.ItemKey, optUATItem.FundAccountId, optUATItem.ContractId);
                        UATPAController.AddOrUpdatePAItem(_PAItemkey, existedPAItem =>
                        {
                            var updateTemplate = new PAItemUpdateTemplate();
                            if (existedPAItem != null)
                                updateTemplate.Volume = existedPAItem.Volume + newAssignQuantity;
                            else
                                updateTemplate.Volume = newAssignQuantity;
                            return updateTemplate;
                        });
                    }
                    args.Handled = true;
                }

                _ctrl.Shutdown();
            };

            popupCtrl.Initialize();
            popupCtrl.Run();
        }

        private void TriggerBatchRemovePAItems(TriggerBatchRemovePAItemsArgs args)
        {
            if (args == null) return;
            if (args.PAItems?.Any() != true) return;

            var containerWin = UIHelper.GetWindowOfUIElement(contentViewModel.View) as Window;
            if (true != messageWindowService.ShowQuestionDialog(containerWin, null, null, null, "确认要移除选中预分配吗？", false, "移除", "取消"))
                return;

            var rmPAItemKeys = new List<PositionPreviewAssignItemKey>();
            foreach (var rmPaItem in args.PAItems)
            {
                rmPAItemKeys.Add(new PositionPreviewAssignItemKey(rmPaItem.SubAccountId, rmPaItem.UATItemKey, rmPaItem.FundAccountId, rmPaItem.ContractId));
            }
            UATPAController.RemovePAItemsWithKey(rmPAItemKeys.ToArray());

            args.Handled = true;
        }

        private void TriggerSingleRemovePAItem(TriggerSingleRemovePAItemArgs args)
        {
            if (args == null) return;
            var rmPAItem = args.PAItem;
            if (rmPAItem == null) return;

            var rmPAItemKeys = new List<PositionPreviewAssignItemKey>();
            rmPAItemKeys.Add(new PositionPreviewAssignItemKey(rmPAItem.SubAccountId, rmPAItem.UATItemKey, rmPAItem.FundAccountId, rmPAItem.ContractId));
            UATPAController.RemovePAItemsWithKey(rmPAItemKeys.ToArray());

            args.Handled = true;
        }
    }
}
