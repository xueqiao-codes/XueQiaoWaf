using System;
using System.Collections.Generic;
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
    /// TradeLameOptView.xaml 的交互逻辑
    /// </summary>
    public partial class TradeLameOptView : UserControl
    {
        public TradeLameOptView()
        {
            InitializeComponent();
        }
        
        private void SelectAllItems(object sender, RoutedEventArgs e)
        {
            ItemsDataGrid?.SelectAll();
        }

        private void ItemsDataGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ItemsDataGrid?.UnselectAll();
        }
    }
}
