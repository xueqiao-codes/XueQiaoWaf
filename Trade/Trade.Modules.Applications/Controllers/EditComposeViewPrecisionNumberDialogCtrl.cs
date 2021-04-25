using business_foundation_lib.helper;
using business_foundation_lib.xq_thriftlib_config;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows;
using xueqiao.trade.hosting.proxy;
using XueQiaoFoundation.BusinessResources.Constants;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.BusinessResources.Helpers;
using XueQiaoFoundation.Interfaces.Applications;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;
using XueQiaoWaf.Trade.Modules.Applications.ViewModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class EditComposeViewPrecisionNumberDialogCtrl : IController
    {
        private readonly EditComposeViewPrecisionNumberVM contentVM;
        private readonly ILoginDataService loginDataService;
        private readonly Lazy<ILoginUserManageService> loginUserManageService;
        private readonly IMessageWindowService messageWindowService;
        private readonly IEventAggregator eventAggregator;

        private readonly DelegateCommand okCommand;
        private readonly DelegateCommand cancelCommand;

        private IMessageWindow dialog;

        [ImportingConstructor]
        public EditComposeViewPrecisionNumberDialogCtrl(
            EditComposeViewPrecisionNumberVM contentVM,
            ILoginDataService loginDataService,
            Lazy<ILoginUserManageService> loginUserManageService,
            IMessageWindowService messageWindowService,
            IEventAggregator eventAggregator)
        {
            this.contentVM = contentVM;
            this.loginDataService = loginDataService;
            this.loginUserManageService = loginUserManageService;
            this.messageWindowService = messageWindowService;
            this.eventAggregator = eventAggregator;

            this.okCommand = new DelegateCommand(SubmitEdit);
            this.cancelCommand = new DelegateCommand(LeaveDialog);

            loginUserManageService.Value.HasLogouted += RecvUserHasLogouted;
        }


        public object DialogOwner { set; get; }
        public Point? DialogShowLocationRelativeToScreen { get; set; }

        /// <summary>
        /// 要修改名称的组合视图容器
        /// </summary>
        public UserComposeViewContainer EditComposeViewContainer { get; set; }

        /// <summary>
        /// 完成修改的小数位数
        /// </summary>
        public short? UpdatedPrecisionNumber { get; private set; }

        public void Initialize()
        {
            if (EditComposeViewContainer == null) throw new ArgumentNullException("EditComposeViewContainer");

            contentVM.EditComposeViewContainer = EditComposeViewContainer;

            contentVM.PrecisionNumberMin = (short)XueQiaoConstants.XQComposePrice_LowerDecimalCount;
            contentVM.PrecisionNumberMax = (short)XueQiaoConstants.XQComposePrice_UpperDecimalCount;
            contentVM.PrecisionNumber = EditComposeViewContainer.UserComposeView?.PrecisionNumber ?? contentVM.PrecisionNumberMin;
            
            contentVM.OkCommand = okCommand;
            contentVM.CancelCommand = cancelCommand;
        }

        public void Run()
        {
            this.dialog = messageWindowService.CreateContentCustomWindow(DialogOwner, DialogShowLocationRelativeToScreen, null, true, false,
                true, "编辑小数位数", contentVM.View);
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
        
        private void SubmitEdit()
        {
            var newPN = this.contentVM.PrecisionNumber;
            var landingInfo = loginDataService.LandingInfo;
            if (landingInfo == null) return;

            var composeId = EditComposeViewContainer.ComposeGraphId;
            XqThriftLibConfigurationManager.SharedInstance.TradeHostingTerminalAoHttpStub
            .changeComposeViewPrecisionNumberAsync(landingInfo, composeId, newPN, CancellationToken.None)
            .ContinueWith(t =>
            {
                var resp = t.Result;
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    if (resp == null || resp.SourceException != null)
                    {
                        var currentWin = GetCurrentWindow();
                        if (currentWin != null)
                        {
                            var errMsg = FoundationHelper.FormatResponseDisplayErrorMsg(resp, "修改小数位数失败！\n");
                            messageWindowService.ShowMessageDialog(currentWin, null, null, null, errMsg);
                        }
                        return;
                    }

                    this.UpdatedPrecisionNumber = newPN;
                    InternalCloseDialog();
                });

            });
        }
        
        private void LeaveDialog()
        {
            this.UpdatedPrecisionNumber = null;
            InternalCloseDialog();
        }
        
        private object GetCurrentWindow()
        {
            return UIHelper.GetWindowOfUIElement(contentVM.View);
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
