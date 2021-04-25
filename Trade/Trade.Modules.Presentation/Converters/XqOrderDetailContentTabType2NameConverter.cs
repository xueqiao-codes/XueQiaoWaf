using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using XueQiaoWaf.Trade.Modules.Applications.ViewModels;

namespace XueQiaoWaf.Trade.Modules.Presentation.Converters
{
    /// <summary>
    /// 将 <see cref="XqOrderDetailContentTabType"/>转换为名称的转换器
    /// </summary>
    public class XqOrderDetailContentTabType2NameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is XqOrderDetailContentTabType tabType)
            {
                return Properties.Resources.ResourceManager
                    .GetString($"{typeof(XqOrderDetailContentTabType).Name}_{tabType.ToString()}");
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
