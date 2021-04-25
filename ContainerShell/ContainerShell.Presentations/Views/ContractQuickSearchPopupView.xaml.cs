using ContainerShell.Applications.Views;
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

namespace ContainerShell.Presentations.Views
{
    /// <summary>
    /// ContractQuickSearchPopupView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IContractQuickSearchPopupView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class ContractQuickSearchPopupView : IContractQuickSearchPopupView
    {
        public ContractQuickSearchPopupView()
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

        public void ScrollToContractSearchResultItemWithData(object listItemData)
        {
            if (listItemData == null) return;
            var listItem = ContractSearchResultListView.ItemContainerGenerator.ContainerFromItem(listItemData) as FrameworkElement;
            if (listItem == null) return;
            listItem.BringIntoView();
        }

        public void FocusSearchTextBox()
        {
            this.SearchTextBox?.Focus();
        }

        public object SearchBoxContainerElement => this.SearchBoxContainer;
    }
}
