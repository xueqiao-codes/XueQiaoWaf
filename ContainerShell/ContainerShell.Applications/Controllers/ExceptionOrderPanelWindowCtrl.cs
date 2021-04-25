using ContainerShell.Applications.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoWaf.Trade.Interfaces.Applications;

namespace ContainerShell.Applications.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class ExceptionOrderPanelWindowCtrl : IController
    {
        private readonly ExceptionOrderPanelVM contentVM;
        private readonly IMessageWindowService messageWindowService;
        private readonly ISelectedOrdersOperateCommandsCtrl selectedOrdersOperateCommandsCtrl;
        private readonly ISelectedTradeLameTNOperateCommandsCtrl selectedTradeLameTNOperateCommandsCtrl;
        private readonly ExportFactory<IXqOrderDetailDialogCtrl> orderDetailDialogCtrlFactory;
        
        private readonly DelegateCommand toShowOrderExecuteDetailCmd;

        private IMessageWindow window;
        private bool windowIsShow = false;

        [ImportingConstructor]
        public ExceptionOrderPanelWindowCtrl(
            ExceptionOrderPanelVM contentVM,
            IMessageWindowService messageWindowService,
            ISelectedOrdersOperateCommandsCtrl selectedOrdersOperateCommandsCtrl,
            ISelectedTradeLameTNOperateCommandsCtrl selectedTradeLameTNOperateCommandsCtrl,
            ExportFactory<IXqOrderDetailDialogCtrl> orderDetailDialogCtrlFactory)
        {
            this.contentVM = contentVM;
            this.messageWindowService = messageWindowService;
            this.selectedOrdersOperateCommandsCtrl = selectedOrdersOperateCommandsCtrl;
            this.selectedTradeLameTNOperateCommandsCtrl = selectedTradeLameTNOperateCommandsCtrl;
            this.orderDetailDialogCtrlFactory = orderDetailDialogCtrlFactory;

            toShowOrderExecuteDetailCmd = new DelegateCommand(ToShowOrderExecuteDetail);
        }

        /// <summary>
        /// 窗口 owner 设置
        /// </summary>
        public Window WindowOwner { get; set; }
        
        /// <summary>
        /// 已关闭的处理
        /// </summary>
        public Action ClosedHandler { get; set; }

        /// <summary>
        /// 窗口是否显示
        /// </summary>
        public bool WindowIsShow => this.windowIsShow;

        public void Initialize()
        {
            selectedOrdersOperateCommandsCtrl.WindowOwnerGetter = () => UIHelper.GetWindowOfUIElement(contentVM.View);
            selectedOrdersOperateCommandsCtrl.Initialize();

            contentVM.ToShowOrderExecuteDetailCmd = toShowOrderExecuteDetailCmd;
            contentVM.SelectedOrdersOptCommands = selectedOrdersOperateCommandsCtrl.SelectedOrdersOptCommands;
            contentVM.SelectedTradeLameTNOptCommands = selectedTradeLameTNOperateCommandsCtrl.SelectedTradeLameTNOptCommands;

            contentVM.CloseMenuButtonClickHandler = (s, e) =>
            {
                // 将关闭按钮点击处理成隐藏窗口
                Hide();
                e.Handled = true;
            };

            var winSize = new Size(SystemParameters.PrimaryScreenWidth * 0.8, SystemParameters.PrimaryScreenHeight * 0.8);
            this.window = messageWindowService.CreateCompleteCustomWindow(WindowOwner, null, winSize, false, true, contentVM.View, contentVM.WindowCaptionHeightHolder);
            this.window.Closed += Window_Closed;
        }
        
        public void Run()
        {

        }

        public void Shutdown()
        {
            selectedOrdersOperateCommandsCtrl.Shutdown();
            ClosedHandler = null;
            contentVM.CloseMenuButtonClickHandler = null;
            InternalClosePanelWindow();
        }

        public void Show()
        {
            if (window != null)
            {
                window.Show(false);
                window.Activate();
                windowIsShow = true;
            }
        }

        public void Hide()
        {
            window?.Hide();
            windowIsShow = false;
            ActivateOwnerWindow();
        }

        public void ActiveExceptionOrderTabInPanel(OrderItemDataModel moveToAndSelectItem)
        {
            contentVM.SelectExceptionOrderTab();
            if (moveToAndSelectItem != null)
            {
                contentVM.SelectExceptionOrderItemAndBringIntoView(moveToAndSelectItem);
            }
        }

        public void ActiveAmbiguousOrderTabInPanel(OrderItemDataModel moveToAndSelectItem)
        {
            contentVM.SelectAmbiguousOrderTab();
            if (moveToAndSelectItem != null)
            {
                contentVM.SelectAmbiguousOrderItemAndBringIntoView(moveToAndSelectItem);
            }
        }

        public void ActiveTradeLameTabInPanel(XQTradeLameTaskNote moveToAndSelectItem)
        {
            contentVM.SelectTradeLameTaskNoteTab();
            if (moveToAndSelectItem != null)
            {
                contentVM.SelectTradeLameTaskNoteAndBringIntoView(moveToAndSelectItem);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            ClosedHandler?.Invoke();
            windowIsShow = false;
            Shutdown();
            ActivateOwnerWindow();
        }

        private void ActivateOwnerWindow()
        {
            // This is a workaround. Without this line the main window might hide behind
            // another running application.
            var ownerWin = (WindowOwner as Window) ?? Application.Current.MainWindow;
            ownerWin?.Activate();
        }
        
        private void InternalClosePanelWindow()
        {
            try
            {
                if (window != null)
                {
                    window.Closed -= Window_Closed;
                    window?.Close();
                    windowIsShow = false;
                    ActivateOwnerWindow();
                }
            }
            catch (Exception e)
            { Console.WriteLine(e); }
        }
        
        private void ToShowOrderExecuteDetail(object obj)
        {
            if (obj is OrderItemDataModel _o)
                ToShowOrderExecuteDetail(_o.OrderId);
            else if (obj is TradeItemDataModel _t)
                ToShowOrderExecuteDetail(_t.OrderId);
        }

        private void ToShowOrderExecuteDetail(string orderId)
        {
            var dialogCtrl = orderDetailDialogCtrlFactory.CreateExport().Value;
            dialogCtrl.DialogOwner = UIHelper.GetWindowOfUIElement(contentVM.View);
            dialogCtrl.XqOrderId = orderId;

            dialogCtrl.Initialize();
            dialogCtrl.Run();
            dialogCtrl.Shutdown();
        }

    }
}
