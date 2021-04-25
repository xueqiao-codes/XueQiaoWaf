using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace XueQiaoFoundation.UI.Styles
{
    public static class HeaderedContentControlHelper
    {
        public static string GetHorizonalHeaderColumnSharedSizeGroup(DependencyObject obj)
        {
            return (string)obj.GetValue(HorizonalHeaderColumnSharedSizeGroupProperty);
        }

        public static void SetHorizonalHeaderColumnSharedSizeGroup(DependencyObject obj, string value)
        {
            obj.SetValue(HorizonalHeaderColumnSharedSizeGroupProperty, value);
        }

        /// <summary>
        /// 横向控件时头部列共享宽度组名
        /// </summary>
        public static readonly DependencyProperty HorizonalHeaderColumnSharedSizeGroupProperty =
            DependencyProperty.RegisterAttached("HorizonalHeaderColumnSharedSizeGroup", 
                typeof(string), typeof(HeaderedContentControlHelper), 
                new PropertyMetadata(null));



        public static double GetHorizonalColumnSpacing(DependencyObject obj)
        {
            return (double)obj.GetValue(HorizonalColumnSpacingProperty);
        }

        public static void SetHorizonalColumnSpacing(DependencyObject obj, double value)
        {
            obj.SetValue(HorizonalColumnSpacingProperty, value);
        }

        /// <summary>
        /// 横向控件时各列的间隙
        /// </summary>
        public static readonly DependencyProperty HorizonalColumnSpacingProperty =
            DependencyProperty.RegisterAttached("HorizonalColumnSpacing", 
                typeof(double), typeof(HeaderedContentControlHelper), new PropertyMetadata((double)0));



    }
}
