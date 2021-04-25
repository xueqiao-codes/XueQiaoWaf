using business_foundation_lib.xq_thriftlib_config;
using lib.xqclient_base.logger;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Touyan.Interface.application;
using TouyanAssembler.app.controller;
using TouyanAssembler.app.Properties;
using TouyanAssembler.app.view;
using TouyanAssembler.BusinessResource.constant;
using TouyanAssembler.BusinessResource.helper;
using TouyanAssembler.Interface.application;
using xueqiao.graph.xiaoha.chart.terminal.ao.thriftapi;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Model;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;

namespace TouyanAssembler.app
{
    [
        Export(typeof(IModuleController)),
        Export(typeof(ITouyanAssemblerService)),
        Export(typeof(ILoginDataService)),
        Export(typeof(ILoginModuleService)),
        Export(typeof(IContainerShellModuleService)),
        PartCreationPolicy(CreationPolicy.Shared)
    ]
    internal class ModuleController : IModuleController, ITouyanAssemblerService, 
        ILoginDataService, ILoginModuleService, IContainerShellModuleService
    {
        private readonly CompositionContainer compositionContainer;
        private readonly IMessageWindowService messageWindowService;
        private readonly ExportFactory<ContainerShellWindowCtrl> containerShellWindowCtrlFactory;
        private readonly ExportFactory<ITouyanModuleRootViewCtrl> touyanModuleRootViewCtrlFactory;
        private readonly ExportFactory<TradeModuleRootViewCtrl> tradeModuleRootViewCtrlFactory;
        private readonly ExportFactory<LoginRegDialogCtrl> loginRegDialogCtrlFactory;

        private IEnumerable<ITouyanAssemblerModuleServiceCtrl> moduleServiceCtrls; 

        private ContainerShellWindowCtrl containerShellWindowCtrl;
        private ITouyanModuleRootViewCtrl touyanModuleRootViewCtrl;
        private TradeModuleRootViewCtrl tradeModuleRootViewCtrl;

        private readonly object sessionInvalidHandleLock = new object();
        private bool isSessionInvalidHandling = false;

        private bool shutdowned;

        [ImportingConstructor]
        public ModuleController(
            CompositionContainer compositionContainer,
            IMessageWindowService messageWindowService,
            ExportFactory<ContainerShellWindowCtrl> containerShellWindowCtrlFactory,
            ExportFactory<ITouyanModuleRootViewCtrl> touyanModuleRootViewCtrlFactory,
            ExportFactory<TradeModuleRootViewCtrl> tradeModuleRootViewCtrlFactory,
            ExportFactory<LoginRegDialogCtrl> loginRegDialogCtrlFactory)
        {
            this.compositionContainer = compositionContainer;
            this.messageWindowService = messageWindowService;
            this.containerShellWindowCtrlFactory = containerShellWindowCtrlFactory;
            this.touyanModuleRootViewCtrlFactory = touyanModuleRootViewCtrlFactory;
            this.tradeModuleRootViewCtrlFactory = tradeModuleRootViewCtrlFactory;
            this.loginRegDialogCtrlFactory = loginRegDialogCtrlFactory;
        }

        public void Initialize()
        {
            InitConfigXqThriftLibConfigurationManager();

            this.AppShutdown += RecvAppShutdown;

            ApplyOriginThriftHttpLibEnvironmentConfig();
        }

        public void Run()
        {
            moduleServiceCtrls = compositionContainer.GetExportedValues<ITouyanAssemblerModuleServiceCtrl>();
            if (moduleServiceCtrls != null)
            {
                foreach (var _ctrl in moduleServiceCtrls)
                {
                    _ctrl.Initialize();
                }
            }
        }

        public void Shutdown()
        {
            if (this.shutdowned) return;
            this.shutdowned = true;

            XqThriftLibConfigurationManager.SharedInstance.ThriftHttpLibEnvironmentChanged -= XqThriftLibConfigurationManager_LibEnvironmentChanged;

            this.AppShutdown -= RecvAppShutdown;

            touyanModuleRootViewCtrl?.Shutdown();
            touyanModuleRootViewCtrl = null;

            tradeModuleRootViewCtrl?.Shutdown();
            tradeModuleRootViewCtrl = null;

            InternalShutdownContainerShellWindowCtrl();

            if (moduleServiceCtrls != null)
            {
                foreach (var _ctrl in moduleServiceCtrls)
                {
                    _ctrl.Shutdown();
                }
            }
        }

        #region ITouyanAssemblerService
        
        public void ResetToStartupUI()
        {
            // 关闭所有窗口
            ShellWindowClosed -= ContainerShell_Closed;
            ShellWindowClosing -= ContainerShell_Closing;
            foreach (var win in System.Windows.Application.Current.Windows)
            {
                if (win is System.Windows.Window _win)
                {
                    _win.Close();
                }
            }

            ShellWindowClosed += ContainerShell_Closed;
            ShellWindowClosing += ContainerShell_Closing;

            ShowShellWindow();
        }
        
        public void ShutdownApplication()
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                AppShutdown?.Invoke();
                Application.Current?.Shutdown();
            }, System.Windows.Threading.DispatcherPriority.Normal);
        }

        public event AppShutdown AppShutdown;

        #endregion

        #region ILoginDataService

        private XiaohaChartLandingInfo _loginLandingInfo;
        public XiaohaChartLandingInfo LoginLandingInfo => _loginLandingInfo;

        #endregion

        #region ILoginModuleService

        public event HasLogined HasLogined;
        public event IsLogouting IsLogouting;
        public event HasLogouted HasLogouted;

        public bool? ShowLoginDialog(object dialogOwner, Action dialogContentRendered)
        {
            return ShowLoginRegDialog(dialogOwner, dialogContentRendered, false);
        }

        public bool? ShowRegisterDialog(object dialogOwner, Action dialogContentRendered)
        {
            return ShowLoginRegDialog(dialogOwner, dialogContentRendered, true);
        }

        public bool? ShowLoginRegDialog(object dialogOwner, Action dialogContentRendered, bool initialIsRegisterView)
        {
            var dialogCtrl = loginRegDialogCtrlFactory.CreateExport().Value;
            dialogCtrl.InitialIsRegisterView = initialIsRegisterView;
            dialogCtrl.DialogOwner = dialogOwner;
            dialogCtrl.DialogContentRendered = dialogContentRendered;
            dialogCtrl.DidLoginedHandler = (_ctrl, _landingInfo) =>
            {
                this._loginLandingInfo = _landingInfo;
                HasLogined?.Invoke();
            };

            dialogCtrl.Initialize();
            dialogCtrl.Run();
            dialogCtrl.Shutdown();

            return dialogCtrl.LoginOrRegResult;
        }

        public void DoSignout()
        {
            var lastLoginLandingInfo = this._loginLandingInfo;
            if (lastLoginLandingInfo == null)
            {
                // 已经登出，无需再次退出
                return;
            }

            // 发布将要退出的通知
            IsLogouting?.Invoke(lastLoginLandingInfo);

            //TODO: StopSessionHeartbeatTimer ???
            //StopSessionHeartbeatTimer();
            this._loginLandingInfo = null;
            
            // 发布已经退出的通知
            HasLogouted?.Invoke(lastLoginLandingInfo);
        }

        #endregion

        #region IContainerShellModuleService

        public event CancelEventHandler ShellWindowClosing;
        public event EventHandler ShellWindowClosed;
        
        public void ShowShellWindow()
        {
            if (containerShellWindowCtrl == null)
            {
                containerShellWindowCtrl = containerShellWindowCtrlFactory.CreateExport().Value;
                containerShellWindowCtrl.ShellClosing += OnCurrentContainerShellWindowClosing;
                containerShellWindowCtrl.ShellClosed += OnCurrentContainerShellWindowClosed;
                containerShellWindowCtrl.Initialize();
                containerShellWindowCtrl.Run();
            }
        }
        
        public void GetTradeModuleRootView(Func<ChromeWindowCaptionDataHolder> embedInWindowCaptionDataHolderGetter,
            out object moduleRootView, out Action showAction, out Action closeAction)
        {
            closeAction = null;
            showAction = null;

            if (tradeModuleRootViewCtrl == null)
            {
                tradeModuleRootViewCtrl = tradeModuleRootViewCtrlFactory.CreateExport().Value;
                tradeModuleRootViewCtrl.EmbedInWindowCaptionDataHolder = embedInWindowCaptionDataHolderGetter?.Invoke();
                tradeModuleRootViewCtrl.Initialize();
                tradeModuleRootViewCtrl.Run();
            }

            moduleRootView = tradeModuleRootViewCtrl.ContentView;
        }

        public void GetTouyanModuleRootView(Func<ChromeWindowCaptionDataHolder> embedInWindowCaptionDataHolderGetter,
            out object moduleRootView, out Action showAction, out Action closeAction)
        {
            closeAction = null;
            showAction = null;

            if (touyanModuleRootViewCtrl == null)
            {
                touyanModuleRootViewCtrl = touyanModuleRootViewCtrlFactory.CreateExport().Value;
                touyanModuleRootViewCtrl.EmbedInWindowCaptionDataHolder = embedInWindowCaptionDataHolderGetter?.Invoke();
                touyanModuleRootViewCtrl.Initialize();
                touyanModuleRootViewCtrl.Run();
            }

            moduleRootView = touyanModuleRootViewCtrl.ContentView;

        }

        #endregion

        private void InitConfigXqThriftLibConfigurationManager()
        {
            XqThriftLibConfigurationManager.SharedInstance.HostingServerUrlMetaGetter = () =>
            {
                return null;
            };

            XqThriftLibConfigurationManager.SharedInstance.ThriftHttpLibSessionInvalidDetectedHandler = () =>
            {
                lock (sessionInvalidHandleLock)
                {
                    if (_loginLandingInfo == null) return;
                    if (isSessionInvalidHandling) return;

                    isSessionInvalidHandling = true;
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        var msgDialog = messageWindowService.CreateMessageWindow(Application.Current.MainWindow, null, null,
                            "重要提示", "您的账号登录状态失效了！请重新登录！\n", "重新登录");
                        msgDialog.ShowDialog(true);

                        DoSignout();

                        lock (sessionInvalidHandleLock)
                        {
                            isSessionInvalidHandling = false;
                        }

                        ShowLoginDialog(Application.Current.MainWindow, null);
                    });
                }
            };

            XqThriftLibConfigurationManager.SharedInstance.ThriftHttpLibEnvironmentChanged += XqThriftLibConfigurationManager_LibEnvironmentChanged;
        }

        private void ApplyOriginThriftHttpLibEnvironmentConfig()
        {
            // 配置 ThriftHttpLib 的环境、语言
            var isDevelopOpenStr = BusinessHelper.GetApplicationRegistryKey(Constants.RegistryKey_IsDevelopOpen);
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

        private void ContainerShell_Closed(object sender, EventArgs e)
        {
            ShellWindowClosed -= ContainerShell_Closed;
            ShellWindowClosing -= ContainerShell_Closing;

            ShutdownApplication();
        }

        private void ContainerShell_Closing(object sender, CancelEventArgs e)
        {
            var confirmCloseApp = messageWindowService.ShowQuestionDialog(Application.Current.MainWindow, null, null, "重要提示", "\n         您确认要关闭软件吗？         \n",
                false, "关闭", "取消");
            e.Cancel = confirmCloseApp != true;
        }

        private void InternalShutdownContainerShellWindowCtrl()
        {
            if (containerShellWindowCtrl != null)
            {
                containerShellWindowCtrl.ShellClosing -= OnCurrentContainerShellWindowClosing;
                containerShellWindowCtrl.ShellClosed -= OnCurrentContainerShellWindowClosed;
                containerShellWindowCtrl.Shutdown();
                containerShellWindowCtrl = null;
            }
        }

        private void OnCurrentContainerShellWindowClosing(object sender, CancelEventArgs e)
        {
            ShellWindowClosing?.Invoke(sender, e);
        }

        private void OnCurrentContainerShellWindowClosed(object sender, EventArgs e)
        {
            ShellWindowClosed?.Invoke(sender, e);
        }
    }
}
