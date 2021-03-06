using Manage.Applications.ViewModels;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using xueqiao.trade.hosting.position.adjust.thriftapi;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;
using IDLAutoGenerated.Util;
using System.Threading;
using XueQiaoFoundation.Shared.Helper;
using xueqiao.trade.hosting.proxy;
using XueQiaoFoundation.BusinessResources.Helpers;
using business_foundation_lib.xq_thriftlib_config;
using business_foundation_lib.helper;

namespace Manage.Applications.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class UpdateDailyPositionVerifyStatusDialogCtrl : IController
    {
        private readonly UpdateDailyPositionVerifyStatusVM contentVM;
        private readonly ILoginDataService loginDataService;
        private readonly Lazy<ILoginUserManageService> loginUserManageService;
        private readonly IMessageWindowService messageWindowService;
        private readonly IEventAggregator eventAggregator;

        private readonly DelegateCommand saveCmd;
        private readonly DelegateCommand leaveDialogCmd;

        private IMessageWindow dialog;

        [ImportingConstructor]
        public UpdateDailyPositionVerifyStatusDialogCtrl(
            UpdateDailyPositionVerifyStatusVM contentVM,
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

            saveCmd = new DelegateCommand(SaveUpdate, CanSaveUpdate);
            leaveDialogCmd = new DelegateCommand(LeaveDialog);

            loginUserManageService.Value.HasLogouted += RecvUserHasLogouted;
        }

        public object DialogOwner { get; set; }
        
        /// <summary>
        /// 要更新的每日持仓比对项
        /// </summary>
        public DailyPositionDifference UpdateSourceDiff { get; set; }

        /// <summary>
        /// 是否成功更新
        /// </summary>
        public bool? IsSuccessUpdated { get; private set; }

        public void Initialize()
        {
            if (UpdateSourceDiff == null) throw new ArgumentNullException("UpdateSourceDiff");
            PropertyChangedEventManager.AddHandler(contentVM, ContentVMPropChanged, "");

            contentVM.SaveCmd = saveCmd;
            contentVM.CancelCmd = leaveDialogCmd;
            contentVM.SelectedVerifyStatus = this.UpdateSourceDiff.VerifyStatus;
            contentVM.VerifyNote = this.UpdateSourceDiff.Note;
        }

        public void Run()
        {
            dialog = messageWindowService.CreateContentCustomWindow(DialogOwner, null, null, true, true,
                true, "备注", contentVM.View);
            dialog.ShowDialog();
        }

        public void Shutdown()
        {
            InternalCloseDialog();
            PropertyChangedEventManager.RemoveHandler(contentVM, ContentVMPropChanged, "");
            loginUserManageService.Value.HasLogouted -= RecvUserHasLogouted;
        }

        private void RecvUserHasLogouted(ProxyLoginResp lastLoginResp)
        {
            Shutdown();
        }

        private void ContentVMPropChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(UpdateDailyPositionVerifyStatusVM.SelectedVerifyStatus)
                || e.PropertyName == nameof(UpdateDailyPositionVerifyStatusVM.VerifyNote))
            {
                saveCmd?.RaiseCanExecuteChanged();
            }
        }

        private bool CanSaveUpdate()
        {
            return contentVM.SelectedVerifyStatus != null && !string.IsNullOrEmpty(contentVM.VerifyNote);
        }

        private void SaveUpdate()
        {
            var verifyStatus = contentVM.SelectedVerifyStatus;
            var verifyNote = contentVM.VerifyNote;

            var landingInfo = loginDataService.ProxyLoginResp?.HostingSession?.HostingSession2LandingInfo();
            if (landingInfo == null) return;

            var diff = new DailyPositionDifference
            {
                DateSec = this.UpdateSourceDiff.DateSec,
                TradeAccountId = this.UpdateSourceDiff.TradeAccountId,
                SledContractId = this.UpdateSourceDiff.SledContractId,
                VerifyStatus = verifyStatus.Value,
                Note = verifyNote,
            };

            XqThriftLibConfigurationManager.SharedInstance.TradeHostingTerminalAoHttpStub.updateDailyPositionDifferenceNoteAsync(
                landingInfo, diff, CancellationToken.None)
            .ContinueWith(t => 
            {
                var resp = t.Result;
                DispatcherHelper.CheckBeginInvokeOnUI(() => 
                {
                    var dialogWin = UIHelper.GetWindowOfUIElement(contentVM.View);
                    if (resp == null || resp.SourceException != null)
                    {
                        this.IsSuccessUpdated = false;
                        // error
                        var errMsg = FoundationHelper.FormatResponseDisplayErrorMsg(resp, "更新出错！\n");
                        if (dialogWin != null)
                            messageWindowService.ShowMessageDialog(dialogWin, null, null, null, errMsg);
                        return;
                    }

                    this.IsSuccessUpdated = true;
                    InternalCloseDialog();
                });
            });
        }

        private void LeaveDialog()
        {
            this.IsSuccessUpdated = null;
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
