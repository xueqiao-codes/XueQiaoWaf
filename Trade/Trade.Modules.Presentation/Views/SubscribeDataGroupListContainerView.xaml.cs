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
    /// ComposeListComponentContentView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(ISubscribeDataGroupListContainerView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class SubscribeDataGroupListContainerView : ISubscribeDataGroupListContainerView
    {
        public SubscribeDataGroupListContainerView()
        {
            InitializeComponent();
            Loaded += FirstLoadedHandler;
        }

        private void FirstLoadedHandler(object sender, RoutedEventArgs e)
        {
            Loaded -= FirstLoadedHandler;
            var selectedGroupItem = DataGroupListViewsTabControl.SelectedItem;
            if (selectedGroupItem != null)
            {
                Scroll2GroupItem(selectedGroupItem);
            }
        }

        public object DisplayInWindow => Window.GetWindow(this);

        public UIElement GroupItemElement(object groupItemData)
        {
            var listItem = this.DataGroupListViewsTabControl.ItemContainerGenerator.ContainerFromItem(groupItemData) as UIElement;
            return listItem;
        }

        public void Scroll2GroupItem(object groupItemData)
        {
            var listItem = this.DataGroupListViewsTabControl.ItemContainerGenerator.ContainerFromItem(groupItemData) as FrameworkElement;
            if (listItem == null) return;
            listItem.BringIntoView();
        }

        public UIElement AddGroupButton
        {
            get
            {
                var butn = DataGroupListViewsTabControl.Template.FindName("AddGroupButton", DataGroupListViewsTabControl) as UIElement;
                return butn;
            }
        }
    }
}
