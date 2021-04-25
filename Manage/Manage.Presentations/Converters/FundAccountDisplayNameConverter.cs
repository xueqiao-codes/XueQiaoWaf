using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Manage.Presentations.Converters
{
    public class FundAccountDisplayNameConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is string loginName 
                && values[1] is string aliasName)
            {
                var sb = new StringBuilder();
                if (!string.IsNullOrEmpty(loginName)) sb.Append(loginName);
                if (!string.IsNullOrEmpty(aliasName))
                {
                    var wrapWithSymbol = sb.Length > 0;
                    if (wrapWithSymbol) sb.Append("(");
                    sb.Append(aliasName);
                    if (wrapWithSymbol) sb.Append(")");
                }
                return sb.ToString();
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
