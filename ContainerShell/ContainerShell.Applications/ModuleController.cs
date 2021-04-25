using ContainerShell.Interfaces.Applications;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using ContainerShell.Interfaces.DataModels;
using ContainerShell.Applications.Properties;
using ContainerShell.Applications.Controllers;
using System.Windows;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoFoundation.BusinessResources.DataModels;
using lib.xqclient_base.logger;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;
using xueqiao.trade.hosting.proxy;
using System.ComponentModel;
using System.ComponentModel.Composition.Hosting;
using AppAssembler.Interfaces.Applications;

namespace ContainerShell.Applications
{
    [Export(typeof(IModuleController)), Export(typeof(IContainerShellService)), PartCreationPolicy(CreationPolicy.Shared)]
    internal class ModuleController : IModuleController, IContainerShellService
    {
        private readonly CompositionContainer compositionContainer;
        private readonly ExportFactory<ContainerShellController> shellCtrlFactory;
        private readonly ExportFactory<ContractQuickSearchPopupController> contractQuickSearchPopupCtrlFactory;
        private readonly ExportFactory<ContractPreviewSelectPopupController> contractPreviewSelectCtrlFactory;
        private readonly ExportFactory<ExceptionOrderPanelWindowCtrl> exceptionOrderPanelWindowCtrlFactory;
        private readonly ExportFactory<TradeNotificationCtrl> tradeNotificationCtrlFactory;
        private readonly IMessageWindowService messageWindowService;
        private readonly Lazy<ILoginUserManageService> loginUserManageService;
        private readonly Lazy<ILoginDataService> loginDataService;
        private readonly Lazy<IAppAssemblerService> appAssemblerService;

        private IEnumerable<IContainerShellModuleServiceCtrl> moduleServiceCtrls;

        private ContainerShellController currentShellCtrl;
        private ExceptionOrderPanelWindowCtrl exceptionOrderPanelWindowCtrl;
        private TradeNotificationCtrl tradeNotificationCtrl;
        private bool _isXqInitializeDataInitalized;

        private bool shutdowned;

        [ImportingConstructor]
        public ModuleController(
            CompositionContainer compositionContainer,
            ExportFactory<ContainerShellController> shellCtrlFactory,
            ExportFactory<ContractQuickSearchPopupController> contractQuickSearchPopupCtrlFactory,
            ExportFactory<ContractPreviewSelectPopupController> contractPreviewSelectCtrlFactory,
            ExportFactory<ExceptionOrderPanelWindowCtrl> exceptionOrderPanelWindowCtrlFactory,
            ExportFactory<TradeNotificationCtrl> tradeNotificationCtrlFactory,
            IMessageWindowService messageWindowService,
            Lazy<ILoginUserManageService> loginUserManageService,
            Lazy<ILoginDataService> loginDataService,
            Lazy<IAppAssemblerService> appAssemblerService)
        {
            this.compositionContainer = compositionContainer;
            this.shellCtrlFactory = shellCtrlFactory;
            this.contractQuickSearchPopupCtrlFactory = contractQuickSearchPopupCtrlFactory;
            this.contractPreviewSelectCtrlFactory = contractPreviewSelectCtrlFactory;
            this.exceptionOrderPanelWindowCtrlFactory = exceptionOrderPanelWindowCtrlFactory;
            this.tradeNotificationCtrlFactory = tradeNotificationCtrlFactory;
            this.messageWindowService = messageWindowService;
            this.loginUserManageService = loginUserManageService;
            this.loginDataService = loginDataService;
            this.appAssemblerService = appAssemblerService;
        }
        
