﻿using business_foundation_lib.xq_thriftlib_config;
using IDLAutoGenerated.Util;
using lib.xqclient_base.thriftapi_mediation.Interface;
using Manage.Applications.DataModels;
using Manage.Applications.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Waf.Applications;
using xueqiao.trade.hosting;
using xueqiao.trade.hosting.asset.thriftapi;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.BusinessResources.Helpers;
using XueQiaoFoundation.BusinessResources.Models;
using XueQiaoFoundation.Interfaces.Applications;
using XueQiaoFoundation.Interfaces.Helper;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.UI.Components.ListPager;
using XueQiaoFoundation.UI.Components.ListPager.ViewModels;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;

namespace Manage.Applications.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class SubAccountPositionHistoryCtrl : IController
    {
        private const int PositionItemsQueryPageSize = 50;

        private readonly SubAccountPositionHistoryVM contentViewModel;
        private readonly SimplePagerViewModel positionItemsPagerVM;
        private readonly ILoginDataService loginDataService;
        private readonly IContractItemTreeQueryController contractItemTreeQueryCtrl;
        private readonly ExportFactory<SubAccountPositionDetailDialogCtrl> positionDetailDialogCtrlFactory;

        private readonly DelegateCommand pageGoBackCmd;
        private readonly DelegateCommand toShowPositionDetailCmd;
        private readonly DelegateCommand jumpToPositionItemsPageCmd;

        private bool isRefreshingData;

        [ImportingConstructor]
        public SubAccountPositionHistoryCtrl(
            SubAccountPositionHistoryVM contentViewModel,
            SimplePagerViewModel positionItemsPagerVM,
            ILoginDataService loginDataService,
            IContractItemTreeQueryController contractItemTreeQueryCtrl,
            ExportFactory<SubAccountPositionDetailDialogCtrl> positionDetailDialogCtrlFactory)
        {
            this.contentViewModel = contentViewModel;
            this.positionItemsPagerVM = positionItemsPagerVM;
            this.loginDataService = loginDataService;
            this.contractItemTreeQueryCtrl = contractItemTreeQueryCtrl;
            this.positionDetailDialogCtrlFactory = positionDetailDialogCtrlFactory;

            pageGoBackCmd = new DelegateCommand(() => this.PageGoBackHandler?.Invoke(this));
            toShowPositionDetailCmd = new DelegateCommand(ToShowPositionDetail);
            jumpToPositionItemsPageCmd = new DelegateCommand(JumpToPositionItemsPage, CanJumpToPositionItemsPage);
        }

        public HostingSubAccount SubAccount { get; set; }

        /// <summary>
        /// 页面后退处理
        /// </summary>
        public Action<SubAccountPositionHistoryCtrl> PageGoBackHandler { get; set; }

        public object PageView => contentViewModel.View;

        public void Initialize()
        {
            if (SubAccount == null) throw new ArgumentNullException("SubAccount");

            contentViewModel.SubAccount = SubAccount;
            contentViewModel.PageGoBackCmd = pageGoBackCmd;
            positionItemsPagerVM.GoJumpPageCmd = jumpToPositionItemsPageCmd;
        }

        public void Run()
        {
            contentViewModel.SelectedDate = DateTime.Now.Date;
            RefreshPageData();
            PropertyChangedEventManager.AddHandler(contentViewModel, ContentViewModelPropChanged, "");
        }

        public void Shutdown()
        {
            PropertyChangedEventManager.RemoveHandler(contentViewModel, ContentViewModelPropChanged, "");
            PageGoBackHandler = null;
        }

        private void ContentViewModelPropChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SubAccountPositionHistoryVM.SelectedDate))
            {
                if (contentViewModel.SelectedDate != null)
                {
                    RefreshPageData();
                }
            }
        }
        
        private void ToShowPositionDetail(object obj)
        {
            var dmItem = obj as SubAccHistoryPositionDM;
            if (dmItem == null) return;
            
            // show detail
            var dialogCtrl = positionDetailDialogCtrlFactory.CreateExport().Value;
            dialogCtrl.SubAccount = this.SubAccount;
            dialogCtrl.ContractId = dmItem.PositionContent.ContractId;
            dialogCtrl.HistorySettlementId = dmItem.HistorySettlementId;
            dialogCtrl.IsQueryHistoryPositionDetail = true;
            dialogCtrl.DialogOwner = UIHelper.GetWindowOfUIElement(contentViewModel.View);
            dialogCtrl.Initialize();
            dialogCtrl.Run();
            dialogCtrl.Shutdown();
        }

        private bool CanJumpToPositionItemsPage(object obj)
        {
            return !isRefreshingData;
        }

        private void JumpToPositionItemsPage(object obj)
        {
            var pagingCtrl = positionItemsPagerVM?.PagingController as PagingController;
            if (pagingCtrl == null) return;
            try
            {
                var page = System.Convert.ToInt32(obj);
                if (page < 0 || page > pagingCtrl.PageCount)
                {
                    return;
                }
                pagingCtrl.CurrentPage = page;
            }
            catch (Exception) { }
        }
        
        private void UpdateIsRefreshingData(bool isRefreshing)
        {
            this.isRefreshingData = isRefreshing;
            DispatcherHelper.CheckBeginInvokeOnUI(() => 
            {
                jumpToPositionItemsPageCmd.RaiseCanExecuteChanged();
            });
        }
        
        private void RefreshPageData()
        {
            if (isRefreshingData) return;
            UpdateIsRefreshingData(true);
            QueryCurrentDatePositionHistoryPage(0).ContinueWith(t => 
            {
                var resp = t.Result;
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    UpdateIsRefreshingData(false);
                    var queriedPositionList = resp?.CorrectResult?.Page;
                    if (resp == null || resp?.SourceException != null)
                    {
                        // error handle
                    }
                    InvalidatePageDataView(resp?.CorrectResult);
                });
            });
        }

        private void InvalidatePageDataView(SettlementPositionDetailPage respData)
        {
            contentViewModel.PositionItems.Clear();
            contentViewModel.PositionItems.AddRange(respData?.Page?.Select(i => GenerateHistoryPositionDataModel(i)).ToArray());

            var oldPagingController = positionItemsPagerVM.PagingController;
            if (oldPagingController != null)
            {
                PropertyChangedEventManager.RemoveHandler(oldPagingController, PositionItemsPagingControllerPropChanged, "");
            }

            bool showPagerView = false;
            if (respData != null)
            {
                var totalCount = respData.Total;
                var currentPageItemNum = respData.Page?.Count() ?? 0;

                positionItemsPagerVM.PagingController = new PagingController(totalCount, PositionItemsQueryPageSize);
                PropertyChangedEventManager.AddHandler(positionItemsPagerVM.PagingController, PositionItemsPagingControllerPropChanged, "");

                showPagerView = totalCount > currentPageItemNum;
            }
            contentViewModel.PositionItemsPagerView = showPagerView?positionItemsPagerVM.View:null;
        }

        private void InvalidatePositionItemsView(IEnumerable<SettlementPositionDetail> positionItems)
        {
            contentViewModel.PositionItems.Clear();
            contentViewModel.PositionItems.AddRange(positionItems?.Select(i => GenerateHistoryPositionDataModel(i)).ToArray());

        }

        private void PositionItemsPagingControllerPropChanged(object sender, PropertyChangedEventArgs e)
        {
            var pagingCtrl = sender as PagingController;
            if (pagingCtrl == null) return;
            if (e.PropertyName == nameof(PagingController.CurrentPage))
            {
                var reqPage = pagingCtrl.CurrentPage - 1;
                if (reqPage < 0) reqPage = 0;
                if (reqPage >= pagingCtrl.PageCount) return;

                QueryCurrentDatePositionHistoryPage(reqPage).ContinueWith(t =>
                {
                    var resp = t.Result;
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        InvalidatePositionItemsView(resp?.CorrectResult?.Page?.ToArray());
                    });
                });
            }
        }

        private async Task<IInterfaceInteractResponse<SettlementPositionDetailPage>> QueryCurrentDatePositionHistoryPage(int pageIndex)
        {
            var subAccountId = this.SubAccount.SubAccountId;
            var landingInfo = loginDataService.ProxyLoginResp?.HostingSession?.HostingSession2LandingInfo();
            if (landingInfo == null) return null;

            var selectedDate = contentViewModel.SelectedDate;
            if (selectedDate == null) return null;

            DateHelper.GetDateStartAndEndTimestampMs(selectedDate.Value, DateTimeKind.Local, out long startTSMs, out long endTSMs);
            var queryOption = new ReqSettlementPositionDetailOption
            {
                SubAccountId = subAccountId,
                StartTimestampMs = startTSMs,
                EndTimestampMs = endTSMs,
            };
            var pageOption = new IndexedPageOption { PageIndex = pageIndex, PageSize = PositionItemsQueryPageSize };
            return await XqThriftLibConfigurationManager.SharedInstance.TradeHostingTerminalAoHttpStub.getSubAccountPositionHistoryAsync(landingInfo, queryOption, pageOption, CancellationToken.None);
        }

        private SubAccHistoryPositionDM GenerateHistoryPositionDataModel(SettlementPositionDetail srcPosition)
        {
            if (srcPosition == null) return null;
            var positionDM = new DiscretePositionDM((int)srcPosition.SledContractId);

            if (srcPosition.__isset.createTimestampMs)
                positionDM.CreateTimestampMs = srcPosition.CreateTimestampMs;
            if (srcPosition.__isset.lastmodifyTimestampMs)
                positionDM.UpdateTimestampMs = srcPosition.LastmodifyTimestampMs;
            if (srcPosition.__isset.prevPosition)
                positionDM.PrevPosition = srcPosition.PrevPosition;
            if (srcPosition.__isset.longPosition)
                positionDM.LongPosition = srcPosition.LongPosition;
            if (srcPosition.__isset.shortPosition)
                positionDM.ShortPosition = srcPosition.ShortPosition;
            if (srcPosition.__isset.position)
                positionDM.NetPosition = srcPosition.Position;
            if (srcPosition.__isset.calculatePrice)
                positionDM.CalculatePrice = srcPosition.CalculatePrice;
            if (srcPosition.__isset.closeProfit)
                positionDM.CloseProfit = srcPosition.CloseProfit;
            if (srcPosition.__isset.positionProfit)
                positionDM.PositionProfit = srcPosition.PositionProfit;
            if (srcPosition.__isset.positionAvgPrice)
                positionDM.PositionAvgPrice = srcPosition.PositionAvgPrice;
            if (srcPosition.__isset.currency)
                positionDM.Currency = srcPosition.Currency;
            if (srcPosition.__isset.goodsValue)
                positionDM.GoodsValue = srcPosition.GoodsValue;
            if (srcPosition.__isset.leverage)
                positionDM.Leverage = srcPosition.Leverage;
            if (srcPosition.__isset.useMargin)
                positionDM.UseMargin = srcPosition.UseMargin;
            if (srcPosition.__isset.useCommission)
                positionDM.UseCommission = srcPosition.UseCommission;

            XueQiaoFoundationHelper.SetupTargetContract_ContractDetail(positionDM.ContractDetailContainer,
                contractItemTreeQueryCtrl,
                XqContractNameFormatType.CommodityAcronym_Code_ContractCode,
                _container =>
                {
                    DiscretePosition_ModelHelper.RectifyPositionPriceRelatedProps(positionDM);
                });

            return new SubAccHistoryPositionDM(positionDM, toShowPositionDetailCmd, srcPosition.SettlementId);
        }
    }
}