using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using xueqiao.trade.hosting.proxy;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;

using XueQiaoWaf.Trade.Interfaces.Applications;
using XueQiaoWaf.Trade.Modules.Applications.ViewModels;
using XueQiaoWaf.Trade.Modules.Domain.Workspaces;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers
{
    /// <summary>
    /// 工作空间模板新增/编辑弹窗 controller
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class TradeWorkspaceTemplateEditDialogController : IController
    {
        private readonly IMessageWindowService messageWindowService;
        private readonly IEventAggregator eventAggregator;
        private readonly ITradeModuleService tradeModuleService;
        private readonly Lazy<ILoginUserManageService> loginUserManageService;
        private readonly TradeWorkspaceTemplateEditDialogContentVM pageViewModel;
        
        private readonly DelegateCommand okCmd;
        private readonly DelegateCommand cancelCmd;
        
        private readonly EditTradeWorkspaceTemplate editingWorkspaceTemplate = new EditTradeWorkspaceTemplate();
        
        private IMessageWindow editDialog;

        [ImportingConstructor]
        public TradeWorkspaceTemplateEditDialogController(IMessageWindowService messageWindowService,
            IEventAggregator eventAggregator,
            ITradeModuleService tradeModuleService,
            Lazy<ILoginUserManageService> loginUserManageService,
            TradeWorkspaceTemplateEditDialogContentVM pageViewModel)
        {
            this.messageWindowService = messageWindowService;
            this.eventAggregator = eventAggregator;
            this.tradeModuleService = tradeModuleService;
            this.loginUserManageService = loginUserManageService;
            this.pageViewModel = pageViewModel;
            
            okCmd = new DelegateCommand(DoSaveTemplate);
            cancelCmd = new DelegateCommand(LeaveDialog);

            loginUserManageService.Value.HasLogouted += RecvUserHasLogouted;
        }

        /// <summary>
        /// 初始需要编辑的模板。若存在于 `TradeWorkspaceTemplateListContainer` 中，则为修改信息模式，否则为新增模式
        /// </summary>
        public TradeTabWorkspaceTemplate InitialEditTemplate { get; set; }

        public string DialogTitle { get; set; }

        public object DialogOwner { get; set; }

        public bool? EditSuccessResult { get; private set; }

        public void Initialize()
        {
            if (InitialEditTemplate == null) throw new ArgumentNullException("InitialEditTemplate");

            editingWorkspaceTemplate.Name = InitialEditTemplate.TemplateName;
            editingWorkspaceTemplate.Remark = InitialEditTemplate.TemplateDesc;

            pageViewModel.EditTemplate = editingWorkspaceTemplate;
            pageViewModel.OkCmd = okCmd;
            pageViewModel.CancelCmd = cancelCmd;
        }

        public void Run()
        {
            var dataRoot = tradeModuleService.TradeWorkspaceDataRoot;
            if (dataRoot == null) return;

            var dialogTitle = DialogTitle
                ??(dataRoot.TradeWorkspaceTemplateListContainer.Templates.Contains(InitialEditTemplate) ? "修改模板信息" : "新增模板");
            this.editDialog = messageWindowService.CreateContentCustomWindow(DialogOwner, null, null, true, false,
                true, dialogTitle, pageViewModel.View);
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
            Shutdown();
        }

        private void DoSaveTemplate()
        {
            if (!editingWorkspaceTemplate.Validate())
            {
                var errorMsg = editingWorkspaceTemplate.JoinErrors();
                messageWindowService.ShowMessageDialog(pageViewModel.DisplayInWindow, null, null, "提示", 
                    string.IsNullOrWhiteSpace(errorMsg)?"请填写必填信息": errorMsg);
                return;
            }

            var dataRoot = tradeModuleService.TradeWorkspaceDataRoot;
            if (dataRoot == null) return;

            if (dataRoot.TradeWorkspaceTemplateListContainer.Templates.Any(i => i.TemplateName == editingWorkspaceTemplate.Name))
            {
                messageWindowService.ShowMessageDialog(pageViewModel.DisplayInWindow, null, null, "提示",
                    "存在同名的模板，请换一个！");
                return;
            }

            var currentTimestamp = (int)DateHelper.NowUnixTimeSpan().TotalSeconds;
            if (!dataRoot.TradeWorkspaceTemplateListContainer.Templates.Contains(InitialEditTemplate))
            {
                InitialEditTemplate.CreateTimestamp = currentTimestamp;
                // 保存进模板容器
                dataRoot.TradeWorkspaceTemplateListContainer.Templates.Add(InitialEditTemplate);
            }

            InitialEditTemplate.TemplateName = editingWorkspaceTemplate.Name;
            InitialEditTemplate.TemplateDesc = editingWorkspaceTemplate.Remark;
            InitialEditTemplate.LastUpdateTimestamp = currentTimestamp;

            // 关闭窗口
            this.EditSuccessResult = true;
            pageViewModel.CloseDisplayInWindow();
        }

        private void LeaveDialog()
        {
            this.EditSuccessResult = null;
            pageViewModel.CloseDisplayInWindow();
        }
    }
}
