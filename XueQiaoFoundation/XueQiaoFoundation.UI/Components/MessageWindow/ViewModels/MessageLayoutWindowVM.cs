using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoFoundation.UI.Components.MessageWindow.Views;

namespace XueQiaoFoundation.UI.Components.MessageWindow.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class MessageLayoutWindowVM : ViewModel<MessageLayoutWindow>, IMessageWindow
    {
        [ImportingConstructor]
        public MessageLayoutWindowVM(MessageLayoutWindow view) : base(view)
        {
            // 设置默认
            WindowMinWidth = 200;
            WindowMinHeight = 100;
        }

        private MessageWindowCaptionHeightHolder captionHeightHolder;
        public MessageWindowCaptionHeightHolder CaptionHeightHolder
        {
            get { return captionHeightHolder; }
            set { SetProperty(ref captionHeightHolder, value); }
        }

        private object contentView;
        public object ContentView
        {
            get { return contentView; }
            set { SetProperty(ref contentView, value); }
        }
        
        private bool windowCanResize;
        public bool WindowCanResize
        {
            get { return windowCanResize; }
            set { SetProperty(ref windowCanResize, value); }
        }

        public event CancelEventHandler Closing
        {
            add { ViewCore.Closing += value; }
            remove { ViewCore.Closing -= value; }
        }

        public event EventHandler Closed
        {
            add { ViewCore.Closed += value; }
            remove { ViewCore.Closed -= value; }
        }

        public event EventHandler ContentRendered
        {
            add { ViewCore.ContentRendered += value; }
            remove { ViewCore.ContentRendered -= value; }
        }
        
        public void Close()
        {
            ViewCore.Close();
        }

        
        public bool WindowShowInTaskbar
        {
            get { return ViewCore.ShowInTaskbar; }
            set
            { 
                ViewCore.ConfigWindow(showInTaskBar: new Tuple<bool>(value));
            }
        }
        
        public object WindowOwner
        {
            get { return ViewCore.Owner; }
            set
            {
                ViewCore.ConfigWindow(owner: new Tuple<Window>(value as Window));
            }
        }
        
        public Point WindowLocation
        {
            get { return new Point(ViewCore.Left, ViewCore.Top); }
            set
            {
                ViewCore.ConfigWindow(location: new Tuple<Point>(value));
            }
        }

        public WindowStartupLocation WindowStartupLocation
        {
            get { return ViewCore.WindowStartupLocation; }
            set
            {
                ViewCore.ConfigWindow(startupLocation: new Tuple<WindowStartupLocation>(value));
            }
        }
        
        public Size WindowSize
        {
            get { return ViewCore.RenderSize; }
            set
            {
                ViewCore.ConfigWindow(windowSize: new Tuple<Size>(value));
            }
        }
        
        public SizeToContent WindowSizeToContent
        {
            get { return ViewCore.SizeToContent; }
            set
            {
                ViewCore.ConfigWindow(sizeToContent: new Tuple<SizeToContent>(value));
            }
        }
        
        public double WindowMaxWidth
        {
            get { return ViewCore.MaxWidth; }
            set
            {
                ViewCore.ConfigWindow(windowMaxWidth: new Tuple<double>(value));
            }
        }
        
        public double WindowMaxHeight
        {
            get { return ViewCore.MaxHeight; }
            set
            {
                ViewCore.ConfigWindow(windowMaxHeight: new Tuple<double>(value));
            }
        }
        
        public double WindowMinWidth
        {
            get { return ViewCore.MinWidth; }
            set
            {
                ViewCore.ConfigWindow(windowMinWidth: new Tuple<double>(value));
            }
        }
        
        public double WindowMinHeight
        {
            get { return ViewCore.MinHeight; }
            set
            {
                ViewCore.ConfigWindow(windowMinHeight: new Tuple<double>(value));
            }
        }

        #region IMessageWindow
        
        public bool Topmost
        {
            get { return ViewCore.Topmost; }
            set
            {
                ViewCore.ConfigWindow(topmost: new Tuple<bool>(value));
            }
        }

        public void ShowDialog(bool topmost = false)
        {
            this.Topmost = topmost;
            ViewCore.ShowDialog();
        }

        public void Show(bool topmost = false)
        {
            this.Topmost = topmost;
            ViewCore.Show();
        }

        public void Activate()
        {
            ViewCore.Activate();
        }

        public void Hide()
        {
            ViewCore.Hide();
        }

        #endregion
    }
}
