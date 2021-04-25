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
using XueQiaoFoundation.UI.Components.MessageWindow.ViewModels;

namespace XueQiaoFoundation.UI.Components.MessageWindow.Views
{
    /// <summary>
    /// SimpleMessageWindowContentView.xaml 的交互逻辑
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class SimpleMessageWindowContentView : IView
    {
        private readonly Lazy<SimpleMessageWindowContentVM> contentVMLazy;

        public SimpleMessageWindowContentView()
        {
            InitializeComponent();
            contentVMLazy = new Lazy<SimpleMessageWindowContentVM>(() => ViewHelper.GetViewModel<SimpleMessageWindowContentVM>(this));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            contentVMLazy.Value?.ButtonClickAction?.Invoke();
        }
    }
}
