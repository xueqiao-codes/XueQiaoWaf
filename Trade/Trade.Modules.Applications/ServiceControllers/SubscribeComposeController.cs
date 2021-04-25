using NativeModel.Trade;
using Prism.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XueQiaoFoundation.Interfaces.Applications;
using XueQiaoFoundation.BusinessResources.Constants;
using XueQiaoFoundation.Interfaces.Event.business;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Helper;
using XueQiaoWaf.Trade.Modules.Applications.ServiceControllers.Events;
using XueQiaoWaf.Trade.Modules.Applications.Services;
using System.Collections.Specialized;
using XueQiaoWaf.Trade.Interfaces.Applications;
using XueQiaoFoundation.BusinessResources.DataModels;
using xueqiao.trade.hosting.proxy;
using System.Collections.ObjectModel;
using XueQiaoFoundation.Interfaces.Event.quotation_server;
using lib.xqclient_base.thriftapi_mediation;
using XueQiaoFoundation.BusinessResources.Models;
using XueQiaoFoundation.Interfaces.Helper;
using XueQiaoFoundation.BusinessResources.Helpers;
using System.Windows.Threading;
using business_foundation_lib.xq_thriftlib_config;

namespace XueQiaoWaf.Trade.Modules.Applications.ServiceControllers
{
    [Export(typeof(ISubscribeComposeController)), Export(typeof(ITradeModuleSingletonController)), PartCreationPolicy(CreationPolicy.Shared)]
    internal class SubscribeComposeController : ISubscribeComposeController, ITradeModuleSingletonController
    {
        private readonly SubscribeComposeService subscribeComposeService;
        private readonly IComposeGraphQueryController composeGraphQueryController;
        private readonly IComposeGraphCacheController composeGraphCacheController;
        private readonly IUserComposeViewQueryController userComposeViewQueryController;
        private readonly IUserComposeViewCacheController userComposeViewCacheController;
        private readonly ICommodityQueryController commodityQueryController;
        private readonly IContractQueryController contractQueryController;
        private readonly ILoginDataService loginDataService;
        private readonly Lazy<ILoginUserManageService> loginUserManageService;
        
        private readonly IEventAggregator eventAggregator;
        private readonly IContractItemTreeQueryController contractItemTreeQueryController;
        private readonly Lazy<IOrderItemsController> orderItemsController;
        private readonly Lazy<IXqTargetPositionItemsController> xqTargetPositionItemsCtrl;
        private readonly OrderItemsService orderItemsService;
        private readonly TargetPositionService targetPositionService;

        private readonly ConcurrentDictionary<Tuple<long, string>, SubscribeComposeDataModel> subscribeComposeDictionary;
        
        private CancellationTokenSource combSubscribeStatusQueryTaskCTS;
        private readonly object combSubscribeStatusQueryTasksLock = new object();

        // 正准备查询订阅状态的组合 id 列表
        private readonly List<long> prepareQuerySubscribeStatusCombItemIds = new List<long>();
        private readonly object prepareQuerySubscribeStatusCombItemIdsLock = new object();

        // 行情降频处理器
        private readonly QuotationDownConversionHandler<long, NativeCombQuotationItem> quotDownConversionHandler;

        // 检查标的是否过期的定时器
        private System.Timers.Timer checkTargetExpiredTimer;

        [ImportingConstructor]
        public SubscribeComposeController(
             SubscribeComposeService subscribeComposeService,
             IComposeGraphQueryController composeGraphQueryController,
             IComposeGraphCacheController composeGraphCacheController,
             IUserComposeViewQueryController userComposeViewQueryController,
             IUserComposeViewCacheController userComposeViewCacheController,
             ICommodityQueryController commodityQueryController,
             IContractQueryController contractQueryController,
             ILoginDataService loginDataService,
             Lazy<ILoginUserManageService> loginUserManageService,
             
             IEventAggregator eventAggregator,
             IContractItemTreeQueryController contractItemTreeQueryController,
             Lazy<IOrderItemsController> orderItemsController,
             Lazy<IXqTargetPositionItemsController> xqTargetPositionItemsCtrl,
             OrderItemsService orderItemsService,
             TargetPositionService targetPositionService)
        {
            this.subscribeComposeService = subscribeComposeService;
            this.composeGraphQueryController = composeGraphQueryController;
            this.composeGraphCacheController = composeGraphCacheController;
            this.userComposeViewQueryController = userComposeViewQueryController;
            this.userComposeViewCacheController = userComposeViewCacheController;
            this.commodityQueryController = commodityQueryController;
            this.contractQueryController = contractQueryController;
            this.loginDataService = loginDataService;
            this.loginUserManageService = loginUserManageService;
            
            this.eventAggregator = eventAggregator;
            this.contractItemTreeQueryController = contractItemTreeQueryController;
            this.orderItemsController = orderItemsController;
            this.xqTargetPositionItemsCtrl = xqTargetPositionItemsCtrl;
            this.orderItemsService = orderItemsService;
            this.targetPositionService = targetPositionService;

            subscribeComposeDictionary = new ConcurrentDictionary<Tuple<long, string>, SubscribeComposeDataModel>();

            quotDownConversionHandler = new QuotationDownConversionHandler<long, NativeCombQuotationItem>(800);
            quotDownConversionHandler.Time2HandleQuotation = (_handler, _time2HandleQuots) =>
            {
                if (_time2HandleQuots?.Any() != true) return;
                foreach (var item in _time2HandleQuots)
                {
                    UpdateSubscribeComposesWithSameId(item.Key,
                        updateItem => new SubscribeComposeUpdateTemplate
                        {
                            CombQuotation = new Tuple<NativeQuotationItem>(item.Value.CombQuotation),
                            LegQuotationItems = new Tuple<IEnumerable<NativeQuotationItem>>(item.Value.LegQuotations)
                        });
                }
            };

            CollectionChangedEventManager.AddHandler(orderItemsService.OrderItems, OrderItemsCollectionChanged);
            CollectionChangedEventManager.AddHandler(targetPositionService.PositionItems, TargetPositionsCollectionChanged);

            eventAggregator.GetEvent<ServerConnectOpen>().Subscribe(ReceivedQuotationServerConnectOpenEvent, ThreadOption.PublisherThread);
            eventAggregator.GetEvent<ServerConnectClose>().Subscribe(ReceivedQuotationServerConnectCloseEvent, ThreadOption.PublisherThread);
            loginUserManageService.Value.HasLogined += RecvUserLogined;
            loginUserManageService.Value.HasLogouted += RecvUserHasLogouted;
            eventAggregator.GetEvent<CombQuotationUpdated>().Subscribe(ReceivedCombQuotationUpdated, ThreadOption.PublisherThread);
            eventAggregator.GetEvent<UserComposeViewUpdatedEvent>().Subscribe(ReceivedUserComposeViewUpdatedEvent);
        }

