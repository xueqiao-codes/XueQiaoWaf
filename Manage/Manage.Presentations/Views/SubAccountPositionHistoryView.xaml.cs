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
    /// SubAccountPositionHistoryView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(ISubAccountPositionHistoryView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class SubAccountPositionHistoryView : ISubAccountPositionHistoryView
    {
        public SubAccountPositionHistoryView()
        {
            InitializeComponent();
            Loaded += ViewFirstLoadedHandler;
        }

        private void ViewFirstLoadedHandler(object sender, RoutedEventArgs e)
        {
            Loaded -= ViewFirstLoadedHandler;
            SetupPositionItemsDataGridColumns();
        }

        private void SetupPositionItemsDataGridColumns()
        {
            var dataGrid = this.PositionItemsDataGrid;
            if (dataGrid == null) return;

            dataGrid.Columns.Clear();
            foreach (var _columnName in PositionManageDM.HistoryPositionViewBySubAccountListColumns.ToArray())
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
