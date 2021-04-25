using BolapanControl.ItemsFilter;
using BolapanControl.ItemsFilter.Initializer;
using BolapanControl.ItemsFilter.Model;
using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.BusinessResources.DataModels;

namespace XueQiaoFoundation.BusinessResources.ItemsFilters
{
    /// <summary>
    /// 订单交易方向 filter
    /// </summary>
    public class OrderTradeDirectionFilter : EqualFilter<OrderItemDataModel>
    {
        public OrderTradeDirectionFilter() : base(o => GetOrderDirection(o), typeof(ClientTradeDirection).GetEnumValues())
        {
        }

        private static object GetOrderDirection(object srcItem)
        {
            if (srcItem is OrderItemDataModel_Entrusted _e_o)
                return _e_o.Direction;
            else if (srcItem is OrderItemDataModel_Parked _p_o)
                return _p_o.Direction;
            return srcItem;
        }
    }

    public class OrderTradeDirectionFilterInitializer : FilterInitializer
    {
        public override Filter NewFilter(FilterPresenter filterPresenter, object key)
        {
            return new OrderTradeDirectionFilter();
        }
    }
}
