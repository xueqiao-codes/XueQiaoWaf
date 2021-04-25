using NativeModel.Trade;
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
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Presentation.Views
{
    /// <summary>
    /// XqTargetClosePositionHistoryView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IXqTargetClosePositionHistoryView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class XqTargetClosePositionHistoryView : IXqTargetClosePositionHistoryView
    {
        public XqTargetClosePositionHistoryView()
        {
            InitializeComponent();
        }
        
        private void SetupTradeItemsDataGridColumns(DataGrid tradeItemsDataGrid)
        {
            var dataGrid = tradeItemsDataGrid;
            if (dataGrid == null) return;

            dataGrid.Columns.Clear();

            string[] columnNames = null;
            if (currentPresentTargetType == ClientXQOrderTargetType.CONTRACT_TARGET)
                columnNames = XqTargetDetailPositionDM.XqContractTargetDetailPositionColumns.ToArray();
            else if (currentPresentTargetType == ClientXQOrderTargetType.COMPOSE_TARGET)
                columnNames = XqTargetDetailPositionDM.XqComposeTargetDetailPositionColumns.ToArray();

            if (columnNames != null)
            {
                foreach (var _columnName in columnNames)
                {
                    var column = ResourceDictionaryHelper.FindResource(_columnName, this.Resources) as DataGridColumn;
                    if (column != null)
                    {
                        dataGrid.Columns.Add(column);
                    }
                }
            }
        }
        
        private ClientXQOrderTargetType? currentPresentTargetType;

        public void UpdatePositionColumnsByPresentTarget(ClientXQOrderTargetType targetType)
        {
            if (currentPresentTargetType == targetType) return;
            currentPresentTargetType = targetType;

            SetupTradeItemsDataGridColumns(this.BuyDetailPositionItemsDataGrid);
            SetupTradeItemsDataGridColumns(this.SellDetailPositionItemsDataGrid);
        }
    }
}
