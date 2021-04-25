using System;
using System.Collections.Generic;
using WebSocketSharp;
using Thrift.Transport;
using Thrift.Protocol;
using System.Text;
using System.IO;
using System.Linq;
using xueqiao.trade.hosting.push.protocol;
using System.Threading.Tasks;
using System.Threading.Tasks.Schedulers;
using System.Timers;
using System.Threading;
using lib.xqclient_base.logger;

namespace business_foundation_lib.quotationpush
{
    // 启动连接回调
    public delegate void OnConnectOpenHandler(QuotationCSharpClient client, bool success);

    public delegate void OnConnectCloseHandler(QuotationCSharpClient client, string closeReason);

    public delegate void OnConnectErrorHandler(QuotationCSharpClient client, Exception e);

    public delegate void OnReceiveProtocolFrame(QuotationCSharpClient client, ProtocolFrame frame);

    // 订阅回调
    public delegate void OnSubscirbedQuotation(QuotationCSharpClient client, SubscribeQuotationKey subKey, bool subscribeSuccess);

    // 退订回调
    public delegate void OnUnsubscirbedQuotation(QuotationCSharpClient client, SubscribeQuotationKey subKey, bool unsubscribeSuccess);

    public class QuotationCSharpClient : IDisposable
    {
        internal class SubscribeTopicReqKey : Tuple<string, string, long>
        {
            public SubscribeTopicReqKey(string platform, string symbol, long reqId) : base(platform, symbol, reqId)
            {
                this.Platform = platform;
                this.Symbol = symbol;
                this.ReqId = reqId;
            }

            public string Platform { get; private set; }
            public string Symbol { get; private set; }
            public long ReqId { get; private set; }
        }

        // 默认的心跳时间
        private const int DEFAULT_HEARTBEAT_INTERVAL = 30 * 1000;
        // 默认订阅最大重试次数
        private const int DEFAULT_SUBSCIRBE_MAX_RETRY_TIMES = 3;
        // 订阅响应超时
        private const int DEFAULT_SUBSCRIBE_CHECK_INTERVAL = 5000;

        // 连接请求最长响应时间
        private const int CONNECT_RESPONSE_MAX_MILLIS = 5000;

        // 重连最大重试次数
        private const int CONNECT_RETRY_MAX_COUNT = 500;

        private static readonly string HEARTBEAT_REQ_TYPE = typeof(HeartBeatReq).Name;
        private static readonly string SUBSCRIBE_REQ_TYPE = typeof(QuotationSubscribeReq).Name;
        private static readonly string UNSUBSCRIBE_REQ_TYPE = typeof(QuotationUnSubscribeReq).Name;
        private static Encoding EncodingUTF8 = Encoding.UTF8;

        private string mLogInvoker;
        private WebSocket mWebSocket;
        private bool isInternalConnecting;  // 是否在连接中
        
        // 当前合约订阅进程列表
        private Dictionary<SubscribeTopicReqKey, SubscribeOrNotProcess> subscribeProcesses = new Dictionary<SubscribeTopicReqKey, SubscribeOrNotProcess>();
        // 当前合约退订进程列表
        private Dictionary<SubscribeTopicReqKey, SubscribeOrNotProcess> unsubscribeProcesses = new Dictionary<SubscribeTopicReqKey, SubscribeOrNotProcess>();
        // 当前订阅的合约列表
        private List<SubscribeQuotationKey> subscribeKeyList = new List<SubscribeQuotationKey>();

        // 订阅超时检测Timer
        private System.Timers.Timer mSubscribeCheckTimer;

        // 心跳 Timer
        private System.Timers.Timer mHeartbeatTimer;
        // 上一次心跳时间戳
        private long? lastHeartBeatTimeStampMs;

        private TaskFactory orderedTaskFactory = new TaskFactory(new OrderedTaskScheduler());
        private System.Threading.CancellationTokenSource orderedTaskCancelTokenSource;
        private readonly object orderedTasksLock = new object();

