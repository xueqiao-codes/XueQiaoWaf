using Manage.Applications.DataModels;
using Manage.Applications.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.Shared.ControllerBase;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;

namespace Manage.Applications.Controllers
{
    /// <summary>
    /// `交易设置` tab页控制器
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class TabController_TradeSetting : IController
    {
        private readonly TradeSettingTabContentViewModel contentViewModel;
        private readonly ILoginDataService loginDataService;
        private readonly ExportFactory<FundAccountManagePageController> fundAccountManagePageCtrlFactory;
        private readonly ExportFactory<SubAccountManageNavContainerController> subAccountManageNavContainerCtrlFactory;
        private readonly ExportFactory<SubUserManagePageController> subUserManagePageCtrlFactory;

        private readonly Dictionary<ManageItemModel, SwitchablePageControllerBase> manageItemControllers = new Dictionary<ManageItemModel, SwitchablePageControllerBase>();
        
        [ImportingConstructor]
        public TabController_TradeSetting(TradeSettingTabContentViewModel contentViewModel, 
            ILoginDataService loginDataService,
            ExportFactory<FundAccountManagePageController> fundAccountManagePageCtrlFactory,
            ExportFactory<SubAccountManageNavContainerController> subAccountManageNavContainerCtrlFactory,
            ExportFactory<SubUserManagePageController> subUserManagePageCtrlFactory)
        {
            this.contentViewModel = contentViewModel;
            this.loginDataService = loginDataService;
            this.fundAccountManagePageCtrlFactory = fundAccountManagePageCtrlFactory;
            this.subAccountManageNavContainerCtrlFactory = subAccountManageNavContainerCtrlFactory;
            this.subUserManagePageCtrlFactory = subUserManagePageCtrlFactory;
        }


        public object ContentView => contentViewModel.View;

        public void Initialize()
        {
            PropertyChangedEventManager.AddHandler(contentViewModel, ContentViewModelPropertyChanged, "");
        }

        public void Run()
        {
            // 在这里添加所有管理的条目
            SwitchablePageControllerBase pageCtrl = null;
            ManageItemModel manageItem = null;

            // 资金账户
            pageCtrl = fundAccountManagePageCtrlFactory.CreateExport().Value;
            pageCtrl.Initialize();
            manageItem = new ManageItemModel { ItemTitle = "资金账户", ItemContentView = (pageCtrl as FundAccountManagePageController).PageViewModel.View };
            manageItemControllers[manageItem] = pageCtrl;
            contentViewModel.ManageItems.Add(manageItem);

            // 操作账户
            pageCtrl = subAccountManageNavContainerCtrlFactory.CreateExport().Value;
            pageCtrl.Initialize();
            manageItem = new ManageItemModel { ItemTitle = "操作账户", ItemContentView = (pageCtrl as SubAccountManageNavContainerController).ContainerView };
            manageItemControllers[manageItem] = pageCtrl;
            contentViewModel.ManageItems.Add(manageItem);

            // 用户
            pageCtrl = subUserManagePageCtrlFactory.CreateExport().Value;
            pageCtrl.Initialize();
            manageItem = new ManageItemModel { ItemTitle = "用户", ItemContentView = (pageCtrl as SubUserManagePageController).PageViewModel.View };
            manageItemControllers[manageItem] = pageCtrl;
            contentViewModel.ManageItems.Add(manageItem);

            contentViewModel.SelectedManageItem = contentViewModel.ManageItems.FirstOrDefault();
        }

        public void Shutdown()
        {
            PropertyChangedEventManager.AddHandler(contentViewModel, ContentViewModelPropertyChanged, "");

            foreach (var i in manageItemControllers.ToArray())
            {
                i.Value.Shutdown();
            }
            manageItemControllers.Clear();
        }

        private void ContentViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TradeSettingTabContentViewModel.SelectedManageItem))
            {
                var selectedManageItem = contentViewModel.SelectedManageItem;
                if (selectedManageItem != null
                    && manageItemControllers.TryGetValue(selectedManageItem, out SwitchablePageControllerBase pageCtrl))
                {
                    pageCtrl.Run();
                }
            }
        }
    }
}
