using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace XueQiaoFoundation.UI.Components.MessageWindow.Views
{
    /// <summary>
    /// MessageLayoutWindow.xaml 的交互逻辑
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class MessageLayoutWindow : IView
    {
        public MessageLayoutWindow()
        {
            InitializeComponent();
        }
        
        public void ConfigWindow(Tuple<Window> owner = null, 
            Tuple<Point> location = null, Tuple<WindowStartupLocation> startupLocation = null,
            Tuple<Size> windowSize = null, Tuple<SizeToContent> sizeToContent = null,
            Tuple<double> windowMaxWidth = null, Tuple<double> windowMaxHeight = null,
            Tuple<double> windowMinWidth = null, Tuple<double> windowMinHeight = null,
            Tuple<bool> topmost = null, Tuple<bool> showInTaskBar = null)
        {
            if (owner != null)
            {
                this.Owner = owner.Item1;
            }

            if (location != null)
            {
                this.Left = location.Item1.X;
                this.Top = location.Item1.Y;
            }

            if (startupLocation != null)
            {
                this.WindowStartupLocation = startupLocation.Item1;
            }

            if (windowSize != null)
            {
                this.Width = windowSize.Item1.Width;
                this.Height = windowSize.Item1.Height;
            }

            if (sizeToContent != null)
            {
                this.SizeToContent = sizeToContent.Item1;
            }

            if (windowMaxWidth != null)
            {
                this.MaxWidth = windowMaxWidth.Item1;
            }

            if (windowMaxHeight != null)
            {
                this.MaxHeight = windowMaxHeight.Item1;
            }

            if (windowMinWidth != null)
            {
                this.MinWidth = windowMinWidth.Item1;
            }

            if (windowMinHeight != null)
            {
                this.MinHeight = windowMinHeight.Item1;
            }

            if (topmost != null)
            {
                this.Topmost = topmost.Item1;
            }

            if (showInTaskBar != null)
            {
                this.ShowInTaskbar = showInTaskBar.Item1;
            }
        }
    }
}
