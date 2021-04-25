using Manage.Applications.ViewModels;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using xueqiao.trade.hosting;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;
using xueqiao.trade.hosting.proxy;

namespace Manage.Applications.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class FundAccountExtraInfoShowDialogController : IController
    {
        private readonly IMessageWindowService messageWindowService;
        private readonly FundAccountExtraInfoViewModel pageViewModel;
        private readonly IEventAggregator eventAggregator;
        private readonly Lazy<ILoginUserManageService> loginUserManageService;

        private readonly DelegateCommand closePageCmd;

        private IMessageWindow detailDialog;

        [ImportingConstructor]
        public FundAccountExtraInfoShowDialogController(IMessageWindowService messageWindowService,
            FundAccountExtraInfoViewModel pageViewModel,
            IEventAggregator eventAggregator,
            Lazy<ILoginUserManageService> loginUserManageService)
        {
            this.messageWindowService = messageWindowService;
            this.pageViewModel = pageViewModel;
            this.eventAggregator = eventAggregator;
            this.loginUserManageService = loginUserManageService;

            this.closePageCmd = new DelegateCommand(ClosePage);
            loginUserManageService.Value.HasLogouted += RecvUserHasLogouted;
        }

        public HostingTradeAccount FundAccount { get; set; }

        public object DialogOwner { get; set; }

        public void Initialize()
        {
            if (FundAccount == null) throw new ArgumentNullException("FundAccount");
            pageViewModel.ClosePageCmd = closePageCmd;
            pageViewModel.ExtraInfoKVs = FundAccount.AccountProperties?
                .OrderBy(i => i.Key)
                .Select(i => new Tuple<string, string>(GetFundAccountExtraInfoKeyHeaderName(i.Key) ?? "", i.Value))
                .ToArray();
        }
        
        public void Run()
        {
            var dialogTitle = "附加信息";
            this.detailDialog = messageWindowService.CreateContentCustomWindow(DialogOwner, null, null, true, false,
                true, dialogTitle, pageViewModel.View);
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

        private string GetFundAccountExtraInfoKeyHeaderName(string extraInfokey)
        {
            if (extraInfokey == trade_hosting_basicConstants.ESUNNY3_APPID_PROPKEY)
                return "APP ID";
            if (extraInfokey == trade_hosting_basicConstants.ESUNNY3_CERTINFO_PROPKEY)
                return "Cert Info";
            if (extraInfokey == trade_hosting_basicConstants.ESUNNY9_AUTHCODE_PROPKEY)
                return "交易授权码";
            if (extraInfokey == trade_hosting_basicConstants.CTP_STS_APPID)
                return "APPID";
            if (extraInfokey == trade_hosting_basicConstants.CTP_STS_AUTHCODE)
                return "授权码";
            return null;
        }

        private void RecvUserHasLogouted(ProxyLoginResp lastLoginResp)
        {
            Shutdown();
        }

        private void ClosePage()
        {
            detailDialog.Close();
        }
    }
}
