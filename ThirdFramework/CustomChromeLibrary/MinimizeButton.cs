using System;
using System.Windows;

namespace CustomChromeLibrary
{
	public class MinimizeButton : CaptionButton
	{
		static MinimizeButton()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(MinimizeButton), new FrameworkPropertyMetadata(typeof(MinimizeButton)));
		}

		protected override void OnClick()
		{
			base.OnClick();
			SystemCommands.MinimizeWindow(Window.GetWindow(this));
		}
	}
}
