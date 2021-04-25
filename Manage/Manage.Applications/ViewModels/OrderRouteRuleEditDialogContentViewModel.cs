using Manage.Applications.DataModels;
using Manage.Applications.Views;
using NativeModel.Contract;
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
    public class OrderRouteRuleEditDialogContentViewModel : ViewModel<IOrderRouteRuleEditDialogContentView>
    {
        [ImportingConstructor]
        public OrderRouteRuleEditDialogContentViewModel(IOrderRouteRuleEditDialogContentView view) : base(view)
        {
            RuleCommodities = new ObservableCollection<OrderRouteRuleCommoditySelectModel>();
            RouteToAccountItems = new ObservableCollection<HostingTradeAccount>();
        }

        public object DisplayInWindow => ViewCore.DisplayInWindow;

        public void CloseDisplayInWindow()
        {
            ViewCore.CloseDisplayInWindow();
        }

        private ICommand saveCmd;
        public ICommand SaveCmd
        {
            get { return saveCmd; }
            set { SetProperty(ref saveCmd, value); }
        }

        private ICommand cancelCmd;
        public ICommand CancelCmd
        {
            get { return cancelCmd; }
            set { SetProperty(ref cancelCmd, value); }
        }

        private OrderRouteRuleLevelType ruleLevelType;
        public OrderRouteRuleLevelType RuleLevelType
        {
            get { return ruleLevelType; }
            set { SetProperty(ref ruleLevelType, value); }
        }
        
        private NativeExchange ruleExchange;
        public NativeExchange RuleExchange
        {
            get { return ruleExchange; }
            set { SetProperty(ref ruleExchange, value); }
        }

        private int? ruleCommodityType;
        public int? RuleCommodityType
        {
            get { return ruleCommodityType; }
            set { SetProperty(ref ruleCommodityType, value); }
        }
        
        public ObservableCollection<OrderRouteRuleCommoditySelectModel> RuleCommodities { get; private set; }

        private bool isForbiddenRule;
        public bool IsForbiddenRule
        {
            get { return isForbiddenRule; }
            set { SetProperty(ref isForbiddenRule, value); }
        }
        
        public ObservableCollection<HostingTradeAccount> RouteToAccountItems { get; private set; }

        private long? selectedRouteToAccountId;
        public long? SelectedRouteToAccountId
        {
            get { return selectedRouteToAccountId; }
            set { SetProperty(ref selectedRouteToAccountId, value); }
        }

    }
}
