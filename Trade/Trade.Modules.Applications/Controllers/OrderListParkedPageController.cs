using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using XueQiaoFoundation.BusinessResources.Models;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoWaf.Trade.Modules.Applications.Controllers.Events;
using XueQiaoWaf.Trade.Modules.Applications.Helper;
using XueQiaoWaf.Trade.Modules.Applications.ViewModels;
using XueQiaoWaf.Trade.Interfaces.Applications;
using System.Windows;
using XueQiaoFoundation.BusinessResources.Constants;
using XueQiaoFoundation.Shared.Helper;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class OrderListParkedPageController : IController
    {
        private readonly IEventAggregator eventAggregator;
        private readonly ExportFactory<DisplayColumnsOrderListParkedConfigDialogController> orderListColumnsConfigDialogCtrlFactory;
        private readonly OrderListParkedPageViewModel pageViewModel;
        private readonly ISelectedOrdersOperateCommandsCtrl selectedOrdersOperateCommandsCtrl;
        private readonly ExportFactory<RelatedOrderDialogCtrl> relatedOrderDialogCtrlFactory;
        private readonly ITradeModuleService tradeModuleService;
        private readonly XqTargetOfItemSubscribeQuotationHelpCtrl xqTargetOfItemSubscribeQuotationHelpCtrl;

        private readonly DelegateCommand toShowChildOrderCmd;
        private readonly DelegateCommand toConfigOrderListColumnsCmd;
        private readonly DelegateCommand toApplyDefaultOrderListColumnsCmd;
        private readonly DelegateCommand clickItemTargetKeyRelatedColumnCmd;
        
        [ImportingConstructor]
        public OrderListParkedPageController(IEventAggregator eventAggregator,
            ExportFactory<DisplayColumnsOrderListParkedConfigDialogController> orderListColumnsConfigDialogCtrlFactory,
            OrderListParkedPageViewModel pageViewModel,
            ISelectedOrdersOperateCommandsCtrl selectedOrdersOperateCommandsCtrl,
            ExportFactory<RelatedOrderDialogCtrl> relatedOrderDialogCtrlFactory,
            ITradeModuleService tradeModuleService,
            XqTargetOfItemSubscribeQuotationHelpCtrl xqTargetOfItemSubscribeQuotationHelpCtrl)
        {
            this.eventAggregator = eventAggregator;
            this.orderListColumnsConfigDialogCtrlFactory = orderListColumnsConfigDialogCtrlFactory;
            this.pageViewModel = pageViewModel;
            this.selectedOrdersOperateCommandsCtrl = selectedOrdersOperateCommandsCtrl;
            this.relatedOrderDialogCtrlFactory = relatedOrderDialogCtrlFactory;
            this.tradeModuleService = tradeModuleService;
            this.xqTargetOfItemSubscribeQuotationHelpCtrl = xqTargetOfItemSubscribeQuotationHelpCtrl;

            toShowChildOrderCmd = new DelegateCommand(ToShowChildOrder);
            toConfigOrderListColumnsCmd = new DelegateCommand(ToConfigOrderListColumns);
            toApplyDefaultOrderListColumnsCmd = new DelegateCommand(ToApplyDefaultOrderListColumns);
            clickItemTargetKeyRelatedColumnCmd = new DelegateCommand(ClickItemTargetKeyRelatedColumn);
        }

        public XueQiaoFoundation.BusinessResources.DataModels.TabWorkspace ParentWorkspace { get; set; }
        public XueQiaoFoundation.BusinessResources.DataModels.TradeComponent Component { get; set; }
        public ITradeComponentController ParentComponentCtrl { get; set; }

        public object ComponentContentView => pageViewModel.View;

        public void Initialize()
        {
            if (ParentWorkspace == null) throw new ArgumentNullException("ParentWorkspace");
            if (Component == null) throw new ArgumentNullException("Component");
            if (Component.AccountContainerComponentDetail == null) throw new ArgumentNullException("Component.AccountContainerComponentDetail");

            selectedOrdersOperateCommandsCtrl.WindowOwnerGetter = () => UIHelper.GetWindowOfUIElement(pageViewModel.View);
            selectedOrdersOperateCommandsCtrl.Initialize();

            InitializeOrderListComponentDetailIfNeed();
            
            pageViewModel.OrderListDataContext.SelectedOrdersOptCommands = selectedOrdersOperateCommandsCtrl.SelectedOrdersOptCommands;

            pageViewModel.ToShowChildOrderCmd = toShowChildOrderCmd;
            pageViewModel.ToConfigOrderListColumnsCmd = toConfigOrderListColumnsCmd;
            pageViewModel.ToApplyDefaultOrderListColumnsCmd = toApplyDefaultOrderListColumnsCmd;
            pageViewModel.ClickItemTargetKeyRelatedColumnCmd = clickItemTargetKeyRelatedColumnCmd;

            xqTargetOfItemSubscribeQuotationHelpCtrl.XqTargetOfItemParser = obj =>
            {
                var item = obj as OrderItemDataModel;
                if (item == null) return null;
                return new XqTargetInfo { TargetType = item.TargetType, TargetKey = item.TargetKey };
            };
            xqTargetOfItemSubscribeQuotationHelpCtrl.MsgDisplayWindowOwnerGetter = () => UIHelper.GetWindowOfUIElement(pageViewModel.View);
            xqTargetOfItemSubscribeQuotationHelpCtrl.Initialize();
            pageViewModel.SubscribeTargetQuotationCmd = xqTargetOfItemSubscribeQuotationHelpCtrl.SubscribeTargetQuotationCmd;


            // configure order list initial display columns
            var listInitialColumns = Component.AccountContainerComponentDetail.ParkedOrderListComponentDetail.OrderListColumns;
            ApplyOrderListDisplayColumnsIfNeed(listInitialColumns);

            pageViewModel.UpdatePresentSubAccountId(ParentWorkspace.SubAccountId);
            
            eventAggregator.GetEvent<GlobalApplyOrderListParkedColumnsEvent>().Subscribe(ReceiveGlobalApplyOrderListDisplayColumnsEvent);
            PropertyChangedEventManager.AddHandler(ParentWorkspace, ParentWorkspacePropertyChanged, "");
        }

        public void Run()
        {

        }

        public void Shutdown()
        {
            xqTargetOfItemSubscribeQuotationHelpCtrl.Shutdown();
            selectedOrdersOperateCommandsCtrl.Shutdown();
            eventAggregator.GetEvent<GlobalApplyOrderListParkedColumnsEvent>().Unsubscribe(ReceiveGlobalApplyOrderListDisplayColumnsEvent);
            PropertyChangedEventManager.RemoveHandler(ParentWorkspace, ParentWorkspacePropertyChanged, "");
        }

        private void InitializeOrderListComponentDetailIfNeed()
        {
            var orderListComponentDetail = Component.AccountContainerComponentDetail.ParkedOrderListComponentDetail;
            if (orderListComponentDetail == null)
            {
                orderListComponentDetail = new ParkedOrderListComponentDetail();
                Component.AccountContainerComponentDetail.ParkedOrderListComponentDetail = orderListComponentDetail;
            }

            if (orderListComponentDetail.OrderListColumns?.Any() != true)
            {
                var listInitialColumns = tradeModuleService.TradeWorkspaceDataRoot?.GlobalAppliedOrderListParkedDisplayColumns?.Select(i => i.ColumnCode).ToArray();
                if (listInitialColumns == null)
                {
                    listInitialColumns = TradeWorkspaceDataDisplayHelper.DefaultOrderListParkedDisplayColumns.Select(i => i.GetHashCode()).ToArray();
                }
                orderListComponentDetail.OrderListColumns = listInitialColumns
                    .Select(i => new ListColumnInfo
                    {
                        ColumnCode = i,
                        ContentAlignment = XueQiaoConstants.ListColumnContentAlignment_Left
                    }).ToArray();
            }
            // 消除重复列
            var listColumnGroups = orderListComponentDetail.OrderListColumns.GroupBy(i => i.ColumnCode);
            orderListComponentDetail.OrderListColumns = listColumnGroups.Select(i => i.First()).ToArray();
        }

        private void ReceiveGlobalApplyOrderListDisplayColumnsEvent(IEnumerable<ListColumnInfo> msg)
        {
            ApplyOrderListDisplayColumnsIfNeed(msg);
        }

        private void ParentWorkspacePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(XueQiaoFoundation.BusinessResources.DataModels.TabWorkspace.SubAccountId))
            {
                pageViewModel.UpdatePresentSubAccountId(ParentWorkspace.SubAccountId);
            }
        }
        
        private void ToShowChildOrder(object obj)
        {
            var orderItem = obj as OrderItemDataModel_Parked;
            if (orderItem == null) return;

            var dialogCtrl = relatedOrderDialogCtrlFactory.CreateExport().Value;
            dialogCtrl.DialogOwner = UIHelper.GetWindowOfUIElement(pageViewModel.View);
            dialogCtrl.CurrentOrder = orderItem;

            dialogCtrl.Initialize();
            dialogCtrl.Run();
            dialogCtrl.Shutdown();
        }

        private void ToConfigOrderListColumns()
        {
            var dialogCtrl = orderListColumnsConfigDialogCtrlFactory.CreateExport().Value;
            dialogCtrl.DialogOwner = UIHelper.GetWindowOfUIElement(pageViewModel.View);
            dialogCtrl.OriginDisplayingColumnInfos = pageViewModel.ListDisplayColumnInfos?
                .Select(i => i.Clone() as ListColumnInfo).ToArray();
            dialogCtrl.Initialize();
            dialogCtrl.Run();
            dialogCtrl.Shutdown();
            if (dialogCtrl.ConfiguredDisplayColomunsResult != null)
            {
                ApplyOrderListDisplayColumnsIfNeed(dialogCtrl.ConfiguredDisplayColomunsResult);
            }
        }

        private void ToApplyDefaultOrderListColumns()
        {
            var defaultDisplayColumnCodes = TradeWorkspaceDataDisplayHelper.DefaultOrderListParkedDisplayColumns.Select(i => i.GetHashCode()).ToArray();
            var currentDisplayColumnInfos = pageViewModel.ListDisplayColumnInfos?.ToArray() ?? new ListColumnInfo[] { };

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

            ApplyOrderListDisplayColumnsIfNeed(resetItems.ToArray());
        }

        private void ApplyOrderListDisplayColumnsIfNeed(IEnumerable<ListColumnInfo> displayColumnInfos)
        {
            var currentDisplayColumnInfos = this.pageViewModel.ListDisplayColumnInfos ?? new ListColumnInfo[] { };
            if (displayColumnInfos?.SequenceEqual(currentDisplayColumnInfos) != true)
            {
                Component.AccountContainerComponentDetail.ParkedOrderListComponentDetail.OrderListColumns
                    = displayColumnInfos.ToArray();
                this.pageViewModel.ResetListDisplayColumns(displayColumnInfos);
            }
        }

        private void ClickItemTargetKeyRelatedColumn(object obj)
        {
            var item = obj as OrderItemDataModel;
            if (item == null) return;

            // 联动
            var associateArgs = new TradeComponentXqTargetAssociateArgs(ParentComponentCtrl.ParentWorkspace, ParentComponentCtrl.Component,
                item.TargetType, item.TargetKey);

            var previewAssociateDialogOwner = UIHelper.GetWindowOfUIElement(pageViewModel.View);
            Point? previewAssociateDialogLocation = null;
            ParentComponentCtrl.XqTargetAssociateHandler?.HandleXqTargetAssociate(previewAssociateDialogOwner, previewAssociateDialogLocation, associateArgs);
        }
    }
}
