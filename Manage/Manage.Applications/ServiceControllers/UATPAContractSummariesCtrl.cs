using Manage.Applications.DataModels;
using Manage.Applications.Services;
using NativeModel.Trade;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.trade.hosting.proxy;
using XueQiaoFoundation.BusinessResources.Models;
using XueQiaoFoundation.Interfaces.Applications;
using XueQiaoFoundation.Interfaces.Helper;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;

namespace Manage.Applications.ServiceControllers
{
    /// <summary>
    /// 未分配成交以合约分组的概况管理 controller
    /// </summary>
    [Export, Export(typeof(IManageModuleSingletonController)), PartCreationPolicy(CreationPolicy.Shared)]
    internal class UATPAContractSummariesCtrl : IManageModuleSingletonController
    {
        private readonly UATPAService UATPAService;
        private readonly Lazy<ILoginUserManageService> loginUserManageService;
        private readonly UATPAContractSummaryService UATPAContractSummaryService;
        private readonly IContractItemTreeQueryController contractItemTreeQueryCtrl;
        private readonly IEventAggregator eventAggregator;

        [ImportingConstructor]
        public UATPAContractSummariesCtrl(
            UATPAService UATPAService,
            Lazy<ILoginUserManageService> loginUserManageService,
            UATPAContractSummaryService UATPAContractSummaryService,
            IContractItemTreeQueryController contractItemTreeQueryCtrl,
            IEventAggregator eventAggregator)
        {
            this.UATPAService = UATPAService;
            this.loginUserManageService = loginUserManageService;
            this.UATPAContractSummaryService = UATPAContractSummaryService;
            this.contractItemTreeQueryCtrl = contractItemTreeQueryCtrl;
            this.eventAggregator = eventAggregator;

            CollectionChangedEventManager.AddHandler(UATPAService.UATItems, UATItemsCollectionChanged);
            loginUserManageService.Value.HasLogouted += RecvUserHasLogouted;

            InvalidateUATContractGroupedSummaries();
            InvalidatePARelatedTotalVolumes();
        }

        public void Shutdown()
        {
            CollectionChangedEventManager.RemoveHandler(UATPAService.UATItems, UATItemsCollectionChanged);
            loginUserManageService.Value.HasLogouted -= RecvUserHasLogouted;
        }

        private void UATItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var newItems = e.NewItems?.Cast<UnAssignTradeDM>().ToArray();
            var oldItems = e.OldItems?.Cast<UnAssignTradeDM>().ToArray();

            if (newItems?.Any() == true)
            {
                foreach (var newItem in newItems)
                {
                    PropertyChangedEventManager.RemoveHandler(newItem, UATItemPropChanged, "");
                    PropertyChangedEventManager.AddHandler(newItem, UATItemPropChanged, "");
                }
            }

            if (oldItems?.Any() == true)
            {
                foreach (var oldItem in oldItems)
                {
                    PropertyChangedEventManager.RemoveHandler(oldItem, UATItemPropChanged, "");
                }
            }

            InvalidateUATContractGroupedSummaries();
            InvalidatePARelatedTotalVolumes();
        }
        
        private void UATItemPropChanged(object sender, PropertyChangedEventArgs e)
        {
            var unAssignTradeItem = sender as UnAssignTradeDM;
            if (unAssignTradeItem == null) return;
            if (e.PropertyName == nameof(UnAssignTradeDM.Volume)
                || e.PropertyName == nameof(UnAssignTradeDM.PreviewAssignVolume)
                || e.PropertyName == nameof(UnAssignTradeDM.UnpreviewAssignVolume))
            {
                InvalidateUATContractGroupedSummaryWithKey(new UATContractGroupedSummaryKey(unAssignTradeItem.FundAccountId, unAssignTradeItem.ContractId));
                InvalidatePARelatedTotalVolumes();
            }
        }

