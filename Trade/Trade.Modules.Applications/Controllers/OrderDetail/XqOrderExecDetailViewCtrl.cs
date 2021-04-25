using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.ViewModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class XqOrderExecDetailViewCtrl : IController
    {
        private readonly XqOrderExecDetailVM contentViewModel;
        private readonly XqOrderExecTradeListVM execTradeListVM;

        [ImportingConstructor]
        public XqOrderExecDetailViewCtrl(XqOrderExecDetailVM contentViewModel,
            XqOrderExecTradeListVM execTradeListVM)
        {
            this.contentViewModel = contentViewModel;
            this.execTradeListVM = execTradeListVM;
        }

        public ClientXQOrderTargetType OrderTargetType { get; set; }

        /// <summary>
        /// 执行订单项获取方法
        /// </summary>
        public Func<XqOrderExecDetailViewCtrl, IEnumerable<ExecOrderDM>> ExecOrderItemsFactory { get; set; }

        /// <summary>
        /// 执行成交项获取方法
        /// </summary>
        public Func<XqOrderExecDetailViewCtrl, IEnumerable<ExecTradeDM>> ExecTradeItemsFactory { get; set; }

        public object ContentView => contentViewModel.View;

        public void Initialize()
        {
        }

        public void Run()
        {
            contentViewModel.ExecTradeItemsView = execTradeListVM.View;
            contentViewModel.InvalidateViewWithOrderTargetType(this.OrderTargetType);
            execTradeListVM.InvalidateViewWithOrderTargetType(this.OrderTargetType);

            InvalidateView();
        }

        public void Shutdown()
        {
        }

        /// <summary>
        /// 刷新页面
        /// </summary>
        public void InvalidateView()
        {
            var execOrderItems = ExecOrderItemsFactory?.Invoke(this);
            contentViewModel.ExecOrderItems.Clear();
            contentViewModel.ExecOrderItems.AddRange(execOrderItems?.ToArray());

            var exeTradeItems = ExecTradeItemsFactory?.Invoke(this);
            execTradeListVM.ExecTradeItems.Clear();
            execTradeListVM.ExecTradeItems.AddRange(exeTradeItems?.ToArray());
        }
    }
}