        public void Shutdown()
        {
            StopCheckTargetExpiredTimer();
            CancelCombSubscribeStatusQueryTasks();
            RemoveAllSubscribeComposes();

            quotDownConversionHandler.Time2HandleQuotation = null;
            quotDownConversionHandler.Dispose();

            CollectionChangedEventManager.RemoveHandler(orderItemsService.OrderItems, OrderItemsCollectionChanged);
            CollectionChangedEventManager.RemoveHandler(targetPositionService.PositionItems, TargetPositionsCollectionChanged);

            loginUserManageService.Value.HasLogined -= RecvUserLogined;
            loginUserManageService.Value.HasLogouted -= RecvUserHasLogouted;
            eventAggregator.GetEvent<CombQuotationUpdated>().Unsubscribe(ReceivedCombQuotationUpdated);
            eventAggregator.GetEvent<UserComposeViewUpdatedEvent>().Unsubscribe(ReceivedUserComposeViewUpdatedEvent);
        }

        public SubscribeComposeDataModel AddOrUpdateSubscribeCompose(long composeId,
            string subscribeGroupKey,
            Func<bool, SubscribeComposeUpdateTemplate> sameComposeIdItemsUpdateAction)
        {
            if (loginDataService.ProxyLoginResp == null) return null;

            if (subscribeGroupKey == null) throw new ArgumentNullException("`subscribeGroupKey` can't be null.");

            bool isNewItem = false;
            var optItem = subscribeComposeDictionary.AddOrUpdate(new Tuple<long, string>(composeId, subscribeGroupKey),
                k =>
                {
                    var updateTemplate = sameComposeIdItemsUpdateAction?.Invoke(false);
                    var newItem = new SubscribeComposeDataModel(composeId, subscribeGroupKey);

                    // 初始化一些默认值
                    var existSameIdSubCompose = FirstSubscribeComposeItemOfComposeId(composeId);
                    var newItemQuotSubState = existSameIdSubCompose?.SubscribeState ?? MarketSubscribeState.Unkown;
                    newItem.SubscribeState = newItemQuotSubState;
                    newItem.SubscribeStateMsg = MarketSubscribeStateHelper.DefaultStateMsgForSubscribeState(newItemQuotSubState);
                    newItem.CombQuotation = existSameIdSubCompose?.CombQuotation;

                    if (updateTemplate == null)
                    {
                        updateTemplate = new SubscribeComposeUpdateTemplate();
                    }
                    if (updateTemplate.OnTradingSubAccountIds == null)
                    {
                        // Get OnTradingSubAccountIds from service
                        updateTemplate.OnTradingSubAccountIds = new Tuple<IEnumerable<long>>(orderItemsController.Value.OnTradingSubAccountIdsOfTarget($"{composeId}", ClientXQOrderTargetType.COMPOSE_TARGET)
                            ?? new long[] { });
                    }
                    if (updateTemplate.ExistPositionSubAccountIds == null)
                    {
                        // Get ExistPositionSubAccountIds from service
                        updateTemplate.ExistPositionSubAccountIds = new Tuple<IEnumerable<long>>(xqTargetPositionItemsCtrl.Value.ExistPositionSubAccountIdsOfTarget($"{composeId}", ClientXQOrderTargetType.COMPOSE_TARGET)
                            ?? new long[] { });
                    }
                    UpdateSubscribeComposeWithTemplate(newItem, updateTemplate);
                    EnqueueCombSubscribeStatusQueryTaskIfNeed(newItem);

                    // 设置 ComposeDetailContainer
                    XueQiaoFoundationHelper.SetupTargetCompose_ComposeDetail(newItem.ComposeDetailContainer,
                        composeGraphCacheController, composeGraphQueryController, userComposeViewCacheController, contractItemTreeQueryController,
                        XqContractNameFormatType.CommodityAcronym_Code_ContractCode,
                        _det =>
                        {
                            if (_det == null) return;

                            // 设置行情项
                            var legQitems = _det.DetailLegs?
                                .Select(i => new ComposeLegQuotationDM((int)i.BasicLeg.SledContractId)
                                {
                                    LegDetail = i,
                                }).ToArray();
                            if (legQitems != null)
                            {
                                newItem.LegQuotations = new ObservableCollection<ComposeLegQuotationDM>(legQitems);
                            }
                            RectifySubscribeItemLegQuotations(newItem);

                            // 更新是否过期
                            var nowTimestamp = (long)DateHelper.NowUnixTimeSpan().TotalSeconds;
                            InvalidateSubscribeItemIsExpired(newItem, nowTimestamp);
                        });

                    // 设置 UserComposeViewContainer
                    XueQiaoFoundationHelper.SetupUserComposeView(newItem.UserComposeViewContainer,
                        userComposeViewCacheController, userComposeViewQueryController, false, false,
                        _container =>
                        {
                            InvalidateSubscribeComposeAfterUpdatedComposeView(newItem);
                        });

                    // 添加至主线程的订阅合约列表
                    UIItemsAddOrRemoveDispatcherBeginInvoke(() =>
                    {
                        subscribeComposeService.Composes.Add(newItem);
                    });

                    // 发布事件 `SubscribeComposeAddedEvent`
                    eventAggregator.GetEvent<SubscribeComposeAddedEvent>().Publish(newItem);

                    isNewItem = true;
                    return newItem;
                },
                (k, existItem) =>
                {
                    return existItem;
                });
            
            Predicate<SubscribeComposeDataModel> predicate = null;
            if (isNewItem)
                predicate = new Predicate<SubscribeComposeDataModel>(i => (i.ComposeId == composeId && i != optItem));
            else
                predicate = new Predicate<SubscribeComposeDataModel>(i => i.ComposeId == composeId);

            var sameIdComposes = subscribeComposeDictionary.Values.Where(i => predicate(i));
            if (sameIdComposes.Any())
            {
                var updateTemplate = sameComposeIdItemsUpdateAction?.Invoke(true);
                // 根据模板，改变相同 id 的合约的一些信息
                if (updateTemplate != null)
                {
                    foreach (var combItem in sameIdComposes)
                    {
                        UpdateSubscribeComposeWithTemplate(combItem, updateTemplate);
                        EnqueueCombSubscribeStatusQueryTaskIfNeed(combItem);
                    }
                }
            }

            return optItem;
        }
        
