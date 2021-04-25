using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using XueQiaoFoundation.Shared.Model;

namespace XueQiaoWaf.LoginUserManage.Modules.Applications.Domains
{
    public class EditPwd : ValidationModel, IValidatableObject
    {
        /// <summary>
        /// 密码规则正则。双引号需要使用 "" 表示
        /// 键盘上所有的特殊符号：<see cref="https://www.douban.com/group/topic/12410327/"/>
        /// 
        /// Combine multiple conditions in a regex
        /// <see cref="https://www.reddit.com/r/learnprogramming/comments/1rb507/can_you_have_multiple_conditions_in_a_regex_has/"/>
        /// </summary>
        private static readonly Regex PwdRegex 
            = new Regex(@"^(?=.*[a-zA-Z])(?=.*[\!\?,\.:;""'`\*\+-\=/\\\|_\$@#%&\^~\{\}\[\]\(\)\<\>])(?=.*\d).{6,20}$", 
                RegexOptions.Compiled);
        
        private string pwd;
        public string Pwd
        {
            get { return pwd; }
            set { SetProperty(ref pwd, value); }
        }

        private ValidationResult ValidatePwd()
        {
            var pwd = this.Pwd??"";

            if (!PwdRegex.IsMatch(pwd))
                return new ValidationResult(Properties.Resources.EditPwdRule);

            return null;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();
            var pwdResult = ValidatePwd();
            if (pwdResult != null) validationResults.Add(pwdResult);
            return validationResults.ToArray();
        }
    }
}
