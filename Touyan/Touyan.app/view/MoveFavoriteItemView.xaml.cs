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
    /// MoveFavoriteItemView.xaml 的交互逻辑
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class MoveFavoriteItemView : IView
    {
        public MoveFavoriteItemView()
        {
            InitializeComponent();
        }
    }
}
