using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using XueQiaoFoundation.BusinessResources.Models;
using XueQiaoFoundation.BusinessResources.Resources;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Helper;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Presentation.Views
{
    /// <summary>
    /// ComposeListView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IComposeListView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class ComposeListView : IComposeListView
    {
        public ComposeListView()
        {
            InitializeComponent();
        }
        
        public UIElement SubscribeItemElement(object subscribeItemData)
        {
            var listItem = this.ComposeListDataGrid.ItemContainerGenerator.ContainerFromItem(subscribeItemData) as UIElement;
            return listItem;
        }

        public IEnumerable<ListColumnInfo> ListDisplayColumnInfos
        {
            get
            {
                foreach (var i in ComposeListDataGrid.Columns.ToArray())
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
            ComposeListDataGrid.Columns.Clear();

            if (listColumnInfos?.Any() != true) return;
            foreach (var addColumnInfo in listColumnInfos)
            {
                if (!Enum.IsDefined(typeof(SubscribeComposeListDisplayColumn), addColumnInfo.ColumnCode)) continue;
                var columnType = (SubscribeComposeListDisplayColumn)addColumnInfo.ColumnCode;
                var dataGridColum = ResourceDictionaryHelper.FindResource($"{typeof(SubscribeComposeListDisplayColumn).Name}_{columnType.ToString()}", this.Resources)
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

                    ComposeListDataGrid.Columns.Add(dataGridColum);

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

            // 设置最后列宽度为*
            var lastColumn = ComposeListDataGrid.Columns.LastOrDefault();
            if (lastColumn != null)
            {
                lastColumn.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
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
            if (ListColumnLivePropertiesHelper.SubscribeComposeFilterableColumns
                .Any(i => i.GetHashCode() == columnInfo.ColumnCode))
            {
                var enumColumn = (SubscribeComposeListDisplayColumn)columnInfo.ColumnCode;
                baseColumnHeaderStyle = ResourceDictionaryHelper.FindResource($"ComposeListColumnHeaderStyle_{enumColumn.ToString()}",
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

        private static DataGridColumn GetTargetDataGridColumn(DataGrid dataGrid, SubscribeContractListDisplayColumn targetColumnType)
        {
            return dataGrid.Columns
                    .FirstOrDefault(i => targetColumnType.GetHashCode() == ListColumnInfoProvider.GetColumnInfo(i)?.ColumnCode);
        }

        private static void UpdateColumnInfoWidth(ContractListView view, SubscribeContractListDisplayColumn columnType, double newWidth)
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

        private void RowDetailTriggerButn_Click(object sender, RoutedEventArgs e)
        {
            DependencyObject obj = (DependencyObject)e.OriginalSource;
            while (!(obj is DataGridRow) && obj != null)
                obj = VisualTreeHelper.GetParent(obj);

            if (obj is DataGridRow dgrow)
            {
                var originDetailsVisibility = dgrow.DetailsVisibility;
                dgrow.DetailsVisibility = (originDetailsVisibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
            }
        }
    }
}
