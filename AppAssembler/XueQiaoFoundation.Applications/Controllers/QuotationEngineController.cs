using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using xueqiao.trade.hosting.proxy;
using xueqiao.trade.hosting.push.protocol;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;
using IDLAutoGenerated.Util;
using XueQiaoFoundation.Interfaces.Applications;
using XueQiaoFoundation.Interfaces.Event.quotation_server;
using XueQiaoFoundation.Interfaces.Event.business;
using Thrift.Protocol;
using Thrift.Transport;
using xueqiao.trade.hosting.events;
using System.Diagnostics;
using xueqiao.trade.hosting.quot.comb.thriftapi;
using business_foundation_lib.quotationpush;
using lib.xqclient_base.logger;
using ContainerShell.Interfaces.Applications;
using ContainerShell.Interfaces.DataModels;

namespace XueQiaoFoundation.Applications.Controllers
{
    [Export(typeof(IQuotationEngineController)), Export(typeof(IXueQiaoFoundationSingletonController)), PartCreationPolicy(CreationPolicy.Shared)]
    internal class QuotationEngineController : IQuotationEngineController, IXueQiaoFoundationSingletonController
    {
        private const int QuotationServerPort = 20021;

        private readonly ILoginDataService loginDataService;
        private readonly ILoginUserManageService loginUserManageService;
        private readonly IContainerShellService containerShellService;
        private readonly IEventAggregator eventAggregator;
        
        private readonly ProtocolFrameCustomMessageDispatcher userMsgDispatcher = new ProtocolFrameCustomMessageDispatcher();
        private readonly ProtocolFrameCustomMessageDispatcher seqMsgDispatcher = new ProtocolFrameCustomMessageDispatcher();

        private QuotationCSharpClient quotationClient;

        private readonly object lastReceivedSeqMsgLock = new object();
        private long? lastReceivedSeqMsgNo;
        private readonly Stopwatch lastReceivedSeqAlignMsgIntervalStopwatch = new Stopwatch();
        private const string ProtocolFrameSeqAlignMsgType = "#SEQALIGN#";
        
        [ImportingConstructor]
        public QuotationEngineController(
            ILoginDataService loginDataService,
            ILoginUserManageService loginUserManageService,
            IContainerShellService containerShellService,
            IEventAggregator eventAggregator)
        {
            this.loginDataService = loginDataService;
            this.loginUserManageService = loginUserManageService;
            this.containerShellService = containerShellService;
            this.eventAggregator = eventAggregator;

            RegisterProtocolFrameUserMsgHandlers();
            RegisterProtocolFrameSeqMsgHandlers();
            
            loginUserManageService.HasLogined += RecvUserHasLogined;
            loginUserManageService.HasLogouted += RecvUserHasLogouted;
            containerShellService.XqInitializeDataInitialized += ReceiveInitialDataInitialized;
        }
        
        public void Shutdown()
        {
            loginUserManageService.HasLogined -= RecvUserHasLogined;
            loginUserManageService.HasLogouted -= RecvUserHasLogouted;
            containerShellService.XqInitializeDataInitialized -= ReceiveInitialDataInitialized;

            lastReceivedSeqAlignMsgIntervalStopwatch.Stop();
            DisconnectQuotationServer();
        }
        
        public bool SubscribeContractQuotation(SubscribeQuotationKey subKey)
        {
            if (subKey == null) return false;
            if (CheckQuotationClientIsAvaliable(out QuotationCSharpClient qClient))
            {
                qClient.Subscribe(new SubscribeQuotationKey[] { subKey });
                return true;
            }
            return false;
        }

        public bool UnsubscribeContractQuotation(SubscribeQuotationKey subKey)
        {
            if (subKey == null) return false;
            if (CheckQuotationClientIsAvaliable(out QuotationCSharpClient qClient))
            {
                qClient.Unsubscribe(new SubscribeQuotationKey[] { subKey });
                return true;
            }
            return false;
        }

        public bool IsQuotationServerConnected
        {
            get
            {
                return quotationClient?.IsConnected() ?? false;
            }
        }
        