        private Dictionary<long, string> protocolFrameReqTypeStructTypesKeyedByRequestId = new Dictionary<long, string>();

        
        public QuotationCSharpClient(string hostingServerIP, Int32 hostingProxyPort,
            Int32 machineId,
            Int32 subUserId,
            string token)
        {
            mLogInvoker = "QCC_" + GetHashCode();

            this.HostingServerIP = hostingServerIP;
            this.HostingProxyPort = hostingProxyPort;
            this.MachineId = machineId;
            this.SubUserId = subUserId;
            this.Token = token;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            lock (orderedTasksLock)
            {
                if (!this.disposed)
                {
                    if (disposing)
                    {
                        // Dispose managed resources.
                        orderedTaskCancelTokenSource?.Cancel();
                        orderedTaskCancelTokenSource?.Dispose();

                        OnConnectOpenHandler = null;
                        OnConnectCloseHandler = null;
                        OnConnectErrorHandler = null;
                        OnReceiveProtocolFrame = null;
                        OnSubscirbedQuotation = null;
                        OnUnsubscirbedQuotation = null;
                        InternalDisconnect(false);
                    }
                    // Call the appropriate methods to clean up
                    // unmanaged resources here.

                    // Note disposing has been done.
                    disposed = true;
                }
            }
        }

        ~ QuotationCSharpClient()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }

        private void EnqueueOrderedTask(Action<System.Threading.CancellationToken> task)
        {
            if (disposed) return;
            lock (orderedTasksLock)
            {
                if (orderedTaskCancelTokenSource == null)
                {
                    orderedTaskCancelTokenSource = new System.Threading.CancellationTokenSource();
                }
                var cancelToken = orderedTaskCancelTokenSource.Token;
                orderedTaskFactory.StartNew(() =>
                {
                    if (cancelToken.IsCancellationRequested) return;
                    task?.Invoke(cancelToken);
                });
            }
        }

        public readonly string HostingServerIP;
        public readonly int HostingProxyPort;
        public readonly int MachineId;
        public readonly int SubUserId;
        public readonly string Token;

        // 订阅失败后的重试次数
        public int SubscribeMaxRetryTimes { get; set; } = DEFAULT_SUBSCIRBE_MAX_RETRY_TIMES;

        // 退订失败后的重试次数
        public int UnsubscribeMaxRetryTimes { get; set; } = DEFAULT_SUBSCIRBE_MAX_RETRY_TIMES;

        // 心跳间隔
        public int HeartBeatInterval { get; set; } = DEFAULT_HEARTBEAT_INTERVAL;
        
        public OnConnectOpenHandler OnConnectOpenHandler { get; set; }

        public OnConnectCloseHandler OnConnectCloseHandler { get;set;}
        
        public OnConnectErrorHandler OnConnectErrorHandler { get; set; }

        public OnReceiveProtocolFrame OnReceiveProtocolFrame { get; set; }

        public OnSubscirbedQuotation OnSubscirbedQuotation { get; set; }

        public OnUnsubscirbedQuotation OnUnsubscirbedQuotation { get; set; }

        public void Connect()
        {
            if (disposed) return;
            AppLog.Debug($"QuotationClient, Request connect.");
            InternalRequestConnect();
        }

        public void Disconnect()
        {
            if (disposed) return;
            AppLog.Debug($"QuotationClient, Request disconnect.");
            InternalDisconnect(true);
        }

        // 当前连接是否正常
        public bool IsConnected()
        {
            var _webSokect = mWebSocket;
            return _webSokect != null && _webSokect.ReadyState == WebSocketState.Open;
        }

