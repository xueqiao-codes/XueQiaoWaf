using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using XueQiaoFoundation.BusinessResources.Constants;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.BusinessResources.Models;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoWaf.Trade.Interfaces.Applications;
using XueQiaoWaf.Trade.Modules.Applications.Controllers.Events;
using XueQiaoWaf.Trade.Modules.Applications.Helper;
using XueQiaoWaf.Trade.Modules.Applications.ViewModels;
using NativeModel.Trade;
using System.Windows;
using XueQiaoFoundation.Shared.Helper;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers
{
    /// <summary>
    /// 成交页面控制器
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class TradeListPageController : IController
    {
        private readonly TradeListPageViewModel pageViewModel;
        private readonly ExportFactory<DisplayColumnsTradeListConfigDialogController> tradeListColumnsConfigDialogCtrlFactory;
        private readonly ExportFactory<IXqOrderDetailDialogCtrl> orderDetailDialogCtrlFactory;
        private readonly ITradeModuleService tradeModuleService;
        private readonly ITradeItemsController tradeItemsController;
        private readonly IEventAggregator eventAggregator;
        private readonly XqTargetOfItemSubscribeQuotationHelpCtrl xqTargetOfItemSubscribeQuotationHelpCtrl;

        private readonly DelegateCommand toShowOrderExecuteDetailCmd;
        private readonly DelegateCommand clickItemTargetKeyRelatedColumnCmd;
        private readonly DelegateCommand toConfigTradeListColumnsCmd;
        private readonly DelegateCommand toApplyDefaultTradeListColumnsCmd;
        
        [ImportingConstructor]
        public TradeListPageController(
            TradeListPageViewModel pageViewModel,
            ExportFactory<DisplayColumnsTradeListConfigDialogController> tradeListColumnsConfigDialogCtrlFactory,
            ExportFactory<IXqOrderDetailDialogCtrl> orderDetailDialogCtrlFactory,
            ITradeModuleService tradeModuleService,
            ITradeItemsController tradeItemsController,
            IEventAggregator eventAggregator,
            XqTargetOfItemSubscribeQuotationHelpCtrl xqTargetOfItemSubscribeQuotationHelpCtrl)
        {
            this.pageViewModel = pageViewModel;            
            this.tradeListColumnsConfigDialogCtrlFactory = tradeListColumnsConfigDialogCtrlFactory;
            this.orderDetailDialogCtrlFactory = orderDetailDialogCtrlFactory;
            this.tradeModuleService = tradeModuleService;
            this.tradeItemsController = tradeItemsController;
            this.eventAggregator = eventAggregator;
            this.xqTargetOfItemSubscribeQuotationHelpCtrl = xqTargetOfItemSubscribeQuotationHelpCtrl;

            toShowOrderExecuteDetailCmd = new DelegateCommand(ToShowOrderExecuteDetail);
            clickItemTargetKeyRelatedColumnCmd = new DelegateCommand(ClickItemTargetKeyRelatedColumn);
            toConfigTradeListColumnsCmd = new DelegateCommand(ToConfigTradeListColumns);
            toApplyDefaultTradeListColumnsCmd = new DelegateCommand(ToApplyDefaultTradeListColumns);
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

            InitializeTradeListComponentDetailIfNeed();

            pageViewModel.ToShowOrderExecuteDetailCmd = toShowOrderExecuteDetailCmd;
            pageViewModel.ClickItemTargetKeyRelatedColumnCmd = clickItemTargetKeyRelatedColumnCmd;
            pageViewModel.ToConfigTradeListColumnsCmd = toConfigTradeListColumnsCmd;
            pageViewModel.ToApplyDefaultTradeListColumnsCmd = toApplyDefaultTradeListColumnsCmd;

            xqTargetOfItemSubscribeQuotationHelpCtrl.XqTargetOfItemParser = obj =>
            {
                var item = obj as TradeItemDataModel;
                if (item == null) return null;
                return new XqTargetInfo { TargetType = item.TargetType, TargetKey = item.TargetKey };
            };
            xqTargetOfItemSubscribeQuotationHelpCtrl.MsgDisplayWindowOwnerGetter = () => UIHelper.GetWindowOfUIElement(pageViewModel.View);
            xqTargetOfItemSubscribeQuotationHelpCtrl.Initialize();
            pageViewModel.SubscribeTargetQuotationCmd = xqTargetOfItemSubscribeQuotationHelpCtrl.SubscribeTargetQuotationCmd;


            // configure trade list initial display columns
            var initialColumns = Component.AccountContainerComponentDetail.TradeListComponentDetail.TradeListColumns;
            ApplyTradeListDisplayColumnsIfNeed(initialColumns);

            pageViewModel.UpdatePresentSubAccountId(ParentWorkspace.SubAccountId);

            eventAggregator.GetEvent<GlobalApplyTradeListDisplayColumnsEvent>().Subscribe(ReceiveGlobalApplyTradeListDisplayColumnsEvent);
            PropertyChangedEventManager.AddHandler(ParentWorkspace, ParentWorkspacePropertyChanged, "");
        }

        public void Run()
        {

        }

        public void Shutdown()
        {
            xqTargetOfItemSubscribeQuotationHelpCtrl.Shutdown();
            eventAggregator.GetEvent<GlobalApplyTradeListDisplayColumnsEvent>().Unsubscribe(ReceiveGlobalApplyTradeListDisplayColumnsEvent);
            PropertyChangedEventManager.RemoveHandler(ParentWorkspace, ParentWorkspacePropertyChanged, "");
        }

        private void InitializeTradeListComponentDetailIfNeed()
        {
            var tradeListComponentDetail = Component.AccountContainerComponentDetail.TradeListComponentDetail;
            if (tradeListComponentDetail == null)
            {
                tradeListComponentDetail = new TradeListComponentDetail();
                Component.AccountContainerComponentDetail.TradeListComponentDetail = tradeListComponentDetail;
            }

            if (tradeListComponentDetail.TradeListColumns?.Any() != true)
            {
                var listInitialColumns = tradeModuleService.TradeWorkspaceDataRoot?.GlobalAppliedTradeListDisplayColumns?.Select(i => i.ColumnCode).ToArray();
                if (listInitialColumns == null)
                {
                    listInitialColumns = TradeWorkspaceDataDisplayHelper.DefaultTradeListDisplayColumns.Select(i => i.GetHashCode()).ToArray();
                }
                tradeListComponentDetail.TradeListColumns = listInitialColumns.Select(i => new ListColumnInfo { ColumnCode = i, ContentAlignment = XueQiaoConstants.ListColumnContentAlignment_Left }).ToArray();
            }
            // 消除重复列
            var listColumnGroups = tradeListComponentDetail.TradeListColumns.GroupBy(i => i.ColumnCode);
            tradeListComponentDetail.TradeListColumns = listColumnGroups.Select(i => i.First()).ToArray();
        }

        private void ReceiveGlobalApplyTradeListDisplayColumnsEvent(IEnumerable<ListColumnInfo> msg)
        {
            ApplyTradeListDisplayColumnsIfNeed(msg);
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
            var item = obj as TradeItemDataModel;
            if (item == null) return;

            var dialogCtrl = orderDetailDialogCtrlFactory.CreateExport().Value;
            dialogCtrl.DialogOwner = pageViewModel.DisplayInWindow;
            dialogCtrl.XqOrderId = item.OrderId;

            dialogCtrl.Initialize();
            dialogCtrl.Run();
            dialogCtrl.Shutdown();
        }

        private void ClickItemTargetKeyRelatedColumn(object obj)
        {
            var item = obj as TradeItemDataModel;
            if (item == null) return;

            // 联动
            var associateArgs = new TradeComponentXqTargetAssociateArgs(ParentComponentCtrl.ParentWorkspace, ParentComponentCtrl.Component,
                item.TargetType, item.TargetKey);

            var previewAssociateDialogOwner = pageViewModel.DisplayInWindow;
            Point? previewAssociateDialogLocation = null;
            ParentComponentCtrl.XqTargetAssociateHandler?.HandleXqTargetAssociate(previewAssociateDialogOwner, previewAssociateDialogLocation, associateArgs);
        }

        private void ToConfigTradeListColumns()
        {
            var dialogCtrl = tradeListColumnsConfigDialogCtrlFactory.CreateExport().Value;
            dialogCtrl.DialogOwner = pageViewModel.DisplayInWindow;
            dialogCtrl.OriginDisplayingColumnInfos = pageViewModel.ListDisplayColumnInfos?
                .Select(i=>i.Clone() as ListColumnInfo).ToArray();
            dialogCtrl.Initialize();
            dialogCtrl.Run();
            dialogCtrl.Shutdown();
            if (dialogCtrl.ConfiguredDisplayColomunsResult != null)
            {
                ApplyTradeListDisplayColumnsIfNeed(dialogCtrl.ConfiguredDisplayColomunsResult);
            }
        }

        private void ToApplyDefaultTradeListColumns()
        {
            var defaultDisplayColumnCodes = TradeWorkspaceDataDisplayHelper.DefaultTradeListDisplayColumns.Select(i => i.GetHashCode()).ToArray();
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

            ApplyTradeListDisplayColumnsIfNeed(resetItems.ToArray());
        }

        private void ApplyTradeListDisplayColumnsIfNeed(IEnumerable<ListColumnInfo> displayColumnInfos)
        {
            var currentDisplayColumnInfos = pageViewModel.ListDisplayColumnInfos ?? new ListColumnInfo[] { };
            if (displayColumnInfos?.SequenceEqual(currentDisplayColumnInfos) != true)
            {
                Component.AccountContainerComponentDetail.TradeListComponentDetail.TradeListColumns
                    = displayColumnInfos.ToArray();
                pageViewModel.ResetListDisplayColumns(displayColumnInfos);
            }
        }
    }
}