        private void TryConnectToQuotationServer()
        {
            var loginResp = loginDataService.ProxyLoginResp;
            if (loginResp == null || !containerShellService.IsXqInitializeDataInitalized)
            {
                AppLog.Warn("Havn't login or havn't inialized data, can't connect to quotation server.");
                return;
            }
            
            var hostingServerIP = loginResp.HostingServerIP;
            var hostingMachineId = loginResp.HostingSession.MachineId;
            var subUserId = loginResp.HostingSession.SubUserId;
            var loginToken = loginResp.HostingSession.Token;
            
            var client = new QuotationCSharpClient(hostingServerIP, QuotationServerPort, (int)hostingMachineId, subUserId, loginToken);
            this.quotationClient = client;

            client.OnConnectOpenHandler = OnQSConnectOpen;
            client.OnConnectCloseHandler = OnQSConnectClose;
            client.OnConnectErrorHandler = OnQSConnectError;
            client.OnReceiveProtocolFrame = OnQSReceiveProtocolFrame;
            client.OnSubscirbedQuotation = OnQSSubscirbedQuotation;
            client.OnUnsubscirbedQuotation = OnQSUnsubscirbedQuotation;
            
            // 客户端连接行情服务器
            try
            {
                AppLog.Info("Begin connect to Quotation sdk.");
                quotationClient?.Connect();
            }
            catch (Exception e)
            {
                AppLog.Error("Quotation sdk connect error:{0}", e);
            }
        }

        public void DisconnectQuotationServer()
        {
            var qClient = quotationClient;
            if (qClient != null)
            {
                qClient.OnConnectOpenHandler = null;
                qClient.OnConnectCloseHandler = null;
                qClient.OnConnectErrorHandler = null;
                qClient.OnReceiveProtocolFrame = null;
                qClient.OnSubscirbedQuotation = null;
                qClient.OnUnsubscirbedQuotation = null;
                qClient.Disconnect();
                ClosedQuotationServerConnect(qClient);
                qClient.Dispose();
            }
        }
        
        private void RegisterProtocolFrameUserMsgHandlers()
        {
            Type messageType = null;
            ProtocolFrameCustomMessageDelegateHandler delegateHandler = null;

            // add `CombQuotationItem` message handler
            messageType = typeof(HostingQuotationComb);
            delegateHandler = new ProtocolFrameCustomMessageDelegateHandler(messageType.Assembly.FullName, messageType.FullName,
                    msg =>
                    {
                        if (msg is HostingQuotationComb _msg)
                        {
                            AppLog.Debug($"Publish CombQuotationItem: {_msg}");
                            var q = _msg.ToNativeCombQuotation();
                            eventAggregator.GetEvent<CombQuotationUpdated>().Publish(q);
                        }
                    });
            userMsgDispatcher.AddMessageHandler("CombQuotationItem", delegateHandler);


            // add `HostingPositionFundEvent` message handler
            messageType = typeof(HostingPositionFundEvent);
            delegateHandler = new ProtocolFrameCustomMessageDelegateHandler(messageType.Assembly.FullName, messageType.FullName,
                    msg =>
                    {
                        if (msg is HostingPositionFundEvent _msg)
                        {
                            AppLog.Debug($"Publish HostingPositionFundEvent: {_msg}");
                            eventAggregator.GetEvent<HostingPositionFundChanged>().Publish(_msg);
                        }
                    });
            userMsgDispatcher.AddMessageHandler(messageType.Name, delegateHandler);


            // add `HostingFundEvent` message handler
            messageType = typeof(HostingFundEvent);
            delegateHandler = new ProtocolFrameCustomMessageDelegateHandler(messageType.Assembly.FullName, messageType.FullName,
                    msg =>
                    {
                        if (msg is HostingFundEvent _msg)
                        {
                            AppLog.Debug($"Publish HostingFundEvent: {_msg}");
                            eventAggregator.GetEvent<HostingFundChanged>().Publish(_msg);
                        }
                    });
            userMsgDispatcher.AddMessageHandler(messageType.Name, delegateHandler);


            // add `StatPositionDynamicInfoEvent`
            messageType = typeof(StatPositionDynamicInfoEvent);
            delegateHandler = new ProtocolFrameCustomMessageDelegateHandler(messageType.Assembly.FullName, messageType.FullName,
                    msg =>
                    {
                        if (msg is StatPositionDynamicInfoEvent _msg)
                        {
                            AppLog.Debug($"Publish StatPositionDynamicInfoEvent: {_msg}");
                            eventAggregator.GetEvent<XQTargetPositionDynamicInfoEvent>().Publish(_msg);
                        }
                    });
            userMsgDispatcher.AddMessageHandler(messageType.Name, delegateHandler);
        }

