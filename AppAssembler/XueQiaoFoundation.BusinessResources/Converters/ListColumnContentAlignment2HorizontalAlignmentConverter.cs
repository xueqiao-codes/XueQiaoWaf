using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using XueQiaoFoundation.BusinessResources.Constants;

namespace XueQiaoFoundation.BusinessResources.Converters
{
    public class ListColumnContentAlignment2HorizontalAlignmentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is HorizontalAlignment alignment) return alignment;

            int? tarIntValue = null;
            try
            {
                tarIntValue = System.Convert.ToInt32(value);
            }
            catch (Exception) { }
            if (tarIntValue == null) return null;
            
            if (tarIntValue == XueQiaoConstants.ListColumnContentAlignment_Center)
                return HorizontalAlignment.Center;
            else if (tarIntValue == XueQiaoConstants.ListColumnContentAlignment_Right)
                return HorizontalAlignment.Right;
            else 
                return HorizontalAlignment.Left;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is HorizontalAlignment alignment)
            {
                if (alignment == HorizontalAlignment.Center)
                    return XueQiaoConstants.ListColumnContentAlignment_Center;
                else if (alignment == HorizontalAlignment.Right)
                    return XueQiaoConstants.ListColumnContentAlignment_Right;
                else 
                    return XueQiaoConstants.ListColumnContentAlignment_Left;
            }
            return null;
        }
    }
}
