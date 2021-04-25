using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using XueQiaoFoundation.Shared.Helper;

namespace XueQiaoFoundation.BusinessResources.Converters
{
    public class ComposeLegIndex2DisplayNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int index = 0;
            try
            {
                index = System.Convert.ToInt32(value);
            }
            catch (Exception)
            {
                return null;
            }
            return CommonConvert.IndexUnder23ToLetter(index, true, null, Properties.Resources.ResourceManager.GetString("Leg", culture));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
