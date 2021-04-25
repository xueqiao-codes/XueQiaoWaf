using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace XueQiaoFoundation.UI.Styles
{
    /// <summary>
    /// Helper class for styles
    /// </summary>
    public static class StylesHelper 
    {
        public static CornerRadius GetCornerRadius(DependencyObject obj)
        {
            return (CornerRadius)obj.GetValue(CornerRadiusProperty);
        }

        public static void SetCornerRadius(DependencyObject obj, CornerRadius value)
        {
            obj.SetValue(CornerRadiusProperty, value);
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.RegisterAttached("CornerRadius", typeof(CornerRadius), typeof(StylesHelper), new PropertyMetadata(default(CornerRadius)));



        public static Brush GetBorderBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(BorderBrushProperty);
        }

        public static void SetBorderBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(BorderBrushProperty, value);
        }

        // Using a DependencyProperty as the backing store for BorderBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BorderBrushProperty =
            DependencyProperty.RegisterAttached("BorderBrush", typeof(Brush), typeof(StylesHelper), new PropertyMetadata(null));




        public static Brush GetHoverBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(HoverBrushProperty);
        }

        public static void SetHoverBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(HoverBrushProperty, value);
        }

        // Using a DependencyProperty as the backing store for HoverBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HoverBrushProperty =
            DependencyProperty.RegisterAttached("HoverBrush", typeof(Brush), typeof(StylesHelper), new PropertyMetadata(null));

        public static Brush GetPressedBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(PressedBrushProperty);
        }

        public static void SetPressedBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(PressedBrushProperty, value);
        }

        // Using a DependencyProperty as the backing store for PressedBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PressedBrushProperty =
            DependencyProperty.RegisterAttached("PressedBrush", typeof(Brush), typeof(StylesHelper), new PropertyMetadata(null));



        public static Brush GetSelectedBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(SelectedBrushProperty);
        }

        public static void SetSelectedBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(SelectedBrushProperty, value);
        }

        // Using a DependencyProperty as the backing store for SelectedBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedBrushProperty =
            DependencyProperty.RegisterAttached("SelectedBrush", typeof(Brush), typeof(StylesHelper), new PropertyMetadata(null));



        public static Brush GetSelectedActiveBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(SelectedActiveBrushProperty);
        }

        public static void SetSelectedActiveBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(SelectedActiveBrushProperty, value);
        }

        // Using a DependencyProperty as the backing store for SelectedActiveBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedActiveBrushProperty =
            DependencyProperty.RegisterAttached("SelectedActiveBrush", typeof(Brush), typeof(StylesHelper), new PropertyMetadata(null));


        public static Brush GetFocusedBorderBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(FocusedBorderBrushProperty);
        }
        
        public static void SetFocusedBorderBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(FocusedBorderBrushProperty, value);
        }

        // Using a DependencyProperty as the backing store for FocusedBorderBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FocusedBorderBrushProperty =
            DependencyProperty.RegisterAttached("FocusedBorderBrush", typeof(Brush), typeof(StylesHelper), new PropertyMetadata(null));


        public static Brush GetValidationErrorBorderBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(ValidationErrorBorderBrushProperty);
        }

        public static void SetValidationErrorBorderBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(ValidationErrorBorderBrushProperty, value);
        }

        // Using a DependencyProperty as the backing store for ValidationErrorBorderBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValidationErrorBorderBrushProperty =
            DependencyProperty.RegisterAttached("ValidationErrorBorderBrush", typeof(Brush), typeof(StylesHelper), new PropertyMetadata(null));



        public static bool GetScrollBarShowLineButton(DependencyObject obj)
        {
            return (bool)obj.GetValue(ScrollBarShowLineButtonProperty);
        }

        public static void SetScrollBarShowLineButton(DependencyObject obj, bool value)
        {
            obj.SetValue(ScrollBarShowLineButtonProperty, value);
        }

        // Using a DependencyProperty as the backing store for ScrollBarShowLineButton.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScrollBarShowLineButtonProperty =
            DependencyProperty.RegisterAttached("ScrollBarShowLineButton", typeof(bool), typeof(StylesHelper), new PropertyMetadata(true));
    }
}