        public void RemoveSubscribeCompose(long composeId, string subscribeGroupKey = null)
        {
            IEnumerable<Tuple<long, string>> rmKeys = null;
            if (subscribeGroupKey == null)
            {
                rmKeys = subscribeComposeDictionary.Keys.Where(i => i.Item1 == composeId);
            }
            else
            {
                rmKeys = new Tuple<long, string>[] { new Tuple<long, string>(composeId, subscribeGroupKey) };
            }

            foreach (var rmKey in rmKeys)
            {
                SubscribeComposeDataModel rmItem = null;
                if (subscribeComposeDictionary.TryRemove(rmKey, out rmItem))
                {
                    UIItemsAddOrRemoveDispatcherBeginInvoke(() =>
                    {
                        subscribeComposeService.Composes.Remove(rmItem);
                    });

                    // 发布事件 `SubscribeComposeRemovedEvent`
                    eventAggregator.GetEvent<SubscribeComposeRemovedEvent>().Publish(rmItem);
                }
            }
        }

        public void UpdateSubscribeComposesWithSameId(long composeId, Func<SubscribeComposeDataModel, SubscribeComposeUpdateTemplate> updateFactory)
        {
            if (updateFactory == null) return;
            var sameIdComposes = subscribeComposeDictionary.Values.Where(i => i.ComposeId == composeId);

            foreach (var combItem in sameIdComposes)
            {
                var updateTemplate = updateFactory?.Invoke(combItem);
                if (updateTemplate != null)
                {
                    UpdateSubscribeComposeWithTemplate(combItem, updateTemplate);
                }
            }
        }

        public IEnumerable<SubscribeComposeDataModel> GetSharedGroupKeySubscribeComposes()
        {
            return subscribeComposeDictionary.Values
                .Where(i => i.SubscribeGroupKey == SubscribeComposeDataModel.SharedListComposeGroupKey)
                .GroupBy(i => i.ComposeId)
                .Select(i => i.First())
                .OrderBy(i => i.ComposeId)
                .ToArray();
        }
        
