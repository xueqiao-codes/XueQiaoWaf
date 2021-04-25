using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Waf.Applications;
using XueQiaoWaf.Trade.Modules.Applications.ViewModels;
using XueQiaoWaf.Trade.Modules.Applications.Helper;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.ControllerBase;
using System.ComponentModel;
using Prism.Events;
using XueQiaoWaf.Trade.Modules.Applications.ServiceControllers;
using Newtonsoft.Json;
using XueQiaoWaf.Trade.Modules.Applications.ServiceControllers.Events;
using xueqiao.trade.hosting;
using XueQiaoWaf.Trade.Interfaces.Applications;
using NativeModel.Trade;
using System.Windows;
using XueQiaoFoundation.BusinessResources.DataModels;
using ContainerShell.Interfaces.Applications;
using XueQiaoFoundation.BusinessResources.Constants;
using System.Collections.ObjectModel;
using XueQiaoFoundation.Shared.Model;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers
{
    /// <summary>
    /// 交易工作空间页管理
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class TradeWorkspaceItemController : SwitchablePageControllerBase, IWorkspaceItemViewCtrl, ITradeComponentXqTargetAssociateHandler
    {
        private readonly TradeWorkspaceViewModel contentVM;
        private readonly IDraggableComponentPanelContextCtrl componentPanelContextCtrl;
        private readonly ExportFactory<SubscribeDataComponentContainerController> subscribeDataComponentCtrlFactory;
        private readonly ExportFactory<PlaceOrderComponentController> palceOrderComponentControllerFactory;
        private readonly ExportFactory<AccountComponentContainerController> accountComponentControllerFactory;
        private readonly ExportFactory<TradeWorkspaceTemplateEditDialogController> workspaceTemplateEditDialogCtrlFactory;
        private readonly ExportFactory<TradeWorkspaceTemplateManageDialogController> workspaceTemplateManDialogCtrlFactory;
        private readonly ITradeModuleService tradeModuleService;
        private readonly IMessageWindowService messageWindowService;
        private readonly IEventAggregator eventAggregator;
        private readonly IOrderItemsController orderItemsController;
        private readonly ITradeItemsController tradeItemsController;
        private readonly IRelatedSubAccountItemsController relatedSubAccountItemsCtrl;
        private readonly ISubscribeContractController subscribeContractCtrl;

        private readonly Dictionary<DraggableComponentUIDM, ITradeComponentController> 
            componentControllers = new Dictionary<DraggableComponentUIDM, ITradeComponentController>();

        private readonly DelegateCommand addComponentCommand;
        private readonly DelegateCommand addChartComponentCmd;
        private readonly DelegateCommand addPlaceOrderComponentCmd;
        private readonly DelegateCommand addChartAndOrderComponentCmd;
        private readonly DelegateCommand closeComponentCommand;
        private readonly DelegateCommand saveCurrentWorkspaceCommand;
        private readonly DelegateCommand workspaceTemplateManageCommand;
        private readonly DelegateCommand toApplyDefaultTemplateCmd;
        private readonly DelegateCommand openWorkspaceTemplateCommand;
        private readonly DelegateCommand pickComponentCommand;

        #region ITradeComponentXqTargetAssociateHandler
        
        void ITradeComponentXqTargetAssociateHandler.HandleXqTargetAssociate(
            object handleDialogOwner,
            Point? handleDialogLocation,
            TradeComponentXqTargetAssociateArgs associateArgs)
        {
            if (PreviewHandleXqTargetAssociate(handleDialogOwner, handleDialogLocation, associateArgs))
            {
                var compCtrls = componentControllers.Values.ToArray();
                int associatedPlaceOrderCompNum = 0;
                foreach (var _tmp in compCtrls)
                {
                    var handled = _tmp.OnAssociateXqTarget(associateArgs);
                    if (handled && TradeWorkspaceDataDisplayHelper.IsBelong2PlaceOrderContainerComponent(_tmp.Component.ComponentType))
                    {
                        associatedPlaceOrderCompNum++;
                    }
                }

                // no place order component has associated, open a new place order component
                if (associatedPlaceOrderCompNum == 0)
                {
                    // 默认不显示图表 layout
                    ParseAssociateArgsAndJudgeShowLayouts(associateArgs, out bool showChart, out bool showPlaceOrder);
                    NewPlaceOrderComponentWithTarget(associateArgs.TargetType, associateArgs.TargetKey, showChart, showPlaceOrder);
                }
            }
        }

        void ITradeComponentXqTargetAssociateHandler.NewXqTargetPlaceOrderComponent(
            object handleDialogOwner,
            Point? handleDialogLocation,
            TradeComponentXqTargetAssociateArgs associateArgs)
        {
            if (PreviewHandleXqTargetAssociate(handleDialogOwner, handleDialogLocation, associateArgs))
            {
                // 默认不显示图表 layout
                ParseAssociateArgsAndJudgeShowLayouts(associateArgs, out bool showChart, out bool showPlaceOrder);
                NewPlaceOrderComponentWithTarget(associateArgs.TargetType, associateArgs.TargetKey, showChart, showPlaceOrder);
            }
        }

        /// <summary>
        /// 预联动
        /// </summary>
        /// <returns>是否通过预联动</returns>
        private bool PreviewHandleXqTargetAssociate(
            object handleDialogOwner,
            Point? handleDialogLocation,
            TradeComponentXqTargetAssociateArgs associateArgs)
        {
            if (associateArgs == null) return false;
            if (associateArgs.SourceWorkspace != this.Workspace) return false;

            return true;
        }
        
        private static void ParseAssociateArgsAndJudgeShowLayouts(TradeComponentXqTargetAssociateArgs associateArgs,
            out bool __showChart, out bool __showPlaceOrder)
        {
            __showChart = true;
            __showPlaceOrder = true;

            bool? arg_showChartLayout = null;
            bool? arg_showPlaceOrderLayout = null;
            object custom_arg1 = null;
            object custom_arg2 = null;
            if (associateArgs.CustomInfos?.TryGetValue(TradeComponentAssociateConstants.ComponentAssociateArg_IsShowChartLayout, out custom_arg1) == true)
                arg_showChartLayout = custom_arg1 as bool?;
            if (associateArgs.CustomInfos?.TryGetValue(TradeComponentAssociateConstants.ComponentAssociateArg_IsShowPlaceOrderLayout, out custom_arg2) == true)
                arg_showPlaceOrderLayout = custom_arg2 as bool?;

            var showChart = false;
            var showPlaceOrder = false;
            if (arg_showChartLayout != null)
            {
                showChart = arg_showChartLayout.Value;
            }
            if (arg_showPlaceOrderLayout != null)
            {
                showPlaceOrder = arg_showPlaceOrderLayout.Value;
            }
            if (showChart == false && showPlaceOrder == false)
                showPlaceOrder = true;

            __showChart = showChart;
            __showPlaceOrder = showPlaceOrder;
        }

        #endregion

        private void NewPlaceOrderComponentWithTarget(ClientXQOrderTargetType? targetType, string targetKey, 
            bool showChartLayout, bool showPlaceOrderLayout)
        {
            var newComp = CreatePlaceOrderComponent(showChartLayout, showPlaceOrderLayout);

            if (targetType == ClientXQOrderTargetType.CONTRACT_TARGET)
                newComp.PlaceOrderContainerComponentDetail.AttachContractId = targetKey;
            else if (targetType == ClientXQOrderTargetType.COMPOSE_TARGET)
                newComp.PlaceOrderContainerComponentDetail.AttachComposeId = targetKey;
            else
                TradeWorkspaceDataDisplayHelper.ConfigureComponentDescTitleDefaultIfNeed(newComp);

            Workspace.TradeComponents.Add(newComp);
            AddViewForPlaceOrderContainerComponent(newComp, out DraggableComponentUIDM addedItem);

            componentPanelContextCtrl.PositionComponent(addedItem, null);
            componentPanelContextCtrl.FocusOnComponent(addedItem);
        }

        [ImportingConstructor]
        public TradeWorkspaceItemController(TradeWorkspaceViewModel contentVM,
            IDraggableComponentPanelContextCtrl componentPanelContextCtrl,
            ExportFactory<SubscribeDataComponentContainerController> subscribeDataComponentCtrlFactory,
            ExportFactory<PlaceOrderComponentController> palceOrderComponentControllerFactory,
            ExportFactory<AccountComponentContainerController> accountComponentControllerFactory,
            ExportFactory<TradeWorkspaceTemplateEditDialogController> workspaceTemplateEditDialogCtrlFactory,
            ExportFactory<TradeWorkspaceTemplateManageDialogController> workspaceTemplateManDialogCtrlFactory,
            ITradeModuleService tradeModuleService,
            IMessageWindowService messageWindowService,
            IEventAggregator eventAggregator,
            IOrderItemsController orderItemsController,
            ITradeItemsController tradeItemsController,
            IRelatedSubAccountItemsController relatedSubAccountItemsCtrl,
            ISubscribeContractController subscribeContractCtrl) 
            : base()
        {
            this.contentVM = contentVM;
            this.componentPanelContextCtrl = componentPanelContextCtrl;
            this.subscribeDataComponentCtrlFactory = subscribeDataComponentCtrlFactory;
            this.palceOrderComponentControllerFactory = palceOrderComponentControllerFactory;
            this.accountComponentControllerFactory = accountComponentControllerFactory;
            this.workspaceTemplateEditDialogCtrlFactory = workspaceTemplateEditDialogCtrlFactory;
            this.workspaceTemplateManDialogCtrlFactory = workspaceTemplateManDialogCtrlFactory;
            this.tradeModuleService = tradeModuleService;
            this.messageWindowService = messageWindowService;
            this.eventAggregator = eventAggregator;
            this.orderItemsController = orderItemsController;
            this.tradeItemsController = tradeItemsController;
            this.relatedSubAccountItemsCtrl = relatedSubAccountItemsCtrl;
            this.subscribeContractCtrl = subscribeContractCtrl;

            addComponentCommand = new DelegateCommand(AddComponentCmdAction);
            addChartComponentCmd = new DelegateCommand(AddChartComponentCmdAction);
            addPlaceOrderComponentCmd = new DelegateCommand(AddPlaceOrderComponentCmdAction);
            addChartAndOrderComponentCmd = new DelegateCommand(AddChartAndOrderComponentCmdAction);
            closeComponentCommand = new DelegateCommand(CloseComponent);
            saveCurrentWorkspaceCommand = new DelegateCommand(SaveCurrentWorkspace);
            workspaceTemplateManageCommand = new DelegateCommand(OpenTemplateManagePage);
            toApplyDefaultTemplateCmd = new DelegateCommand(ToApplyDefaultTemplate);
            openWorkspaceTemplateCommand = new DelegateCommand(OpenWorkspaceTemplate);
            pickComponentCommand = new DelegateCommand(PickOneComponent);
        }

        public TabWorkspace Workspace { get; set; }

        public bool IsInitialFromDefaultTemplate { get; set; }

        public object ContentView => contentVM.View;
        
        protected override void DoInitialize()
        {
            if (Workspace == null) throw new ArgumentNullException("Property `Workspace` must be setted value before Initialize.");
            
            // clear unavailable components
            Workspace.TradeComponents.RemoveAll(i => !TradeWorkspaceDataDisplayHelper.IsBelong2SubscribeDataContainerComponent(i.ComponentType)
                && !TradeWorkspaceDataDisplayHelper.IsBelong2PlaceOrderContainerComponent(i.ComponentType)
                && !TradeWorkspaceDataDisplayHelper.IsBelong2AccountContainerComponent(i.ComponentType));

            componentPanelContextCtrl.ComponentPanelContext = contentVM.DraggableComponentPanelContext;
            componentPanelContextCtrl.Initialize();

            contentVM.AddComponentCommand = addComponentCommand;
            contentVM.AddChartComponentCmd = addChartComponentCmd;
            contentVM.AddPlaceOrderComponentCmd = addPlaceOrderComponentCmd;
            contentVM.AddChartAndOrderComponentCmd = addChartAndOrderComponentCmd;
            contentVM.CloseComponentCommand = closeComponentCommand;
            contentVM.SaveCurrentWorkspaceCommand = saveCurrentWorkspaceCommand;
            contentVM.WorkspaceTemplateManageCommand = workspaceTemplateManageCommand;
            contentVM.ToApplyDefaultTemplateCmd = toApplyDefaultTemplateCmd;
            contentVM.OpenWorkspaceTemplateCommand = openWorkspaceTemplateCommand;
            contentVM.PickComponentCommand = pickComponentCommand;

            // 模板列表
            var templateCollection = tradeModuleService.TradeWorkspaceDataRoot?.TradeWorkspaceTemplateListContainer?.Templates 
                ?? new ObservableCollection<TradeTabWorkspaceTemplate>(); ;
            contentVM.TradeWorkspaceTemplates = new ReadOnlyObservableCollection<TradeTabWorkspaceTemplate>(templateCollection);
            PropertyChangedEventManager.AddHandler(contentVM, WorkspaceViewModelPropertyChanged, "");

            eventAggregator.GetEvent<UserRelatedSubAccountItemsRefreshEvent>().Subscribe(RecvUserRelatedSubAccountItemsRefreshEvent, ThreadOption.UIThread);
            if (contentVM.UserRelatedSubAccountItems?.Any() != true)
            {
                UpdateUserRelatedSubAccountItems(relatedSubAccountItemsCtrl.RelatedSubAccountItems);
            }
        }
        
        protected override void DoRun()
        {
            if (IsInitialFromDefaultTemplate)
            {
                InvalidateWorkspaceWithDefaultTemplate();
            }
            else
            {
                InvalidateComponentsDisplay();
            }
        }

        protected override void DoShutdown()
        {
            componentPanelContextCtrl?.Shutdown();
            eventAggregator.GetEvent<UserRelatedSubAccountItemsRefreshEvent>().Unsubscribe(RecvUserRelatedSubAccountItemsRefreshEvent);
            ShutdownAllComponents();
            PropertyChangedEventManager.RemoveHandler(contentVM, WorkspaceViewModelPropertyChanged, "");
        }

        private void RecvUserRelatedSubAccountItemsRefreshEvent(UserRelatedSubAccountItemsRefreshEventArgs args)
        {
            UpdateUserRelatedSubAccountItems(args.RelatedSubAccountItems?.ToArray());
        }
        
        private void WorkspaceViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TradeWorkspaceViewModel.SelectedRelatedSubAccountItem))
            {
                var changedSubAccount = contentVM.SelectedRelatedSubAccountItem;
                DidSelectedSubAccount(changedSubAccount);
            }
        }

        private void DidSelectedSubAccount(HostingSubAccountRelatedItem selectedSubAccountItem)
        {
            var changedSubAccountId = selectedSubAccountItem?.SubAccountId ?? 0;
            Workspace.SubAccountId = changedSubAccountId;
        }

        private void UpdateUserRelatedSubAccountItems(IEnumerable<HostingSubAccountRelatedItem> newRelatedSubAccountItems)
        {
            var originSelectItem = contentVM.SelectedRelatedSubAccountItem;
            HostingSubAccountRelatedItem newSelectItem = null;

            if (originSelectItem != null)
                newSelectItem = newRelatedSubAccountItems?.FirstOrDefault(i => i.SubAccountId == originSelectItem.SubAccountId && i.SubUserId == originSelectItem.SubUserId);

            if (newSelectItem == null)
                newSelectItem = newRelatedSubAccountItems?.FirstOrDefault(i => i.SubAccountId == Workspace.SubAccountId);
            if (newSelectItem == null)
                newSelectItem = newRelatedSubAccountItems?.FirstOrDefault();

            PropertyChangedEventManager.RemoveHandler(contentVM, WorkspaceViewModelPropertyChanged, "");

            contentVM.UserRelatedSubAccountItems.Clear();
            contentVM.UserRelatedSubAccountItems.AddRange(newRelatedSubAccountItems);
            contentVM.SelectedRelatedSubAccountItem = newSelectItem;
            DidSelectedSubAccount(newSelectItem);

            PropertyChangedEventManager.AddHandler(contentVM, WorkspaceViewModelPropertyChanged, "");
        }

        private static XueQiaoFoundation.BusinessResources.Models.TradeTabWorkspaceTemplate CreateDefaultTradeWorkspaceTemplate()
        {
            return new XueQiaoFoundation.BusinessResources.Models.TradeTabWorkspaceTemplate
            {
                TemplateId = UUIDHelper.CreateUUIDString(true),
                TemplateName = "默认模板",
                TemplateDesc = "提供默认交易窗口的工作空间",
                ChildComponents = new XueQiaoFoundation.BusinessResources.Models.TradeComp[]
                {
                    new XueQiaoFoundation.BusinessResources.Models.TradeComp
                    {
                        ComponentType = XueQiaoConstants.TradeCompType_CONTRACT_LIST,
                        SubscribeDataContainerComponentDetail = new XueQiaoFoundation.BusinessResources.Models.SubscribeDataContainerComponentDetail
                        {
                            TogatherTabedComponentTypes = new int[]
                            {
                                XueQiaoConstants.TradeCompType_CONTRACT_LIST,
                                XueQiaoConstants.TradeCompType_COMPOSE_LIST
                            }
                        }
                    },
                    new XueQiaoFoundation.BusinessResources.Models.TradeComp
                    {
                        ComponentType = XueQiaoConstants.TradeCompType_PLACE_ORDER,
                        PlaceOrderContainerComponentDetail = new XueQiaoFoundation.BusinessResources.Models.PlaceOrderContainerComponentDetail
                        {
                            IsShowChartLayout = false,
                            IsShowOrderLayout = true
                        }
                    },
                    new XueQiaoFoundation.BusinessResources.Models.TradeComp
                    {
                        ComponentType = XueQiaoConstants.TradeCompType_ENTRUSTED_ORDER_LIST,
                        AccountContainerComponentDetail = new XueQiaoFoundation.BusinessResources.Models.AccountContainerComponentDetail
                        {
                            TogatherTabedComponentTypes = new int[]
                            {
                                XueQiaoConstants.TradeCompType_ENTRUSTED_ORDER_LIST,
                                XueQiaoConstants.TradeCompType_PARKED_ORDER_LIST,
                                XueQiaoConstants.TradeCompType_CONDITION_ORDER_LIST
                            }
                        }
                    },
                    new XueQiaoFoundation.BusinessResources.Models.TradeComp
                    {
                        ComponentType = XueQiaoConstants.TradeCompType_TRADE_LIST,
                        AccountContainerComponentDetail = new XueQiaoFoundation.BusinessResources.Models.AccountContainerComponentDetail
                        {
                            TogatherTabedComponentTypes = new int[]
                            {
                                XueQiaoConstants.TradeCompType_TRADE_LIST,
                                XueQiaoConstants.TradeCompType_POSITION_LIST,
                                XueQiaoConstants.TradeCompType_POSITION_ASSISTANT,
                                XueQiaoConstants.TradeCompType_ORDER_HISTORY,
                                XueQiaoConstants.TradeCompType_TRADE_HISTORY,
                                XueQiaoConstants.TradeCompType_POSITION_ASSIGN_HISTORY,
                                XueQiaoConstants.TradeCompType_FUND,
                            }
                        }
                    }
                }
            };
        }

        private void InvalidateWorkspaceWithDefaultTemplate()
        {
            var defaultTemplate = CreateDefaultTradeWorkspaceTemplate();

            // 加载初始化模板数据
            Workspace.Name = defaultTemplate.TemplateName;
            Workspace.WorkspaceDesc = defaultTemplate.TemplateDesc;

            var addTradeComps = new List<TradeComponent>();
            foreach (var templateComp in defaultTemplate.ChildComponents)
            {
                var tarComp = templateComp.ToTradeComponent_DM();
                if (TradeWorkspaceDataDisplayHelper.IsBelong2PlaceOrderContainerComponent(tarComp.ComponentType))
                {
                    tarComp.Width = Extensions.PlaceOrderComponent_NormalWidth(tarComp.PlaceOrderContainerComponentDetail.IsShowChartLayout, tarComp.PlaceOrderContainerComponentDetail.IsShowOrderLayout);
                    tarComp.Height = TradeComponentConstans.PlaceOrderComponent_MininumHeight;
                }
                else
                {
                    tarComp.Width = TradeComponentConstans.ComponentWidthDefault;
                    tarComp.Height = TradeComponentConstans.ComponentHeightDefault;
                }
                addTradeComps.Add(tarComp);
                TradeWorkspaceDataDisplayHelper.ConfigureComponentDescTitleDefaultIfNeed(tarComp);
            }

            Workspace.TradeComponents.Clear();
            Workspace.TradeComponents.AddRange(addTradeComps);
            InvalidateComponentsDisplay();


            // 重新排列组件
            double y_offset = 2;
            IEnumerable<TradeComponent> arrangeComps = Workspace.TradeComponents.ToArray();
            while (arrangeComps.Any())
            {
                var takeComps = arrangeComps.Take(2);
                if (!takeComps.Any())
                {
                    break;
                }
                else
                {
                    double x_offset = 2;
                    foreach (var comp in takeComps)
                    {
                        comp.Left = x_offset;
                        comp.Top = y_offset;
                        x_offset += (comp.Width + 2);
                    }
                }
                y_offset += takeComps.Max(i => i.Height) + 2;
                arrangeComps = arrangeComps.Except(takeComps);
            }
        }

        private void ShutdownAllComponents()
        {
            foreach (var ctrlItem in componentControllers)
            {
                componentPanelContextCtrl.RemoveComponent(ctrlItem.Key);
                ctrlItem.Value.Shutdown();
            }
            componentControllers.Clear();
        }

        private void InvalidateComponentsDisplay()
        {
            if (Workspace != null)
            {
                foreach (var comp in Workspace.TradeComponents)
                {
                    if (TradeWorkspaceDataDisplayHelper.IsBelong2SubscribeDataContainerComponent(comp.ComponentType))
                    {
                        AddViewForSubscribeDataContainerComponent(comp, out DraggableComponentUIDM ignore);
                    }
                    else if (TradeWorkspaceDataDisplayHelper.IsBelong2PlaceOrderContainerComponent(comp.ComponentType))
                    {
                        AddViewForPlaceOrderContainerComponent(comp, out DraggableComponentUIDM ignore);
                    }
                    else if (TradeWorkspaceDataDisplayHelper.IsBelong2AccountContainerComponent(comp.ComponentType))
                    {
                        AddViewForAccountContainerComponent(comp, out DraggableComponentUIDM ignore);
                    }
                }
            }
        }

        private void SaveCurrentWorkspace()
        {
            // 保存工作空间
            var editDialogCtrl = workspaceTemplateEditDialogCtrlFactory.CreateExport().Value;
            editDialogCtrl.DialogOwner = UIHelper.GetWindowOfUIElement(contentVM.View);
            editDialogCtrl.DialogTitle = "保存工作空间为模板";

            var initialTemplate = new TradeTabWorkspaceTemplate(UUIDHelper.CreateUUIDString(false))
            {
                TemplateName = Workspace.Name
            };

            // 归档组件列表
            var childComponents = Workspace.TradeComponents.Select(i => i.ToTradeComponent_M()).ToArray();
            var childComponentsArchive = JsonConvert.SerializeObject(childComponents);
            initialTemplate.ChildComponents = JsonConvert.DeserializeObject<XueQiaoFoundation.BusinessResources.Models.TradeComp[]>(childComponentsArchive);

            editDialogCtrl.InitialEditTemplate = initialTemplate;

            editDialogCtrl.Initialize();
            editDialogCtrl.Run();
            editDialogCtrl.Shutdown();

            if (editDialogCtrl.EditSuccessResult == true)
            {
                // update current workspace name
                Workspace.Name = initialTemplate.TemplateName;
            }
        }

        private void OpenTemplateManagePage()
        {
            // 弹出管理窗口
            var dialogCtrl = workspaceTemplateManDialogCtrlFactory.CreateExport().Value;
            dialogCtrl.DialogOwner = UIHelper.GetWindowOfUIElement(contentVM.View);

            dialogCtrl.Initialize();
            dialogCtrl.Run();
            dialogCtrl.Shutdown();
        }

        private void ToApplyDefaultTemplate()
        {
            // 应用默认模板
            var dialogResult = messageWindowService.ShowQuestionDialog(UIHelper.GetWindowOfUIElement(contentVM.View), null, null, "提示", "要在当前工作区应用默认模板吗？", false, "应用", "取消");
            if (dialogResult != true) return;

            // 清除原页面数据
            ShutdownAllComponents();

            InvalidateWorkspaceWithDefaultTemplate();
        }

        private void OpenWorkspaceTemplate(object param)
        {
            if (param is TradeTabWorkspaceTemplate template)
            {
                var dialogResult = messageWindowService.ShowQuestionDialog(UIHelper.GetWindowOfUIElement(contentVM.View), null, null, "提示", "要在当前工作区应用选中模板吗？", false, "应用", "取消");
                if (dialogResult != true) return;

                // 清除原页面数据
                ShutdownAllComponents();

                // 加载当前模板数据
                Workspace.Name = template.TemplateName;
                Workspace.WorkspaceDesc = template.TemplateDesc;
                Workspace.TradeComponents.Clear();
                foreach (var templateComp in template.ChildComponents)
                {
                    var tarComp = templateComp.ToTradeComponent_DM();
                    Workspace.TradeComponents.Add(tarComp);
                    TradeWorkspaceDataDisplayHelper.ConfigureComponentDescTitleDefaultIfNeed(tarComp);
                }
                InvalidateComponentsDisplay();
            }
        }
        
        private void PickOneComponent(object param)
        {
            if (param is DraggableComponentUIDM pickedCompDataModel)
            {
                componentPanelContextCtrl.FocusOnComponent(pickedCompDataModel);
            }
        }

        private void AddContractListComponent()
        {
            var newComp = new TradeComponent
            {
                ComponentType = XueQiaoConstants.TradeCompType_CONTRACT_LIST,
                Width = TradeComponentConstans.ComponentWidthDefault,
                Height = TradeComponentConstans.ComponentHeightDefault,
            };
            TradeWorkspaceDataDisplayHelper.ConfigureComponentDescTitleDefaultIfNeed(newComp);
            newComp.SubscribeDataContainerComponentDetail.TogatherTabedComponentTypes = new int[] { newComp.ComponentType };
            newComp.SubscribeDataContainerComponentDetail.ContractListComponentDetail = new XueQiaoFoundation.BusinessResources.Models.ContractListComponentDetail();
            
            Workspace.TradeComponents.Add(newComp);
            AddViewForSubscribeDataContainerComponent(newComp, out DraggableComponentUIDM addedItem);

            componentPanelContextCtrl.PositionComponent(addedItem, null);
            componentPanelContextCtrl.FocusOnComponent(addedItem);
        }

        private void AddComposeListComponent()
        {
            var newComp = new TradeComponent
            {
                ComponentType = XueQiaoConstants.TradeCompType_COMPOSE_LIST,
                Width = TradeComponentConstans.ComponentWidthDefault,
                Height = TradeComponentConstans.ComponentHeightDefault,
            };
            TradeWorkspaceDataDisplayHelper.ConfigureComponentDescTitleDefaultIfNeed(newComp);
            newComp.SubscribeDataContainerComponentDetail.TogatherTabedComponentTypes = new int[] { newComp.ComponentType };
            newComp.SubscribeDataContainerComponentDetail.ComposeListComponentDetail = new XueQiaoFoundation.BusinessResources.Models.ComposeListComponentDetail();

            Workspace.TradeComponents.Add(newComp);
            AddViewForSubscribeDataContainerComponent(newComp, out DraggableComponentUIDM addedItem);

            componentPanelContextCtrl.PositionComponent(addedItem, null);
            componentPanelContextCtrl.FocusOnComponent(addedItem);
        }
        
        private void AddEntrustedOrderListComponent()
        {
            var newComp = new TradeComponent
            {
                ComponentType = XueQiaoConstants.TradeCompType_ENTRUSTED_ORDER_LIST,
                Width = TradeComponentConstans.ComponentWidthDefault,
                Height = TradeComponentConstans.ComponentHeightDefault,
            };

            TradeWorkspaceDataDisplayHelper.ConfigureComponentDescTitleDefaultIfNeed(newComp);
            newComp.AccountContainerComponentDetail.TogatherTabedComponentTypes = new int[] { newComp.ComponentType };
            newComp.AccountContainerComponentDetail.EntrustedOrderListComponentDetail = new XueQiaoFoundation.BusinessResources.Models.EntrustedOrderListComponentDetail();

            Workspace.TradeComponents.Add(newComp);
            AddViewForAccountContainerComponent(newComp, out DraggableComponentUIDM addedItem);

            componentPanelContextCtrl.PositionComponent(addedItem, null);
            componentPanelContextCtrl.FocusOnComponent(addedItem);
        }

        private void AddParkedOrderListComponent()
        {
            var newComp = new TradeComponent
            {
                ComponentType = XueQiaoConstants.TradeCompType_PARKED_ORDER_LIST,
                Width = TradeComponentConstans.ComponentWidthDefault,
                Height = TradeComponentConstans.ComponentHeightDefault,
            };

            TradeWorkspaceDataDisplayHelper.ConfigureComponentDescTitleDefaultIfNeed(newComp);
            newComp.AccountContainerComponentDetail.TogatherTabedComponentTypes = new int[] { newComp.ComponentType };
            newComp.AccountContainerComponentDetail.ParkedOrderListComponentDetail = new XueQiaoFoundation.BusinessResources.Models.ParkedOrderListComponentDetail();

            Workspace.TradeComponents.Add(newComp);
            AddViewForAccountContainerComponent(newComp, out DraggableComponentUIDM addedItem);

            componentPanelContextCtrl.PositionComponent(addedItem, null);
            componentPanelContextCtrl.FocusOnComponent(addedItem);
        }

        private void AddConditionOrderListComponent()
        {
            var newComp = new TradeComponent
            {
                ComponentType = XueQiaoConstants.TradeCompType_CONDITION_ORDER_LIST,
                Width = TradeComponentConstans.ComponentWidthDefault,
                Height = TradeComponentConstans.ComponentHeightDefault,
            };

            TradeWorkspaceDataDisplayHelper.ConfigureComponentDescTitleDefaultIfNeed(newComp);
            newComp.AccountContainerComponentDetail.TogatherTabedComponentTypes = new int[] { newComp.ComponentType };
            newComp.AccountContainerComponentDetail.ConditionOrderListComponentDetail = new XueQiaoFoundation.BusinessResources.Models.ConditionOrderListComponentDetail();

            Workspace.TradeComponents.Add(newComp);
            AddViewForAccountContainerComponent(newComp, out DraggableComponentUIDM addedItem);

            componentPanelContextCtrl.PositionComponent(addedItem, null);
            componentPanelContextCtrl.FocusOnComponent(addedItem);
        }

        private void AddTradeListComponent()
        {
            var newComp = new TradeComponent
            {
                ComponentType = XueQiaoConstants.TradeCompType_TRADE_LIST,
                Width = TradeComponentConstans.ComponentWidthDefault,
                Height = TradeComponentConstans.ComponentHeightDefault,
            };

            TradeWorkspaceDataDisplayHelper.ConfigureComponentDescTitleDefaultIfNeed(newComp);
            newComp.AccountContainerComponentDetail.TogatherTabedComponentTypes = new int[] { newComp.ComponentType };
            newComp.AccountContainerComponentDetail.TradeListComponentDetail = new XueQiaoFoundation.BusinessResources.Models.TradeListComponentDetail();

            Workspace.TradeComponents.Add(newComp);
            AddViewForAccountContainerComponent(newComp, out DraggableComponentUIDM addedItem);

            componentPanelContextCtrl.PositionComponent(addedItem, null);
            componentPanelContextCtrl.FocusOnComponent(addedItem);
        }

        private void AddPositionListComponent()
        {
            var newComp = new TradeComponent
            {
                ComponentType = XueQiaoConstants.TradeCompType_POSITION_LIST,
                Width = TradeComponentConstans.ComponentWidthDefault,
                Height = TradeComponentConstans.ComponentHeightDefault,
            };

            TradeWorkspaceDataDisplayHelper.ConfigureComponentDescTitleDefaultIfNeed(newComp);
            newComp.AccountContainerComponentDetail.TogatherTabedComponentTypes = new int[] { newComp.ComponentType };
            newComp.AccountContainerComponentDetail.PositionListComponentDetail = new XueQiaoFoundation.BusinessResources.Models.PositionListComponentDetail();

            Workspace.TradeComponents.Add(newComp);
            AddViewForAccountContainerComponent(newComp, out DraggableComponentUIDM addedItem);

            componentPanelContextCtrl.PositionComponent(addedItem, null);
            componentPanelContextCtrl.FocusOnComponent(addedItem);
        }

        private void AddPositionAssistantComponent()
        {
            var newComp = new TradeComponent
            {
                ComponentType = XueQiaoConstants.TradeCompType_POSITION_ASSISTANT,
                Width = TradeComponentConstans.ComponentWidthDefault,
                Height = TradeComponentConstans.ComponentHeightDefault,
            };

            TradeWorkspaceDataDisplayHelper.ConfigureComponentDescTitleDefaultIfNeed(newComp);
            newComp.AccountContainerComponentDetail.TogatherTabedComponentTypes = new int[] { newComp.ComponentType };
            newComp.AccountContainerComponentDetail.PositionAssistantComponentDetail = new XueQiaoFoundation.BusinessResources.Models.PositionAssistantComponentDetail();

            Workspace.TradeComponents.Add(newComp);
            AddViewForAccountContainerComponent(newComp, out DraggableComponentUIDM addedItem);

            componentPanelContextCtrl.PositionComponent(addedItem, null);
            componentPanelContextCtrl.FocusOnComponent(addedItem);
        }

        private void AddTradeHistoryComponent()
        {
            var newComp = new TradeComponent
            {
                ComponentType = XueQiaoConstants.TradeCompType_TRADE_HISTORY,
                Width = TradeComponentConstans.ComponentWidthDefault,
                Height = TradeComponentConstans.ComponentHeightDefault,
            };

            TradeWorkspaceDataDisplayHelper.ConfigureComponentDescTitleDefaultIfNeed(newComp);
            newComp.AccountContainerComponentDetail.TogatherTabedComponentTypes = new int[] { newComp.ComponentType };
            newComp.AccountContainerComponentDetail.TradeHistoryComponentDetail = new XueQiaoFoundation.BusinessResources.Models.TradeHistoryComponentDetail();

            Workspace.TradeComponents.Add(newComp);
            AddViewForAccountContainerComponent(newComp, out DraggableComponentUIDM addedItem);

            componentPanelContextCtrl.PositionComponent(addedItem, null);
            componentPanelContextCtrl.FocusOnComponent(addedItem);
        }

        private void AddOrderHistoryComponent()
        {
            var newComp = new TradeComponent
            {
                ComponentType = XueQiaoConstants.TradeCompType_ORDER_HISTORY,
                Width = TradeComponentConstans.ComponentWidthDefault,
                Height = TradeComponentConstans.ComponentHeightDefault,
            };

            TradeWorkspaceDataDisplayHelper.ConfigureComponentDescTitleDefaultIfNeed(newComp);
            newComp.AccountContainerComponentDetail.TogatherTabedComponentTypes = new int[] { newComp.ComponentType };
            newComp.AccountContainerComponentDetail.OrderHistoryComponentDetail = new XueQiaoFoundation.BusinessResources.Models.OrderHistoryComponentDetail();

            Workspace.TradeComponents.Add(newComp);
            AddViewForAccountContainerComponent(newComp, out DraggableComponentUIDM addedItem);

            componentPanelContextCtrl.PositionComponent(addedItem, null);
            componentPanelContextCtrl.FocusOnComponent(addedItem);
        }

        private void AddPositionAssignHistoryComponent()
        {
            var newComp = new TradeComponent
            {
                ComponentType = XueQiaoConstants.TradeCompType_POSITION_ASSIGN_HISTORY,
                Width = TradeComponentConstans.ComponentWidthDefault,
                Height = TradeComponentConstans.ComponentHeightDefault,
            };

            TradeWorkspaceDataDisplayHelper.ConfigureComponentDescTitleDefaultIfNeed(newComp);
            newComp.AccountContainerComponentDetail.TogatherTabedComponentTypes = new int[] { newComp.ComponentType };
            newComp.AccountContainerComponentDetail.PositionAssignHistoryComponentDetail = new XueQiaoFoundation.BusinessResources.Models.PositionAssignHistoryComponentDetail();

            Workspace.TradeComponents.Add(newComp);
            AddViewForAccountContainerComponent(newComp, out DraggableComponentUIDM addedItem);

            componentPanelContextCtrl.PositionComponent(addedItem, null);
            componentPanelContextCtrl.FocusOnComponent(addedItem);
        }

        private void AddFundListComponent()
        {
            var newComp = new TradeComponent
            {
                ComponentType = XueQiaoConstants.TradeCompType_FUND,
                Width = TradeComponentConstans.ComponentWidthDefault,
                Height = TradeComponentConstans.ComponentHeightDefault
            };

            TradeWorkspaceDataDisplayHelper.ConfigureComponentDescTitleDefaultIfNeed(newComp);
            newComp.AccountContainerComponentDetail.TogatherTabedComponentTypes = new int[] { newComp.ComponentType };
            newComp.AccountContainerComponentDetail.FundListComponentDetail = new XueQiaoFoundation.BusinessResources.Models.FundListComponentDetail();

            Workspace.TradeComponents.Add(newComp);
            AddViewForAccountContainerComponent(newComp, out DraggableComponentUIDM addedItem);

            componentPanelContextCtrl.PositionComponent(addedItem, null);
            componentPanelContextCtrl.FocusOnComponent(addedItem);
        }

        private void CloseComponentWithDataModel(DraggableComponentUIDM compDataModel)
        {
            if (compDataModel == null) return;

            if (compDataModel.Component is TradeComponent)
            {
                Workspace.TradeComponents.Remove((TradeComponent)compDataModel.Component);
            }
            
            if (componentControllers.TryGetValue(compDataModel, out ITradeComponentController controller))
            {
                this.componentControllers.Remove(compDataModel);
                controller.Shutdown();
                componentPanelContextCtrl.RemoveComponent(compDataModel);
            }
        }

        private void AddComponentCmdAction(object param)
        {
            if (param is int toAddCompType)
            {
                switch (toAddCompType)
                {
                    case XueQiaoConstants.TradeCompType_CONTRACT_LIST:
                        AddContractListComponent();
                        break;
                    case XueQiaoConstants.TradeCompType_COMPOSE_LIST:
                        AddComposeListComponent();
                        break;
                    case XueQiaoConstants.TradeCompType_PLACE_ORDER:
                        NewPlaceOrderComponentWithTarget(null, null, true, true);
                        break;
                    case XueQiaoConstants.TradeCompType_ENTRUSTED_ORDER_LIST:
                        AddEntrustedOrderListComponent();
                        break;
                    case XueQiaoConstants.TradeCompType_PARKED_ORDER_LIST:
                        AddParkedOrderListComponent();
                        break;
                    case XueQiaoConstants.TradeCompType_CONDITION_ORDER_LIST:
                        AddConditionOrderListComponent();
                        break;
                    case XueQiaoConstants.TradeCompType_TRADE_LIST:
                        AddTradeListComponent();
                        break;
                    case XueQiaoConstants.TradeCompType_POSITION_LIST:
                        AddPositionListComponent();
                        break;
                    case XueQiaoConstants.TradeCompType_POSITION_ASSISTANT:
                        AddPositionAssistantComponent();
                        break;
                    case XueQiaoConstants.TradeCompType_ORDER_HISTORY:
                        AddOrderHistoryComponent();
                        break;
                    case XueQiaoConstants.TradeCompType_TRADE_HISTORY:
                        AddTradeHistoryComponent();
                        break;
                    case XueQiaoConstants.TradeCompType_POSITION_ASSIGN_HISTORY:
                        AddPositionAssignHistoryComponent();
                        break;
                    case XueQiaoConstants.TradeCompType_FUND:
                        AddFundListComponent();
                        break;
                }
            }
        }

        private void AddChartComponentCmdAction()
        {
            NewPlaceOrderComponentWithTarget(null, null, true, false);
        }

        private void AddPlaceOrderComponentCmdAction()
        {
            NewPlaceOrderComponentWithTarget(null, null, false, true);
        }

        private void AddChartAndOrderComponentCmdAction()
        {
            NewPlaceOrderComponentWithTarget(null, null, true, true);
        }

        private void CloseComponent(object param)
        {
            if (param is DraggableComponentUIDM compDataModel)
            {
                CloseComponentWithDataModel(compDataModel);
            }
        }
        
        private TradeComponent CreatePlaceOrderComponent(bool isShowChartLayout, bool isShowOrderLayout)
        {
            if (isShowChartLayout == false && isShowOrderLayout == false)
            {
                isShowOrderLayout = true;
            }

            var comp = new TradeComponent();
            comp.ComponentType = XueQiaoConstants.TradeCompType_PLACE_ORDER;
            comp.PlaceOrderContainerComponentDetail.IsShowChartLayout = isShowChartLayout;
            comp.PlaceOrderContainerComponentDetail.IsShowOrderLayout = isShowOrderLayout;
            comp.Width = Extensions.PlaceOrderComponent_NormalWidth(isShowChartLayout, isShowOrderLayout);
            comp.Height = TradeComponentConstans.PlaceOrderComponent_MininumHeight;
            return comp;
        }
        
        private void AddViewForSubscribeDataContainerComponent(TradeComponent contractListCom,
            out DraggableComponentUIDM addedComponentItem)
        {
            var controller = subscribeDataComponentCtrlFactory.CreateExport().Value;
            controller.Component = contractListCom;
            controller.ParentWorkspace = this.Workspace;
            controller.CloseComponentHandler = ctrl =>
            {
                CloseComponentWithDataModel(ctrl.ComponentItemDataModel);
            };
            controller.XqTargetAssociateHandler = this;

            controller.Initialize();
            controller.Run();

            componentControllers[controller.ComponentItemDataModel] = controller;
            componentPanelContextCtrl.AddComponent(controller.ComponentItemDataModel);

            addedComponentItem = controller.ComponentItemDataModel;
        }

        private void AddViewForPlaceOrderContainerComponent(TradeComponent placeOrderCom,
            out DraggableComponentUIDM addedComponentItem)
        {
            var controller = palceOrderComponentControllerFactory.CreateExport().Value;
            controller.Component = placeOrderCom;
            controller.ParentWorkspace = Workspace;
            controller.CloseComponentHandler = (ctrl) => {
                CloseComponentWithDataModel(ctrl.ComponentItemDataModel);
            };
            controller.XqTargetAssociateHandler = this;

            controller.Initialize();
            controller.Run();

            componentControllers[controller.ComponentItemDataModel] = controller;
            componentPanelContextCtrl.AddComponent(controller.ComponentItemDataModel);

            addedComponentItem = controller.ComponentItemDataModel;
        }

        private void AddViewForAccountContainerComponent(TradeComponent accountComp,
            out DraggableComponentUIDM addedComponentItem)
        {
            var controller = accountComponentControllerFactory.CreateExport().Value;
            controller.Component = accountComp;
            controller.ParentWorkspace = this.Workspace;
            controller.CloseComponentHandler = (ctrl) => {
                CloseComponentWithDataModel(ctrl.ComponentItemDataModel);
            };
            controller.XqTargetAssociateHandler = this;
            
            controller.Initialize();
            controller.Run();
            
            componentControllers[controller.ComponentItemDataModel] = controller;
            componentPanelContextCtrl.AddComponent(controller.ComponentItemDataModel);

            addedComponentItem = controller.ComponentItemDataModel;
        }
    }
}
