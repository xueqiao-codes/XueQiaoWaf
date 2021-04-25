using BolapanControl.ItemsFilter;
using BolapanControl.ItemsFilter.Initializer;
using BolapanControl.ItemsFilter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.BusinessResources.ItemsFilters
{
    public class DataItemsCustomFilter : Filter
    {
        public DataItemsCustomFilter()
        {

        }

        public Predicate<object> CustomFilter { get; set; }
        
        public override void IsMatch(FilterPresenter sender, FilterEventArgs e)
        {
            if (e.Accepted)
            {
                var dataItem = e.Item;
                if (dataItem == null)
                {
                    e.Accepted = false;
                }
                else if (CustomFilter != null)
                {
                    e.Accepted = CustomFilter(dataItem);
                }
            }
        }
    }

    public class DataItemsCustomFilterInitializer : FilterInitializer
    {
        public override Filter NewFilter(FilterPresenter filterPresenter, object key)
        {
            return new DataItemsCustomFilter();
        }
    }
}
