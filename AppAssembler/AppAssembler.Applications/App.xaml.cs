using AppAssembler.Applications.Properties;
using AppAssembler.Interfaces.Applications;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows;
using System.Windows.Markup;
using XueQiaoFoundation.BusinessResources.Helpers;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using Microsoft.HockeyApp;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows.Media;
using lib.xqclient_base.logger;
using System.Runtime.CompilerServices;
using CefSharp.Wpf;
using CefSharp;
using XueQiaoFoundation.BusinessResources.Constants;

namespace AppAssembler.Applications
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private AggregateCatalog catalog;
        private CompositionContainer container;

        private IEventAggregator eventAggregator;
        private IAppAssemblerService appAssemblerService;
        private IEnumerable<IModuleController> moduleControllers;
        
        public App()
        {
            //Any CefSharp references have to be in another method with NonInlining
            // attribute so the assembly rolver has time to do it's thing.
            InitializeCefSharp();
        }
        
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            /// <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.windows.frameworkcompatibilitypreferences.keeptextboxdisplaysynchronizedwithtextproperty?redirectedfrom=MSDN&view=netframework-4.7.2#System_Windows_FrameworkCompatibilityPreferences_KeepTextBoxDisplaySynchronizedWithTextProperty"/>
            //System.Windows.FrameworkCompatibilityPreferences
            //   .KeepTextBoxDisplaySynchronizedWithTextProperty = false;

            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(
                XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            // 配置日志系统
            AppLog.Init(Path.Combine(XueQiaoConstants.AppLocalDataDirectoryFullName, "log4net_log"));
            AppLog.Info("App OnStartup.");

            SetupRegisterKey_IsDevelopOpenIfNeed();
            SetupGlobalFontFamily();
            SetupHockeyClient();
            
            // Initialize `DispatcherHelper` first
            DispatcherHelper.Initialize();
            
            catalog = new AggregateCatalog();

            // 已经在 `Settings.Default.ModuleAssemblies` 中添加了 XueQiaoFoundation.UI 不需要重复添加
            // Add the `XueQiaoFoundation.UI` Assembly
            //catalog.Catalogs.Add(new AssemblyCatalog(typeof(IMessageWindowService).Assembly));

            // Load module assemblies as well. See App.config file.
            foreach (string moduleAssembly in Settings.Default.ModuleAssemblies)
            {
                catalog.Catalogs.Add(new AssemblyCatalog(moduleAssembly));
            }


            // Add the current assembly to the catalog
            catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
            
            container = new CompositionContainer(catalog, CompositionOptions.DisableSilentRejection);
            CompositionBatch batch = new CompositionBatch();
            batch.AddExportedValue(container);
            container.Compose(batch);

            eventAggregator = new EventAggregator();
            container.ComposeExportedValue<IEventAggregator>(typeof(IEventAggregator).ToString(),
                eventAggregator);
            
            appAssemblerService = container.GetExportedValue<IAppAssemblerService>();
            
            // Initialize all presentation services
            var presentationServices = container.GetExportedValues<IPresentationService>();
            foreach (var presentationService in presentationServices) { presentationService.Initialize(); }

            // Initialize and run all module controllers before show UI
            moduleControllers = container.GetExportedValues<IModuleController>().ToArray();
            foreach (var moduleController in moduleControllers) { moduleController.Initialize(); }
            foreach (var moduleController in moduleControllers) { moduleController.Run(); }
            
            // Show UI
            appAssemblerService.ShowStartupUI();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            foreach (var moduleController in moduleControllers.Reverse()) { moduleController.Shutdown(); }
            container.Dispose();
            catalog.Dispose();

            base.OnExit(e);
            AppLog.Info("App OnExit.");
        }
        
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void InitializeCefSharp()
        {
            bool isBuildWithAnyCPU = false;
            // Any CPU 模式
#if AnyCPU
            isBuildWithAnyCPU = true;
#endif
            if (isBuildWithAnyCPU)
            {
                //Add Custom assembly resolver
                AppDomain.CurrentDomain.AssemblyResolve += Resolver;

                //Perform dependency check to make sure all relevant resources are in our output directory.
                var settings = new CefSettings();
                settings.BrowserSubprocessPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                                                       Environment.Is64BitProcess ? "x64" : "x86",
                                                       "CefSharp.BrowserSubprocess.exe");

                Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);
            }
            else
            {
                // x64, x86 模式
                //Monitor parent process exit and close subprocesses if parent process exits first
                //This will at some point in the future becomes the default
                CefSharpSettings.SubprocessExitIfParentProcessClosed = true;

                var settings = new CefSettings()
                {
                    //By default CefSharp will use an in-memory cache, you need to specify a Cache Folder to persist data
                    CachePath = Path.Combine(PathHelper.AppSetupDirectoryPath, "CefSharp", "Cache")
                };

                //Example of setting a command line argument
                //Enables WebRTC
                settings.CefCommandLineArgs.Add("enable-media-stream", "1");

                //Perform dependency check to make sure all relevant resources are in our output directory.
                Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);
            }
        }

        // Will attempt to load missing assembly from either x86 or x64 subdir
        // Required by CefSharp to load the unmanaged dependencies when running using AnyCPU
        private static Assembly Resolver(object sender, ResolveEventArgs args)
        {
            if (args.Name.StartsWith("CefSharp"))
            {
                string assemblyName = args.Name.Split(new[] { ',' }, 2)[0] + ".dll";
                string archSpecificPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                                                       Environment.Is64BitProcess ? "x64" : "x86",
                                                       assemblyName);

                return File.Exists(archSpecificPath)
                           ? Assembly.LoadFile(archSpecificPath)
                           : null;
            }

            return null;
        }
        
        private const string X86APPID_FOR_HOCKEYAPP = "73b8c8e00e1c4c8e9995a6d869bbe519";
        private const string X64APPID_FOR_HOCKEYAPP = "30ff83da158249a4938d43f2c12df272";

        private async void SetupHockeyClient()
        {
            var deviceSysInfo = DeviceSystemInfomationHelper.DeviceSystemInformation();

            // The diffrent and usage of DispatcherUnhandledException, TaskScheduler.UnobservedTaskException, AppDomain.CurrentDomain.UnhandledException, AppDomain.CurrentDomain.FirstChanceException, System.Windows.Forms.Application.ThreadException
            // see https://code.msdn.microsoft.com/windowsdesktop/Handling-Unhandled-47492d0b
            //
            //main configuration of HockeySDK
            var myAppId = Environment.Is64BitProcess ? X64APPID_FOR_HOCKEYAPP : X86APPID_FOR_HOCKEYAPP;
            HockeyClient.Current.Configure(myAppId)
                //.UseCustomResourceManager(HockeyApp.ResourceManager) //register your own resourcemanager to override HockeySDK i18n strings
                .RegisterCustomUnhandledExceptionLogic((eArgs) => LogCurrentDomainUnhandledException(eArgs?.ExceptionObject as Exception)) // define a callback that is called after unhandled exception
                .RegisterCustomUnobserveredTaskExceptionLogic((eArgs) => LogTaskSchedulerUnobservedTaskException(eArgs?.Exception)) // define a callback that is called after unobserved task exception
                .RegisterCustomDispatcherUnhandledExceptionLogic((args) => LogDispatcherUnhandledException(args?.Exception)) // define a callback that is called after dispatcher unhandled exception
                //.SetApiDomain("https://your.hockeyapp.server")
                //.SetContactInfo("pengguangbo", "devbool@126.com")
                .SetExceptionDescriptionLoader((Exception ex) =>
                  {
                      return $"{deviceSysInfo.ToString()}";
                  });
            
            //optional should only used in debug builds. register an event-handler to get exceptions in HockeySDK code that are "swallowed" (like problems writing crashlogs etc.)
#if DEBUG
            ((HockeyClient)HockeyClient.Current).OnHockeySDKInternalException += (sender, args) =>
            {
                if (Debugger.IsAttached) { Debugger.Break(); }
            };
#endif

            //send crashes to the HockeyApp server
            await HockeyClient.Current.SendCrashesAsync(true);
        }

        private void LogCurrentDomainUnhandledException(Exception e)
        {
            if (e != null) AppLog.Error($"CurrentDomain.UnhandledException. {e.Message}", e);
        }

        private void LogDispatcherUnhandledException(Exception e)
        {
            if (e != null) AppLog.Error($"DispatcherUnhandledException. {e.Message}", e);
        }

        private void LogTaskSchedulerUnobservedTaskException(Exception e)
        {
            if (e != null) AppLog.Error($"TaskScheduler.UnobservedTaskException. {e.Message}", e);
        }

        private void SetupRegisterKey_IsDevelopOpenIfNeed()
        {
            var isDevelopOpen = XueQiaoBusinessHelper.GetApplicationRegistryKey(XueQiaoConstants.RegistryKey_IsDevelopOpen);
            if (isDevelopOpen == null)
            {
                bool defaultValue = false;
#if DEBUG
                defaultValue = true;
#endif
                XueQiaoBusinessHelper.SetApplicationRegistryKey(XueQiaoConstants.RegistryKey_IsDevelopOpen, defaultValue);
            }
        }

        private void SetupGlobalFontFamily()
        {
            // FIXME:不使用外部字体，外部字体文件太大，因此不引入
            //var defaultFamily = new FontFamily(new Uri("pack://application:,,,/"), "Segoe UI, Fonts/#思源黑体");
            var defaultFamily = new FontFamily("Segoe UI, Microsoft YaHei");
            TextElement.FontFamilyProperty.OverrideMetadata(typeof(TextElement), 
                new FrameworkPropertyMetadata(defaultFamily));

            TextBlock.FontFamilyProperty.OverrideMetadata(typeof(TextBlock), 
                new FrameworkPropertyMetadata(defaultFamily));
        }
    }
}
