using Manage.Applications.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;
using IDLAutoGenerated.Util;
using xueqiao.trade.hosting.terminal.ao;
using Thrift.Collections;
using xueqiao.trade.hosting;
using System.Threading;
using System.Waf.Applications;
using Manage.Applications.DataModels;
using xueqiao.broker;
using System.ComponentModel;
using XueQiaoFoundation.UI.Components.ListPager.ViewModels;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.UI.Components.ListPager;
using XueQiaoFoundation.Shared.ControllerBase;
using lib.xqclient_base.thriftapi_mediation.Interface;
using business_foundation_lib.xq_thriftlib_config;

namespace Manage.Applications.Controllers
{
    /// <summary>
    /// 资金账号管理页面 controller
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class FundAccountManagePageController : SwitchablePageControllerBase
    {
        private const int ListRequestPageSize = 50;

        private readonly ILoginDataService loginDataService;
        private readonly SimplePagerViewModel pagerViewModel;
        private readonly ExportFactory<FundAccounAddDialogController> accountAddDialogCtrlFactory;
        private readonly ExportFactory<FundAccounEditDialogController> accountEditDialogCtrlFactory;
        private readonly ExportFactory<FundAccountExtraInfoShowDialogController> accountExtraInfoShowDialogCtrlFactory;

        private readonly DelegateCommand toRefreshListCmd;
        private readonly DelegateCommand toAddTradeAccountCmd;
        private readonly DelegateCommand toEditAccountCmd;
        private readonly DelegateCommand toEnableAccountCmd;
        private readonly DelegateCommand toDisableAccountCmd;
        //private readonly DelegateCommand toRemoveAccountCmd;
        private readonly DelegateCommand showExtraInfoCmd;
        private readonly DelegateCommand goJumpPageCmd;
        
        [ImportingConstructor]
        public FundAccountManagePageController(ILoginDataService loginDataService,
            FundAccountManageViewModel pageViewModel,
            SimplePagerViewModel pagerViewModel,
            ExportFactory<FundAccounAddDialogController> accountAddDialogCtrlFactory,
            ExportFactory<FundAccounEditDialogController> accountEditDialogCtrlFactory,
            ExportFactory<FundAccountExtraInfoShowDialogController> accountExtraInfoShowDialogCtrlFactory)
        {
            this.loginDataService = loginDataService;
            this.PageViewModel = pageViewModel;
            this.pagerViewModel = pagerViewModel;
            this.accountAddDialogCtrlFactory = accountAddDialogCtrlFactory;
            this.accountEditDialogCtrlFactory = accountEditDialogCtrlFactory;
            this.accountExtraInfoShowDialogCtrlFactory = accountExtraInfoShowDialogCtrlFactory;

            toRefreshListCmd = new DelegateCommand(ToRefreshList);
            toAddTradeAccountCmd = new DelegateCommand(ToAddTradeAccount);
            toEditAccountCmd = new DelegateCommand(ToEditAccount);
            toEnableAccountCmd = new DelegateCommand(ToEnableAccount);
            toDisableAccountCmd = new DelegateCommand(ToDisableAccount);
            //toRemoveAccountCmd = new DelegateCommand(ToRemoveAccount);
            showExtraInfoCmd = new DelegateCommand(ShowExtraInfo);
            goJumpPageCmd = new DelegateCommand(GoJumpPage);
        }

        public FundAccountManageViewModel PageViewModel { get; private set; }

        protected override void DoInitialize()
        {
            PageViewModel.ToRefreshListCmd = toRefreshListCmd;
            PageViewModel.ToAddAccountCmd = toAddTradeAccountCmd;
            PageViewModel.ToEditCmd = toEditAccountCmd;
            PageViewModel.ToEnableCmd = toEnableAccountCmd;
            PageViewModel.ToDisableCmd = toDisableAccountCmd;
            PageViewModel.ShowExtraInfoCmd = showExtraInfoCmd;
            //PageViewModel.ToRmCmd = toRemoveAccountCmd;

            PageViewModel.PagerContentView = pagerViewModel.View;
            pagerViewModel.GoJumpPageCmd = goJumpPageCmd;
        }

        protected override void DoRun()
        {
            RefreshFirstPageTradeAccounts();
        }

        protected override void DoShutdown()
        {

        }

        private void ToRefreshList()
        {
            RefreshFirstPageTradeAccounts();
        }

        private void ToAddTradeAccount()
        {
            var addPageCtrl = accountAddDialogCtrlFactory.CreateExport().Value;
            addPageCtrl.DialogOwner = UIHelper.GetWindowOfUIElement(PageViewModel.View);
            addPageCtrl.Initialize();
            addPageCtrl.Run();
            addPageCtrl.Shutdown();
            var addedAccount = addPageCtrl.AddedAccountResult;
            if (addedAccount != null)
            {
                RefreshFirstPageTradeAccounts();
            }
        }

        private void ToEditAccount(object obj)
        {
            if (obj is FundAccountModel tarItem)
            {
                var editPageCtrl = accountEditDialogCtrlFactory.CreateExport().Value;
                editPageCtrl.ToEditAccountItem = tarItem;
                editPageCtrl.DialogOwner = UIHelper.GetWindowOfUIElement(PageViewModel.View);
                editPageCtrl.Initialize();
                editPageCtrl.Run();
                editPageCtrl.Shutdown();
                var updatedAccount = editPageCtrl.UpdatedAccountResult;
                if (updatedAccount != null)
                {
                    RefreshFundAccountItem(tarItem);
                }
            }
        }

        private void ToEnableAccount(object obj)
        {
            if (obj is FundAccountModel tarItem)
            {
                var landingInfo = loginDataService.ProxyLoginResp?.HostingSession?.HostingSession2LandingInfo();
                if (landingInfo == null) return;
                XqThriftLibConfigurationManager.SharedInstance.TradeHostingTerminalAoHttpStub
                    .enableTradeAccountAsync(landingInfo, tarItem.AccountId, CancellationToken.None)
                    .ContinueWith(t =>
                    {
                        var resp = t.Result;
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            if (resp == null || resp.SourceException != null)
                            {
                                // handle error
                            }
                            else
                            {
                                RefreshFundAccountItem(tarItem);
                            }
                        });
                    });
            }
        }

        private void ToDisableAccount(object obj)
        {
            if (obj is FundAccountModel tarItem)
            {
                var landingInfo = loginDataService.ProxyLoginResp?.HostingSession?.HostingSession2LandingInfo();
                if (landingInfo == null) return;
                XqThriftLibConfigurationManager.SharedInstance.TradeHostingTerminalAoHttpStub
                    .disableTradeAccountAsync(landingInfo, tarItem.AccountId, CancellationToken.None)
                    .ContinueWith(t =>
                    {
                        var resp = t.Result;
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            if (resp == null || resp.SourceException != null)
                            {
                                // handle error
                            }
                            else
                            {
                                RefreshFundAccountItem(tarItem);
                            }
                        });
                    });
            }
        }

        //private void ToRemoveAccount(object obj)
        //{
        //    if (obj is FundAccountModel tarItem)
        //    {
        //        var rmAccountId = tarItem.AccountId;
        //        var landingInfo = loginDataService.ProxyLoginResp?.HostingSession?.HostingSession2LandingInfo();
        //        if (landingInfo == null) return;
        //        XqThriftLibConfigurationManager.SharedInstance.TradeHostingTerminalAoHttpStub
        //            .rmTradeAccountAsync(landingInfo, rmAccountId, CancellationToken.None)
        //            .ContinueWith(t =>
        //            {
        //                var resp = t.Result;
        //                DispatcherHelper.CheckBeginInvokeOnUI(() =>
        //                {
        //                    if (resp == null || resp.SourceException != null)
        //                    {
        //                        // handle error
        //                    }
        //                    else
        //                    {
        //                        PageViewModel.TradeAccountItems.RemoveAll(i => i.AccountId == rmAccountId);
        //                    }
        //                });
        //            });
        //    }
        //}

        private void ShowExtraInfo(object obj)
        {
            if (obj is FundAccountModel tarItem)
            {
                var ctrl = accountExtraInfoShowDialogCtrlFactory.CreateExport().Value;
                ctrl.DialogOwner = UIHelper.GetWindowOfUIElement(PageViewModel.View);
                ctrl.FundAccount = tarItem.AccountMeta;
                ctrl.Initialize();
                ctrl.Run();
                ctrl.Shutdown();
            }
        }

        private void GoJumpPage(object obj)
        {
            var pagingCtrl = pagerViewModel.PagingController as PagingController;
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
            catch (Exception)
            {

            }
        }

        private void PagingControllerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var pagingCtrl = sender as PagingController;
            if (pagingCtrl == null) return;
            if (e.PropertyName == nameof(PagingController.CurrentPage))
            {
                var reqPage = pagingCtrl.CurrentPage - 1;
                if (reqPage < 0) reqPage = 0;
                if (reqPage >= pagingCtrl.PageCount) return;

                LoadTradeAccounts(reqPage, ListRequestPageSize, resp =>
                {
                    var respCorrectResult = resp?.CorrectResult;
                    // 使用 Select 方法得到的类型是 WhereSelectEnumerableIterator，
                    // 它的迭代器在迭代时会对迭代项进行深度拷贝。所以使用 ToArray 方法让容器的迭代器不拷贝输出
                    var newModelList = respCorrectResult?.ResultList?
                        .Select(i =>
                        new FundAccountModel(i.TradeAccountId)
                        {
                            AccountMeta = i,
                            FormatExtraAccountPropertiesMsg = i.DisplayFormatHostingTradeAccountProperties()
                        }).ToArray();

                    RefreshBrokerDetailToTradeAccountModel(newModelList);

                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        if (resp == null || resp.SourceException != null || respCorrectResult == null)
                        {
                            // handle error
                            return;
                        }
                        
                        // 更新列表数据
                        PageViewModel.TradeAccountItems.Clear();
                        PageViewModel.TradeAccountItems.AddRange(newModelList);
                    });
                });
            }
        }

        private void LoadTradeAccounts(int pageIndex, int pageSize,
            Action<IInterfaceInteractResponse<QueryHostingTradeAccountPage>> handler)
        {
            var landingInfo = loginDataService.ProxyLoginResp?.HostingSession?.HostingSession2LandingInfo();
            if (landingInfo == null)
            {
                handler?.Invoke(null);
                return;
            }

            var option = new QueryHostingTradeAccountOption
            {
                NotInAccountStates = new THashSet<TradeAccountState> { TradeAccountState.ACCOUNT_REMOVED }
            };
            var pageOption = new IndexedPageOption
            {
                NeedTotalCount = true,
                PageIndex = pageIndex,
                PageSize = pageSize,
            };

            var task = XqThriftLibConfigurationManager.SharedInstance
                .TradeHostingTerminalAoHttpStub
                .getTradeAccountPageAsync(landingInfo, option, pageOption, CancellationToken.None);
            task.ContinueWith((t) =>
            {
                handler?.Invoke(t.Result);
            });
        }

        private void RefreshFirstPageTradeAccounts()
        {
            LoadTradeAccounts(0, ListRequestPageSize, resp => 
            {
                var respCorrectResult = resp?.CorrectResult;
                // 使用 Select 方法得到的类型是 WhereSelectEnumerableIterator，
                // 它的迭代器在迭代时会对迭代项进行深度拷贝。所以使用 ToArray 方法让容器的迭代器不拷贝输出
                var newModelList = respCorrectResult?.ResultList?
                    .Select(i =>
                    new FundAccountModel(i.TradeAccountId)
                    {
                        AccountMeta = i,
                        FormatExtraAccountPropertiesMsg = i.DisplayFormatHostingTradeAccountProperties()
                    }).ToArray();

                RefreshBrokerDetailToTradeAccountModel(newModelList);

                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    if (resp == null || resp.SourceException != null || respCorrectResult == null)
                    {
                        // handle error
                        return;
                    }

                    // 更新翻页控制器
                    var oldPagingController = pagerViewModel.PagingController;
                    if (oldPagingController != null)
                    {
                        PropertyChangedEventManager.RemoveHandler(oldPagingController, PagingControllerPropertyChanged, "");
                    }
                    pagerViewModel.PagingController = new PagingController(respCorrectResult.TotalCount, ListRequestPageSize);
                    PropertyChangedEventManager.AddHandler(pagerViewModel.PagingController, PagingControllerPropertyChanged, "");

                    // 更新列表数据
                    PageViewModel.TradeAccountItems.Clear();
                    PageViewModel.TradeAccountItems.AddRange(newModelList);
                });
            });
        }

        private void RefreshFundAccountItem(FundAccountModel accountItem)
        {
            if (accountItem == null) return;
            var updatedAccountId = accountItem.AccountId;
            var landingInfo = loginDataService.ProxyLoginResp?.HostingSession?.HostingSession2LandingInfo();
            if (landingInfo == null)
            {
                return;
            }

            var option = new QueryHostingTradeAccountOption { TradeAccountId = updatedAccountId };
            var pageOption = new IndexedPageOption
            {
                NeedTotalCount = false,
                PageIndex = 0,
                PageSize = 1,
            };

            var task = XqThriftLibConfigurationManager.SharedInstance
                .TradeHostingTerminalAoHttpStub
                .getTradeAccountPageAsync(landingInfo, option, pageOption, CancellationToken.None);
            task.ContinueWith(t =>
            {
                var queriedAccount = t.Result?.CorrectResult?.ResultList?.FirstOrDefault(i => i.TradeAccountId == updatedAccountId);
                if (queriedAccount == null) return;

                // 更新信息
                accountItem.AccountMeta = queriedAccount;
                accountItem.FormatExtraAccountPropertiesMsg = queriedAccount.DisplayFormatHostingTradeAccountProperties();
                RefreshBrokerDetailToTradeAccountModel(new FundAccountModel[] { accountItem });
            });
        }

        private void RefreshBrokerDetailToTradeAccountModel(IEnumerable<FundAccountModel> tradeAccountList)
        {
            if (tradeAccountList == null || tradeAccountList.Count() == 0) return;
            
            var accountListGroupedByAccessId = tradeAccountList.Where(i =>
            {
                var accessId = i.AccountMeta?.TradeBrokerAccessId;
                return (accessId != null && accessId > 0);
            }).GroupBy(i => i.AccountMeta.TradeBrokerAccessId);
            var queryBrokerAccessIds = accountListGroupedByAccessId.Select(i => i.Key).ToArray();

            var maxBatchQuerySize = 50;
            IEnumerable<int> readyQueryIds = null;
            IEnumerable<int> restToQueryIds = queryBrokerAccessIds;
            while (true)
            {
                readyQueryIds = restToQueryIds.Take(maxBatchQuerySize);
                restToQueryIds = restToQueryIds.Skip(maxBatchQuerySize);
                if (readyQueryIds.Count() == 0)
                {
                    break;
                }

                // Query broker infos
                var option = new ReqBrokerAccessInfoOption { BrokerAccessIds = new List<int>() };
                option.BrokerAccessIds.AddRange(readyQueryIds);
                XqThriftLibConfigurationManager.SharedInstance.BrokerServiceHttpStub
                    .reqBrokerAccessInfoAsync(option, 0, readyQueryIds.Count(), CancellationToken.None)
                    .ContinueWith(t=> 
                    {
                        var resp = t.Result;
                        var respBrokerInfoDict = resp?.CorrectResult?.Page.ToDictionary(i => i.EntryId);
                        if (respBrokerInfoDict == null) return;
                        foreach (var groupItem in accountListGroupedByAccessId)
                        {
                            var tarAccessId = groupItem.Key;
                            if (respBrokerInfoDict.TryGetValue(tarAccessId, out BrokerAccessInfo queriedBrokerInfo))
                            {
                                foreach (var mergeItem in groupItem)
                                {
                                    mergeItem.BrokerAccessName = queriedBrokerInfo.AccessName;
                                    mergeItem.BrokerName = queriedBrokerInfo.CnName;
                                }
                            }
                        }
                    });
            }
        }
    }
}
