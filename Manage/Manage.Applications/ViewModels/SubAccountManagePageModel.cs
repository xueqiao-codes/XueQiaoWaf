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
    public class SubAccountManagePageModel : ViewModel<ISubAccountManagePage>
    {
        [ImportingConstructor]
        public SubAccountManagePageModel(ISubAccountManagePage view) : base(view)
        {
            SubAccountItems = new ObservableCollection<SubAccountDataModel>();
        }

        public object DisplayInWindow => ViewCore.DisplayInWindow;

        public ObservableCollection<SubAccountDataModel> SubAccountItems { get; private set; }

        private ICommand toRefreshListCmd;
        public ICommand ToRefreshListCmd
        {
            get { return toRefreshListCmd; }
            set { SetProperty(ref toRefreshListCmd, value); }
        }

        private ICommand toAddSubAccountCmd;
        public ICommand ToAddSubAccountCmd
        {
            get { return toAddSubAccountCmd; }
            set { SetProperty(ref toAddSubAccountCmd, value); }
        }

        private ICommand toAuthToSubUserCmd;
        public ICommand ToAuthToSubUserCmd
        {
            get { return toAuthToSubUserCmd; }
            set { SetProperty(ref toAuthToSubUserCmd, value); }
        }

        private ICommand toShowInOutFundHistoryCmd;
        public ICommand ToShowInOutFundHistoryCmd
        {
            get { return toShowInOutFundHistoryCmd; }
            set { SetProperty(ref toShowInOutFundHistoryCmd, value); }
        }

        private ICommand toInFundCmd;
        public ICommand ToInFundCmd
        {
            get { return toInFundCmd; }
            set { SetProperty(ref toInFundCmd, value); }
        }

        private ICommand toOutFundCmd;
        public ICommand ToOutFundCmd
        {
            get { return toOutFundCmd; }
            set { SetProperty(ref toOutFundCmd, value); }
        }

        private ICommand toSetOrderRouteCmd;
        public ICommand ToSetOrderRouteCmd
        {
            get { return toSetOrderRouteCmd; }
            set { SetProperty(ref toSetOrderRouteCmd, value); }
        }

        private ICommand triggerRenameCmd;
        public ICommand TriggerRenameCmd
        {
            get { return triggerRenameCmd; }
            set { SetProperty(ref triggerRenameCmd, value); }
        }

        private object pagerContentView;
        public object PagerContentView
        {
            get { return pagerContentView; }
            set { SetProperty(ref pagerContentView, value); }
        }
    }
}
