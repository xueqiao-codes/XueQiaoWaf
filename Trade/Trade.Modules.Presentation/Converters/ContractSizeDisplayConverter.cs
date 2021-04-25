using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using XueQiaoFoundation.Interfaces.Applications;

namespace XueQiaoWaf.Trade.Modules.Presentation.Converters
{
    /// <summary>
    /// 合约每手乘数显示转换器
    /// </summary>
    public class ContractSizeDisplayConverter : IMultiValueConverter
    {
        /// <summary>
        /// value0: Type of <see cref="ICurrencyUnitsService"/> instance.
        /// value1: ChargeUnit, eg: 1, 0.01
        /// value2: Currency, eg: USD, CNY
        /// value3: MeasureUnit, eg: 顿, 克, 指数点 等   
        /// value4: ContractSize, 每手乘数
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var currencyUnitsService = values[0] as ICurrencyUnitsService;
            var chargeUnit = values[1] as double?;
            var currency = values[2] as string;
            var measureUnit = values[3] as string;
            var contractSize = values[4] as double?;
            
            // 其他单位处理
            if (currencyUnitsService == null || chargeUnit == null
                || string.IsNullOrEmpty(currency) || string.IsNullOrEmpty(measureUnit)
                || contractSize == null)
                return null;

            var currencyUnitName = currencyUnitsService.GetCurrencyUnitName(currency, chargeUnit.Value);
            if (string.IsNullOrEmpty(currencyUnitName)) return null;

            // `指数点`单位做特殊处理
            if (measureUnit == "指数点")
            {
                return $"{measureUnit} * {contractSize.Value}{currencyUnitName}";
            }

            
            return $"{contractSize} {measureUnit}";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
