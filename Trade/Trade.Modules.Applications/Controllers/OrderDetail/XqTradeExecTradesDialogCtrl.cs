using NativeModel.Trade;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using xueqiao.trade.hosting.proxy;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.ViewModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers
{
    /// <summary>
    /// 雪橇成交执行详情弹窗 controller
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class XqTradeExecTradesDialogCtrl : IController
    {
        private readonly XqOrderExecTradeListVM contentViewModel;
        private readonly IMessageWindowService messageWindowService;
        private readonly Lazy<ILoginUserManageService> loginUserManageService;
        private readonly IEventAggregator eventAggregator;

        private IMessageWindow dialog;

        [ImportingConstructor]
        public XqTradeExecTradesDialogCtrl(
            XqOrderExecTradeListVM contentViewModel,
            IMessageWindowService messageWindowService,
            Lazy<ILoginUserManageService> loginUserManageService,
            IEventAggregator eventAggregator)
        {
            this.contentViewModel = contentViewModel;
            this.messageWindowService = messageWindowService;
            this.loginUserManageService = loginUserManageService;
            this.eventAggregator = eventAggregator;

            loginUserManageService.Value.HasLogouted += RecvUserHasLogouted;
        }

        public object DialogOwner { get; set; }

        public ClientXQOrderTargetType XqTargetType { get; set; }
        public IEnumerable<ExecTradeDM> ExecTradeItems { get; set; }

        public void Initialize()
        {
            contentViewModel.InvalidateViewWithOrderTargetType(XqTargetType);
            contentViewModel.ViewMargin = new Thickness(20,20,20,10);
            contentViewModel.ExecTradeItems.AddRange(ExecTradeItems);
        }

        public void Run()
        {
            dialog = messageWindowService.CreateContentCustomWindow(DialogOwner, null, new Size(800, 600), true, true,
                true, "执行成交详情", contentViewModel.View);
            dialog.ShowDialog();
        }

        public void Shutdown()
        {
            if (dialog != null)
            {
                dialog.Close();
                dialog = null;
            }

            loginUserManageService.Value.HasLogouted -= RecvUserHasLogouted;
        }

        private void RecvUserHasLogouted(ProxyLoginResp lastLoginResp)
        {
            Shutdown();
        }
    }
}
