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

namespace xueqiao.trade.hosting.position.adjust.thriftapi
{
    public class TradeHostingPositionAdjustHttpStub
    {
        public string Servant { get; private set; }

        public TradeHostingPositionAdjustHttpStub (string servant)
        {
            if (servant == null) throw new ArgumentNullException("`servant` can't be null.");
            this.Servant = servant;
        }

        public async Task<IInterfaceInteractResponse<ManualInputPositionResp>> manualInputPositionAsync(List<PositionManualInput> positionManualInputs, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => manualInputPosition(positionManualInputs, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<ManualInputPositionResp> manualInputPosition(List<PositionManualInput> positionManualInputs, StubInterfaceInteractParams interactParams = null)
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
            argsList.Add(new Tuple<string, object>("positionManualInputs", positionManualInputs));
            ManualInputPositionResp result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingPositionAdjust.Client(protocol);
                transport.Open();
                result = client.manualInputPosition(platformArgs, positionManualInputs);
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

        public async Task<IInterfaceInteractResponse<PositionManualInputPage>> reqManualInputPositionAsync(ReqPositionManualInputOption option, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => reqManualInputPosition(option, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<PositionManualInputPage> reqManualInputPosition(ReqPositionManualInputOption option, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "reqManualInputPosition";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("option", option));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            PositionManualInputPage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingPositionAdjust.Client(protocol);
                transport.Open();
                result = client.reqManualInputPosition(platformArgs, option, pageOption);
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

        public async Task<IInterfaceInteractResponse<PositionUnassignedPage>> reqPositionUnassignedAsync(ReqPositionUnassignedOption option, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => reqPositionUnassigned(option, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<PositionUnassignedPage> reqPositionUnassigned(ReqPositionUnassignedOption option, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
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
            argsList.Add(new Tuple<string, object>("option", option));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            PositionUnassignedPage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingPositionAdjust.Client(protocol);
                transport.Open();
                result = client.reqPositionUnassigned(platformArgs, option, pageOption);
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

        public async Task<IInterfaceInteractResponse<PositionAssignedPage>> reqPositionAssignedAsync(ReqPositionAssignedOption option, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => reqPositionAssigned(option, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<PositionAssignedPage> reqPositionAssigned(ReqPositionAssignedOption option, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
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
            argsList.Add(new Tuple<string, object>("option", option));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            PositionAssignedPage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingPositionAdjust.Client(protocol);
                transport.Open();
                result = client.reqPositionAssigned(platformArgs, option, pageOption);
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

        public async Task<IInterfaceInteractResponse<xueqiao.trade.hosting.position.adjust.assign.thriftapi.AssignPositionResp>> assignPositionAsync(List<PositionAssignOption> assignOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => assignPosition(assignOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<xueqiao.trade.hosting.position.adjust.assign.thriftapi.AssignPositionResp> assignPosition(List<PositionAssignOption> assignOption, StubInterfaceInteractParams interactParams = null)
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
            argsList.Add(new Tuple<string, object>("assignOption", assignOption));
            xueqiao.trade.hosting.position.adjust.assign.thriftapi.AssignPositionResp result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingPositionAdjust.Client(protocol);
                transport.Open();
                result = client.assignPosition(platformArgs, assignOption);
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

        public async Task<IInterfaceInteractResponse<PositionEditLock>> reqPositionEditLockAsync(string lockKey, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => reqPositionEditLock(lockKey, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<PositionEditLock> reqPositionEditLock(string lockKey, StubInterfaceInteractParams interactParams = null)
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
            argsList.Add(new Tuple<string, object>("lockKey", lockKey));
            PositionEditLock result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingPositionAdjust.Client(protocol);
                transport.Open();
                result = client.reqPositionEditLock(platformArgs, lockKey);
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

        public async Task<IInterfaceInteractResponse> addPositionEditLockAsync(PositionEditLock positionEditLock, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => addPositionEditLock(positionEditLock, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse addPositionEditLock(PositionEditLock positionEditLock, StubInterfaceInteractParams interactParams = null)
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
            argsList.Add(new Tuple<string, object>("positionEditLock", positionEditLock));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingPositionAdjust.Client(protocol);
                transport.Open();
                client.addPositionEditLock(platformArgs, positionEditLock);
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

        public async Task<IInterfaceInteractResponse> removePositionEditLockAsync(PositionEditLock positionEditLock, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => removePositionEditLock(positionEditLock, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse removePositionEditLock(PositionEditLock positionEditLock, StubInterfaceInteractParams interactParams = null)
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
            argsList.Add(new Tuple<string, object>("positionEditLock", positionEditLock));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingPositionAdjust.Client(protocol);
                transport.Open();
                client.removePositionEditLock(platformArgs, positionEditLock);
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

        public async Task<IInterfaceInteractResponse<PositionVerifyPage>> reqPositionVerifyAsync(ReqPositionVerifyOption option, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => reqPositionVerify(option, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<PositionVerifyPage> reqPositionVerify(ReqPositionVerifyOption option, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
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
            argsList.Add(new Tuple<string, object>("option", option));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            PositionVerifyPage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingPositionAdjust.Client(protocol);
                transport.Open();
                result = client.reqPositionVerify(platformArgs, option, pageOption);
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

        public async Task<IInterfaceInteractResponse<PositionDifferencePage>> reqPositionDifferenceAsync(ReqPositionDifferenceOption option, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => reqPositionDifference(option, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<PositionDifferencePage> reqPositionDifference(ReqPositionDifferenceOption option, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
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
            argsList.Add(new Tuple<string, object>("option", option));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            PositionDifferencePage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingPositionAdjust.Client(protocol);
                transport.Open();
                result = client.reqPositionDifference(platformArgs, option, pageOption);
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

        public async Task<IInterfaceInteractResponse<SettlementTimeRelateSledReqTime>> reqSettlementTimeRelateSledReqTimeAsync(long tradeAccountId, string settlementDate, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => reqSettlementTimeRelateSledReqTime(tradeAccountId, settlementDate, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<SettlementTimeRelateSledReqTime> reqSettlementTimeRelateSledReqTime(long tradeAccountId, string settlementDate, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "reqSettlementTimeRelateSledReqTime";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("tradeAccountId", tradeAccountId));
            argsList.Add(new Tuple<string, object>("settlementDate", settlementDate));
            SettlementTimeRelateSledReqTime result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingPositionAdjust.Client(protocol);
                transport.Open();
                result = client.reqSettlementTimeRelateSledReqTime(platformArgs, tradeAccountId, settlementDate);
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

        public async Task<IInterfaceInteractResponse<DailyPositionDifferencePage>> reqDailyPositionDifferenceAsync(ReqDailyPositionDifferenceOption option, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => reqDailyPositionDifference(option, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<DailyPositionDifferencePage> reqDailyPositionDifference(ReqDailyPositionDifferenceOption option, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
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
            argsList.Add(new Tuple<string, object>("option", option));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            DailyPositionDifferencePage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingPositionAdjust.Client(protocol);
                transport.Open();
                result = client.reqDailyPositionDifference(platformArgs, option, pageOption);
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

        public async Task<IInterfaceInteractResponse> updateDailyPositionDifferenceNoteAsync(DailyPositionDifference difference, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => updateDailyPositionDifferenceNote(difference, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse updateDailyPositionDifferenceNote(DailyPositionDifference difference, StubInterfaceInteractParams interactParams = null)
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
            argsList.Add(new Tuple<string, object>("difference", difference));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingPositionAdjust.Client(protocol);
                transport.Open();
                client.updateDailyPositionDifferenceNote(platformArgs, difference);
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

        public async Task<IInterfaceInteractResponse> clearAllAsync(CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => clearAll(interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse clearAll(StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "clearAll";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingPositionAdjust.Client(protocol);
                transport.Open();
                client.clearAll(platformArgs);
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


    }
}