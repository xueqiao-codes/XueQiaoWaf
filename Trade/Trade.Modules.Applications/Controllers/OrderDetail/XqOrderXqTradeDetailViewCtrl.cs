using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.ViewModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers
{
    /// <summary>
    /// 订单的雪橇成交详情页 controller
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class XqOrderXqTradeDetailViewCtrl : IController
    {
        private readonly XqOrderXqTradeDetailVM contentViewModel;
        private readonly ExportFactory<XqTradeExecTradesDialogCtrl> execTradesDialogCtrlFactory;
        
        private readonly DelegateCommand showXqTradeExecTradesCmd;
        
        [ImportingConstructor]
        public XqOrderXqTradeDetailViewCtrl(XqOrderXqTradeDetailVM contentViewModel,
            ExportFactory<XqTradeExecTradesDialogCtrl> execTradesDialogCtrlFactory)
        {
            this.contentViewModel = contentViewModel;
            this.execTradesDialogCtrlFactory = execTradesDialogCtrlFactory;

            showXqTradeExecTradesCmd = new DelegateCommand(ShowXqTradeExecTrades);
        }

        public ClientXQOrderTargetType OrderTargetType { get; set; }

        /// <summary>
        /// 雪橇成交详情项获取方法
        /// </summary>
        public Func<XqOrderXqTradeDetailViewCtrl, IEnumerable<XqTradeDetailDM>> XqTradeDetailItemsFactory { get; set; }
        
        /// <summary>
        /// 雪橇组合标的订单的待成交项获取方法
        /// </summary>
        public Func<XqOrderXqTradeDetailViewCtrl, XqTradeDetailDM> XqComposeWaitTradeItemFactory { get; set; }

        /// <summary>
        /// 某个雪橇成交的执行成交列表获取方法
        /// </summary>
        public Func<XqOrderXqTradeDetailViewCtrl, XqTradeDetailDM, IEnumerable<ExecTradeDM>> XqTradeExecTradeItemsFactory { get; set; }

        public object ContentView => contentViewModel.View;

        public void Initialize()
        {
            contentViewModel.ShowXqTradeExecTradesCmd = showXqTradeExecTradesCmd;
        }

        public void Run()
        {
            contentViewModel.InvalidateViewWithOrderTargetType(this.OrderTargetType);
            InvalidateView();
        }

        public void Shutdown()
        {
            this.XqTradeDetailItemsFactory = null;
            this.XqComposeWaitTradeItemFactory = null;
            this.XqTradeExecTradeItemsFactory = null;
        }

        /// <summary>
        /// 刷新页面
        /// </summary>
        public void InvalidateView()
        {
            var xqTradeDetailItems = XqTradeDetailItemsFactory?.Invoke(this);

            contentViewModel.XqTradeDetailItems.Clear();
            contentViewModel.XqTradeDetailItems.AddRange(xqTradeDetailItems);

            contentViewModel.XqComposeWaitTradeItems.Clear();
            if (this.OrderTargetType == ClientXQOrderTargetType.COMPOSE_TARGET)
            {
                var xqComposeWaitTradeItem = XqComposeWaitTradeItemFactory?.Invoke(this);
                if (xqComposeWaitTradeItem != null)
                {
                    contentViewModel.XqComposeWaitTradeItems.Add(xqComposeWaitTradeItem);
                }
            }
        }

        private void ShowXqTradeExecTrades(object obj)
        {
            var xqTradeItem = obj as XqTradeDetailDM;
            if (xqTradeItem == null) return;

            var execTradeItems = XqTradeExecTradeItemsFactory?.Invoke(this, xqTradeItem);

            var dialogCtrl = execTradesDialogCtrlFactory.CreateExport().Value;
            dialogCtrl.DialogOwner = UIHelper.GetWindowOfUIElement(contentViewModel.View);
            dialogCtrl.XqTargetType = this.OrderTargetType;
            dialogCtrl.ExecTradeItems = execTradeItems;

            dialogCtrl.Initialize();
            dialogCtrl.Run();
            dialogCtrl.Shutdown();
        }
    }
}
