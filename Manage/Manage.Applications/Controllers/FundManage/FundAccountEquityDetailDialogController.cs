using Manage.Applications.DataModels;
using Manage.Applications.ViewModels;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using xueqiao.trade.hosting.proxy;
using xueqiao.trade.hosting.terminal.ao;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;

namespace Manage.Applications.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class FundAccountEquityDetailDialogController : IController
    {
        private readonly IMessageWindowService messageWindowService;
        private readonly FundAccountEquityDetailViewModel contentViewModel;
        private readonly IEventAggregator eventAggregator;
        private readonly Lazy<ILoginUserManageService> loginUserManageService;

        private IMessageWindow detailDialog;

        [ImportingConstructor]
        public FundAccountEquityDetailDialogController(IMessageWindowService messageWindowService,
            FundAccountEquityDetailViewModel contentViewModel,
            IEventAggregator eventAggregator,
             Lazy<ILoginUserManageService> loginUserManageService)
        {
            this.messageWindowService = messageWindowService;
            this.contentViewModel = contentViewModel;
            this.eventAggregator = eventAggregator;
            this.loginUserManageService = loginUserManageService;

            loginUserManageService.Value.HasLogouted += RecvUserHasLogouted;
        }

        public object DialogOwner { get; set; }

        public HostingTAFundCurrencyGroup CurrencyEquityItem { get; set; }

        public void Initialize()
        {
            if (CurrencyEquityItem == null) throw new ArgumentNullException("CurrencyEquityItem");

            contentViewModel.Details.Clear();
            contentViewModel.Details.AddRange(CurrencyEquityItem.ItemFunds
                .Select(i => new FundAccountEquityModel(i)).ToArray());
        }

        public void Run()
        {
            var dialogWidth = Math.Min(SystemParameters.VirtualScreenWidth, 1150);
            var dialogHeight = Math.Min(SystemParameters.VirtualScreenHeight, 500);

            var dialogTitle = $"{CurrencyEquityItem.CurrencyNo} 详情";
            this.detailDialog = messageWindowService.CreateContentCustomWindow(DialogOwner, null, new Size(dialogWidth, dialogHeight), true, true,
                true, dialogTitle, contentViewModel.View);
            detailDialog.ShowDialog();
        }

        public void Shutdown()
        {
            if (detailDialog != null)
            {
                detailDialog.Close();
                detailDialog = null;
            }
            loginUserManageService.Value.HasLogouted -= RecvUserHasLogouted;
        }

        private void RecvUserHasLogouted(ProxyLoginResp lastLoginResp)
        {
            Shutdown();
        }
    }
}
