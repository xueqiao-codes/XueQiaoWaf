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
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class PersonalUserManageModuleRootViewCtrl : IController
    {
        private readonly ManageModuleRootVM contentVM;
        private readonly ExportFactory<PersonalUserTradeAccountManageViewCtrl> tradeAccountManageViewCtrlFactory;

        private readonly Dictionary<SimpleTabItem, IController> tabWorkspaceControllers
            = new Dictionary<SimpleTabItem, IController>();

        private SimpleTabItem workspaceItem_TradeAccountManage;

        [ImportingConstructor]
        public PersonalUserManageModuleRootViewCtrl(ManageModuleRootVM contentVM,
            ExportFactory<PersonalUserTradeAccountManageViewCtrl> tradeAccountManageViewCtrlFactory)
        {
            this.contentVM = contentVM;
            this.tradeAccountManageViewCtrlFactory = tradeAccountManageViewCtrlFactory;
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
            workspaceItem_TradeAccountManage = new SimpleTabItem { Header = "账户" };
            contentVM.WorkspaceItems.Add(workspaceItem_TradeAccountManage);
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
                if (activeWorkspaceItem == workspaceItem_TradeAccountManage)
                {
                    ShowWorkspaceContent_TradeAccountManage();
                }
                else
                {
                    // do other  
                }
            }
        }

        private void ShowWorkspaceContent_TradeAccountManage()
        {
            if (workspaceItem_TradeAccountManage == null) return;

            tabWorkspaceControllers.TryGetValue(workspaceItem_TradeAccountManage, out IController controller);
            var tabWorkspaceCtrl = controller as PersonalUserTradeAccountManageViewCtrl;
            if (tabWorkspaceCtrl == null)
            {
                tabWorkspaceCtrl = tradeAccountManageViewCtrlFactory.CreateExport().Value;
                tabWorkspaceCtrl.Initialize();
                tabWorkspaceCtrl.Run();

                tabWorkspaceControllers[workspaceItem_TradeAccountManage] = tabWorkspaceCtrl;
            }

            if (workspaceItem_TradeAccountManage.ContentView == null)
            {
                workspaceItem_TradeAccountManage.ContentView = tabWorkspaceCtrl.ContentView;
            }
        }
    }
}