        public void Initialize()
        {
            // Upgrade the settings from a previous version when the new version starts the first time.
            // IsUpgradeNeeded default value is true
            if (Settings.Default.IsUpgradeNeeded)
            {
                try
                {
                    // 如果没加 GetPreviousVersion 这行程序，Upgrade 会默认找上一个版本（比如当前为1.0.1，它会默认找1.0.0）的数据进行升级。
                    // 如果没有找到，则数据不会升级过来。
                    // 加上 GetPreviousVersion 会找所有的版本，如果不存在 1.0.0， 则会找 0.0.9 版本的数据进行升级
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

            loginUserManageService.Value.HasLogouted += RecvUserHasLogouted;
            appAssemblerService.Value.AppShutdown += RecvAppShutdown;

            moduleServiceCtrls = compositionContainer.GetExportedValues<IContainerShellModuleServiceCtrl>()?.ToArray();
            if (moduleServiceCtrls?.Any() == true)
            {
                foreach (var ctrl in moduleServiceCtrls)
                {
                    ctrl.Initialize();
                }
            }
        }

        public void Run()
        {
            
        }
        
        public void Shutdown()
        {
            if (this.shutdowned) return;
            this.shutdowned = true;

            loginUserManageService.Value.HasLogouted -= RecvUserHasLogouted;
            appAssemblerService.Value.AppShutdown -= RecvAppShutdown;

            InternalShutdownCurrentShellCtrl();
            InternalShutdownExceptionOrderPanelCtrl();
            InternalShutdownTradeNotificationCtrl();

            SaveCurrentModuleSettings();

            if (moduleServiceCtrls?.Any() == true)
            {
                foreach (var ctrl in moduleServiceCtrls)
                {
                    ctrl.Shutdown();
                }
            }
        }


        void IContainerShellService.ShowShellWindow()
        {
            InternalShutdownCurrentShellCtrl();
            InternalShutdownExceptionOrderPanelCtrl();
            InternalShutdownTradeNotificationCtrl();

            // config currentShellCtrl
            currentShellCtrl = shellCtrlFactory.CreateExport().Value;
            currentShellCtrl.XqDataInitialized += CurrentShellCtrl_XqDataInitialized;
            currentShellCtrl.ShellClosing += ContainerShellWindow_Closing;
            currentShellCtrl.ShellClosed += ContainerShellWindow_Closed;

            currentShellCtrl.Initialize();
            currentShellCtrl.Run();

            // config tradeNotificationCtrl
            tradeNotificationCtrl = tradeNotificationCtrlFactory.CreateExport().Value;
            tradeNotificationCtrl.NotificationPositionWindow = currentShellCtrl.ShellWindow;
            tradeNotificationCtrl.BottomRightCornerPositionOffset = new Point(x: 20, y: 50);
            tradeNotificationCtrl.Initialize();
            tradeNotificationCtrl.Run();
        }

        public event CancelEventHandler ShellWindowClosing;

        public event EventHandler ShellWindowClosed;
        
        InitializeDataRoot IContainerShellService.InitializeData => currentShellCtrl?.InitializeDataRoot;

        public bool IsXqInitializeDataInitalized => _isXqInitializeDataInitalized;

        public event XqInitializeDataInitialized XqInitializeDataInitialized;

        public void ShowContractQuickSearchPopup(object popupPlaceTarget,
            IEnumerable<int> dataSourceCommodityIds,
            Action<int?> closePopupHandler)
        {
            var popupCtrl = contractQuickSearchPopupCtrlFactory.CreateExport().Value;
            popupCtrl.DataSourceCommodityIds = dataSourceCommodityIds;
            popupCtrl.PopupPalcementTarget = popupPlaceTarget;
            popupCtrl.PopupCloseHandler = (_ctrl, _selContractId) =>
            {
                _ctrl.Shutdown();
                closePopupHandler?.Invoke(_selContractId);
            };

            popupCtrl.Initialize();
            popupCtrl.Run();
        }

        public void ShowContractPreviewSelectPopup(object popupPlaceTarget,
            IEnumerable<int> dataSourceCommodityIds,
            Action<int?> closePopupHandler)
        {
            var popupCtrl = contractPreviewSelectCtrlFactory.CreateExport().Value;
            popupCtrl.DataSourceCommodityIds = dataSourceCommodityIds;
            popupCtrl.PopupPalcementTarget = popupPlaceTarget;
            popupCtrl.ClosePopupHandler = (_ctrl, _selectedContractId) => {
                _ctrl.Shutdown();
                closePopupHandler?.Invoke(_selectedContractId);
            };

            popupCtrl.Initialize();
            popupCtrl.Run();
        }

        public void ShowExceptionOrdersPanelWindow()
        {
            if (currentShellCtrl == null) return;
            var ownerWindow = currentShellCtrl.ShellWindow;
            if (ownerWindow == null) return;

            if (exceptionOrderPanelWindowCtrl == null)
            {
                exceptionOrderPanelWindowCtrl = exceptionOrderPanelWindowCtrlFactory.CreateExport().Value;
                exceptionOrderPanelWindowCtrl.WindowOwner = ownerWindow;
                exceptionOrderPanelWindowCtrl.ClosedHandler = () => 
                {
                    InternalShutdownExceptionOrderPanelCtrl();
                };

                exceptionOrderPanelWindowCtrl.Initialize();
                exceptionOrderPanelWindowCtrl.Run();
            }

            exceptionOrderPanelWindowCtrl.Show();
        }

        public void HideExceptionOrdersPanelWindow()
        {
            exceptionOrderPanelWindowCtrl?.Hide();
        }

        public bool ExceptionOrdersPanelWindowIsShow => exceptionOrderPanelWindowCtrl?.WindowIsShow ?? false;
        
        public void ActiveExceptionOrderTabInExceptionOrderPanel(OrderItemDataModel moveToAndSelectItem)
        {
            exceptionOrderPanelWindowCtrl?.ActiveExceptionOrderTabInPanel(moveToAndSelectItem);
        }

        public void ActiveAmbiguousOrderTabInExceptionOrderPanel(OrderItemDataModel moveToAndSelectItem)
        {
            exceptionOrderPanelWindowCtrl?.ActiveAmbiguousOrderTabInPanel(moveToAndSelectItem);
        }

        public void ActiveTradeLameTabInExceptionOrderPanel(XQTradeLameTaskNote moveToAndSelectItem)
        {
            exceptionOrderPanelWindowCtrl?.ActiveTradeLameTabInPanel(moveToAndSelectItem);
        }

        /// <summary>
        /// 显示订单发生异常 Notification
        /// </summary>
        public void ShowOrderOccurExceptionNotification(OrderItemDataModel order)
        {
            tradeNotificationCtrl?.ShowOrderOccurExceptionNotification(order);
        }

        /// <summary>
        /// 显示订单已触发 Notification
        /// </summary>
        public void ShowOrderTriggeredNotification(OrderItemDataModel order)
        {
            tradeNotificationCtrl?.ShowOrderTriggeredNotification(order);
        }

        /// <summary>
        /// 显示订单已成交 Notification
        /// </summary>
        public void ShowOrderTradedNotification(TradeItemDataModel trade)
        {
            tradeNotificationCtrl?.ShowOrderTradedNotification(trade);
        }

        /// <summary>
        /// 显示订单状态异常 Notification
        /// </summary>
        public void ShowOrderStateAmbiguousNotification(OrderItemDataModel order)
        {
            tradeNotificationCtrl?.ShowOrderStateAmbiguousNotification(order);
        }

        public void ShowTradeLameTaskNoteNotification(XQTradeLameTaskNote lameTaskNote)
        {
            tradeNotificationCtrl?.ShowTradeLameTaskNoteNotification(lameTaskNote);
        }

        private void InternalShutdownCurrentShellCtrl()
        {
            if (currentShellCtrl != null)
            {
                currentShellCtrl.XqDataInitialized -= CurrentShellCtrl_XqDataInitialized;
                currentShellCtrl.ShellClosing -= ContainerShellWindow_Closing;
                currentShellCtrl.ShellClosed -= ContainerShellWindow_Closed;
                currentShellCtrl.Shutdown();
                currentShellCtrl = null;
            }
        }
    
        private void CurrentShellCtrl_XqDataInitialized(InitializeDataRoot obj)
        {
            if (loginDataService.Value.ProxyLoginResp == null) return;

            this._isXqInitializeDataInitalized = true;
            XqInitializeDataInitialized?.Invoke(obj);
        }

        private void InternalShutdownExceptionOrderPanelCtrl()
        {
            if (exceptionOrderPanelWindowCtrl != null)
            {
                exceptionOrderPanelWindowCtrl.Shutdown();
                exceptionOrderPanelWindowCtrl = null;
            }
        }

        private void InternalShutdownTradeNotificationCtrl()
        {
            if (tradeNotificationCtrl != null)
            {
                tradeNotificationCtrl.Shutdown();
                tradeNotificationCtrl = null;
            }
        }

        private void ContainerShellWindow_Closing(object sender, CancelEventArgs e)
        {
            ShellWindowClosing?.Invoke(this, e);
        }

        private void ContainerShellWindow_Closed(object sender, EventArgs e)
        {
            ShellWindowClosed?.Invoke(this, e);
        }

        private void RecvUserHasLogouted(ProxyLoginResp lastLoginResp)
        {
            InternalShutdownCurrentShellCtrl();
            InternalShutdownExceptionOrderPanelCtrl();
            InternalShutdownTradeNotificationCtrl();
            _isXqInitializeDataInitalized = false;
        }

        private void RecvAppShutdown()
        {
            Shutdown();
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
    }
}
