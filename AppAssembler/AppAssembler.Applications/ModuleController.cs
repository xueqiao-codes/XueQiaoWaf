using AppAssembler.Applications.Properties;
using AppAssembler.Interfaces.Applications;
using business_foundation_lib.performance_monitor;
using business_foundation_lib.xq_thriftlib_config;
using ContainerShell.Interfaces.Applications;
using lib.configuration;
using lib.xqclient_base.logger;
using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows;
using XueQiaoFoundation.BusinessResources.Constants;
using XueQiaoFoundation.BusinessResources.Helpers;
using XueQiaoFoundation.BusinessResources.Models;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoFoundation.UI.Styles;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;
using XueQiaoWaf.Trade.Interfaces.Applications;

namespace AppAssembler.Applications
{
    [Export(typeof(IModuleController)), Export(typeof(IAppAssemblerService)), PartCreationPolicy(CreationPolicy.Shared)]
    internal class ModuleController : IModuleController, IAppAssemblerService
    {
        private readonly Lazy<IContainerShellService> containerShellService;
        private readonly Lazy<ILoginUserManageService> loginManService;
        private readonly Lazy<ILoginDataService> loginDataService;
        private readonly Lazy<ITradeModuleService> tradeModuleService;
        private readonly IMessageWindowService messageWindowService;
        
        private CurrentProcessPerformanceMonitor _appPerformanceMonitor;
        private XqAppThemeManager _appThemeManager;
        private ConfigurationManager<XqAppPreference> _preferenceManager;

        private bool currentShowingStartupWindow;
        
        private readonly object sessionInvalidHandleLock = new object();
        private bool isSessionInvalidHandling = false;

        private bool shutdowned;

        [ImportingConstructor]
        public ModuleController(Lazy<IContainerShellService> containerShellService,
            Lazy<ILoginUserManageService> loginManService,
            Lazy<ILoginDataService> loginDataService,
            Lazy<ITradeModuleService> tradeModuleService,
            IMessageWindowService messageWindowService)
        {
            this.containerShellService = containerShellService;
            this.loginManService = loginManService;
            this.loginDataService = loginDataService;
            this.tradeModuleService = tradeModuleService;
            this.messageWindowService = messageWindowService;
        }
        
        public void Initialize()
        {
            UpgradeCurrentModuleSettingsIfNeed();

            InitAppPerformanceMonitor();
            InitAppThemeManager();
            InitPreferenceManager();
            InitConfigXqThriftLibConfigurationManager();

            this.AppShutdown += RecvAppShutdown;

            ApplyOriginAppThemeConfig();
            ApplyOriginLanguageConfig();
            ApplyOriginThriftHttpLibEnvironmentConfig();
        }

        public void Run()
        {
            _appPerformanceMonitor?.Start();
        }

        public void Shutdown()
        {
            if (this.shutdowned) return;
            this.shutdowned = true;

            if (_appPerformanceMonitor != null)
            {
                _appPerformanceMonitor.PerformanceDataChanged -= AppPerformanceMonitor_PerformanceDataChanged;
                _appPerformanceMonitor.Stop();
            }

            XqThriftLibConfigurationManager.SharedInstance.ThriftHttpLibEnvironmentChanged -= XqThriftLibConfigurationManager_LibEnvironmentChanged;
            
            this.AppShutdown -= RecvAppShutdown;

            SaveCurrentModuleSettings();
        }
        
        public bool IsShowingStartupWindow => currentShowingStartupWindow;

        public void ShowStartupUI()
        {
            if (currentShowingStartupWindow) return;
            currentShowingStartupWindow = true;

            // 关闭所有窗口
            containerShellService.Value.ShellWindowClosed -= ContainerShell_Closed;
            containerShellService.Value.ShellWindowClosing -= ContainerShell_Closing;
            foreach (var win in System.Windows.Application.Current.Windows)
            {
                if (win is System.Windows.Window _win)
                {
                    _win.Close();
                }
            }
            
            containerShellService.Value.ShellWindowClosed += ContainerShell_Closed;
            containerShellService.Value.ShellWindowClosing += ContainerShell_Closing;

            // 登录后显示主界面
            var loginState = loginManService.Value.ShowLoginDialog(null, null);

            // 已关闭登录界面
            currentShowingStartupWindow = false;

            //
            // containerShellService may disposed when login dialog closed,
            // so use try catch to avoid System.ObjectDisposedException throw
            try
            {
                if (loginState == null)
                {
                    // 已取消登录
                    ShutdownApplication();
                }
                else if (loginState == true)
                {
                    // 登录成功
                    containerShellService.Value.ShowShellWindow();
                }
            }
            catch (Exception e)
            {
                AppLog.Error(e.Message);
            }
        }

