﻿using business_foundation_lib.xq_thriftlib_config;
using IDLAutoGenerated.Util;
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
using xueqiao.trade.hosting.terminal.ao;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;

namespace Manage.Applications.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class FundAccountEquityDailyHistoryController : IController
    {
        private readonly FundAccountEquityDailyHistoryViewModel contentViewModel;
        private readonly ExportFactory<FundAccountEquityDetailDialogController> equityDetailDialogCtrlFactory;
        private readonly ILoginDataService loginDataService;

        private readonly DelegateCommand pageGoBackCmd;
        private readonly DelegateCommand toShowCurrencyGroupedEquityDetailCmd;
        private readonly DelegateCommand dataRefreshCmd;

        private bool isDataRefreshing;
        private HostingTAFundItem currentDateEquityData;


        [ImportingConstructor]
        public FundAccountEquityDailyHistoryController(FundAccountEquityDailyHistoryViewModel contentViewModel,
            ExportFactory<FundAccountEquityDetailDialogController> equityDetailDialogCtrlFactory,
            ILoginDataService loginDataService)
        {
            this.contentViewModel = contentViewModel;
            this.equityDetailDialogCtrlFactory = equityDetailDialogCtrlFactory;
            this.loginDataService = loginDataService;

            pageGoBackCmd = new DelegateCommand(() => this.PageGoBackHandler?.Invoke(this));
            toShowCurrencyGroupedEquityDetailCmd = new DelegateCommand(ToShowCurrencyGroupedEquityDetail);
            dataRefreshCmd = new DelegateCommand(RefreshPageData, CanRefreshPageData);

        }

        public HostingTradeAccount FundAccount { get; set; }

        /// <summary>
        /// 页面后退处理
        /// </summary>
        public Action<FundAccountEquityDailyHistoryController> PageGoBackHandler { get; set; }

        public object PageView => contentViewModel.View;

        public void Initialize()
        {
            if (FundAccount == null) throw new ArgumentNullException("FundAccount");
            contentViewModel.FundAccount = FundAccount;
            contentViewModel.PageGoBackCmd = pageGoBackCmd;
            contentViewModel.DataRefreshCmd = dataRefreshCmd;
        }

        public void Run()
        {
            contentViewModel.SelectedDate = DateTime.Now.Date;
            RefreshPageDataIfNeed();
            PropertyChangedEventManager.AddHandler(contentViewModel, ContentViewModelPropertyChanged, "");
        }

        public void Shutdown()
        {
            PropertyChangedEventManager.RemoveHandler(contentViewModel, ContentViewModelPropertyChanged, "");
            PageGoBackHandler = null;
        }

        private void ContentViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(FundAccountEquityDailyHistoryViewModel.SelectedDate))
            {
                dataRefreshCmd.RaiseCanExecuteChanged();
                RefreshPageDataIfNeed();
            }
        }

        private void ToShowCurrencyGroupedEquityDetail(object obj)
        {
            var model = obj as FundAccountEquityModel;
            if (model == null) return;

            var tarCurrencyNo = model.EquityData.CurrencyNo;
            var tarCurrencyEquityItem = currentDateEquityData?.GroupFunds[tarCurrencyNo];
            if (tarCurrencyEquityItem == null) return;

            var dialogCtrl = equityDetailDialogCtrlFactory.CreateExport().Value;
            dialogCtrl.DialogOwner = contentViewModel.DisplayInWindow;
            dialogCtrl.CurrencyEquityItem = tarCurrencyEquityItem;
            dialogCtrl.Initialize();
            dialogCtrl.Run();
            dialogCtrl.Shutdown();
        }

        private bool CanRefreshPageData()
        {
            return isDataRefreshing == false && contentViewModel.SelectedDate != null;
        }

        private void RefreshPageData()
        {
            RefreshPageDataIfNeed();
        }

        private void UpdateIsDataRefreshing(bool isRefreshing)
        {
            this.isDataRefreshing = isRefreshing;
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                dataRefreshCmd.RaiseCanExecuteChanged();
            });
        }

        private void RefreshPageDataIfNeed()
        {
            if (isDataRefreshing) return;

            var landingInfo = loginDataService.ProxyLoginResp?.HostingSession?.HostingSession2LandingInfo();
            if (landingInfo == null) return;

            var selectedDate = contentViewModel.SelectedDate;
            if (selectedDate == null) return;

            var selectedDateFormated = selectedDate.Value.ToString("yyyy-MM-dd");
            var fundAccountId = FundAccount.TradeAccountId;

            UpdateIsDataRefreshing(true);
            var task = XqThriftLibConfigurationManager.SharedInstance.TradeHostingTerminalAoHttpStub.getTradeAccountFundHisAsync(landingInfo, fundAccountId, selectedDateFormated, selectedDateFormated, CancellationToken.None);
            task.ContinueWith(t => 
            {
                var equityDataResp = t.Result;
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    var equityData = equityDataResp?.CorrectResult?.FirstOrDefault(i=>i.Date == selectedDateFormated)?.Item;

                    UpdateIsDataRefreshing(false);
                    this.currentDateEquityData = equityData;

                    if (equityDataResp == null || equityDataResp?.SourceException != null || equityData == null)
                    {
                        // error handle
                    }

                    FundAccountEquityModel.GenerateEquityModels(equityData,
                        out IEnumerable<FundAccountEquityModel> totalEquityItems,
                        out IEnumerable<FundAccountEquityModel> currencyGroupedEquityItems,
                        _model => { _model.ShowDetailCmd = this.toShowCurrencyGroupedEquityDetailCmd; });

                    contentViewModel.TotalEquityItems.Clear();
                    contentViewModel.TotalEquityItems.AddRange(totalEquityItems);
                    contentViewModel.CurrencyGroupedEquityItems.Clear();
                    contentViewModel.CurrencyGroupedEquityItems.AddRange(currencyGroupedEquityItems);
                    contentViewModel.EquityUpdateTimestampMs = equityData?.UpdateTimestampMs;
                });

            });
        }
    }
}