using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Data;
using System.Windows.Input;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class PositionAssignHistoryVM : ViewModel<IPositionAssignHistoryView>
    {
        [ImportingConstructor]
        protected PositionAssignHistoryVM(IPositionAssignHistoryView view) : base(view)
        {
            PositionAssignedItems = new ObservableCollection<PositionAssignedDM>();
        }

        private TargetContract_TargetContractDetail selectedContractDetailContainer;
        /// <summary>
        /// 所选合约标的的用户组合视图容器
        /// </summary>
        public TargetContract_TargetContractDetail SelectedContractDetailContainer
        {
            get { return selectedContractDetailContainer; }
            set { SetProperty(ref selectedContractDetailContainer, value); }
        }

        private ICommand triggerSelectContractCmd;
        public ICommand TriggerSelectContractCmd
        {
            get { return triggerSelectContractCmd; }
            set { SetProperty(ref triggerSelectContractCmd, value); }
        }
        
        private ICommand clearSelectedContractCmd;
        public ICommand ClearSelectedContractCmd
        {
            get { return clearSelectedContractCmd; }
            set { SetProperty(ref clearSelectedContractCmd, value); }
        }

        private ICommand queryCmd;
        public ICommand QueryCmd
        {
            get { return queryCmd; }
            set { SetProperty(ref queryCmd, value); }
        }

        private DateTime? queryStartDate;
        public DateTime? QueryStartDate
        {
            get { return queryStartDate; }
            set { SetProperty(ref queryStartDate, value); }
        }

        private DateTime? queryEndDate;
        public DateTime? QueryEndDate
        {
            get { return queryEndDate; }
            set { SetProperty(ref queryEndDate, value); }
        }

        private long? queryListTimestamp;
        public long? QueryListTimestamp
        {
            get { return queryListTimestamp; }
            set { SetProperty(ref queryListTimestamp, value); }
        }

        private ICommand clickItemTargetKeyRelatedColumnCmd;
        public ICommand ClickItemTargetKeyRelatedColumnCmd
        {
            get { return clickItemTargetKeyRelatedColumnCmd; }
            set { SetProperty(ref clickItemTargetKeyRelatedColumnCmd, value); }
        }

        public ObservableCollection<PositionAssignedDM> PositionAssignedItems { get; private set; }
    }
}
