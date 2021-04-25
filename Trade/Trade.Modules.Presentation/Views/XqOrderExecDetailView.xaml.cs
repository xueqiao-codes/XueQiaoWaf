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
    /// OrderExecDetailView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IXqOrderExecDetailView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class XqOrderExecDetailView : IXqOrderExecDetailView
    {
        #region 执行订单列
        private const string ExecOrderColumn_ComposeLeg = "ExecOrderColumn_ComposeLeg";
        private const string ExecOrderColumn_State = "ExecOrderColumn_State";
        private const string ExecOrderColumn_Direction = "ExecOrderColumn_Direction";
        private const string ExecOrderColumn_Name = "ExecOrderColumn_Name";
        private const string ExecOrderColumn_Price = "ExecOrderColumn_Price";
        private const string ExecOrderColumn_Quantity = "ExecOrderColumn_Quantity";
        private const string ExecOrderColumn_TradeVolume = "ExecOrderColumn_TradeVolume";
        private const string ExecOrderColumn_TradeAvgPrice = "ExecOrderColumn_TradeAvgPrice";
        private const string ExecOrderColumn_CreateTime = "ExecOrderColumn_CreateTime";
        private const string ExecOrderColumn_StateMsg = "ExecOrderColumn_StateMsg";
        private const string ExecOrderColumn_ExecOrderId = "ExecOrderColumn_ExecOrderId";
        #endregion

        
#region 组合标的执行订单的列配置
        private static readonly string[] composeTargetExecOrderColumns = new string[]
        {
            ExecOrderColumn_ComposeLeg, ExecOrderColumn_State,
            ExecOrderColumn_Direction, ExecOrderColumn_Name,
            ExecOrderColumn_Price,ExecOrderColumn_Quantity,
            ExecOrderColumn_TradeVolume,ExecOrderColumn_TradeAvgPrice,
            ExecOrderColumn_CreateTime,ExecOrderColumn_StateMsg,
            ExecOrderColumn_ExecOrderId
        };
        private static readonly string starWidthColumn_of_composeTargetExecOrder = ExecOrderColumn_StateMsg;
        #endregion

#region 合约标的执行订单的列配置
        private static readonly string[] contractTargetExecOrderColumns = new string[]
        {
            ExecOrderColumn_State, ExecOrderColumn_Direction, 
            ExecOrderColumn_Price,ExecOrderColumn_Quantity,
            ExecOrderColumn_TradeVolume,ExecOrderColumn_TradeAvgPrice,
            ExecOrderColumn_CreateTime,ExecOrderColumn_StateMsg,
            ExecOrderColumn_ExecOrderId
        };
        private static readonly string starWidthColumn_of_contractTargetExecOrder = ExecOrderColumn_StateMsg;
        #endregion

        public XqOrderExecDetailView()
        {
            InitializeComponent();
        }

        public void InvalidateViewWithOrderTargetType(ClientXQOrderTargetType orderTargetType)
        {
            InvalidateExecOrderItemsDataGridColumns(orderTargetType);
        }

        private void InvalidateExecOrderItemsDataGridColumns(ClientXQOrderTargetType orderTargetType)
        {
            IEnumerable<string> columnResKeys = null;
            string starWidthColumnResKey = null;
            if (orderTargetType == ClientXQOrderTargetType.COMPOSE_TARGET)
            {
                columnResKeys = composeTargetExecOrderColumns.ToArray();
                starWidthColumnResKey = starWidthColumn_of_composeTargetExecOrder;
            }
            else if (orderTargetType == ClientXQOrderTargetType.CONTRACT_TARGET)
            {
                columnResKeys = contractTargetExecOrderColumns.ToArray();
                starWidthColumnResKey = starWidthColumn_of_contractTargetExecOrder;
            }

            ConfigDataGridColumns(this.ExecOrderItemsDataGrid, columnResKeys, starWidthColumnResKey);
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
