using Manage.Applications.DataModels;
using Manage.Applications.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;
using xueqiao.trade.hosting;

namespace Manage.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class SubAccountPositionHistoryVM : ViewModel<ISubAccountPositionHistoryView>
    {
        [ImportingConstructor]
        protected SubAccountPositionHistoryVM(ISubAccountPositionHistoryView view) : base(view)
        {
            PositionItems = new ObservableCollection<SubAccHistoryPositionDM>();
        }

        private HostingSubAccount subAccount;
        public HostingSubAccount SubAccount
        {
            get { return subAccount; }
            set { SetProperty(ref subAccount, value); }
        }
        
        private ICommand pageGoBackCmd;
        public ICommand PageGoBackCmd
        {
            get { return pageGoBackCmd; }
            set { SetProperty(ref pageGoBackCmd, value); }
        }
        
        private DateTime? selectedDate;
        public DateTime? SelectedDate
        {
            get { return selectedDate; }
            set { SetProperty(ref selectedDate, value); }
        }

        public ObservableCollection<SubAccHistoryPositionDM> PositionItems { get; private set; }

        private object positionItemsPagerView;
        public object PositionItemsPagerView
        {
            get { return positionItemsPagerView; }
            set { SetProperty(ref positionItemsPagerView, value); }
        }
    }
}
