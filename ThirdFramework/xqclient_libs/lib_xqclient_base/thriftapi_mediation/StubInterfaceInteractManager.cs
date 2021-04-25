using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using lib.xqclient_base.thriftapi_mediation.Interface;
using lib.xqclient_base.thriftapi_mediation.Internal;
using System.Threading.Tasks.Schedulers;
using lib.xqclient_base.logger;
using System.Collections;
using Thrift.Collections;

namespace lib.xqclient_base.thriftapi_mediation
{
    public enum Environment
    {
        DEV,
        GAMMA,
        IDC
    }

    public enum Language
    {
        CN = 1, // 中文
        EN = 2  // 英文
    }

    public delegate IInterfaceCustomParsedExceptionResult InteractInterfaceCustomExceptionParser(string servant,
        string interfaceName,
        Exception responseSourceException,
        bool responseHasTransportException,
        int? httpResponseStatusCode);

    public delegate string ServiceAccessUrlGetter(string servant);

    /// <summary>
    /// 接口交互管理器。单例
    /// </summary>
    public class StubInterfaceInteractManager
    {
        private readonly LimitedConcurrencyLevelTaskScheduler interfaceTaskScheduler;

        private StubInterfaceInteractManager()
        {
            interfaceTaskScheduler = new LimitedConcurrencyLevelTaskScheduler(20);
        }

        /// <summary>
        /// 单例
        /// </summary>
        public static StubInterfaceInteractManager SharedInstance { private set; get; } = new StubInterfaceInteractManager();

        /// <summary>
        /// 环境
        /// </summary>
        public Environment Env { get; set; } = Environment.DEV;

        /// <summary>
        /// 语言
        /// </summary>
        public Language Lang { get; set; } = Language.CN;

        /// <summary>
        /// 服务接入 url 获取方法
        /// </summary>
        public ServiceAccessUrlGetter ServiceAccessUrlFactory { get; set; }

        /// <summary>
        /// 接口自定义异常解析方法
        /// </summary>
        public InteractInterfaceCustomExceptionParser InterfaceCustomExceptionParser { get; set; }

        /// <summary>
        /// 接口回报通过器。可在其中自定义日志
        /// </summary>
        public Action<IInterfaceInteractResponse, StubInterfaceInteractParams> InterfaceResponsePassThrougher { get; set; }

        /// <summary>
        /// 单个字符日志项最大字符长度
        /// </summary>
        public int LogStringItemMaxLength = 4096;


        public async Task StartTaskAsync(Action action, CancellationToken cancellationToken)
        {
            if (action == null) throw new ArgumentNullException("action");
            await Task.Factory.StartNew(action, cancellationToken, TaskCreationOptions.None, interfaceTaskScheduler);
        }

        public async Task<T> StartTaskAsync<T>(Func<T> func, CancellationToken cancellationToken)
        {
            if (func == null) throw new ArgumentNullException("func");
            return await Task.Factory.StartNew(func, cancellationToken, TaskCreationOptions.None, interfaceTaskScheduler);
        }

        public XqclientTHttpClient GetTransportForService(string servant, StubInterfaceInteractParams interactParams,
            out string serviceAccessUrl)
        {
            string accessUrl = ServiceAccessUrlFactory?.Invoke(servant);
            if (accessUrl == null)
            {
                accessUrl = StubInterfaceHelper.GetDefaultAccessUrlForService(Env, servant);
            }
            serviceAccessUrl = accessUrl;

            interactParams = GetDefaultInteractParams(interactParams);
            var transport = new XqclientTHttpClient(new Uri(accessUrl));
            transport.Proxy = null;
            transport.ConnectTimeout = interactParams.TransportConnectTimeoutMS;
            transport.ReadTimeout = interactParams.TransportReadTimeoutMS;
            return transport;
        }
        
