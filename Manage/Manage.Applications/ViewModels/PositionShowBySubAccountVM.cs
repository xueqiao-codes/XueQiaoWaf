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
using System.Windows;
using System.Windows.Input;
using xueqiao.trade.hosting;

namespace Manage.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class PositionShowBySubAccountVM : ViewModel<IPositionShowBySubAccountView>
    {
        [ImportingConstructor]
        protected PositionShowBySubAccountVM(IPositionShowBySubAccountView view) : base(view)
        {
            SubAccountItems = new ObservableCollection<HostingSubAccount>();
            PositionItems = new ObservableCollection<PositionManageDM>();
        }

        public ObservableCollection<HostingSubAccount> SubAccountItems { get; private set; }

        private HostingSubAccount selectedSubAccountItem;
        public HostingSubAccount SelectedSubAccountItem
        {
            get { return selectedSubAccountItem; }
            set { SetProperty(ref selectedSubAccountItem, value); }
        }
        
        private ICommand refreshDataCmd;
        public ICommand RefreshDataCmd
        {
            get { return refreshDataCmd; }
            set { SetProperty(ref refreshDataCmd, value); }
        }
        
        private ICommand toShowHistoryCmd;
        public ICommand ToShowHistoryCmd
        {
            get { return toShowHistoryCmd; }
            set { SetProperty(ref toShowHistoryCmd, value); }
        }

        private long? refreshTimestamp;
        public long? RefreshTimestamp
        {
            get { return refreshTimestamp; }
            set { SetProperty(ref refreshTimestamp, value); }
        }

        public ObservableCollection<PositionManageDM> PositionItems { get; private set; }
    }
}
