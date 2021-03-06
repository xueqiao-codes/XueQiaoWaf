using AppAssembler.Interfaces.Applications;
using business_foundation_lib.helper;
using business_foundation_lib.xq_thriftlib_config;
using ContainerShell.Interfaces.Applications;
using IDLAutoGenerated.Util;
using lib.xqclient_base.logger;
using lib.xqclient_base.thriftapi_mediation;
using NativeModel.Trade;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Waf.Applications;
using xueqiao.trade.hosting.arbitrage.thriftapi;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.BusinessResources.Helpers;
using XueQiaoFoundation.BusinessResources.Models;
using XueQiaoFoundation.Interfaces.Applications;
using XueQiaoFoundation.Interfaces.Helper;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;
using XueQiaoWaf.Trade.Interfaces.Applications;
using XueQiaoWaf.Trade.Interfaces.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Helper;
using XueQiaoWaf.Trade.Modules.Applications.ViewModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class PlaceOrderCreateViewController : IController
    {
        private readonly ILoginDataService loginDataService;
        private readonly ITradeModuleService tradeModuleService;
        private readonly IExchangeDataService exchangeDataService;
        private readonly IOrderItemsController orderItemsController;
        private readonly IContractItemTreeQueryController contractItemTreeQueryCtrl;
        private readonly IComposeGraphCacheController composeGraphCacheCtrl;
        private readonly IComposeGraphQueryController composeGraphQueryCtrl;
        private readonly IUserComposeViewCacheController userComposeViewCacheCtrl;
        private readonly IUserSubAccountRelatedItemQueryController subAccountRelatedItemQueryCtrl;
        private readonly IUserSubAccountRelatedItemCacheController subAccountRelatedItemCacheCtrl;
        private readonly IHostingUserQueryController hostingUserQueryCtrl;
        private readonly IHostingUserCacheController hostingUserCacheCtrl;
        private readonly IUserComposeViewQueryController userComposeViewQueryCtrl;
        private readonly IEventAggregator eventAggregator;
        private readonly IContainerShellService containerShellService;
        private readonly IMessageWindowService messageWindowService;
        private readonly IAppAssemblerService appAssemblerService;
        private readonly PlaceOrderCreateMainViewModel createMainVM;
        private readonly PlaceOrderCreateConditionVM createConditionVM;
        private readonly ExportFactory<XQComposeOrderEPTManDialogCtrl> composeOrderEPTManDialogCtrlFactory;
        private readonly ExportFactory<PlaceOrderConfirmVM> placeOrderConfirmVMFactory;

        private readonly DelegateCommand bidPriceClickCommand;
        private readonly DelegateCommand askPriceClickCommand;
        private readonly DelegateCommand buyCommand;
        private readonly DelegateCommand sellCommand;
        private readonly DelegateCommand toComposeOrderEPTManageCmd;

        private PlaceOrderComponentPresentKey componentPresentKey;
        private PlaceOrderViewCreateDramaBase placeOrderViewCreateDrama;

        [ImportingConstructor]
        public PlaceOrderCreateViewController(ILoginDataService loginDataService,
            ITradeModuleService tradeModuleService,
            IExchangeDataService exchangeDataService,
            IOrderItemsController orderItemsController,
            IContractItemTreeQueryController contractItemTreeQueryCtrl,
            IComposeGraphCacheController composeGraphCacheCtrl,
            IComposeGraphQueryController composeGraphQueryCtrl,
            IUserComposeViewCacheController userComposeViewCacheCtrl,
            IUserSubAccountRelatedItemQueryController subAccountRelatedItemQueryCtrl,
            IUserSubAccountRelatedItemCacheController subAccountRelatedItemCacheCtrl,
            IHostingUserQueryController hostingUserQueryCtrl,
            IHostingUserCacheController hostingUserCacheCtrl,
            IUserComposeViewQueryController userComposeViewQueryCtrl,
            IEventAggregator eventAggregator,
            IContainerShellService containerShellService,
            IMessageWindowService messageWindowService,
            IAppAssemblerService appAssemblerService,
            PlaceOrderCreateMainViewModel createMainVM,
            PlaceOrderCreateConditionVM createConditionVM,
            ExportFactory<XQComposeOrderEPTManDialogCtrl> composeOrderEPTManDialogCtrlFactory,
            ExportFactory<PlaceOrderConfirmVM> placeOrderConfirmVMFactory)
        {
            this.loginDataService = loginDataService;
            this.tradeModuleService = tradeModuleService;
            this.exchangeDataService = exchangeDataService;
            this.orderItemsController = orderItemsController;
            this.contractItemTreeQueryCtrl = contractItemTreeQueryCtrl;
            this.composeGraphCacheCtrl = composeGraphCacheCtrl;
            this.composeGraphQueryCtrl = composeGraphQueryCtrl;
            this.userComposeViewCacheCtrl = userComposeViewCacheCtrl;
            this.subAccountRelatedItemQueryCtrl = subAccountRelatedItemQueryCtrl;
            this.subAccountRelatedItemCacheCtrl = subAccountRelatedItemCacheCtrl;
            this.hostingUserQueryCtrl = hostingUserQueryCtrl;
            this.hostingUserCacheCtrl = hostingUserCacheCtrl;
            this.userComposeViewQueryCtrl = userComposeViewQueryCtrl;
            this.eventAggregator = eventAggregator;
            this.containerShellService = containerShellService;
            this.messageWindowService = messageWindowService;
            this.appAssemblerService = appAssemblerService;
            this.createMainVM = createMainVM;
            this.createConditionVM = createConditionVM;
            this.composeOrderEPTManDialogCtrlFactory = composeOrderEPTManDialogCtrlFactory;
            this.placeOrderConfirmVMFactory = placeOrderConfirmVMFactory;

            bidPriceClickCommand = new DelegateCommand(BidPriceClick);
            askPriceClickCommand = new DelegateCommand(AskPriceClick);
            buyCommand = new DelegateCommand(PlaceOrderBuy);
            sellCommand = new DelegateCommand(PlaceOrderSell);
            toComposeOrderEPTManageCmd = new DelegateCommand(ToComposeOrderEPTManage);
        }
        
        /// <summary>
        /// 下单主视图
        /// </summary>
        public object PlaceOrderCreateMainView => createMainVM.View;

        /// <summary>
        /// 下单条件视图
        /// </summary>
        public object PlaceOrderCreateConditionView => createConditionVM.View;
        
        public void Initialize()
        {
            createMainVM.BidPriceClickCommand = this.bidPriceClickCommand;
            createMainVM.AskPriceClickCommand = this.askPriceClickCommand;
            createMainVM.BuyCommand = buyCommand;
            createMainVM.SellCommand = sellCommand;
            createConditionVM.ToComposeOrderEPTManageCmd = toComposeOrderEPTManageCmd;

            PropertyChangedEventManager.AddHandler(createMainVM, CreateMainVMPropChanged, "");
            PropertyChangedEventManager.AddHandler(createConditionVM, CreateConditionVMPropChanged, "");
        }

        public void Run()
        {

        }

        public void Shutdown()
        {
            PropertyChangedEventManager.RemoveHandler(createMainVM, CreateMainVMPropChanged, "");
            PropertyChangedEventManager.RemoveHandler(createConditionVM, CreateConditionVMPropChanged, "");
        }

        /// <summary>
        /// 更新界面显示 key
        /// </summary>
        /// <param name="componentPresentKey"></param>
        public void UpdateViewPresentKey(PlaceOrderComponentPresentKey componentPresentKey)
        {
            this.componentPresentKey = componentPresentKey;

            UpdateSupportPlaceOrderTypes();
            UpdatePlaceOrderViewCreateDrama();
        }

        /// <summary>
        /// 设置数据，更新界面
        /// </summary>
        public void UpdateViewPresentWithDatas(int? orderQuantity, double? bidPrice1, long? bidQty1, double? askPrice1, long? askQty1,
            double? orderTargetPriceTickSize, double? price, ClientPlaceOrderType? placeOrderType)
        {
            if (orderQuantity != null) createMainVM.OrderQuantity = orderQuantity.Value;
            if (bidPrice1 != null) createMainVM.BidPrice1 = bidPrice1.Value;
            if (bidQty1 != null) createMainVM.BidQty1 = bidQty1.Value;
            if (askPrice1 != null) createMainVM.AskPrice1 = askPrice1.Value;
            if (askQty1 != null) createMainVM.AskQty1 = askQty1.Value;
            if (orderTargetPriceTickSize != null)
            {
                createMainVM.OrderTargetPriceTickSize = orderTargetPriceTickSize.Value;
            }

            if (price != null)
            {
                var priceTypeValue = placeOrderViewCreateDrama?.SelectedPriceTypeValue;
                if (priceTypeValue != null && priceTypeValue.PriceType == HostingXQOrderPriceType.ACTION_PRICE_LIMIT)
                {
                    priceTypeValue.LimitPrice = price.Value;
                }
            }

            if (placeOrderType != null)
            {
                createMainVM.SelectedPlaceOrderType = placeOrderType.Value;
            }
        }

        /// <summary>
        /// 清除界面上的市场信息
        /// </summary>
        public void ClearViewMarketDatas()
        {
            createMainVM.BidPrice1 = null;
            createMainVM.BidQty1 = null;
            createMainVM.AskPrice1 = null;
            createMainVM.AskQty1 = null;
        }
        
        private void CreateMainVMPropChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PlaceOrderCreateMainViewModel.SelectedPlaceOrderType))
            {
                UpdatePlaceOrderViewCreateDrama();
                return;
            }
        }

        private void CreateConditionVMPropChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PlaceOrderCreateConditionVM.SelectedComposeOrderEPT))
            {
                var presentKey = this.componentPresentKey;
                var viewCreateDrama = this.placeOrderViewCreateDrama;
                if (presentKey == null || viewCreateDrama == null) return;

                if (presentKey.TargetType == ClientXQOrderTargetType.COMPOSE_TARGET
                    && viewCreateDrama is PlaceOrderViewCreateDramaXQComposeBase composeOrderDrama)
                {
                    // set drama's EPT with selected EPT
                    composeOrderDrama.OrderExecParamsTemplate = createConditionVM.SelectedComposeOrderEPT;
                }
                return;
            }
        }

        private void BidPriceClick()
        {
            if (createMainVM.BidPrice1 is double bidPrice1)
            {
                var priceTypeValue = placeOrderViewCreateDrama?.SelectedPriceTypeValue;
                if (priceTypeValue != null && priceTypeValue.PriceType == HostingXQOrderPriceType.ACTION_PRICE_LIMIT)
                {
                    priceTypeValue.LimitPrice = bidPrice1;
                }
            }
        }

        private void AskPriceClick()
        {
            if (createMainVM.AskPrice1 is double askPrice1)
            {
                var priceTypeValue = placeOrderViewCreateDrama?.SelectedPriceTypeValue;
                if (priceTypeValue != null && priceTypeValue.PriceType == HostingXQOrderPriceType.ACTION_PRICE_LIMIT)
                {
                    priceTypeValue.LimitPrice = askPrice1;
                }
            }
        }

        private void ToComposeOrderEPTManage()
        {
            var dialogCtrl = composeOrderEPTManDialogCtrlFactory.CreateExport().Value;
            dialogCtrl.DialogOwner = UIHelper.GetWindowOfUIElement(PlaceOrderCreateConditionView);
            dialogCtrl.Initialize();
            dialogCtrl.Run();
            dialogCtrl.Shutdown();
        }

        private void PlaceOrderBuy()
        {
            DoPlaceOrder(true);
        }

        private void PlaceOrderSell()
        {
            DoPlaceOrder(false);
        }

        private void DoPlaceOrder(bool trueForBuyFalseForSell)
        {
            var landingInfo = loginDataService.ProxyLoginResp?.HostingSession?.HostingSession2LandingInfo();
            if (landingInfo == null) return;
            
            var presentKey = this.componentPresentKey;
            var orderSubAccountId = presentKey.SubAccountId;

            if (presentKey == null || presentKey.TargetType == null) return;

            var dialogOwner = UIHelper.GetWindowOfUIElement(createConditionVM.View);
            if (orderSubAccountId == null || orderSubAccountId == 0)
            {
                messageWindowService.ShowMessageDialog(dialogOwner, null, null, null, "请选择操作账户");
                return;
            }

            var orderQuantity = createMainVM.OrderQuantity;
            var viewCreateDrama = this.placeOrderViewCreateDrama;
            if (viewCreateDrama == null) return;
            
            if (orderQuantity <= 0)
            {
                messageWindowService.ShowMessageDialog(dialogOwner, null, null, null, "下单数量必须大于0");
                return;
            }

            viewCreateDrama.ValidateAndGenerateOrderDetail(out HostingXQOrderDetail orderDetail,
                out HostingXQOrderType? xqOrderType, out string validateErrorMsg,
                orderQuantity, 
                trueForBuyFalseForSell ? HostingXQTradeDirection.XQ_BUY : HostingXQTradeDirection.XQ_SELL);
            
            if (validateErrorMsg != null)
            {
                messageWindowService.ShowMessageDialog(dialogOwner, null, null, null, validateErrorMsg);
                return;
            }

            if (xqOrderType == null) throw new ArgumentNullException("xqOrderType can'te be null after ValidatePlaceOrder passed.");
            if (orderDetail == null) throw new ArgumentNullException("orderDetail can'te be null after ValidatePlaceOrder passed.");

            AppLog.Info($"Try create order:");
            AppLog.Info($"{orderDetail}");

            var clientOrderTargetType = viewCreateDrama.ViewOrderTargetType;
            var xqTarget = new HostingXQTarget
            {
                TargetType = clientOrderTargetType.ToHostingXQTargetType(),
                TargetKey = presentKey.TargetKey
            };
            var newOrderId = tradeModuleService.GenerateXQOrderId(orderSubAccountId.Value);

            bool passConfirm = true;
            if (appAssemblerService.PreferenceManager.Config?.PlaceOrderNeedConfirm == true)
            {
                // 弹出确认框
                var subAccountFields = new SubAccountFieldsForTradeData(landingInfo.SubUserId, orderSubAccountId.Value);
                TradeDMLoadHelper.SetupSubAccountFields(subAccountFields,
                    subAccountRelatedItemQueryCtrl, subAccountRelatedItemCacheCtrl,
                    hostingUserQueryCtrl, hostingUserCacheCtrl);

                var targetDM = new XqTargetDM(clientOrderTargetType);
                ConfigXqTargetDM(targetDM, xqTarget.TargetKey);
                
                var confirmVM = placeOrderConfirmVMFactory.CreateExport().Value;
                confirmVM.NotConfirmNextTime = false;
                confirmVM.OrderTarget = targetDM;
                confirmVM.OrderType = xqOrderType.Value;
                confirmVM.OrderSubAccountFields = subAccountFields;
                confirmVM.OrderDetail = orderDetail;
                
                var confirmResult = messageWindowService.ShowQuestionDialog(UIHelper.GetWindowOfUIElement(createMainVM.View), null, null, "下单确认", confirmVM.View, false, "确认", "取消");
                passConfirm = (confirmResult == true);

                if (confirmResult == true && confirmVM.NotConfirmNextTime)
                {
                    appAssemblerService.PreferenceManager.Config.PlaceOrderNeedConfirm = false;
                    appAssemblerService.PreferenceManager.SaveConfig(out Exception _exception);
                    if (_exception != null)
                    {
                        AppLog.Error("Failed to save config.", _exception);
                    }
                }
                
                confirmVM = null;
            }

            if (passConfirm == false) return;

            tradeModuleService.OrderOperateRequestTaskFactory.StartNew(() =>
            {
                // 添加新建订单到订单列表
                var addOrder = AddNewOrderToOrderList(orderSubAccountId.Value, newOrderId, xqOrderType.Value, viewCreateDrama.ViewOrderTargetType, presentKey.TargetKey, orderDetail, landingInfo.SubUserId);

                // 请求接口创建订单
                // 下单时超时设置小一点
                var interactParams = new StubInterfaceInteractParams
                { TransportConnectTimeoutMS = 2000, TransportReadTimeoutMS = 2000, LogInterfaceRequestArgs = false };
                var orderResp = XqThriftLibConfigurationManager.SharedInstance.TradeHostingTerminalAoHttpStub.createXQOrder(landingInfo, orderSubAccountId.Value,
                    newOrderId, xqOrderType.Value, xqTarget, orderDetail,
                    interactParams);

                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    if (orderResp == null || orderResp.HasTransportException)
                    {
                        // 出错，则修改状态为未知
                        UpdateOrderStateInOrderList(newOrderId, xqOrderType.Value,
                            ClientXQOrderState.ClientInaccurate_Unkown, new HostingXQOrderState());

                        // 未知状态订单提醒
                        if (addOrder != null)
                        {
                            containerShellService.ShowOrderStateAmbiguousNotification(addOrder);
                        }
                    }
                    else if (orderResp.CustomParsedExceptionResult?.BusinessErrorCode > 0)
                    {
                        // 出错，则修改状态为未知
                        UpdateOrderStateInOrderList(newOrderId, xqOrderType.Value,
                            ClientXQOrderState.ClientInaccurate_Unkown, new HostingXQOrderState());

                        var errMsg = FoundationHelper.FormatResponseDisplayErrorMsg(orderResp, "下单出错！\n");
                        var owner = UIHelper.GetWindowOfUIElement(createMainVM.View);
                        if (owner != null)
                            messageWindowService.ShowMessageDialog(owner, null, null, null, errMsg);
                        return;
                    }
                });
            });
        }

        private void ConfigXqTargetDM(XqTargetDM xqTargetDM, string targetKey)
        {
            if (xqTargetDM == null) return;
            if (string.IsNullOrEmpty(targetKey)) return;

            if (xqTargetDM.TargetType == ClientXQOrderTargetType.CONTRACT_TARGET)
            {
                var contractId = Convert.ToInt32(targetKey);
                xqTargetDM.TargetContractDetailContainer = new TargetContract_TargetContractDetail(contractId);
                XueQiaoFoundationHelper.SetupTargetContract_ContractDetail(xqTargetDM.TargetContractDetailContainer,
                    contractItemTreeQueryCtrl, XqContractNameFormatType.CommodityAcronym_Code_ContractCode,
                    _container =>
                    {
                        XqTargetDMHelper.InvalidateTargetNameWithAppropriate(xqTargetDM, XqAppLanguages.CN);
                    });
            }
            else if (xqTargetDM.TargetType == ClientXQOrderTargetType.COMPOSE_TARGET)
            {
                var composeId = Convert.ToInt64(targetKey);
                xqTargetDM.TargetComposeDetailContainer = new TargetCompose_ComposeDetail(composeId);
                XueQiaoFoundationHelper.SetupTargetCompose_ComposeDetail(xqTargetDM.TargetComposeDetailContainer,
                    composeGraphCacheCtrl, composeGraphQueryCtrl, userComposeViewCacheCtrl, contractItemTreeQueryCtrl,
                    XqContractNameFormatType.CommodityAcronym_Code_ContractCode,
                    _container =>
                    {
                        XqTargetDMHelper.InvalidateTargetNameWithAppropriate(xqTargetDM, XqAppLanguages.CN);
                    });

                xqTargetDM.TargetComposeUserComposeViewContainer = new UserComposeViewContainer(composeId);
                XueQiaoFoundationHelper.SetupUserComposeView(xqTargetDM.TargetComposeUserComposeViewContainer,
                    userComposeViewCacheCtrl, userComposeViewQueryCtrl, false, true,
                    _container =>
                    {
                        XqTargetDMHelper.InvalidateTargetNameWithAppropriate(xqTargetDM, XqAppLanguages.CN);
                    });
            }
        }

        private OrderItemDataModel AddNewOrderToOrderList(long orderSubAccountId, string newOrderId,
            HostingXQOrderType xqOrderType, ClientXQOrderTargetType targetType, string targetKey,
            HostingXQOrderDetail newOrderDetail, int creatorUserId)
        {
            var addOrderState = ClientXQOrderState.Client_RequestCreating;
            var addOrderStateDetail = new HostingXQOrderState ();
            var currentTimestampMs = (long)DateHelper.NowUnixTimeSpan().TotalMilliseconds;

            // 添加订单，并设置状态为请求创建中
            if (OrderItemDataModel_Entrusted.IsOrder_Entrusted(xqOrderType))
            {
                return orderItemsController.AddOrUpdateOrder_Entrusted(orderSubAccountId, newOrderId,
                    xqOrderType, targetType, targetKey,
                    newOrderDetail, creatorUserId, existOrder =>
                    {
                        if (existOrder != null) return null;
                        var template = new OrderUpdateTemplate_Entrusted
                        {
                            OrderState = new Tuple<ClientXQOrderState>(addOrderState),
                            OrderStateDetail = new Tuple<HostingXQOrderState>(addOrderStateDetail),
                            CreateTimestampMs = new Tuple<long>(currentTimestampMs),
                        };
                        return template;
                    });
            }
            else if (OrderItemDataModel_Parked.IsOrder_Parked(xqOrderType))
            {
                return orderItemsController.AddOrUpdateOrder_Parked(orderSubAccountId, newOrderId,
                    xqOrderType, targetType, targetKey,
                    newOrderDetail, creatorUserId, existOrder =>
                    {
                        if (existOrder != null) return null;
                        var template = new OrderUpdateTemplate_Parked
                        {
                            OrderState = new Tuple<ClientXQOrderState>(addOrderState),
                            OrderStateDetail = new Tuple<HostingXQOrderState>(addOrderStateDetail),
                            CreateTimestampMs = new Tuple<long>(currentTimestampMs),
                        };
                        return template;
                    });
            }
            else if (OrderItemDataModel_Condition.IsOrder_Condition(xqOrderType))
            {
                return orderItemsController.AddOrUpdateOrder_Condition(orderSubAccountId, newOrderId,
                    xqOrderType, targetType, targetKey,
                    newOrderDetail, creatorUserId, existOrder =>
                    {
                        if (existOrder != null) return null;
                        var template = new OrderUpdateTemplate_Condition
                        {
                            OrderState = new Tuple<ClientXQOrderState>(addOrderState),
                            OrderStateDetail = new Tuple<HostingXQOrderState>(addOrderStateDetail),
                            CreateTimestampMs = new Tuple<long>(currentTimestampMs),
                        };
                        return template;
                    });
            }
            else
            {
                return null;
            }
        }

        private void UpdateOrderStateInOrderList(string orderId, HostingXQOrderType xqOrderType, 
            ClientXQOrderState toOrderState, HostingXQOrderState toOrderStateDetail)
        {
            if (OrderItemDataModel_Entrusted.IsOrder_Entrusted(xqOrderType))
            {
                orderItemsController.UpdateOrder_Entrusted(orderId, existOrder =>
                {
                    if (existOrder == null) return null;
                    var template = new OrderUpdateTemplate_Entrusted
                    {
                        OrderState = new Tuple<ClientXQOrderState>(toOrderState),
                        OrderStateDetail = new Tuple<HostingXQOrderState>(toOrderStateDetail),
                    };
                    return template;
                });
            }
            else if (OrderItemDataModel_Parked.IsOrder_Parked(xqOrderType))
            {
                orderItemsController.UpdateOrder_Parked(orderId, existOrder =>
                {
                    if (existOrder == null) return null;
                    var template = new OrderUpdateTemplate_Parked
                    {
                        OrderState = new Tuple<ClientXQOrderState>(toOrderState),
                        OrderStateDetail = new Tuple<HostingXQOrderState>(toOrderStateDetail),
                    };
                    return template;
                });
            }
            else if (OrderItemDataModel_Condition.IsOrder_Condition(xqOrderType))
            {
                orderItemsController.UpdateOrder_Condition(orderId, existOrder =>
                {
                    if (existOrder == null) return null;
                    var template = new OrderUpdateTemplate_Condition
                    {
                        OrderState = new Tuple<ClientXQOrderState>(toOrderState),
                        OrderStateDetail = new Tuple<HostingXQOrderState>(toOrderStateDetail),
                    };
                    return template;
                });
            }
        }

        private void UpdateSupportPlaceOrderTypes()
        {
            var presentKey = this.componentPresentKey;

            PropertyChangedEventManager.RemoveHandler(createMainVM, CreateMainVMPropChanged, "");
            if (presentKey?.TargetType == ClientXQOrderTargetType.CONTRACT_TARGET)
            {
                // 合约标的
                EnumHelper.GetAllTypesForEnum(typeof(ClientPlaceOrderType), out IEnumerable<ClientPlaceOrderType> supportPlaceOrderTypes);
                createMainVM.SupportPlaceOrderTypes.Clear();
                createMainVM.SupportPlaceOrderTypes.AddRange(supportPlaceOrderTypes);
                createMainVM.SelectedPlaceOrderType = supportPlaceOrderTypes.FirstOrDefault();
            }
            else if (presentKey?.TargetType == ClientXQOrderTargetType.COMPOSE_TARGET)
            {
                // 组合标的
                EnumHelper.GetAllTypesForEnum(typeof(ClientPlaceOrderType), out IEnumerable<ClientPlaceOrderType> allPlaceOrderTypes);
                var supportPlaceOrderTypes = allPlaceOrderTypes.Except(new ClientPlaceOrderType[] { ClientPlaceOrderType.PARKED });
                createMainVM.SupportPlaceOrderTypes.Clear();
                createMainVM.SupportPlaceOrderTypes.AddRange(supportPlaceOrderTypes);
                createMainVM.SelectedPlaceOrderType = supportPlaceOrderTypes.FirstOrDefault();
            }
            else
            {
                createMainVM.SupportPlaceOrderTypes.Clear();
                createMainVM.SelectedPlaceOrderType = null;
            }
            PropertyChangedEventManager.AddHandler(createMainVM, CreateMainVMPropChanged, "");
        }

        /// <summary>
        /// 更新视图创建剧本
        /// </summary>
        private void UpdatePlaceOrderViewCreateDrama()
        {
            var presentKey = this.componentPresentKey;
            var placeOrderType = createMainVM.SelectedPlaceOrderType;
            var viewCreateDrama = NewPlaceOrderViewCreateDrama(placeOrderType, presentKey?.TargetType);
            
            this.placeOrderViewCreateDrama = viewCreateDrama;
            createMainVM.PlaceOrderViewCreateDrama = viewCreateDrama;
            createConditionVM.PlaceOrderViewCreateDrama = viewCreateDrama;

            if (presentKey != null)
            {
                if (presentKey.TargetType == ClientXQOrderTargetType.COMPOSE_TARGET
                    && viewCreateDrama is PlaceOrderViewCreateDramaXQComposeBase composeOrderDrama)
                {
                    var composeGraphId = Convert.ToInt64(presentKey.TargetKey);
                    UpdateXqComposeOrderDramaLegMetas(composeOrderDrama, composeGraphId);

                    // set drama's EPT with selected EPT
                    composeOrderDrama.OrderExecParamsTemplate = createConditionVM.SelectedComposeOrderEPT;
                }
            }
        }

        private void UpdateXqComposeOrderDramaLegMetas(PlaceOrderViewCreateDramaXQComposeBase composeOrderDrama, long composeGraphId)
        {
            Task.Run(() => 
            {
                var _composeGraph = XueQiaoFoundationHelper.QueryXQComposeGraph(composeGraphId,
                    composeGraphCacheCtrl,
                    composeGraphQueryCtrl,
                    userComposeViewCacheCtrl);
                if (_composeGraph != null)
                {
                    var legContractIds = _composeGraph.Legs?.Select(i => (int)i.SledContractId).ToArray() ?? new int[] { };
                    var legDetails = contractItemTreeQueryCtrl.QueryTreeItems(legContractIds, true, true, false, CancellationToken.None);
                    composeOrderDrama.LegMetas = legDetails?.Values
                        .Where(i => i.Contract != null && i.ParentExchange != null)
                        .Select(i => new XQOrderComposeLegMeta(i.Contract.SledContractId)
                        {
                            IsBelongOutterExchange = !exchangeDataService.IsInnerExchange(i.ParentExchange.ExchangeMic)
                        })
                        .ToArray();
                }
            });
        }

        private static PlaceOrderViewCreateDramaBase NewPlaceOrderViewCreateDrama(ClientPlaceOrderType? placeOrderType, ClientXQOrderTargetType? orderTargetType)
        {
            PlaceOrderViewCreateDramaBase viewCreateDrama = null;
            if (orderTargetType == ClientXQOrderTargetType.CONTRACT_TARGET)
            {
                // 合约标的
                switch (placeOrderType)
                {
                    case ClientPlaceOrderType.PRICE_LIMIT:
                        viewCreateDrama = new PlaceOrderViewCreateDrama_ContractLimitPrice();
                        break;
                    case ClientPlaceOrderType.PARKED:
                        viewCreateDrama = new PlaceOrderViewCreateDrama_ContractParked();
                        break;
                    case ClientPlaceOrderType.BUY_STOP_LOST:
                        viewCreateDrama = new PlaceOrderViewCreateDrama_ContractStopLostBuy();
                        break;
                    case ClientPlaceOrderType.SELL_STOP_LOST:
                        viewCreateDrama = new PlaceOrderViewCreateDrama_ContractStopLostSell();
                        break;
                    case ClientPlaceOrderType.BUY_STOP_PROFIT:
                        viewCreateDrama = new PlaceOrderViewCreateDrama_ContractStopProfitBuy();
                        break;
                    case ClientPlaceOrderType.SELL_STOP_PROFIT:
                        viewCreateDrama = new PlaceOrderViewCreateDrama_ContractStopProfitSell();
                        break;
                }
            }
            else if (orderTargetType == ClientXQOrderTargetType.COMPOSE_TARGET)
            {
                // 组合标的
                switch (placeOrderType)
                {
                    case ClientPlaceOrderType.PRICE_LIMIT:
                        viewCreateDrama = new PlaceOrderViewCreateDrama_ComposeLimitPrice();
                        break;
                    case ClientPlaceOrderType.BUY_STOP_LOST:
                        viewCreateDrama = new PlaceOrderViewCreateDrama_ComposeStopLostBuy();
                        break;
                    case ClientPlaceOrderType.SELL_STOP_LOST:
                        viewCreateDrama = new PlaceOrderViewCreateDrama_ComposeStopLostSell();
                        break;
                    case ClientPlaceOrderType.BUY_STOP_PROFIT:
                        viewCreateDrama = new PlaceOrderViewCreateDrama_ComposeStopProfitBuy();
                        break;
                    case ClientPlaceOrderType.SELL_STOP_PROFIT:
                        viewCreateDrama = new PlaceOrderViewCreateDrama_ComposeStopProfitSell();
                        break;
                }
            }

            return viewCreateDrama;
        }
    }
}
