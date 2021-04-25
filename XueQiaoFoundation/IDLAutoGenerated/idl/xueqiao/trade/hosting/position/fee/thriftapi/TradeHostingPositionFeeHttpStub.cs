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

namespace xueqiao.trade.hosting.position.fee.thriftapi
{
    public class TradeHostingPositionFeeHttpStub
    {
        public string Servant { get; private set; }

        public TradeHostingPositionFeeHttpStub (string servant)
        {
            if (servant == null) throw new ArgumentNullException("`servant` can't be null.");
            this.Servant = servant;
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
                var client = new TradeHostingPositionFee.Client(protocol);
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

        public async Task<IInterfaceInteractResponse> setGeneralMarginSettingAsync(XQGeneralMarginSettings marginSettings, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => setGeneralMarginSetting(marginSettings, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse setGeneralMarginSetting(XQGeneralMarginSettings marginSettings, StubInterfaceInteractParams interactParams = null)
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
            argsList.Add(new Tuple<string, object>("marginSettings", marginSettings));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingPositionFee.Client(protocol);
                transport.Open();
                client.setGeneralMarginSetting(platformArgs, marginSettings);
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

        public async Task<IInterfaceInteractResponse> setGeneralCommissionSettingAsync(XQGeneralCommissionSettings commissionSettings, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => setGeneralCommissionSetting(commissionSettings, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse setGeneralCommissionSetting(XQGeneralCommissionSettings commissionSettings, StubInterfaceInteractParams interactParams = null)
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
            argsList.Add(new Tuple<string, object>("commissionSettings", commissionSettings));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingPositionFee.Client(protocol);
                transport.Open();
                client.setGeneralCommissionSetting(platformArgs, commissionSettings);
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

        public async Task<IInterfaceInteractResponse> addSpecMarginSettingAsync(XQSpecMarginSettings marginSettings, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => addSpecMarginSetting(marginSettings, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse addSpecMarginSetting(XQSpecMarginSettings marginSettings, StubInterfaceInteractParams interactParams = null)
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
            argsList.Add(new Tuple<string, object>("marginSettings", marginSettings));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingPositionFee.Client(protocol);
                transport.Open();
                client.addSpecMarginSetting(platformArgs, marginSettings);
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

        public async Task<IInterfaceInteractResponse> addSpecCommissionSettingAsync(XQSpecCommissionSettings commissionSettings, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => addSpecCommissionSetting(commissionSettings, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse addSpecCommissionSetting(XQSpecCommissionSettings commissionSettings, StubInterfaceInteractParams interactParams = null)
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
            argsList.Add(new Tuple<string, object>("commissionSettings", commissionSettings));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingPositionFee.Client(protocol);
                transport.Open();
                client.addSpecCommissionSetting(platformArgs, commissionSettings);
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

        public async Task<IInterfaceInteractResponse> updateSpecMarginSettingAsync(XQSpecMarginSettings marginSettings, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => updateSpecMarginSetting(marginSettings, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse updateSpecMarginSetting(XQSpecMarginSettings marginSettings, StubInterfaceInteractParams interactParams = null)
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
            argsList.Add(new Tuple<string, object>("marginSettings", marginSettings));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingPositionFee.Client(protocol);
                transport.Open();
                client.updateSpecMarginSetting(platformArgs, marginSettings);
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

        public async Task<IInterfaceInteractResponse> updateSpecCommissionSettingAsync(XQSpecCommissionSettings commissionSettings, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => updateSpecCommissionSetting(commissionSettings, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse updateSpecCommissionSetting(XQSpecCommissionSettings commissionSettings, StubInterfaceInteractParams interactParams = null)
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
            argsList.Add(new Tuple<string, object>("commissionSettings", commissionSettings));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingPositionFee.Client(protocol);
                transport.Open();
                client.updateSpecCommissionSetting(platformArgs, commissionSettings);
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

        public async Task<IInterfaceInteractResponse> deleteSpecMarginSettingAsync(long subAccountId, long commodityId, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => deleteSpecMarginSetting(subAccountId, commodityId, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse deleteSpecMarginSetting(long subAccountId, long commodityId, StubInterfaceInteractParams interactParams = null)
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
            argsList.Add(new Tuple<string, object>("subAccountId", subAccountId));
            argsList.Add(new Tuple<string, object>("commodityId", commodityId));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingPositionFee.Client(protocol);
                transport.Open();
                client.deleteSpecMarginSetting(platformArgs, subAccountId, commodityId);
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

        public async Task<IInterfaceInteractResponse> deleteSpecCommissionSettingAsync(long subAccountId, long commodityId, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => deleteSpecCommissionSetting(subAccountId, commodityId, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse deleteSpecCommissionSetting(long subAccountId, long commodityId, StubInterfaceInteractParams interactParams = null)
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
            argsList.Add(new Tuple<string, object>("subAccountId", subAccountId));
            argsList.Add(new Tuple<string, object>("commodityId", commodityId));
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingPositionFee.Client(protocol);
                transport.Open();
                client.deleteSpecCommissionSetting(platformArgs, subAccountId, commodityId);
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

        public async Task<IInterfaceInteractResponse<XQGeneralMarginSettings>> queryXQGeneralMarginSettingsAsync(long subAccountId, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => queryXQGeneralMarginSettings(subAccountId, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<XQGeneralMarginSettings> queryXQGeneralMarginSettings(long subAccountId, StubInterfaceInteractParams interactParams = null)
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
            argsList.Add(new Tuple<string, object>("subAccountId", subAccountId));
            XQGeneralMarginSettings result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingPositionFee.Client(protocol);
                transport.Open();
                result = client.queryXQGeneralMarginSettings(platformArgs, subAccountId);
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

        public async Task<IInterfaceInteractResponse<XQGeneralCommissionSettings>> queryXQGeneralCommissionSettingsAsync(long subAccountId, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => queryXQGeneralCommissionSettings(subAccountId, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<XQGeneralCommissionSettings> queryXQGeneralCommissionSettings(long subAccountId, StubInterfaceInteractParams interactParams = null)
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
            argsList.Add(new Tuple<string, object>("subAccountId", subAccountId));
            XQGeneralCommissionSettings result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingPositionFee.Client(protocol);
                transport.Open();
                result = client.queryXQGeneralCommissionSettings(platformArgs, subAccountId);
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

        public async Task<IInterfaceInteractResponse<XQSpecMarginSettingPage>> queryXQSpecMarginSettingPageAsync(QueryXQSpecSettingOptions queryOptions, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => queryXQSpecMarginSettingPage(queryOptions, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<XQSpecMarginSettingPage> queryXQSpecMarginSettingPage(QueryXQSpecSettingOptions queryOptions, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
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
            argsList.Add(new Tuple<string, object>("queryOptions", queryOptions));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            XQSpecMarginSettingPage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingPositionFee.Client(protocol);
                transport.Open();
                result = client.queryXQSpecMarginSettingPage(platformArgs, queryOptions, pageOption);
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

        public async Task<IInterfaceInteractResponse<XQSpecCommissionSettingPage>> queryXQSpecCommissionSettingPageAsync(QueryXQSpecSettingOptions queryOptions, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => queryXQSpecCommissionSettingPage(queryOptions, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<XQSpecCommissionSettingPage> queryXQSpecCommissionSettingPage(QueryXQSpecSettingOptions queryOptions, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
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
            argsList.Add(new Tuple<string, object>("queryOptions", queryOptions));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            XQSpecCommissionSettingPage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingPositionFee.Client(protocol);
                transport.Open();
                result = client.queryXQSpecCommissionSettingPage(platformArgs, queryOptions, pageOption);
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

        public async Task<IInterfaceInteractResponse<UpsideContractMarginPage>> queryUpsideContractMarginPageAsync(QueryUpsidePFeeOptions queryOptions, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => queryUpsideContractMarginPage(queryOptions, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<UpsideContractMarginPage> queryUpsideContractMarginPage(QueryUpsidePFeeOptions queryOptions, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
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
            argsList.Add(new Tuple<string, object>("queryOptions", queryOptions));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            UpsideContractMarginPage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingPositionFee.Client(protocol);
                transport.Open();
                result = client.queryUpsideContractMarginPage(platformArgs, queryOptions, pageOption);
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

        public async Task<IInterfaceInteractResponse<UpsideContractCommissionPage>> queryUpsideContractCommissionPageAsync(QueryUpsidePFeeOptions queryOptions, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => queryUpsideContractCommissionPage(queryOptions, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<UpsideContractCommissionPage> queryUpsideContractCommissionPage(QueryUpsidePFeeOptions queryOptions, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
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
            argsList.Add(new Tuple<string, object>("queryOptions", queryOptions));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            UpsideContractCommissionPage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingPositionFee.Client(protocol);
                transport.Open();
                result = client.queryUpsideContractCommissionPage(platformArgs, queryOptions, pageOption);
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

        public async Task<IInterfaceInteractResponse<XQContractMarginPage>> queryXQContractMarginPageAsync(QueryXQPFeeOptions queryOptions, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => queryXQContractMarginPage(queryOptions, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<XQContractMarginPage> queryXQContractMarginPage(QueryXQPFeeOptions queryOptions, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
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
            argsList.Add(new Tuple<string, object>("queryOptions", queryOptions));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            XQContractMarginPage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingPositionFee.Client(protocol);
                transport.Open();
                result = client.queryXQContractMarginPage(platformArgs, queryOptions, pageOption);
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

        public async Task<IInterfaceInteractResponse<XQContractCommissionPage>> queryXQContractCommissionPageAsync(QueryXQPFeeOptions queryOptions, IndexedPageOption pageOption, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => queryXQContractCommissionPage(queryOptions, pageOption, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<XQContractCommissionPage> queryXQContractCommissionPage(QueryXQPFeeOptions queryOptions, IndexedPageOption pageOption, StubInterfaceInteractParams interactParams = null)
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
            argsList.Add(new Tuple<string, object>("queryOptions", queryOptions));
            argsList.Add(new Tuple<string, object>("pageOption", pageOption));
            XQContractCommissionPage result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingPositionFee.Client(protocol);
                transport.Open();
                result = client.queryXQContractCommissionPage(platformArgs, queryOptions, pageOption);
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

        public async Task<IInterfaceInteractResponse<PositionFee>> queryPositionFeeAsync(long subAccountId, long contractId, CancellationToken cancellationToken, StubInterfaceInteractParams interactParams = null)
        {
            return await StubInterfaceInteractManager.SharedInstance.StartTaskAsync(
            () => queryPositionFee(subAccountId, contractId, interactParams),
            cancellationToken);
        }

        public IInterfaceInteractResponse<PositionFee> queryPositionFee(long subAccountId, long contractId, StubInterfaceInteractParams interactParams = null)
        {
            var requestBeginTimestampMS = StubInterfaceInteractManager.NowUnixTimeSpan().TotalMilliseconds;
            string servant = Servant;
            string interfaceName = "queryPositionFee";
            
            PlatformArgs platformArgs = new PlatformArgs();
            StackFrame stackFrame = new StackTrace(true).GetFrame(1);
            platformArgs.SourceDesc = stackFrame.GetFileName() + "[" + stackFrame.GetMethod().Name + "]:" + stackFrame.GetFileLineNumber();
            platformArgs.ClientLang = StubInterfaceInteractManager.SharedInstance.Lang == Language.CN ? EClientLang.CN : EClientLang.EN;
            
            var argsList = new List<Tuple<string, object>>();
            argsList.Add(new Tuple<string, object>("platformArgs", platformArgs));
            argsList.Add(new Tuple<string, object>("subAccountId", subAccountId));
            argsList.Add(new Tuple<string, object>("contractId", contractId));
            PositionFee result = null;;
            Exception sourceExp = null;
            bool hasTransportExp = false;
            int? httpResponseStatusCode = null;
            var transport = StubInterfaceInteractManager.SharedInstance.GetTransportForService(Servant, interactParams, out string serviceAccessUrl);
            try
            {
                var protocol = new TCompactProtocol(transport);
                var client = new TradeHostingPositionFee.Client(protocol);
                transport.Open();
                result = client.queryPositionFee(platformArgs, subAccountId, contractId);
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