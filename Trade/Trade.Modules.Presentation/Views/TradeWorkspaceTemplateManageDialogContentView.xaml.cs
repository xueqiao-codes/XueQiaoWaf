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
    /// WorkspaceTemplateManageDialogContentView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(ITradeWorkspaceTemplateManageDialogContentView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class TradeWorkspaceTemplateManageDialogContentView : ITradeWorkspaceTemplateManageDialogContentView
    {
        public TradeWorkspaceTemplateManageDialogContentView()
        {
            InitializeComponent();
        }

        public object DisplayInWindow => Window.GetWindow(this);

        public void CloseDisplayInWindow()
        {
            if (DisplayInWindow is Window _win)
            {
                _win.Close();
            }
        }
    }
}
