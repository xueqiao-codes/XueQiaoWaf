using BolapanControl.ItemsFilter;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using XueQiaoFoundation.BusinessResources.Converters;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.BusinessResources.ItemsFilters;
using XueQiaoFoundation.BusinessResources.Models;
using XueQiaoFoundation.BusinessResources.Resources;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.UI.Styles;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Helper;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Presentation.Views
{
    /// <summary>
    /// OrderListEntrustedPageView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IOrderListEntrustedPageView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class OrderListEntrustedPageView : IOrderListEntrustedPageView
    {
        public OrderListEntrustedPageView()
        {
            InitializeComponent();
        }

        public object DisplayInWindow => Window.GetWindow(this);

        public IEnumerable<ListColumnInfo> ListDisplayColumnInfos
        {
            get
            {
                foreach (var i in OrdersDataGrid.Columns.ToArray())
                {
                    var columnInfo = ListColumnInfoProvider.GetColumnInfo(i);
                    if (columnInfo != null)
                    {
                        yield return columnInfo;
                    }
                }
            }
        }

        public void ResetListDisplayColumns(IEnumerable<ListColumnInfo> listColumnInfos)
        {
            OrdersDataGrid.Columns.Clear();

            if (listColumnInfos?.Any() != true) return;
            foreach (var addColumnInfo in listColumnInfos)
            {
                if (!Enum.IsDefined(typeof(OrderListColumn_Entrusted), addColumnInfo.ColumnCode)) continue;
                var columnType = (OrderListColumn_Entrusted)addColumnInfo.ColumnCode;
                var dataGridColum = ResourceDictionaryHelper.FindResource($"{columnType.GetType().Name}_{columnType.ToString()}", this.Resources)
                        as DataGridColumn;

                if (dataGridColum != null)
                {
                    ListColumnInfoProvider.SetColumnInfo(dataGridColum, addColumnInfo);

                    // 设置对齐方式
                    var contentAlignment = (HorizontalAlignment)contentAlignment2HorizontalAlignmentConverter.Convert(addColumnInfo.ContentAlignment,
                            typeof(HorizontalAlignment), null, CultureInfo.CurrentCulture);
                    var cellStyle = CreateColumnCellStyle(contentAlignment);
                    var headerStyle = CreateColumnHeaderStyle(contentAlignment, columnType, this.Resources);
                    if (cellStyle != null)
                    {
                        dataGridColum.CellStyle = cellStyle;
                    }
                    if (headerStyle != null)
                    {
                        dataGridColum.HeaderStyle = headerStyle;
                    }

                    // 设置宽度
                    if (addColumnInfo.Width > 0)
                    {
                        dataGridColum.Width = addColumnInfo.Width.Value;
                    }

                    OrdersDataGrid.Columns.Add(dataGridColum);
                }
            }
        }


        public Predicate<OrderItemDataModel> PageAvailableOrdersFilter { get; set; }


        private static readonly ListColumnContentAlignment2HorizontalAlignmentConverter contentAlignment2HorizontalAlignmentConverter = new ListColumnContentAlignment2HorizontalAlignmentConverter();

        private static Style CreateColumnCellStyle(HorizontalAlignment contentAlignment)
        {
            // setup columnCellStyle
            var baseColumnCellStyle = ResourceDictionaryHelper.FindResource($"DataGridCellDefault", Application.Current.Resources)
                    as Style;
            if (baseColumnCellStyle != null)
            {
                var columnCellStyle = new Style(typeof(DataGridCell), baseColumnCellStyle);
                columnCellStyle.Setters.Add(new Setter(Control.HorizontalContentAlignmentProperty, contentAlignment));
                return columnCellStyle;
            }
            return baseColumnCellStyle;
        }

        private static Style CreateColumnHeaderStyle(HorizontalAlignment contentAlignment, OrderListColumn_Entrusted columnType, ResourceDictionary baseStyleSearchRoot)
        {
            // setup columnHeaderStyle
            if (baseStyleSearchRoot == null) baseStyleSearchRoot = Application.Current.Resources;

            Style columnHeaderStyle = null;
            if (ListColumnLivePropertiesHelper.EntrustedOrderListFilterableColumns
                .Any(i => i == columnType))
            {
                var baseColumnHeaderStyle = ResourceDictionaryHelper.FindResource($"HeaderStyle_{typeof(OrderListColumn_Entrusted).Name}_{columnType.ToString()}",
                    baseStyleSearchRoot) as Style;

                if (baseColumnHeaderStyle != null)
                {
                    columnHeaderStyle = new Style(typeof(DataGridColumnHeader), baseColumnHeaderStyle);
                    columnHeaderStyle.Setters.Add(new Setter(DataGridColumnHeaderHelper.FilterControlStateChangedCallbakProperty, new FilterControlStateChanged(TargetKeyColumnFilterControlState_Changed)));
                    columnHeaderStyle.Setters.Add(new Setter(Control.HorizontalContentAlignmentProperty, contentAlignment));
                    return columnHeaderStyle;
                }
            }

            if (columnHeaderStyle == null)
            {
                var baseColumnHeaderStyle = ResourceDictionaryHelper.FindResource($"DataGridColumnHeaderDefault",
                        Application.Current.Resources) as Style;
                if (baseColumnHeaderStyle != null)
                {
                    columnHeaderStyle = new Style(typeof(DataGridColumnHeader), baseColumnHeaderStyle);
                    columnHeaderStyle.Setters.Add(new Setter(Control.HorizontalContentAlignmentProperty, contentAlignment));
                    return columnHeaderStyle;
                }
            }

            return null;
        }
        
        private void SelectAllOrderItems(object sender, RoutedEventArgs e)
        {
            OrdersDataGrid?.SelectAll();
        }

        private void OrdersDataGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OrdersDataGrid?.UnselectAll();
        }

        
        private static void TargetKeyColumnFilterControlState_Changed(FilterControlStateChangedArgs e)
        {
            if (e.NewState.HasFlag(FilterControlState.OpenActive) || e.NewState.HasFlag(FilterControlState.Open))
            {
                var filterControl = e.FilterControl;
                var orderListPageView = WpfUITreeHelper.FindVisualParent<OrderListEntrustedPageView>(filterControl);
                if (orderListPageView == null) return;

                var availableOrdersFilter = orderListPageView.PageAvailableOrdersFilter;
                var filterPresenter = FilterPresenter.TryGet(e.FilterControl.ParentCollection);
                if (filterPresenter == null) return;

                var targetKeyEqualFilter = filterPresenter.TryGetFilter("TargetKey", new OrderTargetKeyEqualFilterInitializer())
                    as OrderTargetKeyEqualFilter;
                targetKeyEqualFilter?.UpdateAvailableValues(filterPresenter, availableOrdersFilter);
            }
        }
    }
}