        /// <summary>
        /// 现在距离 1970/1/1的时间间隔
        /// </summary>
        /// <returns></returns>
        public static TimeSpan NowUnixTimeSpan()
        {
            return (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
        }

        public IInterfaceInteractResponse ResponseForInterfaceIntereaction(string servant,
            string interfaceName,
            string serviceAccessUrl,
            double beginRequestTimestampMS,
            Exception sourceException,
            bool hasTransportException,
            int? httpResponseStatusCode,
            StubInterfaceInteractParams interactParams,
            IEnumerable<Tuple<string, object>> requestArgs)
        {
            interactParams = GetDefaultInteractParams(interactParams);
            var resp = new InterfaceInteractResponse(servant, interfaceName, sourceException, hasTransportException, httpResponseStatusCode);
            ConfigureInterfaceResponse(resp, serviceAccessUrl, beginRequestTimestampMS, hasTransportException, httpResponseStatusCode);
            LogInterfaceInteractInfo(interactParams, requestArgs, resp);
            PassThroughInterfaceResponse(resp, interactParams);
            return resp;
        }

        public IInterfaceInteractResponse<T> ResponseForInterfaceIntereaction<T>(string servant,
            string interfaceName,
            string serviceAccessUrl,
            double beginRequestTimestampMS,
            T CorrectResult,
            Exception sourceException,
            bool hasTransportException,
            int? httpResponseStatusCode,
            StubInterfaceInteractParams interactParams,
            IEnumerable<Tuple<string, object>> requestArgs)
        {
            interactParams = GetDefaultInteractParams(interactParams);
            var resp = new InterfaceInteractResponse<T>(servant, interfaceName, sourceException, hasTransportException, httpResponseStatusCode, CorrectResult);
            ConfigureInterfaceResponse(resp, serviceAccessUrl, beginRequestTimestampMS, hasTransportException, httpResponseStatusCode);
            LogInterfaceInteractInfo(interactParams, requestArgs, resp);
            PassThroughInterfaceResponse(resp, interactParams);
            return resp;
        }

        private void LogInterfaceInteractInfo(StubInterfaceInteractParams interactParams,
            IEnumerable<Tuple<string, object>> requestArgs, InterfaceInteractResponse response,
            bool isVoidReturn, object returnResult)
        {
            if (interactParams?.LogInterfaceInteractInfo != true) return;
            var sb = new StringBuilder(128);

            // format interface perform info
            var costTimeMs = response?.InteractInformation?.CostTimeMS;
            string costTimeMsStr = "--";
            if (costTimeMs != null) costTimeMsStr = $"{((long)costTimeMs)}ms";
            sb.Append($"[perform={costTimeMsStr}]");

            // format interface request info
            sb.Append($"{response?.InterfaceName}");
            sb.Append("(");
            if (interactParams?.LogInterfaceRequestArgs == true)
            {
                if (requestArgs != null)
                {
                    for (var i = 0; i < requestArgs.Count(); i++)
                    {
                        if (i > 0)
                            sb.Append(", ");
                        var reqArg = requestArgs.ElementAt(i);
                        sb.Append($"{reqArg.Item1}=");
                        string logArgValStr = ReqOrRespArgValueToString(reqArg.Item2);
                        if (logArgValStr.Length > LogStringItemMaxLength)
                        {
                            sb.Append($"[{logArgValStr.Length}]").Append(logArgValStr.Substring(0, LogStringItemMaxLength)).Append(" ...");
                        }
                        else
                        {
                            sb.Append(logArgValStr);
                        }
                    }
                }
            }
            else
            {
                sb.Append("-----因接口交互要求，不打印接口请求参数-----");
            }
            sb.Append(")");

            if (!isVoidReturn)
            {
                // format interface return info
                sb.Append(" return ");
                if (interactParams?.LogInterfaceReturnResult == true)
                {
                    if (returnResult == null)
                    {
                        sb.Append("null");
                    }
                    else
                    {
                        string resultStr = ReqOrRespArgValueToString(returnResult);
                        if (resultStr.Length > LogStringItemMaxLength)
                        {
                            sb.Append($"[{resultStr.Length}]").Append(resultStr.Substring(0, LogStringItemMaxLength)).Append(" ...");
                        }
                        else
                        {
                            sb.Append(resultStr);
                        }
                    }
                }
                else
                {
                    sb.Append("-----因接口交互要求，不打印接口返回结果-----");
                }
            }

            AppLog.Info(sb.ToString());
        }

        private static string ReqOrRespArgValueToString(object reqOrRespArgValue)
        {
            if (reqOrRespArgValue == null) return "";

            var sb = new StringBuilder();

            Type t = reqOrRespArgValue.GetType();
            bool isDict = t.IsGenericType && t.GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<,>));
            if (isDict)
            {
                var dictValue = reqOrRespArgValue as IDictionary;
                var dictKeys = dictValue.Keys;
                
                var isFirst = true;
                sb.Append("{");
                foreach (var key in dictKeys)
                {
                    if (!isFirst)
                    {
                        sb.Append(",");
                        isFirst = false;
                    }
                    sb.Append($"{key.ToString()}:{ReqOrRespArgValueToString(dictValue[key])}");                    
                }
                sb.Append("}");
                return sb.ToString();
            }

