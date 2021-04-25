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

namespace Manage.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class TradeSettingTabContentViewModel : ViewModel<ITradeSettingTabContentView>
    {
        [ImportingConstructor]
        protected TradeSettingTabContentViewModel(ITradeSettingTabContentView view) : base(view)
        {
            ManageItems = new ObservableCollection<ManageItemModel>();
        }
        
        public ObservableCollection<ManageItemModel> ManageItems { get; private set; }

        private ManageItemModel selectedManageItem;
        public ManageItemModel SelectedManageItem
        {
            get { return selectedManageItem; }
            set { SetProperty(ref selectedManageItem, value); }
        }
    }
}
