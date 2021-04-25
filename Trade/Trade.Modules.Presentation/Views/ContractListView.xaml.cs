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
using System.Windows.Interactivity;
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
    /// ContractListView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IContractListView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class ContractListView : IContractListView
    {
        public ContractListView()
        {
            InitializeComponent();
        }

        public UIElement SubscribeItemElement(object subscribeItemData)
        {
            var listItem = this.ContractListDataGrid.ItemContainerGenerator.ContainerFromItem(subscribeItemData) as UIElement;
            return listItem;
        }

        public IEnumerable<ListColumnInfo> ListDisplayColumnInfos
        {
            get
            {
                foreach (var i in ContractListDataGrid.Columns.ToArray())
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
            ContractListDataGrid.Columns.Clear();

            if (listColumnInfos?.Any() != true) return;
            foreach (var addColumnInfo in listColumnInfos)
            {
                if (!Enum.IsDefined(typeof(SubscribeContractListDisplayColumn), addColumnInfo.ColumnCode)) continue;
                var columnType = (SubscribeContractListDisplayColumn)addColumnInfo.ColumnCode;
                var dataGridColum = ResourceDictionaryHelper.FindResource($"{typeof(SubscribeContractListDisplayColumn).Name}_{columnType.ToString()}", this.Resources)
                        as DataGridColumn;
                
                // 更新
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

                    ContractListDataGrid.Columns.Add(dataGridColum);
                    
                    switch (columnType)
                    {
                        case SubscribeContractListDisplayColumn.Name:
                            this.SetBinding(ColumnNameWidthProperty, new Binding("ActualWidth") { Source = dataGridColum });
                            break;

                        case SubscribeContractListDisplayColumn.BidQty:
                            this.SetBinding(ColumnBidQtyWidthProperty, new Binding("ActualWidth") { Source = dataGridColum });
                            break;

                        case SubscribeContractListDisplayColumn.BidPrice:
                            this.SetBinding(ColumnBidPriceWidthProperty, new Binding("ActualWidth") { Source = dataGridColum });
                            break;

                        case SubscribeContractListDisplayColumn.AskPrice:
                            this.SetBinding(ColumnAskPriceWidthProperty, new Binding("ActualWidth") { Source = dataGridColum });
                            break;

                        case SubscribeContractListDisplayColumn.AskQty:
                            this.SetBinding(ColumnAskQtyWidthProperty, new Binding("ActualWidth") { Source = dataGridColum });
                            break;

                        case SubscribeContractListDisplayColumn.UpdateTime:
                            this.SetBinding(ColumnUpdateTimeWidthProperty, new Binding("ActualWidth") { Source = dataGridColum });
                            break;
                        default:
                            // TODO: other
                            break;
                    }
                }
            }

            // 设置最后列宽度为*
            //var lastColumn = ContractListDataGrid.Columns.LastOrDefault();
            //if (lastColumn != null)
            //{
            //    lastColumn.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            //}
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
            if (ListColumnLivePropertiesHelper.SubscribeContractFilterableColumns
                .Any(i => i.GetHashCode() == columnInfo.ColumnCode))
            {
                var enumColumn = (SubscribeContractListDisplayColumn)columnInfo.ColumnCode;
                baseColumnHeaderStyle = ResourceDictionaryHelper.FindResource($"ContractListColumnHeaderStyle_{enumColumn.ToString()}",
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

        // ColumnDisplayNameWidth
        private static readonly DependencyProperty ColumnNameWidthProperty =
            DependencyProperty.Register("ColumnNameWidth", typeof(double), 
                typeof(ContractListView),
                new FrameworkPropertyMetadata((double)0, new PropertyChangedCallback(OnColumnNameWidthChanged)));

        private static void OnColumnNameWidthChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            UpdateColumnInfoWidth((dependencyObject as ContractListView), SubscribeContractListDisplayColumn.Name, (double)e.NewValue);
        }

        // ColumnBidQtyWidth
        private static readonly DependencyProperty ColumnBidQtyWidthProperty =
            DependencyProperty.Register("ColumnBidQtyWidth", typeof(double),
                typeof(ContractListView), 
                new FrameworkPropertyMetadata((double)0, new PropertyChangedCallback(OnColumnBidQtyWidthChanged)));

        private static void OnColumnBidQtyWidthChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            UpdateColumnInfoWidth((dependencyObject as ContractListView), SubscribeContractListDisplayColumn.BidQty, (double)e.NewValue);
        }

        // ColumnBidPriceWidth
        private static readonly DependencyProperty ColumnBidPriceWidthProperty =
            DependencyProperty.Register("ColumnBidPriceWidth", typeof(double),
                typeof(ContractListView), 
                new FrameworkPropertyMetadata((double)0, new PropertyChangedCallback(OnColumnBidPriceWidthChanged)));

        private static void OnColumnBidPriceWidthChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            UpdateColumnInfoWidth((dependencyObject as ContractListView), SubscribeContractListDisplayColumn.BidPrice, (double)e.NewValue);
        }

        // ColumnAskPriceWidth
        private static readonly DependencyProperty ColumnAskPriceWidthProperty =
            DependencyProperty.Register("ColumnAskPriceWidth", typeof(double),
                typeof(ContractListView),
                new FrameworkPropertyMetadata((double)0, new PropertyChangedCallback(OnColumnAskPriceWidthChanged)));

        private static void OnColumnAskPriceWidthChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            UpdateColumnInfoWidth((dependencyObject as ContractListView), SubscribeContractListDisplayColumn.AskPrice, (double)e.NewValue);
        }

        // ColumnAskQtyWidth
        private static readonly DependencyProperty ColumnAskQtyWidthProperty =
            DependencyProperty.Register("ColumnAskQtyWidth", typeof(double),
                typeof(ContractListView),
                new FrameworkPropertyMetadata((double)0, new PropertyChangedCallback(OnColumnAskQtyWidthChanged)));

        private static void OnColumnAskQtyWidthChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            UpdateColumnInfoWidth((dependencyObject as ContractListView), SubscribeContractListDisplayColumn.AskQty, (double)e.NewValue);
        }
        
        // ColumnUpdateTimeWidth
        private static readonly DependencyProperty ColumnUpdateTimeWidthProperty =
            DependencyProperty.Register("ColumnUpdateTimeWidth", typeof(double),
                typeof(ContractListView),
                new FrameworkPropertyMetadata((double)0, new PropertyChangedCallback(OnColumnUpdateTimeWidthChanged)));

        private static void OnColumnUpdateTimeWidthChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            UpdateColumnInfoWidth((dependencyObject as ContractListView), SubscribeContractListDisplayColumn.UpdateTime, (double)e.NewValue);
        }

    }
}