        public Task<CombQuotationSubscribeInteractInfo> SubscribeCombQuotationAsync(long composeId)
        {
            return Task.Run(() => 
            {
                var interactInfo = new CombQuotationSubscribeInteractInfo { HasRequestApi = false };

                var landingInfo = loginDataService.LandingInfo;
                if (landingInfo == null) return interactInfo;
                
                var tarItem = FirstSubscribeComposeItemOfComposeId(composeId);
                if (tarItem == null) return interactInfo;
                
                if (tarItem.SubscribeState != MarketSubscribeState.Unkown
                    && tarItem.SubscribeState != MarketSubscribeState.Unsubscribed)
                    return interactInfo;

                UpdateComposesSubscribeState(composeId,
                        MarketSubscribeState.Subscribing,
                        () => MarketSubscribeStateHelper.DefaultStateMsgForSubscribeState(MarketSubscribeState.Subscribing));

                var sip = new StubInterfaceInteractParams { TransportConnectTimeoutMS = 1000, TransportReadTimeoutMS = 1000 };
                var resp = XqThriftLibConfigurationManager.SharedInstance.TradeHostingTerminalAoHttpStub.subscribeComposeViewQuotation(landingInfo, composeId, sip);

                interactInfo.HasRequestApi = true;
                interactInfo.ApiResponse = resp;

                if (loginDataService.LandingInfo == null)
                    return interactInfo;

                // 重新查询状态
                EnqueueCombSubscribeStatusQueryTask(composeId);

                return interactInfo;
            });
        }

        public Task<CombQuotationSubscribeInteractInfo> UnsubscribeCombQuotationAsync(long composeId)
        {
            return Task.Run(() =>
            {
                var interactInfo = new CombQuotationSubscribeInteractInfo { HasRequestApi = false };

                var landingInfo = loginDataService.LandingInfo;
                if (landingInfo == null) return interactInfo;

                var tarItem = FirstSubscribeComposeItemOfComposeId(composeId);
                if (tarItem == null) return interactInfo;

                if (tarItem.SubscribeState != MarketSubscribeState.Unkown
                    && tarItem.SubscribeState != MarketSubscribeState.Subscribed)
                    return interactInfo;

                UpdateComposesSubscribeState(composeId,
                        MarketSubscribeState.Unsubscribing,
                        () => MarketSubscribeStateHelper.DefaultStateMsgForSubscribeState(MarketSubscribeState.Unsubscribing));

                var iip = new StubInterfaceInteractParams { TransportConnectTimeoutMS = 1000, TransportReadTimeoutMS = 1000 };
                var resp = XqThriftLibConfigurationManager.SharedInstance.TradeHostingTerminalAoHttpStub.unSubscribeComposeViewQuotation(landingInfo, composeId, iip);

                interactInfo.HasRequestApi = true;
                interactInfo.ApiResponse = resp;

                if (loginDataService.LandingInfo == null)
                    return interactInfo;

                // 重新查询状态
                EnqueueCombSubscribeStatusQueryTask(composeId);

                return interactInfo;
            });
        }
        
        public bool ExistLegContractInCurrentComposes(int legContractId)
        {
            return subscribeComposeDictionary.Values
                .Any(i => 
                {
                    var legs = i.ComposeDetailContainer?.BasicComposeGraph?.Legs;
                    if (legs == null) return false;
                    return legs.Any(leg => leg.SledContractId == legContractId);
                });
        }

        public IEnumerable<SubscribeComposeDataModel> GetSubscribeItem(long composeId)
        {
            return subscribeComposeDictionary.Values.Where(i => i.ComposeId == composeId).ToArray();
        }

        public SubscribeComposeDataModel GetSubscribeItem(long composeId, string groupKey)
        {
            subscribeComposeDictionary.TryGetValue(new Tuple<long, string>(composeId, groupKey), out SubscribeComposeDataModel item);
            return item;
        }
        
        private void OrderItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var oldItems = e.OldItems?.Cast<OrderItemDataModel>().ToArray();
            var newItems = e.NewItems?.Cast<OrderItemDataModel>().ToArray();

            if (oldItems?.Any() == true)
            {
                foreach (var oldItem in oldItems)
                {
                    PropertyChangedEventManager.RemoveHandler(oldItem, OrderItemPropChanged, "");
                    UpdateSubscribeComposeIsTradingWhenChangedOrderState(oldItem.TargetType, oldItem.TargetKey,
                        oldItem.SubAccountFields.SubAccountId, null, true);
                }
            }

            if (newItems?.Any() == true)
            {
                foreach (var newItem in newItems)
                {
                    PropertyChangedEventManager.RemoveHandler(newItem, OrderItemPropChanged, "");
                    PropertyChangedEventManager.AddHandler(newItem, OrderItemPropChanged, "");
                    UpdateSubscribeComposeIsTradingWhenChangedOrderState(newItem.TargetType, newItem.TargetKey,
                        newItem.SubAccountFields.SubAccountId, newItem.OrderState, false);
                }
            }
        }

