using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using xueqiao.trade.hosting.position.statis;

namespace XueQiaoWaf.Trade.Modules.Presentation.Converters
{
    /// <summary>
    /// 某个币种的平仓收益转换器
    /// </summary>
    public class ClosedProfitOfCurrencyConverter : IMultiValueConverter
    {
        /// <summary>
        /// value0: <see cref="IEnumerable{ClosedProfit}"/>不同货币类型的收益列表
        /// value1: Currency,货币
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length != 2) return null;
            var currencyProfitList = values[0] as IEnumerable<ClosedProfit>;
            var currency = values[1] as string;
            if (currencyProfitList == null || string.IsNullOrEmpty(currency))
                return null;

            return currencyProfitList?.FirstOrDefault(i => i.TradeCurrency == currency)?.ClosedProfitValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
