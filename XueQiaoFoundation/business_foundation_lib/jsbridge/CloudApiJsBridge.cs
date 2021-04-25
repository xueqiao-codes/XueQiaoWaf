using lib.xqclient_base.thriftapi_mediation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Schedulers;
using System.Windows.Forms;
using Thrift;
using Thrift.Protocol;
using Thrift.Transport;
using xueqiao.trade.hosting.terminal.ao;

namespace business_foundation_lib.jsbridge
{
    [ComVisible(true)]
    public class CloudApiJsBridge
    {
        private delegate void ApiReplyAction(TProtocol replyInputProtocol, int reqId, Exception occurException);

        private readonly string[] needAccessAuthServiceNames = new string[] {
                "contract_terminal_service",
                "trade_hosting_terminal_ao",
                "pelicon_service"
            };

        private readonly WebBrowser webBrowser;
        private readonly LimitedConcurrencyLevelTaskScheduler apiTaskScheduler;

        public CloudApiJsBridge(WebBrowser webBrowser)
        {
            this.webBrowser = webBrowser;
            this.apiTaskScheduler = new LimitedConcurrencyLevelTaskScheduler(3);
        }

        public Func<LandingInfo> LandingInfoFactory;

        public void CloudApiInvoke(string servantName, string reqSerializedMessage)
        {
            Task.Factory.StartNew(() =>
            {
                SendApiRequest(servantName, reqSerializedMessage,
                    (replyInputProto, reqId, occurException) => ReceivedApiRequest(replyInputProto, reqId, occurException));
            },
            CancellationToken.None,
            TaskCreationOptions.None,
            apiTaskScheduler);
        }

        private void SendApiRequest(string servantName,
            string reqSerializedMessage,
            ApiReplyAction apiReplyAction)
        {
            bool servantNeedAccessAuth = needAccessAuthServiceNames.Contains(servantName);

            var connServerTransport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(servantName,
                new StubInterfaceInteractParams(),
                out string serviceAccessUrl);
            var outToServerProtocol = new TCompactProtocol(connServerTransport);
            var replyInputProtocol = outToServerProtocol;

            byte[] reqMessageBytes = Encoding.UTF8.GetBytes(reqSerializedMessage);
            var sliceTransport = new TMemoryBuffer(reqMessageBytes);
            TProtocol sliceInputProtocol = new TJSONProtocol(sliceTransport);

            // begin write message
            TMessage reqMessage = sliceInputProtocol.ReadMessageBegin();
            var reqMessageStruct = sliceInputProtocol.ReadStructBegin();
            var reqId = reqMessage.SeqID;

            try
            {
                connServerTransport.Open();

                outToServerProtocol.WriteMessageBegin(reqMessage);
                outToServerProtocol.WriteStructBegin(reqMessageStruct);

                // begin write fields

                // Write the `PlatformArgs` Field
                outToServerProtocol.WriteFieldBegin(new TField
                {
                    Name = "PlatformArgs",
                    Type = TType.Struct,
                    ID = 1
                });
                PlatformArgs platformArgs = GetPlatformArgs();
                platformArgs.Write(outToServerProtocol);
                outToServerProtocol.WriteFieldEnd();

                // Write LandingInfo if need
                if (servantNeedAccessAuth)
                {
                    outToServerProtocol.WriteFieldBegin(new TField
                    {
                        Name = "LandingInfo",
                        Type = TType.Struct,
                        ID = 2
                    });
                    LandingInfo landingInfo = GetLandingInfo();
                    landingInfo.Write(outToServerProtocol);
                    outToServerProtocol.WriteFieldEnd();
                }

                // Write Other fields
                while (true)
                {
                    TField filed = sliceInputProtocol.ReadFieldBegin();
                    if (filed.Type == TType.Stop)
                    {
                        break;
                    }

                    bool ignoreThisField = false;
                    if (filed.ID == 1)
                    {
                        // PlatformArgs 忽略
                        ignoreThisField = true;
                    }
                    else if (filed.ID == 2 && servantNeedAccessAuth)
                    {
                        // LandingInfo 忽略 
                        ignoreThisField = true;
                    }

                    if (ignoreThisField)
                    {
                        new ThriftProtocolTransfer(sliceInputProtocol, null).StartTransfer(filed.Type);
                    }
                    else
                    {
                        outToServerProtocol.WriteFieldBegin(filed);
                        new ThriftProtocolTransfer(sliceInputProtocol, outToServerProtocol).StartTransfer(filed.Type);
                        outToServerProtocol.WriteFieldEnd();
                    }

                    sliceInputProtocol.ReadFieldEnd();
                }

                sliceInputProtocol.ReadStructEnd();
                sliceInputProtocol.ReadMessageEnd();

                outToServerProtocol.WriteFieldStop();
                outToServerProtocol.WriteStructEnd();
                outToServerProtocol.WriteMessageEnd();

                // Send to server
                connServerTransport.Flush();

                apiReplyAction?.Invoke(replyInputProtocol, reqId, null);
            }
            catch (Exception e)
            {
#if DEBUG
                Console.WriteLine(e);
#endif
                apiReplyAction?.Invoke(replyInputProtocol, reqId, e);
            }
            finally
            {
                connServerTransport.Close();
            }
        }

        private void ReceivedApiRequest(TProtocol replyInputProto, int reqId, Exception occurException)
        {
            if (occurException != null)
            {
                OnCloudApiFinishedInvokeScript(reqId, "", true);
                return;
            }
            // Reply of request
            var replyMessage = replyInputProto.ReadMessageBegin();
            if (replyMessage.Type == TMessageType.Exception)
            {
                TApplicationException x = TApplicationException.Read(replyInputProto);
                replyInputProto.ReadMessageEnd();
                OnCloudApiFinishedInvokeScript(reqId, "", true);
                return;
            }

            var replayStrcut = replyInputProto.ReadStructBegin();

            // 序列化成 json 返回至 js 端
            var outToJsonTransport = new TMemoryBuffer();
            var outToJsonProtocol = new TJSONProtocol(outToJsonTransport);

            outToJsonProtocol.WriteMessageBegin(replyMessage);
            outToJsonProtocol.WriteStructBegin(replayStrcut);

            while (true)
            {
                TField filed = replyInputProto.ReadFieldBegin();
                if (filed.Type == TType.Stop)
                {
                    break;
                }

                outToJsonProtocol.WriteFieldBegin(filed);
                new ThriftProtocolTransfer(replyInputProto, outToJsonProtocol).StartTransfer(filed.Type);
                outToJsonProtocol.WriteFieldEnd();

                replyInputProto.ReadFieldEnd();
            }

            replyInputProto.ReadStructEnd();
            replyInputProto.ReadMessageEnd();

            outToJsonProtocol.WriteFieldStop();
            outToJsonProtocol.WriteStructEnd();
            outToJsonProtocol.WriteMessageEnd();

            var replayMessageBytes = outToJsonTransport.GetBuffer();
            var replyArgsJson = Encoding.UTF8.GetString(replayMessageBytes);

            OnCloudApiFinishedInvokeScript(reqId, replyArgsJson, false);
        }

        private void OnCloudApiFinishedInvokeScript(int reqId, string resultArgs, bool hasTransportError)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                webBrowser?.Document?.InvokeScript("CloudApiNativeBridge_OnApiInvokeFinished", new object[] { reqId, resultArgs ?? "", hasTransportError });
            }));
        }

        private PlatformArgs GetPlatformArgs()
        {
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            return platformArgs;
        }

        private LandingInfo GetLandingInfo()
        {
            return LandingInfoFactory?.Invoke();
        }
    }
}
