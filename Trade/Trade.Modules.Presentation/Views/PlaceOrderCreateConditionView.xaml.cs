using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// PlaceOrderCreateConditionView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IPlaceOrderCreateConditionView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class PlaceOrderCreateConditionView : IPlaceOrderCreateConditionView
    {
        public PlaceOrderCreateConditionView()
        {
            InitializeComponent();
        }

        private void ToComposeOrderEPTManageButn_Click(object sender, RoutedEventArgs e)
        {
            ComposeOrderEPTsComboBox.IsDropDownOpen = false;
        }
        
        private object originSelectEPTItem;

        private void ComposeOrderEPTsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.ToEPTManageComboBoxItem == ComposeOrderEPTsComboBox.SelectedItem)
            {
                ComposeOrderEPTsComboBox.SelectedItem = this.originSelectEPTItem;
            }
            else if (this.originSelectEPTItem != ComposeOrderEPTsComboBox.SelectedItem)
            {
                this.originSelectEPTItem = ComposeOrderEPTsComboBox.SelectedItem;
            }
            e.Handled = true;
        }
    }
}
