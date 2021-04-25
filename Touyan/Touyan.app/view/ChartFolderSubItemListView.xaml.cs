using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Touyan.app.view
{
    /// <summary>
    /// ChartFolderSubItemListView.xaml 的交互逻辑
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class ChartFolderSubItemListView : IView
    {
        /// <summary>
        /// Node 选中的处理方法 
        /// </summary>
        public Action<object> NodeItemSelectedHandler { get; set; }

        /// <summary>
        /// Node 展开的处理方法 
        /// </summary>
        public Action<object> NodeItemExpandedHandler { get; set; }

        public ChartFolderSubItemListView()
        {
            InitializeComponent();
        }

        private void TreeViewItem_Selected(object sender, RoutedEventArgs e)
        {
            var itemData = (sender as TreeViewItem)?.DataContext;
            if (itemData != null)
            {
                NodeItemSelectedHandler?.Invoke(itemData);
            }
        }

        private void TreeViewItem_Expanded(object sender, RoutedEventArgs e)
        {
            var itemData = (sender as TreeViewItem)?.DataContext;
            if (itemData != null)
            {
                NodeItemExpandedHandler?.Invoke(itemData);
            }
        }
    }
}
