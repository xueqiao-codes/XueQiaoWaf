using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Thrift;
using Thrift.Collections;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Runtime.CompilerServices; 
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Thrift.Protocol;
using Thrift.Transport;
using lib.xqclient_base.thriftapi_mediation;
using lib.xqclient_base.thriftapi_mediation.Interface;

namespace xueqiao.trade.hosting.proxy
{
    public class TradeHostingProxyHttpStub
    {
        public string Servant { get; private set; }

        public TradeHostingProxyHttpStub (string servant)
        {
            if (servant == null) throw new ArgumentNullException("`servant` can't be null.");
            this.Servant = servant;
        }

        public async Task<IInterfaceInteractResponse<ProxyLoginResp>> loginAsync(ProxyLoginReq req, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => login(req, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<ProxyLoginResp> login(ProxyLoginReq req, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "login";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("req", req));
            ProxyLoginResp result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingProxy.Client(protocol);
                transport.Open();
                result = client.login(platformArgs, req);
            }            
            catch (TTransportException e)

            {
                hasTransportExp = true;
                sourceExp = e;

            }            
            catch (XqclientTHttpException e)

            {
                hasTransportExp = true;
                sourceExp = e;
                httpResponseStatusCode = e.ResponseStatusCode;

            }            
            catch (Exception e)

            {
                sourceExp = e;

            }            
            finally
            {
                transport.Close();

            }            
            var resp = StubInterfaceInteractManager.SharedInstance.ResponseForInterfaceIntereaction(servant, 
            interfaceName, 
            serviceAccessUrl, 
            requestBeginTimestampMS, 
            result, 
            sourceExp, 
            hasTransportExp, 
            httpResponseStatusCode, 
            interactParams, 
            argsList);
            return resp;
        }

        public async Task<IInterfaceInteractResponse<ProxyFakeLoginResp>> fakeLoginAsync(ProxyFakeLoginReq req, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => fakeLogin(req, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<ProxyFakeLoginResp> fakeLogin(ProxyFakeLoginReq req, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "fakeLogin";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("req", req));
            ProxyFakeLoginResp result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingProxy.Client(protocol);
                transport.Open();
                result = client.fakeLogin(platformArgs, req);
            }            
            catch (TTransportException e)

            {
                hasTransportExp = true;
                sourceExp = e;

            }            
            catch (XqclientTHttpException e)

            {
                hasTransportExp = true;
                sourceExp = e;
                httpResponseStatusCode = e.ResponseStatusCode;

            }            
            catch (Exception e)

            {
                sourceExp = e;

            }            
            finally
            {
                transport.Close();

            }            
            var resp = StubInterfaceInteractManager.SharedInstance.ResponseForInterfaceIntereaction(servant, 
            interfaceName, 
            serviceAccessUrl, 
            requestBeginTimestampMS, 
            result, 
            sourceExp, 
            hasTransportExp, 
            httpResponseStatusCode, 
            interactParams, 
            argsList);
            return resp;
        }

        public async Task<IInterfaceInteractResponse> updateCompanyUserPasswordAsync(ProxyUpdatePasswordReq req, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => updateCompanyUserPassword(req, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse updateCompanyUserPassword(ProxyUpdatePasswordReq req, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "updateCompanyUserPassword";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("req", req));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingProxy.Client(protocol);
                transport.Open();
                client.updateCompanyUserPassword(platformArgs, req);
            }            
            catch (TTransportException e)

            {
                hasTransportExp = true;
                sourceExp = e;

            }            
            catch (XqclientTHttpException e)

            {
                hasTransportExp = true;
                sourceExp = e;
                httpResponseStatusCode = e.ResponseStatusCode;

            }            
            catch (Exception e)

            {
                sourceExp = e;

            }            
            finally
            {
                transport.Close();

            }            
            var resp = StubInterfaceInteractManager.SharedInstance.ResponseForInterfaceIntereaction(servant, 
            interfaceName, 
            serviceAccessUrl, 
            requestBeginTimestampMS, 
            sourceExp, 
            hasTransportExp, 
            httpResponseStatusCode, 
            interactParams, 
            argsList);
            return resp;
        }

        public async Task<IInterfaceInteractResponse<bool>> checkSessionAsync(xueqiao.trade.hosting.HostingSession session, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => checkSession(session, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<bool> checkSession(xueqiao.trade.hosting.HostingSession session, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "checkSession";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("session", session));
            bool result = false;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingProxy.Client(protocol);
                transport.Open();
                result = client.checkSession(platformArgs, session);
            }            
            catch (TTransportException e)

            {
                hasTransportExp = true;
                sourceExp = e;

            }            
            catch (XqclientTHttpException e)

            {
                hasTransportExp = true;
                sourceExp = e;
                httpResponseStatusCode = e.ResponseStatusCode;

            }            
            catch (Exception e)

            {
                sourceExp = e;

            }            
            finally
            {
                transport.Close();

            }            
            var resp = StubInterfaceInteractManager.SharedInstance.ResponseForInterfaceIntereaction(servant, 
            interfaceName, 
            serviceAccessUrl, 
            requestBeginTimestampMS, 
            result, 
            sourceExp, 
            hasTransportExp, 
            httpResponseStatusCode, 
            interactParams, 
            argsList);
            return resp;
        }

