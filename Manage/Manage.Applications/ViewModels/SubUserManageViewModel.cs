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

namespace Manage.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class SubUserManageViewModel : ViewModel<ISubUserManageView>
    {
        [ImportingConstructor]
        public SubUserManageViewModel(ISubUserManageView view) : base(view)
        {
            SubUserItems = new ObservableCollection<SubUserDataModel>();
        }
        
        public ObservableCollection<SubUserDataModel> SubUserItems { get; private set; }

        private ICommand toRefreshListCmd;
        public ICommand ToRefreshListCmd
        {
            get { return toRefreshListCmd; }
            set { SetProperty(ref toRefreshListCmd, value); }
        }
        
        private object pagerContentView;
        public object PagerContentView
        {
            get { return pagerContentView; }
            set { SetProperty(ref pagerContentView, value); }
        }
    }
}
