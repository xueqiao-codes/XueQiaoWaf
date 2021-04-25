using ContainerShell.Applications.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;

namespace ContainerShell.Applications.Domain
{
    public class EditWorkspace : ValidatableModel
    {
        private string workspaceName;
        [Required(ErrorMessageResourceName = "WorkspaceNameRequired", ErrorMessageResourceType = typeof(Resources))]
        public string WorkspaceName
        {
            get { return workspaceName; }
            set { SetPropertyAndValidate(ref workspaceName, value); }
        }
    }
}
