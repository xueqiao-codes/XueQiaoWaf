using ContainerShell.Applications.DataModels;
using ContainerShell.Applications.ViewModels;
using ContainerShell.Interfaces.Applications;
using lib.xqclient_base.thriftapi_mediation.Interface;
using NativeModel.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Waf.Applications;
using xueqiao.config.contractlistrule.thriftapi;
using xueqiao.contract.standard;
using XueQiaoFoundation.BusinessResources.Helpers;
using XueQiaoFoundation.BusinessResources.Models;
using XueQiaoFoundation.Interfaces.Applications;
using XueQiaoFoundation.Interfaces.Helper;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Helper.WeakDelegate;
using XueQiaoFoundation.Shared.Interface;

namespace ContainerShell.Applications.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class ContractPreviewSelectPopupController : IController
    {
        private readonly ContractPreviewSelectPopupViewModel popupViewModel;
        
        private readonly IExchangeDataService exchangeDataService;
        private readonly IContainerShellService containerShellService;
        private readonly IContractQueryController contractQueryController;
        private readonly IContractItemTreeQueryController contractItemTreeQueryController;

        private readonly DelegateCommand okCommand;
        private readonly DelegateCommand cancelCommand;

        [ImportingConstructor]
        public ContractPreviewSelectPopupController(ContractPreviewSelectPopupViewModel popupViewModel,
            
            IExchangeDataService exchangeDataService,
            IContainerShellService containerShellService,
            IContractQueryController contractQueryController,
            IContractItemTreeQueryController contractItemTreeQueryController)
        {
            this.popupViewModel = popupViewModel;
            
            this.exchangeDataService = exchangeDataService;
            this.containerShellService = containerShellService;
            this.contractQueryController = contractQueryController;
            this.contractItemTreeQueryController = contractItemTreeQueryController;

            okCommand = new DelegateCommand(ConfirmSelect, CanConfirmSelect);
            cancelCommand = new DelegateCommand(CancelAndClosePage);
        }

        /// <summary>
        /// 浮层显示标的
        /// </summary>
        public object PopupPalcementTarget { get; set; }

        /// <summary>
        /// 初始选中的商品
        /// </summary>
        public int? InitialSelectCommodityId { get; set; }

        /// <summary>
        /// 商品数据源
        /// </summary>
        public IEnumerable<int> DataSourceCommodityIds { get; set; }

        /// <summary>
        /// 关闭回调。arg0:controller, arg1:选择的合约id
        /// </summary>
        public Action<ContractPreviewSelectPopupController, int?> ClosePopupHandler { get; set; }

        public void Initialize()
        {
            if (PopupPalcementTarget == null) throw new ArgumentNullException("PopupPalcementTarget.");
            
            PropertyChangedEventManager.AddHandler(popupViewModel, PopupViewModelPropertyChanged, "");
            PropertyChangedEventManager.AddHandler(popupViewModel.FilterFields, FilterFieldsPropertyChanged, "");

            popupViewModel.OkCommand = this.okCommand;
            popupViewModel.CancelCommand = this.cancelCommand;
            popupViewModel.Closed += popupViewModel_Closed;

            var initializedExchanges = containerShellService.InitializeData?.AllExchanges ?? new NativeExchange[] { };
            var initiazliedCommodities = containerShellService.InitializeData?.AllCommodities ?? new NativeCommodity[] { };
            if (DataSourceCommodityIds != null)
            {
                initiazliedCommodities = initiazliedCommodities.Where(i => DataSourceCommodityIds.Contains(i.SledCommodityId)).ToArray();
            }

            // 根据优先级排序交易所列表，并返回国内交易所列表和国外交易所列表
            XueQiaoBusinessHelper.GenerateSortedExchangeList(initializedExchanges,
                exchangeDataService.PreferredExchangeMicList,
                exchangeDataService.PreferredExchangeCountryAcronymList,
                exc => exchangeDataService.IsInnerExchange(exc.ExchangeMic),
                out IEnumerable<NativeExchange> innerExchangeList,
                out IEnumerable<NativeExchange> outterExchangeList);
            if (innerExchangeList == null) innerExchangeList = new NativeExchange[] { };
            if (outterExchangeList == null) outterExchangeList = new NativeExchange[] { };
            
            var exchangeComds = initiazliedCommodities.GroupBy(i => i.ExchangeMic);
            var combinedExchanges = new NativeExchange[] { }.Union(innerExchangeList).Union(outterExchangeList)
                .Where(exc => exchangeComds.Any(g => g.Key == exc.ExchangeMic));
            var contractSelectTree = new ContractSelectTree();
            contractSelectTree.UpdateTreeData(combinedExchanges, exc => exchangeComds.FirstOrDefault(g => g.Key == exc.ExchangeMic));
            popupViewModel.ContractSelectTree = contractSelectTree;
            InvalidateLastInnerExchangeNode();

            if (InitialSelectCommodityId != null)
            {
                NativeCommodity initialSelectCommodity = initiazliedCommodities.FirstOrDefault(i => i.SledCommodityId == InitialSelectCommodityId);
                NativeExchange initialSelectExchange = null;
                if (initialSelectCommodity != null)
                {
                    initialSelectExchange = initializedExchanges.FirstOrDefault(i => i.ExchangeMic == initialSelectCommodity.ExchangeMic);
                }
                if (initialSelectExchange != null)
                {
                    var initialSelExchangeNode = popupViewModel.ContractSelectTree.Exchanges.FirstOrDefault(i => i.Node.ExchangeMic == initialSelectExchange.ExchangeMic);
                    var initialSelCommoTypeNode = initialSelExchangeNode?.Children.FirstOrDefault(i => i.Node == initialSelectCommodity.SledCommodityType);
                    var initialSelCommoNode = initialSelCommoTypeNode?.Children.FirstOrDefault(i => i.Node.SledCommodityId == initialSelectCommodity.SledCommodityId);

                    popupViewModel.SelectedExchangeNode = initialSelExchangeNode;
                    popupViewModel.SelectedCommodityTypeNode = initialSelCommoTypeNode;
                    popupViewModel.SelectedCommodityNode = initialSelCommoNode;
                }
            }
        }

        public void Run()
        {
            popupViewModel.ShowPopup(PopupPalcementTarget);
        }

        public void Shutdown()
        {
            popupViewModel.Closed -= popupViewModel_Closed;
            PropertyChangedEventManager.RemoveHandler(popupViewModel, PopupViewModelPropertyChanged, "");
            PropertyChangedEventManager.RemoveHandler(popupViewModel.FilterFields, FilterFieldsPropertyChanged, "");
            ClosePopupHandler = null;
        }

        private void popupViewModel_Closed(object sender, EventArgs e)
        {
            Shutdown();
        }

        private void ConfirmSelect()
        {
            int? selectedContractId = popupViewModel.SelectedContractNode?.ContractDetailContainer.ContractId;

            popupViewModel.Closed -= popupViewModel_Closed;
            popupViewModel.Close();
            
            ClosePopupHandler?.Invoke(this, selectedContractId);
        }

        private bool CanConfirmSelect()
        {
            return (popupViewModel.SelectedContractNode != null && popupViewModel.SelectedCommodityNode != null);
        }

        private void CancelAndClosePage()
        {
            popupViewModel.SelectedContractNode = null;

            popupViewModel.Closed -= popupViewModel_Closed;
            popupViewModel.Close();

            ClosePopupHandler?.Invoke(this, null);
        }

        private void InvalidateLastInnerExchangeNode()
        {
            // 更新内外盘分割线显示
            var visiableExcNodes = popupViewModel.ContractSelectTree?.Exchanges.Where(i => i.IsVisiable).ToArray();
            popupViewModel.LastInnerExchangeNode = visiableExcNodes?.LastOrDefault(i => exchangeDataService.IsInnerExchange(i.Node.ExchangeMic));
            popupViewModel.ExistOutterExchange = visiableExcNodes?.Any(i => !exchangeDataService.IsInnerExchange(i.Node.ExchangeMic))
                ?? false;
        }

        private void PopupViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ContractPreviewSelectPopupViewModel.SelectedExchangeNode)
                || e.PropertyName == nameof(ContractPreviewSelectPopupViewModel.SelectedCommodityTypeNode)
                || e.PropertyName == nameof(ContractPreviewSelectPopupViewModel.SelectedCommodityNode)
                || e.PropertyName == nameof(ContractPreviewSelectPopupViewModel.SelectedContractNode))
            {
                okCommand.RaiseCanExecuteChanged();
            }

            if (e.PropertyName == nameof(ContractPreviewSelectPopupViewModel.SelectedExchangeNode)
                || e.PropertyName == nameof(ContractPreviewSelectPopupViewModel.SelectedCommodityTypeNode))
            {
                popupViewModel.FilterFields.FilterCommodityCode = null;
                popupViewModel.FilterFields.FilterContractCode = null;
            }

            if (e.PropertyName == nameof(ContractPreviewSelectPopupViewModel.SelectedCommodityTypeNode))
            {
                popupViewModel.FilterFields.FilterContractCode = null;
            }

            if (e.PropertyName == nameof(ContractPreviewSelectPopupViewModel.SelectedCommodityNode))
            {
                var selectedCommodityNode = popupViewModel.SelectedCommodityNode;
                if (selectedCommodityNode != null)
                {
                    DoSelectedCommodityNode(selectedCommodityNode);
                }
            }
        }

        private void FilterFieldsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ContractPreviewFilterFields.FilterExchangeMic))
            {
                FilterExchangesByExchangeMic(popupViewModel.FilterFields.FilterExchangeMic);
                InvalidateLastInnerExchangeNode();
            }

            if (e.PropertyName == nameof(ContractPreviewFilterFields.FilterCommodityCode))
            {
                FilterCommoditiesByCode(popupViewModel.FilterFields.FilterCommodityCode);
            }

            if (e.PropertyName == nameof(ContractPreviewFilterFields.FilterContractCode))
            {
                ConfigContractNodeVisiable(popupViewModel.SelectedCommodityNode.Children,
                    (idx, node) => CanPassFilterOfContractNode(node));
            }
        }

        private void FilterExchangesByExchangeMic(string _filterExchangeMic)
        {
            var filterExchangeMic = _filterExchangeMic?.Trim();
            if (popupViewModel.ContractSelectTree != null)
            {
                foreach (var _exch in popupViewModel.ContractSelectTree.Exchanges.ToArray())
                {
                    _exch.IsVisiable = string.IsNullOrWhiteSpace(filterExchangeMic) || _exch.Node.ExchangeMic.Contains(filterExchangeMic);
                }
            }
        }

        private void FilterCommoditiesByCode(string _filterCommodityCode)
        {
            var filterCommodityCode = _filterCommodityCode?.Trim();
            var selCommTypeNode = popupViewModel.SelectedCommodityTypeNode;
            if (selCommTypeNode != null)
            {
                foreach (var _commodity in selCommTypeNode.Children.ToArray())
                {
                    _commodity.IsVisiable = string.IsNullOrWhiteSpace(filterCommodityCode) || _commodity.Node.SledCommodityCode.Contains(filterCommodityCode);
                }
            }
        }

        private bool CanPassFilterOfContractNode(ContractSelectNode contractNode)
        {
            if (contractNode == null) return false;
            var nodeContractCode = contractNode.ContractDetailContainer?.ContractDetail?.SledContractCode;
            if (nodeContractCode == null) return false;

            var likeContractCode = popupViewModel.FilterFields.FilterContractCode?.Trim();
            if (!string.IsNullOrEmpty(likeContractCode)
                && true != nodeContractCode?.Contains(likeContractCode))
            {
                return false;
            }

            var nodeExchangeMic = contractNode.ContractDetailContainer?.CommodityDetail?.ExchangeMic;
            if (nodeExchangeMic == null) return false;

            // FIXME: 是否使用特殊合约的过滤规则
            var tarSpeContractCodesRule = containerShellService.InitializeData?.ContractChooseListRules?
                .FirstOrDefault(i => i.RuleType == ContractListRuleType.SpecificContractCodesOfExchange
                        && nodeExchangeMic == i.SpecificContractCodesOfExchangeRule?.ExchangeMic);
            if (tarSpeContractCodesRule == null) return true;

            return tarSpeContractCodesRule.SpecificContractCodesOfExchangeRule?.SpecificContractCodes?.Contains(nodeContractCode) 
                ?? true;
        }
        
        private void ConfigContractNodeVisiable(IEnumerable<ContractSelectNode> contractNodes, Func<int/*idx*/, ContractSelectNode, bool> nodeVisiableFactory)
        {
            var nodes = contractNodes?.ToArray();
            if (nodes?.Any() != true) return;
            for (var i = 0; i < nodes.Length; i++)
            {
                var node = nodes[i];
                var isVisiable = nodeVisiableFactory?.Invoke(i, node) ?? false;
                node.IsVisiable = isVisiable;
            }
        }

        private void DoSelectedCommodityNode(CommoditySelectNode selectedCommodityNode)
        {
            if (selectedCommodityNode == null) return;

            var selExchangeNode = popupViewModel.SelectedExchangeNode;
            var selCommTypeNode = popupViewModel.SelectedCommodityTypeNode;

            if (selectedCommodityNode.Children.Any())
            {
                // 已经获取到商品的合约列表，不必再从服务端获取
                // 触发过滤
                ConfigContractNodeVisiable(selectedCommodityNode.Children,
                    (idx, node) => CanPassFilterOfContractNode(node));
                return;
            }
            
            var handler = new Action<IInterfaceInteractResponse<IEnumerable<NativeContract>>>(
                    resp =>
                    {
                        var sortedContracts = resp.CorrectResult?.OrderBy(i => i.SledContractCode);
                        // 添加至该商品的合约列表
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            popupViewModel.IsLoadingContracts = false;
                            var contractNodes = new List<ContractSelectNode>();
                            if (sortedContracts != null)
                            {
                                foreach (var contr in sortedContracts)
                                {
                                    var contrNode = new ContractSelectNode(contr.SledContractId);
                                    // 设置 IsVisiable
                                    contrNode.IsVisiable = CanPassFilterOfContractNode(contrNode);

                                    // 设置数据
                                    XueQiaoFoundationHelper.SetupTargetContract_ContractDetail(contrNode.ContractDetailContainer, 
                                        contractItemTreeQueryController, 
                                        XqContractNameFormatType.CommodityCode_ContractCode,
                                        queriedDetail =>
                                        {
                                            contrNode.IsVisiable = CanPassFilterOfContractNode(contrNode);
                                        });
                                    contractNodes.Add(contrNode);
                                }
                            }
                            selectedCommodityNode.Children.Clear();
                            selectedCommodityNode.Children.AddRange(contractNodes);
                        });
                    });

            var handlerDelegateReference = new ActionDelegateReference<IInterfaceInteractResponse<IEnumerable<NativeContract>>>(handler,
                handler.Target == this ? false : true);

            popupViewModel.IsLoadingContracts = true;
            contractQueryController.QueryContracts(new QueryContractsByCommodityReqKey(selectedCommodityNode.Node.SledCommodityId, ContractStatus.NORMAL, null),
                handlerDelegateReference);
        }
    }
}
