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

namespace XueQiaoFoundation.UI.Components.Navigation
{
    /// <summary>
    /// SimpleNavigationContainerView.xaml 的交互逻辑
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class SimpleNavigationContainerView
    {
        public SimpleNavigationContainerView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Navigation to a page
        /// </summary>
        /// <param name="page"></param>
        public void Navigate(Page page)
        {
            if (page != null)
            {
                this.ContentFrame.Navigate(page);
            }
        }

        /// <summary>
        /// Go back to history page if possible
        /// </summary>
        public void GoBackIfPossible()
        {
            if (this.ContentFrame.CanGoBack)
            {
                this.ContentFrame.GoBack();
            }
        }
    }
}
