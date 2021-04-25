using business_foundation_lib.helper;
using business_foundation_lib.xq_thriftlib_config;
using lib.xqclient_base.thriftapi_mediation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using xueqiao.trade.hosting;
using xueqiao.trade.hosting.proxy;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;

namespace Manage.Applications.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class FundAccounAddDialogController : IController
    {
        private readonly FundAccountAddViewCtrl accountAddViewCtrl;
        private readonly ILoginDataService loginDataService;
        private readonly Lazy<ILoginUserManageService> loginUserManageService;
        private readonly IMessageWindowService messageWindowService;
        
        private IMessageWindow dialog;
        
        [ImportingConstructor]
        public FundAccounAddDialogController(
            FundAccountAddViewCtrl accountAddViewCtrl,
            ILoginDataService loginDataService,
            Lazy<ILoginUserManageService> loginUserManageService,
            IMessageWindowService messageWindowService)
        {
            this.accountAddViewCtrl = accountAddViewCtrl;
            this.loginDataService = loginDataService;
            this.loginUserManageService = loginUserManageService;
            this.messageWindowService = messageWindowService;
            
            loginUserManageService.Value.HasLogouted += RecvUserHasLogouted;
        }

        public object DialogOwner { get; set; }

        /// <summary>
        /// 添加的账号结果
        /// </summary>
        public HostingTradeAccount AddedAccountResult { get; private set; }

        public void Initialize()
        {
            accountAddViewCtrl.AccountAddSubmitHandler = (_ctrl, _validateErr, _addInputInfo) =>
            {
                if (!string.IsNullOrEmpty(_validateErr))
                {
                    messageWindowService.ShowMessageDialog(UIHelper.GetWindowOfUIElement(_ctrl.ContentView), null, null, null, _validateErr);
                    return;
                }
                SubmitAddFundAccount(_addInputInfo);
            };

            accountAddViewCtrl.CancelHandler = _ctrl =>
            {
                this.AddedAccountResult = null;
                InternalCloseDialog();
            };

            accountAddViewCtrl.BrokerPlatforms = null;
            accountAddViewCtrl.ShowAccountAliasAddRow = true;

            accountAddViewCtrl.Initialize();
            accountAddViewCtrl.Run();
        }

        public void Run()
        {
            var dialogTitle = "添加资金账户";
            this.dialog = messageWindowService.CreateContentCustomWindow(DialogOwner, null, null, true, false,
                true, dialogTitle, accountAddViewCtrl.ContentView);
            dialog.ShowDialog();
        }

        public void Shutdown()
        {
            InternalCloseDialog();
            loginUserManageService.Value.HasLogouted -= RecvUserHasLogouted;
        }
        
        private void RecvUserHasLogouted(ProxyLoginResp lastLoginResp)
        {
            Shutdown();
        }
        
        private void SubmitAddFundAccount(FundAccountAddItemInputInfo addInputInfo)
        {
            if (addInputInfo == null) return;
            var landingInfo = loginDataService.LandingInfo;
            if (landingInfo == null) return;

            var newAccount = new HostingTradeAccount
            {
                TradeBrokerId = addInputInfo.TradeBrokerId ?? 0,
                TradeBrokerAccessId = addInputInfo.TradeBrokerAccessId ?? 0,
                LoginUserName = addInputInfo.LoginUserName,
                LoginPassword = addInputInfo.LoginPassword,
                TradeAccountRemark = addInputInfo.TradeAccountRemark,
                AccountProperties = addInputInfo.AccountProperties
            };

            var interactParams = new StubInterfaceInteractParams { LogInterfaceRequestArgs = false };
            AddedAccountResult = null;
            XqThriftLibConfigurationManager.SharedInstance.TradeHostingTerminalAoHttpStub
                .addTradeAccountAsync(landingInfo, newAccount, CancellationToken.None, interactParams)
                .ContinueWith(t =>
                {
                    var resp = t.Result;
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        var winOwner = UIHelper.GetWindowOfUIElement(accountAddViewCtrl.ContentView);
                        if (resp == null || resp.SourceException != null)
                        {
                            if (winOwner != null)
                            {
                                var errMsg = FoundationHelper.FormatResponseDisplayErrorMsg(resp, "提交失败\n");
                                messageWindowService.ShowMessageDialog(winOwner, null, null, null, errMsg);
                            }
                            return;
                        }
                        
                        newAccount.TradeAccountId = resp.CorrectResult;
                        this.AddedAccountResult = newAccount;
                        InternalCloseDialog();
                    });
                });
        }

        private void LeaveDialog()
        {
            AddedAccountResult = null;
            InternalCloseDialog();
        }

        private void InternalCloseDialog()
        {
            if (dialog != null)
            {
                dialog.Close();
                dialog = null;
            }
        }
    }
}
