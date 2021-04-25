﻿using business_foundation_lib.xq_thriftlib_config;
using ContainerShell.Interfaces.Applications;
using IDLAutoGenerated.Util;
using lib.xqclient_base.thriftapi_mediation;
using lib.xqclient_base.thriftapi_mediation.Interface;
using NativeModel.Trade;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows;
using xueqiao.trade.hosting;
using xueqiao.trade.hosting.position.adjust.assign.thriftapi;
using xueqiao.trade.hosting.position.adjust.thriftapi;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.BusinessResources.Models;
using XueQiaoFoundation.Interfaces.Applications;
using XueQiaoFoundation.Interfaces.Helper;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Helper.WeakDelegate;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;
using XueQiaoWaf.Trade.Modules.Applications.ViewModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class PositionAssignHistoryComponentCtrl : IController
    {
        private readonly PositionAssignHistoryVM contentVM;
        private readonly ILoginDataService loginDataService;
        private readonly IContainerShellService containerShellService;
        private readonly IContractItemTreeQueryController contractItemTreeQueryController;
        private readonly IUserSubAccountRelatedItemQueryController subAccountRelatedItemQueryController;
        private readonly IUserSubAccountRelatedItemCacheController subAccountRelatedItemCacheController;
        private readonly IHostingUserQueryController hostingUserQueryController;
        private readonly IHostingUserCacheController hostingUserCacheController;
        private readonly IEventAggregator eventAggregator;

        private readonly DelegateCommand triggerSelectContractCmd;
        private readonly DelegateCommand clearSelectedContractCmd;
        private readonly DelegateCommand queryCmd;
        private readonly DelegateCommand clickItemTargetKeyRelatedColumnCmd;

        private bool isQuerying;

        [ImportingConstructor]
        public PositionAssignHistoryComponentCtrl(
            PositionAssignHistoryVM contentVM,
            ILoginDataService loginDataService,
            IContainerShellService containerShellService,
            IContractItemTreeQueryController contractItemTreeQueryController,
            IUserSubAccountRelatedItemQueryController subAccountRelatedItemQueryController,
            IUserSubAccountRelatedItemCacheController subAccountRelatedItemCacheController,
            IHostingUserQueryController hostingUserQueryController,
            IHostingUserCacheController hostingUserCacheController,
            IEventAggregator eventAggregator)
        {
            this.contentVM = contentVM;
            this.loginDataService = loginDataService;
            this.containerShellService = containerShellService;
            this.contractItemTreeQueryController = contractItemTreeQueryController;
            this.subAccountRelatedItemQueryController = subAccountRelatedItemQueryController;
            this.subAccountRelatedItemCacheController = subAccountRelatedItemCacheController;
            this.hostingUserQueryController = hostingUserQueryController;
            this.hostingUserCacheController = hostingUserCacheController;
            this.eventAggregator = eventAggregator;

            triggerSelectContractCmd = new DelegateCommand(TriggerSelectContract);
            clearSelectedContractCmd = new DelegateCommand(ClearSelectedContract);
            queryCmd = new DelegateCommand(QueryHistory, CanQueryHistory);
            clickItemTargetKeyRelatedColumnCmd = new DelegateCommand(ClickItemTargetKeyRelatedColumn);
        }

        public XueQiaoFoundation.BusinessResources.DataModels.TabWorkspace ParentWorkspace { get; set; }
        public ITradeComponentController ParentComponentCtrl { get; set; }

        public object ComponentContentView => contentVM.View;

        public void Initialize()
        {
            if (ParentWorkspace == null) throw new ArgumentNullException("ParentWorkspace");
            if (ParentComponentCtrl == null) throw new ArgumentNullException("ParentComponentCtrl");

            PropertyChangedEventManager.AddHandler(contentVM, ContentVMPropertyChanged, "");

            contentVM.TriggerSelectContractCmd = triggerSelectContractCmd;
            contentVM.ClearSelectedContractCmd = clearSelectedContractCmd;
            contentVM.QueryCmd = queryCmd;
            contentVM.ClickItemTargetKeyRelatedColumnCmd = clickItemTargetKeyRelatedColumnCmd;

            PropertyChangedEventManager.AddHandler(ParentWorkspace, ParentWorkspacePropertyChanged, "");
        }

        public void Run()
        {

        }

        public void Shutdown()
        {

        }

        private void ContentVMPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PositionAssignHistoryVM.SelectedContractDetailContainer)
                || e.PropertyName == nameof(PositionAssignHistoryVM.QueryStartDate)
                || e.PropertyName == nameof(PositionAssignHistoryVM.QueryEndDate))
            {
                queryCmd?.RaiseCanExecuteChanged();
            }
        }

        private void ParentWorkspacePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(XueQiaoFoundation.BusinessResources.DataModels.TabWorkspace.SubAccountId))
            {
                this.contentVM.PositionAssignedItems.Clear();
                this.contentVM.QueryStartDate = null;
                this.contentVM.QueryEndDate = null;
                queryCmd?.RaiseCanExecuteChanged();
            }
        }

        private void UpdateIsQuerying(bool isQuerying)
        {
            this.isQuerying = isQuerying;
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                queryCmd?.RaiseCanExecuteChanged();
            });
        }

        private bool CanQueryHistory()
        {
            if (this.contentVM.SelectedContractDetailContainer != null) return true;
            if (this.contentVM.QueryStartDate != null && this.contentVM.QueryEndDate != null) return true;
            return false;
        }

        private void QueryHistory()
        {
            RefreshListIfNeed();
        }

        private void TriggerSelectContract(object obj)
        {
            var triggerElement = obj as UIElement;
            if (triggerElement == null) return;

            containerShellService.ShowContractQuickSearchPopup(triggerElement, null, 
                _selContractId => 
                {
                    if (_selContractId != null)
                    {
                        var detailContainer = new TargetContract_TargetContractDetail(_selContractId.Value);
                        XueQiaoFoundationHelper.SetupTargetContract_ContractDetail(detailContainer, 
                            contractItemTreeQueryController, XqContractNameFormatType.CommodityAcronym_Code_ContractCode);
                        this.contentVM.SelectedContractDetailContainer = detailContainer;
                        queryCmd?.RaiseCanExecuteChanged();
                    }
                });
        }

        private void ClearSelectedContract()
        {
            this.contentVM.SelectedContractDetailContainer = null;
            queryCmd?.RaiseCanExecuteChanged();
        }


        private void ClickItemTargetKeyRelatedColumn(object obj)
        {
            var item = obj as PositionAssignedDM;
            if (item == null) return;

            // 联动
            var associateArgs = new TradeComponentXqTargetAssociateArgs(ParentComponentCtrl.ParentWorkspace, ParentComponentCtrl.Component,
                ClientXQOrderTargetType.CONTRACT_TARGET, $"{item.ContractId}");

            var previewAssociateDialogOwner = UIHelper.GetWindowOfUIElement(contentVM.View);
            Point? previewAssociateDialogLocation = null;
            ParentComponentCtrl.XqTargetAssociateHandler?.HandleXqTargetAssociate(previewAssociateDialogOwner, previewAssociateDialogLocation, associateArgs);
        }

        private void RefreshListIfNeed()
        {
            if (isQuerying) return;
            var queryContractId = contentVM.SelectedContractDetailContainer?.ContractId;
            var startDate = contentVM.QueryStartDate;
            var endDate = contentVM.QueryEndDate;

            var subAccountId = ParentWorkspace.SubAccountId;
            long? startTimestamp = null;
            long? endTimestamp = null;
            if (startDate != null)
            {
                startTimestamp = (long)DateHelper.UnixTimspan(startDate.Value, DateTimeKind.Local).TotalSeconds;
            }
            if (endDate != null)
            {
                endTimestamp = (long)DateHelper.UnixTimspan(endDate.Value, DateTimeKind.Local).TotalSeconds;
            }

            Task.Run(() =>
            {
                if (isQuerying) return;
                UpdateIsQuerying(true);
                var resp = QueryPositionAssignedItems(subAccountId, queryContractId, startTimestamp, endTimestamp);
                UpdateIsQuerying(false);

                if (subAccountId != this.ParentWorkspace.SubAccountId) return;
                if (resp == null) return;

                var assignedItems = resp?.CorrectResult?.ToArray();
                if (assignedItems == null) return;

                var dmItems = new List<PositionAssignedDM>();
                foreach (var assignedItem in assignedItems)
                {
                    var dmItem = GeneratePositionAssignedDM(assignedItem);
                    dmItems.Add(dmItem);
                }

                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    this.contentVM.PositionAssignedItems.Clear();
                    this.contentVM.PositionAssignedItems.AddRange(dmItems);
                    this.contentVM.QueryListTimestamp = (long)DateHelper.NowUnixTimeSpan().TotalSeconds;
                });
            });
        }
        
        private IInterfaceInteractResponse<IEnumerable<PositionAssigned>> QueryPositionAssignedItems(long subAccountId,
            int? contractId,
            long? startTimestamp, long? endTimestmap)
        {
            var landingInfo = loginDataService.ProxyLoginResp?.HostingSession?.HostingSession2LandingInfo();
            if (landingInfo == null) return null;

            var queryPageSize = 50;
            IInterfaceInteractResponse<PositionAssignedPage> firstPageResp = null;
            var queryAllCtrl = new QueryAllItemsByPageHelper<PositionAssigned>(pageIndex => {
                var option = new ReqPositionAssignedOption { SubAccountId = subAccountId };
                if (contractId != null) option.SledContractId = contractId.Value;
                if (startTimestamp != null) option.AssignStartTimestamp = startTimestamp.Value;
                if (endTimestmap != null) option.AssignEndTimestamp = endTimestmap.Value;

                var pageOption = new IndexedPageOption
                {
                    NeedTotalCount = true,
                    PageIndex = pageIndex,
                    PageSize = queryPageSize,
                };
                var resp = XqThriftLibConfigurationManager.SharedInstance.TradeHostingTerminalAoHttpStub.reqPositionAssigned(landingInfo, option, pageOption);
                if (resp == null) return null;
                if (pageIndex == 0)
                {
                    firstPageResp = resp;
                }
                var pageInfo = resp.CorrectResult;
                var pageResult = new QueryItemsByPageResult<PositionAssigned>(resp.SourceException != null)
                {
                    TotalCount = pageInfo?.Total,
                    Page = pageInfo?.Page?.ToArray()
                };
                return pageResult;
            });

            queryAllCtrl.RemoveDuplicateFunc = _items =>
            {
                if (_items == null) return null;
                var idGroupedItems = _items.GroupBy(i => i.AssignId);
                return idGroupedItems.Select(i => i.LastOrDefault()).ToArray();
            };

            var queriedItems = queryAllCtrl.QueryAllItems();
            if (firstPageResp == null) return null;

            var tarResp = new InterfaceInteractResponse<IEnumerable<PositionAssigned>>(firstPageResp.Servant,
                firstPageResp.InterfaceName,
                firstPageResp.SourceException,
                firstPageResp.HasTransportException,
                firstPageResp.HttpResponseStatusCode,
                queriedItems)
            {
                CustomParsedExceptionResult = firstPageResp.CustomParsedExceptionResult,
                InteractInformation = firstPageResp.InteractInformation
            };
            return tarResp;
        }
        
        private PositionAssignedDM GeneratePositionAssignedDM(PositionAssigned idlItem)
        {
            var dmItem = PositionAssignedDM.FromIDLItem(idlItem);
            XueQiaoFoundationHelper.SetupTargetContract_ContractDetail(dmItem.ContractDetailContainer,
                contractItemTreeQueryController, XqContractNameFormatType.CommodityAcronym_Code_ContractCode);

            // 设置其他信息
            var assignUserId = (int)idlItem.AssignSubUserId;
            var cacheHostingUser = hostingUserCacheController.GetCache(assignUserId);
            if (cacheHostingUser != null)
            {
                dmItem.AssignUserName = cacheHostingUser.LoginName;
            }
            else
            {
                var queryHandler = new Action<IInterfaceInteractResponse<HostingUser>>(resp =>
                {
                    var queriedUser = resp?.CorrectResult;
                    dmItem.AssignUserName = queriedUser?.LoginName;
                });
                var handlerReference = new ActionDelegateReference<IInterfaceInteractResponse<HostingUser>>(queryHandler,
                    true);
                hostingUserQueryController.QueryUser(assignUserId, handlerReference);
            }
            return dmItem;
        }
    }
}
