using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace XueQiaoFoundation.UI.Controls
{
    public class CustomHeaderedWindowHeader : ContentControl
    {
        static CustomHeaderedWindowHeader()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomHeaderedWindowHeader), new FrameworkPropertyMetadata(typeof(CustomHeaderedWindowHeader)));
        }


        public bool IsHideCloseWindowMenuButton
        {
            get { return (bool)GetValue(IsHideCloseWindowMenuButtonProperty); }
            set { SetValue(IsHideCloseWindowMenuButtonProperty, value); }
        }

        public static readonly DependencyProperty IsHideCloseWindowMenuButtonProperty =
            DependencyProperty.Register("IsHideCloseWindowMenuButton", typeof(bool), typeof(CustomHeaderedWindowHeader), new PropertyMetadata(default(bool)));


        public bool IsHideMaximizeWindowMenuButton
        {
            get { return (bool)GetValue(IsHideMaximizeWindowMenuButtonProperty); }
            set { SetValue(IsHideMaximizeWindowMenuButtonProperty, value); }
        }

        public static readonly DependencyProperty IsHideMaximizeWindowMenuButtonProperty =
            DependencyProperty.Register("IsHideMaximizeWindowMenuButton", typeof(bool), typeof(CustomHeaderedWindowHeader), new PropertyMetadata(default(bool)));


        public bool IsHideMinimizeWindowMenuButton
        {
            get { return (bool)GetValue(IsHideMinimizeWindowMenuButtonProperty); }
            set { SetValue(IsHideMinimizeWindowMenuButtonProperty, value); }
        }

        public static readonly DependencyProperty IsHideMinimizeWindowMenuButtonProperty =
            DependencyProperty.Register("IsHideMinimizeWindowMenuButton", typeof(bool), typeof(CustomHeaderedWindowHeader), new PropertyMetadata(default(bool)));

        public Thickness WindowMenuButtonsContainerMargin
        {
            get { return (Thickness)GetValue(WindowMenuButtonsContainerMarginProperty); }
            set { SetValue(WindowMenuButtonsContainerMarginProperty, value); }
        }

        public static readonly DependencyProperty WindowMenuButtonsContainerMarginProperty =
            DependencyProperty.Register("WindowMenuButtonsContainerMargin", typeof(Thickness), typeof(CustomHeaderedWindowHeader), new PropertyMetadata(new Thickness(6,6,6,6)));
        
        public RoutedEventHandler CloseWindowMenuButtonClickHandler
        {
            get { return (RoutedEventHandler)GetValue(CloseWindowMenuButtonClickHandlerProperty); }
            set { SetValue(CloseWindowMenuButtonClickHandlerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CloseWindowMenuButtonClickHandler.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CloseWindowMenuButtonClickHandlerProperty =
            DependencyProperty.Register("CloseWindowMenuButtonClickHandler", typeof(RoutedEventHandler), typeof(CustomHeaderedWindowHeader), new PropertyMetadata(null));
        
    }
}
