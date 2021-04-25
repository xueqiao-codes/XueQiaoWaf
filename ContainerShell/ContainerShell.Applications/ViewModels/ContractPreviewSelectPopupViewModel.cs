using ContainerShell.Applications.DataModels;
using ContainerShell.Applications.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Waf.Foundation;
using System.Windows.Input;

namespace ContainerShell.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class ContractPreviewSelectPopupViewModel : ViewModel<IContractPreviewSelectPopupView>
    { 
        [ImportingConstructor]
        public ContractPreviewSelectPopupViewModel(
            IContractPreviewSelectPopupView view) : base(view)
        {
            this.FilterFields = new ContractPreviewFilterFields();
        }

        private ContractSelectTree contractSelectTree;
        public ContractSelectTree ContractSelectTree
        {
            get { return contractSelectTree; }
            set { SetProperty(ref contractSelectTree, value); }
        }

        private ExchangeSelectNode selectedExchangeNode;
        public ExchangeSelectNode SelectedExchangeNode
        {
            get { return selectedExchangeNode; }
            set { SetProperty(ref selectedExchangeNode, value); }
        }

        private CommodityTypeSelectNode selectedCommodityTypeNode;
        public CommodityTypeSelectNode SelectedCommodityTypeNode
        {
            get { return selectedCommodityTypeNode; }
            set { SetProperty(ref selectedCommodityTypeNode, value); }
        }

        private CommoditySelectNode selectedCommodityNode;
        public CommoditySelectNode SelectedCommodityNode
        {
            get { return selectedCommodityNode; }
            set { SetProperty(ref selectedCommodityNode, value); }
        }

        private ContractSelectNode selectedContractNode;
        public ContractSelectNode SelectedContractNode
        {
            get { return selectedContractNode; }
            set { SetProperty(ref selectedContractNode, value); }
        }

        #region 用于交易所控制分割线显示的属性

        private ExchangeSelectNode lastInnerExchangeNode;
        /// <summary>
        /// 最后一个国内交易所
        /// </summary>
        public ExchangeSelectNode LastInnerExchangeNode
        {
            get { return lastInnerExchangeNode; }
            set { SetProperty(ref lastInnerExchangeNode, value); }
        }

        private bool existOutterExchange;
        /// <summary>
        /// 是否存在国外交易所
        /// </summary>
        public bool ExistOutterExchange
        {
            get { return existOutterExchange; }
            set { SetProperty(ref existOutterExchange, value); }
        }
        #endregion

        private bool isLoadingContracts;
        public bool IsLoadingContracts
        {
            get { return isLoadingContracts; }
            set { SetProperty(ref isLoadingContracts, value); }
        }

        public ContractPreviewFilterFields FilterFields { get; private set; }
        
        private ICommand okCommand;
        public ICommand OkCommand
        {
            get { return okCommand; }
            set { SetProperty(ref okCommand, value); }
        }

        private ICommand cancelCommand;
        public ICommand CancelCommand
        {
            get { return cancelCommand; }
            set { SetProperty(ref cancelCommand, value); }
        }

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

    public class ContractPreviewFilterFields : Model
    {
        private string filterExchangeMic;
        public string FilterExchangeMic
        {
            get { return filterExchangeMic; }
            set { SetProperty(ref filterExchangeMic, value); }
        }

        private string filterCommodityCode;
        public string FilterCommodityCode
        {
            get { return filterCommodityCode; }
            set { SetProperty(ref filterCommodityCode, value); }
        }

        private string filterContractCode;
        public string FilterContractCode
        {
            get { return filterContractCode; }
            set { SetProperty(ref filterContractCode, value); }
        }
    }
}
