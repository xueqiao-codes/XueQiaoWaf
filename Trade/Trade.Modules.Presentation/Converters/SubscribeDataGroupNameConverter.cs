using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using XueQiaoWaf.Trade.Interfaces.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;

namespace XueQiaoWaf.Trade.Modules.Presentation.Converters
{
    public class SubscribeDataGroupNameConverter : IMultiValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="values">value0:<see cref="SubscribeDataGroupType"/>group type; value1:group name</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2) return null;
            var originGroupName = values[1] as string;
            if (values[0] is SubscribeDataGroupType tagType)
            {
                if (tagType == SubscribeDataGroupType.Custom)
                    return originGroupName;
                else
                {
                    var name = originGroupName;
                    if (string.IsNullOrEmpty(name))
                        name = Properties.Resources.ResourceManager.GetString($"SubscribeDataGroupType_{tagType.ToString()}", culture);
                    return name;
                }
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
