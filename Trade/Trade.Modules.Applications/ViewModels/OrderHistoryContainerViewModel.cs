using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class OrderHistoryContainerViewModel : ViewModel<IOrderHistoryContainerView>
    {
        [ImportingConstructor]
        protected OrderHistoryContainerViewModel(IOrderHistoryContainerView view) : base(view)
        {
            OrderHistoryListTypeSelectCmd = new DelegateCommand(DoSelectOrderHistoryListType);
        }

        public DelegateCommand OrderHistoryListTypeSelectCmd { get; private set; }
        
        private OrderHistoryListType selectedOrderHistoryListType;
        public OrderHistoryListType SelectedOrderHistoryListType
        {
            get { return selectedOrderHistoryListType; }
            set { SetProperty(ref selectedOrderHistoryListType, value); }
        }
        
        private object orderHistoryListContentView;
        public object OrderHistoryListContentView
        {
            get { return orderHistoryListContentView; }
            set { SetProperty(ref orderHistoryListContentView, value); }
        }

        private DateTime? selectedDate;
        public DateTime? SelectedDate
        {
            get { return selectedDate; }
            set { SetProperty(ref selectedDate, value); }
        }

        private long? refreshListTimestamp;
        public long? RefreshListTimestamp
        {
            get { return refreshListTimestamp; }
            set { SetProperty(ref refreshListTimestamp, value); }
        }
        
        private ICommand refreshListCmd;
        public ICommand RefreshListCmd
        {
            get { return refreshListCmd; }
            set { SetProperty(ref refreshListCmd, value); }
        }
        
        private void DoSelectOrderHistoryListType(object obj)
        {
            if (obj is OrderHistoryListType listType)
            {
                this.SelectedOrderHistoryListType = listType;
            }
        }
    }

    public enum OrderHistoryListType
    {
        Entrusted = 1,
        Parked = 2,
        Condition = 3
    }
}
