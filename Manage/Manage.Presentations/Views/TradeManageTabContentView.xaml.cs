using Manage.Applications.Views;
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

namespace Manage.Presentations.Views
{
    /// <summary>
    /// TradeManageTabContentView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(ITradeManageTabContentView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class TradeManageTabContentView : ITradeManageTabContentView
    {
        public TradeManageTabContentView()
        {
            InitializeComponent();
        }

        private IEnumerable<ListBox> GetAllManageEntryListBox()
        {
            return new ListBox[] 
            {
                this.Fund_ManageEntryListBox, this.Position_ManageEntryListBox, this.Settlement_ManageEntryListBox
            };
        }

        private void ManageEntryListBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            var listBoxItem = sender as ListBoxItem;
            if (listBoxItem == null) return;

            var parentListBox = WpfUITreeHelper.FindVisualParent<ListBox>(listBoxItem);
            if (parentListBox == null) return;

            var otherEntryListBoxs = GetAllManageEntryListBox().Except(new ListBox[] { parentListBox });
            if (otherEntryListBoxs?.Any() == true)
            {
                foreach (var otherListBox in otherEntryListBoxs)
                {
                    otherListBox.UnselectAll();
                }
            }
        }
    }
}