        public async Task<IInterfaceInteractResponse<AppVersion>> queryUpdateInfoAsync(UpdateInfoReq req, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => queryUpdateInfo(req, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<AppVersion> queryUpdateInfo(UpdateInfoReq req, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "queryUpdateInfo";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("req", req));
            AppVersion result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingProxy.Client(protocol);
                transport.Open();
                result = client.queryUpdateInfo(platformArgs, req);
            }            
            catch (TTransportException e)

            {
                hasTransportExp = true;
                sourceExp = e;

            }            
            catch (XqclientTHttpException e)

            {
                hasTransportExp = true;
                sourceExp = e;
                httpResponseStatusCode = e.ResponseStatusCode;

            }            
            catch (Exception e)

            {
                sourceExp = e;

            }            
            finally
            {
                transport.Close();

            }            
            var resp = StubInterfaceInteractManager.SharedInstance.ResponseForInterfaceIntereaction(servant, 
            interfaceName, 
            serviceAccessUrl, 
            requestBeginTimestampMS, 
            result, 
            sourceExp, 
            hasTransportExp, 
            httpResponseStatusCode, 
            interactParams, 
            argsList);
            return resp;
        }

        public async Task<IInterfaceInteractResponse<xueqiao.mailbox.user.message.thriftapi.UserMessagePage>> queryMailBoxMessageAsync(xueqiao.trade.hosting.HostingSession session, ReqMailBoxMessageOption option, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => queryMailBoxMessage(session, option, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.mailbox.user.message.thriftapi.UserMessagePage> queryMailBoxMessage(xueqiao.trade.hosting.HostingSession session, ReqMailBoxMessageOption option, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "queryMailBoxMessage";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("session", session));
            argsList.Add(new Tuple<string, object>("option", option));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            xueqiao.mailbox.user.message.thriftapi.UserMessagePage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingProxy.Client(protocol);
                transport.Open();
                result = client.queryMailBoxMessage(platformArgs, session, option, pageOption);
            }            
            catch (TTransportException e)

            {
                hasTransportExp = true;
                sourceExp = e;

            }            
            catch (XqclientTHttpException e)

            {
                hasTransportExp = true;
                sourceExp = e;
                httpResponseStatusCode = e.ResponseStatusCode;

            }            
            catch (Exception e)

            {
                sourceExp = e;

            }            
            finally
            {
                transport.Close();

            }            
            var resp = StubInterfaceInteractManager.SharedInstance.ResponseForInterfaceIntereaction(servant, 
            interfaceName, 
            serviceAccessUrl, 
            requestBeginTimestampMS, 
            result, 
            sourceExp, 
            hasTransportExp, 
            httpResponseStatusCode, 
            interactParams, 
            argsList);
            return resp;
        }

        public async Task<IInterfaceInteractResponse<bool>> markMessageAsReadAsync(xueqiao.trade.hosting.HostingSession session, THashSet<long> hostingMessageIds, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => markMessageAsRead(session, hostingMessageIds, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<bool> markMessageAsRead(xueqiao.trade.hosting.HostingSession session, THashSet<long> hostingMessageIds, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "markMessageAsRead";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("session", session));
            argsList.Add(new Tuple<string, object>("hostingMessageIds", hostingMessageIds));
            bool result = false;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingProxy.Client(protocol);
                transport.Open();
                result = client.markMessageAsRead(platformArgs, session, hostingMessageIds);
            }            
            catch (TTransportException e)

            {
                hasTransportExp = true;
                sourceExp = e;

            }            
            catch (XqclientTHttpException e)

            {
                hasTransportExp = true;
                sourceExp = e;
                httpResponseStatusCode = e.ResponseStatusCode;

            }            
            catch (Exception e)

            {
                sourceExp = e;

            }            
            finally
            {
                transport.Close();

            }            
            var resp = StubInterfaceInteractManager.SharedInstance.ResponseForInterfaceIntereaction(servant, 
            interfaceName, 
            serviceAccessUrl, 
            requestBeginTimestampMS, 
            result, 
            sourceExp, 
            hasTransportExp, 
            httpResponseStatusCode, 
            interactParams, 
            argsList);
            return resp;
        }


    }
}