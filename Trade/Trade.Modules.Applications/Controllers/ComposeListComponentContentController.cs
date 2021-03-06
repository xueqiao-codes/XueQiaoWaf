using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoWaf.Trade.Modules.Applications.Controllers.Events;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.ViewModels;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoWaf.Trade.Modules.Applications.Helper;
using XueQiaoFoundation.BusinessResources.Models;
using System.Windows;
using XueQiaoWaf.Trade.Interfaces.DataModels;
using XueQiaoWaf.Trade.Interfaces.Applications;
using XueQiaoFoundation.BusinessResources.Constants;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers
{
    /// <summary>
    /// 组合列表组件的内容控制器
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class ComposeListComponentContentController : IController
    {
        private readonly SubscribeDataGroupListContainerVM listContainerViewModel;
        private readonly ExportFactory<GroupedComposeListController> listControllerFactory;
        private readonly ExportFactory<SubscribeDataGroupEditDialogController> dataGroupEditDialogCtrlFatory;
        private readonly ITradeModuleService tradeModuleService;
        private readonly IEventAggregator eventAggregator;
        private readonly IMessageWindowService messageWindowService;
        
        private readonly Dictionary<SubscribeDataGroup, GroupedComposeListController> groupKeyedComposeListCtrls 
            = new Dictionary<SubscribeDataGroup, GroupedComposeListController>();

        private readonly DelegateCommand toNewGroupTabCmd;
        private readonly DelegateCommand toRemoveGroupTabCmd;
        private readonly DelegateCommand toRenameGroupTabCmd;

        [ImportingConstructor]
        public ComposeListComponentContentController(SubscribeDataGroupListContainerVM listContainerViewModel,
            ExportFactory<GroupedComposeListController> listControllerFactory,
            ExportFactory<SubscribeDataGroupEditDialogController> dataGroupEditDialogCtrlFatory,
            ITradeModuleService tradeModuleService,
            IEventAggregator eventAggregator,
            IMessageWindowService messageWindowService)
        {
            this.listContainerViewModel = listContainerViewModel;
            this.listControllerFactory = listControllerFactory;
            this.dataGroupEditDialogCtrlFatory = dataGroupEditDialogCtrlFatory;
            this.tradeModuleService = tradeModuleService;
            this.eventAggregator = eventAggregator;
            this.messageWindowService = messageWindowService;

            toNewGroupTabCmd = new DelegateCommand(ToNewGroupTab, CanNewGroupTab);
            toRemoveGroupTabCmd = new DelegateCommand(ToRemoveGroupTab, CanRemoveGroupTab);
            toRenameGroupTabCmd = new DelegateCommand(ToRenameGroupTab, CanRenameGroupTab);
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
        /// 组件的内容视图。在 Initialize 后可获得
        /// </summary>
        public object ComponentContentView => listContainerViewModel.View;

        public void Initialize()
        {
            if (Component == null) throw new ArgumentNullException("`Component` can't be null before initialize.");
            if (ParentWorkspace == null) throw new ArgumentNullException("`ParentWorkspace` must be setted before initialize.");
            if (Component.SubscribeDataContainerComponentDetail == null) throw new ArgumentNullException("Component.SubscribeDataContainerComponentDetail");

            InitializeComposeListComponentDetailIfNeed();

            PropertyChangedEventManager.AddHandler(listContainerViewModel, ListContainerViewModelPropertyChanged, "");

            listContainerViewModel.ToNewGroupTabCmd = toNewGroupTabCmd;
            listContainerViewModel.ToRemoveGroupTabCmd = toRemoveGroupTabCmd;
            listContainerViewModel.ToRenameGroupTabCmd = toRenameGroupTabCmd;
            listContainerViewModel.FrozenGroupNum = tradeModuleService.SubscribeDataGroupsDataRoot?.ComposeGroups.Count(i => i.GroupType != SubscribeDataGroupType.Custom)??0;

            var initialGroupListViews = tradeModuleService.SubscribeDataGroupsDataRoot?.ComposeGroups?.Select(i => new SubscribeDataGroupListViewDM(i)).ToArray();
            listContainerViewModel.DataGroupListViews.AddRange(initialGroupListViews);
            if (initialGroupListViews != null)
            {
                var selGroupListViewItem = initialGroupListViews.FirstOrDefault(i => i.Group.GroupKey == Component.SubscribeDataContainerComponentDetail.ComposeListComponentDetail.SelectedListGroupKey);
                if (selGroupListViewItem == null)
                {
                    selGroupListViewItem = initialGroupListViews.FirstOrDefault();
                }
                listContainerViewModel.SelectedGroupListViewItem = selGroupListViewItem;
            }

            CollectionChangedEventManager.AddHandler(listContainerViewModel.DataGroupListViews, DataGroupCollectionChanged);
            eventAggregator.GetEvent<ComposeListCustomGroupsChangedEvent>().Subscribe(ReceiveComposeListCustomGroupsChanged);
            eventAggregator.GetEvent<ComposeListGroupsOrderIndexChangedEvent>().Subscribe(ReceiveComposeListGroupsOrderIndexChanged);
        }

        public void Run()
        {

        }

        public void Shutdown()
        {
            PropertyChangedEventManager.RemoveHandler(listContainerViewModel, ListContainerViewModelPropertyChanged, "");
            CollectionChangedEventManager.RemoveHandler(listContainerViewModel.DataGroupListViews, DataGroupCollectionChanged);
            eventAggregator.GetEvent<ComposeListCustomGroupsChangedEvent>().Unsubscribe(ReceiveComposeListCustomGroupsChanged);
            eventAggregator.GetEvent<ComposeListGroupsOrderIndexChangedEvent>().Subscribe(ReceiveComposeListGroupsOrderIndexChanged);

            foreach (var listCtrl in groupKeyedComposeListCtrls)
            {
                listCtrl.Value.Shutdown();
            }
            groupKeyedComposeListCtrls.Clear();
        }

        private void InitializeComposeListComponentDetailIfNeed()
        {
            var composeListComponentDetail = Component.SubscribeDataContainerComponentDetail.ComposeListComponentDetail;
            if (composeListComponentDetail == null)
            {
                composeListComponentDetail = new ComposeListComponentDetail();
                Component.SubscribeDataContainerComponentDetail.ComposeListComponentDetail = composeListComponentDetail;
            }

            if (string.IsNullOrEmpty(composeListComponentDetail.SelectedListGroupKey))
            {
                var selectedGroupKey = SubscribeDataGroup.Group_All_SharedKey;
                composeListComponentDetail.SelectedListGroupKey = selectedGroupKey;
            }

            if (composeListComponentDetail.ComposeListColumns?.Any() != true)
            {
                var listInitialColumns = tradeModuleService.TradeWorkspaceDataRoot?.GlobalAppliedComposeListDisplayColumns?.Select(i => i.ColumnCode).ToArray();
                if (listInitialColumns == null)
                {
                    listInitialColumns = TradeWorkspaceDataDisplayHelper.DefaultComposeListDisplayColumns.Select(i => i.GetHashCode()).ToArray();
                }
                composeListComponentDetail.ComposeListColumns = listInitialColumns.Select(i => new ListColumnInfo
                { ColumnCode = i, ContentAlignment = XueQiaoConstants.ListColumnContentAlignment_Left })
                .ToArray();
            }

            // 消除重复列
            var composeListColumnGroups = composeListComponentDetail.ComposeListColumns.GroupBy(i => i.ColumnCode);
            composeListComponentDetail.ComposeListColumns = composeListColumnGroups.Select(i => i.First()).ToArray();
        }

        private void ListContainerViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SubscribeDataGroupListContainerVM.SelectedGroupListViewItem))
            {
                var selectedItem = listContainerViewModel.SelectedGroupListViewItem;
                if (selectedItem == null) return;

                // 滚动到选中项
                listContainerViewModel.Scroll2GroupItem(selectedItem);

                // 改变 SelectedListGroupKey
                Component.SubscribeDataContainerComponentDetail.ComposeListComponentDetail.SelectedListGroupKey = selectedItem.Group.GroupKey;

                // Config list view if need
                if (selectedItem.ListView == null)
                {
                    GroupedComposeListController listCtrl = null;
                    if (!groupKeyedComposeListCtrls.TryGetValue(selectedItem.Group, out listCtrl))
                    {
                        listCtrl = listControllerFactory.CreateExport().Value;
                        listCtrl.Component = this.Component;
                        listCtrl.ParentWorkspace = this.ParentWorkspace;
                        listCtrl.XqTargetAssociateHandler = this.XqTargetAssociateHandler;
                        listCtrl.GroupTab = selectedItem.Group;

                        listCtrl.Initialize();
                        listCtrl.Run();

                        groupKeyedComposeListCtrls[selectedItem.Group] = listCtrl;
                    }
                    selectedItem.ListView = listCtrl.ListContentView;
                }
            }
        }

        private void DataGroupCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                // reset action 改变列表大小，更新 DataRoot 数据，同时发布事件
                UpdateDataRootComposeGroups(true);
            }
            else
            {
                // 别的 action 类型只改变顺序，不改变列表大小。改变列表大小只能通过手动的添加或删除和reset action
                var currentGroups = listContainerViewModel.DataGroupListViews.Select(i => i.Group).ToArray();
                var groupOrderIndexes = new Dictionary<SubscribeDataGroup, int>();
                for (var idx = 0; idx < currentGroups.Count(); idx++)
                {
                    groupOrderIndexes.Add(currentGroups[idx], idx);
                }
                eventAggregator.GetEvent<ComposeListGroupsOrderIndexChangedEvent>().Unsubscribe(ReceiveComposeListGroupsOrderIndexChanged);
                eventAggregator.GetEvent<ComposeListGroupsOrderIndexChangedEvent>()
                    .Publish(new SubscribeDataGroupsOrderIndexChangedEventArgs(this, groupOrderIndexes));
                eventAggregator.GetEvent<ComposeListGroupsOrderIndexChangedEvent>().Subscribe(ReceiveComposeListGroupsOrderIndexChanged);
            }
        }

        private void ReceiveComposeListCustomGroupsChanged(SubscribeDataGroupsChangedEventArgs args)
        {
            if (args == null) return;
            if (args.Sender == this) return;

            var newCustomGroups = (args.NewGroups ?? new SubscribeDataGroup[] { })
                .Where(i => i.GroupType == SubscribeDataGroupType.Custom)
                .ToArray();

            var newDataGroupListViews = listContainerViewModel.DataGroupListViews
                .Where(i => i.Group.GroupType != SubscribeDataGroupType.Custom).ToList();
            foreach (var customGroup in newCustomGroups)
            {
                var dataGroupListViewDM = listContainerViewModel.DataGroupListViews.FirstOrDefault(i => i.Group == customGroup);
                if (dataGroupListViewDM == null)
                {
                    dataGroupListViewDM = new SubscribeDataGroupListViewDM(customGroup);
                }
                newDataGroupListViews.Add(dataGroupListViewDM);
            }

            var rmListVMKeys = groupKeyedComposeListCtrls.Keys.ToArray().Except(newDataGroupListViews.Select(i => i.Group).ToArray());
            foreach (var rmKey in rmListVMKeys)
            {
                groupKeyedComposeListCtrls.Remove(rmKey);
            }

            var selGroupListViewItem = listContainerViewModel.SelectedGroupListViewItem;
            if (selGroupListViewItem == null || (selGroupListViewItem != null && !newDataGroupListViews.Contains(selGroupListViewItem)))
            {
                selGroupListViewItem = newDataGroupListViews.FirstOrDefault();
            }

            // Remove DataGroupListViews collection changed event handler before manual change the collection
            CollectionChangedEventManager.RemoveHandler(listContainerViewModel.DataGroupListViews, DataGroupCollectionChanged);

            listContainerViewModel.DataGroupListViews.Clear();
            listContainerViewModel.DataGroupListViews.AddRange(newDataGroupListViews);
            listContainerViewModel.SelectedGroupListViewItem = selGroupListViewItem;

            // Restore DataGroupListViews collection changed event handler after manual change the collection
            CollectionChangedEventManager.AddHandler(listContainerViewModel.DataGroupListViews, DataGroupCollectionChanged);
        }

        private void ReceiveComposeListGroupsOrderIndexChanged(SubscribeDataGroupsOrderIndexChangedEventArgs args)
        {
            if (args == null) return;
            if (args.Sender == this) return;

            // 只改变显示顺序，不对列表项做增减

            var originCustomGroupItems = listContainerViewModel.DataGroupListViews
                .Where(i => i.Group.GroupType == SubscribeDataGroupType.Custom).ToArray();
            var originUnCustomGroupItems = listContainerViewModel.DataGroupListViews
                .Where(i => i.Group.GroupType != SubscribeDataGroupType.Custom).ToArray();

            var orderedOriginCustomGroupItems = originCustomGroupItems.OrderBy(i =>
            {
                var orderIndexRefer = args.GroupOrderIndexes?.FirstOrDefault(idxObj => (i.Group == idxObj.Key) || (i.Group?.GroupKey == idxObj.Key.GroupKey));
                if (orderIndexRefer != null)
                {
                    return orderIndexRefer.Value.Value;
                }
                return int.MaxValue;
            }).ToArray();

            var newDataGroupItems = new List<SubscribeDataGroupListViewDM>();
            newDataGroupItems.AddRange(originUnCustomGroupItems);
            newDataGroupItems.AddRange(orderedOriginCustomGroupItems);

            var originSelectedGroupItem = listContainerViewModel.SelectedGroupListViewItem;

            // Remove DataGroupListViews collection changed event handler before manual change the collection
            CollectionChangedEventManager.RemoveHandler(listContainerViewModel.DataGroupListViews, DataGroupCollectionChanged);

            listContainerViewModel.DataGroupListViews.Clear();
            listContainerViewModel.DataGroupListViews.AddRange(newDataGroupItems);
            listContainerViewModel.SelectedGroupListViewItem = originSelectedGroupItem;

            // Restore DataGroupListViews collection changed event handler after manual change the collection
            CollectionChangedEventManager.AddHandler(listContainerViewModel.DataGroupListViews, DataGroupCollectionChanged);

        }

        private void UpdateDataRootComposeGroups(bool publishCustomGroupsChangedEvent)
        {
            var newGroups = listContainerViewModel.DataGroupListViews
                .Select(i => i.Group).ToArray();

            var oldCustomGroups = tradeModuleService.SubscribeDataGroupsDataRoot?.ComposeGroups?.Where(i => i.GroupType == SubscribeDataGroupType.Custom).ToArray();
            var newCustomGroups = newGroups.Where(i => i.GroupType == SubscribeDataGroupType.Custom).ToArray();

            // 更新 SubscribeDataGroupsDataRoot.ComposeGroups
            var dataRoot = tradeModuleService.SubscribeDataGroupsDataRoot;
            if (dataRoot != null)
            {
                dataRoot.ComposeGroups = newGroups;
            }

            if (publishCustomGroupsChangedEvent)
            {
                // Unsubscribe ComposeListCustomGroupsChangedEvent Before publish 
                eventAggregator.GetEvent<ComposeListCustomGroupsChangedEvent>().Unsubscribe(ReceiveComposeListCustomGroupsChanged);

                eventAggregator.GetEvent<ComposeListCustomGroupsChangedEvent>().Publish(new SubscribeDataGroupsChangedEventArgs(this, oldCustomGroups, newCustomGroups));

                // Restore subscribe ComposeListCustomGroupsChangedEvent Before publish 
                eventAggregator.GetEvent<ComposeListCustomGroupsChangedEvent>().Subscribe(ReceiveComposeListCustomGroupsChanged);
            }
        }

        private bool VerifyCommandParameter<T>(object commandParam, out T targetTypeParam)
        {
            targetTypeParam = default(T);
            if (commandParam is T tarParam)
            {
                targetTypeParam = tarParam;
                return true;
            }
            return false;
        }

        private int GetInsertNewDataGroupIndex(SubscribeDataGroupListViewDM referenceGroupDM)
        {
            var optCollection = listContainerViewModel.DataGroupListViews;
            var reviseReferItem = referenceGroupDM;
            if (referenceGroupDM.Group.GroupType != SubscribeDataGroupType.Custom)
            {
                reviseReferItem = optCollection.LastOrDefault(i => i.Group.GroupType != SubscribeDataGroupType.Custom);
            }
            int insertLocation = optCollection.Count;
            if (reviseReferItem != null)
            {
                insertLocation = optCollection.IndexOf(reviseReferItem) + 1;
            }
            return insertLocation;
        }

        private void ToNewGroupTab(object obj)
        {
            SubscribeDataGroupListViewDM insertBeforeGroupDM = null;
            Point? dialogShowLocationRelativeToScreen = null;
            if (VerifyCommandParameter(obj, out SubscribeDataGroupListViewDM _dataGroupListViewDM))
            {
                insertBeforeGroupDM = _dataGroupListViewDM;
                var itemUIElement = listContainerViewModel.GroupItemElement(insertBeforeGroupDM);
                dialogShowLocationRelativeToScreen = GroupItemAppropriateShowDialogLocation(itemUIElement);
            }
            else if (obj == null)
            {
                insertBeforeGroupDM = listContainerViewModel.SelectedGroupListViewItem;
                dialogShowLocationRelativeToScreen = GroupItemAppropriateShowDialogLocation(listContainerViewModel.AddGroupButton);
            }

            // 弹出添加窗口
            var newDataGroup = new SubscribeDataGroup(SubscribeDataGroupType.Custom, UUIDHelper.CreateUUIDString(false));
            var dialogCtrl = dataGroupEditDialogCtrlFatory.CreateExport().Value;
            dialogCtrl.DialogOwner = listContainerViewModel.DisplayInWindow;
            dialogCtrl.DialogTitle = "添加分组";
            dialogCtrl.DialogShowLocationRelativeToScreen = dialogShowLocationRelativeToScreen;
            dialogCtrl.InitialEditGroup = newDataGroup;

            dialogCtrl.Initialize();
            dialogCtrl.Run();
            dialogCtrl.Shutdown();

            if (dialogCtrl.EditConfirmResult == true)
            {
                CollectionChangedEventManager.RemoveHandler(listContainerViewModel.DataGroupListViews, DataGroupCollectionChanged);

                int insertLocation = 0;
                if (insertBeforeGroupDM != null)
                {
                    insertLocation = GetInsertNewDataGroupIndex(insertBeforeGroupDM);
                }
                var newItem = new SubscribeDataGroupListViewDM(newDataGroup);
                listContainerViewModel.DataGroupListViews.Insert(insertLocation, newItem);
                listContainerViewModel.Scroll2GroupItem(newItem);
                UpdateDataRootComposeGroups(true);

                CollectionChangedEventManager.AddHandler(listContainerViewModel.DataGroupListViews, DataGroupCollectionChanged);
            }
        }

        private bool CanNewGroupTab(object obj)
        {
            if (VerifyCommandParameter(obj, out SubscribeDataGroupListViewDM dataGroupListViewDM)
                || obj == null)
            {
                return true;
            }
            return false;
        }

        private void ToRemoveGroupTab(object obj)
        {
            if (VerifyCommandParameter(obj, out SubscribeDataGroupListViewDM dataGroupListViewDM)
                && dataGroupListViewDM.Group.GroupType == SubscribeDataGroupType.Custom)
            {
                var itemUIElement = listContainerViewModel.GroupItemElement(dataGroupListViewDM);
                var qResult = messageWindowService.ShowQuestionDialog(listContainerViewModel.DisplayInWindow,
                    GroupItemAppropriateShowDialogLocation(itemUIElement),
                    null, null,
                    "确定要删除该分组吗？", false, "删除", "取消");
                if (qResult != true) return;

                CollectionChangedEventManager.RemoveHandler(listContainerViewModel.DataGroupListViews, DataGroupCollectionChanged);
                // 从列表中删除
                if (listContainerViewModel.DataGroupListViews.Remove(dataGroupListViewDM))
                {
                    UpdateDataRootComposeGroups(true);
                }

                CollectionChangedEventManager.AddHandler(listContainerViewModel.DataGroupListViews, DataGroupCollectionChanged);
            }
        }

        private bool CanRemoveGroupTab(object obj)
        {
            if (VerifyCommandParameter(obj, out SubscribeDataGroupListViewDM dataGroupListViewDM))
            {
                return (dataGroupListViewDM.Group.GroupType == SubscribeDataGroupType.Custom);
            }
            return false;
        }

        private void ToRenameGroupTab(object obj)
        {
            if (VerifyCommandParameter(obj, out SubscribeDataGroupListViewDM dataGroupListViewDM)
                && dataGroupListViewDM.Group.GroupType == SubscribeDataGroupType.Custom)
            {
                // 弹出重命名窗口
                var dialogCtrl = dataGroupEditDialogCtrlFatory.CreateExport().Value;
                dialogCtrl.DialogOwner = listContainerViewModel.DisplayInWindow;
                dialogCtrl.DialogTitle = "编辑分组";
                var itemUIElement = listContainerViewModel.GroupItemElement(dataGroupListViewDM);
                dialogCtrl.DialogShowLocationRelativeToScreen = GroupItemAppropriateShowDialogLocation(itemUIElement);
                dialogCtrl.InitialEditGroup = dataGroupListViewDM.Group;

                dialogCtrl.Initialize();
                dialogCtrl.Run();
                dialogCtrl.Shutdown();
            }
        }

        private bool CanRenameGroupTab(object obj)
        {
            if (VerifyCommandParameter(obj, out SubscribeDataGroupListViewDM dataGroupListViewDM))
            {
                return (dataGroupListViewDM.Group.GroupType == SubscribeDataGroupType.Custom);
            }
            return false;
        }

        private Point? GroupItemAppropriateShowDialogLocation(UIElement referUIElement)
        {
            if (referUIElement == null) return null;
            var itemUIElementSize = referUIElement.RenderSize;
            var screenPoint = referUIElement.PointToScreen(new Point(itemUIElementSize.Width / 2, itemUIElementSize.Height / 2));
            var location = UIHelper.TransformToWpfPoint(screenPoint, referUIElement);
            return location;
        }
    }
}
