using NativeModel.Trade;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows;
using System.Windows.Controls.Primitives;
using ToastNotifications.Core;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.UI.Components.Popup;
using XueQiaoFoundation.UI.Components.ToastNotification;
using XueQiaoFoundation.UI.Components.ToastNotification.Impl;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Helper;
using XueQiaoWaf.Trade.Modules.Applications.ServiceControllers;
using XueQiaoWaf.Trade.Modules.Applications.ViewModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class PositionOfContractController : IController
    {
        private readonly PositionOfContractViewModel contentVM;
        private readonly ExportFactory<XqTargetPositionDetailDialogCtrl> xqTargetPositionDetailDialogCtrlFactory;
        private readonly ExportFactory<IPopup> popupFactory;
        private readonly ExportFactory<XqTargetSelectStopLostOrProfitVM> selectStopLostOrProfitVMFactory;
        private readonly XqTargetOfItemSubscribeQuotationHelpCtrl xqTargetOfItemSubscribeQuotationHelpCtrl;
        private readonly IXqTargetPositionItemsController xqTargetPositionItemsCtrl;
        private readonly ExportFactory<SimpleMessageToastNDPVM> simpleMessageToastNDPVMFactory;

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
        private IEnumerable<TargetPositionDataModel> listSelectedPositionItems;

        private NotifierWrapper_ControlPositionProvider viewToastNotifierWrapper;
        
        [ImportingConstructor]
        public PositionOfContractController(
            PositionOfContractViewModel contentVM,
            ExportFactory<XqTargetPositionDetailDialogCtrl> xqTargetPositionDetailDialogCtrlFactory,
            ExportFactory<IPopup> popupFactory,
            ExportFactory<XqTargetSelectStopLostOrProfitVM> selectStopLostOrProfitVMFactory,
            XqTargetOfItemSubscribeQuotationHelpCtrl xqTargetOfItemSubscribeQuotationHelpCtrl,
            IXqTargetPositionItemsController xqTargetPositionItemsCtrl,
            ExportFactory<SimpleMessageToastNDPVM> simpleMessageToastNDPVMFactory)
        {
            this.contentVM = contentVM;
            this.xqTargetPositionDetailDialogCtrlFactory = xqTargetPositionDetailDialogCtrlFactory;
            this.popupFactory = popupFactory;
            this.selectStopLostOrProfitVMFactory = selectStopLostOrProfitVMFactory;
            this.xqTargetOfItemSubscribeQuotationHelpCtrl = xqTargetOfItemSubscribeQuotationHelpCtrl;
            this.xqTargetPositionItemsCtrl = xqTargetPositionItemsCtrl;
            this.simpleMessageToastNDPVMFactory = simpleMessageToastNDPVMFactory;

            clickItemTargetKeyRelatedColumnCmd = new DelegateCommand(ClickItemTargetKeyRelatedColumn);
            toStopLostOrProfitCmd = new DelegateCommand(ToStopLostOrProfit);
            toShowPositionDetailCmd = new DelegateCommand(ToShowPositionDetail);

            listItemsSelectionChangedCmd = new DelegateCommand(ListItemsSelectionChanged);
            selectedItemsDeleteExpiredCmd = new DelegateCommand(SelectedItemsDeleteExpired, CanSelectedItemsDeleteExpired);
            selectedItemsSubscribeQuotationCmd = new DelegateCommand(SelectedItemsSubscribeQuotation, CanSelectedItemsSubscribeQuotation);
        }

        public XueQiaoFoundation.BusinessResources.DataModels.TabWorkspace ParentWorkspace { get; set; }
        public ITradeComponentController ParentComponentCtrl { get; set; }

        public object ContentView => contentVM.View;

        public void Initialize()
        {
            if (ParentWorkspace == null) throw new ArgumentNullException("ParentWorkspace");
            if (ParentComponentCtrl == null) throw new ArgumentNullException("ParentComponentCtrl");

            contentVM.ClickItemTargetKeyRelatedColumnCmd = clickItemTargetKeyRelatedColumnCmd;
            contentVM.ToStopLostOrProfitCmd = toStopLostOrProfitCmd;
            contentVM.ToShowPositionDetailCmd = toShowPositionDetailCmd;

            xqTargetOfItemSubscribeQuotationHelpCtrl.XqTargetOfItemParser = obj =>
            {
                var item = obj as TargetPositionDataModel;
                if (item == null) return null;
                return new XqTargetInfo { TargetType = item.TargetType, TargetKey = item.TargetKey };
            };
            xqTargetOfItemSubscribeQuotationHelpCtrl.MsgDisplayWindowOwnerGetter = () => UIHelper.GetWindowOfUIElement(contentVM.View);
            xqTargetOfItemSubscribeQuotationHelpCtrl.Initialize();

            contentVM.ListItemsSelectionChangedCmd = listItemsSelectionChangedCmd;
            contentVM.SelectedItemsDeleteExpiredCmd = selectedItemsDeleteExpiredCmd;
            contentVM.SelectedItemsSubscribeQuotationCmd = selectedItemsSubscribeQuotationCmd;

            contentVM.UpdatePresentSubAccountId(ParentWorkspace.SubAccountId);
            PropertyChangedEventManager.AddHandler(ParentWorkspace, ParentWorkspacePropertyChanged, "");
        }

        public void Run()
        {

        }

        public void Shutdown()
        {
            DisposeViewToastNotifierWrapper();
            xqTargetOfItemSubscribeQuotationHelpCtrl.Shutdown();
            PropertyChangedEventManager.RemoveHandler(ParentWorkspace, ParentWorkspacePropertyChanged, "");
        }
        
        private NotifierWrapper_ControlPositionProvider AcquireViewToastNotifierWrapper()
        {
            if (viewToastNotifierWrapper == null)
            {
                viewToastNotifierWrapper = new NotifierWrapper_ControlPositionProvider(contentVM.View as FrameworkElement, Corner.BottomCenter, 0, 20,
                    _confWrapper =>
                    {
                        _confWrapper.LifetimeSupervisor = new Tuple<INotificationsLifetimeSupervisor>(new TimeAndFIFONotificationLifetimeSupervisor(TimeSpan.FromSeconds(3), 1));
                        _confWrapper.DisplayOptions = new Tuple<DisplayOptions>(new DisplayOptions { TopMost = false, Width = 280 });
                    });
            }
            return viewToastNotifierWrapper;
        }

        private void DisposeViewToastNotifierWrapper()
        {
            if (viewToastNotifierWrapper != null)
            {
                viewToastNotifierWrapper.Dispose();
                viewToastNotifierWrapper = null;
            }
        }

        private void ParentWorkspacePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(XueQiaoFoundation.BusinessResources.DataModels.TabWorkspace.SubAccountId))
            {
                contentVM.UpdatePresentSubAccountId(ParentWorkspace.SubAccountId);
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

            var newSelItems = (obj as IList)?.Cast<TargetPositionDataModel>().ToArray();
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
            if (e.PropertyName == nameof(TargetPositionDataModel.IsXqTargetExpired))
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
            return listSelectedPositionItems?.Any(i => i.IsXqTargetExpired == true && i.TargetType == ClientXQOrderTargetType.CONTRACT_TARGET)
                ?? false;
        }

        private void SelectedItemsDeleteExpired()
        {
            var targetExpiredItems = listSelectedPositionItems?
                .Where(i => i.IsXqTargetExpired == true && i.TargetType == ClientXQOrderTargetType.CONTRACT_TARGET)
                .ToArray();
            if (targetExpiredItems?.Any() != true) return;
            var canDeleteExpiredItems = targetExpiredItems.Where(i => i.LongPosition == 0 || i.ShortPosition == 0).ToArray();
            if (canDeleteExpiredItems.Count() < targetExpiredItems.Count())
            {
                var noteNDPVM = simpleMessageToastNDPVMFactory.CreateExport().Value;
                noteNDPVM.MessageContent = "未配平的过期持仓不会被删除";
                var notification = new XqToastNotification(noteNDPVM, false);
                AcquireViewToastNotifierWrapper().Notify<XqToastNotification>(() => notification);
            }

            foreach (var i in targetExpiredItems)
            {
                xqTargetPositionItemsCtrl.RequestDeleteExpiredXqTargetPosition(new TargetPositionKey(i.TargetType, i.SubAccountFields.SubAccountId, i.TargetKey));
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
            var item = obj as TargetPositionDataModel;
            if (item == null) return;

            var dialogCtrl = xqTargetPositionDetailDialogCtrlFactory.CreateExport().Value;
            dialogCtrl.DialogOwner = UIHelper.GetWindowOfUIElement(contentVM.View);
            dialogCtrl.XqTargetPositionItem = item;
            dialogCtrl.Initialize();
            dialogCtrl.Run();
            dialogCtrl.Shutdown();
        }
        
        private void ClickItemTargetKeyRelatedColumn(object obj)
        {
            var item = obj as TargetPositionDataModel;
            if (item == null) return;
            HandleXqTargetAssociate(item, null);
        }

        private void ToStopLostOrProfit(object param)
        {
            var args = param as object[];
            if (args == null || args.Length != 2) return;

            var item = args[0] as TargetPositionDataModel;
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

        private void HandleXqTargetAssociate(TargetPositionDataModel item, ClientPlaceOrderType? placeOrderType)
        {
            if (item == null) return;

            var associateCustomInfos = new Dictionary<string, object>();
            if (placeOrderType != null)
                associateCustomInfos.Add(TradeComponentAssociateConstants.ComponentAssociateArg_PlaceOrderType, placeOrderType.Value);

            // 联动
            var associateArgs = new TradeComponentXqTargetAssociateArgs(ParentComponentCtrl.ParentWorkspace, ParentComponentCtrl.Component,
                item.TargetType, item.TargetKey, associateCustomInfos);

            var previewAssociateDialogOwner = UIHelper.GetWindowOfUIElement(contentVM.View);
            Point? previewAssociateDialogLocation = null;
            ParentComponentCtrl.XqTargetAssociateHandler?.HandleXqTargetAssociate(previewAssociateDialogOwner, previewAssociateDialogLocation, associateArgs);
        }
    }
}