        public void ApplyTheme(string themeName)
        {
            // 设置偏好设置的主题项
            if (_preferenceManager.Config.AppTheme != themeName)
            {
                _preferenceManager.Config.AppTheme = themeName;
                _preferenceManager.SaveConfig(out Exception _saveExp);
                if (_saveExp != null)
                {
                    AppLog.Error("Failed to save pref", _saveExp);
                }
            }

            // 应用主题样式
            XqAppThemeType? themeType = null;
            if (Enum.TryParse(themeName, out XqAppThemeType _theme0))
            {
                themeType = _theme0;    
            }
            else if (Enum.TryParse(themeName.ToUpper(), out XqAppThemeType _theme1))
            {
                themeType = _theme1;
            }

            if (themeType != null)
            {
                _appThemeManager.ApplyTheme(App.Current, themeType.Value);
                ThemeApplied?.Invoke(themeType.ToString());
            }
        }

        public event ThemeApplied ThemeApplied;

        public void ApplyLanguage(XqAppLanguages language)
        {
            // 设置偏好设置的语言项
            var langStr = language.ToString();
            if (_preferenceManager.Config.Language != langStr)
            {
                _preferenceManager.Config.Language = langStr;
                _preferenceManager.SaveConfig(out Exception _saveExp);
                if (_saveExp != null)
                {
                    AppLog.Error("Failed to save pref", _saveExp);
                }
            }

            // 设置 ThriftHttpLib 的语言项
            var thriftHttpLibLanguage = XueQiaoBusinessHelper.Convert2ThriftApiLanguage(language);
            XqThriftLibConfigurationManager.SharedInstance.ThriftHttpLibLanguage = thriftHttpLibLanguage;
            LanguageApplied?.Invoke(language);
        }

        public event LanguageApplied LanguageApplied;

        public ConfigurationManager<XqAppPreference> PreferenceManager => _preferenceManager;

