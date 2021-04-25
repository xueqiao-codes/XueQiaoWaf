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
using Thrift.Collections;
using xueqiao.trade.hosting;
using XueQiaoFoundation.Interfaces.Applications;
using XueQiaoFoundation.Shared.ControllerBase;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.UI.Components.ListPager;
using XueQiaoFoundation.UI.Components.ListPager.ViewModels;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;

namespace Manage.Applications.Controllers
{
    /// <summary>
    /// 用户管理页面 controller
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class SubUserManagePageController : SwitchablePageControllerBase
    {
        private const int ListRequestPageSize = 50;

        private readonly ILoginDataService loginDataService;
        private readonly SimplePagerViewModel pagerViewModel;

        private readonly DelegateCommand toRefreshListCmd;
        private readonly DelegateCommand goJumpPageCmd;

        [ImportingConstructor]
        public SubUserManagePageController(ILoginDataService loginDataService,
            SubUserManageViewModel pageViewModel,
            SimplePagerViewModel pagerViewModel)
        {
            this.loginDataService = loginDataService;
            this.PageViewModel = pageViewModel;
            this.pagerViewModel = pagerViewModel;

            toRefreshListCmd = new DelegateCommand(ToRefreshList);
            goJumpPageCmd = new DelegateCommand(GoJumpPage);
        }

        public readonly SubUserManageViewModel PageViewModel;

        protected override void DoInitialize()
        {
            PageViewModel.ToRefreshListCmd = toRefreshListCmd;
            PageViewModel.PagerContentView = pagerViewModel.View;
            pagerViewModel.GoJumpPageCmd = goJumpPageCmd;
        }

        protected override void DoRun()
        {
            RefreshFirstPageSubUsers();
        }

        protected override void DoShutdown()
        {

        }

        private void ToRefreshList()
        {
            RefreshFirstPageSubUsers();
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

                LoadSubUsers(reqPage, ListRequestPageSize, resp =>
                {
                    var respCorrentResult = resp?.CorrectResult;
                    var newModelList = respCorrentResult?.ResultList?
                        .Select(i => new SubUserDataModel(i.SubUserId)
                        {
                            UserMeta = i
                        }).ToArray();
                    if (newModelList != null)
                    {
                        foreach (var i in newModelList)
                        {
                            AsyncLoadRelatedSubAccountsForSubUserItem(i);
                        }
                    }

                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        if (resp == null || resp.SourceException != null || respCorrentResult == null)
                        {
                            // error handle
                            return;
                        }

                        // 更新列表数据
                        PageViewModel.SubUserItems.Clear();
                        PageViewModel.SubUserItems.AddRange(newModelList);
                    });
                });
            }
        }

        private void RefreshFirstPageSubUsers()
        {
            LoadSubUsers(0, ListRequestPageSize, resp =>
            {
                var respCorrentResult = resp?.CorrectResult;
                var newModelList = respCorrentResult?.ResultList?
                    .Select(i => new SubUserDataModel(i.SubUserId)
                    {
                        UserMeta = i
                    }).ToArray();
                if (newModelList != null)
                {
                    foreach (var i in newModelList)
                    {
                        AsyncLoadRelatedSubAccountsForSubUserItem(i);
                    }
                }

                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    if (resp == null || resp.SourceException != null || respCorrentResult == null)
                    {
                        // error handle
                        return;
                    }

                    // 更新翻页控制器
                    var oldPagingController = pagerViewModel.PagingController;
                    if (oldPagingController != null)
                    {
                        PropertyChangedEventManager.RemoveHandler(oldPagingController, PagingControllerPropertyChanged, "");
                    }
                    pagerViewModel.PagingController = new PagingController(respCorrentResult.TotalCount, ListRequestPageSize);
                    PropertyChangedEventManager.AddHandler(pagerViewModel.PagingController, PagingControllerPropertyChanged, "");

                    // 更新列表数据
                    PageViewModel.SubUserItems.Clear();
                    PageViewModel.SubUserItems.AddRange(newModelList);
                });
            });
        }
        
        private void LoadSubUsers(int pageIndex, int pageSize,
            Action<IInterfaceInteractResponse<QueryHostingUserPage>> handler)
        {
            var landingInfo = loginDataService.ProxyLoginResp?.HostingSession?.HostingSession2LandingInfo();
            if (landingInfo == null)
            {
                handler?.Invoke(null);
                return;
            }

            var option = new QueryHostingUserOption { OrderType = HostingUserOrderType.OrderByCreateTimestampDesc };
            var pageOption = new IndexedPageOption
            {
                NeedTotalCount = true,
                PageIndex = pageIndex,
                PageSize = pageSize,
            };

            var task = XqThriftLibConfigurationManager.SharedInstance
                .TradeHostingTerminalAoHttpStub
                .getHostingUserPageAsync(landingInfo, option, pageOption, CancellationToken.None);
            task.ContinueWith(t =>
            {
                handler?.Invoke(t.Result);
            });
        }

        private void AsyncLoadRelatedSubAccountsForSubUserItem(SubUserDataModel subUserItem)
        {
            var landingInfo = loginDataService.ProxyLoginResp?.HostingSession?.HostingSession2LandingInfo();
            if (landingInfo == null) return;

            var subUserId = subUserItem.SubUserId;
            XqThriftLibConfigurationManager.SharedInstance.TradeHostingTerminalAoHttpStub
                .getSARUTBySubUserIdAsync(landingInfo, new THashSet<int> { subUserId }, CancellationToken.None)
                .ContinueWith(t => 
                {
                    List<HostingSubAccountRelatedItem> relatedItems = null;
                    if (t.Result.CorrectResult?.TryGetValue(subUserId, out relatedItems) == true)
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            subUserItem.RelatedToSubAccounts.Clear();
                            subUserItem.RelatedToSubAccounts.AddRange(relatedItems);
                        });
                    }
                });
        }
    }
}