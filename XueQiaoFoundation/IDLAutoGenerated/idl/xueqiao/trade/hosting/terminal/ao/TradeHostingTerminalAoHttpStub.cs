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

namespace xueqiao.trade.hosting.terminal.ao
{
    public class TradeHostingTerminalAoHttpStub
    {
        public string Servant { get; private set; }

        public TradeHostingTerminalAoHttpStub (string servant)
        {
            if (servant == null) throw new ArgumentNullException("`servant` can't be null.");
            this.Servant = servant;
        }

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.QueryHostingUserPage>> getHostingUserPageAsync(LandingInfo landingInfo, xueqiao.trade.hosting.QueryHostingUserOption queryOption, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => getHostingUserPage(landingInfo, queryOption, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.QueryHostingUserPage> getHostingUserPage(LandingInfo landingInfo, xueqiao.trade.hosting.QueryHostingUserOption queryOption, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "getHostingUserPage";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("queryOption", queryOption));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            xueqiao.trade.hosting.QueryHostingUserPage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.getHostingUserPage(platformArgs, landingInfo, queryOption, pageOption);
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

        public async Task<IInterfaceInteractResponse> heartBeatAsync(LandingInfo landingInfo, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => heartBeat(landingInfo, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse heartBeat(LandingInfo landingInfo, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "heartBeat";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                client.heartBeat(platformArgs, landingInfo);
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

        public async Task<IInterfaceInteractResponse> logoutAsync(LandingInfo landingInfo, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => logout(landingInfo, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse logout(LandingInfo landingInfo, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "logout";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                client.logout(platformArgs, landingInfo);
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

        public async Task<IInterfaceInteractResponse<Dictionary<long, HostingComposeViewDetail>>> getComposeViewDetailsAsync(LandingInfo landingInfo, THashSet<long> composeGraphIds, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => getComposeViewDetails(landingInfo, composeGraphIds, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<Dictionary<long, HostingComposeViewDetail>> getComposeViewDetails(LandingInfo landingInfo, THashSet<long> composeGraphIds, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "getComposeViewDetails";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("composeGraphIds", composeGraphIds));
            Dictionary<long, HostingComposeViewDetail> result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.getComposeViewDetails(platformArgs, landingInfo, composeGraphIds);
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

        public async Task<IInterfaceInteractResponse> changeComposeViewPrecisionNumberAsync(LandingInfo landingInfo, long composeGraphId, short precisionNumber, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => changeComposeViewPrecisionNumber(landingInfo, composeGraphId, precisionNumber, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse changeComposeViewPrecisionNumber(LandingInfo landingInfo, long composeGraphId, short precisionNumber, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "changeComposeViewPrecisionNumber";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("composeGraphId", composeGraphId));
            argsList.Add(new Tuple<string, object>("precisionNumber", precisionNumber));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                client.changeComposeViewPrecisionNumber(platformArgs, landingInfo, composeGraphId, precisionNumber);
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

        public async Task<IInterfaceInteractResponse<long>> createComposeGraphAsync(LandingInfo landingInfo, xueqiao.trade.hosting.HostingComposeGraph newGraph, string aliasName, short precisionNumber, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => createComposeGraph(landingInfo, newGraph, aliasName, precisionNumber, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<long> createComposeGraph(LandingInfo landingInfo, xueqiao.trade.hosting.HostingComposeGraph newGraph, string aliasName, short precisionNumber, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "createComposeGraph";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("newGraph", newGraph));
            argsList.Add(new Tuple<string, object>("aliasName", aliasName));
            argsList.Add(new Tuple<string, object>("precisionNumber", precisionNumber));
            long result = 0;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.createComposeGraph(platformArgs, landingInfo, newGraph, aliasName, precisionNumber);
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

        public async Task<IInterfaceInteractResponse> delComposeViewAsync(LandingInfo landingInfo, long composeGraphId, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => delComposeView(landingInfo, composeGraphId, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse delComposeView(LandingInfo landingInfo, long composeGraphId, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "delComposeView";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("composeGraphId", composeGraphId));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                client.delComposeView(platformArgs, landingInfo, composeGraphId);
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

        public async Task<IInterfaceInteractResponse<QueryHostingComposeViewDetailPage>> getComposeViewDetailPageAsync(LandingInfo landingInfo, QueryHostingComposeViewDetailOption queryOption, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => getComposeViewDetailPage(landingInfo, queryOption, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<QueryHostingComposeViewDetailPage> getComposeViewDetailPage(LandingInfo landingInfo, QueryHostingComposeViewDetailOption queryOption, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "getComposeViewDetailPage";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("queryOption", queryOption));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            QueryHostingComposeViewDetailPage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.getComposeViewDetailPage(platformArgs, landingInfo, queryOption, pageOption);
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

        public async Task<IInterfaceInteractResponse<QuerySameComposeGraphsPage>> getSameComposeGraphsPageAsync(LandingInfo landingInfo, xueqiao.trade.hosting.HostingComposeGraph graph, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => getSameComposeGraphsPage(landingInfo, graph, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<QuerySameComposeGraphsPage> getSameComposeGraphsPage(LandingInfo landingInfo, xueqiao.trade.hosting.HostingComposeGraph graph, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "getSameComposeGraphsPage";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("graph", graph));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            QuerySameComposeGraphsPage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.getSameComposeGraphsPage(platformArgs, landingInfo, graph, pageOption);
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

        public async Task<IInterfaceInteractResponse> addComposeViewBySearchAsync(LandingInfo landingInfo, long composeGraphId, string composeGraphKey, string aliasName, short precisionNumber, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => addComposeViewBySearch(landingInfo, composeGraphId, composeGraphKey, aliasName, precisionNumber, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse addComposeViewBySearch(LandingInfo landingInfo, long composeGraphId, string composeGraphKey, string aliasName, short precisionNumber, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "addComposeViewBySearch";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("composeGraphId", composeGraphId));
            argsList.Add(new Tuple<string, object>("composeGraphKey", composeGraphKey));
            argsList.Add(new Tuple<string, object>("aliasName", aliasName));
            argsList.Add(new Tuple<string, object>("precisionNumber", precisionNumber));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                client.addComposeViewBySearch(platformArgs, landingInfo, composeGraphId, composeGraphKey, aliasName, precisionNumber);
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

        public async Task<IInterfaceInteractResponse> subscribeComposeViewQuotationAsync(LandingInfo landingInfo, long composeGraphId, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => subscribeComposeViewQuotation(landingInfo, composeGraphId, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse subscribeComposeViewQuotation(LandingInfo landingInfo, long composeGraphId, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "subscribeComposeViewQuotation";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("composeGraphId", composeGraphId));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                client.subscribeComposeViewQuotation(platformArgs, landingInfo, composeGraphId);
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

        public async Task<IInterfaceInteractResponse> unSubscribeComposeViewQuotationAsync(LandingInfo landingInfo, long composeGraphId, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => unSubscribeComposeViewQuotation(landingInfo, composeGraphId, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse unSubscribeComposeViewQuotation(LandingInfo landingInfo, long composeGraphId, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "unSubscribeComposeViewQuotation";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("composeGraphId", composeGraphId));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                client.unSubscribeComposeViewQuotation(platformArgs, landingInfo, composeGraphId);
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

        public async Task<IInterfaceInteractResponse> changeComposeViewAliasNameAsync(LandingInfo landingInfo, long composeGraphId, string aliasName, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => changeComposeViewAliasName(landingInfo, composeGraphId, aliasName, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse changeComposeViewAliasName(LandingInfo landingInfo, long composeGraphId, string aliasName, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "changeComposeViewAliasName";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("composeGraphId", composeGraphId));
            argsList.Add(new Tuple<string, object>("aliasName", aliasName));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                client.changeComposeViewAliasName(platformArgs, landingInfo, composeGraphId, aliasName);
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

        public async Task<IInterfaceInteractResponse<Dictionary<long, xueqiao.trade.hosting.HostingComposeGraph>>> getComposeGraphsAsync(LandingInfo landingInfo, THashSet<long> composeGraphIds, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => getComposeGraphs(landingInfo, composeGraphIds, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<Dictionary<long, xueqiao.trade.hosting.HostingComposeGraph>> getComposeGraphs(LandingInfo landingInfo, THashSet<long> composeGraphIds, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "getComposeGraphs";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("composeGraphIds", composeGraphIds));
            Dictionary<long, xueqiao.trade.hosting.HostingComposeGraph> result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.getComposeGraphs(platformArgs, landingInfo, composeGraphIds);
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

        public async Task<IInterfaceInteractResponse> addComposeViewByShareAsync(LandingInfo landingInfo, long composeGraphId, string aliasName, short precisionNumber, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => addComposeViewByShare(landingInfo, composeGraphId, aliasName, precisionNumber, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse addComposeViewByShare(LandingInfo landingInfo, long composeGraphId, string aliasName, short precisionNumber, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "addComposeViewByShare";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("composeGraphId", composeGraphId));
            argsList.Add(new Tuple<string, object>("aliasName", aliasName));
            argsList.Add(new Tuple<string, object>("precisionNumber", precisionNumber));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                client.addComposeViewByShare(platformArgs, landingInfo, composeGraphId, aliasName, precisionNumber);
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

        public async Task<IInterfaceInteractResponse<long>> addTradeAccountAsync(LandingInfo landingInfo, xueqiao.trade.hosting.HostingTradeAccount newAccount, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => addTradeAccount(landingInfo, newAccount, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<long> addTradeAccount(LandingInfo landingInfo, xueqiao.trade.hosting.HostingTradeAccount newAccount, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "addTradeAccount";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("newAccount", newAccount));
            long result = 0;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.addTradeAccount(platformArgs, landingInfo, newAccount);
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

        public async Task<IInterfaceInteractResponse> disableTradeAccountAsync(LandingInfo landingInfo, long tradeAccountId, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => disableTradeAccount(landingInfo, tradeAccountId, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse disableTradeAccount(LandingInfo landingInfo, long tradeAccountId, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "disableTradeAccount";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("tradeAccountId", tradeAccountId));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                client.disableTradeAccount(platformArgs, landingInfo, tradeAccountId);
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

        public async Task<IInterfaceInteractResponse<QueryHostingTradeAccountPage>> getTradeAccountPageAsync(LandingInfo landingInfo, QueryHostingTradeAccountOption queryOption, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => getTradeAccountPage(landingInfo, queryOption, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<QueryHostingTradeAccountPage> getTradeAccountPage(LandingInfo landingInfo, QueryHostingTradeAccountOption queryOption, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "getTradeAccountPage";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("queryOption", queryOption));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            QueryHostingTradeAccountPage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.getTradeAccountPage(platformArgs, landingInfo, queryOption, pageOption);
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

        public async Task<IInterfaceInteractResponse> enableTradeAccountAsync(LandingInfo landingInfo, long tradeAccountId, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => enableTradeAccount(landingInfo, tradeAccountId, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse enableTradeAccount(LandingInfo landingInfo, long tradeAccountId, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "enableTradeAccount";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("tradeAccountId", tradeAccountId));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                client.enableTradeAccount(platformArgs, landingInfo, tradeAccountId);
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

        public async Task<IInterfaceInteractResponse> updateTradeAccountInfoAsync(LandingInfo landingInfo, xueqiao.trade.hosting.HostingTradeAccount updateAccount, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => updateTradeAccountInfo(landingInfo, updateAccount, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse updateTradeAccountInfo(LandingInfo landingInfo, xueqiao.trade.hosting.HostingTradeAccount updateAccount, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "updateTradeAccountInfo";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("updateAccount", updateAccount));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                client.updateTradeAccountInfo(platformArgs, landingInfo, updateAccount);
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

        public async Task<IInterfaceInteractResponse> rmTradeAccountAsync(LandingInfo landingInfo, long tradeAccountId, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => rmTradeAccount(landingInfo, tradeAccountId, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse rmTradeAccount(LandingInfo landingInfo, long tradeAccountId, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "rmTradeAccount";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("tradeAccountId", tradeAccountId));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                client.rmTradeAccount(platformArgs, landingInfo, tradeAccountId);
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

        public async Task<IInterfaceInteractResponse<List<xueqiao.trade.hosting.HostingTradeAccount>>> getPersonalUserTradeAccountAsync(LandingInfo landingInfo, long subAccountId, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => getPersonalUserTradeAccount(landingInfo, subAccountId, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<List<xueqiao.trade.hosting.HostingTradeAccount>> getPersonalUserTradeAccount(LandingInfo landingInfo, long subAccountId, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "getPersonalUserTradeAccount";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("subAccountId", subAccountId));
            List<xueqiao.trade.hosting.HostingTradeAccount> result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.getPersonalUserTradeAccount(platformArgs, landingInfo, subAccountId);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.HostingOrderRouteTree>> getHostingOrderRouteTreeAsync(LandingInfo landingInfo, long subAccountId, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => getHostingOrderRouteTree(landingInfo, subAccountId, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.HostingOrderRouteTree> getHostingOrderRouteTree(LandingInfo landingInfo, long subAccountId, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "getHostingOrderRouteTree";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("subAccountId", subAccountId));
            xueqiao.trade.hosting.HostingOrderRouteTree result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.getHostingOrderRouteTree(platformArgs, landingInfo, subAccountId);
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

        public async Task<IInterfaceInteractResponse> updateHostingOrderRouteTreeAsync(LandingInfo landingInfo, long subAccountId, xueqiao.trade.hosting.HostingOrderRouteTree routeTree, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => updateHostingOrderRouteTree(landingInfo, subAccountId, routeTree, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse updateHostingOrderRouteTree(LandingInfo landingInfo, long subAccountId, xueqiao.trade.hosting.HostingOrderRouteTree routeTree, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "updateHostingOrderRouteTree";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("subAccountId", subAccountId));
            argsList.Add(new Tuple<string, object>("routeTree", routeTree));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                client.updateHostingOrderRouteTree(platformArgs, landingInfo, subAccountId, routeTree);
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

        public async Task<IInterfaceInteractResponse<int>> getHostingOrderRouteTreeVersionAsync(LandingInfo landingInfo, long subAccountId, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => getHostingOrderRouteTreeVersion(landingInfo, subAccountId, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<int> getHostingOrderRouteTreeVersion(LandingInfo landingInfo, long subAccountId, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "getHostingOrderRouteTreeVersion";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("subAccountId", subAccountId));
            int result = 0;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.getHostingOrderRouteTreeVersion(platformArgs, landingInfo, subAccountId);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.HostingOrderRouteTree>> getPersonalUserHostingOrderRouteTreeAsync(LandingInfo landingInfo, long subAccountId, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => getPersonalUserHostingOrderRouteTree(landingInfo, subAccountId, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.HostingOrderRouteTree> getPersonalUserHostingOrderRouteTree(LandingInfo landingInfo, long subAccountId, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "getPersonalUserHostingOrderRouteTree";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("subAccountId", subAccountId));
            xueqiao.trade.hosting.HostingOrderRouteTree result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.getPersonalUserHostingOrderRouteTree(platformArgs, landingInfo, subAccountId);
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

        public async Task<IInterfaceInteractResponse> createXQOrderAsync(LandingInfo landingInfo, long subAccountId, string orderId, xueqiao.trade.hosting.arbitrage.thriftapi.HostingXQOrderType orderType, xueqiao.trade.hosting.arbitrage.thriftapi.HostingXQTarget orderTarget, xueqiao.trade.hosting.arbitrage.thriftapi.HostingXQOrderDetail orderDetail, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => createXQOrder(landingInfo, subAccountId, orderId, orderType, orderTarget, orderDetail, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse createXQOrder(LandingInfo landingInfo, long subAccountId, string orderId, xueqiao.trade.hosting.arbitrage.thriftapi.HostingXQOrderType orderType, xueqiao.trade.hosting.arbitrage.thriftapi.HostingXQTarget orderTarget, xueqiao.trade.hosting.arbitrage.thriftapi.HostingXQOrderDetail orderDetail, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "createXQOrder";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("subAccountId", subAccountId));
            argsList.Add(new Tuple<string, object>("orderId", orderId));
            argsList.Add(new Tuple<string, object>("orderType", orderType));
            argsList.Add(new Tuple<string, object>("orderTarget", orderTarget));
            argsList.Add(new Tuple<string, object>("orderDetail", orderDetail));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                client.createXQOrder(platformArgs, landingInfo, subAccountId, orderId, orderType, orderTarget, orderDetail);
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

        public async Task<IInterfaceInteractResponse<Dictionary<string, ErrorInfo>>> batchSuspendXQOrdersAsync(LandingInfo landingInfo, THashSet<string> orderIds, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => batchSuspendXQOrders(landingInfo, orderIds, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<Dictionary<string, ErrorInfo>> batchSuspendXQOrders(LandingInfo landingInfo, THashSet<string> orderIds, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "batchSuspendXQOrders";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("orderIds", orderIds));
            Dictionary<string, ErrorInfo> result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.batchSuspendXQOrders(platformArgs, landingInfo, orderIds);
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

        public async Task<IInterfaceInteractResponse<Dictionary<string, ErrorInfo>>> batchResumeXQOrdersAsync(LandingInfo landingInfo, THashSet<string> orderIds, Dictionary<string, xueqiao.trade.hosting.arbitrage.thriftapi.HostingXQOrderResumeMode> resumeModes, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => batchResumeXQOrders(landingInfo, orderIds, resumeModes, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<Dictionary<string, ErrorInfo>> batchResumeXQOrders(LandingInfo landingInfo, THashSet<string> orderIds, Dictionary<string, xueqiao.trade.hosting.arbitrage.thriftapi.HostingXQOrderResumeMode> resumeModes, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "batchResumeXQOrders";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("orderIds", orderIds));
            argsList.Add(new Tuple<string, object>("resumeModes", resumeModes));
            Dictionary<string, ErrorInfo> result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.batchResumeXQOrders(platformArgs, landingInfo, orderIds, resumeModes);
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

        public async Task<IInterfaceInteractResponse<Dictionary<string, ErrorInfo>>> batchCancelXQOrdersAsync(LandingInfo landingInfo, THashSet<string> orderIds, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => batchCancelXQOrders(landingInfo, orderIds, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<Dictionary<string, ErrorInfo>> batchCancelXQOrders(LandingInfo landingInfo, THashSet<string> orderIds, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "batchCancelXQOrders";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("orderIds", orderIds));
            Dictionary<string, ErrorInfo> result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.batchCancelXQOrders(platformArgs, landingInfo, orderIds);
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

        public async Task<IInterfaceInteractResponse<HostingXQOrderWithTradeListPage>> getEffectXQOrderWithTradeListPageAsync(LandingInfo landingInfo, xueqiao.trade.hosting.arbitrage.thriftapi.QueryEffectXQOrderIndexOption qryOption, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => getEffectXQOrderWithTradeListPage(landingInfo, qryOption, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<HostingXQOrderWithTradeListPage> getEffectXQOrderWithTradeListPage(LandingInfo landingInfo, xueqiao.trade.hosting.arbitrage.thriftapi.QueryEffectXQOrderIndexOption qryOption, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "getEffectXQOrderWithTradeListPage";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("qryOption", qryOption));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            HostingXQOrderWithTradeListPage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.getEffectXQOrderWithTradeListPage(platformArgs, landingInfo, qryOption, pageOption);
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

        public async Task<IInterfaceInteractResponse<Dictionary<string, xueqiao.trade.hosting.arbitrage.thriftapi.HostingXQOrderWithTradeList>>> getXQOrderWithTradeListsAsync(LandingInfo landingInfo, THashSet<string> orderIds, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => getXQOrderWithTradeLists(landingInfo, orderIds, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<Dictionary<string, xueqiao.trade.hosting.arbitrage.thriftapi.HostingXQOrderWithTradeList>> getXQOrderWithTradeLists(LandingInfo landingInfo, THashSet<string> orderIds, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "getXQOrderWithTradeLists";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("orderIds", orderIds));
            Dictionary<string, xueqiao.trade.hosting.arbitrage.thriftapi.HostingXQOrderWithTradeList> result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.getXQOrderWithTradeLists(platformArgs, landingInfo, orderIds);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.arbitrage.thriftapi.HostingXQOrderExecDetail>> getXQOrderExecDetailAsync(LandingInfo landingInfo, string orderId, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => getXQOrderExecDetail(landingInfo, orderId, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.arbitrage.thriftapi.HostingXQOrderExecDetail> getXQOrderExecDetail(LandingInfo landingInfo, string orderId, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "getXQOrderExecDetail";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("orderId", orderId));
            xueqiao.trade.hosting.arbitrage.thriftapi.HostingXQOrderExecDetail result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.getXQOrderExecDetail(platformArgs, landingInfo, orderId);
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

        public async Task<IInterfaceInteractResponse<HostingXQOrderPage>> getXQOrderHisPageAsync(LandingInfo landingInfo, xueqiao.trade.hosting.history.thriftapi.QueryXQOrderHisIndexItemOption qryOption, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => getXQOrderHisPage(landingInfo, qryOption, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<HostingXQOrderPage> getXQOrderHisPage(LandingInfo landingInfo, xueqiao.trade.hosting.history.thriftapi.QueryXQOrderHisIndexItemOption qryOption, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "getXQOrderHisPage";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("qryOption", qryOption));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            HostingXQOrderPage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.getXQOrderHisPage(platformArgs, landingInfo, qryOption, pageOption);
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

        public async Task<IInterfaceInteractResponse<HostingXQTradePage>> getXQTradeHisPageAsync(LandingInfo landingInfo, xueqiao.trade.hosting.history.thriftapi.QueryXQTradeHisIndexItemOption qryOption, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => getXQTradeHisPage(landingInfo, qryOption, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<HostingXQTradePage> getXQTradeHisPage(LandingInfo landingInfo, xueqiao.trade.hosting.history.thriftapi.QueryXQTradeHisIndexItemOption qryOption, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "getXQTradeHisPage";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("qryOption", qryOption));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            HostingXQTradePage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.getXQTradeHisPage(platformArgs, landingInfo, qryOption, pageOption);
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

        public async Task<IInterfaceInteractResponse<HostingUserSetting>> getUserSettingAsync(LandingInfo landingInfo, string key, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => getUserSetting(landingInfo, key, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<HostingUserSetting> getUserSetting(LandingInfo landingInfo, string key, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "getUserSetting";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("key", key));
            HostingUserSetting result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.getUserSetting(platformArgs, landingInfo, key);
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

        public async Task<IInterfaceInteractResponse> updateUserSettingAsync(LandingInfo landingInfo, string key, HostingUserSetting setting, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => updateUserSetting(landingInfo, key, setting, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse updateUserSetting(LandingInfo landingInfo, string key, HostingUserSetting setting, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "updateUserSetting";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("key", key));
            argsList.Add(new Tuple<string, object>("setting", setting));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                client.updateUserSetting(platformArgs, landingInfo, key, setting);
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

        public async Task<IInterfaceInteractResponse<int>> getUserSettingVersionAsync(LandingInfo landingInfo, string key, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => getUserSettingVersion(landingInfo, key, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<int> getUserSettingVersion(LandingInfo landingInfo, string key, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "getUserSettingVersion";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("key", key));
            int result = 0;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.getUserSettingVersion(platformArgs, landingInfo, key);
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

        public async Task<IInterfaceInteractResponse<HostingSAWRUItemListPage>> getSAWRUTListPageAsync(LandingInfo landingInfo, QueryHostingSAWRUItemListOption queryOption, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => getSAWRUTListPage(landingInfo, queryOption, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<HostingSAWRUItemListPage> getSAWRUTListPage(LandingInfo landingInfo, QueryHostingSAWRUItemListOption queryOption, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "getSAWRUTListPage";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("queryOption", queryOption));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            HostingSAWRUItemListPage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.getSAWRUTListPage(platformArgs, landingInfo, queryOption, pageOption);
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

        public async Task<IInterfaceInteractResponse<Dictionary<long, List<xueqiao.trade.hosting.HostingSubAccountRelatedItem>>>> getSARUTBySubAccountIdAsync(LandingInfo landingInfo, THashSet<long> subAccountIds, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => getSARUTBySubAccountId(landingInfo, subAccountIds, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<Dictionary<long, List<xueqiao.trade.hosting.HostingSubAccountRelatedItem>>> getSARUTBySubAccountId(LandingInfo landingInfo, THashSet<long> subAccountIds, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "getSARUTBySubAccountId";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("subAccountIds", subAccountIds));
            Dictionary<long, List<xueqiao.trade.hosting.HostingSubAccountRelatedItem>> result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.getSARUTBySubAccountId(platformArgs, landingInfo, subAccountIds);
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

        public async Task<IInterfaceInteractResponse<Dictionary<int, List<xueqiao.trade.hosting.HostingSubAccountRelatedItem>>>> getSARUTBySubUserIdAsync(LandingInfo landingInfo, THashSet<int> subUserIds, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => getSARUTBySubUserId(landingInfo, subUserIds, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<Dictionary<int, List<xueqiao.trade.hosting.HostingSubAccountRelatedItem>>> getSARUTBySubUserId(LandingInfo landingInfo, THashSet<int> subUserIds, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "getSARUTBySubUserId";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("subUserIds", subUserIds));
            Dictionary<int, List<xueqiao.trade.hosting.HostingSubAccountRelatedItem>> result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.getSARUTBySubUserId(platformArgs, landingInfo, subUserIds);
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

        public async Task<IInterfaceInteractResponse> assignSubAccountRelatedUsersAsync(LandingInfo landingInfo, long subAccountId, THashSet<int> relatedSubUserIds, THashSet<int> unRelatedSubUserIds, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => assignSubAccountRelatedUsers(landingInfo, subAccountId, relatedSubUserIds, unRelatedSubUserIds, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse assignSubAccountRelatedUsers(LandingInfo landingInfo, long subAccountId, THashSet<int> relatedSubUserIds, THashSet<int> unRelatedSubUserIds, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "assignSubAccountRelatedUsers";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("subAccountId", subAccountId));
            argsList.Add(new Tuple<string, object>("relatedSubUserIds", relatedSubUserIds));
            argsList.Add(new Tuple<string, object>("unRelatedSubUserIds", unRelatedSubUserIds));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                client.assignSubAccountRelatedUsers(platformArgs, landingInfo, subAccountId, relatedSubUserIds, unRelatedSubUserIds);
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

        public async Task<IInterfaceInteractResponse> renameSubAccountAsync(LandingInfo landingInfo, long subAccountId, string subAccountName, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => renameSubAccount(landingInfo, subAccountId, subAccountName, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse renameSubAccount(LandingInfo landingInfo, long subAccountId, string subAccountName, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "renameSubAccount";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("subAccountId", subAccountId));
            argsList.Add(new Tuple<string, object>("subAccountName", subAccountName));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                client.renameSubAccount(platformArgs, landingInfo, subAccountId, subAccountName);
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

        public async Task<IInterfaceInteractResponse<long>> createSubAccountAsync(LandingInfo landingInfo, xueqiao.trade.hosting.HostingSubAccount newSubAccount, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => createSubAccount(landingInfo, newSubAccount, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<long> createSubAccount(LandingInfo landingInfo, xueqiao.trade.hosting.HostingSubAccount newSubAccount, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "createSubAccount";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("newSubAccount", newSubAccount));
            long result = 0;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.createSubAccount(platformArgs, landingInfo, newSubAccount);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.asset.thriftapi.HostingSledContractPositionPage>> getHostingSledContractPositionAsync(LandingInfo landingInfo, xueqiao.trade.hosting.asset.thriftapi.ReqHostingSledContractPositionOption option, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => getHostingSledContractPosition(landingInfo, option, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.asset.thriftapi.HostingSledContractPositionPage> getHostingSledContractPosition(LandingInfo landingInfo, xueqiao.trade.hosting.asset.thriftapi.ReqHostingSledContractPositionOption option, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "getHostingSledContractPosition";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("option", option));
            xueqiao.trade.hosting.asset.thriftapi.HostingSledContractPositionPage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.getHostingSledContractPosition(platformArgs, landingInfo, option);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.asset.thriftapi.HostingFundPage>> getHostingSubAccountFundAsync(LandingInfo landingInfo, xueqiao.trade.hosting.asset.thriftapi.ReqHostingFundOption option, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => getHostingSubAccountFund(landingInfo, option, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.asset.thriftapi.HostingFundPage> getHostingSubAccountFund(LandingInfo landingInfo, xueqiao.trade.hosting.asset.thriftapi.ReqHostingFundOption option, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "getHostingSubAccountFund";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("option", option));
            xueqiao.trade.hosting.asset.thriftapi.HostingFundPage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.getHostingSubAccountFund(platformArgs, landingInfo, option);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.asset.thriftapi.HostingSubAccountFund>> changeSubAccountFundAsync(LandingInfo landingInfo, xueqiao.trade.hosting.asset.thriftapi.FundChange fundChange, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => changeSubAccountFund(landingInfo, fundChange, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.asset.thriftapi.HostingSubAccountFund> changeSubAccountFund(LandingInfo landingInfo, xueqiao.trade.hosting.asset.thriftapi.FundChange fundChange, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "changeSubAccountFund";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("fundChange", fundChange));
            xueqiao.trade.hosting.asset.thriftapi.HostingSubAccountFund result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.changeSubAccountFund(platformArgs, landingInfo, fundChange);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.asset.thriftapi.HostingSubAccountFund>> setSubAccountCreditAmountAsync(LandingInfo landingInfo, xueqiao.trade.hosting.asset.thriftapi.CreditAmountChange amountChange, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => setSubAccountCreditAmount(landingInfo, amountChange, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.asset.thriftapi.HostingSubAccountFund> setSubAccountCreditAmount(LandingInfo landingInfo, xueqiao.trade.hosting.asset.thriftapi.CreditAmountChange amountChange, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "setSubAccountCreditAmount";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("amountChange", amountChange));
            xueqiao.trade.hosting.asset.thriftapi.HostingSubAccountFund result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.setSubAccountCreditAmount(platformArgs, landingInfo, amountChange);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.asset.thriftapi.AssetTradeDetailPage>> getAssetPositionTradeDetailAsync(LandingInfo landingInfo, xueqiao.trade.hosting.asset.thriftapi.ReqHostingAssetTradeDetailOption option, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => getAssetPositionTradeDetail(landingInfo, option, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.asset.thriftapi.AssetTradeDetailPage> getAssetPositionTradeDetail(LandingInfo landingInfo, xueqiao.trade.hosting.asset.thriftapi.ReqHostingAssetTradeDetailOption option, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "getAssetPositionTradeDetail";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("option", option));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            xueqiao.trade.hosting.asset.thriftapi.AssetTradeDetailPage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.getAssetPositionTradeDetail(platformArgs, landingInfo, option, pageOption);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.asset.thriftapi.HostingSubAccountMoneyRecordPage>> getHostingSubAccountMoneyRecordAsync(LandingInfo landingInfo, xueqiao.trade.hosting.asset.thriftapi.ReqMoneyRecordOption option, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => getHostingSubAccountMoneyRecord(landingInfo, option, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.asset.thriftapi.HostingSubAccountMoneyRecordPage> getHostingSubAccountMoneyRecord(LandingInfo landingInfo, xueqiao.trade.hosting.asset.thriftapi.ReqMoneyRecordOption option, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "getHostingSubAccountMoneyRecord";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("option", option));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            xueqiao.trade.hosting.asset.thriftapi.HostingSubAccountMoneyRecordPage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.getHostingSubAccountMoneyRecord(platformArgs, landingInfo, option, pageOption);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.asset.thriftapi.HostingFundPage>> getSubAccountFundHistoryAsync(LandingInfo landingInfo, xueqiao.trade.hosting.asset.thriftapi.ReqSubAccountFundHistoryOption option, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => getSubAccountFundHistory(landingInfo, option, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.asset.thriftapi.HostingFundPage> getSubAccountFundHistory(LandingInfo landingInfo, xueqiao.trade.hosting.asset.thriftapi.ReqSubAccountFundHistoryOption option, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "getSubAccountFundHistory";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("option", option));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            xueqiao.trade.hosting.asset.thriftapi.HostingFundPage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.getSubAccountFundHistory(platformArgs, landingInfo, option, pageOption);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.asset.thriftapi.SettlementPositionDetailPage>> getSubAccountPositionHistoryAsync(LandingInfo landingInfo, xueqiao.trade.hosting.asset.thriftapi.ReqSettlementPositionDetailOption option, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => getSubAccountPositionHistory(landingInfo, option, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.asset.thriftapi.SettlementPositionDetailPage> getSubAccountPositionHistory(LandingInfo landingInfo, xueqiao.trade.hosting.asset.thriftapi.ReqSettlementPositionDetailOption option, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "getSubAccountPositionHistory";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("option", option));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            xueqiao.trade.hosting.asset.thriftapi.SettlementPositionDetailPage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.getSubAccountPositionHistory(platformArgs, landingInfo, option, pageOption);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.asset.thriftapi.AssetTradeDetailPage>> getSubAccountPositionHistoryTradeDetailAsync(LandingInfo landingInfo, xueqiao.trade.hosting.asset.thriftapi.ReqSettlementPositionTradeDetailOption option, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => getSubAccountPositionHistoryTradeDetail(landingInfo, option, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.asset.thriftapi.AssetTradeDetailPage> getSubAccountPositionHistoryTradeDetail(LandingInfo landingInfo, xueqiao.trade.hosting.asset.thriftapi.ReqSettlementPositionTradeDetailOption option, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "getSubAccountPositionHistoryTradeDetail";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("option", option));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            xueqiao.trade.hosting.asset.thriftapi.AssetTradeDetailPage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.getSubAccountPositionHistoryTradeDetail(platformArgs, landingInfo, option, pageOption);
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

        public async Task<IInterfaceInteractResponse> deleteExpiredContractPositionAsync(LandingInfo landingInfo, long subAccountId, long sledContractId, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => deleteExpiredContractPosition(landingInfo, subAccountId, sledContractId, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse deleteExpiredContractPosition(LandingInfo landingInfo, long subAccountId, long sledContractId, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "deleteExpiredContractPosition";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("subAccountId", subAccountId));
            argsList.Add(new Tuple<string, object>("sledContractId", sledContractId));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                client.deleteExpiredContractPosition(platformArgs, landingInfo, subAccountId, sledContractId);
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

        public async Task<IInterfaceInteractResponse<List<HostingTAFundItem>>> getTradeAccountFundNowAsync(LandingInfo landingInfo, long tradeAccountId, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => getTradeAccountFundNow(landingInfo, tradeAccountId, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<List<HostingTAFundItem>> getTradeAccountFundNow(LandingInfo landingInfo, long tradeAccountId, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "getTradeAccountFundNow";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("tradeAccountId", tradeAccountId));
            List<HostingTAFundItem> result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.getTradeAccountFundNow(platformArgs, landingInfo, tradeAccountId);
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

        public async Task<IInterfaceInteractResponse<List<HostingTAFundHisItem>>> getTradeAccountFundHisAsync(LandingInfo landingInfo, long tradeAccountId, string fundDateBegin, string fundDateEnd, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => getTradeAccountFundHis(landingInfo, tradeAccountId, fundDateBegin, fundDateEnd, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<List<HostingTAFundHisItem>> getTradeAccountFundHis(LandingInfo landingInfo, long tradeAccountId, string fundDateBegin, string fundDateEnd, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "getTradeAccountFundHis";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("tradeAccountId", tradeAccountId));
            argsList.Add(new Tuple<string, object>("fundDateBegin", fundDateBegin));
            argsList.Add(new Tuple<string, object>("fundDateEnd", fundDateEnd));
            List<HostingTAFundHisItem> result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.getTradeAccountFundHis(platformArgs, landingInfo, tradeAccountId, fundDateBegin, fundDateEnd);
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

        public async Task<IInterfaceInteractResponse<List<xueqiao.trade.hosting.tradeaccount.data.TradeAccountSettlementInfo>>> getTradeAccountSettlementInfosAsync(LandingInfo landingInfo, long tradeAccountId, string settlementDateBegin, string settlementDateEnd, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => getTradeAccountSettlementInfos(landingInfo, tradeAccountId, settlementDateBegin, settlementDateEnd, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<List<xueqiao.trade.hosting.tradeaccount.data.TradeAccountSettlementInfo>> getTradeAccountSettlementInfos(LandingInfo landingInfo, long tradeAccountId, string settlementDateBegin, string settlementDateEnd, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "getTradeAccountSettlementInfos";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("tradeAccountId", tradeAccountId));
            argsList.Add(new Tuple<string, object>("settlementDateBegin", settlementDateBegin));
            argsList.Add(new Tuple<string, object>("settlementDateEnd", settlementDateEnd));
            List<xueqiao.trade.hosting.tradeaccount.data.TradeAccountSettlementInfo> result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.getTradeAccountSettlementInfos(platformArgs, landingInfo, tradeAccountId, settlementDateBegin, settlementDateEnd);
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

        public async Task<IInterfaceInteractResponse<List<TradeAccountSettlementInfoWithRelatedTime>>> getTradeAccountSettlementInfosWithRelatedTimeAsync(LandingInfo landingInfo, long tradeAccountId, string settlementDateBegin, string settlementDateEnd, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => getTradeAccountSettlementInfosWithRelatedTime(landingInfo, tradeAccountId, settlementDateBegin, settlementDateEnd, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<List<TradeAccountSettlementInfoWithRelatedTime>> getTradeAccountSettlementInfosWithRelatedTime(LandingInfo landingInfo, long tradeAccountId, string settlementDateBegin, string settlementDateEnd, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "getTradeAccountSettlementInfosWithRelatedTime";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("tradeAccountId", tradeAccountId));
            argsList.Add(new Tuple<string, object>("settlementDateBegin", settlementDateBegin));
            argsList.Add(new Tuple<string, object>("settlementDateEnd", settlementDateEnd));
            List<TradeAccountSettlementInfoWithRelatedTime> result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.getTradeAccountSettlementInfosWithRelatedTime(platformArgs, landingInfo, tradeAccountId, settlementDateBegin, settlementDateEnd);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.asset.thriftapi.AssetTradeDetailPage>> getTradeAccountPositionTradeDetailAsync(LandingInfo landingInfo, ReqTradeAccountPositionOption option, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => getTradeAccountPositionTradeDetail(landingInfo, option, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.asset.thriftapi.AssetTradeDetailPage> getTradeAccountPositionTradeDetail(LandingInfo landingInfo, ReqTradeAccountPositionOption option, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "getTradeAccountPositionTradeDetail";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("option", option));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            xueqiao.trade.hosting.asset.thriftapi.AssetTradeDetailPage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.getTradeAccountPositionTradeDetail(platformArgs, landingInfo, option, pageOption);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.position.adjust.thriftapi.PositionVerifyPage>> reqPositionVerifyAsync(LandingInfo landingInfo, xueqiao.trade.hosting.position.adjust.thriftapi.ReqPositionVerifyOption option, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => reqPositionVerify(landingInfo, option, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.position.adjust.thriftapi.PositionVerifyPage> reqPositionVerify(LandingInfo landingInfo, xueqiao.trade.hosting.position.adjust.thriftapi.ReqPositionVerifyOption option, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "reqPositionVerify";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("option", option));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            xueqiao.trade.hosting.position.adjust.thriftapi.PositionVerifyPage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.reqPositionVerify(platformArgs, landingInfo, option, pageOption);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.position.adjust.thriftapi.PositionDifferencePage>> reqPositionDifferenceAsync(LandingInfo landingInfo, xueqiao.trade.hosting.position.adjust.thriftapi.ReqPositionDifferenceOption option, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => reqPositionDifference(landingInfo, option, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.position.adjust.thriftapi.PositionDifferencePage> reqPositionDifference(LandingInfo landingInfo, xueqiao.trade.hosting.position.adjust.thriftapi.ReqPositionDifferenceOption option, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "reqPositionDifference";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("option", option));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            xueqiao.trade.hosting.position.adjust.thriftapi.PositionDifferencePage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.reqPositionDifference(platformArgs, landingInfo, option, pageOption);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.position.adjust.thriftapi.ManualInputPositionResp>> manualInputPositionAsync(LandingInfo landingInfo, List<xueqiao.trade.hosting.position.adjust.thriftapi.PositionManualInput> positionManualInputs, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => manualInputPosition(landingInfo, positionManualInputs, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.position.adjust.thriftapi.ManualInputPositionResp> manualInputPosition(LandingInfo landingInfo, List<xueqiao.trade.hosting.position.adjust.thriftapi.PositionManualInput> positionManualInputs, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "manualInputPosition";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("positionManualInputs", positionManualInputs));
            xueqiao.trade.hosting.position.adjust.thriftapi.ManualInputPositionResp result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.manualInputPosition(platformArgs, landingInfo, positionManualInputs);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.position.adjust.thriftapi.PositionUnassignedPage>> reqPositionUnassignedAsync(LandingInfo landingInfo, xueqiao.trade.hosting.position.adjust.thriftapi.ReqPositionUnassignedOption option, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => reqPositionUnassigned(landingInfo, option, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.position.adjust.thriftapi.PositionUnassignedPage> reqPositionUnassigned(LandingInfo landingInfo, xueqiao.trade.hosting.position.adjust.thriftapi.ReqPositionUnassignedOption option, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "reqPositionUnassigned";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("option", option));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            xueqiao.trade.hosting.position.adjust.thriftapi.PositionUnassignedPage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.reqPositionUnassigned(platformArgs, landingInfo, option, pageOption);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.position.adjust.assign.thriftapi.AssignPositionResp>> assignPositionAsync(LandingInfo landingInfo, List<xueqiao.trade.hosting.position.adjust.thriftapi.PositionAssignOption> assignOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => assignPosition(landingInfo, assignOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.position.adjust.assign.thriftapi.AssignPositionResp> assignPosition(LandingInfo landingInfo, List<xueqiao.trade.hosting.position.adjust.thriftapi.PositionAssignOption> assignOption, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "assignPosition";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("assignOption", assignOption));
            xueqiao.trade.hosting.position.adjust.assign.thriftapi.AssignPositionResp result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.assignPosition(platformArgs, landingInfo, assignOption);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.position.adjust.thriftapi.PositionEditLock>> reqPositionEditLockAsync(LandingInfo landingInfo, string lockKey, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => reqPositionEditLock(landingInfo, lockKey, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.position.adjust.thriftapi.PositionEditLock> reqPositionEditLock(LandingInfo landingInfo, string lockKey, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "reqPositionEditLock";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("lockKey", lockKey));
            xueqiao.trade.hosting.position.adjust.thriftapi.PositionEditLock result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.reqPositionEditLock(platformArgs, landingInfo, lockKey);
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

        public async Task<IInterfaceInteractResponse> addPositionEditLockAsync(LandingInfo landingInfo, xueqiao.trade.hosting.position.adjust.thriftapi.PositionEditLock positionEditLock, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => addPositionEditLock(landingInfo, positionEditLock, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse addPositionEditLock(LandingInfo landingInfo, xueqiao.trade.hosting.position.adjust.thriftapi.PositionEditLock positionEditLock, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "addPositionEditLock";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("positionEditLock", positionEditLock));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                client.addPositionEditLock(platformArgs, landingInfo, positionEditLock);
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

        public async Task<IInterfaceInteractResponse> removePositionEditLockAsync(LandingInfo landingInfo, xueqiao.trade.hosting.position.adjust.thriftapi.PositionEditLock positionEditLock, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => removePositionEditLock(landingInfo, positionEditLock, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse removePositionEditLock(LandingInfo landingInfo, xueqiao.trade.hosting.position.adjust.thriftapi.PositionEditLock positionEditLock, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "removePositionEditLock";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("positionEditLock", positionEditLock));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                client.removePositionEditLock(platformArgs, landingInfo, positionEditLock);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.position.adjust.thriftapi.DailyPositionDifferencePage>> reqDailyPositionDifferenceAsync(LandingInfo landingInfo, xueqiao.trade.hosting.position.adjust.thriftapi.ReqDailyPositionDifferenceOption option, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => reqDailyPositionDifference(landingInfo, option, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.position.adjust.thriftapi.DailyPositionDifferencePage> reqDailyPositionDifference(LandingInfo landingInfo, xueqiao.trade.hosting.position.adjust.thriftapi.ReqDailyPositionDifferenceOption option, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "reqDailyPositionDifference";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("option", option));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            xueqiao.trade.hosting.position.adjust.thriftapi.DailyPositionDifferencePage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.reqDailyPositionDifference(platformArgs, landingInfo, option, pageOption);
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

        public async Task<IInterfaceInteractResponse> updateDailyPositionDifferenceNoteAsync(LandingInfo landingInfo, xueqiao.trade.hosting.position.adjust.thriftapi.DailyPositionDifference difference, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => updateDailyPositionDifferenceNote(landingInfo, difference, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse updateDailyPositionDifferenceNote(LandingInfo landingInfo, xueqiao.trade.hosting.position.adjust.thriftapi.DailyPositionDifference difference, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "updateDailyPositionDifferenceNote";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("difference", difference));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                client.updateDailyPositionDifferenceNote(platformArgs, landingInfo, difference);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.position.adjust.thriftapi.PositionAssignedPage>> reqPositionAssignedAsync(LandingInfo landingInfo, xueqiao.trade.hosting.position.adjust.thriftapi.ReqPositionAssignedOption option, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => reqPositionAssigned(landingInfo, option, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.position.adjust.thriftapi.PositionAssignedPage> reqPositionAssigned(LandingInfo landingInfo, xueqiao.trade.hosting.position.adjust.thriftapi.ReqPositionAssignedOption option, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "reqPositionAssigned";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("option", option));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            xueqiao.trade.hosting.position.adjust.thriftapi.PositionAssignedPage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.reqPositionAssigned(platformArgs, landingInfo, option, pageOption);
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

        public async Task<IInterfaceInteractResponse> contructComposeAsync(LandingInfo landingInfo, xueqiao.trade.hosting.position.statis.StatContructComposeReq contructComposeReq, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => contructCompose(landingInfo, contructComposeReq, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse contructCompose(LandingInfo landingInfo, xueqiao.trade.hosting.position.statis.StatContructComposeReq contructComposeReq, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "contructCompose";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("contructComposeReq", contructComposeReq));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                client.contructCompose(platformArgs, landingInfo, contructComposeReq);
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

        public async Task<IInterfaceInteractResponse> disassembleComposeAsync(LandingInfo landingInfo, xueqiao.trade.hosting.position.statis.DisassembleComposePositionReq disassembleComposePositionReq, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => disassembleCompose(landingInfo, disassembleComposePositionReq, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse disassembleCompose(LandingInfo landingInfo, xueqiao.trade.hosting.position.statis.DisassembleComposePositionReq disassembleComposePositionReq, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "disassembleCompose";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("disassembleComposePositionReq", disassembleComposePositionReq));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                client.disassembleCompose(platformArgs, landingInfo, disassembleComposePositionReq);
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

        public async Task<IInterfaceInteractResponse> batchClosePositionAsync(LandingInfo landingInfo, xueqiao.trade.hosting.position.statis.BatchClosedPositionReq batchClosedPositionReq, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => batchClosePosition(landingInfo, batchClosedPositionReq, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse batchClosePosition(LandingInfo landingInfo, xueqiao.trade.hosting.position.statis.BatchClosedPositionReq batchClosedPositionReq, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "batchClosePosition";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("batchClosedPositionReq", batchClosedPositionReq));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                client.batchClosePosition(platformArgs, landingInfo, batchClosedPositionReq);
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

        public async Task<IInterfaceInteractResponse> recoverClosedPositionAsync(LandingInfo landingInfo, long subAccountId, string targetKey, xueqiao.trade.hosting.arbitrage.thriftapi.HostingXQTargetType targetType, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => recoverClosedPosition(landingInfo, subAccountId, targetKey, targetType, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse recoverClosedPosition(LandingInfo landingInfo, long subAccountId, string targetKey, xueqiao.trade.hosting.arbitrage.thriftapi.HostingXQTargetType targetType, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "recoverClosedPosition";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("subAccountId", subAccountId));
            argsList.Add(new Tuple<string, object>("targetKey", targetKey));
            argsList.Add(new Tuple<string, object>("targetType", targetType));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                client.recoverClosedPosition(platformArgs, landingInfo, subAccountId, targetKey, targetType);
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

        public async Task<IInterfaceInteractResponse> mergeToComposeAsync(LandingInfo landingInfo, xueqiao.trade.hosting.position.statis.StatMergeToComposeReq mergeToComposeReq, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => mergeToCompose(landingInfo, mergeToComposeReq, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse mergeToCompose(LandingInfo landingInfo, xueqiao.trade.hosting.position.statis.StatMergeToComposeReq mergeToComposeReq, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "mergeToCompose";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("mergeToComposeReq", mergeToComposeReq));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                client.mergeToCompose(platformArgs, landingInfo, mergeToComposeReq);
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

        public async Task<IInterfaceInteractResponse> deleteExpiredStatContractPositionAsync(LandingInfo landingInfo, long subAccountId, long sledContractId, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => deleteExpiredStatContractPosition(landingInfo, subAccountId, sledContractId, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse deleteExpiredStatContractPosition(LandingInfo landingInfo, long subAccountId, long sledContractId, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "deleteExpiredStatContractPosition";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("subAccountId", subAccountId));
            argsList.Add(new Tuple<string, object>("sledContractId", sledContractId));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                client.deleteExpiredStatContractPosition(platformArgs, landingInfo, subAccountId, sledContractId);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.position.statis.StatPositionSummaryPage>> queryStatPositionSummaryPageAsync(LandingInfo landingInfo, xueqiao.trade.hosting.position.statis.QueryStatPositionSummaryOption queryOption, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => queryStatPositionSummaryPage(landingInfo, queryOption, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.position.statis.StatPositionSummaryPage> queryStatPositionSummaryPage(LandingInfo landingInfo, xueqiao.trade.hosting.position.statis.QueryStatPositionSummaryOption queryOption, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "queryStatPositionSummaryPage";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("queryOption", queryOption));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            xueqiao.trade.hosting.position.statis.StatPositionSummaryPage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.queryStatPositionSummaryPage(platformArgs, landingInfo, queryOption, pageOption);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.position.statis.StatPositionItemPage>> queryStatPositionItemPageAsync(LandingInfo landingInfo, xueqiao.trade.hosting.position.statis.QueryStatPositionItemOption queryOption, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => queryStatPositionItemPage(landingInfo, queryOption, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.position.statis.StatPositionItemPage> queryStatPositionItemPage(LandingInfo landingInfo, xueqiao.trade.hosting.position.statis.QueryStatPositionItemOption queryOption, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "queryStatPositionItemPage";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("queryOption", queryOption));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            xueqiao.trade.hosting.position.statis.StatPositionItemPage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.queryStatPositionItemPage(platformArgs, landingInfo, queryOption, pageOption);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.position.statis.StatClosedPositionDateSummaryPage>> queryCurrentDayStatClosedPositionPageAsync(LandingInfo landingInfo, long subAccountId, string targetKey, xueqiao.trade.hosting.arbitrage.thriftapi.HostingXQTargetType targetType, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => queryCurrentDayStatClosedPositionPage(landingInfo, subAccountId, targetKey, targetType, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.position.statis.StatClosedPositionDateSummaryPage> queryCurrentDayStatClosedPositionPage(LandingInfo landingInfo, long subAccountId, string targetKey, xueqiao.trade.hosting.arbitrage.thriftapi.HostingXQTargetType targetType, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "queryCurrentDayStatClosedPositionPage";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("subAccountId", subAccountId));
            argsList.Add(new Tuple<string, object>("targetKey", targetKey));
            argsList.Add(new Tuple<string, object>("targetType", targetType));
            xueqiao.trade.hosting.position.statis.StatClosedPositionDateSummaryPage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.queryCurrentDayStatClosedPositionPage(platformArgs, landingInfo, subAccountId, targetKey, targetType);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.position.statis.StatClosedPositionDetail>> queryStatClosedPositionDetailAsync(LandingInfo landingInfo, xueqiao.trade.hosting.position.statis.QueryStatClosedPositionItemOption queryOption, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => queryStatClosedPositionDetail(landingInfo, queryOption, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.position.statis.StatClosedPositionDetail> queryStatClosedPositionDetail(LandingInfo landingInfo, xueqiao.trade.hosting.position.statis.QueryStatClosedPositionItemOption queryOption, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "queryStatClosedPositionDetail";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("queryOption", queryOption));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            xueqiao.trade.hosting.position.statis.StatClosedPositionDetail result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.queryStatClosedPositionDetail(platformArgs, landingInfo, queryOption, pageOption);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.position.statis.StatClosedPositionDateSummaryPage>> queryArchivedClosedPositionPageAsync(LandingInfo landingInfo, xueqiao.trade.hosting.position.statis.QueryStatClosedPositionDateSummaryOption queryOption, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => queryArchivedClosedPositionPage(landingInfo, queryOption, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.position.statis.StatClosedPositionDateSummaryPage> queryArchivedClosedPositionPage(LandingInfo landingInfo, xueqiao.trade.hosting.position.statis.QueryStatClosedPositionDateSummaryOption queryOption, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "queryArchivedClosedPositionPage";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("queryOption", queryOption));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            xueqiao.trade.hosting.position.statis.StatClosedPositionDateSummaryPage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.queryArchivedClosedPositionPage(platformArgs, landingInfo, queryOption, pageOption);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.position.statis.StatClosedPositionDetail>> queryArchivedClosedPositionDetailAsync(LandingInfo landingInfo, xueqiao.trade.hosting.position.statis.QueryStatArchiveItemOption queryOption, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => queryArchivedClosedPositionDetail(landingInfo, queryOption, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.position.statis.StatClosedPositionDetail> queryArchivedClosedPositionDetail(LandingInfo landingInfo, xueqiao.trade.hosting.position.statis.QueryStatArchiveItemOption queryOption, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "queryArchivedClosedPositionDetail";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("queryOption", queryOption));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            xueqiao.trade.hosting.position.statis.StatClosedPositionDetail result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.queryArchivedClosedPositionDetail(platformArgs, landingInfo, queryOption, pageOption);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.position.statis.StatPositionSummaryExPage>> queryStatPositionSummaryExPageAsync(LandingInfo landingInfo, xueqiao.trade.hosting.position.statis.QueryStatPositionSummaryOption queryOption, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => queryStatPositionSummaryExPage(landingInfo, queryOption, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.position.statis.StatPositionSummaryExPage> queryStatPositionSummaryExPage(LandingInfo landingInfo, xueqiao.trade.hosting.position.statis.QueryStatPositionSummaryOption queryOption, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "queryStatPositionSummaryExPage";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("queryOption", queryOption));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            xueqiao.trade.hosting.position.statis.StatPositionSummaryExPage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.queryStatPositionSummaryExPage(platformArgs, landingInfo, queryOption, pageOption);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.position.statis.StatPositionUnitPage>> queryStatPositionUnitPageAsync(LandingInfo landingInfo, xueqiao.trade.hosting.position.statis.QueryStatPositionUnitOption queryOption, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => queryStatPositionUnitPage(landingInfo, queryOption, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.position.statis.StatPositionUnitPage> queryStatPositionUnitPage(LandingInfo landingInfo, xueqiao.trade.hosting.position.statis.QueryStatPositionUnitOption queryOption, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "queryStatPositionUnitPage";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("queryOption", queryOption));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            xueqiao.trade.hosting.position.statis.StatPositionUnitPage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.queryStatPositionUnitPage(platformArgs, landingInfo, queryOption, pageOption);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.position.statis.StatClosedPositionDateSummaryPage>> queryHistoryClosedPositionPageAsync(LandingInfo landingInfo, xueqiao.trade.hosting.position.statis.QueryHistoryClosedPositionOption queryOption, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => queryHistoryClosedPositionPage(landingInfo, queryOption, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.position.statis.StatClosedPositionDateSummaryPage> queryHistoryClosedPositionPage(LandingInfo landingInfo, xueqiao.trade.hosting.position.statis.QueryHistoryClosedPositionOption queryOption, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "queryHistoryClosedPositionPage";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("queryOption", queryOption));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            xueqiao.trade.hosting.position.statis.StatClosedPositionDateSummaryPage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.queryHistoryClosedPositionPage(platformArgs, landingInfo, queryOption, pageOption);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.position.statis.StatClosedPositionDetail>> queryHistoryClosedPositionDetailAsync(LandingInfo landingInfo, xueqiao.trade.hosting.position.statis.QueryHistoryClosedPositionOption queryOption, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => queryHistoryClosedPositionDetail(landingInfo, queryOption, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.position.statis.StatClosedPositionDetail> queryHistoryClosedPositionDetail(LandingInfo landingInfo, xueqiao.trade.hosting.position.statis.QueryHistoryClosedPositionOption queryOption, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "queryHistoryClosedPositionDetail";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("queryOption", queryOption));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            xueqiao.trade.hosting.position.statis.StatClosedPositionDetail result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.queryHistoryClosedPositionDetail(platformArgs, landingInfo, queryOption, pageOption);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.tasknote.thriftapi.HostingTaskNotePage>> getXQTradeLameTaskNotePageAsync(LandingInfo landingInfo, QueryXQTradeLameTaskNotePageOption qryOption, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => getXQTradeLameTaskNotePage(landingInfo, qryOption, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.tasknote.thriftapi.HostingTaskNotePage> getXQTradeLameTaskNotePage(LandingInfo landingInfo, QueryXQTradeLameTaskNotePageOption qryOption, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "getXQTradeLameTaskNotePage";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("qryOption", qryOption));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            xueqiao.trade.hosting.tasknote.thriftapi.HostingTaskNotePage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.getXQTradeLameTaskNotePage(platformArgs, landingInfo, qryOption, pageOption);
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

        public async Task<IInterfaceInteractResponse<Dictionary<long, ErrorInfo>>> batchDeleteXQTradeLameTaskNotesAsync(LandingInfo landingInfo, long subAccountId, THashSet<long> xqTradeIds, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => batchDeleteXQTradeLameTaskNotes(landingInfo, subAccountId, xqTradeIds, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<Dictionary<long, ErrorInfo>> batchDeleteXQTradeLameTaskNotes(LandingInfo landingInfo, long subAccountId, THashSet<long> xqTradeIds, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "batchDeleteXQTradeLameTaskNotes";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("subAccountId", subAccountId));
            argsList.Add(new Tuple<string, object>("xqTradeIds", xqTradeIds));
            Dictionary<long, ErrorInfo> result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.batchDeleteXQTradeLameTaskNotes(platformArgs, landingInfo, subAccountId, xqTradeIds);
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

        public async Task<IInterfaceInteractResponse<xueqiao.mailbox.user.message.thriftapi.UserMessagePage>> queryMailBoxMessageAsync(LandingInfo landingInfo, ReqMailBoxMessageOption option, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => queryMailBoxMessage(landingInfo, option, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.mailbox.user.message.thriftapi.UserMessagePage> queryMailBoxMessage(LandingInfo landingInfo, ReqMailBoxMessageOption option, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
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
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
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
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.queryMailBoxMessage(platformArgs, landingInfo, option, pageOption);
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

        public async Task<IInterfaceInteractResponse<bool>> markMessageAsReadAsync(LandingInfo landingInfo, THashSet<long> hostingMessageIds, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => markMessageAsRead(landingInfo, hostingMessageIds, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<bool> markMessageAsRead(LandingInfo landingInfo, THashSet<long> hostingMessageIds, StubInterfaceInteractParams interactParams = null)
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
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("hostingMessageIds", hostingMessageIds));
            bool result = false;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.markMessageAsRead(platformArgs, landingInfo, hostingMessageIds);
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

        public async Task<IInterfaceInteractResponse<List<xueqiao.trade.hosting.risk.manager.thriftapi.HostingRiskSupportedItem>>> getAllSupportedItemsAsync(LandingInfo landingInfo, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => getAllSupportedItems(landingInfo, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<List<xueqiao.trade.hosting.risk.manager.thriftapi.HostingRiskSupportedItem>> getAllSupportedItems(LandingInfo landingInfo, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "getAllSupportedItems";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            List<xueqiao.trade.hosting.risk.manager.thriftapi.HostingRiskSupportedItem> result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.getAllSupportedItems(platformArgs, landingInfo);
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

        public async Task<IInterfaceInteractResponse<int>> getRiskRuleJointVersionAsync(LandingInfo landingInfo, long subAccountId, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => getRiskRuleJointVersion(landingInfo, subAccountId, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<int> getRiskRuleJointVersion(LandingInfo landingInfo, long subAccountId, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "getRiskRuleJointVersion";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("subAccountId", subAccountId));
            int result = 0;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.getRiskRuleJointVersion(platformArgs, landingInfo, subAccountId);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.risk.manager.thriftapi.HostingRiskRuleJoint>> getRiskRuleJointAsync(LandingInfo landingInfo, long subAccountId, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => getRiskRuleJoint(landingInfo, subAccountId, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.risk.manager.thriftapi.HostingRiskRuleJoint> getRiskRuleJoint(LandingInfo landingInfo, long subAccountId, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "getRiskRuleJoint";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("subAccountId", subAccountId));
            xueqiao.trade.hosting.risk.manager.thriftapi.HostingRiskRuleJoint result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.getRiskRuleJoint(platformArgs, landingInfo, subAccountId);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.risk.manager.thriftapi.HostingRiskRuleJoint>> batchSetSupportedItemsAsync(LandingInfo landingInfo, long subAccountId, int version, THashSet<string> openedItemIds, THashSet<string> closedItemIds, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => batchSetSupportedItems(landingInfo, subAccountId, version, openedItemIds, closedItemIds, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.risk.manager.thriftapi.HostingRiskRuleJoint> batchSetSupportedItems(LandingInfo landingInfo, long subAccountId, int version, THashSet<string> openedItemIds, THashSet<string> closedItemIds, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "batchSetSupportedItems";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("subAccountId", subAccountId));
            argsList.Add(new Tuple<string, object>("version", version));
            argsList.Add(new Tuple<string, object>("openedItemIds", openedItemIds));
            argsList.Add(new Tuple<string, object>("closedItemIds", closedItemIds));
            xueqiao.trade.hosting.risk.manager.thriftapi.HostingRiskRuleJoint result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.batchSetSupportedItems(platformArgs, landingInfo, subAccountId, version, openedItemIds, closedItemIds);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.risk.manager.thriftapi.HostingRiskRuleJoint>> batchSetTradedCommodityItemsAsync(LandingInfo landingInfo, long subAccountId, int version, THashSet<long> enabledCommodityIds, THashSet<long> disabledCommodityIds, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => batchSetTradedCommodityItems(landingInfo, subAccountId, version, enabledCommodityIds, disabledCommodityIds, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.risk.manager.thriftapi.HostingRiskRuleJoint> batchSetTradedCommodityItems(LandingInfo landingInfo, long subAccountId, int version, THashSet<long> enabledCommodityIds, THashSet<long> disabledCommodityIds, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "batchSetTradedCommodityItems";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("subAccountId", subAccountId));
            argsList.Add(new Tuple<string, object>("version", version));
            argsList.Add(new Tuple<string, object>("enabledCommodityIds", enabledCommodityIds));
            argsList.Add(new Tuple<string, object>("disabledCommodityIds", disabledCommodityIds));
            xueqiao.trade.hosting.risk.manager.thriftapi.HostingRiskRuleJoint result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.batchSetTradedCommodityItems(platformArgs, landingInfo, subAccountId, version, enabledCommodityIds, disabledCommodityIds);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.risk.manager.thriftapi.HostingRiskRuleJoint>> batchSetGlobalRulesAsync(LandingInfo landingInfo, long subAccountId, int version, Dictionary<string, xueqiao.trade.hosting.risk.manager.thriftapi.HostingRiskRuleItem> ruleItems, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => batchSetGlobalRules(landingInfo, subAccountId, version, ruleItems, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.risk.manager.thriftapi.HostingRiskRuleJoint> batchSetGlobalRules(LandingInfo landingInfo, long subAccountId, int version, Dictionary<string, xueqiao.trade.hosting.risk.manager.thriftapi.HostingRiskRuleItem> ruleItems, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "batchSetGlobalRules";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("subAccountId", subAccountId));
            argsList.Add(new Tuple<string, object>("version", version));
            argsList.Add(new Tuple<string, object>("ruleItems", ruleItems));
            xueqiao.trade.hosting.risk.manager.thriftapi.HostingRiskRuleJoint result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.batchSetGlobalRules(platformArgs, landingInfo, subAccountId, version, ruleItems);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.risk.manager.thriftapi.HostingRiskRuleJoint>> batchSetCommodityRulesAsync(LandingInfo landingInfo, long subAccountId, int version, Dictionary<long, Dictionary<string, xueqiao.trade.hosting.risk.manager.thriftapi.HostingRiskRuleItem>> rules, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => batchSetCommodityRules(landingInfo, subAccountId, version, rules, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.risk.manager.thriftapi.HostingRiskRuleJoint> batchSetCommodityRules(LandingInfo landingInfo, long subAccountId, int version, Dictionary<long, Dictionary<string, xueqiao.trade.hosting.risk.manager.thriftapi.HostingRiskRuleItem>> rules, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "batchSetCommodityRules";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("subAccountId", subAccountId));
            argsList.Add(new Tuple<string, object>("version", version));
            argsList.Add(new Tuple<string, object>("rules", rules));
            xueqiao.trade.hosting.risk.manager.thriftapi.HostingRiskRuleJoint result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.batchSetCommodityRules(platformArgs, landingInfo, subAccountId, version, rules);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.risk.manager.thriftapi.HostingRiskRuleJoint>> setRiskEnabledAsync(LandingInfo landingInfo, long subAccountId, int version, bool riskEnabled, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => setRiskEnabled(landingInfo, subAccountId, version, riskEnabled, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.risk.manager.thriftapi.HostingRiskRuleJoint> setRiskEnabled(LandingInfo landingInfo, long subAccountId, int version, bool riskEnabled, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "setRiskEnabled";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("subAccountId", subAccountId));
            argsList.Add(new Tuple<string, object>("version", version));
            argsList.Add(new Tuple<string, object>("riskEnabled", riskEnabled));
            xueqiao.trade.hosting.risk.manager.thriftapi.HostingRiskRuleJoint result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.setRiskEnabled(platformArgs, landingInfo, subAccountId, version, riskEnabled);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.risk.manager.thriftapi.HostingRiskFrameDataInfo>> getRiskFrameDataInfoAsync(LandingInfo landingInfo, long subAccountId, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => getRiskFrameDataInfo(landingInfo, subAccountId, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.risk.manager.thriftapi.HostingRiskFrameDataInfo> getRiskFrameDataInfo(LandingInfo landingInfo, long subAccountId, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "getRiskFrameDataInfo";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("subAccountId", subAccountId));
            xueqiao.trade.hosting.risk.manager.thriftapi.HostingRiskFrameDataInfo result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.getRiskFrameDataInfo(platformArgs, landingInfo, subAccountId);
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

        public async Task<IInterfaceInteractResponse> setGeneralMarginSettingAsync(LandingInfo landingInfo, xueqiao.trade.hosting.position.fee.thriftapi.XQGeneralMarginSettings marginSettings, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => setGeneralMarginSetting(landingInfo, marginSettings, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse setGeneralMarginSetting(LandingInfo landingInfo, xueqiao.trade.hosting.position.fee.thriftapi.XQGeneralMarginSettings marginSettings, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "setGeneralMarginSetting";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("marginSettings", marginSettings));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                client.setGeneralMarginSetting(platformArgs, landingInfo, marginSettings);
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

        public async Task<IInterfaceInteractResponse> setGeneralCommissionSettingAsync(LandingInfo landingInfo, xueqiao.trade.hosting.position.fee.thriftapi.XQGeneralCommissionSettings commissionSettings, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => setGeneralCommissionSetting(landingInfo, commissionSettings, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse setGeneralCommissionSetting(LandingInfo landingInfo, xueqiao.trade.hosting.position.fee.thriftapi.XQGeneralCommissionSettings commissionSettings, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "setGeneralCommissionSetting";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("commissionSettings", commissionSettings));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                client.setGeneralCommissionSetting(platformArgs, landingInfo, commissionSettings);
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

        public async Task<IInterfaceInteractResponse> addSpecMarginSettingAsync(LandingInfo landingInfo, xueqiao.trade.hosting.position.fee.thriftapi.XQSpecMarginSettings marginSettings, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => addSpecMarginSetting(landingInfo, marginSettings, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse addSpecMarginSetting(LandingInfo landingInfo, xueqiao.trade.hosting.position.fee.thriftapi.XQSpecMarginSettings marginSettings, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "addSpecMarginSetting";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("marginSettings", marginSettings));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                client.addSpecMarginSetting(platformArgs, landingInfo, marginSettings);
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

        public async Task<IInterfaceInteractResponse> addSpecCommissionSettingAsync(LandingInfo landingInfo, xueqiao.trade.hosting.position.fee.thriftapi.XQSpecCommissionSettings commissionSettings, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => addSpecCommissionSetting(landingInfo, commissionSettings, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse addSpecCommissionSetting(LandingInfo landingInfo, xueqiao.trade.hosting.position.fee.thriftapi.XQSpecCommissionSettings commissionSettings, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "addSpecCommissionSetting";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("commissionSettings", commissionSettings));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                client.addSpecCommissionSetting(platformArgs, landingInfo, commissionSettings);
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

        public async Task<IInterfaceInteractResponse> updateSpecMarginSettingAsync(LandingInfo landingInfo, xueqiao.trade.hosting.position.fee.thriftapi.XQSpecMarginSettings marginSettings, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => updateSpecMarginSetting(landingInfo, marginSettings, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse updateSpecMarginSetting(LandingInfo landingInfo, xueqiao.trade.hosting.position.fee.thriftapi.XQSpecMarginSettings marginSettings, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "updateSpecMarginSetting";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("marginSettings", marginSettings));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                client.updateSpecMarginSetting(platformArgs, landingInfo, marginSettings);
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

        public async Task<IInterfaceInteractResponse> updateSpecCommissionSettingAsync(LandingInfo landingInfo, xueqiao.trade.hosting.position.fee.thriftapi.XQSpecCommissionSettings commissionSettings, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => updateSpecCommissionSetting(landingInfo, commissionSettings, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse updateSpecCommissionSetting(LandingInfo landingInfo, xueqiao.trade.hosting.position.fee.thriftapi.XQSpecCommissionSettings commissionSettings, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "updateSpecCommissionSetting";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("commissionSettings", commissionSettings));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                client.updateSpecCommissionSetting(platformArgs, landingInfo, commissionSettings);
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

        public async Task<IInterfaceInteractResponse> deleteSpecMarginSettingAsync(LandingInfo landingInfo, long subAccountId, long commodityId, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => deleteSpecMarginSetting(landingInfo, subAccountId, commodityId, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse deleteSpecMarginSetting(LandingInfo landingInfo, long subAccountId, long commodityId, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "deleteSpecMarginSetting";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("subAccountId", subAccountId));
            argsList.Add(new Tuple<string, object>("commodityId", commodityId));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                client.deleteSpecMarginSetting(platformArgs, landingInfo, subAccountId, commodityId);
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

        public async Task<IInterfaceInteractResponse> deleteSpecCommissionSettingAsync(LandingInfo landingInfo, long subAccountId, long commodityId, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => deleteSpecCommissionSetting(landingInfo, subAccountId, commodityId, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse deleteSpecCommissionSetting(LandingInfo landingInfo, long subAccountId, long commodityId, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "deleteSpecCommissionSetting";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("subAccountId", subAccountId));
            argsList.Add(new Tuple<string, object>("commodityId", commodityId));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                client.deleteSpecCommissionSetting(platformArgs, landingInfo, subAccountId, commodityId);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.position.fee.thriftapi.XQGeneralMarginSettings>> queryXQGeneralMarginSettingsAsync(LandingInfo landingInfo, long subAccountId, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => queryXQGeneralMarginSettings(landingInfo, subAccountId, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.position.fee.thriftapi.XQGeneralMarginSettings> queryXQGeneralMarginSettings(LandingInfo landingInfo, long subAccountId, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "queryXQGeneralMarginSettings";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("subAccountId", subAccountId));
            xueqiao.trade.hosting.position.fee.thriftapi.XQGeneralMarginSettings result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.queryXQGeneralMarginSettings(platformArgs, landingInfo, subAccountId);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.position.fee.thriftapi.XQGeneralCommissionSettings>> queryXQGeneralCommissionSettingsAsync(LandingInfo landingInfo, long subAccountId, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => queryXQGeneralCommissionSettings(landingInfo, subAccountId, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.position.fee.thriftapi.XQGeneralCommissionSettings> queryXQGeneralCommissionSettings(LandingInfo landingInfo, long subAccountId, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "queryXQGeneralCommissionSettings";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("subAccountId", subAccountId));
            xueqiao.trade.hosting.position.fee.thriftapi.XQGeneralCommissionSettings result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.queryXQGeneralCommissionSettings(platformArgs, landingInfo, subAccountId);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.position.fee.thriftapi.XQSpecMarginSettingPage>> queryXQSpecMarginSettingPageAsync(LandingInfo landingInfo, xueqiao.trade.hosting.position.fee.thriftapi.QueryXQSpecSettingOptions queryOptions, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => queryXQSpecMarginSettingPage(landingInfo, queryOptions, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.position.fee.thriftapi.XQSpecMarginSettingPage> queryXQSpecMarginSettingPage(LandingInfo landingInfo, xueqiao.trade.hosting.position.fee.thriftapi.QueryXQSpecSettingOptions queryOptions, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "queryXQSpecMarginSettingPage";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("queryOptions", queryOptions));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            xueqiao.trade.hosting.position.fee.thriftapi.XQSpecMarginSettingPage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.queryXQSpecMarginSettingPage(platformArgs, landingInfo, queryOptions, pageOption);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.position.fee.thriftapi.XQSpecCommissionSettingPage>> queryXQSpecCommissionSettingPageAsync(LandingInfo landingInfo, xueqiao.trade.hosting.position.fee.thriftapi.QueryXQSpecSettingOptions queryOptions, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => queryXQSpecCommissionSettingPage(landingInfo, queryOptions, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.position.fee.thriftapi.XQSpecCommissionSettingPage> queryXQSpecCommissionSettingPage(LandingInfo landingInfo, xueqiao.trade.hosting.position.fee.thriftapi.QueryXQSpecSettingOptions queryOptions, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "queryXQSpecCommissionSettingPage";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("queryOptions", queryOptions));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            xueqiao.trade.hosting.position.fee.thriftapi.XQSpecCommissionSettingPage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.queryXQSpecCommissionSettingPage(platformArgs, landingInfo, queryOptions, pageOption);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.position.fee.thriftapi.UpsideContractMarginPage>> queryUpsideContractMarginPageAsync(LandingInfo landingInfo, xueqiao.trade.hosting.position.fee.thriftapi.QueryUpsidePFeeOptions queryOptions, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => queryUpsideContractMarginPage(landingInfo, queryOptions, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.position.fee.thriftapi.UpsideContractMarginPage> queryUpsideContractMarginPage(LandingInfo landingInfo, xueqiao.trade.hosting.position.fee.thriftapi.QueryUpsidePFeeOptions queryOptions, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "queryUpsideContractMarginPage";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("queryOptions", queryOptions));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            xueqiao.trade.hosting.position.fee.thriftapi.UpsideContractMarginPage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.queryUpsideContractMarginPage(platformArgs, landingInfo, queryOptions, pageOption);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.position.fee.thriftapi.UpsideContractCommissionPage>> queryUpsideContractCommissionPageAsync(LandingInfo landingInfo, xueqiao.trade.hosting.position.fee.thriftapi.QueryUpsidePFeeOptions queryOptions, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => queryUpsideContractCommissionPage(landingInfo, queryOptions, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.position.fee.thriftapi.UpsideContractCommissionPage> queryUpsideContractCommissionPage(LandingInfo landingInfo, xueqiao.trade.hosting.position.fee.thriftapi.QueryUpsidePFeeOptions queryOptions, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "queryUpsideContractCommissionPage";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("queryOptions", queryOptions));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            xueqiao.trade.hosting.position.fee.thriftapi.UpsideContractCommissionPage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.queryUpsideContractCommissionPage(platformArgs, landingInfo, queryOptions, pageOption);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.position.fee.thriftapi.XQContractMarginPage>> queryXQContractMarginPageAsync(LandingInfo landingInfo, xueqiao.trade.hosting.position.fee.thriftapi.QueryXQPFeeOptions queryOptions, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => queryXQContractMarginPage(landingInfo, queryOptions, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.position.fee.thriftapi.XQContractMarginPage> queryXQContractMarginPage(LandingInfo landingInfo, xueqiao.trade.hosting.position.fee.thriftapi.QueryXQPFeeOptions queryOptions, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "queryXQContractMarginPage";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("queryOptions", queryOptions));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            xueqiao.trade.hosting.position.fee.thriftapi.XQContractMarginPage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.queryXQContractMarginPage(platformArgs, landingInfo, queryOptions, pageOption);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.position.fee.thriftapi.XQContractCommissionPage>> queryXQContractCommissionPageAsync(LandingInfo landingInfo, xueqiao.trade.hosting.position.fee.thriftapi.QueryXQPFeeOptions queryOptions, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => queryXQContractCommissionPage(landingInfo, queryOptions, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.position.fee.thriftapi.XQContractCommissionPage> queryXQContractCommissionPage(LandingInfo landingInfo, xueqiao.trade.hosting.position.fee.thriftapi.QueryXQPFeeOptions queryOptions, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "queryXQContractCommissionPage";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("queryOptions", queryOptions));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            xueqiao.trade.hosting.position.fee.thriftapi.XQContractCommissionPage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.queryXQContractCommissionPage(platformArgs, landingInfo, queryOptions, pageOption);
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

        public async Task<IInterfaceInteractResponse<long>> addAssetAccountWorkingOrderAsync(LandingInfo landingInfo, xueqiao.working.order.thriftapi.AssetAccount assetAccount, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => addAssetAccountWorkingOrder(landingInfo, assetAccount, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<long> addAssetAccountWorkingOrder(LandingInfo landingInfo, xueqiao.working.order.thriftapi.AssetAccount assetAccount, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "addAssetAccountWorkingOrder";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("assetAccount", assetAccount));
            long result = 0;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingTerminalAo.Client(protocol);
                transport.Open();
                result = client.addAssetAccountWorkingOrder(platformArgs, landingInfo, assetAccount);
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