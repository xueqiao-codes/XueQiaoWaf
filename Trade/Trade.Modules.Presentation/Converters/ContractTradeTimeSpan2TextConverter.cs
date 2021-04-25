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
    public class ContractTradeTimeSpan2TextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ContractTradeTimeSpan timeSpan)
            {
                var sb = new StringBuilder();
                sb.Append(timeSpan.BeginDateTime.Date.ToString("MM.dd"))
                  .Append(" ")
                  .Append(timeSpan.BeginDateTime.ToString("HH:mm"))
                  .Append("-")
                  .Append(timeSpan.EndDateTime.Date.ToString("MM.dd"))
                  .Append(" ")
                  .Append(timeSpan.EndDateTime.ToString("HH:mm"))
                  .Append(timeSpan.State == xueqiao.contract.TimeSpanState.T_PLUS_ONE_OPEN ? "(T+1)" : "");
                return sb.ToString();
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