        public void Subscribe(IEnumerable<SubscribeQuotationKey> subscribeKeys)
        {
            if (subscribeKeys == null) return;
            EnqueueOrderedTask(cancelToken =>
            {
                if (cancelToken.IsCancellationRequested) return;

                var currentUnsubscibeReqKeys = unsubscribeProcesses.Keys.ToArray();
                foreach (var subscribeKey in subscribeKeys)
                {
                    var tarUnsubKeys = currentUnsubscibeReqKeys.Where(i => i.Platform == subscribeKey.Platform && i.Symbol == subscribeKey.ContractSymbol).ToArray();
                    foreach (var tarUnsubKey in tarUnsubKeys)
                    {
                        unsubscribeProcesses.Remove(tarUnsubKey);
                    }

                    if (!subscribeKeyList.Any(i => i.Platform == subscribeKey.Platform && i.ContractSymbol == subscribeKey.ContractSymbol))
                    {
                        subscribeKeyList.Add(subscribeKey);
                    }

                    if (!subscribeProcesses.Keys.ToArray().Any(i => i.Platform == subscribeKey.Platform && i.Symbol == subscribeKey.ContractSymbol))
                    {
                        InternalSubscribeOneTopic(subscribeKey);
                    }
                }
            });
        }

        public void Unsubscribe(IEnumerable<SubscribeQuotationKey> unsubscribeKeys)
        {
            if (unsubscribeKeys == null) return;
            EnqueueOrderedTask(cancelToken =>
            {
                if (cancelToken.IsCancellationRequested) return;

                var currentSubscibeReqKeys = subscribeProcesses.Keys.ToArray();
                foreach (var unsubscribeKey in unsubscribeKeys)
                {
                    var tarSubKeys = currentSubscibeReqKeys.Where(i => i.Platform == unsubscribeKey.Platform && i.Symbol == unsubscribeKey.ContractSymbol).ToArray();
                    foreach (var tarSubKey in tarSubKeys)
                    {
                        subscribeProcesses.Remove(tarSubKey);
                    }

                    subscribeKeyList.RemoveAll(i => i.Platform == unsubscribeKey.Platform && i.ContractSymbol == unsubscribeKey.ContractSymbol);

                    if (!unsubscribeProcesses.Keys.ToArray().Any(i => i.Platform == unsubscribeKey.Platform && i.Symbol == unsubscribeKey.ContractSymbol))
                    {
                        InternalUnsubscribeOneTopic(unsubscribeKey);
                    }
                }
            });
        }

        private void OnWebsocketOpen(object sender, EventArgs e)
        {
            if (sender != mWebSocket) return;
            EnqueueOrderedTask(cancelToken =>
            {
                if (cancelToken.IsCancellationRequested) return;

                AppLog.Debug($"QuotationClient, OnWebsocketOpen.");

                // 连接成功建立，记录时间戳，开启心跳Timer和订阅超时Timer
                lastHeartBeatTimeStampMs = null;
                StopHeartbeatTimer();
                StartHeartbeatTimerIfNeed();
                StopSubsribeCheckTimer();
                StartSubsribeCheckTimerIfNeed();

                // 恢复订阅合约
                var _subscribeTopics = subscribeKeyList.ToArray();
                if (_subscribeTopics.Any())
                {
                    AppLog.Debug($"QuotationClient, Resume subscribe topics.");
                    foreach (var topic in _subscribeTopics)
                    {
                        InternalSubscribeOneTopic(topic);
                    }
                }

                OnConnectOpenHandler?.Invoke(this, true);
            });
        }

        private void OnWebsocketClose(object sender, CloseEventArgs args)
        {
            if (sender != mWebSocket) return;
            EnqueueOrderedTask(cancelToken =>
            {
                if (cancelToken.IsCancellationRequested) return;
                InternalDisconnect(true);
                InternalRequestConnect();
            });
        }

        private void OnWebsocketError(object sender, WebSocketSharp.ErrorEventArgs args)
        {
            if (sender != mWebSocket) return;
            EnqueueOrderedTask(cancelToken =>
            {
                if (cancelToken.IsCancellationRequested) return;
                OnConnectErrorHandler?.Invoke(this, args.Exception);
            });
        }

