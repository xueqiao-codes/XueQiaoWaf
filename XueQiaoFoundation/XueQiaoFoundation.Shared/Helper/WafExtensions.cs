using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;

namespace XueQiaoFoundation.Shared.Helper
{
    public static class WafExtensions
    {
        public static string JoinErrors(this ValidatableModel validatableModel, string separatorString = "\n")
        {
            var errors = validatableModel.GetErrors();
            if (errors != null)
            {
                return string.Join(separatorString ?? ";", errors.Where(e => !string.IsNullOrWhiteSpace(e.ErrorMessage)).Select(e => e.ErrorMessage));
            }
            return null;
        }
    }
}
