using business_foundation_lib.helper;
using business_foundation_lib.xq_thriftlib_config;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows;
using xueqiao.trade.hosting.proxy;
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
    internal class EditComposeNameDialogController : IController
    {
        private readonly EditComposeNameDialogContentViewModel contentVM;
        private readonly ILoginDataService loginDataService;
        private readonly Lazy<ILoginUserManageService> loginUserManageService;
        private readonly IMessageWindowService messageWindowService;
        private readonly IEventAggregator eventAggregator;

        private readonly DelegateCommand okCommand;
        private readonly DelegateCommand cancelCommand;

        private IMessageWindow dialog;

        [ImportingConstructor]
        public EditComposeNameDialogController(EditComposeNameDialogContentViewModel contentVM,
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

            this.okCommand = new DelegateCommand(SubmitEdit, CanSubmitEdit);
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
        /// 完成重命名的别名
        /// </summary>
        public string UpdatedAliasName { get; private set; }
        
        public void Initialize()
        {
            if (EditComposeViewContainer == null) throw new ArgumentNullException("EditComposeViewContainer");

            contentVM.EditComposeViewContainer = EditComposeViewContainer;
            contentVM.OkCommand = okCommand;
            contentVM.CancelCommand = cancelCommand;

            PropertyChangedEventManager.AddHandler(contentVM, ContentVMPropertyChanged, "");
        }

        public void Run()
        {
            this.dialog = messageWindowService.CreateContentCustomWindow(DialogOwner, DialogShowLocationRelativeToScreen, null, true, false, 
                true, "重命名组合", contentVM.View);
            dialog.ShowDialog();
        }

        public void Shutdown()
        {
            InternalCloseDialog();
            loginUserManageService.Value.HasLogouted -= RecvUserHasLogouted;
            PropertyChangedEventManager.AddHandler(contentVM, ContentVMPropertyChanged, "");
        }

        private void RecvUserHasLogouted(ProxyLoginResp lastLoginResp)
        {
            Shutdown();
        }

        private void ContentVMPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(EditComposeNameDialogContentViewModel.NewAliasName))
            {
                okCommand.RaiseCanExecuteChanged();
            }
        }

        private bool CanSubmitEdit()
        {
            return !string.IsNullOrWhiteSpace(contentVM.NewAliasName);
        }

        private void SubmitEdit()
        {
            var newAliasName = this.contentVM.NewAliasName;
            if (string.IsNullOrWhiteSpace(newAliasName))
            {
                return;
            }


            var landingInfo = loginDataService.LandingInfo;
            if (landingInfo == null) return;

            var composeId = EditComposeViewContainer.ComposeGraphId;
            var task = XqThriftLibConfigurationManager.SharedInstance.TradeHostingTerminalAoHttpStub.changeComposeViewAliasNameAsync(landingInfo, composeId, newAliasName, CancellationToken.None);
            task.ContinueWith(t => 
            {
                var resp = t.Result;
                DispatcherHelper.CheckBeginInvokeOnUI(() => 
                {
                    if (resp == null || resp.SourceException != null)
                    {
                        var currentWin = GetCurrentWindow();
                        if (currentWin != null)
                        {
                            var errMsg = FoundationHelper.FormatResponseDisplayErrorMsg(resp, "重命名失败！\n");
                            messageWindowService.ShowMessageDialog(currentWin, null, null, null, errMsg);
                        }
                        return;
                    }

                    this.UpdatedAliasName = newAliasName;
                    InternalCloseDialog();
                });
                
            });
        }

        private object GetCurrentWindow()
        {
            return UIHelper.GetWindowOfUIElement(contentVM.View);
        }

        private void LeaveDialog()
        {
            this.UpdatedAliasName = null;
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
