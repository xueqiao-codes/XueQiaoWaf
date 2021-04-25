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
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Presentation.Views
{
    /// <summary>
    /// XqTargetPositionTradeDetailView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IXqTargetPositionTradeDetailView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class XqTargetPositionTradeDetailView : IXqTargetPositionTradeDetailView
    {
        #region 执行成交列
        private const string PositionTradeDetailItemColumn_ComposeLeg = "PositionTradeDetailItemColumn_ComposeLeg";
        private const string PositionTradeDetailItemColumn_Name = "PositionTradeDetailItemColumn_Name";
        private const string PositionTradeDetailItemColumn_Direction = "PositionTradeDetailItemColumn_Direction";
        private const string PositionTradeDetailItemColumn_Quantity = "PositionTradeDetailItemColumn_Quantity";
        private const string PositionTradeDetailItemColumn_Price = "PositionTradeDetailItemColumn_Price";
        private const string PositionTradeDetailItemColumn_DataTime = "PositionTradeDetailItemColumn_DataTime";
        #endregion

        #region 组合标的执行成交的列配置
        private static readonly string[] composeTargetExecTradeColumns = new string[]
        {
            PositionTradeDetailItemColumn_ComposeLeg, PositionTradeDetailItemColumn_Name,
            PositionTradeDetailItemColumn_Direction, PositionTradeDetailItemColumn_Quantity,
            PositionTradeDetailItemColumn_Price, PositionTradeDetailItemColumn_DataTime
        };
        #endregion

        #region 合约标的执行成交的列配置
        private static readonly string[] contractTargetExecTradeColumns = new string[]
        {
            PositionTradeDetailItemColumn_Name,
            PositionTradeDetailItemColumn_Direction, PositionTradeDetailItemColumn_Quantity,
            PositionTradeDetailItemColumn_Price, PositionTradeDetailItemColumn_DataTime
        };
        #endregion

        public XqTargetPositionTradeDetailView()
        {
            InitializeComponent();
        }

        public void UpdateListColumnsByPresentTarget(ClientXQOrderTargetType orderTargetType)
        {
            UpdateDetailItemsDataGridColumns(orderTargetType);
        }

        private void UpdateDetailItemsDataGridColumns(ClientXQOrderTargetType orderTargetType)
        {
            IEnumerable<string> columnResKeys = null;
            if (orderTargetType == ClientXQOrderTargetType.COMPOSE_TARGET)
            {
                columnResKeys = composeTargetExecTradeColumns.ToArray();
            }
            else if (orderTargetType == ClientXQOrderTargetType.CONTRACT_TARGET)
            {
                columnResKeys = contractTargetExecTradeColumns.ToArray();
            }

            ConfigDataGridColumns(this.PositionTradeDetailItemsDataGrid, columnResKeys);
        }

        private void ConfigDataGridColumns(DataGrid dataGrid, IEnumerable<string> columnResKeys)
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
                    }
                }
            }
        }
    }
}
