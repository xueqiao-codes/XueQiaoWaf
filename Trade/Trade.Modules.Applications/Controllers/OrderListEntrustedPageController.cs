using NativeModel.Trade;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows;
using XueQiaoFoundation.BusinessResources.Constants;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.BusinessResources.Models;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoWaf.Trade.Interfaces.Applications;
using XueQiaoWaf.Trade.Modules.Applications.Controllers.Events;
using XueQiaoWaf.Trade.Modules.Applications.Helper;
using XueQiaoWaf.Trade.Modules.Applications.ServiceControllers;
using XueQiaoWaf.Trade.Modules.Applications.ViewModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class OrderListEntrustedPageController : IController
    {
        private readonly IEventAggregator eventAggregator;
        private readonly ExportFactory<IXqOrderDetailDialogCtrl> orderDetailDialogCtrlFactory;
        private readonly ExportFactory<DisplayColumnsOrderListEntrustedConfigDialogController> orderListColumnsConfigDialogCtrlFactory;
        private readonly ExportFactory<RelatedOrderDialogCtrl> relatedOrderDialogCtrlFactory;
        private readonly OrderListEntrustedPageViewModel pageViewModel;
        private readonly ISelectedOrdersOperateCommandsCtrl selectedOrdersOperateCommandsCtrl;
        private readonly ITradeModuleService tradeModuleService;
        private readonly XqTargetOfItemSubscribeQuotationHelpCtrl xqTargetOfItemSubscribeQuotationHelpCtrl;

        private readonly DelegateCommand toShowOrderExecuteDetailCmd;
        private readonly DelegateCommand toShowParentOrderCmd;
        private readonly DelegateCommand toConfigOrderListColumnsCmd;
        private readonly DelegateCommand toApplyDefaultOrderListColumnsCmd;
        private readonly DelegateCommand clickItemTargetKeyRelatedColumnCmd;
        
        [ImportingConstructor]
        public OrderListEntrustedPageController(IEventAggregator eventAggregator,
            ExportFactory<IXqOrderDetailDialogCtrl> orderDetailDialogCtrlFactory,
            ExportFactory<DisplayColumnsOrderListEntrustedConfigDialogController> orderListColumnsConfigDialogCtrlFactory,
            ExportFactory<RelatedOrderDialogCtrl> relatedOrderDialogCtrlFactory,
            OrderListEntrustedPageViewModel pageViewModel,
            ISelectedOrdersOperateCommandsCtrl selectedOrdersOperateCommandsCtrl,
            ITradeModuleService tradeModuleService,
            XqTargetOfItemSubscribeQuotationHelpCtrl xqTargetOfItemSubscribeQuotationHelpCtrl)
        {
            this.eventAggregator = eventAggregator;
            this.orderDetailDialogCtrlFactory = orderDetailDialogCtrlFactory;
            this.orderListColumnsConfigDialogCtrlFactory = orderListColumnsConfigDialogCtrlFactory;
            this.relatedOrderDialogCtrlFactory = relatedOrderDialogCtrlFactory;
            this.pageViewModel = pageViewModel;
            this.selectedOrdersOperateCommandsCtrl = selectedOrdersOperateCommandsCtrl;
            this.tradeModuleService = tradeModuleService;
            this.xqTargetOfItemSubscribeQuotationHelpCtrl = xqTargetOfItemSubscribeQuotationHelpCtrl;

            toShowOrderExecuteDetailCmd = new DelegateCommand(ToShowOrderExecuteDetail);
            toShowParentOrderCmd = new DelegateCommand(ToShowParentOrder);
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
            
            pageViewModel.SelectedOrdersOptCommands = selectedOrdersOperateCommandsCtrl.SelectedOrdersOptCommands;

            pageViewModel.ToShowOrderExecuteDetailCmd = toShowOrderExecuteDetailCmd;
            pageViewModel.ToShowParentOrderCmd = toShowParentOrderCmd;
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
            var listInitialColumns = Component.AccountContainerComponentDetail.EntrustedOrderListComponentDetail.OrderListColumns;
            ApplyOrderListDisplayColumnsIfNeed(listInitialColumns);

            pageViewModel.UpdatePresentSubAccountId(ParentWorkspace.SubAccountId);
            
            eventAggregator.GetEvent<GlobalApplyOrderListEntrustedColumnsEvent>().Subscribe(ReceiveGlobalApplyOrderListDisplayColumnsEvent);
            PropertyChangedEventManager.AddHandler(ParentWorkspace, ParentWorkspacePropertyChanged, "");
        }

        public void Run()
        {

        }

        public void Shutdown()
        {
            xqTargetOfItemSubscribeQuotationHelpCtrl.Shutdown();
            selectedOrdersOperateCommandsCtrl.Shutdown();
            pageViewModel.Shutdown();
            eventAggregator.GetEvent<GlobalApplyOrderListEntrustedColumnsEvent>().Unsubscribe(ReceiveGlobalApplyOrderListDisplayColumnsEvent);
            PropertyChangedEventManager.RemoveHandler(ParentWorkspace, ParentWorkspacePropertyChanged, "");
        }

        private void InitializeOrderListComponentDetailIfNeed()
        {
            var orderListComponentDetail = Component.AccountContainerComponentDetail.EntrustedOrderListComponentDetail;
            if (orderListComponentDetail == null)
            {
                orderListComponentDetail = new EntrustedOrderListComponentDetail();
                Component.AccountContainerComponentDetail.EntrustedOrderListComponentDetail = orderListComponentDetail;
            }

            if (orderListComponentDetail.OrderListColumns?.Any() != true)
            {
                var listInitialColumns = tradeModuleService.TradeWorkspaceDataRoot?.GlobalAppliedOrderListEntrustedDisplayColumns?.Select(i => i.ColumnCode).ToArray();
                if (listInitialColumns == null)
                {
                    listInitialColumns = TradeWorkspaceDataDisplayHelper.DefaultOrderListEntrustedDisplayColumns.Select(i => i.GetHashCode()).ToArray();
                }
                orderListComponentDetail.OrderListColumns = listInitialColumns.Select(i => new ListColumnInfo { ColumnCode = i, ContentAlignment = XueQiaoConstants.ListColumnContentAlignment_Left }).ToArray();
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

        private void ToShowOrderExecuteDetail(object obj)
        {
            var orderItem = obj as OrderItemDataModel_Entrusted;
            if (orderItem == null) return;
            
            var dialogCtrl = orderDetailDialogCtrlFactory.CreateExport().Value;
            dialogCtrl.DialogOwner = UIHelper.GetWindowOfUIElement(pageViewModel.View);
            dialogCtrl.XqOrderId = orderItem.OrderId;

            dialogCtrl.Initialize();
            dialogCtrl.Run();
            dialogCtrl.Shutdown();
        }
        
        private void ToShowParentOrder(object obj)
        {
            var orderItem = obj as OrderItemDataModel_Entrusted;
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
                .Select(i=>i.Clone() as ListColumnInfo).ToArray();
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
            var defaultDisplayColumnCodes = TradeWorkspaceDataDisplayHelper.DefaultOrderListEntrustedDisplayColumns.Select(i => i.GetHashCode()).ToArray();
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
                Component.AccountContainerComponentDetail.EntrustedOrderListComponentDetail.OrderListColumns
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
