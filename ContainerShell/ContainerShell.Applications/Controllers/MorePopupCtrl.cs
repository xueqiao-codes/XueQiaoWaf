using AppAssembler.Interfaces.Applications;
using ContainerShell.Applications.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows;
using System.Windows.Controls.Primitives;
using xueqiao.trade.hosting.proxy;
using XueQiaoFoundation.Interfaces.Applications;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;
using IDLAutoGenerated.Util;

namespace ContainerShell.Applications.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class MorePopupCtrl : IController
    {
        private readonly IMessageWindowService messageWindowService;
        private readonly Lazy<IAppAssemblerService> appAssemblerService;
        private readonly Lazy<ILoginUserManageService> loginUserManageService;
        private readonly Lazy<ILoginDataService> loginDataService;
        private readonly ExportFactory<FeedbackDialogCtrl> feedbackDialogCtrlFactory;
        private readonly ExportFactory<XqPreferenceSettingDialogCtrl> prefSettingDialogCtrlFactory;
        private readonly MorePopupVM popupVM;

        private readonly DelegateCommand toShowApplicationInfoCmd;
        private readonly DelegateCommand toPostFeedbackCmd;
        private readonly DelegateCommand toUpdatePrefSettingCmd;
        private readonly DelegateCommand toUpdateLoginPwdCmd;
        //private readonly DelegateCommand toChangeLoginAccountCmd;
        private readonly DelegateCommand toExitAppCmd;

        [ImportingConstructor]
        public MorePopupCtrl(
            IMessageWindowService messageWindowService,
            Lazy<IAppAssemblerService> appAssemblerService,
            Lazy<ILoginUserManageService> loginUserManageService,
            Lazy<ILoginDataService> loginDataService,
            ExportFactory<FeedbackDialogCtrl> feedbackDialogCtrlFactory,
            ExportFactory<XqPreferenceSettingDialogCtrl> prefSettingDialogCtrlFactory,
            MorePopupVM popupVM)
        {
            this.messageWindowService = messageWindowService;
            this.appAssemblerService = appAssemblerService;
            this.loginUserManageService = loginUserManageService;
            this.loginDataService = loginDataService;
            this.feedbackDialogCtrlFactory = feedbackDialogCtrlFactory;
            this.prefSettingDialogCtrlFactory = prefSettingDialogCtrlFactory;
            this.popupVM = popupVM;
            
            toShowApplicationInfoCmd = new DelegateCommand(ToShowApplicationInfo);
            toPostFeedbackCmd = new DelegateCommand(ToPostFeedback);
            toUpdatePrefSettingCmd = new DelegateCommand(ToUpdatePrefSetting);
            toUpdateLoginPwdCmd = new DelegateCommand(ToUpdateLoginPwd);
            //toChangeLoginAccountCmd = new DelegateCommand(ToChangeLoginAccount);
            toExitAppCmd = new DelegateCommand(ToExitApp);
        }

        /// <summary>
        /// 弹层目标
        /// </summary>
        public object PopupPalcementTarget { get; set; }

        /// <summary>
        /// 弹层位置
        /// </summary>
        public PlacementMode PopupPlacement { get; set; }

        /// <summary>
        /// 浮层关闭处理
        /// </summary>
        public Action<MorePopupCtrl> PopupClosed { get; set; }
        
        public void Initialize()
        {
            loginUserManageService.Value.LoginUserAppUpdateInfoQueried += LoginUserAppUpdateInfoQueried;

            popupVM.Closed += PopupVM_Closed;
            popupVM.ShowApplicationInfoCmd = toShowApplicationInfoCmd;
            popupVM.PostFeedbackCmd = toPostFeedbackCmd;
            popupVM.UpdatePrefSettingCmd = toUpdatePrefSettingCmd;
            popupVM.UpdateLoginPwdCmd = toUpdateLoginPwdCmd;
            //popupVM.ChangeLoginAccountCmd = toChangeLoginAccountCmd;
            popupVM.ExitAppCmd = toExitAppCmd;
            popupVM.PopupPlacement = PopupPlacement;

            InvalidateHasNewAppVersion();
        }

        public void Run()
        {
            popupVM.ShowPopup(PopupPalcementTarget);
        }

        public void Shutdown()
        {
            loginUserManageService.Value.LoginUserAppUpdateInfoQueried -= LoginUserAppUpdateInfoQueried;
            popupVM.Closed -= PopupVM_Closed;
        }

        private void PopupVM_Closed(object sender, EventArgs e)
        {
            PopupClosed?.Invoke(this);
            Shutdown();
        }

        private void LoginUserAppUpdateInfoQueried(AppVersion appVersion)
        {
            InvalidateHasNewAppVersion();
        }

        private void InvalidateHasNewAppVersion()
        {
            var currentVersion = AppVersionHelper.GetApplicationVersion();
            var queriedVersion = loginDataService.Value.LoginUserAppUpdateInfo?.VersionNum?.ToLocalVersion();

            var hasNewVersion = false;
            if (queriedVersion != null)
            {
                hasNewVersion = currentVersion != queriedVersion;
            }
            popupVM.HasNewAppVersion = hasNewVersion;
        }

        private void ToShowApplicationInfo()
        {
            popupVM.Close();
            loginUserManageService.Value.ShowUserAppVersionInfoDialog(UIHelper.GetWindowOfUIElement(PopupPalcementTarget));
        }

        private void ToPostFeedback()
        {
            popupVM.Close();
            var dgCtrl = feedbackDialogCtrlFactory.CreateExport().Value;
            dgCtrl.DialogOwner = UIHelper.GetWindowOfUIElement(PopupPalcementTarget);
            dgCtrl.Initialize();
            dgCtrl.Run();
            dgCtrl.Shutdown();
        }

        private void ToUpdatePrefSetting()
        {
            popupVM.Close();
            var dgCtrl = prefSettingDialogCtrlFactory.CreateExport().Value;
            dgCtrl.DialogOwner = UIHelper.GetWindowOfUIElement(PopupPalcementTarget);
            dgCtrl.Initialize();
            dgCtrl.Run();
            dgCtrl.Shutdown();
        }

        private void ToUpdateLoginPwd()
        {
            popupVM.Close();
            loginUserManageService.Value.ShowLoginPwdUpdateDialog(UIHelper.GetWindowOfUIElement(PopupPalcementTarget));
        }

        //private void ToChangeLoginAccount()
        //{
        //    popupVM.Close();

        //    loginUserManageService.Value.DoSignout();
        //    if (!appAssemblerService.Value.IsShowingStartupWindow)
        //    {
        //        appAssemblerService.Value.ShowStartupUI();
        //    }
        //}

        private void ToExitApp()
        {
            popupVM.Close();
            appAssemblerService.Value.ShutdownApplication();
        }
    }
}
