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
    /// RelatedOrderView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IRelatedOrderView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class RelatedOrderView : IRelatedOrderView
    {
        /// <summary>
        /// 委托单显示列
        /// </summary>
        private static readonly OrderListColumn_Entrusted[] EntrustedOrderColumns 
            = new OrderListColumn_Entrusted[]
            {
                OrderListColumn_Entrusted.Name,
                OrderListColumn_Entrusted.Direction,
                OrderListColumn_Entrusted.TargetType,
                OrderListColumn_Entrusted.OrderState,
                OrderListColumn_Entrusted.Price,
                OrderListColumn_Entrusted.Quantity,
                OrderListColumn_Entrusted.TradeVolume,
                OrderListColumn_Entrusted.TradeAvgPrice,
                OrderListColumn_Entrusted.OrderType,
                OrderListColumn_Entrusted.CreateTime,
                OrderListColumn_Entrusted.OrderId
            };

        /// <summary>
        /// 预埋单的显示列
        /// </summary>
        private static readonly OrderListColumn_Parked[] ParkedOrderColumns
            = new OrderListColumn_Parked[]
            {
                OrderListColumn_Parked.Name,
                OrderListColumn_Parked.Direction,
                OrderListColumn_Parked.TargetType,
                OrderListColumn_Parked.OrderState,
                OrderListColumn_Parked.OrderType,
                OrderListColumn_Parked.TriggerOrderPrice,
                OrderListColumn_Parked.Quantity,
                OrderListColumn_Parked.CreateTime,
                OrderListColumn_Parked.TriggerTime,
                OrderListColumn_Parked.OrderId
            };

        /// <summary>
        /// 条件单的显示列
        /// </summary>
        private static readonly OrderListColumn_Condition[] ConditionOrderColumns
            = new OrderListColumn_Condition[]
            {
                OrderListColumn_Condition.Name,
                OrderListColumn_Condition.TargetType,
                OrderListColumn_Condition.OrderState,
                OrderListColumn_Condition.ConditionOrderLabel,
                OrderListColumn_Condition.TriggerConditionInfo,
                OrderListColumn_Condition.CreateTime,
                OrderListColumn_Condition.EffectDateType,
                OrderListColumn_Condition.EffectEndTime,
                OrderListColumn_Condition.OrderId
            };

        public RelatedOrderView()
        {
            InitializeComponent();
        }

        public void UpdateCurrentOrderListColumnsWithShowType(RelatedOrderShowType orderShowType)
        {
            var dataGrid = this.CurrentOrderListDataGrid;
            switch (orderShowType)
            {
                case RelatedOrderShowType.Entrusted:
                    UpdateEntrustedOrderDataGridColumns(dataGrid, EntrustedOrderColumns);
                    break;
                case RelatedOrderShowType.Parked:
                    UpdateParkedOrderDataGridColumns(dataGrid, ParkedOrderColumns);
                    break;
                case RelatedOrderShowType.Condition:
                    UpdateConditionOrderDataGridColumns(dataGrid, ConditionOrderColumns);
                    break;
            }
        }

        public void UpdateParentOrderListColumnsWithShowType(RelatedOrderShowType orderShowType)
        {
            var dataGrid = this.ParentOrderListDataGrid;
            switch (orderShowType)
            {
                case RelatedOrderShowType.Entrusted:
                    UpdateEntrustedOrderDataGridColumns(dataGrid, EntrustedOrderColumns);
                    break;
                case RelatedOrderShowType.Parked:
                    UpdateParkedOrderDataGridColumns(dataGrid, ParkedOrderColumns);
                    break;
                case RelatedOrderShowType.Condition:
                    UpdateConditionOrderDataGridColumns(dataGrid, ConditionOrderColumns);
                    break;
            }
        }

        public void UpdateChildOrderListColumnsWithShowType(RelatedOrderShowType orderShowType)
        {
            var dataGrid = this.ChildOrderListDataGrid;
            switch (orderShowType)
            {
                case RelatedOrderShowType.Entrusted:
                    UpdateEntrustedOrderDataGridColumns(dataGrid, EntrustedOrderColumns);
                    break;
                case RelatedOrderShowType.Parked:
                    UpdateParkedOrderDataGridColumns(dataGrid, ParkedOrderColumns);
                    break;
                case RelatedOrderShowType.Condition:
                    UpdateConditionOrderDataGridColumns(dataGrid, ConditionOrderColumns);
                    break;
            }
        }

        private void UpdateEntrustedOrderDataGridColumns(DataGrid orderDataGrid, OrderListColumn_Entrusted[] columns)
        {
            UpdateOrderDataGridColumnsWithColumnType(orderDataGrid, columns);
        }
        
        private void UpdateParkedOrderDataGridColumns(DataGrid orderDataGrid, OrderListColumn_Parked[] columns)
        {
            UpdateOrderDataGridColumnsWithColumnType(orderDataGrid, columns);
        }

        private void UpdateConditionOrderDataGridColumns(DataGrid orderDataGrid, OrderListColumn_Condition[] columns)
        {
            UpdateOrderDataGridColumnsWithColumnType(orderDataGrid, columns);
        }

        private void UpdateOrderDataGridColumnsWithColumnType<ColumnType>(DataGrid orderDataGrid, ColumnType[] columns)
        {
            orderDataGrid.Columns.Clear();
            if (columns != null)
            {
                foreach (var columnType in columns)
                {
                    var dataGridColum = ResourceDictionaryHelper.FindResource($"{columnType.GetType().Name}_{columnType.ToString()}", this.Resources)
                        as DataGridColumn;
                    if (dataGridColum != null)
                    {
                        orderDataGrid.Columns.Add(dataGridColum);
                    }
                }
            }
        }
    }
}
