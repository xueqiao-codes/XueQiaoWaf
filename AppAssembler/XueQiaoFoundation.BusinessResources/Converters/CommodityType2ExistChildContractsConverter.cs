using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using xueqiao.contract.standard;

namespace XueQiaoFoundation.BusinessResources.Converters
{
    /// <summary>
    /// 某种商品类型是否存在子合约的转换器
    /// </summary>
    public class CommodityType2ExistChildContractsConverter : IValueConverter
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
                return commodityType == SledCommodityType.SPREAD_MONTH || commodityType == SledCommodityType.SPREAD_COMMODITY;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
