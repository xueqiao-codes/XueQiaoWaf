using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using XueQiaoFoundation.Shared.Interface;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using ContainerShell.Applications.ViewModels;
using Prism.Events;
using XueQiaoWaf.Trade.Interfaces.Events;
using XueQiaoFoundation.BusinessResources.DataModels;
using System.Waf.Applications;
using NativeModel.Trade;
using ContainerShell.Interfaces.Applications;
using System.Media;
using lib.xqclient_base.logger;
using xueqiao.trade.hosting.arbitrage.thriftapi;
using XueQiaoFoundation.UI.Components.ToastNotification;
using ToastNotifications.Core;
using AppAssembler.Interfaces.Applications;

namespace ContainerShell.Applications.Controllers
{
    /// <summary>
    /// 处理交易通知的 controller
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class TradeNotificationCtrl : IController
    {
        private readonly ExportFactory<OrderOccurExceptionNDPVM> orderOccurExceptionNDPVMFactory;
        private readonly ExportFactory<OrderTriggeredNDPVM> orderTriggeredNDPVMFactory;
        private readonly ExportFactory<OrderTradedNDPVM> orderTradedNDPVMFactory;
        private readonly ExportFactory<OrderAmbiguousNDPVM> orderAmbiguousNDPVMFactory;
        private readonly ExportFactory<TradeLameNDPVM> tradeLameNDPVMFactory;
        private readonly ExportFactory<OrderCancelledNDPVM> orderCancelledNDPVMFactory;
        private readonly IContainerShellService containerShellService;
        private readonly IAppAssemblerService appAssemblerService;
        private readonly IEventAggregator eventAggregator;
        
        private readonly Dictionary<string, XqToastNotification> orderIdKeyedExceptionOrderToasts = new Dictionary<string, XqToastNotification>();
        private readonly Dictionary<string, XqToastNotification> orderIdKeyedTriggeredOrderToasts = new Dictionary<string, XqToastNotification>();
        private readonly Dictionary<long, XqToastNotification> tradeIdKeyedTradeToasts = new Dictionary<long, XqToastNotification>();
        private readonly Dictionary<string, XqToastNotification> orderIdKeyedAmbiguousOrderToasts = new Dictionary<string, XqToastNotification>();
        private readonly Dictionary<XQTradeItemKey, XqToastNotification> tradeLameTaskNoteToasts = new Dictionary<XQTradeItemKey, XqToastNotification>();
        private readonly Dictionary<string, XqToastNotification> orderCancelledToasts = new Dictionary<string, XqToastNotification>();

        private readonly SoundPlayer soundPlayer = new SoundPlayer();
        
        private NotifierWrapper_WindowPositionProvider tradeToastNotifierWrapper;
        private INotificationsLifetimeSupervisor tradeToastNotifierLifetimeSupervisor;
        
        [ImportingConstructor]
        public TradeNotificationCtrl(
            ExportFactory<OrderOccurExceptionNDPVM> orderOccurExceptionNDPVMFactory,
            ExportFactory<OrderTriggeredNDPVM> orderTriggeredNDPVMFactory,
            ExportFactory<OrderTradedNDPVM> orderTradedNDPVMFactory,
            ExportFactory<OrderAmbiguousNDPVM> orderAmbiguousNDPVMFactory,
            ExportFactory<TradeLameNDPVM> tradeLameNDPVMFactory,
            ExportFactory<OrderCancelledNDPVM> orderCancelledNDPVMFactory,
            IContainerShellService containerShellService,
            IAppAssemblerService appAssemblerService,
            IEventAggregator eventAggregator)
        {
            this.orderOccurExceptionNDPVMFactory = orderOccurExceptionNDPVMFactory;
            this.orderTriggeredNDPVMFactory = orderTriggeredNDPVMFactory;
            this.orderTradedNDPVMFactory = orderTradedNDPVMFactory;
            this.orderAmbiguousNDPVMFactory = orderAmbiguousNDPVMFactory;
            this.tradeLameNDPVMFactory = tradeLameNDPVMFactory;
            this.orderCancelledNDPVMFactory = orderCancelledNDPVMFactory;
            this.containerShellService = containerShellService;
            this.appAssemblerService = appAssemblerService;
            this.eventAggregator = eventAggregator;
        }

        public Window NotificationPositionWindow { get; set; }

        public Point BottomRightCornerPositionOffset { get; set; }

        public void Initialize()
        {
            if (NotificationPositionWindow == null) throw new ArgumentNullException("NotificationPositionWindow");

            AcquireTradeToastNotifierWrapper();

            NotificationPositionWindow.Closed += NotificationPositionWindow_Closed;

            eventAggregator.GetEvent<OrderEventDrivedFromPush>().Subscribe(RecvOrderEvent, ThreadOption.UIThread);
            eventAggregator.GetEvent<OrderTradedEventDrivedFromPush>().Subscribe(RecvOrderTradedEvent, ThreadOption.UIThread);
            eventAggregator.GetEvent<AmbiguousOrderStateQueryFailedEvent>().Subscribe(RecvAmbiguousOrderStateQueryFailed, ThreadOption.UIThread);
            eventAggregator.GetEvent<TradeLameTaskNoteNativeEvent>().Subscribe(RecvTradeLameTaskNoteEvent, ThreadOption.UIThread);
        }

        public void Run()
        {
            
        }

        public void Shutdown()
        {
            NotificationPositionWindow.Closed -= NotificationPositionWindow_Closed;

            DisposeTradeToastNotifierLifetimeSupervisor();
            DisposeTradeToastNotifierWrapper();
            soundPlayer?.Stop();

            eventAggregator.GetEvent<OrderEventDrivedFromPush>().Unsubscribe(RecvOrderEvent);
            eventAggregator.GetEvent<OrderTradedEventDrivedFromPush>().Unsubscribe(RecvOrderTradedEvent);
            eventAggregator.GetEvent<AmbiguousOrderStateQueryFailedEvent>().Unsubscribe(RecvAmbiguousOrderStateQueryFailed);
            eventAggregator.GetEvent<TradeLameTaskNoteNativeEvent>().Unsubscribe(RecvTradeLameTaskNoteEvent);
        }
        
        /// <summary>
        /// 显示订单发生异常 Notification
        /// </summary>
        public void ShowOrderOccurExceptionNotification(OrderItemDataModel order)
        {
            HandleOrderOccurExceptionNotification(order);
        }

        /// <summary>
        /// 显示订单已触发 Notification
        /// </summary>
        public void ShowOrderTriggeredNotification(OrderItemDataModel order)
        {
            HandleOrderTriggeredNotification(order);
        }

        /// <summary>
        /// 显示订单已成交 Notification
        /// </summary>
        public void ShowOrderTradedNotification(TradeItemDataModel trade)
        {
            HandleOrderTradedNotification(trade);
        }
        
        /// <summary>
        /// 显示订单状态异常 Notification
        /// </summary>
        public void ShowOrderStateAmbiguousNotification(OrderItemDataModel order)
        {
            HandleOrderStateAmbiguousNotification(order);
        }
        
        /// <summary>
        /// 显示组合瘸腿成交任务项 Notification
        /// </summary>
        public void ShowTradeLameTaskNoteNotification(XQTradeLameTaskNote lameLastNote)
        {
            HandleTradeLameTaskNoteNotification(lameLastNote);
        }

        private NotifierWrapper_WindowPositionProvider AcquireTradeToastNotifierWrapper()
        {
            if (tradeToastNotifierWrapper == null)
            {
                tradeToastNotifierWrapper = new NotifierWrapper_WindowPositionProvider(NotificationPositionWindow,
                    Corner.BottomRight,
                    BottomRightCornerPositionOffset.X,
                    BottomRightCornerPositionOffset.Y,
                    _confWrapper =>
                    {
                        _confWrapper.LifetimeSupervisor = new Tuple<INotificationsLifetimeSupervisor>(AcquireTradeToastNotifierLifetimeSupervisor());
                        _confWrapper.DisplayOptions = new Tuple<DisplayOptions>(new DisplayOptions { TopMost = true, Width = 360 });
                    });
            }
            return tradeToastNotifierWrapper;
        }

        private INotificationsLifetimeSupervisor AcquireTradeToastNotifierLifetimeSupervisor()
        {
            if (tradeToastNotifierLifetimeSupervisor == null)
            {
                tradeToastNotifierLifetimeSupervisor = new TimeAndFIFONotificationLifetimeSupervisor(TimeSpan.FromSeconds(15), 3);
                tradeToastNotifierLifetimeSupervisor.CloseNotificationRequested += TradeToastNotifierLifetimeSupervisor_ClosedNotification;
            }
            return tradeToastNotifierLifetimeSupervisor;
        }

        private void DisposeTradeToastNotifierWrapper()
        {
            if (tradeToastNotifierWrapper != null)
            {
                tradeToastNotifierWrapper.Dispose();
                tradeToastNotifierWrapper = null;
            }
        }

        private void DisposeTradeToastNotifierLifetimeSupervisor()
        {
            if (tradeToastNotifierLifetimeSupervisor != null)
            {
                tradeToastNotifierLifetimeSupervisor.CloseNotificationRequested -= TradeToastNotifierLifetimeSupervisor_ClosedNotification;
                tradeToastNotifierLifetimeSupervisor.Dispose();
                tradeToastNotifierLifetimeSupervisor = null;
            }
        }

        private void NotificationPositionWindow_Closed(object sender, EventArgs e)
        {
            DisposeTradeToastNotifierLifetimeSupervisor();
            DisposeTradeToastNotifierWrapper();
        }

        private void TradeToastNotifierLifetimeSupervisor_ClosedNotification(object sender, CloseNotificationEventArgs e)
        {
            if (sender != tradeToastNotifierLifetimeSupervisor) return;
            var notification = e.Notification as XqToastNotification;
            if (notification.NotificationDisplayPartVM is OrderOccurExceptionNDPVM orderOccurErrNAPVM)
            {
                // 订单发生异常的 toast 关闭
                var orderId = orderOccurErrNAPVM.Order?.OrderId;
                if (orderId != null)
                    orderIdKeyedExceptionOrderToasts.Remove(orderId);
            }
            else if (notification.NotificationDisplayPartVM is OrderTriggeredNDPVM orderTriggeredNAPVM)
            {
                // 订单已触发的 toast 关闭
                var orderId = orderTriggeredNAPVM.Order?.OrderId;
                if (orderId != null)
                    orderIdKeyedTriggeredOrderToasts.Remove(orderId);
            }
            else if (notification.NotificationDisplayPartVM is OrderTradedNDPVM orderTradedNAPVM)
            {
                // 订单已成交的 toast 关闭
                var tradeId = orderTradedNAPVM.Trade?.TradeId;
                if (tradeId != null)
                    tradeIdKeyedTradeToasts.Remove(tradeId.Value);
            }
            else if (notification.NotificationDisplayPartVM is OrderAmbiguousNDPVM orderAmbiguousNDPVM)
            {
                // 订单状态不明确提醒 toast 关闭
                var orderId = orderAmbiguousNDPVM.Order?.OrderId;
                if (orderId != null)
                    orderIdKeyedAmbiguousOrderToasts.Remove(orderId);
            }
            else if (notification.NotificationDisplayPartVM is TradeLameNDPVM tradeLameNDPVM)
            {
                // 雪橇瘸腿成交提醒 toast 关闭
                var lameTaskNote = tradeLameNDPVM.LameTaskNote;
                if (lameTaskNote != null)
                {
                    var itemKey = new XQTradeItemKey(lameTaskNote.SubAccountId, lameTaskNote.XQTradeId);
                    tradeLameTaskNoteToasts.Remove(itemKey);
                }
            }
            else if (notification.NotificationDisplayPartVM is OrderCancelledNDPVM orderCancelledNDPVM)
            {
                // 撤单提醒 toast 关闭
                var orderId = orderCancelledNDPVM.Order?.OrderId;
                if (orderId != null)
                    orderCancelledToasts.Remove(orderId);
            }
        }

        /// <summary>
        /// 订单是否触发
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        private static bool IsOrderTriggered(OrderItemDataModel order)
        {
            bool triggered = false;
            if ((order is OrderItemDataModel_Condition condOrder)
                && condOrder.OrderState == ClientXQOrderState.XQ_ORDER_FINISHED)
            {
                triggered = true;
            }
            else if ((order is OrderItemDataModel_Parked parkedOrder)
                && parkedOrder.OrderState == ClientXQOrderState.XQ_ORDER_FINISHED)
            {
                triggered = true;
            }
            return triggered;
        }

        /// <summary>
        /// 是否需要提醒特定类型的撤单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        private static bool IsNeedNotifyPeculiarCancelledOrder(OrderItemDataModel order)
        {
            var needNotify = false;
            if (order.OrderStateDetail?.StateValue == HostingXQOrderStateValue.XQ_ORDER_CANCELLED)
            {
                needNotify = order.OrderStateDetail?.CancelledErrorCode == TradeHostingArbitrageErrorCode.ERROR_XQ_ORDER_CANCELLED_STARTED_BUT_AFTER_EFFECT_TIME_PERIOD.GetHashCode();
            }
            return needNotify;
        }

        private void RecvOrderEvent(OrderEventPayload payload)
        {
            if (payload == null) return;
            if (payload.OrderEventType == OrderEventType.Create
                || payload.OrderEventType == OrderEventType.Update)
            {
                var order = payload.Order;
                if (order == null) return;

                if (order.IsSuspendedWithError)
                {
                    HandleOrderOccurExceptionNotification(order);
                }
                else if (IsOrderTriggered(order))
                {
                    HandleOrderTriggeredNotification(order);
                }
                else if (IsNeedNotifyPeculiarCancelledOrder(order))
                {
                    HandleOrderCancelledNotification(order);
                }
            }
            else if (payload.OrderEventType == OrderEventType.Remove)
            {
                if (payload.RemovedOrderId != null)
                {
                    RemoveOrderToastNotification(payload.RemovedOrderId);
                }   
            }
        }

        private void RecvOrderTradedEvent(OrderTradedEventPayload payload)
        {
            if (payload == null) return;
            HandleOrderTradedNotification(payload.Trade);
        }

        private void RecvAmbiguousOrderStateQueryFailed(OrderItemDataModel payload)
        {
            if (payload == null) return;
            HandleOrderStateAmbiguousNotification(payload);
        }
        
        private void RecvTradeLameTaskNoteEvent(TradeLameTaskNoteNativeEventPayload payload)
        {
            if (payload == null) return;
            if (payload.EventType == TradeLameTaskNoteNativeEventType.Create)
            {
                if (payload.TaskNote != null)
                {
                    HandleTradeLameTaskNoteNotification(payload.TaskNote);
                }
            }
            else if (payload.EventType == TradeLameTaskNoteNativeEventType.Delete)
            {
                if (payload.ItemKey != null)
                {
                    RemoveTradeLameTaskNoteToastNotification(payload.ItemKey);
                }
            }
        }


        private void ShowExceptionOrder(OrderItemDataModel order)
        {
            if (order == null) return;
            containerShellService.ShowExceptionOrdersPanelWindow();
            containerShellService.ActiveExceptionOrderTabInExceptionOrderPanel(order);
        }

        private void ShowAmbiguousOrder(OrderItemDataModel order)
        {
            if (order == null) return;
            containerShellService.ShowExceptionOrdersPanelWindow();
            containerShellService.ActiveAmbiguousOrderTabInExceptionOrderPanel(order);
        }
        
        private void ShowXQTradeLameTaskNote(XQTradeLameTaskNote lameTaskNote)
        {
            if (lameTaskNote == null) return;
            containerShellService.ShowExceptionOrdersPanelWindow();
            containerShellService.ActiveTradeLameTabInExceptionOrderPanel(lameTaskNote);
        }
        
        private void PlaySound(string soundLocation)
        {
            soundPlayer.Stop();
            soundPlayer.SoundLocation = soundLocation;
            try
            {
                soundPlayer.Play();
            }
            catch (Exception e)
            {
                AppLog.Error($"Failed to play sound of path: {soundLocation}, {e}");
            }
        }

        /// <summary>
        /// 处理订单异常提醒
        /// </summary>
        /// <param name="order"></param>
        private void HandleOrderOccurExceptionNotification(OrderItemDataModel order)
        {
            if (!orderIdKeyedExceptionOrderToasts.ContainsKey(order.OrderId))
            {
                //XQSoundManager.PlaySound(XQSoundManager.OrderOccurException);
                var audioFileName = appAssemblerService.PreferenceManager.Config.OrderErrAudioFileName;
                PlaySound(audioFileName);
                
                var ndpvm = orderOccurExceptionNDPVMFactory.CreateExport().Value;
                var notification = new XqToastNotification(ndpvm, true);

                ndpvm.Order = order;
                ndpvm.ShowDetailCmd = new DelegateCommand((obj) => 
                {
                    notification.Close();
                    var __order = obj as OrderItemDataModel;
                    if (__order == null) return;
                    ShowExceptionOrder(__order);
                });
                ndpvm.CloseCmd = new DelegateCommand(() => notification.Close());

                orderIdKeyedExceptionOrderToasts.Add(order.OrderId, notification);

                AcquireTradeToastNotifierWrapper()?.Notify<XqToastNotification>(() => notification);
            }
        }

        /// <summary>
        /// 处理订单触发提醒
        /// </summary>
        /// <param name="order"></param>
        private void HandleOrderTriggeredNotification(OrderItemDataModel order)
        {
            if (!orderIdKeyedTriggeredOrderToasts.ContainsKey(order.OrderId))
            {
                //XQSoundManager.PlaySound(XQSoundManager.OrderTriggered);
                var audioFileName = appAssemblerService.PreferenceManager.Config.OrderTriggeredAudioFileName;
                PlaySound(audioFileName);

                var ndpvm = orderTriggeredNDPVMFactory.CreateExport().Value;
                var notification = new XqToastNotification(ndpvm, true);

                ndpvm.Order = order;
                ndpvm.CloseCmd = new DelegateCommand(() => notification.Close());

                orderIdKeyedTriggeredOrderToasts.Add(order.OrderId, notification);

                AcquireTradeToastNotifierWrapper()?.Notify<XqToastNotification>(() => notification);
            }
        }

        /// <summary>
        /// 处理订单撤单提醒
        /// </summary>
        /// <param name="order"></param>
        private void HandleOrderCancelledNotification(OrderItemDataModel order)
        {
            if (!orderCancelledToasts.ContainsKey(order.OrderId))
            {
                var audioFileName = appAssemblerService.PreferenceManager.Config.OrderOtherNotifyAudioFileName;
                PlaySound(audioFileName);

                var ndpvm = orderCancelledNDPVMFactory.CreateExport().Value;
                var notification = new XqToastNotification(ndpvm, true);

                ndpvm.Order = order;
                ndpvm.CloseCmd = new DelegateCommand(() => notification.Close());

                orderCancelledToasts.Add(order.OrderId, notification);

                AcquireTradeToastNotifierWrapper()?.Notify<XqToastNotification>(() => notification);
            }
        }

        /// <summary>
        /// 处理成交提醒
        /// </summary>
        /// <param name="order"></param>
        private void HandleOrderTradedNotification(TradeItemDataModel trade)
        {
            if (!tradeIdKeyedTradeToasts.ContainsKey(trade.TradeId))
            {
                //XQSoundManager.PlaySound(XQSoundManager.OrderTraded);
                var audioFileName = appAssemblerService.PreferenceManager.Config.OrderTradedAudioFileName;
                PlaySound(audioFileName);

                var ndpvm = orderTradedNDPVMFactory.CreateExport().Value;
                var notification = new XqToastNotification(ndpvm, true);

                ndpvm.Trade = trade;
                ndpvm.CloseCmd = new DelegateCommand(() => notification.Close());

                tradeIdKeyedTradeToasts.Add(trade.TradeId, notification);

                AcquireTradeToastNotifierWrapper()?.Notify<XqToastNotification>(() => notification);
            }
        }

        /// <summary>
        /// 处理订单状态不明提醒
        /// </summary>
        /// <param name="order"></param>
        private void HandleOrderStateAmbiguousNotification(OrderItemDataModel order)
        {
            if (!orderIdKeyedAmbiguousOrderToasts.ContainsKey(order.OrderId))
            {
                //XQSoundManager.PlaySound(XQSoundManager.OrderStateAmbiguous);
                var audioFileName = appAssemblerService.PreferenceManager.Config.OrderAmbiguousAudioFileName;
                PlaySound(audioFileName);

                var ndpvm = orderAmbiguousNDPVMFactory.CreateExport().Value;
                var notification = new XqToastNotification(ndpvm, true);

                ndpvm.Order = order;
                ndpvm.ShowDetailCmd = new DelegateCommand((obj) =>
                {
                    notification.Close();
                    var __order = obj as OrderItemDataModel;
                    if (__order == null) return;
                    ShowAmbiguousOrder(__order);
                });
                ndpvm.CloseCmd = new DelegateCommand(() => notification.Close());
                
                orderIdKeyedAmbiguousOrderToasts.Add(order.OrderId, notification);

                AcquireTradeToastNotifierWrapper()?.Notify<XqToastNotification>(() => notification);
            }
        }

        /// <summary>
        /// 处理成交瘸腿提醒
        /// </summary>
        /// <param name="order"></param>
        private void HandleTradeLameTaskNoteNotification(XQTradeLameTaskNote lameTaskNote)
        {
            var itemKey = new XQTradeItemKey(lameTaskNote.SubAccountId, lameTaskNote.XQTradeId);
            if (!tradeLameTaskNoteToasts.ContainsKey(itemKey))
            {
                //XQSoundManager.PlaySound(XQSoundManager.ComposeLameTraded);
                var audioFileName = appAssemblerService.PreferenceManager.Config.LameTradedAudioFileName;
                PlaySound(audioFileName);

                var ndpvm = tradeLameNDPVMFactory.CreateExport().Value;
                var notification = new XqToastNotification(ndpvm, true);

                ndpvm.LameTaskNote = lameTaskNote;
                ndpvm.ShowDetailCmd = new DelegateCommand((obj) =>
                {
                    notification.Close();
                    var __tnItem = obj as XQTradeLameTaskNote;
                    if (__tnItem == null) return;
                    ShowXQTradeLameTaskNote(__tnItem);
                });
                ndpvm.CloseCmd = new DelegateCommand(() => notification.Close());

                tradeLameTaskNoteToasts.Add(itemKey, notification);

                AcquireTradeToastNotifierWrapper()?.Notify<XqToastNotification>(() => notification);
            }
        }
        
        private void RemoveOrderToastNotification(string orderId)
        {
            if (orderIdKeyedExceptionOrderToasts.TryGetValue(orderId, out XqToastNotification _toast1))
            {
                _toast1.Close();
            }

            if (orderIdKeyedTriggeredOrderToasts.TryGetValue(orderId, out XqToastNotification _toast2))
            {
                _toast2.Close();
            }

            if (orderIdKeyedAmbiguousOrderToasts.TryGetValue(orderId, out XqToastNotification _toast3))
            {
                _toast3.Close();
            }

            if (orderCancelledToasts.TryGetValue(orderId, out XqToastNotification _toast4))
            {
                _toast4.Close();
            }
        }

        private void RemoveTradeLameTaskNoteToastNotification(XQTradeItemKey itemKey)
        {
            if (tradeLameTaskNoteToasts.TryGetValue(itemKey, out XqToastNotification _toast))
            {
                _toast.Close();
            }
        }
    }
}
