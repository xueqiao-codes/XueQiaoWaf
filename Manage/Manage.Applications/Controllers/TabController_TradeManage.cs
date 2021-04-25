using Manage.Applications.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;

namespace Manage.Applications.Controllers
{
    /// <summary>
    /// `交易管理` tab 页控制器
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class TabController_TradeManage : IController
    {
        private readonly TradeManageTabContentViewModel contentViewModel;
        private readonly ILoginDataService loginDataService;
        private readonly ExportFactory<FundManageByFundAccountController> fundManageByFundAccountCtrlFactory;
        private readonly ExportFactory<FundManageBySubAccountController> fundManageBySubAccountCtrlFactory;
        private readonly ExportFactory<PositionShowByFundAccountCtrl> positionShowByFundAccountCtrlFactory;
        private readonly ExportFactory<PositionShowBySubAccountCtrl> positionShowBySubAccountCtrlFactory;
        private readonly ExportFactory<PositionVerifyManageCtrl> positionVerifyManageCtrlFactory;
        private readonly ExportFactory<UATManageCtrl> UATManageCtrlFactory;
        private readonly ExportFactory<FundAccountSBCtrl> fundAccountSBCtrlFactory;
        private readonly ExportFactory<SubAccountSBCtrl> subAccountSBCtrlFactory;

        private readonly DelegateCommand fundManageByFundAccountEntryCmd;
        private readonly DelegateCommand fundManageBySubAccountEntryCmd;
        private readonly DelegateCommand positionManageByFundAccountEntryCmd;
        private readonly DelegateCommand positionManageBySubAccountEntryCmd;
        private readonly DelegateCommand notAssignTradeManageEntryCmd;
        private readonly DelegateCommand positionVerifyManageEntryCmd;
        private readonly DelegateCommand settlementFundAccountEntryCmd;
        private readonly DelegateCommand settlementSubAccountEntryCmd;

        private readonly List<IController> entryItemCtrls = new List<IController>();

        [ImportingConstructor]
        public TabController_TradeManage(TradeManageTabContentViewModel contentViewModel,
            ILoginDataService loginDataService,
            ExportFactory<FundManageByFundAccountController> fundManageByFundAccountCtrlFactory,
            ExportFactory<FundManageBySubAccountController> fundManageBySubAccountCtrlFactory,
            ExportFactory<PositionShowByFundAccountCtrl> positionShowByFundAccountCtrlFactory,
            ExportFactory<PositionShowBySubAccountCtrl> positionShowBySubAccountCtrlFactory,
            ExportFactory<PositionVerifyManageCtrl> positionVerifyManageCtrlFactory,
            ExportFactory<UATManageCtrl> UATManageCtrlFactory,
            ExportFactory<FundAccountSBCtrl> fundAccountSBCtrlFactory,
            ExportFactory<SubAccountSBCtrl> subAccountSBCtrlFactory)
        {
            this.contentViewModel = contentViewModel;
            this.loginDataService = loginDataService;
            this.fundManageByFundAccountCtrlFactory = fundManageByFundAccountCtrlFactory;
            this.fundManageBySubAccountCtrlFactory = fundManageBySubAccountCtrlFactory;
            this.positionShowByFundAccountCtrlFactory = positionShowByFundAccountCtrlFactory;
            this.positionShowBySubAccountCtrlFactory = positionShowBySubAccountCtrlFactory;
            this.positionVerifyManageCtrlFactory = positionVerifyManageCtrlFactory;
            this.UATManageCtrlFactory = UATManageCtrlFactory;
            this.fundAccountSBCtrlFactory = fundAccountSBCtrlFactory;
            this.subAccountSBCtrlFactory = subAccountSBCtrlFactory;

            fundManageByFundAccountEntryCmd = new DelegateCommand(EnterPage_FundManageByFundAccount);
            fundManageBySubAccountEntryCmd = new DelegateCommand(EnterPage_FundManageBySubAccount);
            positionManageByFundAccountEntryCmd = new DelegateCommand(EnterPage_PositionManageByFundAccount);
            positionManageBySubAccountEntryCmd = new DelegateCommand(EnterPage_PositionManageBySubAccount);
            positionVerifyManageEntryCmd = new DelegateCommand(EnterPage_PositionVerifyManage);
            notAssignTradeManageEntryCmd = new DelegateCommand(EnterPage_NotAssignTradeManage);
            settlementFundAccountEntryCmd = new DelegateCommand(EnterPage_SettlementFundAccount);
            settlementSubAccountEntryCmd = new DelegateCommand(EnterPage_SettlementSubAccount);
        }
        
        public object ContentView => contentViewModel.View;

        public void Initialize()
        {
            contentViewModel.FundManageByFundAccountEntryCmd = fundManageByFundAccountEntryCmd;
            contentViewModel.FundManageBySubAccountEntryCmd = fundManageBySubAccountEntryCmd;
            contentViewModel.PositionManageByFundAccountEntryCmd = positionManageByFundAccountEntryCmd;
            contentViewModel.PositionManageBySubAccountEntryCmd = positionManageBySubAccountEntryCmd;
            contentViewModel.PositionVerifyManageEntryCmd = positionVerifyManageEntryCmd;
            contentViewModel.NotAssignTradeManageEntryCmd = notAssignTradeManageEntryCmd;
            contentViewModel.SettlementFundAccountEntryCmd = settlementFundAccountEntryCmd;
            contentViewModel.SettlementSubAccountEntryCmd = settlementSubAccountEntryCmd;
        }

        public void Run()
        {

        }

        public void Shutdown()
        {
            foreach (var ctrl in entryItemCtrls)
            {
                ctrl.Shutdown();
            }
            entryItemCtrls.Clear();
        }

        private void EnterPage_FundManageByFundAccount()
        {
            var tarCtrl = entryItemCtrls.FirstOrDefault(i => i is FundManageByFundAccountController) as FundManageByFundAccountController;
            if (tarCtrl == null)
            {
                tarCtrl = fundManageByFundAccountCtrlFactory.CreateExport().Value;
                tarCtrl.Initialize();
                tarCtrl.Run();

                entryItemCtrls.Add(tarCtrl);
            }
            contentViewModel.EntryContentView = tarCtrl.ContentView;
        }

        private void EnterPage_FundManageBySubAccount()
        {
            var tarCtrl = entryItemCtrls.FirstOrDefault(i => i is FundManageBySubAccountController) as FundManageBySubAccountController;
            if (tarCtrl == null)
            {
                tarCtrl = fundManageBySubAccountCtrlFactory.CreateExport().Value;
                tarCtrl.Initialize();
                tarCtrl.Run();

                entryItemCtrls.Add(tarCtrl);
            }
            contentViewModel.EntryContentView = tarCtrl.ContentView;
        }

        private void EnterPage_PositionManageByFundAccount()
        {
            var tarCtrl = entryItemCtrls.FirstOrDefault(i => i is PositionShowByFundAccountCtrl) as PositionShowByFundAccountCtrl;
            if (tarCtrl == null)
            {
                tarCtrl = positionShowByFundAccountCtrlFactory.CreateExport().Value;
                tarCtrl.Initialize();
                tarCtrl.Run();

                entryItemCtrls.Add(tarCtrl);
            }
            contentViewModel.EntryContentView = tarCtrl.ContentView;
        }

        private void EnterPage_PositionManageBySubAccount()
        {
            var tarCtrl = entryItemCtrls.FirstOrDefault(i => i is PositionShowBySubAccountCtrl) as PositionShowBySubAccountCtrl;
            if (tarCtrl == null)
            {
                tarCtrl = positionShowBySubAccountCtrlFactory.CreateExport().Value;
                tarCtrl.Initialize();
                tarCtrl.Run();

                entryItemCtrls.Add(tarCtrl);
            }
            contentViewModel.EntryContentView = tarCtrl.ContentView;
        }

        private void EnterPage_PositionVerifyManage()
        {
            var tarCtrl = entryItemCtrls.FirstOrDefault(i => i is PositionVerifyManageCtrl) as PositionVerifyManageCtrl;
            if (tarCtrl == null)
            {
                tarCtrl = positionVerifyManageCtrlFactory.CreateExport().Value;
                tarCtrl.Initialize();
                tarCtrl.Run();

                entryItemCtrls.Add(tarCtrl);
            }
            else
            {
                tarCtrl.RefreshPageDataIfNeed();
            }
            contentViewModel.EntryContentView = tarCtrl.ContentView;
        }

        private void EnterPage_NotAssignTradeManage()
        {
            var tarCtrl = entryItemCtrls.FirstOrDefault(i => i is UATManageCtrl) as UATManageCtrl;
            if (tarCtrl == null)
            {
                tarCtrl = UATManageCtrlFactory.CreateExport().Value;
                tarCtrl.Initialize();
                tarCtrl.Run();

                entryItemCtrls.Add(tarCtrl);
            }
            else
            {
                tarCtrl.RefreshPageDataIfNeed();
            }
            contentViewModel.EntryContentView = tarCtrl.ContentView;
        }
        
        private void EnterPage_SettlementFundAccount()
        {
            var tarCtrl = entryItemCtrls.FirstOrDefault(i => i is FundAccountSBCtrl) as FundAccountSBCtrl;
            if (tarCtrl == null)
            {
                tarCtrl = fundAccountSBCtrlFactory.CreateExport().Value;
                tarCtrl.Initialize();
                tarCtrl.Run();

                entryItemCtrls.Add(tarCtrl);
            }
            contentViewModel.EntryContentView = tarCtrl.ContentView;
        }

        private void EnterPage_SettlementSubAccount()
        {
            var tarCtrl = entryItemCtrls.FirstOrDefault(i => i is SubAccountSBCtrl) as SubAccountSBCtrl;
            if (tarCtrl == null)
            {
                tarCtrl = subAccountSBCtrlFactory.CreateExport().Value;
                tarCtrl.Initialize();
                tarCtrl.Run();

                entryItemCtrls.Add(tarCtrl);
            }
            contentViewModel.EntryContentView = tarCtrl.ContentView;
        }
    }
}
