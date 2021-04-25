using NativeModel.Contract;
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
    /// 商品的某个合约转化为显示名称的转化器。
    /// values[0]: <see cref="NativeCommodity"/>类型
    /// values[1]: <see cref="NativeContract"/> 类型
    /// </summary>
    public class ContractToDisplayNameConverter : IMultiValueConverter
    {
        public CommodityContractToDisplayNameMode DisplayNameMode { get; set; } = CommodityContractToDisplayNameMode.CommodityNameContractCode;

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2) throw new ArgumentException("values' length must == 2.");
            
            var commodity = values.FirstOrDefault() as NativeCommodity;
            var contract = values.LastOrDefault() as NativeContract;
            var commodityName = (commodity?.CnAcronym ?? commodity?.SledCommodityCode)??"";
            var commodityCode = commodity?.SledCommodityCode??"";
            var contractCode = contract?.SledContractCode ?? "";

            string tarName = null;
            switch (DisplayNameMode)
            {
                case CommodityContractToDisplayNameMode.CommodityCodeContractCode:
                    tarName = $"{commodityCode}{contractCode}";
                    break;
                case CommodityContractToDisplayNameMode.CommodityNameContractCode:
                default:
                    tarName = $"{commodityName}{contractCode}";
                    break;
            }
            return tarName;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw null;
        }
    }

    public enum CommodityContractToDisplayNameMode
    {
        CommodityNameContractCode = 0,
        CommodityCodeContractCode = 1
    }
}