        private void OnWebsocketMessage(object sender, MessageEventArgs args)
        {
            if (sender != mWebSocket) return;

            TBase tbase = new ProtocolFrame();
            UnserializeBytesToTBase(ref tbase, args.RawData);
            ProtocolFrame frame = (ProtocolFrame)tbase;

            // 请求响应帧
            if (frame.Protocol == ProtocolType.RESP)
            {
                EnqueueOrderedTask(cancelToken =>
                {
                    if (cancelToken.IsCancellationRequested) return;

                    RespContent respContent = frame.RespContent;
                    long timestamp = LinuxTimestampMaker.currentTimeMillis();

                    AppLog.Debug($"QuotationClient, receive RESP frame: " + respContent.ToString());
                    if (protocolFrameReqTypeStructTypesKeyedByRequestId.TryGetValue(respContent.RequestId, out string reqStructType))
                    {
                        if (reqStructType == HEARTBEAT_REQ_TYPE)
                        {
                            OnHeartbeatResp(respContent);
                        }
                        else if (reqStructType == SUBSCRIBE_REQ_TYPE)
                        {
                            OnSubscribeResp(respContent);
                        }
                        else if (reqStructType == UNSUBSCRIBE_REQ_TYPE)
                        {
                            OnUnsubscribeResp(respContent);
                        }

                        protocolFrameReqTypeStructTypesKeyedByRequestId.Remove(respContent.RequestId);
                    }
                });
            }
            // 行情帧，用户数据帧
            else
            {
                OnReceiveProtocolFrame?.Invoke(this, frame);
            }
        }

        private void OnSubscribeResp(RespContent resp)
        {
            long requestID = resp.RequestId;
            var tarKey = subscribeProcesses.Keys.FirstOrDefault(i => i.ReqId == requestID);
            if (tarKey == null) return;

            SubscribeOrNotProcess subProcess = null;
            if (!subscribeProcesses.TryGetValue(tarKey, out subProcess)) return;

            if (resp.ErrCode == 0)
            {
                AppLog.Debug($"QuotationClient, Finish subscribe within success state:{subProcess}");
                subscribeProcesses.Remove(tarKey);
                OnSubscirbedQuotation?.Invoke(this, subProcess.TargetSubscribeKey, true);
            }
            else
            {
                RetryOrCancelSubscribeProcessWithKey(tarKey, null);
            }
        }

        private void OnUnsubscribeResp(RespContent resp)
        {
            long requestID = resp.RequestId;
            var tarKey = unsubscribeProcesses.Keys.FirstOrDefault(i => i.ReqId == requestID);
            if (tarKey == null) return;

            SubscribeOrNotProcess unsubProcess = null;
            if (!unsubscribeProcesses.TryGetValue(tarKey, out unsubProcess)) return;

            if (resp.ErrCode == 0)
            {
                AppLog.Debug($"QuotationClient, Finish unsubscribe within success state:{unsubProcess}");
                unsubscribeProcesses.Remove(tarKey);
                OnUnsubscirbedQuotation?.Invoke(this, unsubProcess.TargetSubscribeKey, true);
            }
            else
            {
                RetryOrCancelUnsubscribeProcessWithKey(tarKey, null);
            }
        }

        private void OnHeartbeatResp(RespContent resp)
        {
            long requestID = resp.RequestId;
            var currentTimeMs = LinuxTimestampMaker.currentTimeMillis();
            var lastHeartBeatTimeMs = lastHeartBeatTimeStampMs?? currentTimeMs;

            long interval = currentTimeMs - lastHeartBeatTimeMs;
            AppLog.Info($"QuotationClient HEARTBEAT, receive Heartbeat response in {interval} ms");

            // 心跳超时
            if (interval > CONNECT_RESPONSE_MAX_MILLIS)
            {
                AppLog.Info($"QuotationClient HEARTBEAT, receive heartbeat response too late");
                Reconnect();
            }
            // 心跳成功
            else
            {
                lastHeartBeatTimeStampMs = currentTimeMs;
            }
        }