        public void ShutdownApplication()
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                AppShutdown?.Invoke();
                Application.Current?.Shutdown();
            }, System.Windows.Threading.DispatcherPriority.Normal);
        }

        public event AppShutdown AppShutdown;

        public event CurrentProcessPerformanceDataChanged AppPerformanceDataChanged;

        private void ContainerShell_Closed(object sender, EventArgs e)
        {
            containerShellService.Value.ShellWindowClosed -= ContainerShell_Closed;
            containerShellService.Value.ShellWindowClosing -= ContainerShell_Closing;

            ShutdownApplication();
        }

        private void ContainerShell_Closing(object sender, CancelEventArgs e)
        {
            var confirmCloseApp = messageWindowService.ShowQuestionDialog(Application.Current.MainWindow, null, null, "重要提示", "\n         您确认要关闭软件吗？         \n",
                false, "关闭", "取消");
            e.Cancel = confirmCloseApp != true;
        }

        private void InitAppPerformanceMonitor()
        {
            this._appPerformanceMonitor = new CurrentProcessPerformanceMonitor();
            _appPerformanceMonitor.PerformanceDataChanged += AppPerformanceMonitor_PerformanceDataChanged;
        }

        private void AppPerformanceMonitor_PerformanceDataChanged(AppPerformanceData performanceData)
        {
            AppPerformanceDataChanged?.Invoke(performanceData);
        }

        private void InitAppThemeManager()
        {
            this._appThemeManager = new XqAppThemeManager(_themeType =>
            {
                return System.Waf.Presentation.ResourceHelper.GetPackUri(typeof(StylesHelper).Assembly,
                    $"Themes/ControlTheme.{_themeType.ToString()}.xaml");
            });
        }

        private void InitPreferenceManager()
        {
            // 将偏好设置放到安装目录，预防多个安装位置时进程访问引发的异常
            var appPreferenceFileName = Path.Combine(PathHelper.AppSetupDirectoryPath, "AppPreference.xml");
            this._preferenceManager = new ConfigurationManager<XqAppPreference>(appPreferenceFileName);
            if (_preferenceManager.LoadConfigException != null)
            {
                // Load preference failed
                AppLog.Error($"Failed to load app preference of file. {appPreferenceFileName}", _preferenceManager.LoadConfigException);
            }

            // setup default preference
            var needUpdatePref = false;
            if (string.IsNullOrEmpty(_preferenceManager.Config.Language))
            {
                _preferenceManager.Config.Language = XqAppLanguages.CN.ToString();
                needUpdatePref = true;
            }

            if (string.IsNullOrEmpty(_preferenceManager.Config.AppTheme))
            {
                _preferenceManager.Config.AppTheme = XqAppThemeType.DARK.ToString();
                needUpdatePref = true;
            }

            if (string.IsNullOrEmpty(_preferenceManager.Config.OrderErrAudioFileName))
            {
                _preferenceManager.Config.OrderErrAudioFileName = XueQiaoBusinessHelper.GetDefaultSoundFileFullName("Error.wav");
                needUpdatePref = true;
            }

            if (string.IsNullOrEmpty(_preferenceManager.Config.OrderAmbiguousAudioFileName))
            {
                _preferenceManager.Config.OrderAmbiguousAudioFileName = XueQiaoBusinessHelper.GetDefaultSoundFileFullName("Error.wav");
                needUpdatePref = true;
            }

            if (string.IsNullOrEmpty(_preferenceManager.Config.OrderTriggeredAudioFileName))
            {
                _preferenceManager.Config.OrderTriggeredAudioFileName = XueQiaoBusinessHelper.GetDefaultSoundFileFullName("Trade.wav");
                needUpdatePref = true;
            }

            if (string.IsNullOrEmpty(_preferenceManager.Config.OrderTradedAudioFileName))
            {
                _preferenceManager.Config.OrderTradedAudioFileName = XueQiaoBusinessHelper.GetDefaultSoundFileFullName("Trade2.wav");
                needUpdatePref = true;
            }

            if (string.IsNullOrEmpty(_preferenceManager.Config.LameTradedAudioFileName))
            {
                _preferenceManager.Config.LameTradedAudioFileName = XueQiaoBusinessHelper.GetDefaultSoundFileFullName("Error.wav");
                needUpdatePref = true;
            }

            if (string.IsNullOrEmpty(_preferenceManager.Config.OrderOtherNotifyAudioFileName))
            {
                _preferenceManager.Config.OrderOtherNotifyAudioFileName = XueQiaoBusinessHelper.GetDefaultSoundFileFullName("Trade.wav");
                needUpdatePref = true;
            }

            if (_preferenceManager.Config.PlaceOrderNeedConfirm == null)
            {
                _preferenceManager.Config.PlaceOrderNeedConfirm = true;
                needUpdatePref = true;
            }

            if (_preferenceManager.Config.SuspendOrderNeedConfirm == null)
            {
                _preferenceManager.Config.SuspendOrderNeedConfirm = true;
                needUpdatePref = true;
            }

            if (_preferenceManager.Config.ResumeOrderNeedConfirm == null)
            {
                _preferenceManager.Config.ResumeOrderNeedConfirm = true;
                needUpdatePref = true;
            }

            if (_preferenceManager.Config.StrongChaseOrderNeedConfirm == null)
            {
                _preferenceManager.Config.StrongChaseOrderNeedConfirm = true;
                needUpdatePref = true;
            }

            if (_preferenceManager.Config.RevokeOrderNeedConfirm == null)
            {
                _preferenceManager.Config.RevokeOrderNeedConfirm = true;
                needUpdatePref = true;
            }

            if (needUpdatePref)
            {
                Exception _savePrefErr = null;
                _preferenceManager?.SaveConfig(out _savePrefErr);
                if (_savePrefErr != null) { AppLog.Error("Failed save preference.", _savePrefErr); }
            }
        }

        private void InitConfigXqThriftLibConfigurationManager()
        {
            XqThriftLibConfigurationManager.SharedInstance.HostingServerUrlMetaGetter = () =>
            {
                var loginResp = loginDataService.Value.ProxyLoginResp;
                if (loginResp == null) return null;
                return new XqThriftLibHostingServerUrlMeta
                {
                    HostingServerIP = loginResp.HostingServerIP,
                    HostingServerPort = loginResp.HostingProxyPort
                };
            };

            XqThriftLibConfigurationManager.SharedInstance.ThriftHttpLibSessionInvalidDetectedHandler = () =>
            {
                lock (sessionInvalidHandleLock)
                {
                    if (loginDataService.Value.ProxyLoginResp == null) return;
                    if (isSessionInvalidHandling || IsShowingStartupWindow) return;

                    isSessionInvalidHandling = true;
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        var msgDialog = messageWindowService.CreateMessageWindow(Application.Current.MainWindow, null, null,
                            "重要提示", "您的账号登录状态失效了！请重新登录！\n", "重新登录");
                        msgDialog.ShowDialog(true);

                        loginManService.Value.DoSignout();

                        lock (sessionInvalidHandleLock)
                        {
                            isSessionInvalidHandling = false;
                        }
                        ShowStartupUI();
                    });
                }
            };

            XqThriftLibConfigurationManager.SharedInstance.ThriftHttpLibEnvironmentChanged += XqThriftLibConfigurationManager_LibEnvironmentChanged;
        }

        private void ApplyOriginAppThemeConfig()
        {
            // apply current theme
            if (_preferenceManager != null)
            {
                ApplyTheme(_preferenceManager.Config.AppTheme);
            }
        }
        
        private void ApplyOriginLanguageConfig()
        {
            if (_preferenceManager != null)
            {
                var prefLang = _preferenceManager.Config.Language;
                XqAppLanguages appLang = XqAppLanguages.CN;
                if (!string.IsNullOrEmpty(prefLang))
                {
                    Enum.TryParse(prefLang, out appLang);
                }
                ApplyLanguage(appLang);
            }
        }

        private void ApplyOriginThriftHttpLibEnvironmentConfig()
        {
            // 配置 ThriftHttpLib 的环境、语言
            var isDevelopOpenStr = XueQiaoBusinessHelper.GetApplicationRegistryKey(XueQiaoConstants.RegistryKey_IsDevelopOpen);
            var isDevelopOpen = ("true" == isDevelopOpenStr.ToString()?.ToLower());

            lib.xqclient_base.thriftapi_mediation.Environment libEnv = lib.xqclient_base.thriftapi_mediation.Environment.DEV;
            if (!isDevelopOpen)
            {
                libEnv = lib.xqclient_base.thriftapi_mediation.Environment.IDC;
            }
            else
            {
                var storingLibEnv = Settings.Default.ThriftHttpLibEnv;
                if (!string.IsNullOrEmpty(storingLibEnv))
                {
                    Enum.TryParse(storingLibEnv, out libEnv);
                }
            }

            XqThriftLibConfigurationManager.SharedInstance.ThriftHttpLibEnvironment = libEnv;
        }

        private void XqThriftLibConfigurationManager_LibEnvironmentChanged(XqThriftLibConfigurationManager confManager, lib.xqclient_base.thriftapi_mediation.Environment environment)
        {
            Settings.Default.ThriftHttpLibEnv = environment.ToString();
            SaveCurrentModuleSettings();
        }

        private void UpgradeCurrentModuleSettingsIfNeed()
        {
            // Upgrade the settings from a previous version when the new version starts the first time.
            if (Settings.Default.IsUpgradeNeeded)
            {
                try
                {
                    Settings.Default.GetPreviousVersion(nameof(Settings.Default.IsUpgradeNeeded));
                    Settings.Default.Upgrade();
                    Settings.Default.IsUpgradeNeeded = false;
                    SaveCurrentModuleSettings();
                }
                catch
                {
                    // do nothing
                }
            }
        }

        private void SaveCurrentModuleSettings()
        {
            try
            {
                Settings.Default.Save();
            }
            catch (Exception e)
            {
                // When more application instances are closed at the same time then an exception occurs.
                AppLog.Error($"Save settings error: {e}");
            }
        }

        private void RecvAppShutdown()
        {
            Shutdown();
        }
    }
}