        private void RegisterProtocolFrameSeqMsgHandlers()
        {
            Type messageType = null;
            ProtocolFrameCustomMessageDelegateHandler delegateHandler = null;

            // add `ClientForceSyncEvent` message handler
            messageType = typeof(ClientForceSyncEvent);
            delegateHandler = new ProtocolFrameCustomMessageDelegateHandler(messageType.Assembly.FullName, messageType.FullName,
                msg =>
                {
                    if (msg is ClientForceSyncEvent _msg)
                    {
                        AppLog.Info($"Publish ClientForceSyncEvent: {_msg}");
                        eventAggregator.GetEvent<ClientDataForceSync>().Publish(_msg);
                    }
                });
            seqMsgDispatcher.AddMessageHandler(messageType.Name, delegateHandler);

            // add `XQOrderCreated` message handler
            messageType = typeof(XQOrderCreatedEvent);
            delegateHandler = new ProtocolFrameCustomMessageDelegateHandler(messageType.Assembly.FullName, messageType.FullName,
                msg =>
                {
                    if (msg is XQOrderCreatedEvent _msg)
                    {
                        AppLog.Info($"Publish XQOrderCreatedEvent: {_msg.CreatedOrder?.LogFormatHostingXQOrder()}");
                        eventAggregator.GetEvent<XQOrderCreated>().Publish(_msg);
                    }
                });
            seqMsgDispatcher.AddMessageHandler(messageType.Name, delegateHandler);

            // add `XQOrderChanged` message handler
            messageType = typeof(XQOrderChangedEvent);
            delegateHandler = new ProtocolFrameCustomMessageDelegateHandler(messageType.Assembly.FullName, messageType.FullName,
                msg =>
                {
                    if (msg is XQOrderChangedEvent _msg)
                    {
                        AppLog.Info($"Publish XQOrderChangedEvent: {_msg.ChangedOrder?.LogFormatHostingXQOrder()}");
                        eventAggregator.GetEvent<XQOrderChanged>().Publish(_msg);
                    }
                });
            seqMsgDispatcher.AddMessageHandler(messageType.Name, delegateHandler);

            // add `XQOrderExpiredEvent` message handler
            messageType = typeof(XQOrderExpiredEvent);
            delegateHandler = new ProtocolFrameCustomMessageDelegateHandler(messageType.Assembly.FullName, messageType.FullName,
                msg =>
                {
                    if (msg is XQOrderExpiredEvent _msg)
                    {
                        AppLog.Info($"Publish XQOrderExpiredEvent, OrderId: {_msg.OrderId}");
                        eventAggregator.GetEvent<XQOrderExpired>().Publish(_msg);
                    }
                });
            seqMsgDispatcher.AddMessageHandler(messageType.Name, delegateHandler);

            // add `XQTradeListChangedEvent` message handler
            messageType = typeof(XQTradeListChangedEvent);
            delegateHandler = new ProtocolFrameCustomMessageDelegateHandler(messageType.Assembly.FullName, messageType.FullName,
                msg =>
                {
                    if (msg is XQTradeListChangedEvent _msg)
                    {
                        AppLog.Info($"Publish XQTradeListChangedEvent, order:{_msg.Order?.LogFormatHostingXQOrder()}");
                        foreach (var tradedItem in _msg.TradeList)
                        {
                            AppLog.Info(tradedItem.LogFormatHostingXQTrade());
                        }
                        eventAggregator.GetEvent<XQTradeListChanged>().Publish(_msg);
                    }
                });
            seqMsgDispatcher.AddMessageHandler(messageType.Name, delegateHandler);


            // add `HostingPositionVolumeEvent` message handler
            messageType = typeof(HostingPositionVolumeEvent);
            delegateHandler = new ProtocolFrameCustomMessageDelegateHandler(messageType.Assembly.FullName, messageType.FullName,
                msg =>
                {
                    if (msg is HostingPositionVolumeEvent _msg)
                    {
                        AppLog.Info($"Publish HostingPositionVolumeEvent, event msg:{_msg}");
                        eventAggregator.GetEvent<HostingPositionVolumeChanged>().Publish(_msg);
                    }
                });
            seqMsgDispatcher.AddMessageHandler(messageType.Name, delegateHandler);

            // add `StatPositionSummaryChangedEvent` message handler
            messageType = typeof(StatPositionSummaryChangedEvent);
            delegateHandler = new ProtocolFrameCustomMessageDelegateHandler(messageType.Assembly.FullName, messageType.FullName,
                msg =>
                {
                    if (msg is StatPositionSummaryChangedEvent _msg)
                    {
                        AppLog.Info($"Publish StatPositionSummaryChangedEvent, event msg:{_msg}");
                        eventAggregator.GetEvent<XQTargetPositionSummaryChanged>().Publish(_msg);
                    }
                });
            seqMsgDispatcher.AddMessageHandler(messageType.Name, delegateHandler);


            // add `TaskNoteCreatedEvent` message handler
            messageType = typeof(TaskNoteCreatedEvent);
            delegateHandler = new ProtocolFrameCustomMessageDelegateHandler(messageType.Assembly.FullName, messageType.FullName, 
                msg => 
                {
                    if (msg is TaskNoteCreatedEvent _msg)
                    {
                        AppLog.Info($"Publish TaskNoteCreatedEvent, event msg:{_msg}");
                        eventAggregator.GetEvent<XQTaskNoteCreated>().Publish(_msg);
                    }
                });
            seqMsgDispatcher.AddMessageHandler(messageType.Name, delegateHandler);


            // add `TaskNoteDeletedEvent` message handler
            messageType = typeof(TaskNoteDeletedEvent);
            delegateHandler = new ProtocolFrameCustomMessageDelegateHandler(messageType.Assembly.FullName, messageType.FullName,
                msg =>
                {
                    if (msg is TaskNoteDeletedEvent _msg)
                    {
                        AppLog.Info($"Publish TaskNoteDeletedEvent, event msg:{_msg}");
                        eventAggregator.GetEvent<XQTaskNoteDeleted>().Publish(_msg);
                    }
                });
            seqMsgDispatcher.AddMessageHandler(messageType.Name, delegateHandler);
        }

