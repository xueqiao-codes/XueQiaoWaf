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
    /// PlaceOrderHangingOrdersAreaView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IPlaceOrderHangingOrdersAreaView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class PlaceOrderHangingOrdersAreaView : IPlaceOrderHangingOrdersAreaView
    {
        private const string OrderListColumn_ComposeLegTradeSummary = "OrderListColumn_ComposeLegTradeSummary";
        private static readonly string[] CompleteOrderListColumnNames = new string[] 
        {
            "OrderListColumn_Direction",
            "OrderListColumn_OrderState",
            "OrderListColumn_Price",
            "OrderListColumn_Quantity",
            OrderListColumn_ComposeLegTradeSummary
        };
        private ClientXQOrderTargetType? currentPresentTargetType;

        public PlaceOrderHangingOrdersAreaView()
        {
            InitializeComponent();
        }

        public void ResetOrderListColumnsByPresentTarget(ClientXQOrderTargetType targetType)
        {
            if (currentPresentTargetType == targetType) return;
            currentPresentTargetType = targetType;

            OrdersDataGrid.Columns.Clear();

            IEnumerable<string> displayColumns = CompleteOrderListColumnNames;
            if (targetType != ClientXQOrderTargetType.COMPOSE_TARGET)
            {
                displayColumns = displayColumns.Except(new string[] { OrderListColumn_ComposeLegTradeSummary });
            }
            
            foreach (var columnName in displayColumns)
            {
                var dataGridColum = ResourceDictionaryHelper.FindResource(columnName, this.Resources) as DataGridColumn;
                if (dataGridColum != null)
                {
                    OrdersDataGrid.Columns.Add(dataGridColum);
                }
            }

            // 设置最后列宽度为*
            var lastColumn = OrdersDataGrid.Columns.LastOrDefault();
            if (lastColumn != null)
            {
                lastColumn.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }

        /// <summary>
        /// 选中所有订单项
        /// </summary>
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
