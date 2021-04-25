using Manage.Applications.DataModels;
using Manage.Applications.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using XueQiaoFoundation.Shared.Helper;

namespace Manage.Presentations.Views
{
    /// <summary>
    /// FundAccountEquityDailyHistoryView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IFundAccountEquityDailyHistoryView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class FundAccountEquityDailyHistoryView : IFundAccountEquityDailyHistoryView
    {
        public FundAccountEquityDailyHistoryView()
        {
            InitializeComponent();
            Loaded += ViewFirstLoadedHandler;
        }

        public object DisplayInWindow => Window.GetWindow(this);

        private void ViewFirstLoadedHandler(object sender, RoutedEventArgs e)
        {
            Loaded -= ViewFirstLoadedHandler;
            // 设置列表显示列
            SetupTotalEquityItemsDataGridColumns();
            SetupCurrencyGroupedEquityItemsDataGridColumns();
        }

        private void SetupTotalEquityItemsDataGridColumns()
        {
            var dataGrid = this.TotalEquityItemsDataGrid;
            if (dataGrid == null) return;

            dataGrid.Columns.Clear();
            foreach (var _columnName in FundAccountEquityModel.TotalEquityListDisplayColumns.ToArray())
            {
                var column = ResourceDictionaryHelper.FindResource(_columnName, this.Resources) as DataGridColumn;
                if (column != null)
                {
                    dataGrid.Columns.Add(column);
                }
            }
            var lastColumn = dataGrid.Columns.LastOrDefault();
            if (lastColumn != null)
            {
                lastColumn.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }

        private void SetupCurrencyGroupedEquityItemsDataGridColumns()
        {
            var dataGrid = this.CurrencyGroupedEquityItemsDataGrid;
            if (dataGrid == null) return;

            dataGrid.Columns.Clear();
            foreach (var _columnName in FundAccountEquityModel.CurrencyGroupedEquityListDisplayColumns.ToArray())
            {
                var column = ResourceDictionaryHelper.FindResource(_columnName, this.Resources) as DataGridColumn;
                if (column != null)
                {
                    dataGrid.Columns.Add(column);
                }
            }
            var lastColumn = dataGrid.Columns.LastOrDefault();
            if (lastColumn != null)
            {
                lastColumn.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }
    }
}