        private bool CheckQuotationClientIsAvaliable(out QuotationCSharpClient _quotationClient)
        {
            _quotationClient = this.quotationClient;

            if (_quotationClient == null) return false;
            if (loginDataService.ProxyLoginResp == null) return false;
            return _quotationClient.IsConnected();
        }

        private void ClosedQuotationServerConnect(QuotationCSharpClient qClient)
        {
            if (qClient?.IsConnected() == false)
            {
                AppLog.Info("Quotation server closed. Publish ServerConnectClose event.");
                lock (lastReceivedSeqMsgLock)
                {
                    lastReceivedSeqMsgNo = null;
                    lastReceivedSeqAlignMsgIntervalStopwatch.Reset();
                }
                eventAggregator.GetEvent<ServerConnectClose>().Publish();
            }
        }
        
        private void RecvUserHasLogined()
        {
            TryConnectToQuotationServer();
        }
        
        private void ReceiveInitialDataInitialized(InitializeDataRoot initializedData)
        {
            TryConnectToQuotationServer();
        }
        
        private void RecvUserHasLogouted(ProxyLoginResp lastLoginResp)
        {
            DisconnectQuotationServer();
        }

        private void OnQSConnectOpen(QuotationCSharpClient client, bool success)
        {
            AppLog.Info($"Quotation server on open. success: {success}, host:{client.HostingServerIP}, port:{client.HostingProxyPort}");
            var msg = new ServerConnectOpenEventMsg
            {
                IsOpened = success,
                ConnectInfo = new ServerConnectInfo
                {
                    ConnectHost = client.HostingServerIP,
                    ConnectPort = client.HostingProxyPort
                }
            };
            eventAggregator.GetEvent<ServerConnectOpen>().Publish(msg);
        }

        private void OnQSConnectClose(QuotationCSharpClient client, string closeReason)
        {
            ClosedQuotationServerConnect(this.quotationClient);
        }

        private void OnQSConnectError(QuotationCSharpClient client, Exception e)
        {
            AppLog.Info($"Quotation server on error:{e}");
        }

