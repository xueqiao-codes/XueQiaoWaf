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

namespace xueqiao.graph.xiaoha.chart.terminal.ao.thriftapi
{
    public class XiaohaChartTerminalAoHttpStub
    {
        public string Servant { get; private set; }

        public XiaohaChartTerminalAoHttpStub (string servant)
        {
            if (servant == null) throw new ArgumentNullException("`servant` can't be null.");
            this.Servant = servant;
        }

        public async Task<IInterfaceInteractResponse<XiaohaChartLandingInfo>> loginAsync(LoginReq loginReq, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => login(loginReq, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<XiaohaChartLandingInfo> login(LoginReq loginReq, StubInterfaceInteractParams interactParams = null)
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
            argsList.Add(new Tuple<string, object>("loginReq", loginReq));
            XiaohaChartLandingInfo result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new XiaohaChartTerminalAo.Client(protocol);
                transport.Open();
                result = client.login(platformArgs, loginReq);
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

        public async Task<IInterfaceInteractResponse> logoutAsync(XiaohaChartLandingInfo landingInfo, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => logout(landingInfo, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse logout(XiaohaChartLandingInfo landingInfo, StubInterfaceInteractParams interactParams = null)
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
                var client = new XiaohaChartTerminalAo.Client(protocol);
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

        public async Task<IInterfaceInteractResponse<XiaohaChartLandingInfo>> authorizeXueQiaoAppAsync(XueQiaoAppSession appSession, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => authorizeXueQiaoApp(appSession, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<XiaohaChartLandingInfo> authorizeXueQiaoApp(XueQiaoAppSession appSession, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "authorizeXueQiaoApp";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("appSession", appSession));
            XiaohaChartLandingInfo result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new XiaohaChartTerminalAo.Client(protocol);
                transport.Open();
                result = client.authorizeXueQiaoApp(platformArgs, appSession);
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

        public async Task<IInterfaceInteractResponse<XiaohaChartLandingInfo>> signUpAsync(SignUpReq signUpReq, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => signUp(signUpReq, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<XiaohaChartLandingInfo> signUp(SignUpReq signUpReq, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "signUp";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("signUpReq", signUpReq));
            XiaohaChartLandingInfo result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new XiaohaChartTerminalAo.Client(protocol);
                transport.Open();
                result = client.signUp(platformArgs, signUpReq);
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

        public async Task<IInterfaceInteractResponse> sendVerifyCodeAsync(string tel, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => sendVerifyCode(tel, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse sendVerifyCode(string tel, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "sendVerifyCode";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("tel", tel));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new XiaohaChartTerminalAo.Client(protocol);
                transport.Open();
                client.sendVerifyCode(platformArgs, tel);
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

        public async Task<IInterfaceInteractResponse<xueqiao.graph.xiaoha.chart.thriftapi.ChartFolderPage>> reqChartFolderAsync(xueqiao.graph.xiaoha.chart.thriftapi.ReqChartFolderOption option, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => reqChartFolder(option, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.graph.xiaoha.chart.thriftapi.ChartFolderPage> reqChartFolder(xueqiao.graph.xiaoha.chart.thriftapi.ReqChartFolderOption option, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "reqChartFolder";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("option", option));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            xueqiao.graph.xiaoha.chart.thriftapi.ChartFolderPage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new XiaohaChartTerminalAo.Client(protocol);
                transport.Open();
                result = client.reqChartFolder(platformArgs, option, pageOption);
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

        public async Task<IInterfaceInteractResponse<xueqiao.graph.xiaoha.chart.thriftapi.ChartPage>> reqChartAsync(xueqiao.graph.xiaoha.chart.thriftapi.ReqChartOption option, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => reqChart(option, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.graph.xiaoha.chart.thriftapi.ChartPage> reqChart(xueqiao.graph.xiaoha.chart.thriftapi.ReqChartOption option, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "reqChart";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("option", option));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            xueqiao.graph.xiaoha.chart.thriftapi.ChartPage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new XiaohaChartTerminalAo.Client(protocol);
                transport.Open();
                result = client.reqChart(platformArgs, option, pageOption);
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

        public async Task<IInterfaceInteractResponse<xueqiao.personal.user.thriftapi.FavoriteFolder>> addFavoriteFolderAsync(XiaohaChartLandingInfo landingInfo, xueqiao.personal.user.thriftapi.FavoriteFolder favoriteFolder, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => addFavoriteFolder(landingInfo, favoriteFolder, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.personal.user.thriftapi.FavoriteFolder> addFavoriteFolder(XiaohaChartLandingInfo landingInfo, xueqiao.personal.user.thriftapi.FavoriteFolder favoriteFolder, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "addFavoriteFolder";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("favoriteFolder", favoriteFolder));
            xueqiao.personal.user.thriftapi.FavoriteFolder result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new XiaohaChartTerminalAo.Client(protocol);
                transport.Open();
                result = client.addFavoriteFolder(platformArgs, landingInfo, favoriteFolder);
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

        public async Task<IInterfaceInteractResponse<List<xueqiao.personal.user.thriftapi.FavoriteFolder>>> reqFavoriteFolderAsync(XiaohaChartLandingInfo landingInfo, xueqiao.personal.user.thriftapi.ReqFavoriteFolderOption option, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => reqFavoriteFolder(landingInfo, option, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<List<xueqiao.personal.user.thriftapi.FavoriteFolder>> reqFavoriteFolder(XiaohaChartLandingInfo landingInfo, xueqiao.personal.user.thriftapi.ReqFavoriteFolderOption option, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "reqFavoriteFolder";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("option", option));
            List<xueqiao.personal.user.thriftapi.FavoriteFolder> result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new XiaohaChartTerminalAo.Client(protocol);
                transport.Open();
                result = client.reqFavoriteFolder(platformArgs, landingInfo, option);
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

        public async Task<IInterfaceInteractResponse> removeFavoriteFolderAsync(XiaohaChartLandingInfo landingInfo, long folderId, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => removeFavoriteFolder(landingInfo, folderId, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse removeFavoriteFolder(XiaohaChartLandingInfo landingInfo, long folderId, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "removeFavoriteFolder";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("folderId", folderId));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new XiaohaChartTerminalAo.Client(protocol);
                transport.Open();
                client.removeFavoriteFolder(platformArgs, landingInfo, folderId);
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

        public async Task<IInterfaceInteractResponse> renameFavoriteFolderAsync(XiaohaChartLandingInfo landingInfo, long folderId, string newName, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => renameFavoriteFolder(landingInfo, folderId, newName, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse renameFavoriteFolder(XiaohaChartLandingInfo landingInfo, long folderId, string newName, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "renameFavoriteFolder";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("folderId", folderId));
            argsList.Add(new Tuple<string, object>("newName", newName));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new XiaohaChartTerminalAo.Client(protocol);
                transport.Open();
                client.renameFavoriteFolder(platformArgs, landingInfo, folderId, newName);
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

        public async Task<IInterfaceInteractResponse> moveFavoriteFolderAsync(XiaohaChartLandingInfo landingInfo, long folderId, long parentFolderId, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => moveFavoriteFolder(landingInfo, folderId, parentFolderId, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse moveFavoriteFolder(XiaohaChartLandingInfo landingInfo, long folderId, long parentFolderId, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "moveFavoriteFolder";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("folderId", folderId));
            argsList.Add(new Tuple<string, object>("parentFolderId", parentFolderId));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new XiaohaChartTerminalAo.Client(protocol);
                transport.Open();
                client.moveFavoriteFolder(platformArgs, landingInfo, folderId, parentFolderId);
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

        public async Task<IInterfaceInteractResponse<xueqiao.personal.user.thriftapi.FavoriteChart>> addFavoriteChartAsync(XiaohaChartLandingInfo landingInfo, xueqiao.personal.user.thriftapi.FavoriteChart favoriteChart, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => addFavoriteChart(landingInfo, favoriteChart, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.personal.user.thriftapi.FavoriteChart> addFavoriteChart(XiaohaChartLandingInfo landingInfo, xueqiao.personal.user.thriftapi.FavoriteChart favoriteChart, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "addFavoriteChart";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("favoriteChart", favoriteChart));
            xueqiao.personal.user.thriftapi.FavoriteChart result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new XiaohaChartTerminalAo.Client(protocol);
                transport.Open();
                result = client.addFavoriteChart(platformArgs, landingInfo, favoriteChart);
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

        public async Task<IInterfaceInteractResponse<List<xueqiao.personal.user.thriftapi.FavoriteChart>>> reqFavoriteChartAsync(XiaohaChartLandingInfo landingInfo, xueqiao.personal.user.thriftapi.ReqFavoriteChartOption option, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => reqFavoriteChart(landingInfo, option, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<List<xueqiao.personal.user.thriftapi.FavoriteChart>> reqFavoriteChart(XiaohaChartLandingInfo landingInfo, xueqiao.personal.user.thriftapi.ReqFavoriteChartOption option, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "reqFavoriteChart";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("option", option));
            List<xueqiao.personal.user.thriftapi.FavoriteChart> result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new XiaohaChartTerminalAo.Client(protocol);
                transport.Open();
                result = client.reqFavoriteChart(platformArgs, landingInfo, option);
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

        public async Task<IInterfaceInteractResponse> removeFavoriteChartAsync(XiaohaChartLandingInfo landingInfo, long favoriteChartId, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => removeFavoriteChart(landingInfo, favoriteChartId, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse removeFavoriteChart(XiaohaChartLandingInfo landingInfo, long favoriteChartId, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "removeFavoriteChart";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("favoriteChartId", favoriteChartId));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new XiaohaChartTerminalAo.Client(protocol);
                transport.Open();
                client.removeFavoriteChart(platformArgs, landingInfo, favoriteChartId);
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

        public async Task<IInterfaceInteractResponse> moveFavoriteChartAsync(XiaohaChartLandingInfo landingInfo, long favoriteChartId, long parentFolderId, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => moveFavoriteChart(landingInfo, favoriteChartId, parentFolderId, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse moveFavoriteChart(XiaohaChartLandingInfo landingInfo, long favoriteChartId, long parentFolderId, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "moveFavoriteChart";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("favoriteChartId", favoriteChartId));
            argsList.Add(new Tuple<string, object>("parentFolderId", parentFolderId));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new XiaohaChartTerminalAo.Client(protocol);
                transport.Open();
                client.moveFavoriteChart(platformArgs, landingInfo, favoriteChartId, parentFolderId);
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

        public async Task<IInterfaceInteractResponse> linkExistAccountAsync(XiaohaChartLandingInfo landingInfo, string tel, string verifyCode, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => linkExistAccount(landingInfo, tel, verifyCode, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse linkExistAccount(XiaohaChartLandingInfo landingInfo, string tel, string verifyCode, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "linkExistAccount";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            argsList.Add(new Tuple<string, object>("tel", tel));
            argsList.Add(new Tuple<string, object>("verifyCode", verifyCode));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new XiaohaChartTerminalAo.Client(protocol);
                transport.Open();
                client.linkExistAccount(platformArgs, landingInfo, tel, verifyCode);
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

        public async Task<IInterfaceInteractResponse<LinkState>> getLinkStateAsync(XiaohaChartLandingInfo landingInfo, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => getLinkState(landingInfo, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<LinkState> getLinkState(XiaohaChartLandingInfo landingInfo, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "getLinkState";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("landingInfo", landingInfo));
            LinkState result = (LinkState)0;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new XiaohaChartTerminalAo.Client(protocol);
                transport.Open();
                result = client.getLinkState(platformArgs, landingInfo);
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