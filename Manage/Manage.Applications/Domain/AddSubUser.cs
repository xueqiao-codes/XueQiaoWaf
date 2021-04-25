using Manage.Applications.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Applications.Domain
{
    public class AddSubUser : EditSubUser
    {
        private string loginPwd;
        [Required(ErrorMessageResourceName = "LoginPwdRequired", ErrorMessageResourceType = typeof(Resources))]
        public new string LoginPwd
        {
            get { return loginPwd; }
            set { SetPropertyAndValidate(ref loginPwd, value); }
        }

        private string repeatConfirmLoginPwd;
        [Required(ErrorMessageResourceName = "RepeatConfirmLoginPwdRequired", ErrorMessageResourceType = typeof(Resources))]
        public new string RepeatConfirmLoginPwd
        {
            get { return repeatConfirmLoginPwd; }
            set { SetPropertyAndValidate(ref repeatConfirmLoginPwd, value); }
        }
    }
}
