using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class SubscribeDataComponentHeaderViewModel : ViewModel<ISubscribeDataComponentHeaderView>
    {
        private ICommand triggerSubscribeContractCmd;
        private ICommand triggerNewComposeCmd;

        [ImportingConstructor]
        public SubscribeDataComponentHeaderViewModel(
            ISubscribeDataComponentHeaderView view) : base(view)
        {
            TogatherTabbedComponentTypes = new ObservableCollection<int>();
            TogatherTabbedComponentTypeListView = CollectionViewSource.GetDefaultView(TogatherTabbedComponentTypes) as ListCollectionView;
            TogatherTabbedComponentTypeListView.CustomSort = new IntegerAscendingComparer();
            TogatherTabbedComponentTypeListView.IsLiveSorting = true;
        }

        public ObservableCollection<int> TogatherTabbedComponentTypes { get; private set; }
        public ListCollectionView TogatherTabbedComponentTypeListView { get; private set; }

        private int selectedTabComponentType;
        public int SelectedTabComponentType
        {
            get { return selectedTabComponentType; }
            set { SetProperty(ref selectedTabComponentType, value); }
        }
        
        public ICommand TriggerSubscribeContractCmd
        {
            get { return triggerSubscribeContractCmd; }
            set { SetProperty(ref triggerSubscribeContractCmd, value); }
        }

        public ICommand TriggerNewComposeCmd
        {
            get { return triggerNewComposeCmd; }
            set { SetProperty(ref triggerNewComposeCmd, value); }
        }
    }
}
