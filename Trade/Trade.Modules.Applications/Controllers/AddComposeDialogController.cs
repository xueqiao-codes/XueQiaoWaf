using business_foundation_lib.helper;
using business_foundation_lib.xq_thriftlib_config;
using ContainerShell.Interfaces.Applications;
using lib.xqclient_base.thriftapi_mediation;
using lib.xqclient_base.thriftapi_mediation.Interface;
using NativeModel.Trade;
using Prism.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Waf.Foundation;
using System.Windows;
using xueqiao.trade.hosting;
using xueqiao.trade.hosting.proxy;
using xueqiao.trade.hosting.terminal.ao;
using XueQiaoFoundation.BusinessResources.Constants;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.BusinessResources.Helpers;
using XueQiaoFoundation.BusinessResources.Models;
using XueQiaoFoundation.Interfaces.Applications;
using XueQiaoFoundation.Interfaces.Helper;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Helper;
using XueQiaoWaf.Trade.Modules.Applications.ViewModels;
using XueQiaoWaf.Trade.Modules.Domain.Trades;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers
{
    /// <summary>
    /// 负责编辑、创建组合
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class AddComposeDialogController : IController
    {
        private readonly AddComposeDialogContentViewModel pageViewModel;
        private readonly IContainerShellService containerShellService;
        private readonly ILoginDataService loginDataService;
        private readonly Lazy<ILoginUserManageService> loginUserManageService;
        private readonly ICommodityQueryController commodityQueryController;
        private readonly IContractQueryController contractQueryController;
        private readonly IUserComposeViewCacheController userComposeViewCacheCtrl;
        private readonly IUserComposeViewQueryController userComposeViewQueryCtrl;
        private readonly IComposeGraphCacheController composeGraphCacheCtrl;
        private readonly IComposeGraphQueryController composeGraphQueryCtrl;
        private readonly IMessageWindowService messageWindowService;
        private readonly IContractItemTreeQueryController contractItemTreeQueryController;
        private readonly ExportFactory<CreateComposeWithSameTemplateDialogController> createComposeWithSameTemplateDialogCtrlFactory;
        private readonly IEventAggregator eventAggregator;

        private readonly DelegateCommand toCreateCommand;
        private readonly DelegateCommand cancelCommand;
        private readonly DelegateCommand toAddUnitCommand;
        private readonly DelegateCommand toDeleteUnitCommand;
        private readonly DelegateCommand toSelectUnitContractCommand;
        private readonly DelegateCommand isJoinTradeUncheckedCmd;
        private readonly DelegateCommand isJoinTradeCheckedCmd;
        private readonly DelegateCommand toSetDefaultNameCmd;

        private bool isQueringInitialData;
        
        private IMessageWindow dialog;

        [ImportingConstructor]
        public AddComposeDialogController(AddComposeDialogContentViewModel pageViewModel,
            IContainerShellService containerShellService,
            ILoginDataService loginDataService,
            Lazy<ILoginUserManageService> loginUserManageService,
            ICommodityQueryController commodityQueryController,
            IContractQueryController contractQueryController,
            IUserComposeViewCacheController userComposeViewCacheCtrl,
            IUserComposeViewQueryController userComposeViewQueryCtrl,
            IComposeGraphCacheController composeGraphCacheCtrl,
            IComposeGraphQueryController composeGraphQueryCtrl,
            IMessageWindowService messageWindowService,
            IContractItemTreeQueryController contractItemTreeQueryController,
            ExportFactory<CreateComposeWithSameTemplateDialogController> createComposeWithSameTemplateDialogCtrlFactory,
            IEventAggregator eventAggregator)
        {
            this.pageViewModel = pageViewModel;
            this.containerShellService = containerShellService;
            this.loginDataService = loginDataService;
            this.loginUserManageService = loginUserManageService;
            this.commodityQueryController = commodityQueryController;
            this.contractQueryController = contractQueryController;
            this.userComposeViewCacheCtrl = userComposeViewCacheCtrl;
            this.userComposeViewQueryCtrl = userComposeViewQueryCtrl;
            this.composeGraphCacheCtrl = composeGraphCacheCtrl;
            this.composeGraphQueryCtrl = composeGraphQueryCtrl;
            this.messageWindowService = messageWindowService;
            this.contractItemTreeQueryController = contractItemTreeQueryController;
            this.createComposeWithSameTemplateDialogCtrlFactory = createComposeWithSameTemplateDialogCtrlFactory;
            this.eventAggregator = eventAggregator;

            this.toCreateCommand = new DelegateCommand(ConfirmNewCompose, CanConfirmNewCompose);
            this.cancelCommand = new DelegateCommand(LeaveDialog);
            this.toAddUnitCommand = new DelegateCommand(ToAddUnit, CanToAddUnit);
            this.toDeleteUnitCommand = new DelegateCommand(ToDeleteUnit, CanToDeleteUnit);
            this.toSelectUnitContractCommand = new DelegateCommand(ToSelectUnitContract, CanSelectUnitContract);
            this.isJoinTradeUncheckedCmd = new DelegateCommand(IsJoinTradeUnchecked, CanCheckOrUncheckJoinTrade);
            this.isJoinTradeCheckedCmd = new DelegateCommand(IsJoinTradeChecked, CanCheckOrUncheckJoinTrade);
            this.toSetDefaultNameCmd = new DelegateCommand(ToSetDefaultName, CanSetDefaultName);

            loginUserManageService.Value.HasLogouted += RecvUserHasLogouted;
        }

        public object DialogOwner { set; get; }
        public string DialogTitle { get; set; }
        public Point? DialogShowLocationRelativeToScreen { get; set; }

        /// <summary>
        /// 通过拷贝某个组合的信息来创建新的组合。如果提供，则查询该组合信息然后设置为新组合的相应信息
        /// </summary>
        public long? NewFromCopyComposeId { get; set; }
        
        /// <summary>
        /// 完成创建的组合 id
        /// </summary>
        public long? CreatedComposeId { get; private set; }
        
        /// <summary>
        /// 创建的组合视图是否来自引用
        /// </summary>
        public bool? IsComposeViewCreatedByReference { get; private set; }

        public void Initialize()
        {
            // 初始化默认 PrecisionNumber
            pageViewModel.PrecisionNumberMin = (short)XueQiaoConstants.XQComposePrice_LowerDecimalCount;
            pageViewModel.PrecisionNumberMax = (short)XueQiaoConstants.XQComposePrice_UpperDecimalCount;
            pageViewModel.EditCompose.PrecisionNumber = pageViewModel.PrecisionNumberMin;
            
            PropertyChangedEventManager.AddHandler(this.pageViewModel, EditComposeViewModelPropertyChanged, "");
            CollectionChangedEventManager.AddHandler(this.pageViewModel.EditCompose.ComposeUnits, ComposeUnitCollectionChanged);

            this.pageViewModel.ToCreateCommand = this.toCreateCommand;
            this.pageViewModel.CancelCommand = this.cancelCommand;
            this.pageViewModel.ToAddUnitCommand = this.toAddUnitCommand;
            this.pageViewModel.ToDeleteUnitCommand = this.toDeleteUnitCommand;
            this.pageViewModel.ToSelectUnitContractCommand = this.toSelectUnitContractCommand;
            this.pageViewModel.IsJoinTradeUncheckedCmd = this.isJoinTradeUncheckedCmd;
            this.pageViewModel.IsJoinTradeCheckedCmd = this.isJoinTradeCheckedCmd;
            this.pageViewModel.ToSetDefaultNameCmd = this.toSetDefaultNameCmd;
        }
        
        public void Run()
        {
            LoadInitialData();

            this.dialog = messageWindowService.CreateContentCustomWindow(DialogOwner, DialogShowLocationRelativeToScreen, null, true, false,
                true, DialogTitle??"创建组合", pageViewModel.View);
            dialog.ShowDialog();
        }

        public void Shutdown()
        {
            InternalCloseDialog();
            loginUserManageService.Value.HasLogouted -= RecvUserHasLogouted;
            PropertyChangedEventManager.RemoveHandler(this.pageViewModel, EditComposeViewModelPropertyChanged, "");
            CollectionChangedEventManager.RemoveHandler(this.pageViewModel.EditCompose.ComposeUnits, ComposeUnitCollectionChanged);
        }

        private void RecvUserHasLogouted(ProxyLoginResp lastLoginResp)
        {
            Shutdown();
        }

        private void LoadInitialData()
        {
            var copyComposeId = this.NewFromCopyComposeId;
            Task.Run(() =>
            {
                NativeComposeView initialComposeView = null;
                NativeComposeGraph initialComposeGraph = null;
                if (copyComposeId != null)
                {
                    UpdateIsQueringInitialData(true);
                    var viewDetail = XueQiaoFoundationHelper.QueryUserComposeView(copyComposeId.Value, userComposeViewCacheCtrl, userComposeViewQueryCtrl, false, true);
                    if (viewDetail != null)
                    {
                        initialComposeView = viewDetail.UserComposeView;
                        initialComposeGraph = viewDetail.ComposeGraph;
                    }
                    else
                    {
                        initialComposeGraph = XueQiaoFoundationHelper.QueryXQComposeGraph(copyComposeId.Value, composeGraphCacheCtrl, composeGraphQueryCtrl, userComposeViewCacheCtrl);
                    }
                    UpdateIsQueringInitialData(false);
                }

                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    pageViewModel.EditCompose.ComposeUnits.Clear();

                    if (initialComposeView != null)
                    {
                        pageViewModel.EditCompose.ComposeName = initialComposeView.AliasName;
                        pageViewModel.EditCompose.PrecisionNumber = initialComposeView.PrecisionNumber;
                    }

                    if (initialComposeGraph != null)
                    {
                        pageViewModel.EditCompose.Formular = initialComposeGraph.Formular;

                        var initialLegs = initialComposeGraph.Legs?.ToArray() ?? new NativeComposeLeg[] { };
                        foreach (var leg in initialLegs)
                        {
                            pageViewModel.EditCompose.ComposeUnits.Add(GenerateComposeUnitFromLeg(leg));
                        }
                    }
                    else
                    {
                        pageViewModel.EditCompose.ComposeUnits.Add(CreateNormalComposeUnit(ClientTradeDirection.BUY));
                        pageViewModel.EditCompose.ComposeUnits.Add(CreateNormalComposeUnit(ClientTradeDirection.SELL));
                    }
                });
            });
        }

        private void UpdateIsQueringInitialData(bool value)
        {
            isQueringInitialData = value;
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                toCreateCommand?.RaiseCanExecuteChanged();
                toAddUnitCommand?.RaiseCanExecuteChanged();
                toDeleteUnitCommand?.RaiseCanExecuteChanged();
                toSelectUnitContractCommand?.RaiseCanExecuteChanged();
                isJoinTradeUncheckedCmd?.RaiseCanExecuteChanged();
                isJoinTradeCheckedCmd?.RaiseCanExecuteChanged();
                toSetDefaultNameCmd?.RaiseCanExecuteChanged();
            });
        }
        
        private AddComposeLegDetail GenerateComposeUnitFromLeg(NativeComposeLeg leg)
        {
            var contractId = (int)leg.SledContractId;
            var addUnitDetail = new AddComposeLegDetail
            {
                Quantity = leg.Quantity,
                Direction = leg.TradeDirection,
                ContractId = contractId,
                IsJoinTrade = (leg.Quantity > 0),
                LegDetailContainer = new TargetContract_TargetContractDetail(contractId)
            };

            XueQiaoFoundationHelper.SetupTargetContract_ContractDetail(addUnitDetail.LegDetailContainer,
                contractItemTreeQueryController, XqContractNameFormatType.CommodityAcronym_Code_ContractCode);
            return addUnitDetail;
        }

        private AddComposeLegDetail CreateNormalComposeUnit(ClientTradeDirection dir)
        {
            return new AddComposeLegDetail { Direction = dir, IsJoinTrade = true, Quantity = 0 };
        }

        private IInterfaceInteractResponse<IEnumerable<HostingComposeGraph>> QuerySameTemplateComposeGraphs
            (LandingInfo landingInfo, HostingComposeGraph templateCompose)
        {
            if (landingInfo == null) return null;
            if (templateCompose == null) return null;
            var queryPageSize = 20;
            IInterfaceInteractResponse<QuerySameComposeGraphsPage> firstPageResp = null;
            var queryAllCtrl = new QueryAllItemsByPageHelper<HostingComposeGraph>(pageIndex =>
            {
                var pageOption = new IndexedPageOption
                {
                    NeedTotalCount = true,
                    PageIndex = pageIndex,
                    PageSize = queryPageSize
                };

                var resp = XqThriftLibConfigurationManager.SharedInstance.TradeHostingTerminalAoHttpStub.getSameComposeGraphsPage(landingInfo, templateCompose, pageOption);
                if (resp == null) return null;
                if (pageIndex == 0)
                {
                    firstPageResp = resp;
                }
                var pageInfo = resp.CorrectResult;
                var pageResult = new QueryItemsByPageResult<HostingComposeGraph>(resp.SourceException != null)
                {
                    TotalCount = pageInfo?.TotalCount,
                    Page = pageInfo?.Graphs?.ToArray()
                };
                return pageResult;
            });
            queryAllCtrl.RetryNumberWhenFailed = 0;

            queryAllCtrl.RemoveDuplicateFunc = _items =>
            {
                if (_items == null) return null;
                var idGroupedItems = _items.GroupBy(i => i.ComposeGraphId);
                return idGroupedItems.Select(i => i.LastOrDefault()).ToArray();
            };

            var queriedItems = queryAllCtrl.QueryAllItems();
            if (firstPageResp == null) return null;

            var tarResp = new InterfaceInteractResponse<IEnumerable<HostingComposeGraph>>(firstPageResp.Servant,
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

        private bool CanConfirmNewCompose()
        {
            return !isQueringInitialData;
        }

        private void ConfirmNewCompose()
        {
            var toCreateCompose = this.pageViewModel.EditCompose;
            var errorMessage = toCreateCompose.Validate();
            if (!string.IsNullOrEmpty(errorMessage))
            {
                messageWindowService.ShowMessageDialog(GetCurrentWindow(), null, null, null, errorMessage, null);
                return;
            }

            var unitLegDetails = toCreateCompose.ComposeUnits.Cast<AddComposeLegDetail>().ToArray();
            var unitLegTickSizes = unitLegDetails.Select(i => i.LegDetailContainer?.CommodityDetail?.TickSize);
            if (unitLegTickSizes.Any(i => !i.HasValue))
            {
                messageWindowService.ShowMessageDialog(GetCurrentWindow(), null, null, null, "各腿信息未完整获取到，不能创建组合。\n请稍后再试！");
                return;
            }

            var unitLegTickSizeKeyedByVariable = new Dictionary<string, double>();
            var unitLegDirKeyedByVariable = new Dictionary<string, ClientTradeDirection>();
            for (int i = 0; i < unitLegTickSizes.Count(); i++)
            {
                var variableName = CommonConvert.IndexUnder23ToLetter(i, true, null, null);
                unitLegTickSizeKeyedByVariable[variableName] = unitLegTickSizes.ElementAt(i).Value;
                unitLegDirKeyedByVariable[variableName] = unitLegDetails[i].Direction;
            }
            
            double? composeTrialAskPrice = null;
            double? composeTrialBidPrice = null;
            string trialErrorMsg = null;

            AddComposeHelper.TrialCalculateComposeAskBidPrices(toCreateCompose.Formular, toCreateCompose.PrecisionNumber,
                _var => 
                {
                    if (unitLegTickSizeKeyedByVariable.TryGetValue(_var, out double _ts))
                        return _ts;
                    return null;
                },
                _var => 
                {
                    if (unitLegDirKeyedByVariable.TryGetValue(_var, out ClientTradeDirection _dir))
                        return _dir;
                    return null;
                },
                out composeTrialAskPrice, out composeTrialBidPrice, out trialErrorMsg);

            if (composeTrialAskPrice == null || composeTrialBidPrice == null)
            {
                if (string.IsNullOrEmpty(trialErrorMsg))
                    trialErrorMsg = "试算出错，请检查您的公式或稍后再试";
                messageWindowService.ShowMessageDialog(GetCurrentWindow(), null, null, null, trialErrorMsg, null);
                return;
            }

            var landingInfo = loginDataService.LandingInfo;
            if (landingInfo == null) return;

            AddComposeHelper.DomainAddComposeToServerCompose(toCreateCompose, out HostingComposeGraph serverCompose, out string editComposeName);
            serverCompose.CreateSubUserId = landingInfo.SubUserId;

            Task.Run(() => 
            {
                var sameTemplateResp = QuerySameTemplateComposeGraphs(landingInfo, serverCompose);
                var sameTemplates = sameTemplateResp.CorrectResult;
                if (sameTemplateResp == null || sameTemplateResp.SourceException != null || sameTemplates == null)
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        var currentWin = GetCurrentWindow();
                        if (currentWin != null)
                        {
                            var errMsg = FoundationHelper.FormatResponseDisplayErrorMsg(sameTemplateResp, "检查相同组合出错！\n");
                            messageWindowService.ShowMessageDialog(currentWin, null, null, "错误", errMsg);
                        }
                    });
                    return;
                }
                
                if (sameTemplates.Any())
                {
                    // 存在相同模板，弹出选择框
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        var currentWin = GetCurrentWindow();
                        if (currentWin == null) return;

                        var dialogCtrl = createComposeWithSameTemplateDialogCtrlFactory.CreateExport().Value;
                        dialogCtrl.DialogOwner = currentWin;
                        dialogCtrl.TemplateComposeGraph = sameTemplates.OrderBy(i => i.CreateTimestamp).FirstOrDefault();
                        dialogCtrl.MyAliasName = toCreateCompose.ComposeName;
                        dialogCtrl.MyPrecisionNumber = toCreateCompose.PrecisionNumber;
                        dialogCtrl.Initialize();
                        dialogCtrl.Run();
                        dialogCtrl.Shutdown();

                        if (dialogCtrl.CreatedComposeId != null)
                        {
                            this.CreatedComposeId = dialogCtrl.CreatedComposeId;
                            this.IsComposeViewCreatedByReference = dialogCtrl.IsComposeViewCreatedByReference;
                            InternalCloseDialog();
                        }
                    });
                    return;
                }
                else
                {
                    // 直接创建
                    var precisionNumber = toCreateCompose.PrecisionNumber;
                    var createResp = XqThriftLibConfigurationManager.SharedInstance.TradeHostingTerminalAoHttpStub
                    .createComposeGraph(landingInfo, serverCompose, editComposeName, precisionNumber);
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        var currentWin = GetCurrentWindow();
                        if (currentWin == null) return;

                        if (createResp == null || createResp.SourceException != null)
                        {
                            var errMsg = FoundationHelper.FormatResponseDisplayErrorMsg(createResp, "创建组合失败！\n");
                            messageWindowService.ShowMessageDialog(currentWin, null, null, null, errMsg, "知道了");
                            return;
                        }

                        this.CreatedComposeId = createResp.CorrectResult;
                        this.IsComposeViewCreatedByReference = false;
                        InternalCloseDialog();
                    });
                }
            });
        }
        
        private void LeaveDialog()
        {
            this.CreatedComposeId = null;
            this.IsComposeViewCreatedByReference = null;
            InternalCloseDialog();
        }

        private void ToAddUnit()
        {
            var existBuyDir = this.pageViewModel.EditCompose.ComposeUnits
                .Any(i => i.Direction == ClientTradeDirection.BUY);

            var newUnit = CreateNormalComposeUnit(existBuyDir ? ClientTradeDirection.SELL : ClientTradeDirection.BUY);
            pageViewModel.EditCompose.ComposeUnits.Add(newUnit);

            Task.Delay(200).ContinueWith(t => 
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() => 
                {
                    pageViewModel?.ScrollToComposeUnitItem(newUnit);
                });
            });
        }

        private bool CanToAddUnit()
        {
            // 限定数目
            if (isQueringInitialData) return false;

            return this.pageViewModel.EditCompose.ComposeUnits.Count < 4;
        }

        private void ToDeleteUnit(object param)
        {
            if (param is AddComposeUnit item)
            {
                this.pageViewModel.EditCompose.ComposeUnits.Remove(item);
            }
        }

        private bool CanToDeleteUnit(object param)
        {
            if (isQueringInitialData) return false;

            if (param is AddComposeUnit item)
            {
                return this.pageViewModel.EditCompose.ComposeUnits.Count > 2;
            }
            return false;
        }

        private bool CanSelectUnitContract(object param)
        {
            return !isQueringInitialData;
        }
        
        private void ToSelectUnitContract(object param)
        {
            var args = param as object[];
            if (args?.Length != 2) return;
            var legItem = args[0] as AddComposeLegDetail;
            var triggerElement = args[1];
            if (legItem == null) return;

            containerShellService.ShowContractQuickSearchPopup(triggerElement, null,
                _selContractId =>
                {
                    if (_selContractId != null)
                    {
                        legItem.ContractId = _selContractId.Value;
                        legItem.LegDetailContainer = new TargetContract_TargetContractDetail(_selContractId.Value);
                        XueQiaoFoundationHelper.SetupTargetContract_ContractDetail(legItem.LegDetailContainer, contractItemTreeQueryController,
                            XqContractNameFormatType.CommodityAcronym_Code_ContractCode, null);
                    }
                });
        }


        private bool CanCheckOrUncheckJoinTrade(object obj)
        {
            return !isQueringInitialData;
        }
        
        private void IsJoinTradeUnchecked(object obj)
        {
            if (obj is AddComposeUnit item)
            {
                item.IsJoinTrade = false;
                item.Quantity = 0;
            }
        }

        private void IsJoinTradeChecked(object obj)
        {
            if (obj is AddComposeUnit item)
            {
                item.IsJoinTrade = true;
            }
        }

        private bool CanSetDefaultName()
        {
            return !isQueringInitialData;
        }

        private void ToSetDefaultName()
        {
            var composeUnits = pageViewModel.EditCompose.ComposeUnits.Cast<AddComposeLegDetail>();
            var unitNames = new List<string>();
            foreach(var unit in composeUnits)
            {
                var unitName = unit.LegDetailContainer?.CnDisplayName;
                if (string.IsNullOrEmpty(unitName)) unitName = "--";
                unitNames.Add(unitName);
            }
            pageViewModel.EditCompose.ComposeName = XueQiaoBusinessHelper.GenerateXQComposeDefaultName(unitNames.ToArray());
        }
        
        private void EditComposeViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AddComposeDialogContentViewModel.SelectedComposeUnit))
            {
                this.toDeleteUnitCommand.RaiseCanExecuteChanged();
            }
        }
        
        private void ComposeUnitCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.toAddUnitCommand?.RaiseCanExecuteChanged();
            this.toDeleteUnitCommand?.RaiseCanExecuteChanged();
        }


        private object GetCurrentWindow()
        {
            return UIHelper.GetWindowOfUIElement(pageViewModel.View);
        }

        private void InternalCloseDialog()
        {
            if (dialog != null)
            {
                dialog.Close();
                dialog = null;
            }
        }
    }
}
