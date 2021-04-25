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
    /// OrderXqTradeDetailView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IXqOrderXqTradeDetailView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class XqOrderXqTradeDetailView : IXqOrderXqTradeDetailView
    {
        private const string XqTradeDetailColumn_TargetContractName = "XqTradeDetailColumn_TargetContractName";
        private const string XqTradeDetailColumn_ItemType = "XqTradeDetailColumn_ItemType";
        private const string XqTradeDetailColumn_Direction = "XqTradeDetailColumn_Direction";
        private const string XqTradeDetailColumn_TradeVolume = "XqTradeDetailColumn_TradeVolume";
        private const string XqTradeDetailColumn_TradePrice = "XqTradeDetailColumn_TradePrice";
        private const string XqTradeDetailColumn_TradeTime = "XqTradeDetailColumn_TradeTime";
        private const string XqTradeDetailColumn_TargetComposeLegTradeSummarys = "XqTradeDetailColumn_TargetComposeLegTradeSummarys";
        private const string XqTradeDetailColumn_Detail = "XqTradeDetailColumn_Detail";
        private const string XqTradeDetailColumn_OrderId = "XqTradeDetailColumn_OrderId";

        private static readonly string[] composeTargetXqTradeDetailColumns = new string[] 
        {
            XqTradeDetailColumn_ItemType, XqTradeDetailColumn_Direction,
            XqTradeDetailColumn_TradeVolume, XqTradeDetailColumn_TradePrice,
            XqTradeDetailColumn_TradeTime, XqTradeDetailColumn_TargetComposeLegTradeSummarys,
            XqTradeDetailColumn_Detail
        };
        private static readonly string starWidthColumn_of_composeTargetXqTradeDetail = XqTradeDetailColumn_TargetComposeLegTradeSummarys;

        private static readonly string[] contractTargetXqTradeDetailColumns = new string[]
        {
            XqTradeDetailColumn_TargetContractName, XqTradeDetailColumn_ItemType, XqTradeDetailColumn_Direction,
            XqTradeDetailColumn_TradeVolume, XqTradeDetailColumn_TradePrice,
            XqTradeDetailColumn_TradeTime, XqTradeDetailColumn_OrderId
        };
        private static readonly string starWidthColumn_of_contractTargetXqTradeDetail = XqTradeDetailColumn_OrderId;

        public XqOrderXqTradeDetailView()
        {
            InitializeComponent();
        }

        public void InvalidateViewWithOrderTargetType(ClientXQOrderTargetType orderTargetType)
        {
            InvalidateTradeDetailItemsDataGridColumns(orderTargetType);
        }

        private void InvalidateTradeDetailItemsDataGridColumns(ClientXQOrderTargetType orderTargetType)
        {
            IEnumerable<string> columnResKeys = null;
            string starWidthColumnResKey = null;
            if (orderTargetType == ClientXQOrderTargetType.COMPOSE_TARGET)
            {
                columnResKeys = composeTargetXqTradeDetailColumns.ToArray();
                starWidthColumnResKey = starWidthColumn_of_composeTargetXqTradeDetail;
            }
            else if (orderTargetType == ClientXQOrderTargetType.CONTRACT_TARGET)
            {
                columnResKeys = contractTargetXqTradeDetailColumns.ToArray();
                starWidthColumnResKey = starWidthColumn_of_contractTargetXqTradeDetail;
            }

            var dataGrid = this.XqTradeDetailItemsDataGrid;
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
