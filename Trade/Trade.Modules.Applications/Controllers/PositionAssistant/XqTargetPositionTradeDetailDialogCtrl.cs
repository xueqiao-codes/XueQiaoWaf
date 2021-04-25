using lib.xqclient_base.thriftapi_mediation;
using lib.xqclient_base.thriftapi_mediation.Interface;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.trade.hosting.position.statis;
using xueqiao.trade.hosting.proxy;
using XueQiaoFoundation.Interfaces.Applications;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.ViewModels;
using NativeModel.Trade;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.Interfaces.Helper;
using XueQiaoFoundation.BusinessResources.Models;
using System.Windows;
using XueQiaoFoundation.BusinessResources.Helpers;
using business_foundation_lib.xq_thriftlib_config;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class XqTargetPositionTradeDetailDialogCtrl : IController
    {
        private readonly XqTargetPositionTradeDetailVM contentVM;
        private readonly ILoginDataService loginDataService;
        private readonly ILoginUserManageService loginUserManageService;
        private readonly IMessageWindowService messageWindowService;
        private readonly IEventAggregator eventAggregator;
        private readonly IContractItemTreeQueryController contractItemTreeQueryCtrl;
        private readonly IComposeGraphCacheController composeGraphCacheCtrl;
        private readonly IComposeGraphQueryController composeGraphQueryCtrl;
        private readonly IUserComposeViewCacheController userComposeViewCacheCtrl;
        private readonly IUserComposeViewQueryController userComposeViewQueryCtrl;
        
        private IMessageWindow dialog;
        private bool isViewDataRefreshing;
        private NativeComposeGraph queriedComposeGraph;

        [ImportingConstructor]
        public XqTargetPositionTradeDetailDialogCtrl(
            XqTargetPositionTradeDetailVM contentVM,
            ILoginDataService loginDataService,
            ILoginUserManageService loginUserManageService,
            IMessageWindowService messageWindowService,
            IEventAggregator eventAggregator,
            IContractItemTreeQueryController contractItemTreeQueryCtrl,
            IComposeGraphCacheController composeGraphCacheCtrl,
            IComposeGraphQueryController composeGraphQueryCtrl,
            IUserComposeViewCacheController userComposeViewCacheCtrl,
            IUserComposeViewQueryController userComposeViewQueryCtrl)
        {
            this.contentVM = contentVM;
            this.loginDataService = loginDataService;
            this.loginUserManageService = loginUserManageService;
            this.messageWindowService = messageWindowService;
            this.eventAggregator = eventAggregator;
            this.contractItemTreeQueryCtrl = contractItemTreeQueryCtrl;
            this.composeGraphCacheCtrl = composeGraphCacheCtrl;
            this.composeGraphQueryCtrl = composeGraphQueryCtrl;
            this.userComposeViewCacheCtrl = userComposeViewCacheCtrl;
            this.userComposeViewQueryCtrl = userComposeViewQueryCtrl;

            loginUserManageService.HasLogouted += RecvUserHasLogouted;
        }

        public object DialogOwner { get; set; }
        
        public XqTargetDetailPositionDM DetailPositionDM { get; set; }

        public long DetailPositionId { get; set; }

        public void Initialize()
        {
            if (DetailPositionDM == null) throw new ArgumentNullException("DetailPositionDM");

            var xqTargetItem = new XqTargetDM(DetailPositionDM.TargetType);
            if (xqTargetItem.TargetType == ClientXQOrderTargetType.CONTRACT_TARGET)
            {
                var contractId = Convert.ToInt32(DetailPositionDM.TargetKey);
                xqTargetItem.TargetContractDetailContainer = new TargetContract_TargetContractDetail(contractId);
                XueQiaoFoundationHelper.SetupTargetContract_ContractDetail(xqTargetItem.TargetContractDetailContainer,
                    contractItemTreeQueryCtrl, XqContractNameFormatType.CommodityAcronym_Code_ContractCode,
                    _container => 
                    {
                        XqTargetDMHelper.InvalidateTargetNameWithAppropriate(xqTargetItem, XqAppLanguages.CN);
                    });
            }
            else if (xqTargetItem.TargetType == ClientXQOrderTargetType.COMPOSE_TARGET)
            {
                var composeId = Convert.ToInt64(DetailPositionDM.TargetKey);
                xqTargetItem.TargetComposeDetailContainer = new TargetCompose_ComposeDetail(composeId);
                XueQiaoFoundationHelper.SetupTargetCompose_ComposeDetail(xqTargetItem.TargetComposeDetailContainer,
                    composeGraphCacheCtrl, composeGraphQueryCtrl, userComposeViewCacheCtrl, contractItemTreeQueryCtrl,
                    XqContractNameFormatType.CommodityAcronym_Code_ContractCode,
                    _container =>
                    {
                        XqTargetDMHelper.InvalidateTargetNameWithAppropriate(xqTargetItem, XqAppLanguages.CN);
                    });

                xqTargetItem.TargetComposeUserComposeViewContainer = new UserComposeViewContainer(composeId);
                XueQiaoFoundationHelper.SetupUserComposeView(xqTargetItem.TargetComposeUserComposeViewContainer,
                    userComposeViewCacheCtrl, userComposeViewQueryCtrl, false, true,
                    _container =>
                    {
                        XqTargetDMHelper.InvalidateTargetNameWithAppropriate(xqTargetItem, XqAppLanguages.CN);
                    });
            }

            contentVM.XqTargetItem = xqTargetItem;
            contentVM.DetailPositionItem = DetailPositionDM;
            contentVM.InvalidateViewWithPositionXqTargetType(xqTargetItem.TargetType);
        }

        public void Run()
        {
            RefreshViewData();

            dialog = messageWindowService.CreateContentCustomWindow(DialogOwner, null, new Size(600,600), true, true,
                true, "详情", contentVM.View);
            dialog.ShowDialog();
        }
        
        public void Shutdown()
        {
            if (dialog != null)
            {
                dialog.Close();
                dialog = null;
            }
            loginUserManageService.HasLogouted -= RecvUserHasLogouted;
        }

        private void RecvUserHasLogouted(ProxyLoginResp lastLoginResp)
        {
            Shutdown();
        }

        private void RefreshViewData()
        {
            if (isViewDataRefreshing) return;
            isViewDataRefreshing = true;

            Task.Run(() => 
            {
                // 查询组合信息
                NativeComposeGraph targetComposeGraph = null;
                if (DetailPositionDM.TargetType == ClientXQOrderTargetType.COMPOSE_TARGET)
                {
                    var composeId = Convert.ToInt64(DetailPositionDM.TargetKey);
                    targetComposeGraph = XueQiaoFoundationHelper.QueryXQComposeGraph(composeId, 
                        composeGraphCacheCtrl, composeGraphQueryCtrl, userComposeViewCacheCtrl);
                }
                
                // 查询持仓成交的详情项信息
                var detailPositionItemsResp = QueryPositionTradeDetailItems(this.DetailPositionId);
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    isViewDataRefreshing = false;
                    
                    this.queriedComposeGraph = targetComposeGraph;

                    contentVM.PositionTradeDetailItems.Clear();

                    var srcItems = detailPositionItemsResp?.CorrectResult;
                    var tarItems = new List<PositionTradeDetailItemDM>();
                    if (srcItems != null)
                    {
                        foreach (var src in srcItems)
                        {
                            NativeComposeLeg legMeta = null;
                            if (DetailPositionDM.TargetType == ClientXQOrderTargetType.COMPOSE_TARGET)
                            {
                                legMeta = targetComposeGraph?.Legs?.FirstOrDefault(i => i.SledContractId == src.SledContractId);
                            }
                            tarItems.Add(GeneratePositionTradeDetailItemDM(src, DetailPositionDM.TargetType, DetailPositionDM.TargetKey, legMeta));
                        }
                    }
                    contentVM.PositionTradeDetailItems.AddRange(tarItems);
                });
            });
        }

        private PositionTradeDetailItemDM GeneratePositionTradeDetailItemDM(StatPositionUnit idlItem,
            ClientXQOrderTargetType parentTradeTargetType,
            string parentTradeTargetKey,
            NativeComposeLeg targetComposeLeg)
        {
            var dm = new PositionTradeDetailItemDM((int)idlItem.SledContractId,
                parentTradeTargetType,
                parentTradeTargetKey)
            {
                Direction = idlItem.Direction.ToClientTradeDirection(),
                Price = idlItem.UnitPrice,
                Quantity = idlItem.UnitQuantity,
                DataTimestampMs = idlItem.SourceDataTimestampMs
            };

            if (parentTradeTargetType == ClientXQOrderTargetType.COMPOSE_TARGET)
            {
                dm.XqComposeLegMeta = targetComposeLeg;
            }

            XueQiaoFoundationHelper.SetupTargetContract_ContractDetail(dm.ContractDetailContainer, contractItemTreeQueryCtrl, XqContractNameFormatType.CommodityAcronym_Code_ContractCode);
            return dm;
        }

        /// <summary>
        /// 查询未归档持仓详情项
        /// </summary>
        /// <param name="positionTradeItemId">持仓成交项 id</param>
        /// <returns></returns>
        private IInterfaceInteractResponse<IEnumerable<StatPositionUnit>> QueryPositionTradeDetailItems(long positionTradeItemId)
        {
            var landingInfo = loginDataService.LandingInfo;
            if (landingInfo == null) return null;
            var subAccountId = this.DetailPositionDM.SubAccountId;

            var queryPageSize = 50;
            IInterfaceInteractResponse<StatPositionUnitPage> firstPageResp = null;
            var queryAllCtrl = new QueryAllItemsByPageHelper<StatPositionUnit>(pageIndex => {
                var option = new QueryStatPositionUnitOption
                {
                    SubAccountId = subAccountId,
                    PositionItemId = positionTradeItemId
                };

                var pageOption = new IndexedPageOption { NeedTotalCount = true, PageIndex = pageIndex, PageSize = queryPageSize };
                var resp = XqThriftLibConfigurationManager.SharedInstance.TradeHostingTerminalAoHttpStub.queryStatPositionUnitPage(landingInfo, option, pageOption);
                if (resp == null) return null;
                if (pageIndex == 0)
                {
                    firstPageResp = resp;
                }
                var pageInfo = resp.CorrectResult;
                var pageResult = new QueryItemsByPageResult<StatPositionUnit>(resp.SourceException != null)
                {
                    TotalCount = pageInfo?.TotalNum,
                    Page = pageInfo?.StatPositionUnitList
                };
                return pageResult;
            });

            queryAllCtrl.RemoveDuplicateFunc = _items =>
            {
                if (_items == null) return null;
                var idGroupedItems = _items.GroupBy(i => i.UnitId);
                return idGroupedItems.Select(i => i.LastOrDefault()).ToArray();
            };

            var queriedItems = queryAllCtrl.QueryAllItems();
            if (firstPageResp == null) return null;

            var tarResp = new InterfaceInteractResponse<IEnumerable<StatPositionUnit>>(firstPageResp.Servant,
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
    }
}
