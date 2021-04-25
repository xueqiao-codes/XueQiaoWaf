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
using System.Windows.Shapes;

namespace ContainerShell.Presentations.Views
{
    /// <summary>
    /// ContainerShellWindow.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IContainerShellView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class ContainerShellWindow : IContainerShellView
    {
        public ContainerShellWindow()
        {
            InitializeComponent();
            Loaded += ContainerShellWindow_FirstLoaded;
        }

        private void ContainerShellWindow_FirstLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= ContainerShellWindow_FirstLoaded;
            
            Width = SystemParameters.WorkArea.Width;
            Height = SystemParameters.WorkArea.Height;
            WindowState = WindowState.Maximized;
        }

        public object ShowMoreFunctionTriggerElement => this.ShowMoreFunctionButnContainer;

    }
}
