using NativeModel.Trade;
using Prism.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows;
using XueQiaoFoundation.BusinessResources.Models;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoWaf.Trade.Interfaces.Applications;
using XueQiaoWaf.Trade.Interfaces.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Controllers.Events;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Helper;
using XueQiaoWaf.Trade.Modules.Applications.ServiceControllers;
using XueQiaoWaf.Trade.Modules.Applications.ViewModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers
{
    /// <summary>
    /// 分组的订阅合约列表 controller
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class GroupedContractListController : IController
    {
        private readonly ContractListViewModel listViewModel;
        private readonly ExportFactory<DisplayColumnsSubContractListConfigDialogController> listColumnsConfigDialogFactory;
        private readonly ExportFactory<ContractInfoDialogController> contractInfoDialogCtrlFactory;
        private readonly ISubscribeContractController subscribeContractController;
        private readonly IEventAggregator eventAggregator;
        private readonly IMessageWindowService messageWindowService;
        private readonly ITradeModuleService tradeModuleService;

        private readonly DelegateCommand toConfigureListDisplayColumnsCmd;
        private readonly DelegateCommand toApplyListDefaultDisplayColumnsCmd;
        private readonly DelegateCommand clickItemTargetKeyRelatedColumnCmd;
        private readonly DelegateCommand clickItemPriceRelatedColumnCmd;

        private readonly DelegateCommand contractsSelectionChangedCmd;
        private readonly DelegateCommand showSelContractsInfoCmd;
        private readonly DelegateCommand subSelContractsQuotationCmd;
        private readonly DelegateCommand unsubSelContractsQuotationCmd;
        private readonly DelegateCommand removeSelContractsCmd;
        private readonly DelegateCommand addSelContractsToGroupCmd;
        private readonly DelegateCommand removeSelContractsFromGroupCmd;
        private readonly DelegateCommand openPlaceOrderForSelContractsCmd;
        private readonly SelectedContractsOperateCommands selectedContractsOptCommands;
        private IEnumerable<SubscribeContractDataModel> selectedContractItems;

        [ImportingConstructor]
        public GroupedContractListController(ContractListViewModel listViewModel,
            ExportFactory<DisplayColumnsSubContractListConfigDialogController> listColumnsConfigDialogFactory,
            ExportFactory<ContractInfoDialogController> contractInfoDialogCtrlFactory,
            ISubscribeContractController subscribeContractController,
            IEventAggregator eventAggregator,
            IMessageWindowService messageWindowService,
            ITradeModuleService tradeModuleService)
        {
            this.listViewModel = listViewModel;
            this.listColumnsConfigDialogFactory = listColumnsConfigDialogFactory;
            this.contractInfoDialogCtrlFactory = contractInfoDialogCtrlFactory;
            this.subscribeContractController = subscribeContractController;
            this.eventAggregator = eventAggregator;
            this.messageWindowService = messageWindowService;
            this.tradeModuleService = tradeModuleService;

            toConfigureListDisplayColumnsCmd = new DelegateCommand(ToConfigureListDisplayColumns);
            toApplyListDefaultDisplayColumnsCmd = new DelegateCommand(ToApplyListDefaultDisplayColumns);
            clickItemTargetKeyRelatedColumnCmd = new DelegateCommand(ClickItemTargetKeyRelatedColumn);
            clickItemPriceRelatedColumnCmd = new DelegateCommand(ClickItemPriceRelatedColumn);

            showSelContractsInfoCmd = new DelegateCommand(ShowSelContractsInfo, CanShowSelContractsInfo);
            subSelContractsQuotationCmd = new DelegateCommand(SubSelContractsQuotation, CanSubSelContractsQuotation);
            unsubSelContractsQuotationCmd = new DelegateCommand(UnsubSelContractsQuotation, CanUnsubSelContractsQuotation);
            removeSelContractsCmd = new DelegateCommand(RemoveSelContracts, CanRemoveSelContracts);
            addSelContractsToGroupCmd = new DelegateCommand(AddSelContractsToGroup, CanAddSelContractsToGroup);
            removeSelContractsFromGroupCmd = new DelegateCommand(RemoveSelContractsFromGroup, CanRemoveSelContractsFromGroup);
            openPlaceOrderForSelContractsCmd = new DelegateCommand(OpenPlaceOrderForSelContracts, CanOpenPlaceOrderForSelContracts);
            contractsSelectionChangedCmd = new DelegateCommand(ContractsSelectionChanged);
            selectedContractsOptCommands = new SelectedContractsOperateCommands
            {
                ItemsSelectionChangedCmd = contractsSelectionChangedCmd,
                ShowContractInfoCmd = showSelContractsInfoCmd,
                SubscribeQuotationCmd = subSelContractsQuotationCmd,
                UnsubscribeQuotationCmd = unsubSelContractsQuotationCmd,
                RemoveItemCmd = removeSelContractsCmd,
                AddToGroupCmd = addSelContractsToGroupCmd,
                RemoveFromGroupCmd = removeSelContractsFromGroupCmd,
                OpenPlaceOrderComponentCmd = openPlaceOrderForSelContractsCmd,
            };
        }

        /// <summary>
        /// 组件信息
        /// </summary>
        public XueQiaoFoundation.BusinessResources.DataModels.TradeComponent Component { get; set; }

        /// <summary>
        /// 所在工作空间
        /// </summary>
        public XueQiaoFoundation.BusinessResources.DataModels.TabWorkspace ParentWorkspace { get; set; }
        
        /// <summary>
        /// 标的关联处理器
        /// </summary>
        public ITradeComponentXqTargetAssociateHandler XqTargetAssociateHandler { get; set; }
        /// <summary>
        /// 所在分组
        /// </summary>
        public SubscribeDataGroup GroupTab { get; set; }

        /// <summary>
        /// 列表内容视图。在 Initialize 后可获得
        /// </summary>
        public object ListContentView => listViewModel.View;

        public void Initialize()
        {
            if (Component == null) throw new ArgumentNullException("`Component` can't be null before initialize.");
            if (ParentWorkspace == null) throw new ArgumentNullException("`ParentWorkspace` must be setted before initialize.");
            if (GroupTab == null) throw new ArgumentNullException("GroupTab");
            if (Component.SubscribeDataContainerComponentDetail == null) throw new ArgumentNullException("Component.SubscribeDataContainerComponentDetail");
            if (Component.SubscribeDataContainerComponentDetail.ContractListComponentDetail == null) throw new ArgumentNullException("Component.SubscribeDataContainerComponentDetail.ContractListComponentDetail");

            listViewModel.GroupTab = this.GroupTab;
            listViewModel.AddibleCustomGroups.Clear();
            listViewModel.AddibleCustomGroups.AddRange(tradeModuleService.SubscribeDataGroupsDataRoot?.ContractGroups?
                .Where(i => i.GroupType == SubscribeDataGroupType.Custom)
                .Select(i => new AddSubscribeDataToGroupDM(i, this.addSelContractsToGroupCmd))
                .ToArray());
            listViewModel.ToConfigureListDisplayColumnsCmd = toConfigureListDisplayColumnsCmd;
            listViewModel.ToApplyListDefaultDisplayColumnsCmd = toApplyListDefaultDisplayColumnsCmd;
            listViewModel.SelectedItemsOptCommands = selectedContractsOptCommands;
            listViewModel.ClickItemTargetKeyRelatedColumnCmd = clickItemTargetKeyRelatedColumnCmd;
            listViewModel.ClickItemPriceRelatedColumnCmd = clickItemPriceRelatedColumnCmd;

            // configure contract list initial display columns
            var initialColumns = Component.SubscribeDataContainerComponentDetail.ContractListComponentDetail.ContractListColumns;
            ApplyListDisplayColumnsIfNeed(initialColumns);

            listViewModel.PresentSubAccountId = ParentWorkspace.SubAccountId;

            eventAggregator.GetEvent<GlobalApplyContractListDisplayColumnsEvent>().Subscribe(ReceiveGlobalApplyContractListDisplayColumnsEvent);
            eventAggregator.GetEvent<ContractListCustomGroupsChangedEvent>().Subscribe(ReceiveContractListCustomGroupsChanged);
            PropertyChangedEventManager.AddHandler(ParentWorkspace, ParentWorkspacePropertyChanged, "");
        }

        public void Run()
        {

        }

        public void Shutdown()
        {
            eventAggregator.GetEvent<GlobalApplyContractListDisplayColumnsEvent>().Unsubscribe(ReceiveGlobalApplyContractListDisplayColumnsEvent);
            eventAggregator.GetEvent<ContractListCustomGroupsChangedEvent>().Unsubscribe(ReceiveContractListCustomGroupsChanged);
            PropertyChangedEventManager.RemoveHandler(ParentWorkspace, ParentWorkspacePropertyChanged, "");
        }

        private void ParentWorkspacePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TabWorkspace.SubAccountId))
            {
                listViewModel.PresentSubAccountId = ParentWorkspace.SubAccountId;
            }
        }

        private void ReceiveContractListCustomGroupsChanged(SubscribeDataGroupsChangedEventArgs args)
        {
            var newCustomGroups = (args.NewGroups ?? new SubscribeDataGroup[] {})
                .Where(i => i.GroupType == SubscribeDataGroupType.Custom)
                .ToArray();

            listViewModel.AddibleCustomGroups.Clear();
            listViewModel.AddibleCustomGroups.AddRange(newCustomGroups
                    .Select(i => new AddSubscribeDataToGroupDM(i, this.addSelContractsToGroupCmd))
                    .ToArray());
        }

        private void ReceiveGlobalApplyContractListDisplayColumnsEvent(IEnumerable<ListColumnInfo> msg)
        {
            ApplyListDisplayColumnsIfNeed(msg);
        }

        private void ApplyListDisplayColumnsIfNeed(IEnumerable<ListColumnInfo> displayColumnInfos)
        {
            var currentDisplayColumnInfos = this.listViewModel.ListDisplayColumnInfos ?? new ListColumnInfo[] { };
            if (displayColumnInfos?.SequenceEqual(currentDisplayColumnInfos) != true)
            {
                Component.SubscribeDataContainerComponentDetail.ContractListComponentDetail.ContractListColumns 
                    = displayColumnInfos.ToArray();
                this.listViewModel.ResetListDisplayColumns(displayColumnInfos);
            }
        }


        private void AddPropertyChangedHandlerForContractItems(IEnumerable<SubscribeContractDataModel> contractItems)
        {
            if (contractItems == null) return;
            foreach (var o in contractItems)
            {
                PropertyChangedEventManager.RemoveHandler(o, ContractItemPropChanged, "");
                PropertyChangedEventManager.AddHandler(o, ContractItemPropChanged, "");
            }
        }

        private void RemovePropertyChangedHandlerForContractItems(IEnumerable<SubscribeContractDataModel> contractItems)
        {
            if (contractItems == null) return;
            foreach (var o in contractItems)
            {
                PropertyChangedEventManager.RemoveHandler(o, ContractItemPropChanged, "");
            }
        }

        private void ContractItemPropChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SubscribeContractDataModel.SubscribeState))
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    subSelContractsQuotationCmd?.RaiseCanExecuteChanged();
                    unsubSelContractsQuotationCmd?.RaiseCanExecuteChanged();
                });
            }
            else if (e.PropertyName == nameof(SubscribeContractDataModel.OnTradingSubAccountIds)
                || e.PropertyName == nameof(SubscribeContractDataModel.ExistPositionSubAccountIds)
                || e.PropertyName == nameof(SubscribeContractDataModel.IsComposeRelated)
                || e.PropertyName == nameof(SubscribeContractDataModel.CustomGroupKeys))
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    addSelContractsToGroupCmd?.RaiseCanExecuteChanged();
                    removeSelContractsFromGroupCmd?.RaiseCanExecuteChanged();
                });
            }
            else if (e.PropertyName == nameof(SubscribeContractDataModel.IsXqTargetExpired))
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    removeSelContractsCmd?.RaiseCanExecuteChanged();
                    openPlaceOrderForSelContractsCmd?.RaiseCanExecuteChanged();
                });
            }
        }

        private void ContractsSelectionChanged(object obj)
        {
            var oldSelContractItems = this.selectedContractItems;
            RemovePropertyChangedHandlerForContractItems(oldSelContractItems);

            var newSelContracts = (obj as IList)?.Cast<SubscribeContractDataModel>().ToArray();

            this.selectedContractItems = newSelContracts;
            AddPropertyChangedHandlerForContractItems(newSelContracts);
        
            subSelContractsQuotationCmd?.RaiseCanExecuteChanged();
            unsubSelContractsQuotationCmd?.RaiseCanExecuteChanged();
            removeSelContractsCmd?.RaiseCanExecuteChanged();
            addSelContractsToGroupCmd?.RaiseCanExecuteChanged();
            removeSelContractsFromGroupCmd?.RaiseCanExecuteChanged();
            openPlaceOrderForSelContractsCmd?.RaiseCanExecuteChanged();
        }

        private void ToConfigureListDisplayColumns()
        {
            var dialogCtrl = listColumnsConfigDialogFactory.CreateExport().Value;
            dialogCtrl.DialogOwner = UIHelper.GetWindowOfUIElement(listViewModel.View);
            dialogCtrl.OriginDisplayingColumnInfos = listViewModel.ListDisplayColumnInfos?
                .Select(i => i.Clone() as ListColumnInfo).ToArray();
            dialogCtrl.Initialize();
            dialogCtrl.Run();
            dialogCtrl.Shutdown();
            if (dialogCtrl.ConfiguredDisplayColomunsResult != null)
            {
                ApplyListDisplayColumnsIfNeed(dialogCtrl.ConfiguredDisplayColomunsResult);
            }
        }

        private void ToApplyListDefaultDisplayColumns()
        {
            var defaultDisplayColumnCodes = TradeWorkspaceDataDisplayHelper.DefaultContractListDisplayColumns.Select(i => i.GetHashCode()).ToArray();
            var currentDisplayColumnInfos = listViewModel.ListDisplayColumnInfos?.ToArray() ?? new ListColumnInfo[] { };

            var currentDisplayColumnCodes = currentDisplayColumnInfos.Select(i => i.ColumnCode).ToArray();
            if (currentDisplayColumnCodes.SequenceEqual(defaultDisplayColumnCodes)) return;

            var resetItems = new List<ListColumnInfo>();
            foreach (var columnCode in defaultDisplayColumnCodes)
            {
                var columnInfo = currentDisplayColumnInfos.FirstOrDefault(i => i.ColumnCode == columnCode);
                if (columnInfo == null)
                {
                    columnInfo = new ListColumnInfo { ColumnCode = columnCode };
                }
                resetItems.Add(columnInfo);
            }

            ApplyListDisplayColumnsIfNeed(resetItems.ToArray());
        }

        private void ShowSelContractsInfo()
        {
            var selectedContract = this.selectedContractItems.FirstOrDefault();
            if (selectedContract == null) return;

            var dialogShowLocation = SubscribeItemAppropriateShowDialogLocation(selectedContract);

            var infoDialog = contractInfoDialogCtrlFactory.CreateExport().Value;
            infoDialog.TargetContractId = selectedContract.ContractId;
            infoDialog.DialogOwner = UIHelper.GetWindowOfUIElement(listViewModel.View);
            infoDialog.DialogShowLocationRelativeToScreen = dialogShowLocation;

            infoDialog.Initialize();
            infoDialog.Run();
            infoDialog.Shutdown();
        }

        private bool CanShowSelContractsInfo()
        {
            return 1 == this.selectedContractItems?.Count();
        }

        private void SubSelContractsQuotation()
        {
            var selectedContracts = this.selectedContractItems;
            if (selectedContracts?.Any() != true) return;

            var waitSubItems = selectedContracts.Where(i => i.SubscribeState != MarketSubscribeState.Subscribed && i.SubscribeState != MarketSubscribeState.Subscribing).ToArray();
            foreach (var subscribeItem in waitSubItems)
            {
                subscribeContractController.SubscribeContractQuotationIfNeed(subscribeItem.ContractId, null);
            }
        }

        private bool CanSubSelContractsQuotation()
        {
            return this.selectedContractItems?
                .Any(i => i.SubscribeState != MarketSubscribeState.Subscribed && i.SubscribeState != MarketSubscribeState.Subscribing)
                ?? false;
        }

        private void UnsubSelContractsQuotation()
        {
            var selectedContracts = this.selectedContractItems;
            if (selectedContracts?.Any() != true) return;

            var waitUnsubItems = selectedContracts.Where(i => i.SubscribeState != MarketSubscribeState.Unsubscribed && i.SubscribeState != MarketSubscribeState.Unsubscribing).ToArray();
            foreach (var subscribeItem in waitUnsubItems)
            {
                subscribeContractController.UnsubscribeContractQuotationIfNeed(subscribeItem.ContractId, null);
            }
        }

        private bool CanUnsubSelContractsQuotation()
        {
            return this.selectedContractItems?
                .Any(i => i.SubscribeState != MarketSubscribeState.Unsubscribed && i.SubscribeState != MarketSubscribeState.Unsubscribing)
                ?? false;
        }

        private void RemoveSelContracts(object obj)
        {
            var groupTab = obj as SubscribeDataGroup;
            if (SubscribeDataGroupType.All != groupTab?.GroupType && SubscribeDataGroupType.IsExpired != groupTab?.GroupType) return;

            var selectedContracts = this.selectedContractItems;
            if (selectedContracts?.Any() != true) return;
            
            var dialogOwner = UIHelper.GetWindowOfUIElement(listViewModel.View);
            var dialogShowLocation = SubscribeItemAppropriateShowDialogLocation(selectedContracts.First());

            if (messageWindowService.ShowQuestionDialog(dialogOwner, dialogShowLocation, null, null, "确认要删除选中的项目吗？", false, "删除", "取消") != true)
                return;

            foreach (var subscribeItem in selectedContracts)
            {
                // FIXME: 改成删除该合约的全部订阅???
                //subscribeContractController.RemoveSubscribeConract(subscribeItem.ContractId, subscribeItem.SubscribeGroupKey);
                subscribeContractController.RemoveSubscribeConract(subscribeItem.ContractId);
            }
        }

        private bool CanRemoveSelContracts(object obj)
        {
            var groupTab = obj as SubscribeDataGroup;
            if (SubscribeDataGroupType.All != groupTab?.GroupType && SubscribeDataGroupType.IsExpired != groupTab?.GroupType) return false;
            if (this.selectedContractItems?.Any() != true) return false;
            return true;
        }

        private void AddSelContractsToGroup(object obj)
        {
            var groupTab = obj as SubscribeDataGroup;
            if (SubscribeDataGroupType.Custom != groupTab?.GroupType) return;
            var selectedContracts = this.selectedContractItems;
            if (selectedContracts?.Any() != true) return;

            foreach (var subscribeItem in selectedContracts)
            {
                var newCustomGroupKeys = (subscribeItem.CustomGroupKeys ?? new string[] { }).Union(new string[] { groupTab.GroupKey });
                subscribeContractController.UpdateSubscribeContractsWithSameId(subscribeItem.ContractId, null,
                    () => new SubscribeContractUpdateTemplate { CustomGroupKeys = new Tuple<IEnumerable<string>>(newCustomGroupKeys) });
            }
        }

        private bool CanAddSelContractsToGroup(object obj)
        {
            var groupTab = obj as SubscribeDataGroup;
            if (SubscribeDataGroupType.Custom != groupTab?.GroupType) return false;
            if (this.selectedContractItems?.Any() != true) return false;
            return true;
        }

        private void RemoveSelContractsFromGroup(object obj)
        {
            var groupTab = obj as SubscribeDataGroup;
            if (SubscribeDataGroupType.Custom != groupTab?.GroupType) return;
            var selectedContracts = this.selectedContractItems;
            if (selectedContracts?.Any() != true) return;

            foreach (var subscribeItem in selectedContracts)
            {
                var newCustomGroupKeys = (subscribeItem.CustomGroupKeys ?? new string[] { }).Except(new string[] { groupTab.GroupKey });
                subscribeContractController.UpdateSubscribeContractsWithSameId(subscribeItem.ContractId, null,
                    () => new SubscribeContractUpdateTemplate { CustomGroupKeys = new Tuple<IEnumerable<string>>(newCustomGroupKeys) });
            }
        }

        private bool CanRemoveSelContractsFromGroup(object obj)
        {
            var groupTab = obj as SubscribeDataGroup;
            if (SubscribeDataGroupType.Custom != groupTab?.GroupType) return false;
            if (this.selectedContractItems?.Any() != true) return false;
            return true;
        }

        private void OpenPlaceOrderForSelContracts()
        {
            var selItems = this.selectedContractItems;
            var tarItem = selItems?.FirstOrDefault(i => i.IsXqTargetExpired != true);
            if (tarItem != null)
            {
                var associateArgs = new TradeComponentXqTargetAssociateArgs(this.ParentWorkspace, this.Component, ClientXQOrderTargetType.CONTRACT_TARGET, $"{tarItem.ContractId}");
                XqTargetAssociateHandler?.NewXqTargetPlaceOrderComponent(UIHelper.GetWindowOfUIElement(listViewModel.View),
                    SubscribeItemAppropriateShowDialogLocation(tarItem), associateArgs);
            }
        }

        private bool CanOpenPlaceOrderForSelContracts()
        {
            var selItems = this.selectedContractItems;
            if (selItems?.Count(i => i.IsXqTargetExpired != true) == 1) return true;
            return false;
        }
        
        private void ClickItemTargetKeyRelatedColumn(object obj)
        {
            var item = obj as SubscribeContractDataModel;
            if (item == null || item.IsXqTargetExpired == true) return;

            // 联动
            var associateArgs = new TradeComponentXqTargetAssociateArgs(this.ParentWorkspace, this.Component,
                ClientXQOrderTargetType.CONTRACT_TARGET, $"{item.ContractId}");
            XqTargetAssociateHandler?.HandleXqTargetAssociate(UIHelper.GetWindowOfUIElement(listViewModel.View),
                SubscribeItemAppropriateShowDialogLocation(item), associateArgs);
        }

        private void ClickItemPriceRelatedColumn(object obj)
        {
            var args = obj as object[];
            if (args?.Count() != 2) return;

            var item = args[0] as SubscribeContractDataModel;
            var price = args[1] as double?;

            if (item == null || item.IsXqTargetExpired == true) return;

            var associateCustomInfos = new Dictionary<string, object>();
            if (price != null)
                associateCustomInfos.Add(TradeComponentAssociateConstants.ComponentAssociateArg_Price, price.Value);

            // 联动
            var associateArgs = new TradeComponentXqTargetAssociateArgs(this.ParentWorkspace, this.Component,
                ClientXQOrderTargetType.CONTRACT_TARGET, $"{item.ContractId}", associateCustomInfos);
            XqTargetAssociateHandler?.HandleXqTargetAssociate(UIHelper.GetWindowOfUIElement(listViewModel.View),
                SubscribeItemAppropriateShowDialogLocation(item), associateArgs);
        }

        private Point? SubscribeItemAppropriateShowDialogLocation(SubscribeContractDataModel subscribeItem)
        {
            var itemUIElement = listViewModel.SubscribeItemElement(subscribeItem);
            if (itemUIElement == null) return null;
            var itemUIElementSize = itemUIElement.RenderSize;
            var screenPoint = itemUIElement.PointToScreen(new Point(itemUIElementSize.Width / 4, itemUIElementSize.Height / 4));
            var location = UIHelper.TransformToWpfPoint(screenPoint, itemUIElement);
            return location;
        }
    }
}
