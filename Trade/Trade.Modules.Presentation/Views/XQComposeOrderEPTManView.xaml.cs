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
    /// XQComposeOrderExecParamsManView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IXQComposeOrderEPTManView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class XQComposeOrderEPTManView : IXQComposeOrderEPTManView
    {
        public XQComposeOrderEPTManView()
        {
            InitializeComponent();
        }

        private void TriggerNewTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            var triggerEle = sender as FrameworkElement;
            if (triggerEle == null) return;

            if (triggerEle.ContextMenu.PlacementTarget == null)
            {
                triggerEle.ContextMenu.PlacementTarget = triggerEle;
            }
            triggerEle.ContextMenu.IsOpen = true;
        }
    }
}
