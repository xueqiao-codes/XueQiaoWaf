using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace ContainerShell.Presentations.Views
{
    /// <summary>
    /// ExceptionOrdersOptView.xaml 的交互逻辑
    /// </summary>
    public partial class ExceptionOrdersOptView : UserControl
    {
        public ExceptionOrdersOptView()
        {
            InitializeComponent();
        }
        
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
