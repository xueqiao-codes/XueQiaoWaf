using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Data;
using System.Windows.Input;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoWaf.Trade.Modules.Applications.Views;
using XueQiaoWaf.Trade.Modules.Domain.Trades;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class AddComposeDialogContentViewModel : ViewModel<IAddComposeDialogContentView>
    {
        [ImportingConstructor]
        public AddComposeDialogContentViewModel(IAddComposeDialogContentView view) : base(view)
        {
            this.EditCompose = new AddCompose();
            ComposeUnitCollectionView = CollectionViewSource.GetDefaultView(EditCompose.ComposeUnits) as ListCollectionView;

            EnumHelper.GetAllTypesForEnum(typeof(ClientTradeDirection), out IEnumerable<ClientTradeDirection> allDirs);
            this.tradeDirections = allDirs;
        }

        public AddCompose EditCompose { get; private set; }
        
        public ListCollectionView ComposeUnitCollectionView { get; private set; }
        
        private short precisionNumberMin;
        public short PrecisionNumberMin
        {
            get { return precisionNumberMin; }
            set { SetProperty(ref precisionNumberMin, value); }
        }

        private short precisionNumberMax;
        public short PrecisionNumberMax
        {
            get { return precisionNumberMax; }
            set { SetProperty(ref precisionNumberMax, value); }
        }

        private ICommand toCreateCommand;
        public ICommand ToCreateCommand
        {
            get { return toCreateCommand; }
            set { SetProperty(ref toCreateCommand, value); }
        }

        private ICommand cancelCommand;
        public ICommand CancelCommand
        {
            get { return cancelCommand; }
            set { SetProperty(ref cancelCommand, value); }
        }

        private ICommand toAddUnitCommand;
        public ICommand ToAddUnitCommand
        {
            get { return toAddUnitCommand; }
            set { SetProperty(ref toAddUnitCommand, value); }
        }

        private ICommand toDeleteUnitCommand;
        public ICommand ToDeleteUnitCommand
        {
            get { return toDeleteUnitCommand; }
            set { SetProperty(ref toDeleteUnitCommand, value); }
        }

        private ICommand toSelectUnitContractCommand;
        public ICommand ToSelectUnitContractCommand
        {
            get { return toSelectUnitContractCommand; }
            set { SetProperty(ref toSelectUnitContractCommand, value); }
        }

        private ICommand isJoinTradeUncheckedCmd;
        public ICommand IsJoinTradeUncheckedCmd
        {
            get { return isJoinTradeUncheckedCmd; }
            set { SetProperty(ref isJoinTradeUncheckedCmd, value); }
        }

        private ICommand isJoinTradeCheckedCmd;
        public ICommand IsJoinTradeCheckedCmd
        {
            get { return isJoinTradeCheckedCmd; }
            set { SetProperty(ref isJoinTradeCheckedCmd, value); }
        }
        
        private ICommand toSetDefaultNameCmd;
        public ICommand ToSetDefaultNameCmd
        {
            get { return toSetDefaultNameCmd; }
            set { SetProperty(ref toSetDefaultNameCmd, value); }
        }

        private ICommand formulaHelpCmd;
        public ICommand FormulaHelpCmd
        {
            get { return formulaHelpCmd; }
            set { SetProperty(ref formulaHelpCmd, value); }
        }
        
        private AddComposeUnit selectedComposeUnit;
        public AddComposeUnit SelectedComposeUnit
        {
            get { return selectedComposeUnit; }
            set { SetProperty(ref selectedComposeUnit, value); }
        }

        public void ScrollToComposeUnitItem(AddComposeUnit unitItem)
        {
            ComposeUnitCollectionView?.MoveCurrentTo(unitItem);
        }

        private readonly IEnumerable<ClientTradeDirection> tradeDirections;

        public ClientTradeDirection[] TradeDirections
        {
            get
            {
                return tradeDirections?.ToArray();
            }
        }
    }
}