        private void OnQSSubscirbedQuotation(QuotationCSharpClient client, SubscribeQuotationKey subKey, bool subscribeSuccess)
        {
            AppLog.Info($"Subscribed quotation key:{subKey}, isSuccess:{subscribeSuccess}");
            eventAggregator.GetEvent<SpecQuotationSubscribedEvent>().Publish(new SpecQuotationSubscribedEventArgs(subKey, subscribeSuccess));
        }

        private void OnQSUnsubscirbedQuotation(QuotationCSharpClient client, SubscribeQuotationKey subKey, bool unsubscribeSuccess)
        {
            AppLog.Info($"Unsubscribed quotation key:{subKey}, isSuccess:{unsubscribeSuccess}");
            eventAggregator.GetEvent<SpecQuotationUnsubscribedEvent>().Publish(new SpecQuotationUnsubscribedEventArgs(subKey, unsubscribeSuccess));
        }

        private void OnQSReceiveProtocolFrame(QuotationCSharpClient client, ProtocolFrame frame)
        {
            if (frame == null) return;
            if (frame.Protocol == ProtocolType.RESP)
            {
                RespContent respContent = frame.RespContent;
                var msg = $"Quotation receive RESP Frame, responseID({respContent.ResponseId}) for requestId({respContent.RequestId}), errorCode: {respContent.ErrCode}";
                AppLog.Debug(msg);
            }
            else if (frame.Protocol == ProtocolType.QUOTATION && frame.QuotationItem != null)
            {
                AppLog.Debug($"Receive quotation Frame: {frame.QuotationItem}");
                var q = frame.QuotationItem.ToNativeQuotation();
                eventAggregator.GetEvent<SpecQuotationUpdated>().Publish(q);
            }
            else if (frame.Protocol == ProtocolType.USERMSG)
            {
                OnReceiveProtocolFrameUserMsg(frame.UserMsg);
            }
            else if (frame.Protocol == ProtocolType.SEQMSG)
            {
                OnReceiveProtocolFrameSeqMsg(frame.SeqMsg);
            }
            else
            {
                AppLog.Debug($"receive ProtocolFrame: {frame}");
            }
        }

        private void OnReceiveProtocolFrameUserMsg(UserMsgContent userMsg)
        {
            if (userMsg == null) return;
            AppLog.Debug($"Receive UserMsg Frame: {userMsg}");
            userMsgDispatcher.Dispatcher(userMsg.MsgType, userMsg.MsgBytes);
        }

        
        private void OnReceiveProtocolFrameSeqMsg(SeqMsgContent seqMsg)
        {
            if (seqMsg == null) return;
            AppLog.Info($"Receive SeqMsg Frame: {seqMsg}");

            bool needForceSyncData = false;
            bool hasReconnectedQuotationServer = false;
            lock (lastReceivedSeqMsgLock)
            {
                if (seqMsg.MsgType == ProtocolFrameSeqAlignMsgType)
                {
                    this.lastReceivedSeqAlignMsgIntervalStopwatch.Stop();
                    var lastReceivedSeqAlignMsgInterval = this.lastReceivedSeqAlignMsgIntervalStopwatch.Elapsed.TotalSeconds;
                    AppLog.Info($"lastReceivedSeqAlignMsgInterval:{lastReceivedSeqAlignMsgInterval}");
                    if (lastReceivedSeqAlignMsgInterval > 30)
                    {
                        AppLog.Info($"The interval between current received seq align msg and last is over 30 seconds, disconnect and reconnect to quotation server.");

                        this.lastReceivedSeqAlignMsgIntervalStopwatch.Reset();
                        this.lastReceivedSeqMsgNo = null;
                        hasReconnectedQuotationServer = true;

                        DisconnectQuotationServer();
                        TryConnectToQuotationServer();
                    }
                    else
                    {
                        this.lastReceivedSeqAlignMsgIntervalStopwatch.Restart();
                    }
                }

                if (!hasReconnectedQuotationServer)
                {
                    var lastSeqNo = this.lastReceivedSeqMsgNo.GetValueOrDefault(-1);
                    if (lastSeqNo >= 0 && seqMsg.SeqNo != (lastSeqNo + 1))
                    {
                        AppLog.Info($"Last seq msg No ({lastSeqNo}), Not equals to current received seq mgs No({seqMsg.SeqNo}) -1. So need to force sync data.");
                        // 序列数据不一致，强制数据同步
                        needForceSyncData = true;
                    }
                    this.lastReceivedSeqMsgNo = seqMsg.SeqNo;
                }
            }

            if (hasReconnectedQuotationServer) return;

            if (needForceSyncData)
            {
                // 序列数据不一致，强制数据同步
                var loginSubUserId = loginDataService.ProxyLoginResp?.HostingSession?.SubUserId;
                if (loginSubUserId.HasValue)
                {
                    AppLog.Info($"Publish ClientForceSync Event. subUserId:{loginSubUserId.Value}");
                    eventAggregator.GetEvent<ClientDataForceSync>().Publish(new ClientForceSyncEvent(loginSubUserId.Value));
                }
                return;
            }

            // 序列数据一致，进行正常逻辑
            if (seqMsg.MsgType != ProtocolFrameSeqAlignMsgType)
            {
                seqMsgDispatcher.Dispatcher(seqMsg.MsgType, seqMsg.MsgBytes);
            }
        }