            var isThriftHashSet = t.IsGenericType && t.GetGenericTypeDefinition().IsAssignableFrom(typeof(THashSet<>));
            var isSet = t.IsGenericType && t.GetGenericTypeDefinition().IsAssignableFrom(typeof(HashSet<>));
            var isList = t.IsGenericType && t.GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
            if (isThriftHashSet || isSet || isList)
            {
                var enumerableObj = (reqOrRespArgValue as IEnumerable);
                var isFirst = true;
                sb.Append("[");
                foreach (var item in enumerableObj)
                {
                    if (!isFirst)
                    {
                        sb.Append(",");
                        isFirst = false;
                    }
                    sb.Append(ReqOrRespArgValueToString(item));
                }
                sb.Append("]");
                return sb.ToString();
            }

            return reqOrRespArgValue.ToString()??"";
        }

        private void LogInterfaceInteractInfo<RT>(StubInterfaceInteractParams interactParams,
            IEnumerable<Tuple<string, object>> requestArgs, InterfaceInteractResponse<RT> response)
        {
            object returnResult = null;
            if (response != null)
                returnResult = response.CorrectResult;
            LogInterfaceInteractInfo(interactParams, requestArgs, response, false, returnResult);
        }

        private void LogInterfaceInteractInfo(StubInterfaceInteractParams interactParams,
            IEnumerable<Tuple<string, object>> requestArgs, InterfaceInteractResponse response)
        {
            LogInterfaceInteractInfo(interactParams, requestArgs, response, true, null);
        }

        private void ConfigureInterfaceResponse(InterfaceInteractResponse response,
            string serviceAccessUrl,
            double beginRequestTimestampMS,
            bool hasTransportException,
            int? httpResponseStatusCode)
        {
            if (response == null) return;

            // 设置 InteractInformation
            response.InteractInformation = new InterfaceInteractInformation
            {
                CostTimeMS = NowUnixTimeSpan().TotalMilliseconds - beginRequestTimestampMS,
                BeginRequestTimestampMS = beginRequestTimestampMS,
                ServiceAccessUrl = serviceAccessUrl
            };

            // 设置 InteractExceptionResult
            var holder = InterfaceCustomExceptionParser?.Invoke(response.Servant,
                    response.InterfaceName,
                    response.SourceException,
                    hasTransportException,
                    httpResponseStatusCode);
            response.CustomParsedExceptionResult = holder;
        }

        private void PassThroughInterfaceResponse(IInterfaceInteractResponse interfaceResponse,
            StubInterfaceInteractParams interactParams)
        {
            if (interactParams.IsPassThroughResponse == true)
            {
                InterfaceResponsePassThrougher?.Invoke(interfaceResponse, interactParams);
            }
        }

        private static StubInterfaceInteractParams GetDefaultInteractParams(StubInterfaceInteractParams interactParams = null)
        {
            if (interactParams != null)
                return interactParams;
            return new StubInterfaceInteractParams();
        }

        internal class StubInterfaceHelper
        {
            private const string SERVANT = "servant";
            private const string DefaultDEVUrlBase = "http://devproxy.xueqiao.cn";
            private const string DefaultGAMMAUrlBase = "http://gaproxy.xueqiao.cn";
            private const string DefaultIDCUrlBase = "http://proxy.xueqiao.cn";

            public static string GetDefaultAccessUrlForService(Environment env, string servant)
            {
                string url = "";
                switch (env)
                {
                    case Environment.DEV:
                        url += DefaultDEVUrlBase;
                        break;
                    case Environment.GAMMA:
                        url += DefaultGAMMAUrlBase;
                        break;
                    case Environment.IDC:
                        url += DefaultIDCUrlBase;
                        break;
                }

                url += ("?" + SERVANT + "=" + servant ?? "");
                return url;
            }
        }
    }
}
