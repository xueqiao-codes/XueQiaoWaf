using ContainerShell.Applications.DataModels;
using ContainerShell.Applications.Helper;
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
using System.Threading.Tasks;
using System.Waf.Applications;
using xueqiao.contract.standard;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.BusinessResources.Models;
using XueQiaoFoundation.Interfaces.Applications;
using XueQiaoFoundation.Interfaces.Helper;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Helper.WeakDelegate;
using XueQiaoFoundation.Shared.Interface;

namespace ContainerShell.Applications.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class ContractQuickSearchPopupController : IController
    {
        // 合约搜索结果数目限定
        private const int QuickSearchContractsLimitSize = 12;

        private readonly ContractQuickSearchPopupViewModel popupViewModel;
        
        private readonly IContainerShellService containerShellService;
        private readonly IContractQueryController contractQueryController;
        private readonly IContractItemTreeQueryController contractItemTreeQueryController;
        private readonly ExportFactory<ContractPreviewSelectPopupController> contractPreviewSelectCtrlFactory;

        private readonly DelegateCommand triggerShowPreviewSelPageCmd;
        private readonly DelegateCommand selectPrevContractCmd;
        private readonly DelegateCommand selectNextContractCmd;
        private readonly DelegateCommand confirmSelContractCmd;
        private readonly DelegateCommand toSeeAllContractsCmd;
        private readonly List<QuickSearchCommodityNode> cachedSearchCommodityNodes = new List<QuickSearchCommodityNode>();

        [ImportingConstructor]
        public ContractQuickSearchPopupController(ContractQuickSearchPopupViewModel popupViewModel,
            
            IContainerShellService containerShellService,
            IContractQueryController contractQueryController,
            IContractItemTreeQueryController contractItemTreeQueryController,
            ExportFactory<ContractPreviewSelectPopupController> contractPreviewSelectCtrlFactory)
        {
            this.popupViewModel = popupViewModel;
            
            this.containerShellService = containerShellService;
            this.contractQueryController = contractQueryController;
            this.contractItemTreeQueryController = contractItemTreeQueryController;
            this.contractPreviewSelectCtrlFactory = contractPreviewSelectCtrlFactory;

            triggerShowPreviewSelPageCmd = new DelegateCommand(TriggerShowPreviewSelPage);
            selectPrevContractCmd = new DelegateCommand(SelectPrevContract);
            selectNextContractCmd = new DelegateCommand(SelectNextContract);
            confirmSelContractCmd = new DelegateCommand(ConfirmSelContract);
            toSeeAllContractsCmd = new DelegateCommand(ToSeeAllContracts);
        }

        /// <summary>
        /// 弹层位置目标
        /// </summary>
        public object PopupPalcementTarget { get; set; }

        /// <summary>
        /// 关闭回调。arg0:controller, arg1:选择的合约id
        /// </summary>
        public Action<ContractQuickSearchPopupController, int?> PopupCloseHandler { get; set; }

        /// <summary>
        /// 商品数据源
        /// </summary>
        public IEnumerable<int> DataSourceCommodityIds { get; set; }

        public void Initialize()
        {
            popupViewModel.Closed += PopupViewModel_Closed;
            popupViewModel.ConfirmSelContractCmd = confirmSelContractCmd;
            popupViewModel.SelectPrevContractCmd = selectPrevContractCmd;
            popupViewModel.SelectNextContractCmd = selectNextContractCmd;
            popupViewModel.TriggerShowPreviewSelPageCmd = triggerShowPreviewSelPageCmd;
            popupViewModel.ToSeeAllContractsCmd = toSeeAllContractsCmd;
            PropertyChangedEventManager.AddHandler(popupViewModel, PopupViewModelPropChanged, "");
        }

        public void Run()
        {
            popupViewModel.ShowPopup(PopupPalcementTarget);
        }

        public void Shutdown()
        {
            cachedSearchCommodityNodes.Clear();
            popupViewModel.Closed -= PopupViewModel_Closed;
            PropertyChangedEventManager.RemoveHandler(popupViewModel, PopupViewModelPropChanged, "");
            PopupCloseHandler = null;
        }

        private void PopupViewModel_Closed(object sender, EventArgs e)
        {
            Shutdown();
        }

        private void PopupViewModelPropChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ContractQuickSearchPopupViewModel.SearchText))
            {
                FilterCommoditiesWithSearchText(popupViewModel.SearchText);
                return;
            }
            if (e.PropertyName == nameof(ContractQuickSearchPopupViewModel.SelectedCommodityNode))
            {
                DidSelectedCommodityNode(popupViewModel.SelectedCommodityNode);
                return;
            }
        }

        private void TriggerShowPreviewSelPage(object triggerElement)
        {
            var popupCtrl = contractPreviewSelectCtrlFactory.CreateExport().Value;
            popupCtrl.PopupPalcementTarget = triggerElement;
            popupCtrl.ClosePopupHandler = (_ctrl, _selectedContractId) => {
                _ctrl.Shutdown();
                if (_selectedContractId != null)
                {
                    popupViewModel.Closed -= PopupViewModel_Closed;
                    popupViewModel.Close();
                    PopupCloseHandler?.Invoke(this, _selectedContractId);
                }
                else
                {
                    popupViewModel.FocusSearchTextBox();
                }
            };

            popupCtrl.Initialize();
            popupCtrl.Run();
        }

        private void SelectPrevContract()
        {
            SelectContractNodeOffset2Current(-1);
        }

        private void SelectNextContract()
        {
            SelectContractNodeOffset2Current(1);
        }
        
        private void ConfirmSelContract()
        {
            int? selectedContractId = popupViewModel.SelectedContractNode?.ContractDetailContainer.ContractId;
            if (selectedContractId == null) return;

            popupViewModel.Closed -= PopupViewModel_Closed;
            popupViewModel.Close();

            this.PopupCloseHandler?.Invoke(this, selectedContractId);
        }

        private void ToSeeAllContracts()
        {
            var selCommodityId = popupViewModel.SelectedCommodityNode?.Node.CommodityId;
            if (selCommodityId == null) return;

            var popupCtrl = contractPreviewSelectCtrlFactory.CreateExport().Value;
            popupCtrl.PopupPalcementTarget = popupViewModel.SearchBoxContainerElement;
            popupCtrl.InitialSelectCommodityId = selCommodityId.Value;
            popupCtrl.ClosePopupHandler = (_ctrl, _selectedContractId) => {
                _ctrl.Shutdown();
                if (_selectedContractId != null)
                {
                    popupViewModel.Closed -= PopupViewModel_Closed;
                    popupViewModel.Close();
                    PopupCloseHandler?.Invoke(this, _selectedContractId);
                }
                else
                {
                    popupViewModel.FocusSearchTextBox();
                }
            };

            popupCtrl.Initialize();
            popupCtrl.Run();
        }

        private void SelectContractNodeOffset2Current(int offset2CurrentNode)
        {
            if (offset2CurrentNode == 0) return;

            var searchedContractNodes = popupViewModel.SelectedCommodityNode?.Children;
            if (searchedContractNodes?.Any() != true) return;

            var originSelContractNode = popupViewModel.SelectedContractNode;
            QuickSearchContractNode newSelNode = null;
            if (originSelContractNode == null)
            {
                newSelNode = searchedContractNodes.FirstOrDefault();
            }
            else
            {
                var originSelNodeIdx = searchedContractNodes.IndexOf(originSelContractNode);
                if (originSelNodeIdx < 0)
                    newSelNode = searchedContractNodes.FirstOrDefault();
                else
                    newSelNode = searchedContractNodes.ElementAtOrDefault(originSelNodeIdx + offset2CurrentNode);
            }

            popupViewModel.SelectedContractNode = newSelNode;
            if (newSelNode != null)
            {
                popupViewModel.ScrollToContractSearchResultItemWithData(newSelNode);
            }
        }

        private void FilterCommoditiesWithSearchText(string searchText)
        {
            var searchGraph = ContractQuickSearchHelper.GetContractSearchTextGraph(searchText);
#if DEBUG
            Console.WriteLine($"FilterCommodities, searchGraph:{searchGraph}");
#endif
            popupViewModel.ContractSearchTree.Commodities.Clear();

            if (searchGraph == null || string.IsNullOrEmpty(searchGraph.CommoditySearchPart)) return;

            var limitCommodityShowNum = 10;
            var dataSourceCommoditis = containerShellService.InitializeData?.AllCommodities;
            if (DataSourceCommodityIds != null)
            {
                dataSourceCommoditis = dataSourceCommoditis.Where(i => DataSourceCommodityIds.Contains(i.SledCommodityId)).ToArray();
            }
            var tarCommodities = ContractQuickSearchHelper.SearchMostMatchedCommodities(dataSourceCommoditis,
                searchGraph.CommoditySearchPart, XqAppLanguages.CN, limitCommodityShowNum);
            if (tarCommodities == null) return;
            
            foreach (var tarCommodity in tarCommodities)
            {
                var searchCommodityNode = cachedSearchCommodityNodes.FirstOrDefault(i => i.Node?.CommodityId == tarCommodity.SledCommodityId);
                if (searchCommodityNode == null)
                {
                    var commodityContainer = new CommodityDetailContainer(tarCommodity.SledCommodityId);
                    XueQiaoFoundationHelper.SetupCommodityDetailContainer(commodityContainer, contractItemTreeQueryController, true);

                    searchCommodityNode = new QuickSearchCommodityNode { Node = commodityContainer };
                    cachedSearchCommodityNodes.Add(searchCommodityNode);
                }
                popupViewModel.ContractSearchTree.Commodities.Add(searchCommodityNode);
            }
            popupViewModel.SelectedCommodityNode = popupViewModel.ContractSearchTree.Commodities.FirstOrDefault();

            //var letterPieces = ContractQuickSearchHelper.GetLetterPieces(searchText);

            //popupViewModel.ContractSearchTree.Commodities.Clear();
            //if (letterPieces?.Any() == true)
            //{
            //    var limitCommodityShowNum = 5;
            //    var sourceCommodities = containerShellService.InitializeData?.AllCommodities;
            //    var _innerSource = sourceCommodities;
            //    var tarCommodities = new List<NativeCommodity>();
            //    foreach (var piece in letterPieces)
            //    {
            //        _innerSource = _innerSource.Except(tarCommodities);
            //        var results = ContractQuickSearchHelper.SearchMostMatchedCommodities(_innerSource, piece, limitCommodityShowNum);
            //        if (results?.Any() == true)
            //        {
            //            tarCommodities.AddRange(results);
            //        }

            //        if (tarCommodities.Count >= limitCommodityShowNum)
            //            break;
            //    }

            //    foreach (var tarCommodity in tarCommodities)
            //    {
            //        var searchCommodityNode = cachedSearchCommodityNodes.FirstOrDefault(i => i.Node?.CommodityId == tarCommodity.SledCommodityId);
            //        if (searchCommodityNode == null)
            //        {
            //            var commodityContainer = new CommodityDetailContainer(tarCommodity.SledCommodityId);
            //            DataModelDatasLoadHelper.SetupCommodityDetailContainer(commodityContainer, contractItemTreeQueryController, true);

            //            searchCommodityNode = new QuickSearchCommodityNode { Node = commodityContainer };
            //            cachedSearchCommodityNodes.Add(searchCommodityNode);
            //        }
            //        popupViewModel.ContractSearchTree.Commodities.Add(searchCommodityNode);
            //    }
            //    popupViewModel.SelectedCommodityNode = popupViewModel.ContractSearchTree.Commodities.FirstOrDefault();
            //}
        }

        private void DidSelectedCommodityNode(QuickSearchCommodityNode selCommodityNode)
        {
            if (selCommodityNode == null) return;
            if (selCommodityNode.Children.Any())
            {
                // 该商品已查询到合约，不必再查询
                popupViewModel.IsShowSeeAllContractsEntry = (selCommodityNode.Children.Count >= QuickSearchContractsLimitSize);
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
                            var contractNodes = new List<QuickSearchContractNode>();
                            if (sortedContracts != null)
                            {
                                foreach (var contr in sortedContracts)
                                {
                                    var contrNode = new QuickSearchContractNode(contr.SledContractId);
                                    // 设置数据
                                    XueQiaoFoundationHelper.SetupTargetContract_ContractDetail(contrNode.ContractDetailContainer,
                                        contractItemTreeQueryController,
                                        XqContractNameFormatType.CommodityCode_ContractCode);
                                    contractNodes.Add(contrNode);
                                }
                            }
                            selCommodityNode.Children.Clear();
                            selCommodityNode.Children.AddRange(contractNodes);
                            popupViewModel.IsShowSeeAllContractsEntry = (contractNodes.Count >= QuickSearchContractsLimitSize);
                        });
                    });

            var handlerDelegateReference = new ActionDelegateReference<IInterfaceInteractResponse<IEnumerable<NativeContract>>>(handler,
                handler.Target == this ? false : true);

            popupViewModel.IsLoadingContracts = true;
            popupViewModel.IsShowSeeAllContractsEntry = false;
            contractQueryController.QueryContracts(new QueryContractsByCommodityReqKey(selCommodityNode.Node.CommodityId, ContractStatus.NORMAL, QuickSearchContractsLimitSize),
                handlerDelegateReference);
        }
    }
}