        internal interface IProtocolFrameCustomMessageHandler
        {
            string MessageTypeAssemblyName { get; }

            string MessageTypeFullName { get; }

            void HandleCustomMessage(TBase customMessage);
        }
        
        internal class ProtocolFrameCustomMessageDispatcher
        {
            private Dictionary<string, IProtocolFrameCustomMessageHandler> messageHandlers;

            public ProtocolFrameCustomMessageDispatcher()
            {
                messageHandlers = new Dictionary<string, IProtocolFrameCustomMessageHandler>();
            }

            public void AddMessageHandler(string messageName, IProtocolFrameCustomMessageHandler handler)
            {
                messageHandlers[messageName] = handler;
            }

            public void RemoveMessageHandler(string messageName)
            {
                messageHandlers.Remove(messageName);
            }

            public void Dispatcher(string messageName, byte[] userMessageBytes)
            {
                if (string.IsNullOrWhiteSpace(messageName)) return;
                if (messageHandlers.TryGetValue(messageName, out IProtocolFrameCustomMessageHandler handler))
                {
                    TBase messageObj = null;
                    try
                    {
                        messageObj = CreateTBaseMessage(handler.MessageTypeAssemblyName, handler.MessageTypeFullName, userMessageBytes);
                    }
                    catch (Exception e)
                    {
                        AppLog.Error($"CreateTBaseMessage failed:{e}");
                    }

                    if (messageObj != null)
                    {
                        handler.HandleCustomMessage(messageObj);
                    }
                }
                else
                {
                    AppLog.Warn($"Not found handler for message:{messageName}, it is ignored.");
                }
            }

            private TBase CreateTBaseMessage(string messageTypeAssemblyName, string messageTypeFullName,  byte[] userMessageBytes)
            {
                if (userMessageBytes == null) return default(TBase);
                var stream = new MemoryStream(userMessageBytes);
                TCompactProtocol tProtocol = new TCompactProtocol(new TStreamTransport(stream, stream));
                var tbaseObject = Activator.CreateInstance(messageTypeAssemblyName, messageTypeFullName).Unwrap() as TBase;
                tbaseObject?.Read(tProtocol);
                return tbaseObject;
            }
        }

        internal class ProtocolFrameCustomMessageDelegateHandler : IProtocolFrameCustomMessageHandler
        {
            private readonly string messageTypeAssemblyName;
            private readonly string messageTypeFullName;

            public ProtocolFrameCustomMessageDelegateHandler(string messageTypeAssemblyName,
                string messageTypeFullName,
                Action<TBase> customMessageHandler)
            {
                this.messageTypeAssemblyName = messageTypeAssemblyName;
                this.messageTypeFullName = messageTypeFullName;
                this.CustomMessageHandler = customMessageHandler;
            }

            public string MessageTypeAssemblyName => messageTypeAssemblyName;

            public string MessageTypeFullName => messageTypeFullName;

            public readonly Action<TBase> CustomMessageHandler;

            public void HandleCustomMessage(TBase customMessage)
            {
                CustomMessageHandler?.Invoke(customMessage);
            }
        }
    }
}