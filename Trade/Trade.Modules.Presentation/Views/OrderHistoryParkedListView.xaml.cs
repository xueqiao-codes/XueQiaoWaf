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
using System.Windows.Shapes;
using XueQiaoFoundation.BusinessResources.Converters;
using XueQiaoFoundation.BusinessResources.Models;
using XueQiaoFoundation.BusinessResources.Resources;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Helper;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Presentation.Views
{
    /// <summary>
    /// OrderConditionListView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IOrderHistoryParkedListView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class OrderHistoryParkedListView : IOrderHistoryParkedListView
    {
        public OrderHistoryParkedListView()
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
                if (!Enum.IsDefined(typeof(OrderListColumn_Parked), addColumnInfo.ColumnCode)) continue;
                var columnType = (OrderListColumn_Parked)addColumnInfo.ColumnCode;
                var dataGridColum = ResourceDictionaryHelper.FindResource($"{typeof(OrderListColumn_Parked).Name}_{columnType.ToString()}", this.Resources)
                        as DataGridColumn;

                if (dataGridColum != null)
                {
                    ListColumnInfoProvider.SetColumnInfo(dataGridColum, addColumnInfo);

                    // 设置对齐方式
                    Style cellStyle = null, headerStyle = null;
                    GetListDataGridCellAndHeaderStyleWithColumnInfo(addColumnInfo, out cellStyle, out headerStyle);
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

                    // TODO: 列宽度 binding
                    //switch (columnType)
                    //{
                    //    case TradeListColumn.DisplayName:
                    //        this.SetBinding(ColumnDisplayNameWidthProperty, new Binding("ActualWidth") { Source = dataGridColum });
                    //        break;

                    //    default:
                    //        // TODO: other
                    //        break;
                    //}
                }
            }
        }

        private static readonly ListColumnContentAlignment2HorizontalAlignmentConverter contentAlignment2HorizontalAlignmentConverter = new ListColumnContentAlignment2HorizontalAlignmentConverter();

        private static void GetListDataGridCellAndHeaderStyleWithColumnInfo(ListColumnInfo columnInfo,
            out Style columnCellStyle, out Style columnHeaderStyle)
        {
            columnCellStyle = null;
            columnHeaderStyle = null;

            var alignment = contentAlignment2HorizontalAlignmentConverter.Convert(columnInfo.ContentAlignment,
                typeof(HorizontalAlignment), null, CultureInfo.CurrentCulture);
            if (alignment == null) return;

            // setup columnCellStyle
            var baseColumnCellStyle = ResourceDictionaryHelper.FindResource($"DataGridCellDefault", Application.Current.Resources)
                    as Style;
            if (baseColumnCellStyle != null)
            {
                columnCellStyle = new Style(typeof(DataGridCell), baseColumnCellStyle);
                columnCellStyle.Setters.Add(new Setter(Control.HorizontalContentAlignmentProperty, alignment));
            }

            // setup columnHeaderStyle
            Style baseColumnHeaderStyle = null;
            if (ListColumnLivePropertiesHelper.ParkedOrderListFilterableColumns
                .Any(i => i.GetHashCode() == columnInfo.ColumnCode))
            {
                var enumColumn = (OrderListColumn_Parked)columnInfo.ColumnCode;
                baseColumnHeaderStyle = ResourceDictionaryHelper.FindResource($"ParkedOrderListColumnHeaderStyle_{enumColumn.ToString()}",
                    Application.Current.Resources) as Style;
            }
            else
            {
                baseColumnHeaderStyle = ResourceDictionaryHelper.FindResource($"DataGridColumnHeaderDefault",
                    Application.Current.Resources) as Style;
            }
            if (baseColumnHeaderStyle != null)
            {
                columnHeaderStyle = new Style(typeof(DataGridColumnHeader), baseColumnHeaderStyle);
                columnHeaderStyle.Setters.Add(new Setter(Control.HorizontalContentAlignmentProperty, alignment));
            }
        }

        private static DataGridColumn GetTargetDataGridColumn(DataGrid dataGrid, OrderListColumn_Parked targetColumnType)
        {
            return dataGrid.Columns
                    .FirstOrDefault(i => targetColumnType.GetHashCode() == ListColumnInfoProvider.GetColumnInfo(i)?.ColumnCode);
        }

        private static void UpdateColumnInfoWidth(ContractListView view, OrderListColumn_Parked columnType, double newWidth)
        {
            if (view == null) return;
            var targetColumn = GetTargetDataGridColumn(view.ContractListDataGrid, columnType);
            if (targetColumn == null) return;

            var columnInfo = ListColumnInfoProvider.GetColumnInfo(targetColumn);
            if (columnInfo != null)
            {
                columnInfo.Width = newWidth;
            }
        }

        private void SelectAllOrderItems(object sender, RoutedEventArgs e)
        {
            OrdersDataGrid?.SelectAll();
        }

        private void OrdersDataGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OrdersDataGrid?.UnselectAll();
        }
    }
}
