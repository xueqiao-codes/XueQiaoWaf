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

namespace XueQiaoFoundation.UI.Components.Popup
{
    /// <summary>
    /// XueQiaoPopup.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IPopup)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class XueQiaoPopup : IPopup
    {
        public XueQiaoPopup()
        {
            InitializeComponent();
        }

        public void Open()
        {
            this.IsOpen = true;
        }

        public void Close()
        {
            this.IsOpen = false;
        }

        private UIElement _content;
        public UIElement Content
        {
            get { return _content; }
            set
            {
                if (_content == value) return;
                _content = value;

                this.RootGrid.Children.Clear();
                if (_content != null)
                {
                    this.RootGrid.Children.Add(_content);
                }
            }
        }
    }
}
