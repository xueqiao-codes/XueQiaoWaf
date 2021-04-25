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
    public class ListColumnContentAlignment2NameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int? tarIntValue = null;
            if (value is HorizontalAlignment alignment)
            {
                if (alignment == HorizontalAlignment.Center)
                    tarIntValue = XueQiaoConstants.ListColumnContentAlignment_Center;
                else if (alignment == HorizontalAlignment.Right)
                    tarIntValue = XueQiaoConstants.ListColumnContentAlignment_Right;
                else
                    tarIntValue = XueQiaoConstants.ListColumnContentAlignment_Left;
            }
            
            try
            {
                tarIntValue = System.Convert.ToInt32(value);
            }
            catch (Exception) { }
            if (tarIntValue == null) return null;

            return Properties.Resources.ResourceManager.GetString($"ListColumnContentAlignment_{tarIntValue.Value}", culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
