using Manage.Applications.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;

namespace Manage.Applications.Domain
{
    public class EditSubUser : ValidatableModel
    {
        private string loginName;
        [Required(ErrorMessageResourceName = "LoginNameRequired", ErrorMessageResourceType = typeof(Resources))]
        public string LoginName
        {
            get { return loginName; }
            set { SetPropertyAndValidate(ref loginName, value); }
        }
        
        private string nickName;
        public string NickName
        {
            get { return nickName; }
            set { SetPropertyAndValidate(ref nickName, value); }
        }

        private string loginPwd;
        public string LoginPwd
        {
            get { return loginPwd; }
            set { SetPropertyAndValidate(ref loginPwd, value); }
        }

        private string repeatConfirmLoginPwd;
        public string RepeatConfirmLoginPwd
        {
            get { return repeatConfirmLoginPwd; }
            set { SetPropertyAndValidate(ref repeatConfirmLoginPwd, value); }
        }
    }
}
