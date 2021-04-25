using lib.xqclient_base.logger;
using lib.xqclient_base.thriftapi_mediation;
using lib.xqclient_base.thriftapi_mediation.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.broker;
using xueqiao.contract.online;
using xueqiao.graph.xiaoha.chart.terminal.ao.thriftapi;
using xueqiao.trade.hosting;
using xueqiao.trade.hosting.proxy;
using xueqiao.trade.hosting.terminal.ao;

namespace business_foundation_lib.xq_thriftlib_config
{
    public delegate void ThriftHttpLibEnvironmentChanged(XqThriftLibConfigurationManager confManager, lib.xqclient_base.thriftapi_mediation.Environment environment);

    public delegate void ThriftHttpLibLanguageChanged(XqThriftLibConfigurationManager confManager, lib.xqclient_base.thriftapi_mediation.Language language);

    /// <summary>
    /// 雪橇客户端 thrift lib 配置管理器 
    /// </summary>
    public class XqThriftLibConfigurationManager
    {
        public const string __TradeHostingProxyName = "trade_hosting_proxy";
        public const string __TradeHostingTerminalAoName = "trade_hosting_terminal_ao";
        public const string __ContractOnlineServiceName = "contract_online";
        public const string __BrokerServiceName = "broker_service";
        public const string __XiaohaChartTerminalAoName = "xiaoha_chart_terminal_ao";
        public const int ProxyHttpResponseStatusCode_SessionInvalid = 401;

        private Action _sessionInvalidDetectedHandler;
        private Func<XqThriftLibHostingServerUrlMeta> _hostingServerUrlMetaGetter;

        private readonly ContractOnlineDaoServiceHttpStub _contractOnlineServiceHttpStub;
        private readonly TradeHostingProxyHttpStub _tradeHostingProxyHttpStub;
        private readonly TradeHostingTerminalAoHttpStub _tradeHostingTerminalAoHttpStub;
        private readonly BrokerServiceHttpStub _brokerServiceHttpStub;
        private readonly XiaohaChartTerminalAoHttpStub _xiaohaChartTerminalAoHttpStub;
        
        private XqThriftLibConfigurationManager()
        {
            _contractOnlineServiceHttpStub = new ContractOnlineDaoServiceHttpStub(__ContractOnlineServiceName);
            _tradeHostingProxyHttpStub = new TradeHostingProxyHttpStub(__TradeHostingProxyName);
            _tradeHostingTerminalAoHttpStub = new TradeHostingTerminalAoHttpStub(__TradeHostingTerminalAoName);
            _brokerServiceHttpStub = new BrokerServiceHttpStub(__BrokerServiceName);
            _xiaohaChartTerminalAoHttpStub = new XiaohaChartTerminalAoHttpStub(__XiaohaChartTerminalAoName);

            ConfigThriftHttpLib();
        }

        public readonly static XqThriftLibConfigurationManager SharedInstance = new XqThriftLibConfigurationManager();

        /// <summary>
        /// 托管服务 url 的元信息获取方法
        /// </summary>
        public Func<XqThriftLibHostingServerUrlMeta> HostingServerUrlMetaGetter
        {
            get { return _hostingServerUrlMetaGetter; }
            set { _hostingServerUrlMetaGetter = value; }
        }

        /// <summary>
        /// 获取或设置 ThriftHttpLib 检查到 session 失效的处理方法 
        /// </summary>
        public Action ThriftHttpLibSessionInvalidDetectedHandler
        {
            get { return _sessionInvalidDetectedHandler; }
            set { _sessionInvalidDetectedHandler = value; }
        }
        
        /// <summary>
        /// 获取或设置 ThriftHttpLib 当前环境
        /// </summary>
        /// <returns></returns>
        public lib.xqclient_base.thriftapi_mediation.Environment ThriftHttpLibEnvironment
        {
            get { return StubInterfaceInteractManager.SharedInstance.Env; }
            set
            {
                StubInterfaceInteractManager.SharedInstance.Env = value;
                ThriftHttpLibEnvironmentChanged?.Invoke(this, value);
            }
        }

        /// <summary>
        /// ThriftHttpLib 当前环境变化事件
        /// </summary>
        public event ThriftHttpLibEnvironmentChanged ThriftHttpLibEnvironmentChanged;

        /// <summary>
        ///  获取或设置 ThriftHttpLib 当前语言
        /// </summary>
        /// <returns></returns>
        public lib.xqclient_base.thriftapi_mediation.Language ThriftHttpLibLanguage
        {
            get { return StubInterfaceInteractManager.SharedInstance.Lang; }
            set
            {
                StubInterfaceInteractManager.SharedInstance.Lang = value;
                ThriftHttpLibLanguageChanged?.Invoke(this, value);
            }
        }

