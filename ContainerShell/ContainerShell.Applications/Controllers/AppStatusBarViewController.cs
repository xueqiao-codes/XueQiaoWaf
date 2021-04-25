using AppAssembler.Interfaces.Applications;
using business_foundation_lib.xq_thriftlib_config;
using ContainerShell.Applications.ViewModels;
using ContainerShell.Interfaces.Applications;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using xueqiao.trade.hosting;
using XueQiaoFoundation.BusinessResources.Constants;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.BusinessResources.Helpers;
using XueQiaoFoundation.Interfaces.Applications;
using XueQiaoFoundation.Interfaces.Event.quotation_server;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;
using XueQiaoWaf.Trade.Interfaces.Services;

namespace ContainerShell.Applications.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class AppStatusBarViewController : IController
    {
        private readonly AppStatusBarVM contentVM;
        private readonly IOrderItemsService orderItemsService;
        private readonly IXQTradeLameTaskNoteService tradeLameTaskNoteService;
        private readonly IQuotationEngineController quotationEngineController;
        private readonly IContainerShellService containerShellService;
        private readonly ILoginDataService loginDataService;
        private readonly IAppAssemblerService appAssemblerService;
        private readonly IEventAggregator eventAggregator;

        private readonly DelegateCommand openOrHideExceptionOrderPanelCmd;

        [ImportingConstructor]
        public AppStatusBarViewController(
            AppStatusBarVM contentVM,
            IOrderItemsService orderItemsService,
            IXQTradeLameTaskNoteService tradeLameTaskNoteService,
            IQuotationEngineController quotationEngineController,
            IContainerShellService containerShellService,
            ILoginDataService loginDataService,
            IAppAssemblerService appAssemblerService,
            IEventAggregator eventAggregator)
        {
            this.contentVM = contentVM;
            this.orderItemsService = orderItemsService;
            this.tradeLameTaskNoteService = tradeLameTaskNoteService;
            this.quotationEngineController = quotationEngineController;
            this.containerShellService = containerShellService;
            this.loginDataService = loginDataService;
            this.appAssemblerService = appAssemblerService;
            this.eventAggregator = eventAggregator;

            openOrHideExceptionOrderPanelCmd = new DelegateCommand(OpenOrHideExceptionOrderPanel);
        }

        public object ContentView => contentVM.View;

        public void Initialize()
        {
            CollectionChangedEventManager.AddHandler(orderItemsService.OrderItems, OrderItemsChanged);
            PropertyChangedEventManager.AddHandler(tradeLameTaskNoteService.TaskNoteItems, TradeLameTaskNoteItemsPropChanged, "");

            contentVM.OpenOrHideExceptionOrderPanelCmd = openOrHideExceptionOrderPanelCmd;

            appAssemblerService.AppPerformanceDataChanged += RecvAppPerformanceDataChanged;
            eventAggregator.GetEvent<ServerConnectOpen>().Subscribe(ReceivedQuotationServerConnectOpenEvent);
            eventAggregator.GetEvent<ServerConnectClose>().Subscribe(ReceivedQuotationServerConnectCloseEvent);
        }
        
        public void Run()
        {
            var isDevelopOpenStr = XueQiaoBusinessHelper.GetApplicationRegistryKey(XueQiaoConstants.RegistryKey_IsDevelopOpen);
            var isDevelopOpen = ("true" == isDevelopOpenStr.ToString()?.ToLower());
            contentVM.ApiLibEnvironmentInfo = isDevelopOpen ?
                $"接口环境：{XqThriftLibConfigurationManager.SharedInstance.ThriftHttpLibEnvironment.ToString()}" : null;

            UpdateDisplay_LoginInfo();
            UpdateDisplay_QuotationPushState();
            InvalidateTradeExceptionItemCount();
        }

        public void Shutdown()
        {
            CollectionChangedEventManager.RemoveHandler(orderItemsService.OrderItems, OrderItemsChanged);
            PropertyChangedEventManager.RemoveHandler(tradeLameTaskNoteService.TaskNoteItems, TradeLameTaskNoteItemsPropChanged, "");

            appAssemblerService.AppPerformanceDataChanged -= RecvAppPerformanceDataChanged;
            eventAggregator.GetEvent<ServerConnectOpen>().Unsubscribe(ReceivedQuotationServerConnectOpenEvent);
            eventAggregator.GetEvent<ServerConnectClose>().Unsubscribe(ReceivedQuotationServerConnectCloseEvent);
        }

        private void OrderItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var newItems = e.NewItems?.OfType<OrderItemDataModel>().ToArray();
            var oldItems = e.OldItems?.OfType<OrderItemDataModel>().ToArray();
            if (newItems?.Any() == true)
            {
                foreach (var orderItem in newItems)
                {
                    PropertyChangedEventManager.RemoveHandler(orderItem, OrderItemPropChanged, "");
                    PropertyChangedEventManager.AddHandler(orderItem, OrderItemPropChanged, "");
                }
            }
            if (oldItems?.Any() == true)
            {
                foreach (var orderItem in oldItems)
                {
                    PropertyChangedEventManager.RemoveHandler(orderItem, OrderItemPropChanged, "");
                }
            }
            InvalidateTradeExceptionItemCount();
        }

        private void OrderItemPropChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(OrderItemDataModel.IsSuspendedWithError)
                || e.PropertyName == nameof(OrderItemDataModel.IsStateAmbiguous))
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() => 
                {
                    InvalidateTradeExceptionItemCount();
                });
            }
        }

        private void InvalidateTradeExceptionItemCount()
        {
            var exceptionOrderItemCount = orderItemsService.OrderItems.Count(i => i.IsSuspendedWithError || i.IsStateAmbiguous);
            var tradeLameTaskNoteCount = tradeLameTaskNoteService.TaskNoteItems.Count;

            contentVM.TradeExceptionItemCount = (exceptionOrderItemCount + tradeLameTaskNoteCount);
        }
        
        private void TradeLameTaskNoteItemsPropChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Count")
            {
                InvalidateTradeExceptionItemCount();
            }
        }
        
        private void RecvAppPerformanceDataChanged(business_foundation_lib.performance_monitor.AppPerformanceData performanceData)
        {
            if (performanceData != null)
            {
                contentVM.TotalCpuUsage = $"{performanceData.TotalCpuUsage.ToString("F1")}%";
                contentVM.AppCpuUsage = $"{performanceData.CurrentProcessCpuUsage.ToString("F1")}%";
                contentVM.AppMemoryUsage = $"{(performanceData.WorkingSetPrivateMemory / (1024 * 1024)).ToString("F1")}M";
                contentVM.RestAvailableMemory = $"{performanceData.AvailableMBytes.ToString("F1")}M";
                contentVM.AppThreadCount = $"{performanceData.ThreadCount}";
            }
        }

        private void ReceivedQuotationServerConnectOpenEvent(ServerConnectOpenEventMsg eventMsg)
        {
            UpdateDisplay_QuotationPushState();
        }

        private void ReceivedQuotationServerConnectCloseEvent()
        {
            UpdateDisplay_QuotationPushState();
        }

        private void OpenOrHideExceptionOrderPanel()
        {
            if (containerShellService.ExceptionOrdersPanelWindowIsShow)
                containerShellService.HideExceptionOrdersPanelWindow();
            else
                containerShellService.ShowExceptionOrdersPanelWindow();
        }
        
        private void UpdateDisplay_LoginInfo()
        {
            contentVM.LoginUserName = loginDataService.ProxyLoginResp?.LoginUserInfo?.LoginName;
            contentVM.LoginCompanyName = loginDataService.CompanyCode;
            contentVM.LoginCompanyGroupName = loginDataService.CompanyGroup?.GroupName;
            contentVM.HostingRunningMode = loginDataService.ProxyLoginResp?.HostingRunningMode ?? HostingRunningMode.SIM_HOSTING;
        }

        private void UpdateDisplay_QuotationPushState()
        {
            string stateMsg = null;
            var isConnected = quotationEngineController.IsQuotationServerConnected;
            if (isConnected)
            {
                stateMsg = "已连接";
            }
            else
            {
                stateMsg = "未连接";
            }
            contentVM.QuotationPushStateMsg = stateMsg;
        }
    }
}
