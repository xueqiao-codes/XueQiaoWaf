using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;

namespace XueQiaoWaf.Trade.Modules.Presentation.Converters
{
    public class OrderListColumnEntrusted2NameConverter : IValueConverter
    {
        /// <summary>
        /// 转换方法
        /// </summary>
        /// <param name="value"><see cref="OrderListColumn_Entrusted"/></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int? tarValue = null;
            if (value is int tarInt)
            {
                tarValue = tarInt;
            }
            else if (value is OrderListColumn_Entrusted columnType)
            {
                tarValue = columnType.GetHashCode();
            }
            if (tarValue != null)
            {
                return Properties.Resources.ResourceManager.GetString($"OrderListColumn_Entrusted_{tarValue}");
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
