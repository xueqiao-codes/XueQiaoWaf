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
using XueQiaoWaf.Trade.Interfaces.Applications;
using XueQiaoWaf.Trade.Modules.Applications.Helper;
using XueQiaoWaf.Trade.Modules.Applications.ViewModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers
{
    /// <summary>
    /// 委托单历史列表内容 controller
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class OrderHistoryEntrustedListContentController : IOrderHistoryListContentController
    {
        private readonly OrderHistoryEntrustedListVM listViewModel;
        private readonly ExportFactory<IXqOrderDetailDialogCtrl> orderDetailDialogCtrlFactory;
        private readonly ExportFactory<RelatedOrderDialogCtrl> relatedOrderDialogCtrlFactory;

        private readonly DelegateCommand toShowOrderExecuteDetailCmd;
        private readonly DelegateCommand toShowParentOrderCmd;
        private readonly DelegateCommand clickItemTargetKeyRelatedColumnCmd;

        [ImportingConstructor]
        public OrderHistoryEntrustedListContentController(OrderHistoryEntrustedListVM listViewModel,
            ExportFactory<IXqOrderDetailDialogCtrl> orderDetailDialogCtrlFactory,
            ExportFactory<RelatedOrderDialogCtrl> relatedOrderDialogCtrlFactory)
        {
            this.listViewModel = listViewModel;
            this.orderDetailDialogCtrlFactory = orderDetailDialogCtrlFactory;
            this.relatedOrderDialogCtrlFactory = relatedOrderDialogCtrlFactory;

            toShowOrderExecuteDetailCmd = new DelegateCommand(ToShowOrderExecuteDetail);
            toShowParentOrderCmd = new DelegateCommand(ToShowParentOrder);
            clickItemTargetKeyRelatedColumnCmd = new DelegateCommand(ClickItemTargetKeyRelatedColumn);
        }

        public object ListContentView => listViewModel.View;

        public Func<IOrderHistoryListContentController, IEnumerable<OrderItemDataModel>> OrderHistoryListFactory { get; set; }

        public ITradeComponentController ParentComponentCtrl { get; set; }

        public void InvalidateOrderHistoryList()
        {
            var historyList = OrderHistoryListFactory?.Invoke(this)?.OfType<OrderItemDataModel_Entrusted>().ToArray();
            listViewModel.OrderList.Clear();
            listViewModel.OrderList.AddRange(historyList);
        }

        public void Initialize()
        {
            if (ParentComponentCtrl == null) throw new ArgumentNullException("ParentComponentCtrl");

            listViewModel.ToShowOrderExecuteDetailCmd = toShowOrderExecuteDetailCmd;
            listViewModel.ToShowParentOrderCmd = toShowParentOrderCmd;
            listViewModel.ClickItemTargetKeyRelatedColumnCmd = clickItemTargetKeyRelatedColumnCmd;

            var initialColumnInfos = TradeWorkspaceDataDisplayHelper.DefaultOrderListEntrustedDisplayColumns
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
            listViewModel.Shutdown();
        }
        
        private void ToShowOrderExecuteDetail(object obj)
        {
            var orderItem = obj as OrderItemDataModel_Entrusted;
            if (orderItem == null) return;

            var dialogCtrl = orderDetailDialogCtrlFactory.CreateExport().Value;
            dialogCtrl.DialogOwner = UIHelper.GetWindowOfUIElement(listViewModel.View);
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
            dialogCtrl.DialogOwner = UIHelper.GetWindowOfUIElement(listViewModel.View);
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

            var previewAssociateDialogOwner = UIHelper.GetWindowOfUIElement(listViewModel.View);
            Point? previewAssociateDialogLocation = null;
            ParentComponentCtrl.XqTargetAssociateHandler?.HandleXqTargetAssociate(previewAssociateDialogOwner, previewAssociateDialogLocation, associateArgs);
        }
    }
}
