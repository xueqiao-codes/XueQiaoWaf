using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows;
using XueQiaoFoundation.BusinessResources.Constants;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.BusinessResources.Models;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoWaf.Trade.Modules.Applications.Helper;
using XueQiaoWaf.Trade.Modules.Applications.ViewModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers
{
    /// <summary>
    /// 委托单历史列表内容 controller
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class OrderHistoryParkedListContentController : IOrderHistoryListContentController
    {
        private readonly OrderHistoryParkedListVM listViewModel;
        private readonly ExportFactory<RelatedOrderDialogCtrl> relatedOrderDialogCtrlFactory;

        private readonly DelegateCommand toShowChildOrderCmd;
        private readonly DelegateCommand clickItemTargetKeyRelatedColumnCmd;

        [ImportingConstructor]
        public OrderHistoryParkedListContentController(OrderHistoryParkedListVM listViewModel,
            ExportFactory<RelatedOrderDialogCtrl> relatedOrderDialogCtrlFactory)
        {
            this.listViewModel = listViewModel;
            this.relatedOrderDialogCtrlFactory = relatedOrderDialogCtrlFactory;

            toShowChildOrderCmd = new DelegateCommand(ToShowChildOrder);
            clickItemTargetKeyRelatedColumnCmd = new DelegateCommand(ClickItemTargetKeyRelatedColumn);
        }

        public object ListContentView => listViewModel.View;

        public Func<IOrderHistoryListContentController, IEnumerable<OrderItemDataModel>> OrderHistoryListFactory { get; set; }

        public ITradeComponentController ParentComponentCtrl { get; set; }

        public void InvalidateOrderHistoryList()
        {
            var historyList = OrderHistoryListFactory?.Invoke(this)?.OfType<OrderItemDataModel_Parked>().ToArray();
            listViewModel.OrderList.Clear();
            listViewModel.OrderList.AddRange(historyList);
        }

        public void Initialize()
        {
            listViewModel.ToShowChildOrderCmd = toShowChildOrderCmd;
            listViewModel.ClickItemTargetKeyRelatedColumnCmd = clickItemTargetKeyRelatedColumnCmd;

            var initialColumnInfos = TradeWorkspaceDataDisplayHelper.DefaultOrderListParkedDisplayColumns
                .Select(i => new ListColumnInfo
                 {
                    ColumnCode = i.GetHashCode(),
                    ContentAlignment = XueQiaoConstants.ListColumnContentAlignment_Left
                }).ToArray();
            listViewModel.ResetListDisplayColumns(initialColumnInfos);
        }
        
        public void Run()
        {

        }

        public void Shutdown()
        {
            OrderHistoryListFactory = null;
        }

        private void ToShowChildOrder(object obj)
        {
            var orderItem = obj as OrderItemDataModel_Parked;
            if (orderItem == null) return;

            var dialogCtrl = relatedOrderDialogCtrlFactory.CreateExport().Value;
            dialogCtrl.DialogOwner = listViewModel.DisplayInWindow;
            dialogCtrl.CurrentOrder = orderItem;

            dialogCtrl.Initialize();
            dialogCtrl.Run();
            dialogCtrl.Shutdown();
        }
        
        private void ClickItemTargetKeyRelatedColumn(object obj)
        {
            var item = obj as OrderItemDataModel;
            if (item == null) return;

            // 联动
            var associateArgs = new TradeComponentXqTargetAssociateArgs(ParentComponentCtrl.ParentWorkspace, ParentComponentCtrl.Component,
                item.TargetType, item.TargetKey);

            var previewAssociateDialogOwner = listViewModel.DisplayInWindow;
            Point? previewAssociateDialogLocation = null;
            ParentComponentCtrl.XqTargetAssociateHandler?.HandleXqTargetAssociate(previewAssociateDialogOwner, previewAssociateDialogLocation, associateArgs);
        }
    }
}
