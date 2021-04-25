using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Waf.Foundation;
using xueqiao.trade.hosting.proxy;
using XueQiaoFoundation.BusinessResources.Helpers;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoWaf.LoginUserManage.Modules.Applications.ViewModels;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;
using business_foundation_lib.xq_thriftlib_config;
using business_foundation_lib.helper;

namespace XueQiaoWaf.LoginUserManage.Modules.Applications.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class UpdateLoginPwdDialogCtrl : IController
    {
        private readonly IMessageWindowService messageWindowService;
        private readonly ILoginDataService loginDataService;
        private readonly Lazy<ILoginUserManageService> loginUserManageService;
        
        private readonly IEventAggregator eventAggregator;
        private readonly UpdateLoginPwdContentVM contentVM;

        private readonly DelegateCommand updatePwdCmd;
        private readonly DelegateCommand closeDialogCmd;

        private IMessageWindow dialog;

        [ImportingConstructor]
        public UpdateLoginPwdDialogCtrl(
            IMessageWindowService messageWindowService,
            ILoginDataService loginDataService,
            Lazy<ILoginUserManageService> loginUserManageService,
            
            IEventAggregator eventAggregator,
            UpdateLoginPwdContentVM contentVM)
        {
            this.messageWindowService = messageWindowService;
            this.loginDataService = loginDataService;
            this.loginUserManageService = loginUserManageService;
            
            this.eventAggregator = eventAggregator;
            this.contentVM = contentVM;

            updatePwdCmd = new DelegateCommand(UpdatePwd);
            closeDialogCmd = new DelegateCommand(CloseDialog);

            loginUserManageService.Value.HasLogouted += RecvUserHasLogouted;
        }

        public object DialogOwner { get; set; }

        public void Initialize()
        {
            contentVM.UpdatePwdCmd = updatePwdCmd;
            contentVM.CancelCmd = closeDialogCmd;
        }

        public void Run()
        {
            dialog = messageWindowService.CreateContentCustomWindow(DialogOwner, null, null, true, false,
                true, "修改登录密码", contentVM.View);
            dialog.ShowDialog();
        }

        public void Shutdown()
        {
            CloseDialog();
            loginUserManageService.Value.HasLogouted -= RecvUserHasLogouted;
        }

        private void RecvUserHasLogouted(ProxyLoginResp lastLoginResp)
        {
            Shutdown();
        }

        private void UpdatePwd()
        {
            var errorMsg = contentVM.UpdatePwd.Validate();
            if (!string.IsNullOrEmpty(errorMsg))
            {
                var owner = UIHelper.GetWindowOfUIElement(contentVM.View);
                messageWindowService.ShowMessageDialog(owner, null, null, null, errorMsg);
                return;
            }

            var loginCompanyId = loginDataService.ProxyLoginResp?.CompanyId;
            var loginUserName = loginDataService.ProxyLoginResp?.LoginUserInfo?.LoginName;

            if (loginCompanyId == null || string.IsNullOrEmpty(loginUserName))
            {
                // not logined
                return;
            }

            var oldPwd_encrypted = XueQiaoBusinessHelper.EncryptLoginPwd(contentVM.UpdatePwd.OldPwd);
            var newPwd_encrypted = XueQiaoBusinessHelper.EncryptLoginPwd(contentVM.UpdatePwd.NewPwdEdit.Pwd);
            if (string.IsNullOrEmpty(oldPwd_encrypted) || string.IsNullOrEmpty(newPwd_encrypted))
            {
                return;
            }

            var req = new ProxyUpdatePasswordReq
            {
                CompanyId = loginCompanyId.Value,
                UserName = loginUserName,
                OldPassword = oldPwd_encrypted,
                NewPassword = newPwd_encrypted
            };
            XqThriftLibConfigurationManager.SharedInstance.TradeHostingProxyHttpStub.updateCompanyUserPasswordAsync(req, CancellationToken.None)
                .ContinueWith(t => 
                {
                    var resp = t.Result;
                    DispatcherHelper.CheckBeginInvokeOnUI(() => 
                    {
                        if (resp == null) return;
                        var ownerWin = UIHelper.GetWindowOfUIElement(contentVM.View);
                        if (ownerWin == null) return;
                        if (resp.SourceException == null)
                        {
                            messageWindowService.ShowMessageDialog(ownerWin, null, null, null, "密码修改成功");
                            CloseDialog();
                        }
                        else
                        {
                            var errMsg = FoundationHelper.FormatResponseDisplayErrorMsg(resp, "密码修改出错！\n");
                            messageWindowService.ShowMessageDialog(ownerWin, null, null, null, errMsg);
                        }
                    });
                });
        }

        private void CloseDialog()
        {
            if (dialog != null)
            {
                dialog.Close();
                dialog = null;
            }
        }
    }
}
