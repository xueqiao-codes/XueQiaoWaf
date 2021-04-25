using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace XueQiaoFoundation.UI.Controls
{
    [TemplatePart(Name = ElementPasswordBox, Type = typeof(PasswordBox))]
    public class CustomPasswordBox : Control
    {
        private const string ElementPasswordBox = "PART_PasswordBox";

        public static readonly DependencyProperty PasswordCharProperty = PasswordBox.PasswordCharProperty.AddOwner(typeof(CustomPasswordBox));
        public static readonly DependencyProperty MaxLengthProperty = PasswordBox.MaxLengthProperty.AddOwner(typeof(CustomPasswordBox));
        public static readonly DependencyProperty SelectionBrushProperty = PasswordBox.SelectionBrushProperty.AddOwner(typeof(CustomPasswordBox));
        public static readonly DependencyProperty SelectionOpacityProperty = PasswordBox.SelectionOpacityProperty.AddOwner(typeof(CustomPasswordBox));
        public static readonly DependencyProperty CaretBrushProperty = PasswordBox.CaretBrushProperty.AddOwner(typeof(CustomPasswordBox));
        public static readonly DependencyProperty IsInactiveSelectionHighlightEnabledProperty = PasswordBox.IsInactiveSelectionHighlightEnabledProperty.AddOwner(typeof(CustomPasswordBox));
        public static readonly RoutedEvent PasswordChangedEvent = EventManager.RegisterRoutedEvent(
            "PasswordChanged",
            RoutingStrategy.Direct,
            typeof(RoutedEventHandler),
            typeof(CustomPasswordBox));

        private PasswordBox _PART_PasswordBox;


        static CustomPasswordBox()
        {   
            EventManager.RegisterClassHandler(typeof(CustomPasswordBox), UIElement.GotFocusEvent, new RoutedEventHandler(OnGotFocus));
        }


        public char PasswordChar
        {
            get { return (char)GetValue(PasswordCharProperty); }
            set { SetValue(PasswordCharProperty, value); }
        }
        
        public int MaxLength
        {
            get { return (int)GetValue(MaxLengthProperty); }
            set { SetValue(MaxLengthProperty, value); }
        }
        
        public Brush SelectionBrush
        {
            get { return (Brush)GetValue(SelectionBrushProperty); }
            set { SetValue(SelectionBrushProperty, value); }
        }
        
        public double SelectionOpacity
        {
            get { return (double)GetValue(SelectionOpacityProperty); }
            set { SetValue(SelectionOpacityProperty, value); }
        }
        
        public Brush CaretBrush
        {
            get { return (Brush)GetValue(CaretBrushProperty); }
            set { SetValue(CaretBrushProperty, value); }
        }
        
        public bool IsInactiveSelectionHighlightEnabled
        {
            get { return (bool)GetValue(IsInactiveSelectionHighlightEnabledProperty); }
            set { SetValue(IsInactiveSelectionHighlightEnabledProperty, value); }
        }

        public SecureString SecurePassword
        {
            get
            {
                if (_PART_PasswordBox != null)
                {
                    return _PART_PasswordBox.SecurePassword;
                }
                return null;
            }
        }
        
        public string Password
        {
            get
            {
                return _PART_PasswordBox?.Password;
            }
            set
            {
                if (_PART_PasswordBox != null)
                {
                    _PART_PasswordBox.Password = value;
                }
            }
        }
        
        public event RoutedEventHandler PasswordChanged
        {
            add { AddHandler(PasswordChangedEvent, value); }
            remove { RemoveHandler(PasswordChangedEvent, value); }
        }
        
        protected Binding GetBinding(DependencyProperty property)
        {
            return new Binding(property.Name) { Source = this };
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _PART_PasswordBox = GetTemplateChild(ElementPasswordBox) as PasswordBox;

            ApplyBindings();
        }

        protected virtual void ApplyBindings()
        {
            if (_PART_PasswordBox != null)
            {
                _PART_PasswordBox.SetBinding(PasswordBox.PasswordCharProperty, GetBinding(PasswordCharProperty));
                _PART_PasswordBox.SetBinding(PasswordBox.MaxLengthProperty, GetBinding(MaxLengthProperty));
                _PART_PasswordBox.SetBinding(PasswordBox.SelectionBrushProperty, GetBinding(SelectionBrushProperty));
                _PART_PasswordBox.SetBinding(PasswordBox.SelectionOpacityProperty, GetBinding(SelectionOpacityProperty));
                _PART_PasswordBox.SetBinding(PasswordBox.CaretBrushProperty, GetBinding(CaretBrushProperty));
                _PART_PasswordBox.SetBinding(PasswordBox.IsInactiveSelectionHighlightEnabledProperty, GetBinding(IsInactiveSelectionHighlightEnabledProperty));
                _PART_PasswordBox.PasswordChanged += PasswordBox_OnPasswordChanged; ;
            }
        }

        private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(PasswordChangedEvent, this));
        }

        /// <summary> 
        ///     Called when this element or any below gets focus.
        /// </summary>
        private static void OnGotFocus(object sender, RoutedEventArgs e)
        {
            // When NumericUpDown gets logical focus, select the text inside us.
            CustomPasswordBox _customPasswordBox = (CustomPasswordBox)sender;

            // Forward focus to the TextBox element
            if (!e.Handled)
            {
                if (_customPasswordBox.Focusable && _customPasswordBox._PART_PasswordBox != null)
                {
                    if (e.OriginalSource == _customPasswordBox)
                    {
                        // MoveFocus takes a TraversalRequest as its argument.
                        var request = new TraversalRequest((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift ? FocusNavigationDirection.Previous : FocusNavigationDirection.Next);
                        // Gets the element with keyboard focus.
                        var elementWithFocus = Keyboard.FocusedElement as UIElement;
                        // Change keyboard focus.
                        elementWithFocus?.MoveFocus(request);
                        e.Handled = true;
                    }
                }
            }
        }
    }
}
