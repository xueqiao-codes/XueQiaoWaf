using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using XueQiaoFoundation.Shared.Model;

namespace XueQiaoWaf.LoginUserManage.Modules.Applications.Domains
{
    public class UpdatePwd : ValidationModel, IValidatableObject
    {
        public UpdatePwd()
        {
            NewPwdEdit = new EditPwd();
        }

        private string oldPwd;
        [Required(ErrorMessageResourceName = "UpdateOldPwdRequired", ErrorMessageResourceType = typeof(Properties.Resources))]
        public string OldPwd
        {
            get { return oldPwd; }
            set { SetProperty(ref oldPwd, value); }
        }
        
        public EditPwd NewPwdEdit { get; private set; }

        private string repeatNewPwd;
        public string RepeatNewPwd
        {
            get { return repeatNewPwd; }
            set { SetProperty(ref repeatNewPwd, value); }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            var errorMsg = NewPwdEdit.Validate();
            if (!string.IsNullOrEmpty(errorMsg))
            {
                validationResults.Add(new ValidationResult(errorMsg, new[] { nameof(NewPwdEdit) }));
                return validationResults;
            }
            
            if (NewPwdEdit.Pwd != this.RepeatNewPwd)
            {
                validationResults.Add(new ValidationResult(Properties.Resources.UpdateNewAndRepeatPwdNotSame));
                return validationResults;
            }
            return validationResults;
        }
    }
}