        private void OrderItemPropChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(OrderItemDataModel.OrderState))
            {
                var orderItem = sender as OrderItemDataModel;
                if (orderItem == null) return;
                UpdateSubscribeComposeIsTradingWhenChangedOrderState(orderItem.TargetType, orderItem.TargetKey,
                        orderItem.SubAccountFields.SubAccountId, orderItem.OrderState, false);
            }
        }

        private void TargetPositionsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var oldItems = e.OldItems?.OfType<TargetPositionDataModel>().ToArray();
            var newItems = e.NewItems?.OfType<TargetPositionDataModel>().ToArray();

            if (oldItems?.Any() == true)
            {
                foreach (var oldItem in oldItems)
                {
                    UpdateSubscribeComposeExistPositionWhenChangedPositionExistState(oldItem.TargetType, oldItem.TargetKey,
                        oldItem.SubAccountFields.SubAccountId, false);
                }
            }

            if (newItems?.Any() == true)
            {
                foreach (var newItem in newItems)
                {
                    UpdateSubscribeComposeExistPositionWhenChangedPositionExistState(newItem.TargetType, newItem.TargetKey,
                        newItem.SubAccountFields.SubAccountId, true);
                }
            }
        }

        private void ReceivedQuotationServerConnectOpenEvent(ServerConnectOpenEventMsg eventMsg)
        {
            if (eventMsg?.IsOpened == true)
            {
                // 行情 client 已启动
                // 开启行情降频处理器
                quotDownConversionHandler.Start();
            }
        }

        private void ReceivedQuotationServerConnectCloseEvent()
        {
            // 停止行情降频处理器
            quotDownConversionHandler.Stop();
        }

        private void RecvUserLogined()
        {
            StartCheckTargetExpiredTimer();
        }

        private void RecvUserHasLogouted(ProxyLoginResp lastLoginResp)
        {
            // 停止行情降频处理器
            quotDownConversionHandler.Stop();
            
            CancelCombSubscribeStatusQueryTasks();
            RemoveAllSubscribeComposes();
        }
        
        private void ReceivedCombQuotationUpdated(NativeCombQuotationItem combQuotation)
        {
            if (combQuotation.CombQuotation == null) return;
            var composeId = combQuotation.ComposeGraphId;

            quotDownConversionHandler.UpdateLastQuotation(composeId, combQuotation);
        }
        
        private void ReceivedUserComposeViewUpdatedEvent(NativeComposeView composeView)
        {
            if (composeView == null) return;
            var composeId = composeView.ComposeGraphId;

            Task.Run(() => 
            {
                var tarItems = subscribeComposeDictionary.Values.Where(i => i.ComposeId == composeId).ToArray();
                if (tarItems?.Any() != true) return;
                foreach (var item in tarItems)
                {
                    item.CreateTimestamp = composeView.CreateTimestamp;
                    var composeViewContainer = item.UserComposeViewContainer;
                    if (composeViewContainer != null)
                    {
                        composeViewContainer.UserComposeView = composeView;
                        InvalidateSubscribeComposeAfterUpdatedComposeView(item);
                    }
                }
            });
        }

        /// <summary>
        /// 为了防止死锁，用于 UI 显示的列表的增加或删除需要在主线程执行。该方法能保证顺序执行
        /// </summary>
        /// <param name="action"></param>
        private static void UIItemsAddOrRemoveDispatcherBeginInvoke(Action action)
        {
            if (action == null) return;
            DispatcherHelper.CheckBeginInvokeOnUI(() => action(),
                DispatcherPriority.Normal);
        }

        private void InvalidateSubscribeComposeAfterUpdatedComposeView(SubscribeComposeDataModel subscribeItem)
        {
            if (subscribeItem == null) return;
            var composeView = subscribeItem.UserComposeViewContainer?.UserComposeView;

            subscribeItem.XqTargetName = composeView?.AliasName;
            if (composeView != null)
            {
                subscribeItem.CreateTimestamp = composeView.CreateTimestamp;
            }

            var srcCombQuotation = subscribeItem.CombQuotation;
            if (srcCombQuotation != null)
            {
                RectifyComposeQuotation(srcCombQuotation, srcCombQuotation, composeView?.PrecisionNumber);
            }
        }


        /// <summary>
        /// 纠正组合腿行情数据，使其更精确
        /// </summary>
        /// <param name="targetQuotation">要精确的行情实体</param>
        /// <param name="srcQuatation">用于提供源数据的行情</param>
        /// <param name="commodityTickSize">商品 tick size</param>
        private static void RectifyComposeLegQuotation(NativeQuotationItem targetQuotation,
            NativeQuotationItem srcQuotation, double? commodityTickSize)
        {
            if (targetQuotation == null || srcQuotation == null) return;
            SubscribeDataQuotationHelper.UpdateSubscribeDataQuotation(targetQuotation, srcQuotation,
                (_propName, _sourceValue) =>
                {
                    if (_sourceValue == null) return _sourceValue;
                    double? destValue = null;
                    if (commodityTickSize != null && commodityTickSize != 0)
                        destValue = MathHelper.MakeValuePrecise(_sourceValue.Value, Math.Abs(commodityTickSize.Value));

                    return destValue;
                },
                (_propName, _scoureValue) =>
                {
                    if (_scoureValue?.Any() != true) return _scoureValue;
                    var destValues = new List<double>();

                    foreach (var srcVal in _scoureValue)
                    {
                        double? destVal = null;
                        if (commodityTickSize != null && commodityTickSize != 0)
                        {
                            destVal = MathHelper.MakeValuePrecise(srcVal, Math.Abs(commodityTickSize.Value));
                            destValues.Add(destVal.Value);
                        }
                    }
                    return destValues;
                });
        }

        /// <summary>
        /// 纠正组合行情数据，使其更精确
        /// </summary>
        /// <param name="targetQuotation">要精确的行情实体</param>
        /// <param name="srcQuatation">用于提供源数据的行情</param>
        private static void RectifyComposeQuotation(NativeQuotationItem targetQuotation,
            NativeQuotationItem srcQuotation, short? composeViewPrecisionNumber)
        {
            if (targetQuotation == null || srcQuotation == null) return;
            var priceTick = XueQiaoBusinessHelper.CalculteXqTargetPriceTick(composeViewPrecisionNumber, XueQiaoConstants.XQContractPriceMinimumPirceTick);
            SubscribeDataQuotationHelper.UpdateSubscribeDataQuotation(targetQuotation, srcQuotation,
                    (_propName, _sourceValue) =>
                    {
                        if (_sourceValue == null) return _sourceValue;
                        var destPrice = MathHelper.MakeValuePrecise(_sourceValue.Value, priceTick);
                        return destPrice;
                    },
                    (_propName, _scoureValue) =>
                    {
                        if (_scoureValue?.Any() != true) return _scoureValue;
                        var destValues = new List<double>();
                        foreach (var srcVal in _scoureValue)
                        {
                            var destVal = MathHelper.MakeValuePrecise(srcVal, priceTick);
                            if (!MathHelper.DoubleAreEqual(destVal, srcVal, XueQiaoConstants.XQTargetPriceAccuracyTolerance))
                            {
                                if (_propName == nameof(NativeQuotationItem.AskPrice))
                                {
                                    // 卖价在精确化处理后小于原始值时，进行向上一个 tick 处理
                                    if (destVal < srcVal) destVal += priceTick;
                                }
                                else if (_propName == nameof(NativeQuotationItem.BidPrice))
                                {
                                    // 买价在精确化处理后大于原始值时，进行向下一个 tick 处理
                                    if (destVal > srcVal) destVal -= priceTick;
                                }
                            }
                            destValues.Add(destVal);
                        }
                        return destValues;
                    });
        }

        private void UpdateSubscribeComposeIsTradingWhenChangedOrderState(ClientXQOrderTargetType orderTargetType, string orderTargetKey, long orderSubAccountId,
            ClientXQOrderState? changedOrderState, bool isRemoveOrder)
        {
            if (string.IsNullOrEmpty(orderTargetKey)) return;
            if (orderTargetType != ClientXQOrderTargetType.COMPOSE_TARGET) return;
            var composeId = Convert.ToInt64(orderTargetKey);

            bool onTradingState = true;
            if (isRemoveOrder)
                onTradingState = false;
            else if (changedOrderState != null)
                onTradingState = XueQiaoConstants.UnfinishedOrderStates.Contains(changedOrderState.Value);
            else {
                // if changedOrderState is null, do nothing
                return;
            }
            
            var existTargetSubItem = subscribeComposeDictionary.Values.FirstOrDefault(i => i.ComposeId == composeId);
            if (existTargetSubItem != null)
            {
                var originSubAccIds = existTargetSubItem.OnTradingSubAccountIds;
                if (onTradingState)
                {
                    IEnumerable<long> newSubAccIds = originSubAccIds ?? new long[] { };
                    if (!newSubAccIds.Contains(orderSubAccountId))
                    {
                        newSubAccIds = newSubAccIds.Union(new long[] { orderSubAccountId });
                        UpdateSubscribeComposesWithSameId(composeId,
                            _innerExistItem => new SubscribeComposeUpdateTemplate
                            {
                                OnTradingSubAccountIds = new Tuple<IEnumerable<long>>(newSubAccIds)
                            });
                    }
                }
                else
                {
                    IEnumerable<long> newSubAccIds = originSubAccIds ?? new long[] { };
                    if (newSubAccIds.Contains(orderSubAccountId))
                    {
                        newSubAccIds = newSubAccIds.Except(new long[] { orderSubAccountId });
                        UpdateSubscribeComposesWithSameId(composeId,
                            _innerExistItem => new SubscribeComposeUpdateTemplate
                            {
                                OnTradingSubAccountIds = new Tuple<IEnumerable<long>>(newSubAccIds)
                            });
                    }
                }
            }
        }

        private void UpdateSubscribeComposeExistPositionWhenChangedPositionExistState(ClientXQOrderTargetType targetType, string targetKey, long subAccountId,
            bool changedExistPosition)
        {
            if (string.IsNullOrEmpty(targetKey)) return;
            if (targetType != ClientXQOrderTargetType.COMPOSE_TARGET) return;
            var composeId = Convert.ToInt64(targetKey);

            var existTargetSubItem = subscribeComposeDictionary.Values.FirstOrDefault(i => i.ComposeId == composeId);
            if (existTargetSubItem != null)
            {
                var originSubAccIds = existTargetSubItem.ExistPositionSubAccountIds;
                if (changedExistPosition)
                {
                    IEnumerable<long> newSubAccIds = originSubAccIds ?? new long[] { };
                    if (!newSubAccIds.Contains(subAccountId))
                    {
                        newSubAccIds = newSubAccIds.Union(new long[] { subAccountId });
                        UpdateSubscribeComposesWithSameId(composeId,
                            _innerExistItem => new SubscribeComposeUpdateTemplate
                            {
                                ExistPositionSubAccountIds = new Tuple<IEnumerable<long>>(newSubAccIds)
                            });
                    }
                }
                else
                {
                    IEnumerable<long> newSubAccIds = originSubAccIds ?? new long[] { };
                    if (newSubAccIds.Contains(subAccountId))
                    {
                        newSubAccIds = newSubAccIds.Except(new long[] { subAccountId });
                        UpdateSubscribeComposesWithSameId(composeId,
                            _innerExistItem => new SubscribeComposeUpdateTemplate
                            {
                                ExistPositionSubAccountIds = new Tuple<IEnumerable<long>>(newSubAccIds)
                            });
                    }
                }
            }
        }

        private void RemoveAllSubscribeComposes()
        {
            var subItems = subscribeComposeDictionary.Values;
            subscribeComposeDictionary.Clear();
            UIItemsAddOrRemoveDispatcherBeginInvoke(() =>
            {
                subscribeComposeService.Composes.Clear();
            });
            foreach (var subItem in subItems)
            {
                eventAggregator.GetEvent<SubscribeComposeRemovedEvent>().Publish(subItem);
            }
        }

        private void EnqueueCombSubscribeStatusQueryTaskIfNeed(SubscribeComposeDataModel combSubscribeItem)
        {
            if (combSubscribeItem == null) return;
            if (combSubscribeItem.SubscribeState == MarketSubscribeState.Unkown)
            {
                // 去查询状态
                EnqueueCombSubscribeStatusQueryTask(combSubscribeItem.ComposeId);
            }
        }

        private void UpdateSubscribeComposeWithTemplate(SubscribeComposeDataModel subContractItem, SubscribeComposeUpdateTemplate template)
        {
            if (subContractItem == null) return;
            if (template == null) return;
            
            if (template.CombQuotation != null)
            {
                var tempQ = template.CombQuotation.Item1;
                var newQuot = new NativeQuotationItem();
                if (tempQ != null)
                {
                    RectifyComposeQuotation(newQuot, tempQ, subContractItem.UserComposeViewContainer?.UserComposeView?.PrecisionNumber);
                }
                subContractItem.CombQuotation = newQuot;
            }

            if (template.LegQuotationItems != null)
            {
                var tempLegQs = template.LegQuotationItems.Item1;
                var currentLegQuotationDMs = subContractItem.LegQuotations;
                if (currentLegQuotationDMs?.Any() == true && tempLegQs?.Any() == true)
                {
                    foreach (var newLegQuot in tempLegQs)
                    {
                        var existLegQuotDM = currentLegQuotationDMs.FirstOrDefault(i => 
                            newLegQuot.ContractSymbol?.ExchangeMic == i.LegDetail?.LegDetailContainer?.CommodityDetail?.ExchangeMic
                            && newLegQuot.ContractSymbol?.CommodityType == i.LegDetail?.LegDetailContainer?.CommodityDetail?.SledCommodityType
                            && newLegQuot.ContractSymbol?.CommodityCode == i.LegDetail?.LegDetailContainer?.CommodityDetail?.SledCommodityCode
                            && newLegQuot.ContractSymbol?.ContractCode == i.LegDetail?.LegDetailContainer?.ContractDetail?.SledContractCode);
                        if(existLegQuotDM == null) continue;

                        var newQuot = new NativeQuotationItem { ContractSymbol = newLegQuot.ContractSymbol };
                        RectifyComposeLegQuotation(newQuot, newLegQuot, existLegQuotDM.LegDetail?.LegDetailContainer?.CommodityDetail?.TickSize);
                        existLegQuotDM.Quotation = newQuot;
                    }
                }
            }

            if (template.SubscribeState != null)
            {
                subContractItem.SubscribeState = template.SubscribeState.Item1;
            }

            if (template.SubscribeStateMsg != null)
            {
                subContractItem.SubscribeStateMsg = template.SubscribeStateMsg.Item1;
            }
            
            if (template.CustomGroupKeys != null)
            {
                subContractItem.CustomGroupKeys = template.CustomGroupKeys.Item1?.ToArray();
            }

            if (template.OnTradingSubAccountIds != null)
            {
                subContractItem.OnTradingSubAccountIds = template.OnTradingSubAccountIds.Item1?.ToArray();
            }

            if (template.ExistPositionSubAccountIds != null)
            {
                subContractItem.ExistPositionSubAccountIds = template.ExistPositionSubAccountIds.Item1?.ToArray();
            }
        }
        
        private void UpdateComposesSubscribeState(long withComposeId, MarketSubscribeState targetSubscribeState, Func<string> targetSubscribeStateMsgFactory)
        {
            var sameIdItems = subscribeComposeDictionary.Values.Where(i => i.ComposeId == withComposeId);
            foreach (var i in sameIdItems)
            {
                i.SubscribeState = targetSubscribeState;
                if (targetSubscribeStateMsgFactory != null)
                {
                    i.SubscribeStateMsg = targetSubscribeStateMsgFactory();
                }
            }
        }

        private SubscribeComposeDataModel FirstSubscribeComposeItemOfComposeId(long composeId)
        {
            return subscribeComposeDictionary.Values.FirstOrDefault(i => i.ComposeId == composeId);
        }
        
        private void EnqueueCombSubscribeStatusQueryTask(long composeId)
        {
            if (composeId <= 0) return;

            CancellationToken cToken;
            lock (combSubscribeStatusQueryTasksLock)
            {
                if (prepareQuerySubscribeStatusCombItemIds.Contains(composeId)) return;
                if (combSubscribeStatusQueryTaskCTS == null
                    || combSubscribeStatusQueryTaskCTS.IsCancellationRequested)
                {
                    combSubscribeStatusQueryTaskCTS = new CancellationTokenSource();
                }
                cToken = combSubscribeStatusQueryTaskCTS.Token;
            }

            var needEnque2Query = false; 
            lock (prepareQuerySubscribeStatusCombItemIdsLock)
            {
                if (!prepareQuerySubscribeStatusCombItemIds.Contains(composeId))
                {
                    prepareQuerySubscribeStatusCombItemIds.Add(composeId);
                    needEnque2Query = true;
                }
            }
            if (!needEnque2Query) return;

            Task.Run(() =>
            {
                var needReallyQuery = false;
                lock (prepareQuerySubscribeStatusCombItemIdsLock)
                {
                    if (prepareQuerySubscribeStatusCombItemIds.Contains(composeId))
                    {
                        prepareQuerySubscribeStatusCombItemIds.Remove(composeId);
                        needReallyQuery = true;
                    }
                }
                if (!needReallyQuery) return;

                if (cToken.IsCancellationRequested) return;
                var resp = userComposeViewQueryController.QueryCurrentComposeView(composeId);
                if (cToken.IsCancellationRequested) return;
                if (resp?.CorrectResult != null)
                {
                    var queriedStatus = resp?.CorrectResult.UserComposeView.SubscribeStatus;
                    MarketSubscribeState subState = MarketSubscribeState.Unkown;
                    if (queriedStatus == ClientComposeViewSubscribeStatus.SUBSCRIBED)
                    {
                        subState = MarketSubscribeState.Subscribed;
                    }
                    else if (queriedStatus == ClientComposeViewSubscribeStatus.UNSUBSCRIBED)
                    {
                        subState = MarketSubscribeState.Unsubscribed;
                    }
                    UpdateComposesSubscribeState(composeId, subState,
                            () => MarketSubscribeStateHelper.DefaultStateMsgForSubscribeState(subState));
                }
                else
                {
                    UpdateComposesSubscribeState(composeId, MarketSubscribeState.Unkown, () => "未获取到订阅状态");
                }
            }, cToken);
        }

        private void CancelCombSubscribeStatusQueryTasks()
        {
            lock (combSubscribeStatusQueryTasksLock)
            {
                if (combSubscribeStatusQueryTaskCTS != null)
                {
                    combSubscribeStatusQueryTaskCTS.Cancel();
                    combSubscribeStatusQueryTaskCTS.Dispose();
                    combSubscribeStatusQueryTaskCTS = null;
                }
            }
        }


        /// <summary>
        /// 更新订阅项的过期状态。和指定的时间比较，如果标的过期时间小于指定时间，则标识持仓过期
        /// </summary>
        /// <param name="positionItem"></param>
        /// <param name="compareTimestamp">指定的比较时间</param>
        private void InvalidateSubscribeItemIsExpired(SubscribeComposeDataModel subscribeItem, long compareTimestamp)
        {
            if (subscribeItem == null) return;
            var existExpiredLeg = subscribeItem.ComposeDetailContainer?.DetailLegs?
                .Any(i =>
                {
                    var expDate = i.LegDetailContainer?.ContractDetail?.ContractExpDate;
                    return expDate != null && (compareTimestamp > expDate);
                });
            if (existExpiredLeg != null)
            {
                subscribeItem.IsXqTargetExpired = existExpiredLeg.Value;
            }
        }

        private void RectifySubscribeItemLegQuotations(SubscribeComposeDataModel subscribeItem)
        {
            if (subscribeItem == null) return;
            var srcQuotationLegs = subscribeItem.LegQuotations?.ToArray();
            if (srcQuotationLegs?.Any() == true)
            {
                foreach (var srcQuotationLeg in srcQuotationLegs)
                {
                    var srcLegQuotation = srcQuotationLeg.Quotation;
                    if (srcLegQuotation != null)
                    {
                        RectifyComposeLegQuotation(srcLegQuotation, srcLegQuotation, srcQuotationLeg.LegDetail?.LegDetailContainer?.CommodityDetail?.TickSize);
                    }
                }
            }
        }

        private void StartCheckTargetExpiredTimer()
        {
            StopCheckTargetExpiredTimer();
            // 10 minute interval
            checkTargetExpiredTimer = new System.Timers.Timer(1000 * 60 * 10);
            checkTargetExpiredTimer.Elapsed += CheckTargetExpiredTimer_Elapsed;
            checkTargetExpiredTimer.Start();
        }

        private void CheckTargetExpiredTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var nowTimestamp = (long)DateHelper.NowUnixTimeSpan().TotalSeconds;
            var items = subscribeComposeDictionary.Values;
            foreach (var item in items)
            {
                InvalidateSubscribeItemIsExpired(item, nowTimestamp);
            }
        }
        
        private void StopCheckTargetExpiredTimer()
        {
            if (checkTargetExpiredTimer != null)
            {
                checkTargetExpiredTimer.Elapsed -= CheckTargetExpiredTimer_Elapsed;
                checkTargetExpiredTimer.Stop();
                checkTargetExpiredTimer.Dispose();
                checkTargetExpiredTimer = null;
            }
        }
    }
}
