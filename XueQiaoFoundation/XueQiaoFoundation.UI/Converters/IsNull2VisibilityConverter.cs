using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace XueQiaoFoundation.UI.Converters
{
    public class IsNull2VisibilityConverter : IValueConverter
    {
        public Visibility NullTargetVisibility { get; set; }
        public Visibility NotNullTargetVisibility { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (null == value)
            {
                return NullTargetVisibility;
            }
            return NotNullTargetVisibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
