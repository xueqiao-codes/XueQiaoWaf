using ContainerShell.Applications.DataModels;
using ContainerShell.Applications.Views;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;

namespace ContainerShell.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class ContractQuickSearchPopupViewModel : ViewModel<IContractQuickSearchPopupView>
    {
        [ImportingConstructor]
        protected ContractQuickSearchPopupViewModel(IContractQuickSearchPopupView view) : base(view)
        {
            ContractSearchTree = new ContractQuickSearchTree();
            CollectionChangedEventManager.AddHandler(ContractSearchTree.Commodities, SearchedCommodityCollectionChanged);
        }

        private void SearchedCommodityCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ExistSearchCommodityResult = ContractSearchTree.Commodities.Any();
        }

        private void SelectedCommodityNodeChildrenCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ExistSearchContractResult = SelectedCommodityNode?.Children.Any() ?? false;
        }

        private bool existSearchCommodityResult;
        public bool ExistSearchCommodityResult
        {
            get { return existSearchCommodityResult; }
            private set { SetProperty(ref existSearchCommodityResult, value); }
        }

        private bool existSearchContractResult;
        public bool ExistSearchContractResult
        {
            get { return existSearchContractResult; }
            private set { SetProperty(ref existSearchContractResult, value); }
        }

        public ContractQuickSearchTree ContractSearchTree { get; private set; }

        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set { SetProperty(ref searchText, value); }
        }

        private QuickSearchCommodityNode selectedCommodityNode;
        public QuickSearchCommodityNode SelectedCommodityNode
        {
            get { return selectedCommodityNode; }
            set
            {
                var originSelNode = selectedCommodityNode;

                ExistSearchContractResult = value?.Children.Any() ?? false;

                if (SetProperty(ref selectedCommodityNode, value))
                {
                    if (originSelNode != null)
                    {
                        CollectionChangedEventManager.RemoveHandler(originSelNode.Children, SelectedCommodityNodeChildrenCollectionChanged);
                    }
                    if (selectedCommodityNode != null)
                    {
                        CollectionChangedEventManager.AddHandler(selectedCommodityNode.Children, SelectedCommodityNodeChildrenCollectionChanged);
                    }
                }
            }
        }

        private QuickSearchContractNode selectedContractNode;
        public QuickSearchContractNode SelectedContractNode
        {
            get { return selectedContractNode; }
            set { SetProperty(ref selectedContractNode, value); }
        }
        
        private bool isLoadingContracts;
        public bool IsLoadingContracts
        {
            get { return isLoadingContracts; }
            set { SetProperty(ref isLoadingContracts, value); }
        }

        private ICommand confirmSelContractCmd;
        public ICommand ConfirmSelContractCmd
        {
            get { return confirmSelContractCmd; }
            set { SetProperty(ref confirmSelContractCmd, value); }
        }

        private ICommand selectPrevContractCmd;
        public ICommand SelectPrevContractCmd
        {
            get { return selectPrevContractCmd; }
            set { SetProperty(ref selectPrevContractCmd, value); }
        }

        private ICommand selectNextContractCmd;
        public ICommand SelectNextContractCmd
        {
            get { return selectNextContractCmd; }
            set { SetProperty(ref selectNextContractCmd, value); }
        }

        private ICommand triggerShowPreviewSelPageCmd;
        public ICommand TriggerShowPreviewSelPageCmd
        {
            get { return triggerShowPreviewSelPageCmd; }
            set { SetProperty(ref triggerShowPreviewSelPageCmd, value); }
        }

        private bool isShowSeeAllContractsEntry;
        /// <summary>
        /// 是否显示查看全部合约的入口
        /// </summary>
        public bool IsShowSeeAllContractsEntry
        {
            get { return isShowSeeAllContractsEntry; }
            set { SetProperty(ref isShowSeeAllContractsEntry, value); }
        }
        
        private ICommand toSeeAllContractsCmd;
        public ICommand ToSeeAllContractsCmd
        {
            get { return toSeeAllContractsCmd; }
            set { SetProperty(ref toSeeAllContractsCmd, value); }
        }


        public void ScrollToContractSearchResultItemWithData(QuickSearchContractNode contractNode)
        {
            ViewCore.ScrollToContractSearchResultItemWithData(contractNode);
        }

        public void FocusSearchTextBox()
        {
            ViewCore.FocusSearchTextBox();
        }

        public object SearchBoxContainerElement => ViewCore.SearchBoxContainerElement;
        
        public event EventHandler Closed
        {
            add { ViewCore.Closed += value; }
            remove { ViewCore.Closed -= value; }
        }

        public void ShowPopup(object targetElement)
        {
            ViewCore.ShowPopup(targetElement);
        }

        public void Close()
        {
            ViewCore.Close();
        }
    }
}
