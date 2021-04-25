using NativeModel.Trade;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows;
using System.Windows.Controls.Primitives;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.UI.Components.Popup;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Helper;
using XueQiaoWaf.Trade.Modules.Applications.ServiceControllers;
using XueQiaoWaf.Trade.Modules.Applications.ViewModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class PositionDiscreteListPageController : IController
    {
        private readonly PositionDiscreteListPageViewModel pageViewModel;
        private readonly ExportFactory<PositionDiscreteTradeDetailDialogController> positionDetailDialogCtrlFactory;
        private readonly ExportFactory<IPopup> popupFactory;
        private readonly ExportFactory<XqTargetSelectStopLostOrProfitVM> selectStopLostOrProfitVMFactory;
        private readonly XqTargetOfItemSubscribeQuotationHelpCtrl xqTargetOfItemSubscribeQuotationHelpCtrl;
        private readonly IPositionDiscreteItemsController positionDiscreteItemsCtrl;

        private readonly DelegateCommand clickItemTargetKeyRelatedColumnCmd;
        private readonly DelegateCommand toStopLostOrProfitCmd;
        private readonly DelegateCommand toShowPositionDetailCmd;

        // 列表选中项变化的 command
        private readonly DelegateCommand listItemsSelectionChangedCmd;
        // 删除选中的过期持仓 command
        private readonly DelegateCommand selectedItemsDeleteExpiredCmd;
        // 订阅选中的标的行情
        private readonly DelegateCommand selectedItemsSubscribeQuotationCmd;
        // 列表当前选中的项目
        private IEnumerable<PositionDiscreteItemDataModel> listSelectedPositionItems;

        [ImportingConstructor]
        public PositionDiscreteListPageController(PositionDiscreteListPageViewModel pageViewModel,
            ExportFactory<PositionDiscreteTradeDetailDialogController> positionDetailDialogCtrlFactory,
            ExportFactory<IPopup> popupFactory,
            ExportFactory<XqTargetSelectStopLostOrProfitVM> selectStopLostOrProfitVMFactory,
            XqTargetOfItemSubscribeQuotationHelpCtrl xqTargetOfItemSubscribeQuotationHelpCtrl,
            IPositionDiscreteItemsController positionDiscreteItemsCtrl)
        {
            this.pageViewModel = pageViewModel;
            this.positionDetailDialogCtrlFactory = positionDetailDialogCtrlFactory;
            this.popupFactory = popupFactory;
            this.selectStopLostOrProfitVMFactory = selectStopLostOrProfitVMFactory;
            this.xqTargetOfItemSubscribeQuotationHelpCtrl = xqTargetOfItemSubscribeQuotationHelpCtrl;
            this.positionDiscreteItemsCtrl = positionDiscreteItemsCtrl;

            clickItemTargetKeyRelatedColumnCmd = new DelegateCommand(ClickItemTargetKeyRelatedColumn);
            toStopLostOrProfitCmd = new DelegateCommand(ToStopLostOrProfit);
            toShowPositionDetailCmd = new DelegateCommand(ToShowPositionDetail);

            listItemsSelectionChangedCmd = new DelegateCommand(ListItemsSelectionChanged);
            selectedItemsDeleteExpiredCmd = new DelegateCommand(SelectedItemsDeleteExpired, CanSelectedItemsDeleteExpired);
            selectedItemsSubscribeQuotationCmd = new DelegateCommand(SelectedItemsSubscribeQuotation, CanSelectedItemsSubscribeQuotation);
        }
        
        public XueQiaoFoundation.BusinessResources.DataModels.TabWorkspace ParentWorkspace { get; set; }
        public ITradeComponentController ParentComponentCtrl { get; set; }

        public object ComponentContentView => pageViewModel.View;

        public void Initialize()
        {
            if (ParentWorkspace == null) throw new ArgumentNullException("ParentWorkspace");
            if (ParentComponentCtrl == null) throw new ArgumentNullException("ParentComponentCtrl");

            pageViewModel.ClickItemTargetKeyRelatedColumnCmd = clickItemTargetKeyRelatedColumnCmd;
            pageViewModel.ToStopLostOrProfitCmd = toStopLostOrProfitCmd;
            pageViewModel.ToShowPositionDetailCmd = toShowPositionDetailCmd;

            xqTargetOfItemSubscribeQuotationHelpCtrl.XqTargetOfItemParser = obj =>
            {
                var item = obj as PositionDiscreteItemDataModel;
                if (item == null) return null;
                return new XqTargetInfo { TargetType = ClientXQOrderTargetType.CONTRACT_TARGET, TargetKey = $"{item.ContractId}" };
            };
            xqTargetOfItemSubscribeQuotationHelpCtrl.MsgDisplayWindowOwnerGetter = () => UIHelper.GetWindowOfUIElement(pageViewModel.View);
            xqTargetOfItemSubscribeQuotationHelpCtrl.Initialize();

            pageViewModel.ListItemsSelectionChangedCmd = listItemsSelectionChangedCmd;
            pageViewModel.SelectedItemsDeleteExpiredCmd = selectedItemsDeleteExpiredCmd;
            pageViewModel.SelectedItemsSubscribeQuotationCmd = selectedItemsSubscribeQuotationCmd;

            pageViewModel.UpdatePresentSubAccountId(ParentWorkspace.SubAccountId);
            PropertyChangedEventManager.AddHandler(ParentWorkspace, ParentWorkspacePropertyChanged, "");
        }

        public void Run()
        {
        }

        public void Shutdown()
        {
            xqTargetOfItemSubscribeQuotationHelpCtrl.Shutdown();
            PropertyChangedEventManager.RemoveHandler(ParentWorkspace, ParentWorkspacePropertyChanged, "");
        }

        private void ParentWorkspacePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(XueQiaoFoundation.BusinessResources.DataModels.TabWorkspace.SubAccountId))
            {
                pageViewModel.UpdatePresentSubAccountId(ParentWorkspace.SubAccountId);
            }
        }

        private void ListItemsSelectionChanged(object obj)
        {
            var oldSelItems = this.listSelectedPositionItems;
            if (oldSelItems != null)
            {
                foreach (var i in oldSelItems)
                {
                    PropertyChangedEventManager.RemoveHandler(i, SelectedPositionItemPropChanged, "");
                }
            }

            var newSelItems = (obj as IList)?.Cast<PositionDiscreteItemDataModel>().ToArray();
            this.listSelectedPositionItems = newSelItems;

            if (newSelItems != null)
            {
                foreach (var i in newSelItems)
                {
                    PropertyChangedEventManager.AddHandler(i, SelectedPositionItemPropChanged, "");
                }
            }

            selectedItemsDeleteExpiredCmd?.RaiseCanExecuteChanged();
            selectedItemsSubscribeQuotationCmd?.RaiseCanExecuteChanged();
        }

        private void SelectedPositionItemPropChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PositionDiscreteItemDataModel.IsXqTargetExpired))
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    selectedItemsDeleteExpiredCmd?.RaiseCanExecuteChanged();
                    selectedItemsSubscribeQuotationCmd?.RaiseCanExecuteChanged();
                });
            }
        }

        private bool CanSelectedItemsDeleteExpired()
        {
            return listSelectedPositionItems?.Any(i => i.IsXqTargetExpired == true) ?? false;
        }

        private void SelectedItemsDeleteExpired()
        {
            var targetExpiredItems = listSelectedPositionItems?.Where(i => i.IsXqTargetExpired == true).ToArray();
            if (targetExpiredItems?.Any() != true) return;

            foreach (var i in targetExpiredItems)
            {
                positionDiscreteItemsCtrl.RequestDeleteExpiredPosition(new PositionDiscreteItemKey(i.ContractId, i.SubAccountFields.SubAccountId));
            }
        }

        private bool CanSelectedItemsSubscribeQuotation()
        {
            // 只允许每次订阅一个
            if (listSelectedPositionItems?.Count() != 1) return false;
            var tarItem = listSelectedPositionItems?.FirstOrDefault();
            if (tarItem == null) return false;
            return tarItem.IsXqTargetExpired != true;
        }

        private void SelectedItemsSubscribeQuotation()
        {
            var tarItem = listSelectedPositionItems?.FirstOrDefault();
            if (tarItem == null) return;

            xqTargetOfItemSubscribeQuotationHelpCtrl.DoSubscribeTargetQuotation(tarItem);
        }

        private void ToShowPositionDetail(object obj)
        {
            var positionItem = obj as PositionDiscreteItemDataModel;
            if (positionItem == null) return;

            var dialogCtrl = positionDetailDialogCtrlFactory.CreateExport().Value;
            dialogCtrl.DialogOwner = UIHelper.GetWindowOfUIElement(pageViewModel.View);
            dialogCtrl.SourcePositionItem = positionItem;

            dialogCtrl.Initialize();
            dialogCtrl.Run();
            dialogCtrl.Shutdown();
        }

        private void ClickItemTargetKeyRelatedColumn(object obj)
        {
            var item = obj as PositionDiscreteItemDataModel;
            if (item == null) return;
            HandleXqTargetAssociate(item, null);
        }

        private bool CanRemovePositionItem(object obj)
        {
            var item = obj as PositionDiscreteItemDataModel;
            if (item == null) return false;
            return item.IsXqTargetExpired == true;
        }
        
        private void ToStopLostOrProfit(object param)
        {
            var args = param as object[];
            if (args == null || args.Length != 2) return;

            var item = args[0] as PositionDiscreteItemDataModel;
            var triggerElement = args[1] as UIElement;
            if (item == null || triggerElement == null) return;

            var vm = selectStopLostOrProfitVMFactory.CreateExport().Value;
            var popup = popupFactory.CreateExport().Value;
            popup.StaysOpen = false;
            
            popup.Placement = PlacementMode.Bottom;
            popup.PlacementTarget = triggerElement;
            popup.Content = vm.View as UIElement;

            vm.ToStopLostBuyHandler = () => 
            {
                popup?.Close();
                HandleXqTargetAssociate(item, ClientPlaceOrderType.BUY_STOP_LOST);
            };
            vm.ToStopLostSellHandler = () =>
            {
                popup?.Close();
                HandleXqTargetAssociate(item, ClientPlaceOrderType.SELL_STOP_LOST);
            };
            vm.ToStopProfitBuyHandler = () =>
            {
                popup?.Close();
                HandleXqTargetAssociate(item, ClientPlaceOrderType.BUY_STOP_PROFIT);
            };
            vm.ToStopProfitSellHandler = () =>
            {
                popup?.Close();
                HandleXqTargetAssociate(item, ClientPlaceOrderType.SELL_STOP_PROFIT);
            };
            
            popup.Open();
        }

        private void HandleXqTargetAssociate(PositionDiscreteItemDataModel item, ClientPlaceOrderType? placeOrderType)
        {
            if (item == null) return;

            var associateCustomInfos = new Dictionary<string, object>();
            if (placeOrderType != null)
                associateCustomInfos.Add(TradeComponentAssociateConstants.ComponentAssociateArg_PlaceOrderType, placeOrderType.Value);

            // 联动
            var associateArgs = new TradeComponentXqTargetAssociateArgs(ParentComponentCtrl.ParentWorkspace, ParentComponentCtrl.Component,
                ClientXQOrderTargetType.CONTRACT_TARGET, $"{item.ContractId}", associateCustomInfos);

            var previewAssociateDialogOwner = UIHelper.GetWindowOfUIElement(pageViewModel.View);
            Point? previewAssociateDialogLocation = null;
            ParentComponentCtrl.XqTargetAssociateHandler?.HandleXqTargetAssociate(previewAssociateDialogOwner, previewAssociateDialogLocation, associateArgs);
        }
    }
}
