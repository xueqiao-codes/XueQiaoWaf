using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using xueqiao.contract.standard;
using XueQiaoFoundation.Interfaces.Applications;

namespace XueQiaoWaf.Trade.Modules.Presentation.Converters
{
    /// <summary>
    /// 合约`保证金`显示转换器
    /// </summary>
    public class ContractMarginDisplayConverter : IMultiValueConverter
    {
        /// <summary>
        /// value0: Margin 保证金, eg: 1, 0.01
        /// value1: MarginCalculateMode 保证金计算方式, <see cref="CalculateMode"/>
        /// value2: Type of <see cref="ICurrencyUnitsService"/> instance.
        /// value3: Currency, eg: USD, CNY
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var margin = values[0] as double?;
            var calculateMode = values[1] as CalculateMode?;
            var currencyUnitsService = values[2] as ICurrencyUnitsService;
            var currency = values[3] as string;
            if (margin == null || calculateMode == null
                || currencyUnitsService == null || string.IsNullOrEmpty(currency))
                return null;

            var currencyUnitName = currencyUnitsService.GetCurrencyUnitName(currency, 1);
            if (string.IsNullOrEmpty(currencyUnitName)) return null;

            // Only support `PERCENTAGE` and  `QUOTA` now
            if (calculateMode == CalculateMode.PERCENTAGE)
                return margin.Value.ToString("P");
            else if (calculateMode == CalculateMode.QUOTA)
                return $"{margin.Value} {currencyUnitName}";

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
