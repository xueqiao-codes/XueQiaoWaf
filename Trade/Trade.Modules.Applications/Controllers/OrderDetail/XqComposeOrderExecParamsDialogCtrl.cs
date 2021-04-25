using NativeModel.Trade;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using xueqiao.trade.hosting.arbitrage.thriftapi;
using xueqiao.trade.hosting.proxy;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.BusinessResources.Models;
using XueQiaoFoundation.Interfaces.Applications;
using XueQiaoFoundation.Interfaces.Helper;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.ViewModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class XqComposeOrderExecParamsDialogCtrl : IController
    {
        private readonly XqComposeOrderExecParamsVM contentViewModel;
        private readonly ILoginDataService loginDataService;
        private readonly Lazy<ILoginUserManageService> loginUserManageService;
        private readonly IMessageWindowService messageWindowService;
        private readonly IEventAggregator eventAggregator;
        private readonly IContractItemTreeQueryController contractItemTreeQueryCtrl;

        private IMessageWindow dialog;

    #region Initial datas
        private XQComposeOrderExecParamsSendType? orderSendType;
        private int? earlySuspendedForMarketSeconds;
        private XqComposeOrderFirstLegChooseDM firstLegChooseDM;
        private IEnumerable<XqComposeOrderLegExecParamsDM> legExecParamsList;
    #endregion

        [ImportingConstructor]
        public XqComposeOrderExecParamsDialogCtrl(
            XqComposeOrderExecParamsVM contentViewModel,
            ILoginDataService loginDataService,
            Lazy<ILoginUserManageService> loginUserManageService,
            IMessageWindowService messageWindowService,
            IEventAggregator eventAggregator,
            IContractItemTreeQueryController contractItemTreeQueryCtrl)
        {
            this.contentViewModel = contentViewModel;
            this.loginDataService = loginDataService;
            this.loginUserManageService = loginUserManageService;
            this.messageWindowService = messageWindowService;
            this.eventAggregator = eventAggregator;
            this.contractItemTreeQueryCtrl = contractItemTreeQueryCtrl;

            loginUserManageService.Value.HasLogouted += RecvUserHasLogouted;
        }

        public object DialogOwner { get; set; }

        /// <summary>
        /// 雪橇组合信息
        /// </summary>
        public NativeComposeGraph ComposeGraph { get; set; }

        /// <summary>
        /// 订单执行参数
        /// </summary>
        public HostingXQComposeLimitOrderExecParams OrderExecParams { get; set; }

        public void Initialize()
        {
            if (ComposeGraph == null) throw new ArgumentNullException("ComposeGraph");
            if (OrderExecParams == null) throw new ArgumentNullException("OrderExecParams");

            ConstructInitialDatas();

            contentViewModel.OrderSendType = this.orderSendType;
            contentViewModel.EarlySuspendedForMarketSeconds = this.earlySuspendedForMarketSeconds;
            contentViewModel.FirstLegChooseDM = this.firstLegChooseDM;
            contentViewModel.LegExecParamsList.Clear();
            contentViewModel.LegExecParamsList.AddRange(this.legExecParamsList);
        }
        
        public void Run()
        {
            dialog = messageWindowService.CreateContentCustomWindow(DialogOwner, null, new Size(700, 600), true, true,
                true, "订单执行参数", contentViewModel.View);
            dialog.ShowDialog();
        }

        public void Shutdown()
        {
            if (dialog != null)
            {
                dialog.Close();
                dialog = null;
            }
            loginUserManageService.Value.HasLogouted -= RecvUserHasLogouted;
        }

        private void RecvUserHasLogouted(ProxyLoginResp lastLoginResp)
        {
            Shutdown();
        }

        private void ConstructInitialDatas()
        {
            var legs = this.ComposeGraph.Legs?.ToArray() ?? new NativeComposeLeg[] { };
            var orderExecParams = this.OrderExecParams;

            XQComposeOrderExecParamsSendType? orderSendType = null;
            XqComposeOrderFirstLegChooseDM firstLegChooseDM = null;
            var legExecParamsList = new List<XqComposeOrderLegExecParamsDM>();

            if (orderExecParams.ExecType == HostingXQComposeLimitOrderExecType.PARALLEL_LEG)
            {
                orderSendType = XQComposeOrderExecParamsSendType.PARALLEL_LEG;
                var srcParallelTypeParams = orderExecParams.ExecParallelParams;
                if (srcParallelTypeParams != null)
                {
                    foreach (var leg in legs)
                    {
                        var paramsDM = new XqComposeOrderLegExecParamsDM((int)leg.SledContractId, orderSendType.Value);
                        paramsDM.LegDetail = new ComposeLegDetail(leg);
                        XueQiaoFoundationHelper.SetupTargetContract_ContractDetail(paramsDM.LegDetail.LegDetailContainer,
                            contractItemTreeQueryCtrl, XqContractNameFormatType.CommodityAcronym_Code_ContractCode);

                        var parallelTypeParamsDM = paramsDM.ParallelTypeParams;
                        if (true == srcParallelTypeParams.LegSendOrderExtraParam?.ContainsKey(leg.SledContractId))
                        {
                            parallelTypeParamsDM.SendOrderExtraParam = srcParallelTypeParams.LegSendOrderExtraParam[leg.SledContractId];
                        }
                        if (true == srcParallelTypeParams.LegChaseParams?.ContainsKey(leg.SledContractId))
                        {
                            parallelTypeParamsDM.ChaseParam = srcParallelTypeParams.LegChaseParams[leg.SledContractId];
                        }

                        legExecParamsList.Add(paramsDM);
                    }
                }
            }

            else if (orderExecParams.ExecType == HostingXQComposeLimitOrderExecType.LEG_BY_LEG)
            {
                var srcTypeParams = orderExecParams.ExecLegByParams;
                if (srcTypeParams != null)
                {
                    if (srcTypeParams.LegByTriggerType == HostingXQComposeLimitOrderLegByTriggerType.PRICE_BEST)
                        orderSendType = XQComposeOrderExecParamsSendType.SERIAL_LEG_PRICE_BEST;
                    else if (srcTypeParams.LegByTriggerType == HostingXQComposeLimitOrderLegByTriggerType.PRICE_TRYING)
                        orderSendType = XQComposeOrderExecParamsSendType.SERIAL_LEG_PRICE_TRYING;

                    if (srcTypeParams.FirstLegChooseParam != null)
                    {
                        firstLegChooseDM = new XqComposeOrderFirstLegChooseDM(srcTypeParams.FirstLegChooseParam.Mode, (int)srcTypeParams.FirstLegChooseParam.AppointSledContractId);
                        if (srcTypeParams.FirstLegChooseParam.Mode == HostingXQComposeLimitOrderFirstLegChooseMode.FIRST_LEG_CHOOSE_MODE_APPOINT)
                        {
                            firstLegChooseDM.AppointContractDetailContainer = new TargetContract_TargetContractDetail((int)srcTypeParams.FirstLegChooseParam.AppointSledContractId);
                            XueQiaoFoundationHelper.SetupTargetContract_ContractDetail(firstLegChooseDM.AppointContractDetailContainer, contractItemTreeQueryCtrl, XqContractNameFormatType.CommodityAcronym_Code_ContractCode);
                        }
                    }

                    if (orderSendType != null)
                    {
                        foreach (var leg in legs)
                        {
                            var paramsDM = new XqComposeOrderLegExecParamsDM((int)leg.SledContractId, orderSendType.Value);
                            paramsDM.LegDetail = new ComposeLegDetail(leg);
                            XueQiaoFoundationHelper.SetupTargetContract_ContractDetail(paramsDM.LegDetail.LegDetailContainer,
                                contractItemTreeQueryCtrl, XqContractNameFormatType.CommodityAcronym_Code_ContractCode);

                            XqComposeOrderLegParams_SerialLeg typeParamsDM = null;
                            if (orderSendType == XQComposeOrderExecParamsSendType.SERIAL_LEG_PRICE_BEST)
                                typeParamsDM = paramsDM.SerialLegPriceBestTypeParams;
                            else if (orderSendType == XQComposeOrderExecParamsSendType.SERIAL_LEG_PRICE_TRYING)
                                typeParamsDM = paramsDM.SerialLegPriceTryingTypeParams;

                            if (typeParamsDM != null)
                            {
                                if (true == srcTypeParams.LegSendOrderExtraParam?.ContainsKey(leg.SledContractId))
                                {
                                    typeParamsDM.SendOrderExtraParam = srcTypeParams.LegSendOrderExtraParam[leg.SledContractId];
                                }
                                if (true == srcTypeParams.LegChaseParams?.ContainsKey(leg.SledContractId))
                                {
                                    typeParamsDM.ChaseParam = srcTypeParams.LegChaseParams[leg.SledContractId];
                                }
                                if (true == srcTypeParams.FirstLegExtraParams?.ContainsKey(leg.SledContractId))
                                {
                                    typeParamsDM.FirstLegExtraParam = srcTypeParams.FirstLegExtraParams[leg.SledContractId];
                                }
                                if (orderSendType == XQComposeOrderExecParamsSendType.SERIAL_LEG_PRICE_TRYING
                                    && typeParamsDM is XqComposeOrderLegParams_SerialPriceTrying)
                                {
                                    if (true == srcTypeParams.FirstLegTryingParams?.ContainsKey(leg.SledContractId))
                                    {
                                        ((XqComposeOrderLegParams_SerialPriceTrying)typeParamsDM).FirstLegTryingParam = srcTypeParams.FirstLegTryingParams[leg.SledContractId];
                                    }
                                }
                            }

                            legExecParamsList.Add(paramsDM);
                        }
                    }
                }
            }

            this.orderSendType = orderSendType;

            if (orderExecParams.__isset.earlySuspendedForMarketSeconds)
            {
                this.earlySuspendedForMarketSeconds = orderExecParams.EarlySuspendedForMarketSeconds;
            }

            this.firstLegChooseDM = firstLegChooseDM;
            this.legExecParamsList = legExecParamsList?.ToArray();
        }
    }
}
