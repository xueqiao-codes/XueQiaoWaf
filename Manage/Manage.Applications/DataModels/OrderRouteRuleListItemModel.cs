using NativeModel.Contract;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;

namespace Manage.Applications.DataModels
{
    /// <summary>
    /// 显示在列表的订单路由 item model
    /// </summary>
    public class OrderRouteRuleListItemModel : Model
    {
        public OrderRouteRuleListItemModel()
        {
            GroupedCommodities = new ObservableCollection<NativeCommodity>();
        }

        private OrderRouteRuleLevelType routeLevelType;
        public OrderRouteRuleLevelType RouteLevelType
        {
            get { return routeLevelType; }
            set { SetProperty(ref routeLevelType, value); }
        }

        private bool isForbiddenTradeRule;
        public bool IsForbiddenTradeRule
        {
            get { return isForbiddenTradeRule; }
            set { SetProperty(ref isForbiddenTradeRule, value); }
        }

        private long routeToTradeAccountId;
        public long RouteToTradeAccountId
        {
            get { return routeToTradeAccountId; }
            set { SetProperty(ref routeToTradeAccountId, value); }
        }

        private string routeToTradeAccountName;
        public string RouteToTradeAccountName
        {
            get { return routeToTradeAccountName; }
            set { SetProperty(ref routeToTradeAccountName, value); }
        }

        private string routeExchangeMic;
        public string RouteExchangeMic
        {
            get { return routeExchangeMic; }
            set { SetProperty(ref routeExchangeMic, value); }
        }

        private string routeExchangeName;
        public string RouteExchangeName
        {
            get { return routeExchangeName; }
            set { SetProperty(ref routeExchangeName, value); }
        }

        private int routeCommodityType;
        public int RouteCommodityType
        {
            get { return routeCommodityType; }
            set { SetProperty(ref routeCommodityType, value); }
        }

        public ObservableCollection<NativeCommodity> GroupedCommodities { get; private set; }
    }
}