        private void SubscribeCheckTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            EnqueueOrderedTask(cancelToken =>
            {
                if (cancelToken.IsCancellationRequested) return;

                AppLog.Debug("QuotationClient, SubscribeCheckTimer_Elapsed.");
                var subProcesses = subscribeProcesses.ToArray();
                if (subProcesses.Any())
                {
                    foreach (var subItem in subProcesses)
                    {
                        var itemKey = subItem.Key;
                        var itemValue = subItem.Value;
                        long currentTimestampMs = LinuxTimestampMaker.currentTimeMillis();
                        if ((currentTimestampMs - itemValue.LastReqTimeStampMs) > DEFAULT_SUBSCRIBE_CHECK_INTERVAL / 2)
                        {
                            RetryOrCancelSubscribeProcessWithKey(itemKey, currentTimestampMs);
                        }
                    }
                }

                var unsubProcesses = unsubscribeProcesses.ToArray();
                if (unsubProcesses.Any())
                {
                    foreach (var unsubItem in unsubProcesses)
                    {
                        var itemKey = unsubItem.Key;
                        var itemValue = unsubItem.Value;
                        long currentTimestampMs = LinuxTimestampMaker.currentTimeMillis();
                        if ((currentTimestampMs - itemValue.LastReqTimeStampMs) > DEFAULT_SUBSCRIBE_CHECK_INTERVAL / 2)
                        {
                            RetryOrCancelUnsubscribeProcessWithKey(itemKey, currentTimestampMs);
                        }
                    }
                }
            });
        }

        private void HeartbeatTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            EnqueueOrderedTask(cancelToken =>
            {
                if (cancelToken.IsCancellationRequested) return;

                if (!IsConnected())
                {
                    // 心跳时发现连接断开，重连
                    AppLog.Debug("QuotationClient, lost connect in heartbeat");
                    Reconnect();
                }
                else
                {
                    long currentTimeMs = LinuxTimestampMaker.currentTimeMillis();
                    var lastHeartBeatTimeMs = lastHeartBeatTimeStampMs ?? currentTimeMs;

                    // 本次心跳距上次心跳超过间隔的2倍，发起重连
                    long interval = currentTimeMs - lastHeartBeatTimeMs;
                    AppLog.Debug("QuotationClient, interval between last two heartbeat is " + interval + " ms");

                    int maxInterval = HeartBeatInterval * 2;
                    if (interval > maxInterval)
                    {
                        AppLog.Debug("QuotationClient, interval between last two heartbeat longer than HeartBeatInterval * 2 = " + maxInterval);
                        Reconnect();
                        return;
                    }
                    // 本次心跳正常，记录时间戳
                    else
                    {
                        lastHeartBeatTimeStampMs = currentTimeMs;
                        HeartBeatReq req = new HeartBeatReq(MachineId, SubUserId, Token);
                        var reqId = IDMaker.nextID();
                        
                        AppLog.Info($"QuotationClient HEARTBEAT, heartbeating");
                        var reqContent = new ReqContent();
                        reqContent.RequestId = reqId;
                        reqContent.RequestStructType = HEARTBEAT_REQ_TYPE;
                        reqContent.RequestStructBytes = SerializeTBaseToBytes(req);
                        SendReqTypeProtocolFrame(reqContent);
                    }
                }
            });
        }

        private void InternalDisconnect(bool notifyConnectCloseEvent)
        {
            isInternalConnecting = false;
            lastHeartBeatTimeStampMs = null;
            StopHeartbeatTimer();
            StopSubsribeCheckTimer();

            var socket = mWebSocket;
            if (socket != null)
            {
                socket.OnOpen -= OnWebsocketOpen;
                socket.OnClose -= OnWebsocketClose;
                socket.OnError -= OnWebsocketError;
                socket.OnMessage -= OnWebsocketMessage;
                if (socket.ReadyState != WebSocketState.Connecting)
                {
                    // 根据 websocket的代码可知，当 Close、Connect 方法使用了socket状态的互斥锁，当 Close 等待 Connect 且Connect由于网络问题迟迟不能建立连接时时，会导致 Close 时间很长。
                    // 所以当不在 connecting 时进行关闭，才不会卡住
                    socket.Close();
                }
            }

            if (notifyConnectCloseEvent)
            {
                OnConnectCloseHandler?.Invoke(this, null);
            }
        }

        private void InternalRequestConnect()
        {
            var serverIP = this.HostingServerIP;
            var serverPort = this.HostingProxyPort;

            if (this.isInternalConnecting) return;

            var url = $"ws://{serverIP}:{serverPort}/connect?machineId={MachineId}&subUserId={SubUserId}&token={Token}";
            var socket = new WebSocket(url);
            socket.OnOpen += OnWebsocketOpen;
            socket.OnClose += OnWebsocketClose;
            socket.OnError += OnWebsocketError;
            socket.OnMessage += OnWebsocketMessage;
            this.mWebSocket = socket;

            // 失败重试机制
            this.isInternalConnecting = true;
            Task.Run(() => 
            {
                if (!this.isInternalConnecting) return;
                
                AppLog.Debug($"QuotationClient, trying to connect, url:{url}");

                var retryNumIdx = 0;
                while (this.isInternalConnecting && socket.ReadyState != WebSocketState.Open)
                {
                    AppLog.Debug($"QuotationClient, connect retryNumIdx:{retryNumIdx}");
                    try
                    {
                        socket.Connect();
                        if (this.isInternalConnecting == false || disposed)
                        {
                            InternalDisconnect(false);
                            AppLog.Debug($"QuotationClient is disposed or requested disconnect. close it though it is connected.");
                            break;
                        }
                        else
                        {
                            var readyState = socket.ReadyState;
                            if (readyState == WebSocketState.Open)
                            {
                                AppLog.Debug($"QuotationClient, connect success");
                                this.isInternalConnecting = false;
                                break;
                            }
                            AppLog.Debug($"QuotationClient connect failed, socket.ReadyState:{readyState}, next time will retry connect.");
                        }
                    }
                    catch (Exception e)
                    {
                        AppLog.Debug($"QuotationClient connect failed, next time will retry connect. e:{e.ToString()}");
                    }

                    retryNumIdx++;
                    Thread.Sleep(1000);
                }
            });
        }

        private void InternalSubscribeOneTopic(SubscribeQuotationKey subKey)
        {
            if (subKey == null) return;
            var addKey = new SubscribeTopicReqKey(subKey.Platform, subKey.ContractSymbol, IDMaker.nextID());
            var addSubProcess = new SubscribeOrNotProcess(addKey.ReqId, subKey);
            addSubProcess.LastReqTimeStampMs = LinuxTimestampMaker.currentTimeMillis();

            AppLog.Info($"QuotationClient, subscribe {subKey.ToString()}");
            subscribeProcesses[addKey] = addSubProcess;
            SendSubscribeRequest(addSubProcess);
        }

        private void InternalUnsubscribeOneTopic(SubscribeQuotationKey subKey)
        {
            if (subKey == null) return;
            var addKey = new SubscribeTopicReqKey(subKey.Platform, subKey.ContractSymbol, IDMaker.nextID());
            var addUnsubProcess = new SubscribeOrNotProcess(addKey.ReqId, subKey);
            addUnsubProcess.LastReqTimeStampMs = LinuxTimestampMaker.currentTimeMillis();
            
            AppLog.Info($"QuotationClient, unsubscribe {subKey.ToString()}");
            unsubscribeProcesses[addKey] = addUnsubProcess;
            SendUnsubscribeRequest(addUnsubProcess);
        }

        private void SendSubscribeRequest(SubscribeOrNotProcess subProcess)
        {
            var topic = new QuotationTopic { Platform = subProcess.TargetSubscribeKey.Platform, ContractSymbol = subProcess.TargetSubscribeKey.ContractSymbol };
            var topics = new List<QuotationTopic> { topic };
            QuotationSubscribeReq req = new QuotationSubscribeReq(topics);
            var reqContent = new ReqContent();
            reqContent.RequestId = subProcess.ReqId;
            reqContent.RequestStructType = SUBSCRIBE_REQ_TYPE;
            reqContent.RequestStructBytes = SerializeTBaseToBytes(req);
            SendReqTypeProtocolFrame(reqContent);
        }

        private void SendUnsubscribeRequest(SubscribeOrNotProcess unsubProcess)
        {
            var topic = new QuotationTopic { Platform = unsubProcess.TargetSubscribeKey.Platform, ContractSymbol = unsubProcess.TargetSubscribeKey.ContractSymbol };
            var topics = new List<QuotationTopic> { topic };
            QuotationUnSubscribeReq req = new QuotationUnSubscribeReq(topics);
            var reqContent = new ReqContent();
            reqContent.RequestId = unsubProcess.ReqId;
            reqContent.RequestStructType = UNSUBSCRIBE_REQ_TYPE;
            reqContent.RequestStructBytes = SerializeTBaseToBytes(req);
            SendReqTypeProtocolFrame(reqContent);
        }

        private void RetryOrCancelSubscribeProcessWithKey(SubscribeTopicReqKey subProcessKey, long? currentTimestampMs)
        {
            if (subProcessKey == null) return;

            if (!currentTimestampMs.HasValue || currentTimestampMs <= 0)
            {
                currentTimestampMs = LinuxTimestampMaker.currentTimeMillis();
            }

            SubscribeOrNotProcess tarSubProcess = null;
            if (!subscribeProcesses.TryGetValue(subProcessKey, out tarSubProcess)) return;

            if (tarSubProcess.RetryNum < SubscribeMaxRetryTimes)
            {
                AppLog.Info($"QuotationClient, Retry subscribe: {tarSubProcess}");
                tarSubProcess.LastReqTimeStampMs = currentTimestampMs.Value;
                tarSubProcess.RetryNum++;
                SendSubscribeRequest(tarSubProcess);
            }
            else
            {
                AppLog.Info($"QuotationClient, Cancel subscribe within failed state: {tarSubProcess}");
                subscribeProcesses.Remove(subProcessKey);
                OnSubscirbedQuotation?.Invoke(this, tarSubProcess.TargetSubscribeKey, false);
            }
        }

        private void RetryOrCancelUnsubscribeProcessWithKey(SubscribeTopicReqKey unsubProcessKey, long? currentTimestampMs)
        {
            if (unsubProcessKey == null) return;

            if (!currentTimestampMs.HasValue || currentTimestampMs <= 0)
            {
                currentTimestampMs = LinuxTimestampMaker.currentTimeMillis();
            }

            SubscribeOrNotProcess tarUnsubProcess = null;
            if (!unsubscribeProcesses.TryGetValue(unsubProcessKey, out tarUnsubProcess)) return;
            
            if (tarUnsubProcess.RetryNum < SubscribeMaxRetryTimes)
            {
                AppLog.Info($"QuotationClient, Retry unsubscribe: {tarUnsubProcess}");
                tarUnsubProcess.RetryNum++;
                tarUnsubProcess.LastReqTimeStampMs = currentTimestampMs.Value;
                SendUnsubscribeRequest(tarUnsubProcess);
            }
            else
            {
                AppLog.Info($"QuotationClient, Cancel unsubscribe within failed state: {tarUnsubProcess}");
                unsubscribeProcesses.Remove(unsubProcessKey);
                OnUnsubscirbedQuotation?.Invoke(this, tarUnsubProcess.TargetSubscribeKey, false);
            }
        }
        
        private void StartSubsribeCheckTimerIfNeed()
        {
            if (mSubscribeCheckTimer != null) return;

            mSubscribeCheckTimer = new System.Timers.Timer(DEFAULT_SUBSCRIBE_CHECK_INTERVAL);
            mSubscribeCheckTimer.Elapsed += SubscribeCheckTimer_Elapsed;
            mSubscribeCheckTimer.Start();
        }

        private void StopSubsribeCheckTimer()
        {
            if (mSubscribeCheckTimer != null)
            {
                mSubscribeCheckTimer.Elapsed -= SubscribeCheckTimer_Elapsed;
                mSubscribeCheckTimer.Stop();
                mSubscribeCheckTimer.Dispose();
                mSubscribeCheckTimer = null;
            }
        }

        private void StartHeartbeatTimerIfNeed()
        {
            if (mHeartbeatTimer != null) return;

            mHeartbeatTimer = new System.Timers.Timer(HeartBeatInterval);
            mHeartbeatTimer.Elapsed += HeartbeatTimer_Elapsed;
            mHeartbeatTimer.Start();
        }

        private void StopHeartbeatTimer()
        {
            if (mHeartbeatTimer != null)
            {
                mHeartbeatTimer.Elapsed -= HeartbeatTimer_Elapsed;
                mHeartbeatTimer.Stop();
                mHeartbeatTimer.Dispose();
                mHeartbeatTimer = null;
            }
        }

        private void Reconnect()
        {
            // 重连重试
            InternalDisconnect(false);
            InternalRequestConnect();
        }

        private void SendReqTypeProtocolFrame(ReqContent reqContent)
        {
            if (reqContent == null) return;
            if (string.IsNullOrEmpty(reqContent.RequestStructType)) throw new ArgumentException("reqContent.RequestStructType can't be empty.");

            ProtocolFrame topFrame = new ProtocolFrame();
            topFrame.Protocol = ProtocolType.REQ;
            topFrame.ReqContent = reqContent;

            AppLog.Info($"QuotationClient, SendReqTypeProtocolFrame: type=" + reqContent.RequestStructType + ", requestId=" + reqContent.RequestId);
            
            // Add into protocolFrameReqTypeStructTypesKeyedByRequestId
            protocolFrameReqTypeStructTypesKeyedByRequestId[reqContent.RequestId] = reqContent.RequestStructType;

            SocketSendMessage(SerializeTBaseToBytes(topFrame));
        }


        private byte[] SerializeTBaseToBytes(TBase tBaseObj)
        {
            if (tBaseObj == null) return null;

            MemoryStream memStream = null;
            TTransport transport = null;
            TProtocol protocol = null;
            BufferedStream bufStream = null;
            byte[] result = null;

            try
            {
                memStream = new MemoryStream();
                transport = new TStreamTransport(null, memStream);
                protocol = new TCompactProtocol(transport);
                tBaseObj.Write(protocol);

                bufStream = new BufferedStream(memStream);
                long len = bufStream.Length;
                byte[] bytes = new byte[len];
                bufStream.Position = 0;
                bufStream.Read(bytes, 0, (int)len);
                result = bytes;
            }
            finally
            {
                memStream?.Close();
                transport?.Close();
                protocol?.Dispose();
                bufStream?.Close();
            }

            return result;
        }
        
        private void UnserializeBytesToTBase(ref TBase tbaseObj, byte[] bytes)
        {
            MemoryStream memStream = null;
            BufferedStream bufStream = null;
            TTransport transport = null;
            TProtocol protocol = null;

            try
            {
                memStream = new MemoryStream();
                bufStream = new BufferedStream(memStream);
                bufStream.Write(bytes, 0, bytes.Length);

                transport = new TStreamTransport(bufStream, null);
                protocol = new TCompactProtocol(transport);
                bufStream.Position = 0;
                tbaseObj.Read(protocol);
            }
            finally
            {
                memStream?.Close();
                bufStream?.Close();
                transport?.Close();
                protocol?.Dispose();
            }
        }

        private void SocketSendMessage(byte[] message)
        {
            if (disposed) return;
            if (IsConnected())
            {
                mWebSocket?.Send(message);
            }
        }
    }
}
