using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using XueQiaoFoundation.Shared.Model;

namespace XueQiaoFoundation.UI.Extra.converter
{
    /// <summary>
    /// 检查是否为 <see cref="XueQiaoFoundation.Shared.Model.DraggableComponentUIDM"/> 的转换器
    /// </summary>
    public class DraggableComponentUIDMCheckConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is DraggableComponentUIDM;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
