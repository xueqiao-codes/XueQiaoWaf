using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using XueQiaoFoundation.Shared.Model;

namespace XueQiaoFoundation.BusinessResources.Resources
{
    /// <summary>
    /// 行情项附加属性提供者类
    /// </summary>
    public class QuotationAttachPropertiesProvider
    {
        public static bool GetIsXqTargetExpired(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsXqTargetExpiredProperty);
        }

        public static void SetIsXqTargetExpired(DependencyObject obj, bool value)
        {
            obj.SetValue(IsXqTargetExpiredProperty, value);
        }

        /// <summary>
        /// `标的是否过期`附加属性
        /// </summary>
        public static readonly DependencyProperty IsXqTargetExpiredProperty =
            DependencyProperty.RegisterAttached("IsXqTargetExpired", typeof(bool), typeof(QuotationAttachPropertiesProvider), new PropertyMetadata(false));

        
        public static bool GetIsInteractive(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsInteractiveProperty);
        }

        public static void SetIsInteractive(DependencyObject obj, bool value)
        {
            obj.SetValue(IsInteractiveProperty, value);
        }
        
        /// <summary>
        /// `是否可交互`附加属性
        /// </summary>
        public static readonly DependencyProperty IsInteractiveProperty =
            DependencyProperty.RegisterAttached("IsInteractive", typeof(bool), typeof(QuotationAttachPropertiesProvider), new PropertyMetadata(false));
        


        public static XQCompareResult? GetCompareResult1(DependencyObject obj)
        {
            return (XQCompareResult?)obj.GetValue(CompareResult1Property);
        }

        public static void SetCompareResult1(DependencyObject obj, XQCompareResult? value)
        {
            obj.SetValue(CompareResult1Property, value);
        }

        /// <summary>
        /// `比较结果1`附加属性。可以根据各自业务需要，指定该值
        /// </summary>
        public static readonly DependencyProperty CompareResult1Property =
            DependencyProperty.RegisterAttached("CompareResult1", typeof(XQCompareResult?), typeof(QuotationAttachPropertiesProvider), new PropertyMetadata(default(XQCompareResult?)));

        

        public static XQCompareResult? GetCompareResult2(DependencyObject obj)
        {
            return (XQCompareResult?)obj.GetValue(CompareResult2Property);
        }

        public static void SetCompareResult2(DependencyObject obj, XQCompareResult? value)
        {
            obj.SetValue(CompareResult2Property, value);
        }

        /// <summary>
        /// `比较结果2`附加属性。可以根据各自业务需要，指定该值
        /// </summary>
        public static readonly DependencyProperty CompareResult2Property =
            DependencyProperty.RegisterAttached("CompareResult2", typeof(XQCompareResult?), typeof(QuotationAttachPropertiesProvider), new PropertyMetadata(default(XQCompareResult?)));


    }
}
