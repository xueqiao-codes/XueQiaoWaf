using Manage.Applications.Domain;
using Manage.Applications.Views;
using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.Shared.Helper;

namespace Manage.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class SettlementXqPreviewTradeItemEditVM : ViewModel<ISettlementXqPreviewTradeItemEditView>
    {
        [ImportingConstructor]
        protected SettlementXqPreviewTradeItemEditVM(ISettlementXqPreviewTradeItemEditView view) : base(view)
        {
            EnumHelper.GetAllTypesForEnum(typeof(ClientTradeDirection), out IEnumerable<ClientTradeDirection> allDirs);
            this.tradeDirections = allDirs?.ToArray();
        }

        private readonly ClientTradeDirection[] tradeDirections;
        public ClientTradeDirection[] TradeDirections
        {
            get
            {
                return tradeDirections?.ToArray();
            }
        }

        private bool isContractSelectable;
        /// <summary>
        /// 是否可以选择合约
        /// </summary>
        public bool IsContractSelectable
        {
            get { return isContractSelectable; }
            set { SetProperty(ref isContractSelectable, value); }
        }
        
        private TargetContract_TargetContractDetail selectedContractDetailContainer;
        public TargetContract_TargetContractDetail SelectedContractDetailContainer
        {
            get { return selectedContractDetailContainer; }
            set { SetProperty(ref selectedContractDetailContainer, value); }
        }

        private EditSettlementXqTradeItem editSettlementXqTradeItem;
        public EditSettlementXqTradeItem EditSettlementXqTradeItem
        {
            get { return editSettlementXqTradeItem; }
            set { SetProperty(ref editSettlementXqTradeItem, value); }
        }

        private ICommand triggerShowContractSelectPageCmd;
        public ICommand TriggerShowContractSelectPageCmd
        {
            get { return triggerShowContractSelectPageCmd; }
            set { SetProperty(ref triggerShowContractSelectPageCmd, value); }
        }

        private ICommand okCmd;
        public ICommand OkCmd
        {
            get { return okCmd; }
            set { SetProperty(ref okCmd, value); }
        }

        private ICommand cancelCmd;
        public ICommand CancelCmd
        {
            get { return cancelCmd; }
            set { SetProperty(ref cancelCmd, value); }
        }

    }
}
