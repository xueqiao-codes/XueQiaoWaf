using business_foundation_lib.helper;
using business_foundation_lib.xq_thriftlib_config;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using xueqiao.broker;
using xueqiao.trade.hosting;
using xueqiao.trade.hosting.proxy;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;

namespace Manage.Applications.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class AddFundAccountWorkOrderDialogCtrl : IController
    {
        private readonly FundAccountAddViewCtrl accountAddViewCtrl;
        private readonly ILoginDataService loginDataService;
        private readonly Lazy<ILoginUserManageService> loginUserManageService;
        private readonly IMessageWindowService messageWindowService;

        private IMessageWindow dialog;

        [ImportingConstructor]
        public AddFundAccountWorkOrderDialogCtrl(
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

        public string DialogTitle { get; set; }

        /// <summary>
        /// 添加账号的券商平台范围。如果为 null 或空，则不限定范围
        /// </summary>
        public BrokerPlatform[] BrokerPlatforms { get; set; }

        /// <summary>
        /// 添加成功后的工单 id
        /// </summary>
        public long? AddedWorkOrderId { get; private set; }

        public void Initialize()
        {
            accountAddViewCtrl.AccountAddSubmitHandler = (_ctrl, _validateErr, _addInputInfo) =>
            {
                if (!string.IsNullOrEmpty(_validateErr))
                {
                    messageWindowService.ShowMessageDialog(UIHelper.GetWindowOfUIElement(_ctrl.ContentView), null, null,null, _validateErr);
                    return;
                }
                SubmitAddFundAccountWorkOrder(_addInputInfo);
            };

            accountAddViewCtrl.CancelHandler = _ctrl => 
            {
                this.AddedWorkOrderId = null;
                InternalCloseDialog();
            };

            accountAddViewCtrl.BrokerPlatforms = BrokerPlatforms;
            accountAddViewCtrl.ShowAccountAliasAddRow = false;

            accountAddViewCtrl.Initialize();
            accountAddViewCtrl.Run();
        }

        public void Run()
        {
            var dialogTitle = DialogTitle ?? "设置资金账户";
            this.dialog = messageWindowService.CreateContentCustomWindow(DialogOwner, null, null, true, false,
                true, dialogTitle, accountAddViewCtrl.ContentView);
            dialog.ShowDialog();
        }

        public void Shutdown()
        {
            loginUserManageService.Value.HasLogouted -= RecvUserHasLogouted;
            InternalCloseDialog();
            accountAddViewCtrl?.Shutdown();
        }
        
        private void RecvUserHasLogouted(ProxyLoginResp lastLoginResp)
        {
            Shutdown();
        }

        private void InternalCloseDialog()
        {
            if (dialog != null)
            {
                dialog.Close();
                dialog = null;
            }
        }

        private void SubmitAddFundAccountWorkOrder(FundAccountAddItemInputInfo addInputInfo)
        {
            if (addInputInfo == null) return;
            var landingInfo = loginDataService.LandingInfo;
            if (landingInfo == null) return;

            var workOrderInfo = new xueqiao.working.order.thriftapi.AssetAccount
            {
                AccountName = addInputInfo.LoginUserName,
                Password = addInputInfo.LoginPassword,
                NickName = addInputInfo.TradeAccountRemark,
                BrokerId = addInputInfo.TradeBrokerId ?? 0,
                BrokerAccessId = addInputInfo.TradeBrokerAccessId ?? 0,
                ExtraInfo = addInputInfo.AccountProperties
            };

            // 处理授权码
            if (addInputInfo.BrokerTechPlatform == BrokerTechPlatform.TECH_ESUNNY_9)
            {
                string authCode = null;
                if (addInputInfo.AccountProperties?.TryGetValue(trade_hosting_basicConstants.ESUNNY9_AUTHCODE_PROPKEY, out authCode) == true)
                {
                    workOrderInfo.AuthorizationCode = authCode;
                }
            }

            XqThriftLibConfigurationManager.SharedInstance.TradeHostingTerminalAoHttpStub
                .addAssetAccountWorkingOrderAsync(landingInfo, workOrderInfo, CancellationToken.None)
                .ContinueWith(t => 
                {
                    var resp = t.Result;
                    DispatcherHelper.CheckBeginInvokeOnUI(()=> 
                    {
                        var winOwner = UIHelper.GetWindowOfUIElement(accountAddViewCtrl.ContentView);
                        if (resp?.CorrectResult == null)
                        {
                            if (winOwner != null)
                            {
                                var errMsg = FoundationHelper.FormatResponseDisplayErrorMsg(resp, "提交失败\n");
                                messageWindowService.ShowMessageDialog(winOwner, null, null, null, errMsg);
                            }
                            return;
                        }
                        
                        this.AddedWorkOrderId = resp?.CorrectResult;
                        InternalCloseDialog();
                    });
                });
        }
    }
}
