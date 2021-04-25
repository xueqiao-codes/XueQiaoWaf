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
using xueqiao.trade.hosting;
using xueqiao.trade.hosting.proxy;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.BusinessResources.Helpers;
using XueQiaoFoundation.BusinessResources.Models;
using XueQiaoFoundation.Interfaces.Applications;
using XueQiaoFoundation.Interfaces.Helper;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;
using XueQiaoWaf.Trade.Modules.Applications.ViewModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class CreateComposeWithSameTemplateDialogController : IController
    {
        private readonly IMessageWindowService messageWindowService;
        private readonly IEventAggregator eventAggregator;
        private readonly IComposeGraphCacheController composeGraphCacheCtrl;
        private readonly IComposeGraphQueryController composeGraphQueryCtrl;
        private readonly IUserComposeViewCacheController userComposeViewCacheCtrl;
        private readonly IContractItemTreeQueryController contractItemTreeQueryCtrl;
        private readonly ILoginDataService loginDataService;
        private readonly Lazy<ILoginUserManageService> loginUserManageService;
        private readonly CreateComposeWithSameTemplateVM pageViewModel;

        private readonly DelegateCommand createWithTemplateCmd;

        private IMessageWindow dialog;

        [ImportingConstructor]
        public CreateComposeWithSameTemplateDialogController(
            IMessageWindowService messageWindowService,
            IEventAggregator eventAggregator,
            IComposeGraphCacheController composeGraphCacheCtrl,
            IComposeGraphQueryController composeGraphQueryCtrl,
            IUserComposeViewCacheController userComposeViewCacheCtrl,
            IContractItemTreeQueryController contractItemTreeQueryCtrl,
            ILoginDataService loginDataService,
            Lazy<ILoginUserManageService> loginUserManageService,
            CreateComposeWithSameTemplateVM pageViewModel)
        {
            this.messageWindowService = messageWindowService;
            this.eventAggregator = eventAggregator;
            this.composeGraphCacheCtrl = composeGraphCacheCtrl;
            this.composeGraphQueryCtrl = composeGraphQueryCtrl;
            this.userComposeViewCacheCtrl = userComposeViewCacheCtrl;
            this.contractItemTreeQueryCtrl = contractItemTreeQueryCtrl;
            this.loginDataService = loginDataService;
            this.loginUserManageService = loginUserManageService;
            this.pageViewModel = pageViewModel;

            this.createWithTemplateCmd = new DelegateCommand(CreateWithTemplate);

            loginUserManageService.Value.HasLogouted += RecvUserHasLogouted;
        }

        public object DialogOwner { get; set; }

        public HostingComposeGraph TemplateComposeGraph { get; set; }

        public string MyAliasName { get; set; }

        public short MyPrecisionNumber { get; set; }
        
        /// <summary>
        /// 完成创建的组合 id
        /// </summary
        public long? CreatedComposeId { get; private set; }
        
        /// <summary>
        /// 创建的组合视图是否来自云引用
        /// </summary>
        public bool? IsComposeViewCreatedByReference { get; private set; }

        public void Initialize()
        {
            if (TemplateComposeGraph == null) throw new ArgumentNullException("TemplateComposeGraph");
            if (string.IsNullOrEmpty(MyAliasName)) throw new ArgumentException("MyAliasName can't be null or empty.");

            var detailContainer = new TargetCompose_ComposeDetail(TemplateComposeGraph.ComposeGraphId);
            XueQiaoFoundationHelper.SetupTargetCompose_ComposeDetail(detailContainer,
                composeGraphCacheCtrl, composeGraphQueryCtrl,
                userComposeViewCacheCtrl, contractItemTreeQueryCtrl,
                XqContractNameFormatType.CommodityAcronym_Code_ContractCode);

            pageViewModel.TemplateComposeDetailContainer = detailContainer;
            pageViewModel.CreateWithTemplateCmd = createWithTemplateCmd;
        }

        public void Run()
        {
            dialog = messageWindowService.CreateContentCustomWindow(DialogOwner, null, null, false, false, 
                true, "创建组合提醒", pageViewModel.View);
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

        private void CreateWithTemplate()
        {
            var landingInfo = loginDataService.LandingInfo;
            if (landingInfo == null) return;
            
            var templateCompose = TemplateComposeGraph;

            pageViewModel.IsCreating = true;

            var shareComposeId = templateCompose.ComposeGraphId;
            var task = XqThriftLibConfigurationManager.SharedInstance.TradeHostingTerminalAoHttpStub.addComposeViewBySearchAsync(landingInfo, shareComposeId,
                templateCompose.ComposeGraphKey, MyAliasName, MyPrecisionNumber, CancellationToken.None);
            task.ContinueWith(t => 
            {
                var createResp = t.Result;
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    pageViewModel.IsCreating = false;
                    if (createResp == null || createResp.SourceException != null)
                    {
                        var currentWin = GetCurrentWindow();
                        if (currentWin != null)
                        {
                            var errMsg = FoundationHelper.FormatResponseDisplayErrorMsg(createResp, "创建组合失败！\n");
                            messageWindowService.ShowMessageDialog(currentWin, null, null, null, errMsg);
                        }
                        return;
                    }

                    //成功
                    this.CreatedComposeId = shareComposeId;
                    this.IsComposeViewCreatedByReference = true;
                    InternalCloseDialog();
                });
            });
        }


        private object GetCurrentWindow()
        {
            return UIHelper.GetWindowOfUIElement(pageViewModel.View);
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
