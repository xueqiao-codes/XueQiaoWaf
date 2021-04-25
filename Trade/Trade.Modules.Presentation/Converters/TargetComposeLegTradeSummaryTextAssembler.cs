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
    /// <summary>
    /// 将<see cref="TargetComposeLegTradeSummary"/>的几个元素组装起来形成文字描述
    /// </summary>
    public class TargetComposeLegTradeSummaryTextAssembler : IMultiValueConverter
    {
        /// <summary>
        /// value0: LegVariableName 腿变量名
        /// value1: TradeVolume 成交量
        /// value2: TotalVolume 总量
        /// value3: SummaryPrice 概要价格
        /// 
        /// 缺少某个元素值时，则不拼装该元素
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var legVariableName = values[0] as string;
            var tradeVolume = values[1] as int?;
            var totalVolume = values[2] as int?;
            var summaryPrice = values[3] as double?;

            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(legVariableName))
            {
                sb.Append(legVariableName).Append(": ");
            }

            if (tradeVolume != null)
            {
                sb.Append(tradeVolume.Value);
            }

            if (tradeVolume != null && totalVolume != null)
            {
                // tradeVolume 和 totalVolume 都存在时，显示 "/totalVolume"
                sb.Append("/").Append(totalVolume.Value);
            }

            if (summaryPrice != null)
            {
                if (sb.Length > 0) sb.Append(" ");
                sb.Append($"({summaryPrice})");
            }

            return sb.ToString();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