        /// <summary>
        /// ThriftHttpLib 当前语言变化事件
        /// </summary>
        public event ThriftHttpLibLanguageChanged ThriftHttpLibLanguageChanged;

        public ContractOnlineDaoServiceHttpStub ContractOnlineServiceHttpStub => _contractOnlineServiceHttpStub;

        public TradeHostingProxyHttpStub TradeHostingProxyHttpStub => _tradeHostingProxyHttpStub;

        public TradeHostingTerminalAoHttpStub TradeHostingTerminalAoHttpStub => _tradeHostingTerminalAoHttpStub;

        public BrokerServiceHttpStub BrokerServiceHttpStub => _brokerServiceHttpStub;

        public XiaohaChartTerminalAoHttpStub XiaohaChartTerminalAoHttpStub => _xiaohaChartTerminalAoHttpStub;

        private void ConfigThriftHttpLib()
        {
            this.ThriftHttpLibEnvironment = lib.xqclient_base.thriftapi_mediation.Environment.DEV;
            this.ThriftHttpLibLanguage = Language.CN;

            // 配置 ServiceAccessUrlFactory
            StubInterfaceInteractManager.SharedInstance.ServiceAccessUrlFactory
                = (servant) => {
                    // 自定义配置 service access url
                    if (servant?.Equals(__TradeHostingTerminalAoName) == true
                        || servant?.Equals(__ContractOnlineServiceName) == true)
                    {
                        var hostingServerUrlMeta = _hostingServerUrlMetaGetter?.Invoke();
                        if (hostingServerUrlMeta != null)
                        {
                            var sb = new StringBuilder();
                            sb.Append("http://");
                            sb.Append(hostingServerUrlMeta.HostingServerIP);
                            sb.Append(":");
                            sb.Append(hostingServerUrlMeta.HostingServerPort);
                            sb.Append("?servant=");
                            sb.Append(servant);
                            return sb.ToString();
                        }
                    }

                    return null;
                };

            // 配置 InterfaceCustomExceptionParser
            StubInterfaceInteractManager.SharedInstance.InterfaceCustomExceptionParser
                = (servant, interfaceName, sourceExp, hasTransportExcep, httpRespStatusCode) => {
                    if (httpRespStatusCode == ProxyHttpResponseStatusCode_SessionInvalid)
                    {
                        var customExcepHolder = new InterfaceCustomParsedExceptionResultHolder();
                        customExcepHolder.SessionInvalid = true;
                        return customExcepHolder;
                    }
                    if (sourceExp != null)
                    {
                        Type sourceExpClassType = sourceExp.GetType();
                        Type ErrorInfoType = typeof(ErrorInfo);
                        if (sourceExpClassType == ErrorInfoType || ErrorInfoType.IsAssignableFrom(sourceExpClassType))
                        {
                            var customExcepHolder = new InterfaceCustomParsedExceptionResultHolder();
                            ErrorInfo errorInfo = sourceExp as ErrorInfo;
                            customExcepHolder.BusinessErrorCode = errorInfo.ErrorCode;
                            customExcepHolder.BusinessErrorMessage = !string.IsNullOrEmpty(errorInfo.ClientMsg) ? errorInfo.ClientMsg : errorInfo.ErrorMsg;
                            if (errorInfo.ErrorCode == TradeHostingBasicErrorCode.ERROR_USER_SESSION.GetHashCode())
                            {
                                customExcepHolder.SessionInvalid = true;
                            }
                            return customExcepHolder;
                        }
                    }
                    return null;
                };

            // 配置 InterfaceResponsePassThrougher
            StubInterfaceInteractManager.SharedInstance.InterfaceResponsePassThrougher =
                (resp, interactParams) =>
                {
                    if (resp == null)
                    {
                        AppLog.Warn($"InterfaceResponsePassThrougher. resp is null.");
                        return;
                    }

                    AppLog.Info($"InterfaceResponsePassThrougher.{resp?.ToString()}");

                    if (IsSessionInvalid(resp))
                    {
                        _sessionInvalidDetectedHandler?.Invoke();
                    }
                };
        }

        private static bool IsSessionInvalid(IInterfaceInteractResponse resp)
        {
            if (resp == null) return false;
            return (resp.CustomParsedExceptionResult?.SessionInvalid == true)
                   || (resp.HttpResponseStatusCode == ProxyHttpResponseStatusCode_SessionInvalid);
        }
    }
}
