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
    /// 合约`报价单位`显示转换器
    /// </summary>
    public class ContractQuoteUnitDisplayConverter : IMultiValueConverter
    {
        /// <summary>
        /// value0: Type of <see cref="ICurrencyUnitsService"/> instance.
        /// value1: ChargeUnit, eg: 1, 0.01
        /// value2: Currency, eg: USD, CNY
        /// value3: MeasureUnit, eg: 顿, 克, 指数点 等   
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

            // `指数点`单位做特殊处理
            if (measureUnit == "指数点")
                return measureUnit;

            // 其他单位处理
            if (currencyUnitsService == null || chargeUnit == null 
                || string.IsNullOrEmpty(currency) || string.IsNullOrEmpty(measureUnit))
                return null;
            
            var currencyUnitName = currencyUnitsService.GetCurrencyUnitName(currency, chargeUnit.Value);
            if (string.IsNullOrEmpty(currencyUnitName)) return null;

            return $"{currencyUnitName}/{measureUnit}";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
