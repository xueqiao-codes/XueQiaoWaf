using NativeModel.Contract;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using xueqiao.contract.standard;

namespace XueQiaoFoundation.BusinessResources.Converters
{
    public class CommodityType2NameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SledCommodityType? commodityType = null;
            if (value is SledCommodityType sledType)
            {
                commodityType = sledType;
            }
            else if (value is int typeCode)
            {
                if (Enum.IsDefined(typeof(SledCommodityType), typeCode))
                {
                    commodityType = (SledCommodityType)typeCode;
                }
            }
            else if (value is string cTypeName)
            {
                if (Enum.TryParse(cTypeName, out SledCommodityType parsedType))
                {
                    commodityType = parsedType;
                }
            }
            
            if (commodityType != null)
            {
                return Properties.Resources.ResourceManager
                    .GetString($"CommodityType_{commodityType.ToString()}", culture);
            }
            
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
