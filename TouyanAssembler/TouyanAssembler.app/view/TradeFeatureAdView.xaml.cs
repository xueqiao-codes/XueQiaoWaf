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

namespace TouyanAssembler.app.view
{
    /// <summary>
    /// TradeFeatureAdView.xaml 的交互逻辑
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class TradeFeatureAdView : UserControl
    {
        public TradeFeatureAdView()
        {
            InitializeComponent();
            this.IsShowPersonalUserAdTab = true;
        }
        
        public bool IsShowPersonalUserAdTab
        {
            get { return (bool)GetValue(IsShowPersonalUserAdTabProperty); }
            set { SetValue(IsShowPersonalUserAdTabProperty, value); }
        }

        /// <summary>
        /// 是否显示个人用户广告 tab
        /// </summary>
        public static readonly DependencyProperty IsShowPersonalUserAdTabProperty =
            DependencyProperty.Register("IsShowPersonalUserAdTab", typeof(bool), typeof(TradeFeatureAdView), new PropertyMetadata(false));
        
        private void AdTabToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender == this.PersonalUserAdTabToggleButton)
            {
                this.IsShowPersonalUserAdTab = true;
            }
            else if (sender == this.CompanyUserAdTabToggleButton)
            {
                this.IsShowPersonalUserAdTab = false;
            }
        }

        private void OpenAdWebsiteButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.xueqiao.cn");
        }
    }
}
