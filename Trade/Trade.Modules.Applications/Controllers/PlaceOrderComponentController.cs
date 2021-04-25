using NativeModel.Trade;
using Prism.Events;
using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Waf.Applications;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.ServiceControllers;
using XueQiaoWaf.Trade.Modules.Applications.ViewModels;
using XueQiaoFoundation.Interfaces.Applications;
using XueQiaoWaf.Trade.Interfaces.Applications;
using XueQiaoFoundation.Interfaces.Event.business;
using System.Collections.Generic;
using NativeModel.Contract;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Helper.WeakDelegate;
using XueQiaoFoundation.BusinessResources.Constants;
using XueQiaoFoundation.BusinessResources.DataModels;
using ContainerShell.Interfaces.Applications;
using xueqiao.trade.hosting.events;
using XueQiaoWaf.Trade.Modules.Applications.Services;
using System.Collections.Specialized;
using XueQiaoWaf.Trade.Modules.Applications.Helper;
using XueQiaoFoundation.Interfaces.Helper;
using XueQiaoFoundation.BusinessResources.Helpers;
using XueQiaoWaf.Trade.Modules.Applications.ServiceControllers.Events;
using XueQiaoFoundation.Shared.Model;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class PlaceOrderComponentController : ITradeComponentController
    {        
        private readonly PlaceOrderComponentHeaderVM componentHeaderViewModel;
        private readonly PlaceOrderComponentContentViewModel componentContentViewModel;
        private readonly ComponentHeaderLayoutCtrl headerLayoutCtrl;
        private readonly PlaceOrderCreateViewController orderCreateViewController;
        private readonly PlaceOrderHangingOrdersAreaController hangingOrdersAreaController;
        private readonly TargetChartViewModel chartViewModel;
        private readonly ExportFactory<ComposeSearchPopupController> composeSearchPopupCtrlFactory;
        private readonly ILoginDataService loginDataService;
        private readonly ITradeModuleService tradeModuleService;
        private readonly IEventAggregator eventAggregator;
        private readonly ISubscribeComposeController subscribeComposeController;
        private readonly ISubscribeContractController subscribeContractController;
        private readonly IContractItemTreeQueryController contractItemTreeQueryController;
        private readonly IOrderItemsController orderItemsController;
        private readonly ITradeItemsController tradeItemsController;
        private readonly ISpecQuotationCacheController specQuotationCacheController;
        private readonly IXqTargetPositionItemsController targetPositionItemsCtrl;
        private readonly IMessageWindowService messageWindowService;
        private readonly IContainerShellService containerShellService;
        private readonly SubscribeContractService subscribeContractService;
        private readonly SubscribeComposeService subscribeComposeService;
        private readonly IUserComposeViewQueryController userComposeViewQueryCtrl;
        private readonly IContractItemTreeQueryController contractItemTreeQueryCtrl;
        private readonly IComposeGraphCacheController composeGraphCacheCtrl;
        private readonly IComposeGraphQueryController composeGraphQueryCtrl;
        private readonly IUserComposeViewCacheController userComposeViewCacheCtrl;
        private readonly ExportFactory<XqTargetConditionOrdersDialogCtrl> targetConditionOrdersDialogCtrlFactory;
        private readonly ExportFactory<XqTargetParkedOrdersDialogCtrl> targetParkedOrdersDialogCtrlFactory;

        private bool HasShutdowned;
        
        private SubscribeContractDataModel currentSubscribeContractItem;
        private SubscribeComposeDataModel currentSubscribeComposeItem;
        private PlaceOrderComponentPresentKey componentPresentKey;

        private readonly DelegateCommand showOrHideChartPartCommand;
        private readonly DelegateCommand showOrHidePlaceOrderPartCommand;
        private readonly DelegateCommand triggerSelectContractCmd;
        private readonly DelegateCommand triggerSelectComposeCmd;
        private readonly DelegateCommand showTargetConditionOrdersCmd;
        private readonly DelegateCommand showTargetParkedOrdersCmd;

        [ImportingConstructor]
        public PlaceOrderComponentController(PlaceOrderComponentHeaderVM componentHeaderViewModel,
            PlaceOrderComponentContentViewModel componentContentViewModel,
            ComponentHeaderLayoutCtrl headerLayoutCtrl,
            PlaceOrderCreateViewController orderCreateViewController,
            PlaceOrderHangingOrdersAreaController hangingOrdersAreaController,
            TargetChartViewModel chartViewModel,
            ExportFactory<ComposeSearchPopupController> composeSearchPopupCtrlFactory,
            ILoginDataService loginDataService,
            ITradeModuleService tradeModuleService,
            IEventAggregator eventAggregator,
            ISubscribeComposeController subscribeComposeController,
            ISubscribeContractController subscribeContractController,
            IContractItemTreeQueryController contractItemTreeQueryController,
            IOrderItemsController orderItemsController,
            ITradeItemsController tradeItemsController,
            ISpecQuotationCacheController specQuotationCacheController,
            IXqTargetPositionItemsController targetPositionItemsCtrl,
            IMessageWindowService messageWindowService,
            IContainerShellService containerShellService,
            SubscribeContractService subscribeContractService,
            SubscribeComposeService subscribeComposeService,
            IUserComposeViewQueryController userComposeViewQueryCtrl,
            IContractItemTreeQueryController contractItemTreeQueryCtrl,
            IComposeGraphCacheController composeGraphCacheCtrl,
            IComposeGraphQueryController composeGraphQueryCtrl,
            IUserComposeViewCacheController userComposeViewCacheCtrl,
            ExportFactory<XqTargetConditionOrdersDialogCtrl> targetConditionOrdersDialogCtrlFactory,
            ExportFactory<XqTargetParkedOrdersDialogCtrl> targetParkedOrdersDialogCtrlFactory)
        {
            this.componentHeaderViewModel = componentHeaderViewModel;
            this.componentContentViewModel = componentContentViewModel;
            this.headerLayoutCtrl = headerLayoutCtrl;
            this.orderCreateViewController = orderCreateViewController;
            this.hangingOrdersAreaController = hangingOrdersAreaController;
            this.chartViewModel = chartViewModel;
            this.composeSearchPopupCtrlFactory = composeSearchPopupCtrlFactory;
            this.loginDataService = loginDataService;
            this.tradeModuleService = tradeModuleService;
            this.eventAggregator = eventAggregator;
            this.subscribeComposeController = subscribeComposeController;
            this.subscribeContractController = subscribeContractController;
            this.contractItemTreeQueryController = contractItemTreeQueryController;
            this.orderItemsController = orderItemsController;
            this.tradeItemsController = tradeItemsController;
            this.specQuotationCacheController = specQuotationCacheController;
            this.targetPositionItemsCtrl = targetPositionItemsCtrl;
            this.messageWindowService = messageWindowService;
            this.containerShellService = containerShellService;
            this.subscribeContractService = subscribeContractService;
            this.subscribeComposeService = subscribeComposeService;
            this.userComposeViewQueryCtrl = userComposeViewQueryCtrl;
            this.contractItemTreeQueryCtrl = contractItemTreeQueryCtrl;
            this.composeGraphCacheCtrl = composeGraphCacheCtrl;
            this.composeGraphQueryCtrl = composeGraphQueryCtrl;
            this.userComposeViewCacheCtrl = userComposeViewCacheCtrl;
            this.targetConditionOrdersDialogCtrlFactory = targetConditionOrdersDialogCtrlFactory;
            this.targetParkedOrdersDialogCtrlFactory = targetParkedOrdersDialogCtrlFactory;

            showOrHideChartPartCommand = new DelegateCommand(ToggleShowOrHideChartPart);
            showOrHidePlaceOrderPartCommand = new DelegateCommand(ToggleShowOrHidePlaceOrderPart);
            triggerSelectContractCmd = new DelegateCommand(TriggerSelectContract);
            triggerSelectComposeCmd = new DelegateCommand(TriggerSelectCompose);
            showTargetConditionOrdersCmd = new DelegateCommand(ShowTargetConditionOrders);
            showTargetParkedOrdersCmd = new DelegateCommand(ShowTargetParkedOrders);
        }

        #region ITradeComponentController

        public XueQiaoFoundation.BusinessResources.DataModels.TradeComponent Component { get; set; }

        public XueQiaoFoundation.BusinessResources.DataModels.TabWorkspace ParentWorkspace { get; set; }

        public Action<ITradeComponentController> CloseComponentHandler { get; set; }

        public ITradeComponentXqTargetAssociateHandler XqTargetAssociateHandler { get; set; }

        /// <summary>
        /// 组件 data model，在 Initialize 后可获得
        /// </summary>
        public DraggableComponentUIDM ComponentItemDataModel { get; private set; }

        public bool OnAssociateXqTarget(TradeComponentXqTargetAssociateArgs associateArgs)
        {
            if (associateArgs == null) return false;
            if (HasShutdowned) return false;
            if (Component.IsLocked) return false;

            bool? arg_showChartLayout = null;
            bool? arg_showPlaceOrderLayout = null;
            object custom_arg1 = null;
            object custom_arg2 = null;
            if (associateArgs.CustomInfos?.TryGetValue(TradeComponentAssociateConstants.ComponentAssociateArg_IsShowChartLayout, out custom_arg1) == true)
                arg_showChartLayout = custom_arg1 as bool?;
            if (associateArgs.CustomInfos?.TryGetValue(TradeComponentAssociateConstants.ComponentAssociateArg_IsShowPlaceOrderLayout, out custom_arg2) == true)
                arg_showPlaceOrderLayout = custom_arg2 as bool?;

            bool needChangeLayout = false;
            if (arg_showChartLayout != null || arg_showPlaceOrderLayout != null)
            {
                if (arg_showChartLayout != Component.PlaceOrderContainerComponentDetail.IsShowChartLayout
                    || arg_showPlaceOrderLayout != Component.PlaceOrderContainerComponentDetail.IsShowOrderLayout)
                {
                    needChangeLayout = true;
                }
            }

            var originChartLayoutColumnWidth = componentContentViewModel.ChartColumnViewWidth;
            var originOrderLayoutColumnWidth = componentContentViewModel.PlaceOrderColumnViewWidth;
            var originIsShowChartLayout = Component.PlaceOrderContainerComponentDetail.IsShowChartLayout;
            var originIsShowOrderLayout = Component.PlaceOrderContainerComponentDetail.IsShowOrderLayout;
            var originComponentWidth = Component.Width;

            if (needChangeLayout)
            {
                if (arg_showChartLayout != null)
                {
                    Component.PlaceOrderContainerComponentDetail.IsShowChartLayout = arg_showChartLayout.Value;
                }
                if (arg_showPlaceOrderLayout != null)
                {
                    Component.PlaceOrderContainerComponentDetail.IsShowOrderLayout = arg_showPlaceOrderLayout.Value;
                }

                var nowIsShowChartLayout = Component.PlaceOrderContainerComponentDetail.IsShowChartLayout;
                var nowIsShowOrderLayout = Component.PlaceOrderContainerComponentDetail.IsShowOrderLayout;

                // 改变组件显示宽度
                Component.Width = CalculateCurrentComponentWidth(nowIsShowChartLayout, nowIsShowOrderLayout,
                    originComponentWidth, originIsShowChartLayout, originChartLayoutColumnWidth,
                    originIsShowOrderLayout, originOrderLayoutColumnWidth);
            }

            // 设置 Component 相关数据
            if (associateArgs.TargetType == ClientXQOrderTargetType.CONTRACT_TARGET)
            {
                Component.PlaceOrderContainerComponentDetail.AttachContractId = associateArgs.TargetKey;
                Component.PlaceOrderContainerComponentDetail.AttachComposeId = null;
            }
            else if (associateArgs.TargetType == ClientXQOrderTargetType.COMPOSE_TARGET)
            {
                Component.PlaceOrderContainerComponentDetail.AttachContractId = null;
                Component.PlaceOrderContainerComponentDetail.AttachComposeId = associateArgs.TargetKey;
            }


            double? arg_price = null;
            ClientPlaceOrderType? arg_placeOrderType = null;
            object custom_arg3 = null;
            object custom_arg4 = null;
            if (associateArgs.CustomInfos?.TryGetValue(TradeComponentAssociateConstants.ComponentAssociateArg_Price, out custom_arg3) == true)
                arg_price = custom_arg3 as double?;
            if (associateArgs.CustomInfos?.TryGetValue(TradeComponentAssociateConstants.ComponentAssociateArg_PlaceOrderType, out custom_arg4) == true)
                arg_placeOrderType = custom_arg4 as ClientPlaceOrderType?;

            var originPresentKey = this.componentPresentKey;

            ApplyCurrentComponent();

            // 如果改变了标的，则初始化下单数量
            int? change2OrderQuantity = null;
            if (associateArgs.TargetType != originPresentKey?.TargetType || associateArgs.TargetKey != originPresentKey?.TargetKey)
            {
                change2OrderQuantity = 1;
            }
            orderCreateViewController.UpdateViewPresentWithDatas(change2OrderQuantity, null, null, null, null, null, arg_price, arg_placeOrderType);

            return true;
        }

        #endregion

        public void Initialize()
        {
            if (Component == null) throw new ArgumentNullException("`Component` must be setted before initialize.");
            if (ParentWorkspace == null) throw new ArgumentNullException("`ParentWorkspace` must be setted before initialize.");
            if (Component.PlaceOrderContainerComponentDetail == null) throw new ArgumentNullException("Component.PlaceOrderContainerComponentDetail");

            // config header layout controller 
            headerLayoutCtrl.Component = this.Component;
            headerLayoutCtrl.ClickComponentClose = _ctrl => CloseComponentHandler?.Invoke(this);
            headerLayoutCtrl.ClickComponentSetting = null;
            headerLayoutCtrl.Initialize();
            headerLayoutCtrl.Run();

            componentHeaderViewModel.ComponentInfo = Component;
            componentHeaderViewModel.ShowOrHideChartPartCommand = showOrHideChartPartCommand;
            componentHeaderViewModel.ShowOrHidePlaceOrderPartCommand = showOrHidePlaceOrderPartCommand;
            componentHeaderViewModel.TriggerSelectContractCmd = triggerSelectContractCmd;
            componentHeaderViewModel.TriggerSelectComposeCmd = triggerSelectComposeCmd;

            headerLayoutCtrl.HeaderLayoutVM.ShowCloseItem = true;
            headerLayoutCtrl.HeaderLayoutVM.ShowSettingItem = false;
            headerLayoutCtrl.HeaderLayoutVM.ShowComponentLockItem = true;
            headerLayoutCtrl.HeaderLayoutVM.CustomPartView = componentHeaderViewModel.View;

            orderCreateViewController.Initialize();
            orderCreateViewController.Run();
            componentContentViewModel.PlaceOrderCreateMainView = orderCreateViewController.PlaceOrderCreateMainView;
            componentContentViewModel.PlaceOrderCreateConditionView = orderCreateViewController.PlaceOrderCreateConditionView;

            hangingOrdersAreaController.Initialize();
            hangingOrdersAreaController.Run();
            componentContentViewModel.HangingOrdersAreaView = hangingOrdersAreaController.HangingOrdersAreaView;

            componentContentViewModel.ChartView = chartViewModel.View;

            componentContentViewModel.ShowTargetConditionOrdersCmd = showTargetConditionOrdersCmd;
            componentContentViewModel.ShowTargetParkedOrdersCmd = showTargetParkedOrdersCmd;

            ComponentItemDataModel = new DraggableComponentUIDM(Component,
                headerLayoutCtrl.HeaderLayoutVM.View,
                componentContentViewModel.View);
            
            eventAggregator.GetEvent<SpecQuotationUpdated>()
                .Subscribe(RecvSpecQuotationUpdatedEvent, ThreadOption.UIThread);

            eventAggregator.GetEvent<CombQuotationUpdated>()
                .Subscribe(RecvCombQuotationUpdatedEvent, ThreadOption.UIThread);

            eventAggregator.GetEvent<XQTargetPositionSummaryChanged>()
                .Subscribe(RecvXQTargetPositionSummaryChanged, ThreadOption.UIThread);
            
            eventAggregator.GetEvent<XQTargetPositionDynamicInfoEvent>()
                .Subscribe(RecvXQTargetPositionDynamicInfoEvent, ThreadOption.UIThread);

            eventAggregator.GetEvent<UserComposeViewUpdatedEvent>()
                .Subscribe(ReceivedUserComposeViewUpdatedEvent, ThreadOption.UIThread);
            
            PropertyChangedEventManager.AddHandler(componentContentViewModel, ComponentContentViewModelPropertyChanged, "");
            PropertyChangedEventManager.AddHandler(ParentWorkspace, ParentWorkspacePropertyChanged, "");
            CollectionChangedEventManager.AddHandler(subscribeContractService.Contracts, SubscribeContractCollectionChanged);
            CollectionChangedEventManager.AddHandler(subscribeComposeService.Composes, SubscribeComposeCollectionChanged);
        }
        
        public void Run()
        {
            ApplyCurrentComponent();
        }
        
        public void Shutdown()
        {
            orderCreateViewController?.Shutdown();
            
            eventAggregator.GetEvent<SpecQuotationUpdated>()
                .Unsubscribe(RecvSpecQuotationUpdatedEvent);

            eventAggregator.GetEvent<CombQuotationUpdated>()
               .Unsubscribe(RecvCombQuotationUpdatedEvent);
            
            eventAggregator.GetEvent<XQTargetPositionSummaryChanged>()
                .Unsubscribe(RecvXQTargetPositionSummaryChanged);

            eventAggregator.GetEvent<XQTargetPositionDynamicInfoEvent>()
                .Unsubscribe(RecvXQTargetPositionDynamicInfoEvent);
            
            eventAggregator.GetEvent<UserComposeViewUpdatedEvent>()
                .Unsubscribe(ReceivedUserComposeViewUpdatedEvent);

            PropertyChangedEventManager.RemoveHandler(componentContentViewModel, ComponentContentViewModelPropertyChanged, "");
            chartViewModel?.Shutdown();
            PropertyChangedEventManager.RemoveHandler(ParentWorkspace, ParentWorkspacePropertyChanged, "");
            CollectionChangedEventManager.RemoveHandler(subscribeContractService.Contracts, SubscribeContractCollectionChanged);
            CollectionChangedEventManager.RemoveHandler(subscribeComposeService.Composes, SubscribeComposeCollectionChanged);

            if (currentSubscribeContractItem != null)
            {
                PropertyChangedEventManager.RemoveHandler(currentSubscribeContractItem, CurrentSubscribeContractItemPropertyChanged, "");
                
                subscribeContractController.RemoveSubscribeConract(currentSubscribeContractItem.ContractId, currentSubscribeContractItem.SubscribeGroupKey);
                currentSubscribeContractItem = null;
            }

            if (currentSubscribeComposeItem != null)
            {
                PropertyChangedEventManager.RemoveHandler(currentSubscribeComposeItem, CurrentSubscribeComposeItemPropertyChanged, "");
               
                subscribeComposeController.RemoveSubscribeCompose(currentSubscribeComposeItem.ComposeId, currentSubscribeComposeItem.SubscribeGroupKey);
                currentSubscribeComposeItem = null;
            }
            
            CloseComponentHandler = null;
            headerLayoutCtrl?.Shutdown();
            HasShutdowned = true;
        }
        
        private void ComponentContentViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PlaceOrderComponentContentViewModel.ChartPartShowing)
                || e.PropertyName == nameof(PlaceOrderComponentContentViewModel.PlaceOrderPartShowing))
            {
                CaculateComponentMininumSize(out double miniWidth, out double miniHeight,
                            componentContentViewModel.ChartPartShowing, componentContentViewModel.PlaceOrderPartShowing);
                ComponentItemDataModel.ComponentMinWidth = miniWidth;
                ComponentItemDataModel.ComponentMinHeight = miniHeight;
            }
        }

        private void ParentWorkspacePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TabWorkspace.SubAccountId))
            {
                var currentPresentKey = this.componentPresentKey;
                if (currentPresentKey != null)
                {
                    var newPresentKey = new PlaceOrderComponentPresentKey(ParentWorkspace.SubAccountId, currentPresentKey.TargetType, currentPresentKey.TargetKey);
                    this.componentPresentKey = newPresentKey;
                    hangingOrdersAreaController.UpdateViewPresentKey(newPresentKey);
                    orderCreateViewController.UpdateViewPresentKey(newPresentKey);
                    componentContentViewModel.UpdateViewPresentKey(newPresentKey);
                }
            }
        }

        private void SubscribeContractCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var compSubscribeContractItem = this.currentSubscribeContractItem;
            if (compSubscribeContractItem != null && !subscribeContractService.Contracts.Contains(compSubscribeContractItem))
            {
                // 处理当外部删除当前订阅项时
                Component.PlaceOrderContainerComponentDetail.AttachContractId = null;
                Component.PlaceOrderContainerComponentDetail.AttachComposeId = null;
                ApplyCurrentComponent();
            }
        }

        private void SubscribeComposeCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var compSubscribeComposeItem = this.currentSubscribeComposeItem;
            if (compSubscribeComposeItem != null && !subscribeComposeService.Composes.Contains(compSubscribeComposeItem))
            {
                // 处理当外部删除当前订阅项时
                Component.PlaceOrderContainerComponentDetail.AttachContractId = null;
                Component.PlaceOrderContainerComponentDetail.AttachComposeId = null;
                ApplyCurrentComponent();
            }
        }
        
        private void RecvSpecQuotationUpdatedEvent(NativeQuotationItem quot)
        {
            if (HasShutdowned) return;
            if (quot == null) return;
            var subscribeItem = currentSubscribeContractItem;
            if (subscribeItem == null) return;

            var _contractCode = subscribeItem.ContractDetailContainer.ContractDetail?.SledContractCode;
            var _commodityType = subscribeItem.ContractDetailContainer.CommodityDetail?.SledCommodityType;
            var _commodityCode = subscribeItem.ContractDetailContainer.CommodityDetail?.SledCommodityCode;
            var _exchangeMic = subscribeItem.ContractDetailContainer.CommodityDetail?.ExchangeMic;

            // 过滤掉其他合约的行情
            if (quot.ContractSymbol?.ContractCode == _contractCode
                && quot.ContractSymbol?.CommodityType == _commodityType
                && quot.ContractSymbol?.CommodityCode == _commodityCode
                && quot.ContractSymbol?.ExchangeMic == _exchangeMic)
            {
                chartViewModel.AddChartQuotations(false, new NativeQuotationItem[] { quot });
            }
        }

        private void RecvCombQuotationUpdatedEvent(NativeCombQuotationItem msg)
        {
            var quot = msg?.CombQuotation;
            if (HasShutdowned) return;
            if (quot == null) return;
            var subscribeItem = currentSubscribeComposeItem;
            if (subscribeItem == null) return;
            if (msg.ComposeGraphId != subscribeItem.ComposeId) return;
            
            chartViewModel.AddChartQuotations(false, new NativeQuotationItem[] { quot });
        }
        
        private void RecvXQTargetPositionSummaryChanged(StatPositionSummaryChangedEvent msg)
        {
            if (msg == null) return;
            var positionSummary = msg?.StatPositionSummary;
            var currentPresentKey = this.componentPresentKey;
            if (positionSummary == null || currentPresentKey == null) return;
            if (msg.SubAccountId == currentPresentKey.SubAccountId 
                && positionSummary.TargetKey == currentPresentKey.TargetKey
                && positionSummary.TargetType.ToClientXQOrderTargetType() == currentPresentKey.TargetType)
            {
                if (msg.EventType == StatPositionEventType.STAT_ITEM_REMOVED)
                {
                    // 删除标的持仓
                    componentContentViewModel.TargetPositionVolume = null;
                    componentContentViewModel.TargetPositionAvgPrice = null;
                    return;
                }
                else
                {
                    componentContentViewModel.TargetPositionVolume = 
                        positionSummary.__isset.netPosition ? (long?)positionSummary.NetPosition : null;
                    componentContentViewModel.TargetPositionAvgPrice =
                        positionSummary.__isset.positionAvgPrice ? (double?)positionSummary.PositionAvgPrice : null;
                }
            }
        }

        private void RecvXQTargetPositionDynamicInfoEvent(StatPositionDynamicInfoEvent payload)
        {
            if (payload == null) return;
            var currentPresentKey = this.componentPresentKey;
            var data = payload.PositionDynamicInfo;
            if (data == null || currentPresentKey == null) return;
            if (payload.SubAccountId == currentPresentKey.SubAccountId
                && data.TargetKey == currentPresentKey.TargetKey
                && data.TargetType.ToClientXQOrderTargetType() == currentPresentKey.TargetType)
            {
                componentContentViewModel.TargetProfitLoss = 
                    data.__isset.positionProfit ? (double?)data.PositionProfit : null;
            }
        }

        private void ReceivedUserComposeViewUpdatedEvent(NativeComposeView composeView)
        {
            if (composeView == null) return;
            if (CheckIsSameTargetWithCurrent(ClientXQOrderTargetType.COMPOSE_TARGET, $"{composeView.ComposeGraphId}"))
            {
                // 更新 price tick
                var priceTick = XueQiaoBusinessHelper.CalculteXqTargetPriceTick(composeView?.PrecisionNumber, XueQiaoConstants.XQContractPriceMinimumPirceTick);
                orderCreateViewController.UpdateViewPresentWithDatas(null, null, null, null, null, priceTick, null, null);

                // 更新 component title
                if (Component != null)
                {
                    Component.ComponentDescTitle = composeView?.AliasName;
                }
            }
        }

        private void CurrentSubscribeContractItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender != currentSubscribeContractItem) return;
            ApplyCurrentSubscribeContractItemPropertyChanged(currentSubscribeContractItem, e.PropertyName);
        }
        
        private void CurrentSubscribeComposeItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender != currentSubscribeComposeItem) return;
            ApplyCurrentSubscribeComposeItemPropertyChanged(currentSubscribeComposeItem, e.PropertyName);
        }
        
        private void ToggleShowOrHideChartPart()
        {
            var originChartLayoutColumnWidth = componentContentViewModel.ChartColumnViewWidth;
            var originOrderLayoutColumnWidth = componentContentViewModel.PlaceOrderColumnViewWidth;
            var originIsShowChartLayout = Component.PlaceOrderContainerComponentDetail.IsShowChartLayout;
            var originIsShowOrderLayout = Component.PlaceOrderContainerComponentDetail.IsShowOrderLayout;
            var originComponentWidth = Component.Width;
            
            var isShowingThisPart = originIsShowChartLayout;
            var destShowing = !isShowingThisPart;
            if (destShowing)
            {
                if (isShowingThisPart) return;
                componentContentViewModel.ChartPartShowing = true;
                Component.PlaceOrderContainerComponentDetail.IsShowChartLayout = true;
            }
            else
            {
                if (IsShowingSinglePartOnly()) return;
                componentContentViewModel.ChartPartShowing = false;
                Component.PlaceOrderContainerComponentDetail.IsShowChartLayout = false;
            }

            // 改变组件宽度
            var currentShowChartPart = Component.PlaceOrderContainerComponentDetail.IsShowChartLayout;
            var currentShowPlaceOrderPart = Component.PlaceOrderContainerComponentDetail.IsShowOrderLayout;

            Component.Width = CalculateCurrentComponentWidth(currentShowChartPart, currentShowPlaceOrderPart,
                originComponentWidth, originIsShowChartLayout, originChartLayoutColumnWidth,
                originIsShowOrderLayout, originOrderLayoutColumnWidth);

            showOrHideChartPartCommand?.RaiseCanExecuteChanged();
            showOrHidePlaceOrderPartCommand?.RaiseCanExecuteChanged();
        }

        private void ToggleShowOrHidePlaceOrderPart()
        {
            var originChartLayoutColumnWidth = componentContentViewModel.ChartColumnViewWidth;
            var originOrderLayoutColumnWidth = componentContentViewModel.PlaceOrderColumnViewWidth;
            var originIsShowChartLayout = Component.PlaceOrderContainerComponentDetail.IsShowChartLayout;
            var originIsShowOrderLayout = Component.PlaceOrderContainerComponentDetail.IsShowOrderLayout;
            var originComponentWidth = Component.Width;

            var isShowingThisPart = originIsShowOrderLayout;
            var destShowing = !isShowingThisPart;
            if (destShowing)
            {
                if (isShowingThisPart) return;
                componentContentViewModel.PlaceOrderPartShowing = true;
                Component.PlaceOrderContainerComponentDetail.IsShowOrderLayout = true;
            }
            else
            {
                if (IsShowingSinglePartOnly()) return;
                componentContentViewModel.PlaceOrderPartShowing = false;
                Component.PlaceOrderContainerComponentDetail.IsShowOrderLayout = false;
            }

            // 改变组件宽度
            var currentShowChartPart = Component.PlaceOrderContainerComponentDetail.IsShowChartLayout;
            var currentShowPlaceOrderPart = Component.PlaceOrderContainerComponentDetail.IsShowOrderLayout;

            Component.Width = CalculateCurrentComponentWidth(currentShowChartPart, currentShowPlaceOrderPart,
                originComponentWidth, originIsShowChartLayout, originChartLayoutColumnWidth,
                originIsShowOrderLayout, originOrderLayoutColumnWidth);

            showOrHideChartPartCommand?.RaiseCanExecuteChanged();
            showOrHidePlaceOrderPartCommand?.RaiseCanExecuteChanged();
        }

        private void TriggerSelectContract(object triggerElement)
        {
            containerShellService.ShowContractQuickSearchPopup(triggerElement, null,
                _selContractId => 
                {
                    if (_selContractId == null) return;

                    // 添加订阅合约
                    subscribeContractController.AddOrUpdateSubscribeContract(
                        _selContractId.Value,
                        SubscribeContractDataModel.UniqueSubscribeContractGroupKey(), null);
                    // 立即订阅
                    subscribeContractController.SubscribeContractQuotationIfNeed(_selContractId.Value, null);

                    // 设置 Component 相关数据
                    Component.PlaceOrderContainerComponentDetail.AttachComposeId = null;
                    Component.PlaceOrderContainerComponentDetail.AttachContractId = $"{_selContractId.Value}";
                    
                    ApplyCurrentComponent();
                });
        }

        private void TriggerSelectCompose(object triggerElement)
        {
            var popCtrl = composeSearchPopupCtrlFactory.CreateExport().Value;
            popCtrl.PopupPalcementTarget = triggerElement;
            popCtrl.PopupCloseHandler = (_ctrl, _selectComposeId) =>
            {
                _ctrl.Shutdown();

                if (_selectComposeId == null) return;

                // 添加订阅合约
                subscribeComposeController.AddOrUpdateSubscribeCompose(
                    _selectComposeId.Value,
                    SubscribeComposeDataModel.UniqueSubscribeComposeGroupKey(), null);
                // 立即订阅
                subscribeComposeController.SubscribeCombQuotationAsync(_selectComposeId.Value);

                // 设置 Component 相关数据
                Component.PlaceOrderContainerComponentDetail.AttachComposeId = $"{_selectComposeId.Value}";
                Component.PlaceOrderContainerComponentDetail.AttachContractId = null;

                ApplyCurrentComponent();
            };

            popCtrl.Initialize();
            popCtrl.Run();
        }

        private void ShowTargetConditionOrders()
        {
            var presentKey = this.componentPresentKey;
            if (presentKey == null || presentKey.SubAccountId == null || presentKey.TargetType == null)
                return;

            var ctrl = targetConditionOrdersDialogCtrlFactory.CreateExport().Value;
            ctrl.DialogOwner = UIHelper.GetWindowOfUIElement(componentHeaderViewModel.View);
            ctrl.SubAccountId = presentKey.SubAccountId.Value;
            ctrl.TargetType = presentKey.TargetType.Value;
            ctrl.TargetKey = presentKey.TargetKey;
            ctrl.Initialize();
            ctrl.Run();
            ctrl.Shutdown();
        }

        private void ShowTargetParkedOrders()
        {
            var presentKey = this.componentPresentKey;
            if (presentKey == null || presentKey.SubAccountId == null || presentKey.TargetType == null)
                return;

            var ctrl = targetParkedOrdersDialogCtrlFactory.CreateExport().Value;
            ctrl.DialogOwner = UIHelper.GetWindowOfUIElement(componentHeaderViewModel.View);
            ctrl.SubAccountId = presentKey.SubAccountId.Value;
            ctrl.TargetType = presentKey.TargetType.Value;
            ctrl.TargetKey = presentKey.TargetKey;
            ctrl.Initialize();
            ctrl.Run();
            ctrl.Shutdown();
        }

        private bool IsShowingSinglePartOnly()
        {
            var containerComponentDetail = Component.PlaceOrderContainerComponentDetail;
            var compareItems = new bool[] { containerComponentDetail.IsShowChartLayout, containerComponentDetail.IsShowOrderLayout };
            return (compareItems.Count(i => i == true) <= 1);
        }

        private double CalculateCurrentComponentWidth(bool currentShowChartColumn, bool currentShowPlaceOrderColumn,
            double componentLastWidth, bool lastShowChartColumn,
            double lastChartColumnWidth, bool lastShowPlaceOrderColumn,
            double lastPlaceOrderColumnWidth)
        {
            double compCurrentWidth = componentLastWidth;

            if (lastShowChartColumn && !currentShowChartColumn)
            {
                compCurrentWidth -= lastChartColumnWidth;
            }
            else if (!lastShowChartColumn && currentShowChartColumn)
            {
                var originCompWidth = compCurrentWidth;
                compCurrentWidth += TradeComponentConstans.PlaceOrderComponent_ChartColumnNormalWidth;
                compCurrentWidth = Math.Min(compCurrentWidth, Math.Max(originCompWidth, Extensions.PlaceOrderComponent_NormalWidth(true, true)));
            }

            if (lastShowPlaceOrderColumn && !currentShowPlaceOrderColumn)
            {
                compCurrentWidth -= lastPlaceOrderColumnWidth;
            }
            else if (!lastShowPlaceOrderColumn && currentShowPlaceOrderColumn)
            {
                var originCompWidth = compCurrentWidth;
                compCurrentWidth += TradeComponentConstans.PlaceOrderComponent_PlaceOrderColumnNormalWidth;
                compCurrentWidth = Math.Min(compCurrentWidth, Math.Max(originCompWidth, Extensions.PlaceOrderComponent_NormalWidth(true, true)));
            }

            return compCurrentWidth;
        }

        private void CaculateComponentMininumSize(out double _mininumWidth, out double _mininumHeight, bool showChartColumn, bool showPlaceOrderColumn)
        {
            _mininumHeight = TradeComponentConstans.PlaceOrderComponent_MininumHeight;
            _mininumWidth = Extensions.PlaceOrderComponent_MinimumWidth(showChartColumn, showPlaceOrderColumn);
        }

        private void RemoveCurrentSubscribeComposeItem()
        {
            var currentItem = currentSubscribeComposeItem;
            if (currentItem == null) return;

            this.currentSubscribeComposeItem = null;

            PropertyChangedEventManager.RemoveHandler(currentItem, CurrentSubscribeComposeItemPropertyChanged, "");
            
            CollectionChangedEventManager.RemoveHandler(subscribeComposeService.Composes, SubscribeComposeCollectionChanged);
            subscribeComposeController.RemoveSubscribeCompose(currentItem.ComposeId, currentItem.SubscribeGroupKey);
            CollectionChangedEventManager.AddHandler(subscribeComposeService.Composes, SubscribeComposeCollectionChanged);
        }

        private void RemoveCurrentSubscribeContractItem()
        {
            var currentItem = currentSubscribeContractItem;
            if (currentItem == null) return;

            this.currentSubscribeContractItem = null;

            PropertyChangedEventManager.RemoveHandler(currentItem, CurrentSubscribeContractItemPropertyChanged, "");
            
            CollectionChangedEventManager.RemoveHandler(subscribeContractService.Contracts, SubscribeContractCollectionChanged);
            subscribeContractController.RemoveSubscribeConract(currentItem.ContractId, currentItem.SubscribeGroupKey);
            CollectionChangedEventManager.AddHandler(subscribeContractService.Contracts, SubscribeContractCollectionChanged);
        }

        private void ApplyCurrentComponent()
        {
            var currentComponent = Component;
            if (currentComponent == null) return;

            componentContentViewModel.ChartPartShowing = Component.PlaceOrderContainerComponentDetail.IsShowChartLayout;
            componentContentViewModel.PlaceOrderPartShowing = Component.PlaceOrderContainerComponentDetail.IsShowOrderLayout;
            
            int? attachedContractId = null;
            long? attachedComposeId = null;
            try
            {
                attachedContractId = System.Convert.ToInt32(currentComponent.PlaceOrderContainerComponentDetail.AttachContractId);
                attachedComposeId = System.Convert.ToInt32(currentComponent.PlaceOrderContainerComponentDetail.AttachComposeId);
            }
            catch (Exception) { }

            if (attachedContractId != null && attachedContractId > 0)
            {
                RemoveCurrentSubscribeComposeItem();
                if (attachedContractId != currentSubscribeContractItem?.ContractId)
                {
                    RemoveCurrentSubscribeContractItem();

                    // subscribe this contract
                    currentSubscribeContractItem = subscribeContractController.AddOrUpdateSubscribeContract(attachedContractId.Value,
                        SubscribeContractDataModel.UniqueSubscribeContractGroupKey(), null);
                    subscribeContractController.SubscribeContractQuotationIfNeed(attachedContractId, null);

                    PropertyChangedEventManager.AddHandler(currentSubscribeContractItem, CurrentSubscribeContractItemPropertyChanged, "");
                   
                    // 设置 view model 的property
                    var newPresentKey = new PlaceOrderComponentPresentKey(ParentWorkspace.SubAccountId, ClientXQOrderTargetType.CONTRACT_TARGET, $"{attachedContractId}");
                    this.componentPresentKey = newPresentKey;
                    hangingOrdersAreaController.UpdateViewPresentKey(newPresentKey);
                    orderCreateViewController.UpdateViewPresentKey(newPresentKey);
                    componentHeaderViewModel.ExistAttachTarget = true;
                    componentContentViewModel.UpdateViewPresentKey(newPresentKey);

                    ClearViewMarketProperties();
                    ApplyCurrentTargetPositionInfos();
                    ApplyCurrentSubscribeContractItem();
                    RefreshCurrentContractTargetDetail();
                }
                return;
            }
            else if (attachedComposeId != null && attachedComposeId > 0)
            {
                RemoveCurrentSubscribeContractItem();
                if (attachedComposeId != currentSubscribeComposeItem?.ComposeId)
                {
                    RemoveCurrentSubscribeComposeItem();

                    // subscribe this compose
                    currentSubscribeComposeItem = subscribeComposeController.AddOrUpdateSubscribeCompose(attachedComposeId.Value,
                        SubscribeComposeDataModel.UniqueSubscribeComposeGroupKey(), null);

                    PropertyChangedEventManager.AddHandler(currentSubscribeComposeItem, CurrentSubscribeComposeItemPropertyChanged, "");
                   
                    // 设置 view model 的property
                    var newPresentKey = new PlaceOrderComponentPresentKey(ParentWorkspace.SubAccountId, ClientXQOrderTargetType.COMPOSE_TARGET, $"{attachedComposeId}");
                    this.componentPresentKey = newPresentKey;
                    hangingOrdersAreaController.UpdateViewPresentKey(newPresentKey);
                    orderCreateViewController.UpdateViewPresentKey(newPresentKey);
                    componentHeaderViewModel.ExistAttachTarget = true;
                    componentContentViewModel.UpdateViewPresentKey(newPresentKey);

                    ClearViewMarketProperties();
                    ApplyCurrentTargetPositionInfos();
                    ApplyCurrentSubscribeComposeItem();
                    RefreshCurrentComposeTargetDetail();
                }
                return;
            }
            else
            {
                RemoveCurrentSubscribeContractItem();
                RemoveCurrentSubscribeComposeItem();

                // 设置 view model 的property
                var newPresentKey = new PlaceOrderComponentPresentKey(ParentWorkspace.SubAccountId, null, "0");
                this.componentPresentKey = newPresentKey;
                hangingOrdersAreaController.UpdateViewPresentKey(newPresentKey);
                orderCreateViewController.UpdateViewPresentKey(newPresentKey);

                componentHeaderViewModel.ExistAttachTarget = false;
                componentContentViewModel.UpdateViewPresentKey(newPresentKey);
                ClearViewMarketProperties();
                ApplyCurrentTargetPositionInfos();
                Component.ComponentDescTitle = TradeWorkspaceDataDisplayHelper.GetTradeComponentTypeDisplayName(Component.ComponentType);
            }
        }

        private void ClearViewMarketProperties()
        {
            componentContentViewModel.LastPrice = null;
            componentContentViewModel.HighPrice = null;
            componentContentViewModel.OpenPrice = null;
            componentContentViewModel.Volume = null;
            componentContentViewModel.LowPrice = null;
            componentContentViewModel.PreClosePrice = null;
            componentContentViewModel.Turnover = null;
            componentContentViewModel.LowerLimitPrice = null;
            componentContentViewModel.BidPrice1 = null;
            componentContentViewModel.BidQty1 = null;
            componentContentViewModel.AskPrice1 = null;
            componentContentViewModel.AskQty1 = null;

            orderCreateViewController.ClearViewMarketDatas();
        }

        private void ApplyCurrentTargetPositionInfos()
        {
            // clear position infos first
            componentContentViewModel.TargetPositionVolume = null;
            componentContentViewModel.TargetPositionAvgPrice = null;
            componentContentViewModel.TargetProfitLoss = null;

            var currentPresentKey = this.componentPresentKey;
            if (currentPresentKey != null)
            {
                var targetType = currentPresentKey.TargetType;
                var subAccountId = currentPresentKey.SubAccountId;
                var targetKey = currentPresentKey.TargetKey;
                if (targetType.HasValue && subAccountId.HasValue)
                {
                    var tarPositionItem = targetPositionItemsCtrl.GetPositionItem(new TargetPositionKey(targetType.Value, subAccountId.Value, targetKey));
                    if (tarPositionItem != null)
                    {
                        // set new infos then
                        componentContentViewModel.TargetPositionVolume = tarPositionItem.NetPosition;
                        componentContentViewModel.TargetPositionAvgPrice = tarPositionItem.PositionAvgPrice;
                        componentContentViewModel.TargetProfitLoss = tarPositionItem.DynamicInfo?.PositionProfit;
                    }
                }
            }
        }

        private void ApplyCurrentSubscribeContractItem()
        {
            var subscribeItem = currentSubscribeContractItem;
            if (subscribeItem == null) return;

            Action<NativeContract, NativeCommodity> applyInitialQuotations = (_contractDetail, _commDetail) =>
            {
                if (_contractDetail == null || _commDetail == null) return;
                var symbol = IDLAutoGenerated.Util.Extensions
                                    .GenerateQuotationContractSymbol(_contractDetail?.SledContractCode ?? "",
                                        _commDetail?.SledCommodityType ?? 0,
                                        _commDetail?.SledCommodityCode ?? "",
                                        _commDetail?.ExchangeMic ?? "");
                var quotations = specQuotationCacheController.GetCachedQuotationsBySymbol(symbol);
                if (quotations == null || quotations.Count() == 0) return;
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    chartViewModel.AddChartQuotations(true, quotations);
                });
            };

            var subscribeContractId = subscribeItem.ContractId;
            var currentItemContract = subscribeItem.ContractDetailContainer.ContractDetail;
            var currentItemCommodity = subscribeItem.ContractDetailContainer.CommodityDetail;

            // Clear TickDataPoints first
            chartViewModel.ResetChart();
            if (currentItemContract != null && currentItemCommodity != null)
            {
                applyInitialQuotations(currentItemContract, currentItemCommodity);
            }
            else
            {
                var contractTreeQueryHandler = new Action<Dictionary<int, ContractItemTree>>(
                    resp =>
                    {
                        if (resp != null
                            && resp.TryGetValue(subscribeContractId, out ContractItemTree treeItem))
                        {
                            if (currentSubscribeContractItem == null) return;
                            if (currentSubscribeContractItem.ContractId != subscribeContractId) return;
                            applyInitialQuotations(treeItem.Contract, treeItem.ParentCommodity);
                        }
                    });

                // 正确设置 `keepReferenceAlive`,确保不内存泄漏
                var delegateReference = new ActionDelegateReference<Dictionary<int, ContractItemTree>>(contractTreeQueryHandler,
                    contractTreeQueryHandler.Target == this ? false : true);
                contractItemTreeQueryController.QueryTreeItems(new int[] { subscribeContractId }, true, false, false, delegateReference);
            }
            
            // Apply properties
            var props = subscribeItem.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var prop in props)
            {
                ApplyCurrentSubscribeContractItemPropertyChanged(subscribeItem, prop.Name);
            }
        }
        
        private void ApplyCurrentSubscribeContractItemPropertyChanged(SubscribeContractDataModel sender, string changedPropName)
        {
            if (currentSubscribeContractItem != sender) return;
            if (changedPropName == null) return;

            if (changedPropName == nameof(SubscribeContractDataModel.Quotation))
            {
                var quot = sender.Quotation;
                componentContentViewModel.LastPrice = quot?.LastPrice;
                componentContentViewModel.HighPrice = quot?.HighPrice;
                componentContentViewModel.OpenPrice = quot?.OpenPrice;
                componentContentViewModel.Volume = quot?.Volume;
                componentContentViewModel.LowPrice = quot?.LowPrice;
                componentContentViewModel.PreClosePrice = quot?.PreClosePrice;
                componentContentViewModel.Turnover = quot?.Turnover;
                componentContentViewModel.LowerLimitPrice = quot?.LowerLimitPrice;

                var bidPrice1 = quot?.BidPrice?.FirstOrDefault();
                componentContentViewModel.BidPrice1 = bidPrice1;

                var bidQty1 = quot?.BidQty?.FirstOrDefault();
                componentContentViewModel.BidQty1 = bidQty1;

                var askPrice1 = quot?.AskPrice?.FirstOrDefault();
                componentContentViewModel.AskPrice1 = askPrice1;

                var askQty1 = quot?.AskQty?.FirstOrDefault();
                componentContentViewModel.AskQty1 = askQty1;

                orderCreateViewController.UpdateViewPresentWithDatas(null, bidPrice1, bidQty1, askPrice1, askQty1, null, null, null);
            }
        }
        
        private void ApplyCurrentSubscribeComposeItem()
        {
            var subscribeItem = currentSubscribeComposeItem;
            if (subscribeItem == null) return;

            // TODO:组合的缓存行情显示
            
            var props = subscribeItem.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var prop in props)
            {
                ApplyCurrentSubscribeComposeItemPropertyChanged(subscribeItem, prop.Name);
            }
        }

        private void ApplyCurrentSubscribeComposeItemPropertyChanged(SubscribeComposeDataModel sender, string changedPropName)
        {
            if (currentSubscribeComposeItem != sender) return;
            if (changedPropName == null) return;

            if (changedPropName == nameof(SubscribeComposeDataModel.CombQuotation))
            {
                var quot = sender.CombQuotation;
                componentContentViewModel.LastPrice = quot?.LastPrice;
                componentContentViewModel.HighPrice = quot?.HighPrice;
                componentContentViewModel.OpenPrice = quot?.OpenPrice;
                componentContentViewModel.LowPrice = quot?.LowPrice;
                
                var bidPrice1 = quot?.BidPrice?.FirstOrDefault();
                componentContentViewModel.BidPrice1 = bidPrice1;
                
                var bidQty1 = quot?.BidQty?.FirstOrDefault();
                componentContentViewModel.BidQty1 = bidQty1;
                
                var askPrice1 = quot?.AskPrice?.FirstOrDefault();
                componentContentViewModel.AskPrice1 = askPrice1;
              
                var askQty1 = quot?.AskQty?.FirstOrDefault();
                componentContentViewModel.AskQty1 = askQty1;

                orderCreateViewController.UpdateViewPresentWithDatas(null, bidPrice1, bidQty1, askPrice1, askQty1, null, null, null);
            }
        }

        private void RefreshCurrentContractTargetDetail()
        {
            var contractIdStr = this.Component?.PlaceOrderContainerComponentDetail?.AttachContractId;
            if (contractIdStr == null) return;
            var contractId = Convert.ToInt32(contractIdStr);
            var container = new TargetContract_TargetContractDetail(contractId);
            XueQiaoFoundationHelper.SetupTargetContract_ContractDetail(container,
                contractItemTreeQueryCtrl, XueQiaoFoundation.BusinessResources.Models.XqContractNameFormatType.CommodityAcronym_Code_ContractCode,
                _container => 
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() => 
                    {
                        if (CheckIsSameTargetWithCurrent(ClientXQOrderTargetType.CONTRACT_TARGET, $"{contractId}"))
                        {
                            // 设置组件标题
                            if (Component != null)
                            {
                                Component.ComponentDescTitle = _container?.CnDisplayName;
                                // 设置 price tick
                                orderCreateViewController.UpdateViewPresentWithDatas(null, null, null, null, null, _container.CommodityDetail?.TickSize, null, null);
                            }
                        }
                    });
                });
        }

        private void RefreshCurrentComposeTargetDetail()
        {
            var composeIdStr = this.Component?.PlaceOrderContainerComponentDetail?.AttachComposeId;
            if (composeIdStr == null) return;
            var composeId = Convert.ToInt32(composeIdStr);
            
            var LoadedComposeView = new Action<NativeComposeView>(_view => 
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    if (CheckIsSameTargetWithCurrent(ClientXQOrderTargetType.COMPOSE_TARGET, $"{composeId}"))
                    {
                        // 更新 price tick
                        var priceTick = XueQiaoBusinessHelper.CalculteXqTargetPriceTick(_view?.PrecisionNumber, XueQiaoConstants.XQContractPriceMinimumPirceTick);
                        orderCreateViewController.UpdateViewPresentWithDatas(null, null, null, null, null, priceTick, null, null);

                        // 更新 component title
                        if (Component != null)
                        {
                            Component.ComponentDescTitle = _view?.AliasName;
                        }
                    }
                });
            });

            // 先查询是否存在 UserComposeView，存在则使用组合名称，不存在则使用 Legs 拼接的名称

            var existComposeView = subscribeComposeController.GetSubscribeItem(composeId)?
                    .FirstOrDefault(i => i.UserComposeViewContainer.UserComposeView != null)?.UserComposeViewContainer.UserComposeView;
            if (existComposeView != null)
            {
                LoadedComposeView(existComposeView);
                return;
            }

            XueQiaoFoundationHelper.LoadUserComposeView(composeId, userComposeViewCacheCtrl, userComposeViewQueryCtrl, true, true, 
                _detail => 
                {
                    if (_detail != null)
                    {
                        LoadedComposeView(_detail.UserComposeView);
                        return;
                    }

                    Task.Run(() => 
                    {
                        // 如果不存在组合视图，则查询组合信息
                        var composeGraph = composeGraphQueryCtrl.QueryComposeGraph(composeId)?.CorrectResult;

                        string composeViewName = null;
                        if (composeGraph != null)
                        {
                            var legNames = new List<string>();
                            foreach (var leg in composeGraph.Legs)
                            {
                                var legContainer = new TargetContract_TargetContractDetail((int)leg.SledContractId);
                                XueQiaoFoundationHelper.SyncQueryAndFillContractContainer(legContainer, contractItemTreeQueryController);
                                XueQiaoFoundationHelper.SetupDisplayNamesForContractContainer(legContainer, XueQiaoFoundation.BusinessResources.Models.XqContractNameFormatType.CommodityAcronym_Code_ContractCode);
                                if (!string.IsNullOrEmpty(legContainer.CnDisplayName))
                                    legNames.Add(legContainer.CnDisplayName);
                            }
                            composeViewName = XueQiaoBusinessHelper.GenerateXQComposeDefaultName(legNames.ToArray());
                        }
                        DispatcherHelper.CheckBeginInvokeOnUI(() => 
                        {
                            if (CheckIsSameTargetWithCurrent(ClientXQOrderTargetType.COMPOSE_TARGET, $"{composeId}"))
                            {
                                // 更新 price tick
                                var priceTick = XueQiaoConstants.XQContractPriceMinimumPirceTick;
                                orderCreateViewController.UpdateViewPresentWithDatas(null, null, null, null, null, priceTick, null, null);

                                // 更新 component title
                                if (Component != null)
                                {
                                    Component.ComponentDescTitle = composeViewName;
                                }
                            }
                        });
                    });
                });
        }
        
        private bool CheckIsSameTargetWithCurrent(ClientXQOrderTargetType targetType, string targetKey)
        {
            var currentComp = this.Component;
            if (currentComp == null) return false;

            if (targetType == ClientXQOrderTargetType.CONTRACT_TARGET)
            {
                var attachContractId = currentComp.PlaceOrderContainerComponentDetail?.AttachContractId;
                return targetKey == $"{attachContractId}";
            }
            else if (targetType == ClientXQOrderTargetType.COMPOSE_TARGET)
            {
                var attachComposeId = currentComp.PlaceOrderContainerComponentDetail?.AttachComposeId;
                return targetKey == $"{attachComposeId}";
            }
            return false;
        }
    }
}
