using System;
using System.Windows;
using System.Windows.Controls;

namespace CustomChromeLibrary
{
	public class CloseButton : CaptionButton
	{
		static CloseButton()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(CloseButton), new FrameworkPropertyMetadata(typeof(CloseButton)));
		}

		protected override void OnClick()
		{
			base.OnClick();

            var routedEventArgs = new RoutedEventArgs(Button.ClickEvent, this);
            this.CustomButtonClickHandler?.Invoke(this, routedEventArgs);
            if (!routedEventArgs.Handled)
            {
                SystemCommands.CloseWindow(Window.GetWindow(this));
            }
        }
	}
}
