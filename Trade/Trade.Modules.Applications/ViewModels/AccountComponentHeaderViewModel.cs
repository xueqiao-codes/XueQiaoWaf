using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Data;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class AccountComponentHeaderViewModel : ViewModel<IAccountComponentHeaderView>
    {
        [ImportingConstructor]
        public AccountComponentHeaderViewModel(IAccountComponentHeaderView view) : base(view)
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
        

        //private TradeComponent componentInfo;
        //public TradeComponent ComponentInfo
        //{
        //    get { return componentInfo; }
        //    set
        //    {
        //        if (componentInfo != null)
        //        {
        //            CollectionChangedEventManager.RemoveHandler(componentInfo.AllPinnedComponentTypes, ComponentPinnedComponentTypeCollectionChanged);
        //        }
        //        SetProperty(ref componentInfo, value);
        //        PinnedComponentTypeListView = CollectionViewSource.GetDefaultView(componentInfo.AllPinnedComponentTypes) as ListCollectionView;
        //        PinnedComponentTypeListView.IsLiveSorting = true;
        //        PinnedComponentTypeListView.CustomSort = new IntegerAscendingComparer();
        //        if (componentInfo != null)
        //        {
        //            CollectionChangedEventManager.AddHandler(componentInfo.AllPinnedComponentTypes, ComponentPinnedComponentTypeCollectionChanged);
        //        }
        //    }
        //}

        //private ListCollectionView pinnedComponentTypeListView;
        //public ListCollectionView PinnedComponentTypeListView
        //{
        //    get { return pinnedComponentTypeListView; }
        //    private set { SetProperty(ref pinnedComponentTypeListView, value); }
        //}

        //private void ComponentPinnedComponentTypeCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    if (PinnedComponentTypeListView.CurrentItem == null)
        //    {
        //        PinnedComponentTypeListView.MoveCurrentToFirst();
        //    }
        //}
    }
}
