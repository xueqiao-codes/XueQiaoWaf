using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Presentation.Views
{
    /// <summary>
    /// PlaceOrderAndChartComponentContentView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IPlaceOrderComponentContentView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class PlaceOrderComponentContentView : IPlaceOrderComponentContentView
    {
        public PlaceOrderComponentContentView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 下单部分的视图宽度
        /// </summary>
        public double PlaceOrderColumnViewWidth => this.PlaceOrderColumn.ActualWidth;

        /// <summary>
        /// 图表部分的视图宽度
        /// </summary>
        public double ChartColumnViewWidth => this.ChartColumn.ActualWidth;
        
    }
}
