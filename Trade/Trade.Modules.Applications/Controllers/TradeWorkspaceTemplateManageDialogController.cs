using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using xueqiao.trade.hosting.proxy;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;

using XueQiaoWaf.Trade.Interfaces.Applications;
using XueQiaoWaf.Trade.Modules.Applications.ViewModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class TradeWorkspaceTemplateManageDialogController : IController
    {
        private readonly IMessageWindowService messageWindowService;
        private readonly TradeWorkspaceTemplateManageDialogContentVM pageViewModel;
        private readonly IEventAggregator eventAggregator;
        private readonly ITradeModuleService tradeModuleService;
        private readonly Lazy<ILoginUserManageService> loginUserManageService;
        private readonly ExportFactory<TradeWorkspaceTemplateEditDialogController> workspaceTemplateEditDialogCtrlFactory;

        private readonly DelegateCommand toEditItemCmd;
        private readonly DelegateCommand toDeleteItemCmd;

        private IMessageWindow displayDialog;

        [ImportingConstructor]
        public TradeWorkspaceTemplateManageDialogController(IMessageWindowService messageWindowService,
            TradeWorkspaceTemplateManageDialogContentVM pageViewModel,
            IEventAggregator eventAggregator,
            ITradeModuleService tradeModuleService,
            Lazy<ILoginUserManageService> loginUserManageService,
            ExportFactory<TradeWorkspaceTemplateEditDialogController> workspaceTemplateEditDialogCtrlFactory)
        {
            this.messageWindowService = messageWindowService;
            this.pageViewModel = pageViewModel;
            this.eventAggregator = eventAggregator;
            this.tradeModuleService = tradeModuleService;
            this.loginUserManageService = loginUserManageService;
            this.workspaceTemplateEditDialogCtrlFactory = workspaceTemplateEditDialogCtrlFactory;

            toEditItemCmd = new DelegateCommand(ToEditItem);
            toDeleteItemCmd = new DelegateCommand(ToDeleteItem);

            loginUserManageService.Value.HasLogouted += RecvUserHasLogouted;
        }

        public object DialogOwner { get; set; }

        public void Initialize()
        {
            // 模板列表
            var templateCollection = tradeModuleService.TradeWorkspaceDataRoot?.TradeWorkspaceTemplateListContainer?.Templates
                ?? new ObservableCollection<TradeTabWorkspaceTemplate>(); ;
            pageViewModel.TradeWorkspaceTemplates = new ReadOnlyObservableCollection<TradeTabWorkspaceTemplate>(templateCollection);

            pageViewModel.ToEditItemCmd = toEditItemCmd;
            pageViewModel.ToDeleteItemCmd = toDeleteItemCmd;
        }

        public void Run()
        {
            this.displayDialog = messageWindowService.CreateContentCustomWindow(DialogOwner, null, null, true, false, 
                true, "工作空间模板管理", pageViewModel.View);
            displayDialog.ShowDialog();
        }

        public void Shutdown()
        {
            if (displayDialog != null)
            {
                displayDialog.Close();
                displayDialog = null;
            }
            loginUserManageService.Value.HasLogouted -= RecvUserHasLogouted;
        }

        private void RecvUserHasLogouted(ProxyLoginResp lastLoginResp)
        {
            Shutdown();
        }

        private void ToEditItem(object obj)
        {
            if (obj is TradeTabWorkspaceTemplate toEditTemplate)
            {
                var editDialogCtrl = workspaceTemplateEditDialogCtrlFactory.CreateExport().Value;
                editDialogCtrl.DialogOwner = pageViewModel.DisplayInWindow;
                editDialogCtrl.DialogTitle = "修改模板信息";
                editDialogCtrl.InitialEditTemplate = toEditTemplate;

                editDialogCtrl.Initialize();
                editDialogCtrl.Run();
                editDialogCtrl.Shutdown();
            }
        }

        private void ToDeleteItem(object obj)
        {
            if (obj is TradeTabWorkspaceTemplate toDeleteTemplate)
            {
                var dataRoot = tradeModuleService.TradeWorkspaceDataRoot;
                if (dataRoot != null)
                {
                    dataRoot.TradeWorkspaceTemplateListContainer.Templates.Remove(toDeleteTemplate);
                }
            }
        }
    }
}
