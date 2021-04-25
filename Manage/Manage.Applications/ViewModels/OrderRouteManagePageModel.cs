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
    public class OrderRouteManagePageModel : ViewModel<IOrderRouteManagePage>
    {
        [ImportingConstructor]
        protected OrderRouteManagePageModel(IOrderRouteManagePage view) : base(view)
        {
            OrderRouteRuleItems = new ObservableCollection<OrderRouteRuleListItemModel>();
        }
        
        public ObservableCollection<OrderRouteRuleListItemModel> OrderRouteRuleItems { get; private set; }

        private HostingSubAccount subAccount;
        public HostingSubAccount SubAccount
        {
            get { return subAccount; }
            set { SetProperty(ref subAccount, value); }
        }

        private ICommand goBackCmd;
        public ICommand GoBackCmd
        {
            get { return goBackCmd; }
            set { SetProperty(ref goBackCmd, value); }
        }
        
        private ICommand toRefreshListCmd;
        public ICommand ToRefreshListCmd
        {
            get { return toRefreshListCmd; }
            set { SetProperty(ref toRefreshListCmd, value); }
        }

        private ICommand toAddRouteRuleCmd;
        public ICommand ToAddRouteRuleCmd
        {
            get { return toAddRouteRuleCmd; }
            set { SetProperty(ref toAddRouteRuleCmd, value); }
        }

        private ICommand toEditRouteRuleCmd;
        public ICommand ToEditRouteRuleCmd
        {
            get { return toEditRouteRuleCmd; }
            set { SetProperty(ref toEditRouteRuleCmd, value); }
        }

        private ICommand toRmRouteRuleCmd;
        public ICommand ToRmRouteRuleCmd
        {
            get { return toRmRouteRuleCmd; }
            set { SetProperty(ref toRmRouteRuleCmd, value); }
        }
    }
}
