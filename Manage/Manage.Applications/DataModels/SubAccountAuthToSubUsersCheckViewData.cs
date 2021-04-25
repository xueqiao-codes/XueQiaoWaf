using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Data;

namespace Manage.Applications.DataModels
{
    public class SubAccountAuthToSubUsersCheckViewData
    {
        public SubAccountAuthToSubUsersCheckViewData()
        {
            this.ToCheckSubUsers = new ObservableCollection<SubUserSelectModel>();

            var syncSubUsers = new SynchronizingCollection<SubUserSelectModel, SubUserSelectModel>(ToCheckSubUsers, i => i);
            CheckedSubUserCollectionView = CollectionViewSource.GetDefaultView(syncSubUsers) as ListCollectionView;
            CheckedSubUserCollectionView.LiveFilteringProperties.Add(nameof(SubUserSelectModel.IsChecked));
            CheckedSubUserCollectionView.Filter = i =>
            {
                if (i is SubUserSelectModel _dm)
                {
                    return _dm.IsChecked;
                }
                return false;
            };
            CheckedSubUserCollectionView.IsLiveFiltering = true;
        }

        public ObservableCollection<SubUserSelectModel> ToCheckSubUsers { get; private set; }

        public ListCollectionView CheckedSubUserCollectionView { get; private set; }
    }
}
