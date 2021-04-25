using System;
using System.ComponentModel;
using System.Windows;

namespace CustomChromeLibrary
{
	public class MaximizeButton : CaptionButton
	{
		static MaximizeButton()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(MaximizeButton), new FrameworkPropertyMetadata(typeof(MaximizeButton)));
		}

		public MaximizeButton()
		{
			DataContext = this;
            Loaded += _Loaded;
            Unloaded += _Unloaded;
        }

        //protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
        //{
        //    base.OnRender(drawingContext);
        //    SubscribeWindowStateChanged();
        //    InvalidateIconsVisibility();
        //}

        protected override void OnClick()
        {
            base.OnClick();
            Window w = Window.GetWindow(this);
            if (w.WindowState == System.Windows.WindowState.Maximized)
                SystemCommands.RestoreWindow(w);
            else
                SystemCommands.MaximizeWindow(w);
        }

        public Visibility MaximizeIconVisibility
        {
            get { return (Visibility)GetValue(MaximizeIconVisibilityProperty); }
            private set { SetValue(MaximizeIconVisibilityProperty, value); }
        }

        public static readonly DependencyProperty MaximizeIconVisibilityProperty =
            DependencyProperty.Register("MaximizeIconVisibility", typeof(Visibility), typeof(MaximizeButton), new PropertyMetadata(default(Visibility)));
        
        public Visibility RestoreIconVisibility
        {
            get { return (Visibility)GetValue(RestoreIconVisibilityProperty); }
            private set { SetValue(RestoreIconVisibilityProperty, value); }
        }

        public static readonly DependencyProperty RestoreIconVisibilityProperty =
            DependencyProperty.Register("RestoreIconVisibility", typeof(Visibility), typeof(MaximizeButton), new PropertyMetadata(default(Visibility)));


        private void _Loaded(object sender, RoutedEventArgs e)
        {
            SubscribeWindowStateChanged();
            InvalidateIconsVisibility();
        }

        private void _Unloaded(object sender, RoutedEventArgs e)
        {
            UnsubscribeWindowStateChanged();
        }
        
        private void SubscribeWindowStateChanged()
        {
            Window w = Window.GetWindow(this);
            if (w != null)
            {
                w.StateChanged -= WindowStateChanged;
                w.StateChanged += WindowStateChanged;
            }
        }

        private void UnsubscribeWindowStateChanged()
        {
            Window w = Window.GetWindow(this);
            if (w != null)
            {
                w.StateChanged -= WindowStateChanged;
            }
        }

        private void WindowStateChanged(object sender, EventArgs e)
		{
            InvalidateIconsVisibility();
        }
        
        private void InvalidateIconsVisibility()
        {
            Window w = Window.GetWindow(this);
            if (w != null)
            {
                this.MaximizeIconVisibility = w.WindowState == System.Windows.WindowState.Maximized ? Visibility.Collapsed : Visibility.Visible;
                this.RestoreIconVisibility = w.WindowState != System.Windows.WindowState.Maximized ? Visibility.Collapsed : Visibility.Visible;
            }
        }
	}
}
