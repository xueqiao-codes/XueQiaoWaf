using ContainerShell.Interfaces.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.BusinessResources.DataModels;

namespace ContainerShell.Interfaces.Applications
{
    public delegate void XqInitializeDataInitialized(InitializeDataRoot initializeData);

    public interface IContainerShellService
    {
        /// <summary>
        /// 显示容器窗口
        /// </summary>
        void ShowShellWindow();

        /// <summary>
        /// 容器窗口关闭中事件
        /// </summary>
        event CancelEventHandler ShellWindowClosing;

        /// <summary>
        /// 容器窗口关闭事件
        /// </summary>
        event EventHandler ShellWindowClosed;
        
        /// <summary>
        /// 初始化数据
        /// </summary>
        InitializeDataRoot InitializeData { get; }

        /// <summary>
        /// 雪橇初始化数据是否已初始化
        /// </summary>
        bool IsXqInitializeDataInitalized { get; }

        /// <summary>
        /// 雪橇初始化数据已初始化的事件
        /// </summary>
        event XqInitializeDataInitialized XqInitializeDataInitialized;

        /// <summary>
        /// 显示合约快速搜索弹层
        /// </summary>
        /// <param name="popupPlaceTarget">弹层目标</param>
        /// <param name="dataSourceCommodityIds">商品数据源。为null，则使用默认商品数据源</param>
        /// <param name="selectedContractHandler">选择某个合约的回调</param>
        void ShowContractQuickSearchPopup(object popupPlaceTarget, 
            IEnumerable<int> dataSourceCommodityIds,
            Action<int?> selectedContractHandler);

        /// <summary>
        /// 显示合约预览选择弹层
        /// </summary>
        /// <param name="popupPlaceTarget">弹层目标</param>
        /// <param name="dataSourceCommodityIds">商品数据源。为null，则使用默认商品数据源</param>
        /// <param name="selectedContractHandler">选择某个合约的回调</param>
        void ShowContractPreviewSelectPopup(object popupPlaceTarget,
            IEnumerable<int> dataSourceCommodityIds,
            Action<int?> selectedContractHandler);

        /// <summary>
        /// 显示异常订单面板窗口
        /// </summary>
        void ShowExceptionOrdersPanelWindow();
        
        /// <summary>
        /// 隐藏异常订单面板窗口
        /// </summary>
        void HideExceptionOrdersPanelWindow();

        /// <summary>
        /// 异常订单面板窗口是否显示
        /// </summary>
        bool ExceptionOrdersPanelWindowIsShow { get; }

        /// <summary>
        /// 显示异常订单面板上的异常订单 tab，并且选中某个订单
        /// </summary>
        /// <param name="moveToAndSelectItem">要选中的订单</param>
        void ActiveExceptionOrderTabInExceptionOrderPanel(OrderItemDataModel moveToAndSelectItem);

        /// <summary>
        /// 显示异常订单面板上的状态不明确订单 tab，并且选中某个订单
        /// </summary>
        /// <param name="moveToAndSelectItem">要选中的订单</param>
        void ActiveAmbiguousOrderTabInExceptionOrderPanel(OrderItemDataModel moveToAndSelectItem);
        
        /// <summary>
        /// 显示异常订单面板上的瘸腿成交 tab，并且选中某个瘸腿处理
        /// </summary>
        /// <param name="moveToAndSelectItem"></param>
        void ActiveTradeLameTabInExceptionOrderPanel(XQTradeLameTaskNote moveToAndSelectItem);

        /// <summary>
        /// 显示订单发生异常 Notification
        /// </summary>
        void ShowOrderOccurExceptionNotification(OrderItemDataModel order);

        /// <summary>
        /// 显示订单已触发 Notification
        /// </summary>
        void ShowOrderTriggeredNotification(OrderItemDataModel order);

        /// <summary>
        /// 显示订单已成交 Notification
        /// </summary>
        void ShowOrderTradedNotification(TradeItemDataModel trade);

        /// <summary>
        /// 显示订单状态异常 Notification
        /// </summary>
        void ShowOrderStateAmbiguousNotification(OrderItemDataModel order);

        /// <summary>
        /// 显示瘸腿成交任务项 Notification
        /// </summary>
        void ShowTradeLameTaskNoteNotification(XQTradeLameTaskNote lameTaskNote);
    }
}
