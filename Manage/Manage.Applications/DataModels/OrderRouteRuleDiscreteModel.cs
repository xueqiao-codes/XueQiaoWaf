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
    /// 订单路由离散型 model
    /// </summary>
    public class OrderRouteRuleDiscreteModel : Model
    {
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

        private string routeExchangeMic;
        public string RouteExchangeMic
        {
            get { return routeExchangeMic; }
            set { SetProperty(ref routeExchangeMic, value); }
        }

        private int routeCommodityType;
        public int RouteCommodityType
        {
            get { return routeCommodityType; }
            set { SetProperty(ref routeCommodityType, value); }
        }

        private string routeCommoditycode;
        public string RouteCommoditycode
        {
            get { return routeCommoditycode; }
            set { SetProperty(ref routeCommoditycode, value); }
        }
    }

    public enum OrderRouteRuleLevelType
    {
        Exchange = 1,
        CommodityType = 2,
        Commodity = 3
    }
}
