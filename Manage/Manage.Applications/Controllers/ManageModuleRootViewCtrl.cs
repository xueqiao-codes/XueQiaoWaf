using Manage.Applications.ServiceControllers;
using Manage.Applications.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.Shared.Model;

namespace Manage.Applications.Controllers
{
    /// <summary>
    /// 管理导航页面管理
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class ManageModuleRootViewCtrl : IController
    {
        private readonly ManageModuleRootVM contentVM;
        private readonly ExportFactory<TabController_TradeSetting> tradeSettingCtrlFactory;
        private readonly ExportFactory<TabController_TradeManage> tradeManageCtrlFactory;
        private readonly IManageFundAccountItemsController manageFundAccountItemsCtrl;
        private readonly IManageSubAccountItemsController manageSubAccountItemsCtrl;

        private SimpleTabItem item_TradeSetting;
        private SimpleTabItem item_TradeManage;

        private readonly Dictionary<SimpleTabItem, IController> tabWorkspaceControllers 
            = new Dictionary<SimpleTabItem, IController>();
        
        [ImportingConstructor]
        public ManageModuleRootViewCtrl(ManageModuleRootVM contentVM, 
            ExportFactory<TabController_TradeSetting> tradeSettingCtrlFactory,
            ExportFactory<TabController_TradeManage> tradeManageCtrlFactory,
            IManageFundAccountItemsController manageFundAccountItemsCtrl,
            IManageSubAccountItemsController manageSubAccountItemsCtrl)
        {
            this.contentVM = contentVM;
            this.tradeSettingCtrlFactory = tradeSettingCtrlFactory;
            this.tradeManageCtrlFactory = tradeManageCtrlFactory;
            this.manageFundAccountItemsCtrl = manageFundAccountItemsCtrl;
            this.manageSubAccountItemsCtrl = manageSubAccountItemsCtrl;

        }

        public ChromeWindowCaptionDataHolder EmbedInWindowCaptionDataHolder { get; set; }

        public object ContentView => contentVM.View;

        public void Initialize()
        {
            contentVM.EmbedInWindowCaptionDataHolder = this.EmbedInWindowCaptionDataHolder;
            PropertyChangedEventManager.AddHandler(contentVM, ContentVMPropChanged, "");
        }

        public void Run()
        {
            // 在这里添加所有管理的 tab 页条目

            // 交易设置
            item_TradeSetting = new SimpleTabItem { Header = "交易设置" };
            contentVM.WorkspaceItems.Add(item_TradeSetting);

            // 交易管理
            item_TradeManage = new SimpleTabItem { Header = "交易管理" };
            contentVM.WorkspaceItems.Add(item_TradeManage);

            contentVM.ActiveWorkspaceItem = contentVM.WorkspaceItems.FirstOrDefault();
        }

        public void Shutdown()
        {
            PropertyChangedEventManager.RemoveHandler(contentVM, ContentVMPropChanged, "");

            foreach (var i in tabWorkspaceControllers.ToArray())
            {
                i.Value.Shutdown();
            }
            tabWorkspaceControllers.Clear();
        }

        private void ContentVMPropChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ManageModuleRootVM.ActiveWorkspaceItem))
            {
                var activeWorkspaceItem = contentVM.ActiveWorkspaceItem;
                if (activeWorkspaceItem == item_TradeSetting)
                {
                    ShowWorkspaceContent_TradeSetting();
                }
                else if (activeWorkspaceItem == item_TradeManage)
                {
                    ShowWorkspaceContent_TradeManage();

                    // 切换到`交易管理` tab 时强制刷新资金账号、子账号列表数据
                    manageFundAccountItemsCtrl.RefreshFundAccountItemsForce();
                    manageSubAccountItemsCtrl.RefreshSubAccountItemsForce();
                }
            }
        }

        private void ShowWorkspaceContent_TradeSetting()
        {
            if (item_TradeSetting == null) return;

            tabWorkspaceControllers.TryGetValue(item_TradeSetting, out IController controller);
            var tabWorkspaceCtrl = controller as TabController_TradeSetting;
            if (tabWorkspaceCtrl == null)
            {
                tabWorkspaceCtrl = tradeSettingCtrlFactory.CreateExport().Value;
                tabWorkspaceCtrl.Initialize();
                tabWorkspaceCtrl.Run();
                
                tabWorkspaceControllers[item_TradeSetting] = tabWorkspaceCtrl;
            }

            if (item_TradeSetting.ContentView == null)
            {
                item_TradeSetting.ContentView = tabWorkspaceCtrl.ContentView;
            }
        }

        private void ShowWorkspaceContent_TradeManage()
        {
            if (item_TradeManage == null) return;

            tabWorkspaceControllers.TryGetValue(item_TradeManage, out IController controller);
            var tabWorkspaceCtrl = controller as TabController_TradeManage;
            if (tabWorkspaceCtrl == null)
            {
                tabWorkspaceCtrl = tradeManageCtrlFactory.CreateExport().Value;
                tabWorkspaceCtrl.Initialize();
                tabWorkspaceCtrl.Run();

                tabWorkspaceControllers[item_TradeManage] = tabWorkspaceCtrl;
            }

            if (item_TradeManage.ContentView == null)
            {
                item_TradeManage.ContentView = tabWorkspaceCtrl.ContentView;
            }
        }
    }
}
