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
using System.Windows;
using System.Windows.Controls.Primitives;
using TimeZoneConverter;
using xueqiao.contract;
using xueqiao.contract.standard;
using xueqiao.trade.hosting.proxy;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.BusinessResources.Models;
using XueQiaoFoundation.Interfaces.Applications;
using XueQiaoFoundation.Interfaces.Helper;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoFoundation.UI.Components.Popup;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;

using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.ViewModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class ContractInfoDialogController : IController
    {
        private readonly ContractInfoContentVM contentVM;
        private readonly IContractItemTreeQueryController contractItemTreeQueryController;
        private readonly IMessageWindowService messageWindowService;
        private readonly IEventAggregator eventAggregator;
        private readonly Lazy<ILoginUserManageService> loginUserManageService;
        private readonly ICurrencyUnitsService currencyUnitsService;
        private readonly ExportFactory<IPopup> popupFactory;
        private readonly ExportFactory<ChildrenContractDetailVM> childrenContractDetailVMFactory;

        private readonly DelegateCommand triggerShowChildrenContractInfoCmd;
        private IMessageWindow detailDialog;

        private ContractBasicInfoDM contractBasicInfo;

        [ImportingConstructor]
        public ContractInfoDialogController(ContractInfoContentVM contentVM,
            IContractItemTreeQueryController contractItemTreeQueryController,
            IMessageWindowService messageWindowService,
            IEventAggregator eventAggregator,
            Lazy<ILoginUserManageService> loginUserManageService,
            ICurrencyUnitsService currencyUnitsService,
            ExportFactory<IPopup> popupFactory,
            ExportFactory<ChildrenContractDetailVM> childrenContractDetailVMFactory)
        {
            this.contentVM = contentVM;
            this.contractItemTreeQueryController = contractItemTreeQueryController;
            this.messageWindowService = messageWindowService;
            this.eventAggregator = eventAggregator;
            this.loginUserManageService = loginUserManageService;
            this.currencyUnitsService = currencyUnitsService;
            this.popupFactory = popupFactory;
            this.childrenContractDetailVMFactory = childrenContractDetailVMFactory;

            triggerShowChildrenContractInfoCmd = new DelegateCommand(TriggerShowChildrenContractInfo);

            loginUserManageService.Value.HasLogouted += RecvUserHasLogouted;
        }

        /// <summary>
        /// 目标合约 id
        /// </summary>
        public int TargetContractId { get; set; }

        public object DialogOwner { get; set; }
        public Point? DialogShowLocationRelativeToScreen { get; set; }

        private SledCommodityConfig GetCommodityConfig(TargetContract_TargetContractDetail contractDetail)
        {
            if (contractDetail == null) return null;
            var currentTimestamp = (int)DateHelper.NowUnixTimeSpan().TotalSeconds;
            var commodityConfig = contractDetail.CommodityDetail?.Configs?
                .FirstOrDefault(i => i.ActiveStartTimestamp <= currentTimestamp && i.ActiveEndTimestamp >= currentTimestamp);
            return commodityConfig;
        }

        public void Initialize()
        {
            this.contractBasicInfo = new ContractBasicInfoDM();
            this.contractBasicInfo.CurrencyUnitsService = this.currencyUnitsService;
            this.contractBasicInfo.ContractDetailContainer = new TargetContract_TargetContractDetail(TargetContractId);
                        
            contentVM.TriggerShowChildrenContractInfoCmd = triggerShowChildrenContractInfoCmd;
            contentVM.ContractDetailContainer = this.contractBasicInfo.ContractDetailContainer;
            contentVM.ContractBasicInfo = this.contractBasicInfo;

            // 加载详情
            XueQiaoFoundationHelper.SetupTargetContract_ContractDetail(this.contractBasicInfo.ContractDetailContainer,
                contractItemTreeQueryController, XqContractNameFormatType.CommodityAcronym_Code_ContractCode,
                _detail =>
                {
                    this.contractBasicInfo.CommodityConfig = GetCommodityConfig(_detail);
                    RefreshTradeTimeInfo();
                });
        }

        public void Run()
        {
            detailDialog = messageWindowService.CreateContentCustomWindow(DialogOwner, DialogShowLocationRelativeToScreen, null, true, false,
                true, "合约信息", contentVM.View);
            detailDialog.ShowDialog();
        }

        public void Shutdown()
        {
            if (detailDialog != null)
            {
                detailDialog.Close();
                detailDialog = null;
            }

            loginUserManageService.Value.HasLogouted -= RecvUserHasLogouted;
        }

        private async void RefreshTradeTimeInfo()
        {
            var commodityTimeZoneId = this.contractBasicInfo.ContractDetailContainer.CommodityDetail?.ZoneId;
            var commodityId = this.contractBasicInfo.ContractDetailContainer.CommodityDetail?.SledCommodityId;
            if (commodityId == null) return;

            var destTimeZoneId = "Asia/Shanghai";
            var destTimeZone = TZConvert.GetTimeZoneInfo(destTimeZoneId);
            // Convert to datetimeoffset
            DateTimeOffset destZoneTodayDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, destTimeZone).Date;

            // or DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified)

            var option = new ReqSledTradeTimeOption
            {
                SledCommodityIds = new List<int> { commodityId.Value }
            };
            var resp = await XqThriftLibConfigurationManager.SharedInstance.ContractOnlineServiceHttpStub.reqSledTradeTimeAsync(option, 0, 1, CancellationToken.None);

            var contractTradeTime = resp.CorrectResult?.Page?.FirstOrDefault(i=>i.SledCommodityId == commodityId);
            if (contractTradeTime == null) return;
            
            var tradeTimeSpanDailyDetails = ContractTradeTimeSpanDailyDetail.GenerateDailyDetails(contractTradeTime.DateTimeSpans, commodityTimeZoneId, destTimeZoneId)?
                .Where(i => i.DestZoneDailyDataItem.Date.Date >= destZoneTodayDate.Date && i.DestZoneDailyDataItem.Date.Date < (destZoneTodayDate.Date + new TimeSpan(7,0,0,0)))
                .ToArray();
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                contentVM.TradeTimeSpanDailyDetails.Clear();
                contentVM.TradeTimeSpanDailyDetails.AddRange(tradeTimeSpanDailyDetails);
            });
        }

        private void RecvUserHasLogouted(ProxyLoginResp lastLoginResp)
        {
            Shutdown();
        }

        private void TriggerShowChildrenContractInfo(object obj)
        {
            var triggerElement = obj as UIElement;
            if (triggerElement == null) return;

            var relatedContractDetails = this.contractBasicInfo.ContractDetailContainer.RelatedContractDetails;
            if (relatedContractDetails?.Any() != true) return;

            var childrenContractDetailVM = childrenContractDetailVMFactory.CreateExport().Value;
            var childrenContractBasicInfos = new List<ContractBasicInfoDM>();
            var currentTimestamp = (int)DateHelper.NowUnixTimeSpan().TotalSeconds;
            foreach (var _childContractContainer in relatedContractDetails)
            {
                var basicInfo = new ContractBasicInfoDM();
                basicInfo.CurrencyUnitsService = this.currencyUnitsService;
                basicInfo.ContractDetailContainer = _childContractContainer;
                XueQiaoFoundationHelper.SetupDisplayNamesForContractContainer(_childContractContainer, XqContractNameFormatType.CommodityAcronym_Code_ContractCode);
                basicInfo.CommodityConfig = GetCommodityConfig(_childContractContainer);
                childrenContractBasicInfos.Add(basicInfo);
            }
            childrenContractDetailVM.ChildrenContractBasicInfos.Clear();
            childrenContractDetailVM.ChildrenContractBasicInfos.AddRange(childrenContractBasicInfos);

            //var dialog = messageWindowService.CreateContentCustomWindow(DialogOwner, null, null, false, false, true, "详情", childrenContractDetailVM.View);
            //dialog.ShowDialog();

            var popup = popupFactory.CreateExport().Value;
            popup.Placement = PlacementMode.Bottom;
            popup.PlacementTarget = triggerElement;
            popup.StaysOpen = false;

            popup.Content = childrenContractDetailVM.View as UIElement;
            popup.Open();
        }
    }
}
