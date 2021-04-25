using Manage.Applications.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using xueqiao.trade.hosting;
using XueQiaoFoundation.Shared.ControllerBase;
using XueQiaoFoundation.UI.Components.Navigation;

namespace Manage.Applications.Controllers
{
    /// <summary>
    /// 操作账户管理导航容器页面 controller
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class SubAccountManageNavContainerController : SwitchablePageControllerBase
    {
        private readonly SimpleNavigationContainerView navigationContainerView;
        private readonly SubAccountManagePageController subAccountManagePageController;
        private readonly ExportFactory<OrderRouteManagePageController> orderRouteManagePageCtrlFactory;
        private readonly ExportFactory<SubAccountInOutFundHistoryPageController> inOutFundHistoryPageCtrlFactory;

        private readonly List<OrderRouteManagePageController> orderRouteManagePageCtrls;
        private readonly List<SubAccountInOutFundHistoryPageController> inOutFundHistoryPageCtrls;

        [ImportingConstructor]
        public SubAccountManageNavContainerController(SimpleNavigationContainerView navigationContainerView,
            SubAccountManagePageController subAccountManagePageController,
            ExportFactory<OrderRouteManagePageController> orderRouteManagePageCtrlFactory,
            ExportFactory<SubAccountInOutFundHistoryPageController> inOutFundHistoryPageCtrlFactory)
        {
            this.navigationContainerView = navigationContainerView;
            this.subAccountManagePageController = subAccountManagePageController;
            this.orderRouteManagePageCtrlFactory = orderRouteManagePageCtrlFactory;
            this.inOutFundHistoryPageCtrlFactory = inOutFundHistoryPageCtrlFactory;

            orderRouteManagePageCtrls = new List<OrderRouteManagePageController>();
            inOutFundHistoryPageCtrls = new List<SubAccountInOutFundHistoryPageController>();
        }

        public object ContainerView => this.navigationContainerView;

        protected override void DoInitialize()
        {
            
        }

        protected override void DoRun()
        {
            subAccountManagePageController.ToShowOrderRoutePageHandler = this.ToShowSubUserOrderRoutePage;
            subAccountManagePageController.ToShowInOutFundHistoryPageHandler = this.ToShowInOutFundHistoryPage;
            subAccountManagePageController.Initialize();
            subAccountManagePageController.Run();

            navigationContainerView.Navigate(subAccountManagePageController.PageViewModel.View as Page);
        }

        protected override void DoShutdown()
        {
            subAccountManagePageController.Shutdown();

            foreach (var i in orderRouteManagePageCtrls)
            {
                i.Shutdown();
            }
            orderRouteManagePageCtrls.Clear();


            foreach (var i in inOutFundHistoryPageCtrls)
            {
                i.Shutdown();
            }
            inOutFundHistoryPageCtrls.Clear();
        }
        
        private void ToShowSubUserOrderRoutePage(HostingSubAccount subAccount)
        {
            if (subAccount == null) return;

            var pageCtrl = orderRouteManagePageCtrlFactory.CreateExport().Value;
            pageCtrl.TargetSubAccount = subAccount;
            pageCtrl.GoBackHandler = ctrl => 
            {
                navigationContainerView.GoBackIfPossible();
                orderRouteManagePageCtrls.Remove(ctrl);
                ctrl.Shutdown();
            };
            pageCtrl.Initialize();
            pageCtrl.Run();
            
            orderRouteManagePageCtrls.Add(pageCtrl);
            navigationContainerView.Navigate(pageCtrl.PageViewModel.View as Page);
        }

        private void ToShowInOutFundHistoryPage(HostingSubAccount subAccount)
        {
            if (subAccount == null) return;

            var pageCtrl = inOutFundHistoryPageCtrlFactory.CreateExport().Value;
            pageCtrl.TargetSubAccount = subAccount;
            pageCtrl.GoBackHandler = ctrl =>
            {
                navigationContainerView.GoBackIfPossible();
                inOutFundHistoryPageCtrls.Remove(ctrl);
                ctrl.Shutdown();
            };
            pageCtrl.Initialize();
            pageCtrl.Run();

            inOutFundHistoryPageCtrls.Add(pageCtrl);
            navigationContainerView.Navigate(pageCtrl.PageViewModel.View as Page);
        }
    }
}
