using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using XueQiaoFoundation.Interfaces.Applications;

namespace XueQiaoWaf.Trade.Modules.Presentation.Converters
{
    /// <summary>
    /// 外盘合约的某些属性转换为 null 的转换器
    /// </summary>
    public class OutterContractProp2NullConverter : IMultiValueConverter
    {
        /// <summary>
        /// value0: originValue，某个属性的 value 值
        /// value1: exchangeMic，合约所属交易所 Mic
        /// value2: exchangeDataService， <see cref="IExchangeDataService"/> 对象
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var originValue = values[0];
            var exchangeMic = values[1] as string;
            var exchangeDataService = values[2] as IExchangeDataService;
            if (exchangeMic == null || exchangeDataService == null)
                return null;
            if (!exchangeDataService.IsInnerExchange(exchangeMic))
                return null;
            return ReturnValidTargetValue(originValue, targetType);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private object ReturnValidTargetValue(object srcValue, Type targetType)
        {
            if (targetType == null) return srcValue;
            if (typeof(string).IsAssignableFrom(targetType))
            {
                if (srcValue == DependencyProperty.UnsetValue)
                    return "";
                return srcValue?.ToString();
            }
            return srcValue;
        }
    }
}
