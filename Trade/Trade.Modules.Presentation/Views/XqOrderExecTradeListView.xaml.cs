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
using NativeModel.Trade;
using XueQiaoWaf.Trade.Modules.Applications.Views;
using XueQiaoFoundation.Shared.Helper;

namespace XueQiaoWaf.Trade.Modules.Presentation.Views
{
    /// <summary>
    /// XqOrderExecTradeListView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IXqOrderExecTradeListView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class XqOrderExecTradeListView : IXqOrderExecTradeListView
    {
        #region 执行成交列
        private const string ExecTradeColumn_ComposeLeg = "ExecTradeColumn_ComposeLeg";
        private const string ExecTradeColumn_Direction = "ExecTradeColumn_Direction";
        private const string ExecTradeColumn_Name = "ExecTradeColumn_Name";
        private const string ExecTradeColumn_TradeVolume = "ExecTradeColumn_TradeVolume";
        private const string ExecTradeColumn_TradePrice = "ExecTradeColumn_TradePrice";
        private const string ExecTradeColumn_CreateTime = "ExecTradeColumn_CreateTime";
        private const string ExecTradeColumn_ExecTradeId = "ExecTradeColumn_ExecTradeId";
        private const string ExecTradeColumn_ExecOrderId = "ExecTradeColumn_ExecOrderId";
        #endregion

        #region 组合标的执行成交的列配置
        private static readonly string[] composeTargetExecTradeColumns = new string[]
        {
            ExecTradeColumn_ComposeLeg, ExecTradeColumn_Direction,
            ExecTradeColumn_Name, ExecTradeColumn_TradeVolume,
            ExecTradeColumn_TradePrice, ExecTradeColumn_CreateTime,
            ExecTradeColumn_ExecTradeId, ExecTradeColumn_ExecOrderId
        };
        private static readonly string starWidthColumn_of_composeTargetExecTrade = ExecTradeColumn_ExecOrderId;
        #endregion

        #region 合约标的执行成交的列配置
        private static readonly string[] contractTargetExecTradeColumns = new string[]
        {
            ExecTradeColumn_Direction, ExecTradeColumn_TradeVolume,
            ExecTradeColumn_TradePrice, ExecTradeColumn_CreateTime,
            ExecTradeColumn_ExecTradeId, ExecTradeColumn_ExecOrderId
        };
        private static readonly string starWidthColumn_of_contractTargetExecTrade = ExecTradeColumn_ExecOrderId;
        #endregion

        public XqOrderExecTradeListView()
        {
            InitializeComponent();
        }

        public void UpdateListColumnsByPresentTarget(ClientXQOrderTargetType orderTargetType)
        {
            UpdateExecTradeItemsDataGridColumns(orderTargetType);
        }

        private void UpdateExecTradeItemsDataGridColumns(ClientXQOrderTargetType orderTargetType)
        {
            IEnumerable<string> columnResKeys = null;
            string starWidthColumnResKey = null;
            if (orderTargetType == ClientXQOrderTargetType.COMPOSE_TARGET)
            {
                columnResKeys = composeTargetExecTradeColumns.ToArray();
                starWidthColumnResKey = starWidthColumn_of_composeTargetExecTrade;
            }
            else if (orderTargetType == ClientXQOrderTargetType.CONTRACT_TARGET)
            {
                columnResKeys = contractTargetExecTradeColumns.ToArray();
                starWidthColumnResKey = starWidthColumn_of_contractTargetExecTrade;
            }

            ConfigDataGridColumns(this.ExecTradeItemsDataGrid, columnResKeys, starWidthColumnResKey);
        }

        private void ConfigDataGridColumns(DataGrid dataGrid, IEnumerable<string> columnResKeys, string starWidthColumnResKey)
        {
            if (dataGrid == null) return;

            dataGrid.Columns.Clear();
            if (columnResKeys?.Any() == true)
            {
                foreach (var columnResKey in columnResKeys)
                {
                    var dataGridColumn = ResourceDictionaryHelper.FindResource(columnResKey, this.Resources) as DataGridColumn;
                    if (dataGridColumn != null)
                    {
                        dataGrid.Columns.Add(dataGridColumn);
                        if (columnResKey == starWidthColumnResKey)
                        {
                            dataGridColumn.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
                        }
                        else
                        {
                            dataGridColumn.Width = new DataGridLength(0, DataGridLengthUnitType.Auto);
                        }
                    }
                }
            }
        }
    }
}
