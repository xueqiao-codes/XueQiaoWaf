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
    public class FundAccountManageViewModel : ViewModel<IFundAccountManageView>
    {
        [ImportingConstructor]
        public FundAccountManageViewModel(IFundAccountManageView view) : base(view)
        {
            TradeAccountItems = new ObservableCollection<FundAccountModel>();
        }
        
        public ObservableCollection<FundAccountModel> TradeAccountItems { get; private set; }

        private ICommand toRefreshListCmd;
        public ICommand ToRefreshListCmd
        {
            get { return toRefreshListCmd; }
            set { SetProperty(ref toRefreshListCmd, value); }
        }
        
        private ICommand toAddAccountCmd;
        public ICommand ToAddAccountCmd
        {
            get { return toAddAccountCmd; }
            set { SetProperty(ref toAddAccountCmd, value); }
        }

        private ICommand toEditCmd;
        public ICommand ToEditCmd
        {
            get { return toEditCmd; }
            set { SetProperty(ref toEditCmd, value); }
        }

        private ICommand toEnableCmd;
        public ICommand ToEnableCmd
        {
            get { return toEnableCmd; }
            set { SetProperty(ref toEnableCmd, value); }
        }

        private ICommand toDisableCmd;
        public ICommand ToDisableCmd
        {
            get { return toDisableCmd; }
            set { SetProperty(ref toDisableCmd, value); }
        }

        private ICommand showExtraInfoCmd;
        public ICommand ShowExtraInfoCmd
        {
            get { return showExtraInfoCmd; }
            set { SetProperty(ref showExtraInfoCmd, value); }
        }
        
        private ICommand toRmCmd;
        public ICommand ToRmCmd
        {
            get { return toRmCmd; }
            set { SetProperty(ref toRmCmd, value); }
        }

        private object pagerContentView;
        public object PagerContentView
        {
            get { return pagerContentView; }
            set { SetProperty(ref pagerContentView, value); }
        }
    }
}
