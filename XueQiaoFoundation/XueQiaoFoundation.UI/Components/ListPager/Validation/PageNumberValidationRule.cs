using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace XueQiaoFoundation.UI.Components.ListPager.Validation
{
    public class PageNumberValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int page = 0;
            try
            {
                if (((string)value).Length > 0)
                {
                    page = int.Parse((string)value, NumberStyles.Any);
                }
            }
            catch
            {
                return new ValidationResult(false, Properties.Resources.ResourceManager.GetString("FormatNotCorrect", cultureInfo));
            }

            if (page < 0)
            {
                return new ValidationResult(false, Properties.Resources.ResourceManager.GetString("NotLittleThan0", cultureInfo));
            }
            else
            {
                return new ValidationResult(true, null);
            }
        }
    }
}