        private void RecvUserHasLogouted(ProxyLoginResp lastLoginResp)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() => 
            {
                UATPAContractSummaryService.ContractGroupedSummaries.Clear();
            });
        }

        /// <summary>
        /// 更新预分配、未预分配总数统计
        /// </summary>
        private void InvalidatePARelatedTotalVolumes()
        {
            var UATItems = UATPAService.UATItems.ToArray();
            UATPAContractSummaryService.PATotalVolume = UATItems.Sum(i => i.PreviewAssignVolume);
            UATPAContractSummaryService.UPATotalVolume = UATItems.Sum(i => i.UnpreviewAssignVolume);
        }

        private void InvalidateUATContractGroupedSummaries()
        {
            var UATItems = UATPAService.UATItems.ToArray();
            var UATContractGroupedSummaries = UATPAContractSummaryService.ContractGroupedSummaries.ToArray();
            
            var UATContractGroupedItems = UATItems.GroupBy(i => new UATContractGroupedSummaryKey(i.FundAccountId, i.ContractId));

            var rmUATContractGroupedSummaryKeys = UATContractGroupedSummaries.Select(i => new UATContractGroupedSummaryKey(i.FundAccountId, i.ContractId))
                .ToArray().Except(UATContractGroupedItems.Select(i=>i.Key).ToArray()).ToArray();

            // remove summaries if not exist
            UATPAContractSummaryService.ContractGroupedSummaries.RemoveAll(i => 
            {
                var _key = new UATContractGroupedSummaryKey(i.FundAccountId, i.ContractId);
                return rmUATContractGroupedSummaryKeys.Contains(_key);
            });

            // update exist item or add new item
            UATContractGroupedSummaries = UATPAContractSummaryService.ContractGroupedSummaries.ToArray();
            foreach (var UATContractGroupedItem in UATContractGroupedItems)
            {
                var fundAccountId = UATContractGroupedItem.Key.FundAccountId;
                var contractId = UATContractGroupedItem.Key.ContractId;
                var tarSummary = UATContractGroupedSummaries.FirstOrDefault(i => i.FundAccountId == fundAccountId && i.ContractId == contractId);
                if (tarSummary == null)
                {
                    tarSummary = new UATContractGroupedSummary(fundAccountId, contractId);
                    XueQiaoFoundationHelper.SetupTargetContract_ContractDetail(tarSummary.ContractDetailContainer, 
                        contractItemTreeQueryCtrl, XqContractNameFormatType.CommodityAcronym_Code_ContractCode);
                    UATPAContractSummaryService.ContractGroupedSummaries.Add(tarSummary);
                }
                InvalidateUATContractGroupedSummaryVolume(tarSummary, UATContractGroupedItem.ToArray());
            }
        }

        private void InvalidateUATContractGroupedSummaryWithKey(UATContractGroupedSummaryKey summaryKey)
        {
            if (summaryKey == null) return;

            var tarSummary = UATPAContractSummaryService.ContractGroupedSummaries.ToArray()
                .FirstOrDefault(i => i.FundAccountId == summaryKey.FundAccountId && i.ContractId == summaryKey.ContractId);
            if (tarSummary != null)
            {
                var tarUATItems = UATPAService.UATItems.ToArray()
                    .Where(i => i.FundAccountId == summaryKey.FundAccountId && i.ContractId == summaryKey.ContractId);
                InvalidateUATContractGroupedSummaryVolume(tarSummary, tarUATItems);
            }
        }

        private void InvalidateUATContractGroupedSummaryVolume(UATContractGroupedSummary summsary,
            IEnumerable<UnAssignTradeDM> unAssignTradeItems)
        {
            if (summsary == null) return;
            if (unAssignTradeItems == null) unAssignTradeItems = new UnAssignTradeDM[] { };

            // 统计买方向的数量
            var buyItems = unAssignTradeItems.Where(i => i.Direction == ClientTradeDirection.BUY).ToArray();
            summsary.BuyVolumeSummary.Volume = buyItems.Sum(i => i.Volume);
            summsary.BuyVolumeSummary.PreviewAssignVolume = buyItems.Sum(i => i.PreviewAssignVolume);
            summsary.BuyVolumeSummary.UnpreviewAssignVolume = buyItems.Sum(i => i.UnpreviewAssignVolume);

            // 统计卖方向的数量
            var sellItems = unAssignTradeItems.Except(buyItems).ToArray();
            summsary.SellVolumeSummary.Volume = sellItems.Sum(i => i.Volume);
            summsary.SellVolumeSummary.PreviewAssignVolume = sellItems.Sum(i => i.PreviewAssignVolume);
            summsary.SellVolumeSummary.UnpreviewAssignVolume = sellItems.Sum(i => i.UnpreviewAssignVolume);
        }
    }
}
