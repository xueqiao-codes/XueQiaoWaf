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
    public class OrderRouteRuleAddDialogContentViewModel : ViewModel<IOrderRouteRuleAddDialogContentView>
    {
        [ImportingConstructor]
        public OrderRouteRuleAddDialogContentViewModel(IOrderRouteRuleAddDialogContentView view) : base(view)
        {
            RouteToAccountItems = new ObservableCollection<HostingTradeAccount>();
            RuleCreateTypeChangeCmd = new DelegateCommand((object obj) => 
            {
                if (obj is OrderRouteRuleCreateType _createType)
                {
                    RuleCreateType = _createType;
                }
            });
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
        
        public DelegateCommand RuleCreateTypeChangeCmd { get; private set; }

        private OrderRouteRuleCreateType ruleCreateType;
        public OrderRouteRuleCreateType RuleCreateType
        {
            get { return ruleCreateType; }
            set { SetProperty(ref ruleCreateType, value); }
        }


        private OrderRouteRuleSelectTree byCommodityCreateRuleSelectTree;
        public OrderRouteRuleSelectTree ByCommodityCreateRuleSelectTree
        {
            get { return byCommodityCreateRuleSelectTree; }
            set { SetProperty(ref byCommodityCreateRuleSelectTree, value); }
        }

        private RuleExchangeSelectNode byCommoditySelectedRuleExchangeNode;
        public RuleExchangeSelectNode ByCommoditySelectedRuleExchangeNode
        {
            get { return byCommoditySelectedRuleExchangeNode; }
            set { SetProperty(ref byCommoditySelectedRuleExchangeNode, value); }
        }

        private RuleCommodityTypeSelectNode byCommoditySelectedRuleCommodityTypeNode;
        public RuleCommodityTypeSelectNode ByCommoditySelectedRuleCommodityTypeNode
        {
            get { return byCommoditySelectedRuleCommodityTypeNode; }
            set { SetProperty(ref byCommoditySelectedRuleCommodityTypeNode, value); }
        }
        

        private OrderRouteRuleSelectTree byCommodityTypeCreateRuleSelectTree;
        public OrderRouteRuleSelectTree ByCommodityTypeCreateRuleSelectTree
        {
            get { return byCommodityTypeCreateRuleSelectTree; }
            set { SetProperty(ref byCommodityTypeCreateRuleSelectTree, value); }
        }
        
        private RuleExchangeSelectNode byCommodityTypeSelectedRuleExchangeNode;
        public RuleExchangeSelectNode ByCommodityTypeSelectedRuleExchangeNode
        {
            get { return byCommodityTypeSelectedRuleExchangeNode; }
            set { SetProperty(ref byCommodityTypeSelectedRuleExchangeNode, value); }
        }

        private RuleCommodityTypeSelectNode byCommodityTypeSelectedRuleCommodityTypeNode;
        public RuleCommodityTypeSelectNode ByCommodityTypeSelectedRuleCommodityTypeNode
        {
            get { return byCommodityTypeSelectedRuleCommodityTypeNode; }
            set { SetProperty(ref byCommodityTypeSelectedRuleCommodityTypeNode, value); }
        }
        
        private HostingTradeAccount selectedRouteToAccount;
        public HostingTradeAccount SelectedRouteToAccount
        {
            get { return selectedRouteToAccount; }
            set { SetProperty(ref selectedRouteToAccount, value); }
        }

        public ObservableCollection<HostingTradeAccount> RouteToAccountItems { get; private set; }
    }

    public enum OrderRouteRuleCreateType
    {
        ByCommodityType = 1,
        ByCommodity = 2
    }
}
