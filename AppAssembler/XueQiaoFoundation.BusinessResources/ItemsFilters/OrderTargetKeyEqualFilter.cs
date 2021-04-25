using BolapanControl.ItemsFilter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BolapanControl.ItemsFilter;
using BolapanControl.ItemsFilter.Initializer;
using XueQiaoFoundation.BusinessResources.DataModels;

namespace XueQiaoFoundation.BusinessResources.ItemsFilters
{
    /// <summary>
    /// 订单的标的Key相等过滤器
    /// </summary>
    public class OrderTargetKeyEqualFilter : EqualFilter<OrderItemDataModel>
    {
        public OrderTargetKeyEqualFilter() : base(value => ((OrderItemDataModel)value))
        {
        }

        protected override void OnAttachPresenter(FilterPresenter presenter)
        {
            base.OnAttachPresenter(presenter);
            UpdateAvailableValues(presenter, null);
        }

        protected override void OnDetachPresenter(FilterPresenter presenter)
        {
            base.OnDetachPresenter(presenter);
        }

        /// <summary>
        /// 更新 <see cref="AvailableValues"/>
        /// </summary>
        /// <param name="presenter">FilterPresenter</param>
        /// <param name="predicate">The predicator to predicate available values</param>
        public void UpdateAvailableValues(FilterPresenter presenter, Predicate<OrderItemDataModel> predicate)
        {
            IEnumerable<OrderItemDataModel> availableValues = null;
            var targetKeyGroups = presenter.CollectionView.SourceCollection?.OfType<OrderItemDataModel>().ToArray()
                .Where(i=> predicate?.Invoke(i)??true).GroupBy(i => i.TargetKey);
            availableValues = targetKeyGroups.Select(i => i.First()).ToArray();

            base.AvailableValues = availableValues;
        }

        /// <summary>
        /// Determines whether the specified target is a match.
        /// </summary>
        public override void IsMatch(FilterPresenter sender, FilterEventArgs e)
        {
            if (e.Accepted)
            {
                if (e.Item == null)
                    e.Accepted = false;
                else
                {
                    var value = getter(e.Item) as OrderItemDataModel;
                    if (value == null) e.Accepted = false;
                    e.Accepted = SelectedValues.Cast<OrderItemDataModel>().Any(i=>i.TargetKey == value.TargetKey);
                }
            }
        }
    }

    public class OrderTargetKeyEqualFilterInitializer : FilterInitializer
    {
        public override Filter NewFilter(FilterPresenter filterPresenter, object key)
        {
            return new OrderTargetKeyEqualFilter();
        }
    }
}
