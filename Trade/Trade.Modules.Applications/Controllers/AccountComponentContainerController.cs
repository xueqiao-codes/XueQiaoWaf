using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using XueQiaoFoundation.BusinessResources.Constants;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.Shared.Model;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Helper;
using XueQiaoWaf.Trade.Modules.Applications.ViewModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers
{
    /// <summary>
    /// 账户组件控制器
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class AccountComponentContainerController : ITradeComponentController
    {
        private readonly AccountComponentHeaderViewModel componentHeaderViewModel;
        private readonly BlankContentViewModel componentContentContainerModel;
        private readonly Lazy<ComponentTabListSelectionViewModel> tabListSelectionViewModel;
        private readonly ComponentHeaderLayoutCtrl headerLayoutCtrl;
        private readonly ExportFactory<OrderListEntrustedPageController> orderListEntrustedPageCtrlFactory;
        private readonly ExportFactory<OrderListParkedPageController> orderListParkedPageCtrlFactory;
        private readonly ExportFactory<OrderListConditionPageController> orderListConditionPageCtrlFactory;
        private readonly ExportFactory<TradeListPageController> tradeListPageControllerFactory;
        private readonly ExportFactory<PositionDiscreteListPageController> positionPageCtrlFactory;
        private readonly ExportFactory<PositionAssistantComponentController> positionAssistantCtrlFactory;
        private readonly ExportFactory<OrderHistoryComponentController> orderHistoryCtrlFactory;
        private readonly ExportFactory<TradeHistoryComponentController> tradeHistoryCtrlFactory;
        private readonly ExportFactory<PositionAssignHistoryComponentCtrl> positionAssignHistoryCtrlFactory;
        private readonly ExportFactory<FundPageController> fundPageCtrlFactory;
        private readonly Dictionary<int, IController> tabPageControllers = new Dictionary<int, IController>();

        private DelegateCommand componentTypeCheckCommand;
        private DelegateCommand componentTypeUncheckCommand;

        [ImportingConstructor]
        public AccountComponentContainerController(AccountComponentHeaderViewModel componentHeaderViewModel,
            BlankContentViewModel componentContentContainerModel,
            Lazy<ComponentTabListSelectionViewModel> tabListSelectionViewModel,
            ComponentHeaderLayoutCtrl headerLayoutCtrl,
            ExportFactory<OrderListEntrustedPageController> orderListEntrustedPageCtrlFactory,
            ExportFactory<OrderListParkedPageController> orderListParkedPageCtrlFactory,
            ExportFactory<OrderListConditionPageController> orderListConditionPageCtrlFactory,
            ExportFactory<TradeListPageController> tradeListPageControllerFactory,
            ExportFactory<PositionDiscreteListPageController> positionPageCtrlFactory,
            ExportFactory<PositionAssistantComponentController> positionAssistantCtrlFactory,
            ExportFactory<OrderHistoryComponentController> orderHistoryCtrlFactory,
            ExportFactory<TradeHistoryComponentController> tradeHistoryCtrlFactory,
            ExportFactory<PositionAssignHistoryComponentCtrl> positionAssignHistoryCtrlFactory,
            ExportFactory<FundPageController> fundPageCtrlFactory)
        {
            this.componentHeaderViewModel = componentHeaderViewModel;
            this.componentContentContainerModel = componentContentContainerModel;
            this.tabListSelectionViewModel = tabListSelectionViewModel;
            this.headerLayoutCtrl = headerLayoutCtrl;
            this.orderListEntrustedPageCtrlFactory = orderListEntrustedPageCtrlFactory;
            this.orderListParkedPageCtrlFactory = orderListParkedPageCtrlFactory;
            this.orderListConditionPageCtrlFactory = orderListConditionPageCtrlFactory;
            this.tradeListPageControllerFactory = tradeListPageControllerFactory;
            this.positionPageCtrlFactory = positionPageCtrlFactory;
            this.positionAssistantCtrlFactory = positionAssistantCtrlFactory;
            this.orderHistoryCtrlFactory = orderHistoryCtrlFactory;
            this.tradeHistoryCtrlFactory = tradeHistoryCtrlFactory;
            this.positionAssignHistoryCtrlFactory = positionAssignHistoryCtrlFactory;
            this.fundPageCtrlFactory = fundPageCtrlFactory;

            componentTypeCheckCommand = new DelegateCommand(this.CheckComponentType);
            componentTypeUncheckCommand = new DelegateCommand(this.UncheckComponentType);
        }


        #region ITradeComponentController

        public XueQiaoFoundation.BusinessResources.DataModels.TradeComponent Component { get; set; }

        public XueQiaoFoundation.BusinessResources.DataModels.TabWorkspace ParentWorkspace { get; set; }

        public Action<ITradeComponentController> CloseComponentHandler { get; set; }

        public ITradeComponentXqTargetAssociateHandler XqTargetAssociateHandler { get; set; }

        /// <summary>
        /// 组件 data model，在 Initialize 后可获得
        /// </summary>
        public DraggableComponentUIDM ComponentItemDataModel { get; private set; }

        public bool OnAssociateXqTarget(TradeComponentXqTargetAssociateArgs associateArgs)
        {
            return false;
        }

        #endregion


        public void Initialize()
        {
            if (Component == null) throw new ArgumentNullException("`Component` can't be null before Initialize.");
            if (ParentWorkspace == null) throw new ArgumentNullException("`ParentWorkspace` must be setted before initialize.");

            var containerComponentDetail = Component.AccountContainerComponentDetail;
            if (containerComponentDetail == null) throw new ArgumentNullException("Component.AccountContainerComponentDetail");

            // config header layout controller 
            headerLayoutCtrl.Component = this.Component;
            headerLayoutCtrl.ClickComponentClose = _ctrl => CloseComponentHandler?.Invoke(this);
            headerLayoutCtrl.ClickComponentSetting = (c, p) => PopupPageTypeSelection(p);
            headerLayoutCtrl.Initialize();
            headerLayoutCtrl.Run();

            headerLayoutCtrl.HeaderLayoutVM.ShowCloseItem = true;
            headerLayoutCtrl.HeaderLayoutVM.ShowSettingItem = true;
            headerLayoutCtrl.HeaderLayoutVM.ShowComponentLockItem = false;
            headerLayoutCtrl.HeaderLayoutVM.CustomPartView = componentHeaderViewModel.View;

            componentHeaderViewModel.TogatherTabbedComponentTypes.AddRange(containerComponentDetail.TogatherTabedComponentTypes);
            componentHeaderViewModel.SelectedTabComponentType = Component.ComponentType;

            componentContentContainerModel.Content = null;

            ComponentItemDataModel = new DraggableComponentUIDM(Component,
                headerLayoutCtrl.HeaderLayoutVM.View,
                componentContentContainerModel.View);
            ComponentItemDataModel.ComponentMinWidth = TradeComponentConstans.ComponentMininumWidthDefault;
            ComponentItemDataModel.ComponentMinHeight = TradeComponentConstans.ComponentMininumHeightDefault;

            PropertyChangedEventManager.AddHandler(componentHeaderViewModel, ComponentHeadeVMPropertyChanged, "");
        }

        public void Run()
        {
            OnSelectedTabComponentTypeChanged();
        }

        public void Shutdown()
        {
            PropertyChangedEventManager.RemoveHandler(componentHeaderViewModel, ComponentHeadeVMPropertyChanged, "");

            CloseComponentHandler = null;
            headerLayoutCtrl?.Shutdown();

            foreach (var pageCtrl in tabPageControllers)
            {
                pageCtrl.Value.Shutdown();
            }
            tabPageControllers.Clear();
        }

        private void OnSelectedTabComponentTypeChanged()
        {
            var selTabComponentType = componentHeaderViewModel.SelectedTabComponentType;

            Component.ComponentType = selTabComponentType;
            Component.ComponentDescTitle = TradeWorkspaceDataDisplayHelper.GetTradeComponentTypeDisplayName(selTabComponentType);

            if (selTabComponentType == XueQiaoConstants.TradeCompType_ENTRUSTED_ORDER_LIST)
            {
                ShowEntrustedOrderPageInComponentContent();
            }
            else if (selTabComponentType == XueQiaoConstants.TradeCompType_PARKED_ORDER_LIST)
            {
                ShowParkedOrderPageInComponentContent();
            }
            else if (selTabComponentType == XueQiaoConstants.TradeCompType_CONDITION_ORDER_LIST)
            {
                ShowConditionOrderPageInComponentContent();
            }
            else if (selTabComponentType == XueQiaoConstants.TradeCompType_TRADE_LIST)
            {
                ShowTradeListPageInComponentContent();
            }
            else if (selTabComponentType == XueQiaoConstants.TradeCompType_POSITION_LIST)
            {
                ShowPositionListPageInComponentContent();
            }
            else if (selTabComponentType == XueQiaoConstants.TradeCompType_POSITION_ASSISTANT)
            {
                ShowPositionAssistantPageInComponentContent();
            }
            else if (selTabComponentType == XueQiaoConstants.TradeCompType_ORDER_HISTORY)
            {
                ShowOrderHistoryPageInComponentContent();
            }
            else if (selTabComponentType == XueQiaoConstants.TradeCompType_TRADE_HISTORY)
            {
                ShowTradeHistoryPageInComponentContent();
            }
            else if (selTabComponentType == XueQiaoConstants.TradeCompType_POSITION_ASSIGN_HISTORY)
            {
                ShowPositionAssignHistoryPageInComponentContent();
            }
            else if (selTabComponentType == XueQiaoConstants.TradeCompType_FUND)
            {
                ShowFundPageInComponentContent();
            }
        }

        private void ComponentHeadeVMPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AccountComponentHeaderViewModel.SelectedTabComponentType))
            {
                if (ComponentItemDataModel != null)
                {
                    OnSelectedTabComponentTypeChanged();
                }
            }
        }

        private void CheckComponentType(object param)
        {
            if (param is int componentType)
            {
                var tabbedComponentCollection = componentHeaderViewModel.TogatherTabbedComponentTypes;
                if (!tabbedComponentCollection.Contains(componentType))
                {
                    tabbedComponentCollection.Add(componentType);
                    Component.AccountContainerComponentDetail.TogatherTabedComponentTypes
                        = tabbedComponentCollection.ToArray();
                }
            }
        }

        private void UncheckComponentType(object param)
        {
            if (param is int componentType)
            {
                var tabbedComponentCollection = componentHeaderViewModel.TogatherTabbedComponentTypes;
                if (tabbedComponentCollection.Contains(componentType))
                {
                    tabbedComponentCollection.Remove(componentType);
                    Component.AccountContainerComponentDetail.TogatherTabedComponentTypes
                        = tabbedComponentCollection.ToArray();
                }
            }
        }

        private void PopupPageTypeSelection(object param)
        {
            if (!tabListSelectionViewModel.IsValueCreated)
            {
                var tabsViewModel = tabListSelectionViewModel.Value;
                tabsViewModel.CheckCommand = componentTypeCheckCommand;
                tabsViewModel.UncheckCommand = componentTypeUncheckCommand;
                var componentTabs = TradeWorkspaceDataDisplayHelper.ComponentsInAccountContainer.Select(i => new ComponentTabSelectionItem(i)).ToArray();
                var togatherTabbedCompTypes = Component.AccountContainerComponentDetail.TogatherTabedComponentTypes;
                foreach (var tab in componentTabs)
                {
                    tab.IsChecked = togatherTabbedCompTypes?.Contains(tab.ComponentType)??false;
                }
                tabsViewModel.ComponentTypes.Clear();
                tabsViewModel.ComponentTypes.AddRange(componentTabs);
            }
            tabListSelectionViewModel.Value.ShowPopup(param);
        }

        private void ShowEntrustedOrderPageInComponentContent()
        {
            var componentType = XueQiaoConstants.TradeCompType_ENTRUSTED_ORDER_LIST;

            tabPageControllers.TryGetValue(componentType, out IController pageController);
            var orderPageCtrl = pageController as OrderListEntrustedPageController;
            if (orderPageCtrl == null)
            {
                if (Component.AccountContainerComponentDetail.EntrustedOrderListComponentDetail == null)
                {
                    Component.AccountContainerComponentDetail.EntrustedOrderListComponentDetail = new XueQiaoFoundation.BusinessResources.Models.EntrustedOrderListComponentDetail();
                }

                orderPageCtrl = orderListEntrustedPageCtrlFactory.CreateExport().Value;
                orderPageCtrl.ParentWorkspace = this.ParentWorkspace;
                orderPageCtrl.Component = this.Component;
                orderPageCtrl.ParentComponentCtrl = this;

                orderPageCtrl.Initialize();
                orderPageCtrl.Run();

                // 添加进集合
                tabPageControllers[componentType] = orderPageCtrl;
            }

            // change the content display
            componentContentContainerModel.Content = orderPageCtrl.ComponentContentView;
        }

        private void ShowParkedOrderPageInComponentContent()
        {
            var componentType = XueQiaoConstants.TradeCompType_PARKED_ORDER_LIST;

            tabPageControllers.TryGetValue(componentType, out IController pageController);
            var orderPageCtrl = pageController as OrderListParkedPageController;
            if (orderPageCtrl == null)
            {
                if (Component.AccountContainerComponentDetail.ParkedOrderListComponentDetail == null)
                {
                    Component.AccountContainerComponentDetail.ParkedOrderListComponentDetail = new XueQiaoFoundation.BusinessResources.Models.ParkedOrderListComponentDetail();
                }

                orderPageCtrl = orderListParkedPageCtrlFactory.CreateExport().Value;
                orderPageCtrl.ParentWorkspace = this.ParentWorkspace;
                orderPageCtrl.Component = this.Component;
                orderPageCtrl.ParentComponentCtrl = this;

                orderPageCtrl.Initialize();
                orderPageCtrl.Run();

                // 添加进集合
                tabPageControllers[componentType] = orderPageCtrl;
            }

            // change the content display
            componentContentContainerModel.Content = orderPageCtrl.ComponentContentView;
        }

        private void ShowConditionOrderPageInComponentContent()
        {
            var componentType = XueQiaoConstants.TradeCompType_CONDITION_ORDER_LIST;

            tabPageControllers.TryGetValue(componentType, out IController pageController);
            var orderPageCtrl = pageController as OrderListConditionPageController;
            if (orderPageCtrl == null)
            {
                if (Component.AccountContainerComponentDetail.ConditionOrderListComponentDetail == null)
                {
                    Component.AccountContainerComponentDetail.ConditionOrderListComponentDetail = new XueQiaoFoundation.BusinessResources.Models.ConditionOrderListComponentDetail();
                }

                orderPageCtrl = orderListConditionPageCtrlFactory.CreateExport().Value;
                orderPageCtrl.ParentWorkspace = this.ParentWorkspace;
                orderPageCtrl.Component = this.Component;
                orderPageCtrl.ParentComponentCtrl = this;

                orderPageCtrl.Initialize();
                orderPageCtrl.Run();

                // 添加进集合
                tabPageControllers[componentType] = orderPageCtrl;
            }

            // change the content display
            componentContentContainerModel.Content = orderPageCtrl.ComponentContentView;
        }

        private void ShowTradeListPageInComponentContent()
        {
            var componentType = XueQiaoConstants.TradeCompType_TRADE_LIST;

            tabPageControllers.TryGetValue(componentType, out IController pageController);
            var tradePageCtrl = pageController as TradeListPageController;
            if (pageController == null)
            {
                if (Component.AccountContainerComponentDetail.TradeListComponentDetail == null)
                {
                    Component.AccountContainerComponentDetail.TradeListComponentDetail = new XueQiaoFoundation.BusinessResources.Models.TradeListComponentDetail();
                }

                tradePageCtrl = tradeListPageControllerFactory.CreateExport().Value;
                tradePageCtrl.ParentWorkspace = this.ParentWorkspace;
                tradePageCtrl.Component = this.Component;
                tradePageCtrl.ParentComponentCtrl = this;

                tradePageCtrl.Initialize();
                tradePageCtrl.Run();

                // 添加进集合
                tabPageControllers[componentType] = tradePageCtrl;
            }

            // change the content display
            componentContentContainerModel.Content = tradePageCtrl.ComponentContentView;
        }

        private void ShowPositionListPageInComponentContent()
        {
            var componentType = XueQiaoConstants.TradeCompType_POSITION_LIST;

            tabPageControllers.TryGetValue(componentType, out IController pageController);
            var positionPageCtrl = pageController as PositionDiscreteListPageController;
            if (positionPageCtrl == null)
            {
                if (Component.AccountContainerComponentDetail.PositionListComponentDetail == null)
                {
                    Component.AccountContainerComponentDetail.PositionListComponentDetail 
                        = new XueQiaoFoundation.BusinessResources.Models.PositionListComponentDetail();
                }

                positionPageCtrl = positionPageCtrlFactory.CreateExport().Value;
                positionPageCtrl.ParentWorkspace = this.ParentWorkspace;
                positionPageCtrl.ParentComponentCtrl = this;

                positionPageCtrl.Initialize();
                positionPageCtrl.Run();

                // 添加进集合
                tabPageControllers[componentType] = positionPageCtrl;
            }

            // change the content display
            componentContentContainerModel.Content = positionPageCtrl.ComponentContentView;
        }

        private void ShowPositionAssistantPageInComponentContent()
        {
            var componentType = XueQiaoConstants.TradeCompType_POSITION_ASSISTANT;

            tabPageControllers.TryGetValue(componentType, out IController pageController);
            var positionPageCtrl = pageController as PositionAssistantComponentController;
            if (positionPageCtrl == null)
            {
                if (Component.AccountContainerComponentDetail.PositionAssistantComponentDetail == null)
                {
                    Component.AccountContainerComponentDetail.PositionAssistantComponentDetail 
                        = new XueQiaoFoundation.BusinessResources.Models.PositionAssistantComponentDetail();
                }

                positionPageCtrl = positionAssistantCtrlFactory.CreateExport().Value;
                positionPageCtrl.ParentWorkspace = this.ParentWorkspace;
                positionPageCtrl.ParentComponentCtrl = this;

                positionPageCtrl.Initialize();
                positionPageCtrl.Run();

                // 添加进集合
                tabPageControllers[componentType] = positionPageCtrl;
            }

            // change the content display
            componentContentContainerModel.Content = positionPageCtrl.ComponentContentView;
        }

        private void ShowFundPageInComponentContent()
        {
            var componentType = XueQiaoConstants.TradeCompType_FUND;

            tabPageControllers.TryGetValue(componentType, out IController pageController);
            var fundPageCtrl = pageController as FundPageController;
            if (fundPageCtrl == null)
            {
                if (Component.AccountContainerComponentDetail.FundListComponentDetail == null)
                {
                    Component.AccountContainerComponentDetail.FundListComponentDetail = new XueQiaoFoundation.BusinessResources.Models.FundListComponentDetail();
                }

                fundPageCtrl = fundPageCtrlFactory.CreateExport().Value;
                fundPageCtrl.ParentWorkspace = this.ParentWorkspace;

                fundPageCtrl.Initialize();
                fundPageCtrl.Run();

                // 添加进集合
                tabPageControllers[componentType] = fundPageCtrl;
            }

            // change the content display
            componentContentContainerModel.Content = fundPageCtrl.ComponentContentView;
        }

        private void ShowOrderHistoryPageInComponentContent()
        {
            var componentType = XueQiaoConstants.TradeCompType_ORDER_HISTORY;

            tabPageControllers.TryGetValue(componentType, out IController pageController);
            var historyPageCtrl = pageController as OrderHistoryComponentController;
            if (historyPageCtrl == null)
            {
                if (Component.AccountContainerComponentDetail.OrderHistoryComponentDetail == null)
                {
                    Component.AccountContainerComponentDetail.OrderHistoryComponentDetail = new XueQiaoFoundation.BusinessResources.Models.OrderHistoryComponentDetail();
                }

                historyPageCtrl = orderHistoryCtrlFactory.CreateExport().Value;
                historyPageCtrl.ParentWorkspace = this.ParentWorkspace;
                historyPageCtrl.ParentComponentCtrl = this;

                historyPageCtrl.Initialize();
                historyPageCtrl.Run();

                // 添加进集合
                tabPageControllers[componentType] = historyPageCtrl;
            }

            // change the content display
            componentContentContainerModel.Content = historyPageCtrl.ComponentContentView;
        }

        private void ShowTradeHistoryPageInComponentContent()
        {
            var componentType = XueQiaoConstants.TradeCompType_TRADE_HISTORY;

            tabPageControllers.TryGetValue(componentType, out IController pageController);
            var historyPageCtrl = pageController as TradeHistoryComponentController;
            if (historyPageCtrl == null)
            {
                if (Component.AccountContainerComponentDetail.TradeHistoryComponentDetail == null)
                {
                    Component.AccountContainerComponentDetail.TradeHistoryComponentDetail = new XueQiaoFoundation.BusinessResources.Models.TradeHistoryComponentDetail();
                }

                historyPageCtrl = tradeHistoryCtrlFactory.CreateExport().Value;
                historyPageCtrl.ParentWorkspace = this.ParentWorkspace;
                historyPageCtrl.ParentComponentCtrl = this;

                historyPageCtrl.Initialize();
                historyPageCtrl.Run();

                // 添加进集合
                tabPageControllers[componentType] = historyPageCtrl;
            }

            // change the content display
            componentContentContainerModel.Content = historyPageCtrl.ComponentContentView;
        }

        private void ShowPositionAssignHistoryPageInComponentContent()
        {
            var componentType = XueQiaoConstants.TradeCompType_POSITION_ASSIGN_HISTORY;

            tabPageControllers.TryGetValue(componentType, out IController pageController);
            var historyPageCtrl = pageController as PositionAssignHistoryComponentCtrl;
            if (historyPageCtrl == null)
            {
                if (Component.AccountContainerComponentDetail.TradeHistoryComponentDetail == null)
                {
                    Component.AccountContainerComponentDetail.PositionAssignHistoryComponentDetail = new XueQiaoFoundation.BusinessResources.Models.PositionAssignHistoryComponentDetail();
                }

                historyPageCtrl = positionAssignHistoryCtrlFactory.CreateExport().Value;
                historyPageCtrl.ParentWorkspace = this.ParentWorkspace;
                historyPageCtrl.ParentComponentCtrl = this;

                historyPageCtrl.Initialize();
                historyPageCtrl.Run();

                // 添加进集合
                tabPageControllers[componentType] = historyPageCtrl;
            }

            // change the content display
            componentContentContainerModel.Content = historyPageCtrl.ComponentContentView;
        }
    }
}
