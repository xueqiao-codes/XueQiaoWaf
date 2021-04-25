using BolapanControl.ItemsFilter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace BolapanControl.ItemsFilter.View
{
    /// <summary>
    /// 过滤器的 DataTemplate 的选择器
    /// </summary>
    public class FilterItemTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// 类型为<see cref="BolapanControl.ItemsFilter.Model.EqualFilter"/>的项目的 DataTemplate
        /// </summary>
        public DataTemplate EqualFilter_DataTemplate { get; set; }

        /// <summary>
        /// 类型为<see cref="BolapanControl.ItemsFilter.Model.LessOrEqualFilter<T>"/>的项目的 DataTemplate
        /// </summary>
        public DataTemplate LessOrEqualFilter_DataTemplate { get; set; }

        /// <summary>
        /// 类型为<see cref="BolapanControl.ItemsFilter.Model.GreaterOrEqualFilter<T>"/>的项目的 DataTemplate
        /// </summary>
        public DataTemplate GreaterOrEqualFilter_DataTemplate { get; set; }

        /// <summary>
        /// 类型为<see cref="BolapanControl.ItemsFilter.Model.RangeFilter<T>"/>的项目的 DataTemplate
        /// </summary>
        public DataTemplate RangeFilter_DataTemplate { get; set; }

        /// <summary>
        /// 类型为<see cref="BolapanControl.ItemsFilter.Model.StringFilter"/>的项目的 DataTemplate
        /// </summary>
        public DataTemplate StringFilter_DataTemplate { get; set; }

        /// <summary>
        /// 类型为<see cref="BolapanControl.ItemsFilter.Model.EnumFilter<T>"/>的项目的 DataTemplate
        /// </summary>
        public DataTemplate EnumFilter_DataTemplate { get; set; }


        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (GetRootType(item.GetType(), "EqualFilter", 0) != null)
            {
                return EqualFilter_DataTemplate;
            }
            else if (GetRootType(item.GetType(), "LessOrEqualFilter", 1) != null)
            {
                return LessOrEqualFilter_DataTemplate;
            }
            else if (GetRootType(item.GetType(), "GreaterOrEqualFilter", 1) != null)
            {
                return GreaterOrEqualFilter_DataTemplate;
            }
            else if (GetRootType(item.GetType(), "RangeFilter", 1) != null)
            {
                return RangeFilter_DataTemplate;
            }
            else if (GetRootType(item.GetType(), "StringFilter", 0) != null)
            {
                return StringFilter_DataTemplate;
            }
            else if (GetRootType(item.GetType(), "EnumFilter", 1) != null)
            {
                return EnumFilter_DataTemplate;
            }

            return base.SelectTemplate(item, container);
        }

        private Type GetRootType(Type itemType, string rootTypeNameWithoutGenericArity, UInt16 rootTypeGenericArgsNum)
        {
            if (itemType == null) return null;

            var baseType = itemType;
            while (baseType != null)
            {
                var baseTypeNameWithoutGeneric = GetNameWithoutGenericArity(baseType);
                if (baseTypeNameWithoutGeneric == rootTypeNameWithoutGenericArity
                    && baseType.GetGenericArguments().Length == rootTypeGenericArgsNum)
                {
                    return baseType;
                }

                baseType = baseType.BaseType;
            }

            return null;
        }

        private static string GetNameWithoutGenericArity(Type t)
        {
            string name = t.Name;
            int index = name.IndexOf('`');
            return index == -1 ? name : name.Substring(0, index);
        }
    }
}
