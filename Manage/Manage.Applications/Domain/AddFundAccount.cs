using Manage.Applications.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Applications.Domain
{
    public class AddFundAccount : EditFundAccount
    {
        private string loginPasswd;
        [Required(ErrorMessageResourceName = "FundAccountLoginPasswordRequired", ErrorMessageResourceType = typeof(Resources))]
        public string LoginPasswd
        {
            get { return loginPasswd; }
            set { SetPropertyAndValidate(ref loginPasswd, value); }
        }
    }
}
