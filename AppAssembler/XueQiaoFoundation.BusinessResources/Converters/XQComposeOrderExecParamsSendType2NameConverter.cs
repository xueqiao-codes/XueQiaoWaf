using NativeModel.Trade;
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
    /// 将 <see cref="XQComposeOrderExecParamsSendType"/>转换为名称的转换器
    /// </summary>
    public class XQComposeOrderExecParamsSendType2NameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is XQComposeOrderExecParamsSendType _type)
            {
                return Properties.Resources.ResourceManager
                    .GetString($"{typeof(XQComposeOrderExecParamsSendType).Name}_{_type.ToString()}", culture);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
