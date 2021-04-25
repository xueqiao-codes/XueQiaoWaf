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
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Presentation.Views
{
    /// <summary>
    /// ComposeSearchPopupView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IComposeSearchPopupView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class ComposeSearchPopupView : IComposeSearchPopupView
    {
        public ComposeSearchPopupView()
        {
            InitializeComponent();
        }

        public void Close()
        {
            this.IsOpen = false;
            this.PlacementTarget = null;
        }

        public void ShowPopup(object targetElement)
        {
            this.PlacementTarget = targetElement as UIElement;
            this.IsOpen = true;
            FocusSearchTextBox();
        }

        public void ScrollToComposeItemWithData(object composeItem)
        {
            if (composeItem == null) return;
            var rowItem = ComposeListDataGrid.ItemContainerGenerator.ContainerFromItem(composeItem) as DataGridRow;
            if (rowItem == null) return;
            rowItem.BringIntoView();
        }

        public void FocusSearchTextBox()
        {
            this.SearchTextBox?.Focus();
        }
    }
}
