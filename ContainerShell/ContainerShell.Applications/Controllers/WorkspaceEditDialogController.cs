using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;

using XueQiaoFoundation.BusinessResources.DataModels;
using ContainerShell.Applications.ViewModels;
using ContainerShell.Applications.Domain;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;
using xueqiao.trade.hosting.proxy;

namespace ContainerShell.Applications.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class WorkspaceEditDialogController : IController
    {
        private readonly IMessageWindowService messageWindowService;
        private readonly IEventAggregator eventAggregator;
        private readonly Lazy<ILoginUserManageService> loginUserManageService;
        private readonly WorkspaceEditDialogContentVM contentVM;

        private readonly DelegateCommand okCmd;
        private readonly DelegateCommand cancelCmd;

        private TabWorkspace internalInitialEditWorkspace;
        private readonly EditWorkspace editingWorkspace = new EditWorkspace();

        private IMessageWindow editDialog;

        [ImportingConstructor]
        public WorkspaceEditDialogController(IMessageWindowService messageWindowService,
            IEventAggregator eventAggregator,
            Lazy<ILoginUserManageService> loginUserManageService,
            WorkspaceEditDialogContentVM contentVM)
        {
            this.messageWindowService = messageWindowService;
            this.eventAggregator = eventAggregator;
            this.loginUserManageService = loginUserManageService;
            this.contentVM = contentVM;

            okCmd = new DelegateCommand(DoSaveWorkspace);
            cancelCmd = new DelegateCommand(LeaveDialog);

            loginUserManageService.Value.HasLogouted += RecvUserHasLogouted;
        }

        public object DialogOwner { get; set; }
        public string DialogTitle { get; set; } = "重命名工作空间";
        public Point? DialogShowLocationRelativeToScreen { get; set; }

        /// <summary>
        /// 初始编辑的工作空间
        /// </summary>
        public TabWorkspace InitialEditWorkspace { get; set; }

        /// <summary>
        /// 编辑确认结果
        /// </summary>
        public bool? EditConfirmResult { get; private set; }

        public void Initialize()
        {
            this.internalInitialEditWorkspace = this.InitialEditWorkspace;
            if (internalInitialEditWorkspace == null) throw new ArgumentNullException("InitialEditWorkspace");

            editingWorkspace.WorkspaceName = internalInitialEditWorkspace.Name;

            contentVM.EditWorkspace = editingWorkspace;
            contentVM.OkCmd = okCmd;
            contentVM.CancelCmd = cancelCmd;
        }

        public void Run()
        {
            this.editDialog = messageWindowService.CreateContentCustomWindow(DialogOwner, DialogShowLocationRelativeToScreen, null, true, false,
                true, DialogTitle, contentVM.View);
            editDialog.ShowDialog();
        }

        public void Shutdown()
        {
            if (editDialog != null)
            {
                editDialog.Close();
                editDialog = null;
            }
            loginUserManageService.Value.HasLogouted -= RecvUserHasLogouted;
        }

        private void RecvUserHasLogouted(ProxyLoginResp lastLoginResp)
        {
            this.EditConfirmResult = null;
            contentVM.CloseDisplayInWindow();
        }

        private void DoSaveWorkspace()
        {
            if (!editingWorkspace.Validate())
            {
                var errorMsg = editingWorkspace.JoinErrors();
                messageWindowService.ShowMessageDialog(contentVM.DisplayInWindow, null, null, "提示",
                    string.IsNullOrWhiteSpace(errorMsg) ? "请填写必填信息" : errorMsg);
                return;
            }

            internalInitialEditWorkspace.Name = editingWorkspace.WorkspaceName;
            this.EditConfirmResult = true;
            // 关闭窗口
            contentVM.CloseDisplayInWindow();
        }

        private void LeaveDialog()
        {
            this.EditConfirmResult = null;
            contentVM.CloseDisplayInWindow();
        }
    }
}
