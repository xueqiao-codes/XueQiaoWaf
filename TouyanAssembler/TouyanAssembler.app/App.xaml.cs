using CefSharp;
using CefSharp.Wpf;
using lib.xqclient_base.logger;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows;
using System.Windows.Markup;
using TouyanAssembler.app.Properties;
using TouyanAssembler.BusinessResource.constant;
using TouyanAssembler.BusinessResource.helper;
using TouyanAssembler.Interface.application;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;

namespace TouyanAssembler.app
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private AggregateCatalog catalog;
        private CompositionContainer container;

        private IEventAggregator eventAggregator;
        private IEnumerable<IModuleController> moduleControllers;
        private ITouyanAssemblerService appAssemblerService;

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
            AppLog.Init(Path.Combine(Constants.AppLocalDataDirectoryFullName, "log4net_log"));
            AppLog.Info("App OnStartup.");

            SetupRegisterKey_IsDevelopOpenIfNeed();

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
            
            // Initialize all presentation services
            var presentationServices = container.GetExportedValues<IPresentationService>();
            foreach (var presentationService in presentationServices) { presentationService.Initialize(); }

            // Initialize and run all module controllers before show UI
            moduleControllers = container.GetExportedValues<IModuleController>().ToArray();
            foreach (var moduleController in moduleControllers) { moduleController.Initialize(); }
            foreach (var moduleController in moduleControllers) { moduleController.Run(); }

            appAssemblerService = container.GetExportedValue<ITouyanAssemblerService>();

            // Show UI
            appAssemblerService.ResetToStartupUI();
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
                    CachePath = Path.Combine(Constants.AppSetupDirectoryPath, "CefSharp", "Cache")
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
            var isDevelopOpen = BusinessHelper.GetApplicationRegistryKey(Constants.RegistryKey_IsDevelopOpen);
            if (isDevelopOpen == null)
            {
                bool defaultValue = false;
#if DEBUG
                defaultValue = true;
#endif
                BusinessHelper.SetApplicationRegistryKey(Constants.RegistryKey_IsDevelopOpen, defaultValue);
            }
        }
    }
}
