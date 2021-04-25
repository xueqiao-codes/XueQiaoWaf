using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace XueQiaoFoundation.BusinessResources.Converters
{
    /// <summary>
    /// 将 bool 转换为字面显示
    /// </summary>
    public class BooleanToDisplayNameConverter : IValueConverter
    {
        public string TrueDisplayText { get; set; }

        public string FalseDisplayText { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool tarVal)
            {
                return tarVal ? TrueDisplayText : FalseDisplayText;
            }
            return value;
        }
        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }

        private string DefaultBooleanDisplayText(bool value, CultureInfo culture)
        {
            return Properties.Resources.ResourceManager.GetString($"BooleanDisplayText_{(value?"true":"false")}", culture);
        }
    }
}
