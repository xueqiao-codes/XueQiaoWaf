using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using XueQiaoFoundation.BusinessResources.DataModels;

namespace XueQiaoFoundation.BusinessResources.Converters
{
    /// <summary>
    /// 检查是否为 TradeTabWorkspaceTemplate 的转换器
    /// </summary>
    public class TradeTabWorkspaceTemplateCheckConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is TradeTabWorkspaceTemplate;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
