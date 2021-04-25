using Manage.Applications.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Manage.Presentations.Converters
{
    public class PAItemsPreviewType2NameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PAItemsPreviewType previewType)
            {
                return Properties.Resources.ResourceManager
                    .GetString($"{typeof(PAItemsPreviewType).Name}_{previewType.ToString()}");
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
