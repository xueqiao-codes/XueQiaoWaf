using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows;
using xueqiao.trade.hosting.proxy;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;

using XueQiaoWaf.Trade.Interfaces.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.ViewModels;
using XueQiaoWaf.Trade.Modules.Domain.Trades;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers
{
    /// <summary>
    /// 订阅数据分组新增/编辑弹窗 controller
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class SubscribeDataGroupEditDialogController : IController
    {
        private readonly IMessageWindowService messageWindowService;
        private readonly IEventAggregator eventAggregator;
        private readonly Lazy<ILoginUserManageService> loginUserManageService;
        private readonly SubscribeDataGroupEditDialogContentViewModel pageViewModel;

        private readonly DelegateCommand okCmd;
        private readonly DelegateCommand cancelCmd;

        private SubscribeDataGroup internalInitialEditGroup;
        private readonly EditSubscribeDataGroup editingDataGroup = new EditSubscribeDataGroup();

        private IMessageWindow editDialog;

        [ImportingConstructor]
        public SubscribeDataGroupEditDialogController(IMessageWindowService messageWindowService,
            IEventAggregator eventAggregator,
            Lazy<ILoginUserManageService> loginUserManageService,
            SubscribeDataGroupEditDialogContentViewModel pageViewModel)
        {
            this.messageWindowService = messageWindowService;
            this.eventAggregator = eventAggregator;
            this.loginUserManageService = loginUserManageService;
            this.pageViewModel = pageViewModel;

            okCmd = new DelegateCommand(DoSaveGroup);
            cancelCmd = new DelegateCommand(LeaveDialog);

            loginUserManageService.Value.HasLogouted += RecvUserHasLogouted;
        }

        public object DialogOwner { get; set; }
        public string DialogTitle { get; set; } = "编辑分组";
        public Point? DialogShowLocationRelativeToScreen { get; set; }

        /// <summary>
        /// 初始编辑的分组
        /// </summary>
        public SubscribeDataGroup InitialEditGroup { get; set; }

        /// <summary>
        /// 编辑确认结果
        /// </summary>
        public bool? EditConfirmResult { get; private set; }
        
        public void Initialize()
        {
            if (InitialEditGroup == null) throw new ArgumentNullException("InitialEditGroup");

            this.internalInitialEditGroup = this.InitialEditGroup;
            editingDataGroup.GroupName = internalInitialEditGroup?.GroupName;

            pageViewModel.EditGroup = editingDataGroup;
            pageViewModel.OkCmd = okCmd;
            pageViewModel.CancelCmd = cancelCmd;
        }

        public void Run()
        {
            this.editDialog = messageWindowService.CreateContentCustomWindow(DialogOwner, DialogShowLocationRelativeToScreen, null, 
                true, false, true, DialogTitle, pageViewModel.View);
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
            pageViewModel.CloseDisplayInWindow();
        }

        private void DoSaveGroup()
        {
            if (!editingDataGroup.Validate())
            {
                var errorMsg = editingDataGroup.JoinErrors();
                messageWindowService.ShowMessageDialog(pageViewModel.DisplayInWindow, null, null, "提示",
                    string.IsNullOrWhiteSpace(errorMsg) ? "请填写必填信息" : errorMsg);
                return;
            }

            InitialEditGroup.GroupName = editingDataGroup.GroupName;
            this.EditConfirmResult = true;
            // 关闭窗口
            pageViewModel.CloseDisplayInWindow();
        }

        private void LeaveDialog()
        {
            this.EditConfirmResult = null;
            pageViewModel.CloseDisplayInWindow();
        }
    }
}
